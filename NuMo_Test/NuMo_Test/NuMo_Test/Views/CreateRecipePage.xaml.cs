using NuMo_Test.DatabaseItems;
using NuMo_Test.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuMo_Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateRecipePage : ContentPage
    {
        List<FoodHistoryItem> recipeList = new List<FoodHistoryItem>();

        public CreateRecipePage()
        {
            InitializeComponent();
        }

        async void SaveRecipe(object sender, EventArgs e)
        {

        }

        async void AddIngredient(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddItemToRecipe(recipeList));
        }
    }
}