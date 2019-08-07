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
    public class MainViewModel : BaseViewModel
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
        public bool ErrorPresent { get; set; }
        public bool ValidPresent { get; set; }
        private async void CountryList()
        {
            ErrorPresent = false;
            ValidPresent = !ErrorPresent;
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    var Resback = await MainServices.GetAllCountries();
                    Countries = Resback;
                    foreach (var item in Countries)
                    {
                        item.Title = (Settings.LastUserGravity == "Arabic") ? item.name : item.enname;
                        item.Image = (item.enname == "Ethiopia") ? "ethiopia.png" : "sudan.png";
                    }
                    CountryName = Countries.Select(o => o.Title).ToList();
                }
                catch (System.Exception)
                {
                    ErrorPresent = true;
                    ValidPresent = !ErrorPresent;
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Message", AppResources.ErrorMessage, "Ok");
            }

        }
        
    }

}
