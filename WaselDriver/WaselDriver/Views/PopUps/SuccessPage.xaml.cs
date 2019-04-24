using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaselDriver.Views.PopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessPage : PopupPage
    {
   
        public SuccessPage(string message)
        {
            InitializeComponent();
            Messagelbl.Text = message;
        }
    }
}