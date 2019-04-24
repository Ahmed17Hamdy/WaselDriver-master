using Plugin.Connectivity;
using WaselDriver.Helper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using WaselDriver.Models;
using WaselDriver.Services;
using System.Linq;
using System.Collections.Generic;

namespace WaselDriver.ViewModels
{
  public  class MainViewModel : BaseViewModel
    {
        private static ObservableCollection<Country> _countries;
        public ObservableCollection<Country> Countries
        {
            get
            {
                return _countries;
            }
            set
            {
                _countries = value;
                OnPropertyChanged();
            }
        }
        private static List<string> _countryname;
        public List<string> CountryName
        {
            get
            {
                return _countryname;
            }
            set
            {
                _countryname = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            CountryList();
        }
        private async void CountryList()
        {
            MainServices mainServices = new MainServices();
            if(CrossConnectivity.Current.IsConnected)
            {
               var  Resback = await mainServices.GetAllCountries();
                Countries = Resback;
                foreach (var item in Countries)
                {
                    item.Title = (Settings.LastUserGravity == "Arabic") ? item.name : item.enname;
                    item.Image = (item.enname == "Ethiopia") ? "ethiopia.png" : "sudan.png";
                }
               CountryName= Countries.Select(o => o.Title).ToList();
            }
            else
            {
              await App.Current.MainPage.DisplayAlert("Message", AppResources.ErrorMessage, "Ok");
            }
                     
        }
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
    
}
