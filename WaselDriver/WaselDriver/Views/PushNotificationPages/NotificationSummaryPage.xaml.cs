using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WaselDriver.Helper;
using System.Net.Http;
using WaselDriver.Views.OrderPage;
using TK.CustomMap.Overlays;
using TK.CustomMap;
using WaselDriver.Views.PopUps;
using Rg.Plugins.Popup.Services;
using WaselDriver.Views.IntroPages;
using TK.CustomMap.Api.Google;
using WaselDriver.ViewModels;

namespace WaselDriver.Views.PushNotificationPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationSummaryPage : ContentPage
	{
        List<TKRoute> routes = new List<TKRoute>();
        List<TKCustomMapPin> Pins = new List<TKCustomMapPin>();

        public MapSpan Bounds { get; set; }

        public NotificationSummaryPage ()
		{
			InitializeComponent ();
            ChechNotification();
            GmsDirection.Init("AIzaSyB7rB6s8fc317zCPz8HS_yqwi7HjMsAqks");
            SetMyLocation();
            OrderMap.RouteCalculationFinished += OrderMap_RouteCalculationFinished;
            OrderMap.RouteCalculationFailed += OrderMap_RouteCalculationFailed;
        }
        private async void OrderMap_RouteCalculationFailed(object sender, TKGenericEventArgs<TK.CustomMap.Models.TKRouteCalculationError> e)
        {
            await DisplayAlert(AppResources.Error, AppResources.RouteNotFound, AppResources.Ok);

            var request = new GeolocationRequest(GeolocationAccuracy.High);
            var location = await Geolocation.GetLocationAsync(request);
            OrderMap.MapRegion = MapSpan.FromCenterAndRadius(
                    new Position(location.Longitude, location.Longitude), Distance.FromMiles(1));

        }
        private void OrderMap_RouteCalculationFinished(object sender, TKGenericEventArgs<TKRoute> e)
        {
            OrderMap.MapRegion = e.Value.Bounds;

        }
        private async void SetMyLocation()
        {
            Pins.Clear();
            routes.Clear();
            var route = new TKRoute();
            route.TravelMode = TKRouteTravelMode.Driving;
            var myposition = new Position(Convert.ToDouble(Settings.LastLat), Convert.ToDouble(Settings.LastLng));
            var toposition = new Position(Convert.ToDouble(Settings.Latfrom), Convert.ToDouble(Settings.Lngfrom));
            route.Source = myposition;
            route.Destination = toposition;
            route.Color = Color.OrangeRed;
            route.LineWidth = 4;

            Pins.Add(new RoutePin
            {
                Route = route,
                Image = "deliverytruck.png",
                IsSource = true,
                IsDraggable = true,
                Position = myposition,
                Title = "From",
                ShowCallout = true,

            });
            Pins.Add(new RoutePin
            {
                Route = route,
                IsSource = false,
                IsDraggable = true,
                Position = toposition,
                Title = "To",
                ShowCallout = true,
                DefaultPinColor = Color.Red
            });
            routes.Add(route);
            OrderMap.Routes = routes;
            OrderMap.Pins = Pins;

        }

        private void ChechNotification()
        {
            if(Settings.LastNotify!= "Has been approved" && Settings.LastNotify!= "Not approved")
            {
                var Req = JsonConvert.DeserializeObject<DelivaryObject>(Settings.LastNotify);
                userNamelbl.Text = Req.driver_id;
                Settings.LastOrderid = Req.id;
                Settings.LastUsedID = int.Parse(Req.user_id);
                Settings.Latto = Req.latto;
                Settings.Lngto = Req.lngto;
                Settings.Latfrom = Req.latfrom;
                Settings.Lngfrom = Req.lngfrom;
                GetAddressFrom(Settings.Latfrom, Settings.Lngfrom);
                GetAddressTo(Settings.Latto, Settings.Lngto);
               
            }
            //else
            //{
            //    Settings.LastNotify = null;
            //    Application.Current.MainPage = new NavigationPage( new MainTabbed());
            //}
            
        }

        private async void GetAddressFrom(string latfrom , string lngfrom)
        {
            var addresses = await Geocoding.GetPlacemarksAsync(Convert.ToDouble(latfrom), Convert.ToDouble(lngfrom));
            var placemark = addresses?.FirstOrDefault();
            if (placemark != null)
            {
                if (addresses.FirstOrDefault().Thoroughfare != null)
                {
                    var loc = addresses.FirstOrDefault();
                    AddressFromlbl.Text = loc.Thoroughfare.ToString() + " , " + loc.AdminArea + " , " + loc.CountryName;
                    Settings.Latfrom = latfrom;
                    Settings.Lngfrom = lngfrom;
                    Settings.PlaceFrom = AddressFromlbl.Text;
                }
                else
                {
                    AddressFromlbl.Text = AppResources.LocationNotFound;
                }
            }
            else
            {
                AddressFromlbl.Text = AppResources.LocationNotFound;
            }
        }
        private async void GetAddressTo(string latto, string lngto)
        {
            var addresses = await Geocoding.GetPlacemarksAsync(Convert.ToDouble(latto), Convert.ToDouble(lngto));
            var placemark = addresses?.FirstOrDefault();
            if (placemark != null)
            {
                if (addresses.FirstOrDefault().Thoroughfare != null)
                {
                    var loc = addresses.FirstOrDefault();
                    AddressTolbl.Text = loc.Thoroughfare.ToString() + " , " + loc.AdminArea + " , " + loc.CountryName;
                    Settings.Latto = latto;
                    Settings.Lngto = lngto;
                    Settings.PlaceFrom = AddressTolbl.Text;
                }
                else
                {
                    AddressTolbl.Text = AppResources.LocationNotFound;
                }
            }
            else
            {
                AddressTolbl.Text = AppResources.LocationNotFound;
            }
        }

        private async void Acceptbtn_Clicked(object sender, EventArgs e)
        {
            Settings.Lastdone=1;
            CheckedTirhalOrder order = new CheckedTirhalOrder
            {
                latfrom = Settings.Latfrom,
                lngfrom = Settings.Lngfrom,
                user_id = Settings.LastUsedID.ToString(),
                driver_id = Settings.LastUsedDriverID.ToString(),
                car_model_id = Settings.LastUsedCarModel.ToString(),
                latto = Settings.Latto,
                lngto = Settings.Lngto,
                done = Settings.Lastdone,
                order_id = Settings.LastOrderid.ToString()
            };
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("user_id", order.user_id);
            values.Add("driver_id", order.driver_id);
            values.Add("latfrom", order.latfrom);
            values.Add("lngfrom", order.lngfrom);
            values.Add("done", order.done.ToString());
            values.Add("latto", order.latto);
            values.Add("lngto", order.lngto);
            values.Add("car_model", order.car_model_id);
            values.Add("created_at", order.created_at);
            values.Add("order_id", order.order_id);
            string content = JsonConvert.SerializeObject(values);
            var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("http://wassel.alsalil.net/api/addResponse", 
                    new StringContent(content, Encoding.UTF8, "text/json"));
                var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                var json = JsonConvert.DeserializeObject<Response<TirhalOrder, string>>(serverResponse);
                if (json.success == false)
                {
                    Active.IsRunning = false;
                    Settings.LastNotify = null;
                    await PopupNavigation.Instance.PushAsync(new SuccessPage(json.message));
                    Device.BeginInvokeOnMainThread(() => {
                        Navigation.PushModalAsync(new MainTabbed());
                    });                    
                }
                else

                {
                    Active.IsRunning = false;
                    Settings.LastNotify = null;
                    // await DisplayAlert(AppResources.OrderSuccess, json.message, AppResources.Ok);
                    //  Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new OrdersPage());
                    await PopupNavigation.Instance.PushAsync(new SuccessPage(json.message));
                    await PopupNavigation.Instance.PushAsync(new SuccessPage(json.message));
                    Device.BeginInvokeOnMainThread(() => {
                        Navigation.PushModalAsync(new OrdersPage());
                    });
                    
                }
            }
            catch (Exception)
            {
                Active.IsRunning = false;
                await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);
            }
        }
       
        private async void Cancelbtn_Clicked(object sender, EventArgs e)
        {
            Settings.Lastdone =0;
            CheckedTirhalOrder order = new CheckedTirhalOrder
            {
                latfrom = Settings.Latfrom,
                lngfrom = Settings.Lngfrom,
                user_id = Settings.LastUsedID.ToString(),
                driver_id = Settings.LastUsedDriverID.ToString(),
                car_model_id = Settings.LastUsedCarModel.ToString(),
                latto = Settings.Latto,
                lngto = Settings.Lngto,
                done = Settings.Lastdone,
                order_id = Settings.LastOrderid.ToString()
            };
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("user_id", order.user_id.ToString());
            values.Add("driver_id", order.driver_id);
            values.Add("latfrom", order.latfrom);
            values.Add("lngfrom", order.lngfrom);
            values.Add("done", order.done.ToString());
            values.Add("latto", order.latto);
            values.Add("lngto", order.lngto);
            values.Add("car_model", order.car_model_id);
            values.Add("created_at", order.created_at);
            values.Add("order_id", order.order_id);
            string content = JsonConvert.SerializeObject(values);
            var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("http://wassel.alsalil.net/api/addResponse",
                    new StringContent(content, Encoding.UTF8, "text/json"));
                var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                var json = JsonConvert.DeserializeObject<Response<TirhalOrder, string>>(serverResponse);
                if (json.success == false)
                {
                    Active.IsRunning = false;
                    Settings.LastNotify = null;
                    await DisplayAlert(AppResources.Error, json.message, AppResources.Ok);
                }
                else
                {
                    Active.IsRunning = false;
                    Settings.LastNotify = null;
                    await DisplayAlert(AppResources.OrderSuccess, json.message, AppResources.Ok);
                    await PopupNavigation.Instance.PushAsync(new SuccessPage(json.message));
                    Device.BeginInvokeOnMainThread(() => {
                        Navigation.PushModalAsync(new MainTabbed());
                    });
                }
            }
            catch (Exception)
            {
                Active.IsRunning = false;
                await DisplayAlert(AppResources.ErrorMessage, AppResources.ErrorMessage, AppResources.Ok);
            }
        }
    }
}