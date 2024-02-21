using Android.Content.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterPage : ContentPage
    {
        //цвета верхней и нижней частей прописываются в android/resources/values/styles.xml
        public EnterPage()
        {
            InitializeComponent();
        }

        private async void EMCButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EMCPage());
        }

        private async void AppointmentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppointmentPage());
        }
    }
}