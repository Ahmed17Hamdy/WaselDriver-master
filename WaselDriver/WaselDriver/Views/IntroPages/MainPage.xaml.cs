using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TK.CustomMap;
using TK.CustomMap.Overlays;
using WaselDriver.Helper;
using WaselDriver.Models;
using WaselDriver.Views.OrderPage;
using WaselDriver.Views.PopUps;
using WaselDriver.Views.PushNotificationPages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WaselDriver
{
    public partial class MainPage : ContentPage
    {
        string Lat, Lan;
        public MainPage()
        {
            InitializeComponent();
            GetLocation();
            CheckUserStatus();
        }
        private async void GetLocation()
        {
            var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (locationStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                locationStatus = results[Permission.Location];
            }
            if (locationStatus == PermissionStatus.Granted)
            {
                try
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Lowest);
                    var location = await Geolocation.GetLocationAsync(request);
                    MainMap.MapRegion = (MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(0.2)));
                    await StartListening(location.Latitude, location.Longitude);
                }
                catch (FeatureNotEnabledException)
                {
                    await PopupNavigation.Instance.PushAsync(new RequestPopUp(AppResources.LocationEnabled));
                }
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new RequestPopUp(AppResources.PermissionsDenied));
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();
            }
            OneSignal.Current.StartInit("1126a3d0-1d80-42ee-94db-d0449ac0a62c")
              .InFocusDisplaying(OSInFocusDisplayOption.None)
              .HandleNotificationReceived(OnNotificationRecevied)
              .HandleNotificationOpened(OnNotificationOpened)
              .EndInit();
        }

        async Task StartListening(double x, double y)
        {
            try
            {
                var addresses = await Geocoding.GetPlacemarksAsync(x, y);
                var placemark = addresses.FirstOrDefault();
                if (placemark != null)
                {
                    if (addresses.FirstOrDefault().Thoroughfare != null)
                    {
                        UserStatuslbl.Text = placemark.Thoroughfare + " , " + placemark.AdminArea + " , " + placemark.CountryName;
                    }
                    else
                    {
                        UserStatuslbl.Text = AppResources.LocationNotFound;
                    }
                }
                else
                {
                    UserStatuslbl.Text = AppResources.LocationNotFound;
                }
            }
            catch (FeatureNotEnabledException)
            {
                await PopupNavigation.Instance.PushAsync(new RequestPopUp(AppResources.LocationEnabled));
            }
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
                App.Current.MainPage = new NotificationSummaryPage(labelText, Lat, Lan);
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
                App.Current.MainPage = new NotificationSummaryPage(labelText,Lat,Lan);
            }
        }
        private void CheckUserStatus()
        {
            if (Settings.LastUserStatus == "0")
            {
                UserStatuslbl.IsVisible = true;
            }
            else
            {
                UserStatuslbl.IsVisible = false;
            }
        }
        private async  void  UserLocationChanged(object sender, TK.CustomMap.TKGenericEventArgs<TK.CustomMap.Position> e)
        {
            var x = e.Value.Latitude;
            var y = e.Value.Longitude;
            if (Lat != x.ToString() || Lan != y.ToString())
            {
                if (Settings.LastUserStatus != "0")
                {
                    Lat = x.ToString();
                    Lan = y.ToString();
                    try
                    {
                        var CurrentLocation = new Position(x, y);
                        if (CurrentLocation != null)
                        {
                            Dictionary<string, string> values = new Dictionary<string, string>();
                            values.Add("driver_id",Settings.LastUsedDriverID.ToString());
                            values.Add("lat", Lat);
                            values.Add("lng", Lan);
                            string content = JsonConvert.SerializeObject(values);
                            var httpClient = new HttpClient();
                            try
                            {
                                var response = await httpClient.PostAsync("http://wassel.alsalil.net/api/updatelocal", 
                                    new StringContent(content, Encoding.UTF8, "text/json"));
                                var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                                var json = JsonConvert.DeserializeObject<Response<string, string>>(serverResponse);
                              
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
}
