using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuMo_Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DRIPage : ContentPage
    {
        public DRIPage()
        {
            InitializeComponent();
        }

        async void GetHelpInfo(object sender, EventArgs e)
        {
            await DisplayAlert("DRI Information", "Dietary Reference Intakes refers to a set of values that assess your nutrition intake. These values are the recommended minimal values. You may set your own target values based on your needs and goals.", "OK");
        }

        async void CustomizeDRIs(object sender, EventArgs e)
        {
            await DisplayAlert("Customize", "Are you sure you want to customize your recommended Dietary Reference Intakes?", "No", "Yes");
        }

        async void ResetDRIs(object sender, EventArgs e)
        {
            await DisplayAlert("Reset", "Are you sure you want to reset your Dietary Reference Intakes to the minimum recommended values?", "No", "Yes");
        }

        internal void saveNoLoad()
        {
            throw new NotImplementedException();
        }
    }
}