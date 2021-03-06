﻿using System;
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
using Xamarin.Essentials;
using WaselDriver.Views.IntroPages;
using Com.OneSignal;
using Com.OneSignal.Abstractions;

namespace WaselDriver.Views.OrderPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrdersPage : ContentPage
	{
       List<TKRoute> routes = new List<TKRoute>();
        List<TKCustomMapPin> Pins = new List<TKCustomMapPin>();

        DelivaryObject delivary;
        string Lat, Lan;
        TirhalOrder TOrder;
        public OrdersPage (TirhalOrder Order,string Lt,string Ln)
		{
			InitializeComponent ();
            TOrder = Order;
            Lat = Lt;Lan = Ln;
            GmsDirection.Init("AIzaSyB7rB6s8fc317zCPz8HS_yqwi7HjMsAqks");
           // SetMyLocation();
            OrderMap.RouteCalculationFinished += OrderMap_RouteCalculationFinished;
            OrderMap.RouteCalculationFailed += OrderMap_RouteCalculationFailed;
            OneSignal.Current.StartInit("1126a3d0-1d80-42ee-94db-d0449ac0a62c")
             .InFocusDisplaying(OSInFocusDisplayOption.None)
             .HandleNotificationReceived(OnNotificationRecevied)    
             .HandleNotificationOpened(OnNotificationOpened)
             .EndInit();
        }
        private void OnNotificationOpened(OSNotificationOpenedResult result)
        {
            if (result.notification?.payload?.additionalData == null)
            {
                return;
            }

            if (result.notification.payload.additionalData.ContainsKey("body"))
            {
                var labelText = result.notification.payload.additionalData["body"].ToString();
                var Req = JsonConvert.DeserializeObject<DelivaryObject>(labelText);
                if (Req.done == 3)
                {
                    DisplayAlert(AppResources.Alert, AppResources.CanceledOrder, AppResources.Ok);
                    Device.BeginInvokeOnMainThread(() => {
                        Navigation.PushModalAsync(new MainTabbed());
                    });
                }
            }

        }
        private void OnNotificationRecevied(OSNotification notification)
        {
            if (notification.payload?.additionalData == null)
            {
                return;
            }

            if (notification.payload.additionalData.ContainsKey("body"))
            {
                var labelText = notification.payload.additionalData["body"].ToString();
              
                var Req = JsonConvert.DeserializeObject<DelivaryObject>(labelText);
                if (Req.done == 3)
                {
                    App.Current.MainPage = new MainTabbed();
                    DisplayAlert(AppResources.Alert, AppResources.CanceledOrder, AppResources.Ok);
                    //Device.BeginInvokeOnMainThread(() => {
                    //    Navigation.PushModalAsync(new MainTabbed());
                    //});
                }
               
            }
        }
        private async void OrderMap_RouteCalculationFailed(object sender, 
            TKGenericEventArgs<TK.CustomMap.Models.TKRouteCalculationError> e)
        {
            await DisplayAlert(AppResources.Error, AppResources.RouteNotFound, AppResources.Ok);
           
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);
                OrderMap.MapRegion = MapSpan.FromCenterAndRadius(
                        new Position(location.Longitude, location.Longitude), Distance.FromMiles(1));
           
        }
        private async void SetMyLocation()
        {
            Pins.Clear();
            routes.Clear();
            var route = new TKRoute();
            route.TravelMode = TKRouteTravelMode.Driving;
            var myposition = new Position(Convert.ToDouble(Lat), Convert.ToDouble(Lan));         
            var toposition = new Position(Convert.ToDouble(TOrder.latfrom), Convert.ToDouble(TOrder.lngfrom));
            route.Source = myposition;
            route.Destination = toposition;
            route.Color = Color.OrangeRed;           
            route.LineWidth = 7;
            
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
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);
            var x = location.Latitude;
            var y = location.Longitude;
            if (Lat != x.ToString() || Lan != y.ToString())
            {
                    Lat = x.ToString();
                    Lan = y.ToString();                        
                    try
                    {
                        var CurrentLocation = new Position(Convert.ToDouble(Lat),Convert.ToDouble(Lan));
                        if (CurrentLocation != null)
                        {
                            Dictionary<string, string> values = new Dictionary<string, string>();
                            values.Add("user_id", Settings.LastUsedID.ToString());
                            values.Add("driver_id", Settings.LastUsedDriverID.ToString());
                            values.Add("lat", Lat);
                            values.Add("lng", Lan);
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
        private async void Button_Clicked(object sender, EventArgs e)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("tirhal_order_id", TOrder.id.ToString());
            values.Add("driver_id", TOrder.driver_id.ToString());
            string content = JsonConvert.SerializeObject(values);
            var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("http://wassel.alsalil.net/api/drivercanceltirhalorder",
                    new StringContent(content, Encoding.UTF8, "text/json"));
                var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                var json = JsonConvert.DeserializeObject<Response<TirhalOrder, string>>(serverResponse);
                if (json.success == false)
                {
                    Activ.IsRunning = false;
                    await DisplayAlert(AppResources.Error, json.message, AppResources.Ok);
                }
                else
                {
                    Activ.IsRunning = false;
                    App.Current.MainPage = new MainTabbed();
                    await DisplayAlert(AppResources.OrderSuccess, json.message, AppResources.Ok);
                }
            }
            catch (Exception)
            {
                Activ.IsRunning = false;
                await DisplayAlert(AppResources.ErrorMessage, AppResources.ErrorMessage, AppResources.Ok);
            }
        }
        private async void  FinishedOrder_Clicked(object sender, EventArgs e)
        {
            TirhalOrder order = new TirhalOrder
            {
                user_id = Settings.LastUsedID.ToString(),
                id = TOrder.id,
                driver_id = Settings.LastUsedDriverID.ToString()
            };
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("user_id", order.user_id.ToString());
            values.Add("tirhal_order_id", order.id.ToString());
            values.Add("driver_id", order.driver_id.ToString());
            string content = JsonConvert.SerializeObject(values);
            var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsync("http://wassel.alsalil.net/api/completetirhalorder",
                    new StringContent(content, Encoding.UTF8, "text/json"));
                var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                var json = JsonConvert.DeserializeObject<Response<TirhalOrder, string>>(serverResponse);
                if (json.success == false)
                {
                    Activ.IsRunning = false;
                    await DisplayAlert(AppResources.Error, json.message, AppResources.Ok);
                }
                else
                {
                    Activ.IsRunning = false;
                    //Device.BeginInvokeOnMainThread(() => {
                    //    Navigation.PushModalAsync(new MainTabbed());
                    //});
                    App.Current.MainPage = new MainTabbed();
                    await DisplayAlert(AppResources.OrderSuccess, json.message, AppResources.Ok);
                }
            }
            catch (Exception)
            {
                Activ.IsRunning = false;
                await DisplayAlert(AppResources.ErrorMessage, AppResources.ErrorMessage, AppResources.Ok);
            }
        }
    }
    }
