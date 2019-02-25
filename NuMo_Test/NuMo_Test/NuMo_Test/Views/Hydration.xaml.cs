using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuMo_Test.Views
{

    public partial class Hydration : ContentPage
    {
        public Hydration()
        {
            InitializeComponent();
            this.Title = "Waterworld";
        }
        private async void AddWater_OnClicked(object sender, EventArgs e)
        {
            // If the add button is clicked
            await DisplayAlert("Yay", "Water successfully drank", "Yeet");


        }

        private async void MinusWater_OnClicked(object sender, EventArgs e)
        {
            // If the loss button is picked
            await DisplayAlert("Oh No", "Lost Water", "OK");

        }
    }
}