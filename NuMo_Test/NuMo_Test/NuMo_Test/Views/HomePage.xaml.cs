using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using NuMo_Test.Models;
using NuMo_Test.Views;
using NuMo_Test.ViewModels;
using NuMo_Test.ItemViews;

namespace NuMo_Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;
        List<IMyDayViewItem> ViewItemList;
        int daysToLoad = 1;
        DateTime date;

        public HomePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HomeViewModel();
            ViewItemList = new List<IMyDayViewItem>();
            date = datePicker.Date;
        }

        //Display the food items associated with today, and back in time to the number of selected days.
        void OnItemsClicked()
        {

            //Image pic2 = new Image();
            //if (Application.Current.Properties.ContainsKey("Profile Pic"))
            //{
            //    pic2 = Application.Current.Properties["Profile Pic"] as Image;
            //    pic.Source = pic2.Source;
            //}

            listView.BeginRefresh();
            listView.ItemsSource = null;
            var db = DataAccessor.getDataAccessor();
            ViewItemList.Clear();
            var remainderList = new List<MyDayRemainderItem>();
            for (int i = 0; i < daysToLoad; i++)
            {
                remainderList = db.getRemainders(date.AddDays(-i).ToString());
                foreach (var item in remainderList)
                {
                    ViewItemList.Add(item);
                }
            }
            for (int i = 0; i < daysToLoad; i++)
            {
                var baseList = db.getFoodHistory(date.AddDays(-i).ToString());

                foreach (var item in baseList)
                {
                    ViewItemList.Add(item);
                }
            }
            listView.ItemsSource = ViewItemList;
            listView.EndRefresh();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            //ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}