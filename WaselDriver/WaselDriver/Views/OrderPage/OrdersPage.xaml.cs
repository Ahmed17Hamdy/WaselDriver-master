using System;
using System.Collections.Generic;
using System.Text;
using TK.CustomMap;
using TK.CustomMap.Api.Google;
using TK.CustomMap.Overlays;
using WaselDriver.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WaselDriver.Helper;
using Newtonsoft.Json;
using System.Net.Http;
using WaselDriver.Models;

namespace WaselDriver.Views.OrderPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrdersPage : ContentPage
	{
       List<TKRoute> routes = new List<TKRoute>();
        List<TKCustomMapPin> Pins = new List<TKCustomMapPin>();
      
            public MapSpan Bounds { get; set; }
        public OrdersPage ()
		{
			InitializeComponent ();
            Settings.LastNotify = null;
            GmsDirection.Init("AIzaSyB7rB6s8fc317zCPz8HS_yqwi7HjMsAqks");
            SetMyLocation();
            OrderMap.RouteCalculationFinished += OrderMap_RouteCalculationFinished;
            OrderMap.RouteCalculationFailed += OrderMap_RouteCalculationFailed;
            
        }

        private void OrderMap_RouteCalculationFailed(object sender, TKGenericEventArgs<TK.CustomMap.Models.TKRouteCalculationError> e)
        {
           
        }

        private async void SetMyLocation()
        {
            Pins.Clear();           
            routes.Clear();
            var route = new TKRoute();
            route.TravelMode = TKRouteTravelMode.Driving;
            var myposition = new Position(Convert.ToDouble(Settings.LastLat), Convert.ToDouble(Settings.LastLng));         
            var toposition = new Position(Convert.ToDouble(Settings.Latto), Convert.ToDouble(Settings.Lngto));
            route.Source = myposition;
            route.Destination = toposition;
            route.Color = Color.OrangeRed;           
            route.LineWidth = 4;
            
            Pins.Add(new RoutePin
            {
                Route = route,
                Image= "deliverytruck.png",
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

        private void OrderMap_RouteCalculationFinished(object sender, TKGenericEventArgs<TKRoute> e)
        {
            OrderMap.MapRegion = e.Value.Bounds;      
          
        }

        private async void OrderMap_UserLocationChanged(object sender, TKGenericEventArgs<Position> e)
        {
            var x = e.Value.Latitude;
            var y = e.Value.Longitude;
            if (Settings.LastLat != x.ToString() || Settings.LastLng != y.ToString())
            {
                    Settings.LastLat = x.ToString();
                    Settings.LastLng = y.ToString();                        
                    try
                    {
                        var CurrentLocation = new Position(Convert.ToDouble(Settings.LastLat),
                            Convert.ToDouble(Settings.LastLng));
                        if (CurrentLocation != null)
                        {
                            Dictionary<string, string> values = new Dictionary<string, string>();
                            values.Add("user_id", Settings.LastUsedID.ToString());
                            values.Add("driver_id", Settings.LastUsedDriverID.ToString());
                            values.Add("lat", Settings.LastLat);
                            values.Add("lng", Settings.LastLng);
                            string content = JsonConvert.SerializeObject(values);
                            var httpClient = new HttpClient();
                            try
                            {
                                var response = await httpClient.PostAsync("http://wassel.alsalil.net/api/updatelocalandnoti",
                                    new StringContent(content, Encoding.UTF8, "text/json"));
                                var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                                var json = JsonConvert.DeserializeObject<Response<string, string>>(serverResponse);
                                    SetMyLocation();
                        }
                            catch (Exception)
                            {                                
                                await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);
                            }
                        }
                    }
                    catch
                    {
                        return;
                    }
                }
               
            }
        }
    }
