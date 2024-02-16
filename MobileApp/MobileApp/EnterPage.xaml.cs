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
        public EnterPage()
        {
            InitializeComponent();
        }

        private void EMCButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EMCPage());
        }

        private void AppointmentButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AppointmentPage());
        }
    }
}