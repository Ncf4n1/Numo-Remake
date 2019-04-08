using NuMo_Test.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NuMo_Test.DatabaseItems;

namespace NuMo_Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        DateTime date = DateTime.Now;

        int daysToLoad = 1;

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
                        var foodHistory = AddItemToFoodHistory.getFoodHistory(date);
                        MenuPages.Add(id, new NavigationPage(foodHistory));
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
                    case (int)MenuItemType.CameraPage:
                        MenuPages.Add(id, new NavigationPage(new CameraStuff(date)));
                        break;
                    case (int)MenuItemType.Hydration:
                        MenuPages.Add(id, new NavigationPage(new Hydration()));
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

        private List<Nutrient> getNutrients()
        {
            var db = DataAccessor.getDataAccessor();
            var baseList = new List<FoodHistoryItem>();
            for (int i = 0; i < daysToLoad; i++)
            {
                baseList.AddRange(db.getFoodHistoryList(date.AddDays(-i).ToString()));
            }
            var nutrientList = db.getNutrientsFromHistoryList(baseList);

            foreach (var item in nutrientList)
            {
                item.quantity /= daysToLoad;
            }
            return nutrientList;
        }
    }
}
