using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaselDriver.Views.PopUps
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequestPopUp : PopupPage
    {
        public RequestPopUp(string message)
        {
        }

        public RequestPopUp(bool Check, string ss)
        {
            InitializeComponent();
            if (Check == false)
            {
                PopFrame.BackgroundColor = Color.Red;
                LblTitle.Text = "خطأ";
                LblTitle.Text = ss;
            }
            else if (Check == true)
            {
                PopFrame.BackgroundColor = Color.Green;
                LblTitle.Text = ss;
            }
        }

        public RequestPopUp(bool check, int PageType)
        {
            InitializeComponent();
            //var Response = JsonConvert.DeserializeObject<User>(rq.message);
            FlowDirection = (WaselDriver.Helper.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            if (PageType == 0)
            {
                if (check == false)
                {
                    PopFrame.BackgroundColor = Color.Red;
                    LblTitle.Text = "خطأ";
                    LblTitle.Text = "من فضلك تحقق من البريد الإلكترونى و كلمة السر";
                }
                else if (check == true)
                {
                    PopFrame.BackgroundColor = Color.Green;
                    LblTitle.Text = "لقد تم التسجيل بنجاح";
                }
            }
            else if (PageType == 1)
            {
                if (check == false)
                {
                    PopFrame.BackgroundColor = Color.Red;
                    LblTitle.Text = "خطأ";
                    LblTitle.Text = "لقد تم إدخال البريد الإلكترونى و رقم الهاتف من قبل";
                }
                else if (check == true)
                {
                    PopFrame.BackgroundColor = Color.Green;
                    LblTitle.Text = "لقد تم التسجيل بنجاح";
                }
            }

            else if (PageType == 2)
            {
                if (check == true)
                {
                    PopFrame.BackgroundColor = Color.Green;
                    LblTitle.Text = "لقد تم التسجيل بنجاح";
                }
            }

            else if (PageType == 3)
            {
                if (check == true)
                {
                    PopFrame.BackgroundColor = Color.Green;
                    LblTitle.Text = "لقد تم تسجيل طلبك بنجاح";
                }
                if (check == false)
                {
                    PopFrame.BackgroundColor = Color.Green;
                    LblTitle.Text = "نأسف لقد تم حجز هذا المعاد من قبل";
                }
            }
            {

            }

        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}