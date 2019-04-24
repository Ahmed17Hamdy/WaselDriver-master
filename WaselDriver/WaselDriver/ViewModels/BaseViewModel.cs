using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;

namespace WaselDriver.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public bool IsNotConnected { get; set; }
        public bool IsConnected { get; set; }
        public BaseViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        ~BaseViewModel()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsNotConnected = e.NetworkAccess != NetworkAccess.Internet;
            IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;

            /*
           DependencyService.Get<IToastNotificator>().Notify(new NotificationOptions()
            {
                Description = "Oops, looks like you don't have internet connection :(",
                Title = "Connection lost"
            });
            if (e.NetworkAccess != NetworkAccess.Internet)
                UserDialogs.Instance.Toast("Oops, looks like you don't have internet connection :(");
            else
                UserDialogs.Instance.Toast("Your internet connection is back :)");*/
        }
    }
}
