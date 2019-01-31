using NuMo_Test.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuMo_Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<Models.MenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<Models.MenuItem>
            {
                new Models.MenuItem {Id = MenuItemType.Home, Title="Home" },
                new Models.MenuItem {Id = MenuItemType.CreateRecipe, Title="Create Recipe" },
                new Models.MenuItem {Id = MenuItemType.CreateFood, Title="Create Food" },
                new Models.MenuItem {Id = MenuItemType.Visualize, Title="Visualize"},
                new Models.MenuItem {Id = MenuItemType.DRI, Title="Dietary Reference Intake" },
                new Models.MenuItem {Id = MenuItemType.AddFood, Title="Add Food" },
                new Models.MenuItem {Id = MenuItemType.Settings, Title="Settings" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((Models.MenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}
