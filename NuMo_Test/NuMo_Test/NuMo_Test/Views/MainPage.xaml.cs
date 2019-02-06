using NuMo_Test.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuMo_Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));
                        break;
                    case (int)MenuItemType.AddFood:
                        MenuPages.Add(id, new NavigationPage(new AddFoodPage()));
                        break;
                    case (int)MenuItemType.CreateRecipe:
                        MenuPages.Add(id, new NavigationPage(new CreateRecipePage()));
                        break;
                    case (int)MenuItemType.CreateFood:
                        MenuPages.Add(id, new NavigationPage(new CreateFoodPage()));
                        break;
                    case (int)MenuItemType.Visualize:
                        MenuPages.Add(id, new NavigationPage(new VisualizePage()));
                        break;
                    case (int)MenuItemType.DRI:
                        MenuPages.Add(id, new NavigationPage(new DRIPage()));
                        break;
                    case (int)MenuItemType.Settings:
                        MenuPages.Add(id, new NavigationPage(new SettingsPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}
