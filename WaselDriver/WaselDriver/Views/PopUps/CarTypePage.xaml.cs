using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WaselDriver.Helper;
using WaselDriver.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace WaselDriver.Views.PopUps
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CarTypePage : ContentPage
	{
		public CarTypePage ()
		{
			InitializeComponent ();
		}

        private  void CarTypeList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as TirhalModel;
            Settings.CarModelID = item.id.ToString();
            Returnbtn.IsEnabled = true;
           
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DelivaryViewModel modl = new DelivaryViewModel();
            this.BindingContext = modl;
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}