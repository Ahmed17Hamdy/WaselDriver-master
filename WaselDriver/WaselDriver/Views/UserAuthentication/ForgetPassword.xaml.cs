using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Models;
using WaselDriver.Services;
using WaselDriver.Views.PopUps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaselDriver.Views.UserAuthentication
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgetPassword : ContentPage
    {
        public ForgetPassword()
        {
            InitializeComponent();
            FlowDirection = (WaselDriver.Helper.Settings.LastUserGravity == "Arabic") ?
                FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }


        private void PopAlert(bool x)
        {
            PopupNavigation.Instance.PushAsync(new RequestPopUp(x, 2));
            return;
        }
        private bool AllFieldsFilled()
        {
            bool check = ((String.IsNullOrEmpty(EntryEmail.Text))) ? false : true;
            if (EntryEmail.Text == null || EntryEmail.Text.Length < 1)
            {
                Activ.IsRunning = false;
                DisplayAlert("خطأ", "من فضلك أكمل الحقول الفارغة", "OK");
            }
            return check;

        }
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            if (MyChecked.IsChecked)
            {
                MailGrid.IsVisible = false;
                CodeGrid.IsVisible = true;
            }
            else
            {
                if (AllFieldsFilled())
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        string email = EntryEmail.Text;
                        var ResBack = await UserServices.BackupEmail(email);
                        if (ResBack == null)
                        {
                            await DisplayAlert("Communication Error", "من فضلك تحقق من البريد الإلكتروني الخاص بحسابك", "OK");
                        }
                        else
                        {
                            bool checker = false;
                            try
                            {
                                Activ.IsRunning = false;
                                var JsonResponse = JsonConvert.DeserializeObject<Response<string, string>>(ResBack);
                                if (JsonResponse.data == "success")
                                {
                                    checker = true;
                                    await DisplayAlert("تنبيه", "لقد تم إرسال رقم التأكد من الهوية إلي البريد الإلكتروني الذى تم إدخاله .. برجاء التحقق من البريد كما يرجي التنبيه بأنه من المحتمل أن يوجد في القائمة المحظورة أو Spam", "موافق");
                                    Helper.Settings.LastUsedEmail = EntryEmail.Text;
                                    MailGrid.IsVisible = false;
                                    CodeGrid.IsVisible = true;
                                }
                                else
                                {
                                    Activ.IsRunning = false;
                                    await DisplayAlert("تنبيه", "عفوا لايوجد حساب لهذا البريد الإلكتروني من فضلك قم بالتسجيل ", "موافق");
                                    Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new LoginPage());
                                    return;
                                }

                            }
                            catch (Exception)
                            {

                                await DisplayAlert("تنبيه", "عفوا لايوجد حساب لهذا البريد الإلكتروني من فضلك قم بالتسجيل ", "موافق");
                                Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new LoginPage());
                                return;
                            }
                        }

                        Activ.IsRunning = false;
                    }
                    else
                    {
                        await DisplayAlert("تنبيه", AppResources.ErrorMessage, "موافق");
                    }
                }
                else await DisplayAlert("تنبيه", "من فضلك تحقق من البريد الإلكتروني الخاص بحسابك !!", "موافق");
            }
            Activ.IsRunning = false;
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private void MyChecked_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            EntryEmail.IsEnabled = !EntryEmail.IsEnabled;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            if (EntryCode.Text != null)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var resback = await UserServices.CodeVerfication(Helper.Settings.LastUsedEmail, EntryCode.Text);
                    try
                    {
                        var mess = JsonConvert.DeserializeObject<Response<string, string>>(resback);
                        if (mess.data == "success")
                        {
                            Activ.IsRunning = false;
                            CodeGrid.IsVisible = false;
                            PassGrid.IsVisible = true;
                        }
                        else await DisplayAlert("تنبيه", "من فضلك تحقق من الرقم التعريفي المرسل للبريد الذي تم ذكره مسبقاً !!", "موافق");
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("تنبيه", "من فضلك تحقق من الرقم التعريفي المرسل للبريد الذي تم ذكره مسبقاً !!", "موافق");
                    }
                    Activ.IsRunning = false;
                }
                else
                {
                    await DisplayAlert("تنبيه", AppResources.ErrorMessage, "موافق");
                }
            }
            else await DisplayAlert("تنبيه", "من فضلك أدخل الكود المرسل لك عبر البريد الإلكتروني الذي أدخلته مسبقاً", "موافق");

        }
        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            if (AllFieldsFilled2())
            {
                var response = await UserServices.ResetPassword(EntryNewPass.Text);
                var mess = JsonConvert.DeserializeObject<Response<string, string>>(response);
                if (mess.data == "success")
                {
                    await DisplayAlert("تنبيه", mess.message, "موافق");
                    App.Current.MainPage = new LoginPage();
                }
                else await DisplayAlert("تنبيه", "من فضلك تحقق من كلمة السر", "موافق");

            }
            else await DisplayAlert("تنبيه", "من فضلك أكمل الفراغات", "موافق");
            Activ.IsRunning = false;
        }
        private bool AllFieldsFilled2()
        {

            bool check = false;
            if (EntryNewPass.Text != EntryConfirmPass.Text)
            {
                Activ.IsRunning = false;
                DisplayAlert("تنبيه", "عفواً كلمة السر غير متطابقة!!", "موافق");
            }
            else if (EntryNewPass.Text == "" || EntryConfirmPass.Text == "")
            {
                DisplayAlert("تنبيه", "عفواً كلمة السر غير متطابقة!!", "موافق");
            }
            else return true;

            return check;

        }


    }
}