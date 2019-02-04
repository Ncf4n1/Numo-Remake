using NuMo_Test.ItemViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NuMo_Test.MyDayPageHelpers
{
    //Template selector for MyDayPage to differenciate how different objects should be displayed. All 
    //templates are in the MyDayPage.xaml file.
    class MyDayDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MyDayFoodItemTemplate { get; set; }
        public DataTemplate MyDayRemainderTemplate { get; set; }
        public DataTemplate MyDayNutrientTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is MyDayFoodItem)
                return MyDayFoodItemTemplate;
            if (item is MyDayRemainderItem)
                return MyDayRemainderTemplate;
            if (item is Nutrient)
                return MyDayNutrientTemplate;
            
            return null;
        }

        //Should never be used.
        private DataTemplate CreateTemplateToReportError(string message)
        {
            DataTemplate template = new DataTemplate(() =>
            {
                View view = new Label
                {
                    Text = message,
                    FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label))
                };

                ViewCell vc = new ViewCell
                {
                    View = view
                };

                return vc;
            });

            return template;
        }
    }
}
