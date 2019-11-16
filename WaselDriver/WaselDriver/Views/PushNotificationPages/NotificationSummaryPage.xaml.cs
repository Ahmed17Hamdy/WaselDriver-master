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
        DelivaryObject PosFrom;
        string Lat, Lan;
        public MapSpan Bounds { get; set; }

        public NotificationSummaryPage (string ss,string Lt,string Ln)
		{
			InitializeComponent ();
            Lat = Lt; Lan = Ln;
            _=ChechNotification(ss);
            //GmsDirection.Init("AIzaSyB7rB6s8fc317zCPz8HS_yqwi7HjMsAqks");
            SetMyLocation();
            OrderMap.RouteCalculationFinished += OrderMap_RouteCalculationFinished;
            OrderMap.RouteCalculationFailed += OrderMap_RouteCalculationFailed;
        }
        public NotificationSummaryPage(string ss)
        {
            InitializeComponent();
            _=ChechNotification(ss);
            //GmsDirection.Init("AIzaSyB7rB6s8fc317zCPz8HS_yqwi7HjMsAqks");
            SetMyLocation();
            OrderMap.RouteCalculationFinished += OrderMap_RouteCalculationFinished;
            OrderMap.RouteCalculationFailed += OrderMap_RouteCalculationFailed;
        }
        private async void OrderMap_RouteCalculationFailed(object sender,TKGenericEventArgs<TK.CustomMap.Models.TKRouteCalculationError> e)
        {
            await DisplayAlert(AppResources.Error, AppResources.RouteNotFound, AppResources.Ok);
            var request = new GeolocationRequest(GeolocationAccuracy.High);
            var location = await Geolocation.GetLocationAsync(request);
            OrderMap.MapRegion = MapSpan.FromCenterAndRadius( new Position(location.Longitude, location.Longitude), Distance.FromMiles(1));
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
            var myposition = new Position(Convert.ToDouble(Lat), Convert.ToDouble(Lan));
            var toposition = new Position(Convert.ToDouble(PosFrom.latfrom), Convert.ToDouble(PosFrom.lngfrom));
            route.Source = myposition;
            route.Destination = toposition;
            route.Color = Color.OrangeRed;
            route.LineWidth = 7;
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
        int UserID;
        private async Task ChechNotification(string data)
        {
            if(data != "Has been approved" && data != "Not approved")
            {
                var Req = JsonConvert.DeserializeObject<DelivaryObject>(data);
                PosFrom = Req;
                userNamelbl.Text = Req.driver_id;
                UserID = int.Parse(Req.user_id);
                Settings.LastUsedID = UserID;
                AddressFromlbl.Text=await GetAddress(Req.latfrom,Req.lngfrom);
                AddressTolbl.Text =await GetAddress(Req.latto, Req.lngto);
            }
        }
        private async Task<string> GetAddress(string latto, string lngto)
        {
            var addresses = await Geocoding.GetPlacemarksAsync(Convert.ToDouble(latto), Convert.ToDouble(lngto));
            var placemark = addresses?.FirstOrDefault();
            if (placemark != null)
            {
                if (addresses.FirstOrDefault().Thoroughfare != null)
                {
                    return placemark.Thoroughfare.ToString() + " , " + placemark.AdminArea + " , " + placemark.CountryName;
                }
                else
                {
                    return AppResources.LocationNotFound;
                }
            }
            else
            {
                return AppResources.LocationNotFound;
            }
        }
        private async void Acceptbtn_Clicked(object sender, EventArgs e)
        {
            //Settings.Lastdone=1;
            CheckedTirhalOrder order = new CheckedTirhalOrder
            {
                latfrom = PosFrom.latfrom,
                lngfrom = PosFrom.lngfrom,
                user_id = UserID.ToString(),
                driver_id = Settings.LastUsedDriverID.ToString(),
                car_model_id = PosFrom.car_model_id,
                latto = PosFrom.latto,
                lngto = PosFrom.lngto,
                done = 1,
                order_id = PosFrom.id.ToString()
            };
            string content = JsonConvert.SerializeObject(order);
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
                    await PopupNavigation.Instance.PushAsync(new SuccessPage(json.message));
                }
                else
                {
                    Active.IsRunning = false;
                    await PopupNavigation.Instance.PushAsync(new SuccessPage(json.message));
                    Device.BeginInvokeOnMainThread(() => {
                        Navigation.PushModalAsync(new OrdersPage(json.data,Lat,Lan));
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
            //Settings.Lastdone =0;
            CheckedTirhalOrder order = new CheckedTirhalOrder
            {
                latfrom = PosFrom.latfrom,
                lngfrom = PosFrom.lngfrom,
                user_id = UserID.ToString(),
                driver_id = Settings.LastUsedDriverID.ToString(),
                car_model_id = PosFrom.car_model_id,
                latto = PosFrom.latto,
                lngto = PosFrom.lngto,
                done = 0,
                order_id = PosFrom.id.ToString()
            };

            string content = JsonConvert.SerializeObject(order);
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
                    await DisplayAlert(AppResources.Error, json.message, AppResources.Ok);
                }
                else
                {
                    Active.IsRunning = false;
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