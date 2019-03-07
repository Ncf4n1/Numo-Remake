using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuMo_Test.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KetoPage : ContentPage
	{
        public KetoPage()
        {
            InitializeComponent();

            this.Title = "Keto Index Information";
        }

        public void calculateGKI(object sender, EventArgs args)
        {
            try
            {
                double index = (double.Parse(glucoseEntry.Text) / 18) / double.Parse(ketoEntry.Text);
                Label indexText = new Label { Text = "Your Glucose Ketone Index is " + index.ToString("F"), FontSize = 21, TextColor = Color.FromHex("#00A4A7") };

                ketoStack.Children.Add(indexText);
            }
            catch (Exception e)
            {
                DisplayAlert("Invalid Input", "Please enter a number", "OK");
            }
        }
    }
}