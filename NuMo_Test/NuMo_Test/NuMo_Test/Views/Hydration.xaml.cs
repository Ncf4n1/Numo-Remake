using Plugin.LocalNotifications;
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
        int numberOfDrinks = 0;
        Image waterLevel = new Image();

        //initial setup
        public Hydration()
        {
            InitializeComponent();
            this.Title = "The Hydration Station";
            this.Drinks.Text = numberOfDrinks + " glasses of water";
            SetNewWaterImage(); //initialize water level at 0
            MainImage.Source = waterLevel.Source;
            CrossLocalNotifications.Current.Show("Stay Hydrated!", "Don't forget to drink water today!", 101, DateTime.Now.AddDays(1)); //send a notification to remind to stay hydrated every 24 hours
            
        }
        //action events for when water is added
        private void AddWater_OnClicked(object sender, EventArgs e)
        {
            // If the add button is clicked
            numberOfDrinks++;
            this.Drinks.Text = numberOfDrinks + " glasses of water";
            SetNewWaterImage(); //refresh water level
            MainImage.Source=waterLevel.Source; //display water level

        }

        //action events for when water loss button is activated
        private void MinusWater_OnClicked(object sender, EventArgs e)
        {
            // If the loss button is picked
            if(numberOfDrinks>0) numberOfDrinks--;
            this.Drinks.Text = numberOfDrinks + " glasses of water";
            SetNewWaterImage(); //set new appropriate image to display hydration
            MainImage.Source = waterLevel.Source;
            

        }

        //sets a new hydration image depending on the number of drinks
        private void SetNewWaterImage()
        {
            switch(numberOfDrinks){
                case 0:
                    //waterLevel = new Image { Source = "water.jpg" };
                    waterLevel.Source = "NoWater.png";
                    break;
                case 1:
                    waterLevel.Source = "LowWater.png";
                    break;
                case 2:
                    waterLevel.Source = "LowMedWater.png";
                    break;
                case 3:
                    waterLevel.Source = "LowHighWater.png";
                    break;
                case 4:
                    waterLevel.Source = "MedWater.png";
                    break;
                case 5:
                    waterLevel.Source = "MedLowWater.png";
                    break;
                case 6:
                    waterLevel.Source = "MedMedWater.png";
                    break;
                case 7:
                    waterLevel.Source = "MedHighWater.png";
                    break;
                default:
                    waterLevel.Source = "HighWater.png";
                    break;

            }
        }
    }
}