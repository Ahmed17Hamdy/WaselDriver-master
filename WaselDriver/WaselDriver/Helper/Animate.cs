using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WaselDriver.Helper
{
    public class Animate
    {
        //
        public static async Task BallAnimate(Image view, int Top, int reduce, int time)
        {
            await view.RelRotateTo(360, 1000);
            do
            {
                await view.TranslateTo(view.TranslationX, view.TranslationY - Top, 500, Easing.CubicOut);

                await view.TranslateTo(view.TranslationX, view.TranslationY + Top, 300, Easing.CubicIn);

                Top = Top - reduce;
                time--;
            } while (time != 0);


            await view.TranslateTo(view.TranslationX, view.TranslationY - 50, 500, Easing.Linear);
            await view.TranslateTo(view.TranslationX, view.TranslationY + 50, 300, Easing.Linear);
            await view.TranslateTo(view.TranslationX, view.TranslationY - 20, 300, Easing.Linear);
            await view.TranslateTo(view.TranslationX, view.TranslationY + 20, 150, Easing.Linear);
            await view.TranslateTo(view.TranslationX, view.TranslationY - 10, 150, Easing.Linear);
            await view.TranslateTo(view.TranslationX, view.TranslationY + 10, 100, Easing.Linear);
            await view.FadeTo(-0, 1000);

        }
    }
}
