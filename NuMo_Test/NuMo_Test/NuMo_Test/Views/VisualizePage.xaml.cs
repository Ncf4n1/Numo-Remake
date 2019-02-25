using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuMo_Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualizePage : ContentPage
    {
        List<String> names;
        List<Double> quantities;
        List<Double> dris;
        List<ProgressBar> progBars;
        IDictionary<String, Double[]> items;
        int dayMultiplier;

        public VisualizePage(List<Nutrient> nutrientList)
        {

            InitializeComponent();

            //nutrient names
            names = new List<String>();
            //quantities consumed
            quantities = new List<Double>();
            //dri values
            dris = new List<Double>();
            //Progress Bars
            progBars = new List<ProgressBar>();

            items = new Dictionary<String, Double[]>();
            initializeItems();

            //find how many days to calculate nutrients for
            //Title += " " + titleExtra;
            //if (titleExtra[0] == '7')
            //{
            //    dayMultiplier = 7;
            //}
            //else if (titleExtra[0] == '3')
            //{
            //    dayMultiplier = 30;
            //}
            //else
            //{
            //    dayMultiplier = 1;
            //}

            //call to fill the names/quantities/dri lists
            getData(nutrientList);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //animate the progress bars
            drawBars();
        }

        private void drawBars()
        {
            for  (int i = 0; i < names.Count; i++){
                var quantity = quantities[i];
                var dri = dris[i];
                var fill = quantity / dri;

                var bar = new ProgressBar();
                bar.Progress = fill;
                progBars.Add(bar);
            }
        }

        private void getData(List<Nutrient> nutrientList)
        {

            foreach (var item in nutrientList)
            {
                //add nutrient names to list to be passed
                names.Add(item.name);
                //add quantities to list to be passed
                quantities.Add(item.quantity);

                try
                {
                    items[item.name][0] = item.quantity;
                }
                catch (Exception) { }
            }
            //getDRI();
        }


        //get all of the DRI information, put it into List to be passed
        public void getDRI()
        {
            var db = DataAccessor.getDataAccessor();

            dris.Add(Convert.ToDouble(db.getDRIValue("dri_protein")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_totalCarbs")));
            //dris.Add(0); // sugar

            dris.Add(Convert.ToDouble(db.getDRIValue("dri_dietaryFiber")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_calcium")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_iron")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_magnesium")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_phosphorus")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_potassium")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_sodium")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_zinc")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_copper")));

            dris.Add(Convert.ToDouble(db.getDRIValue("dri_manganese")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_selenium")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminA")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminC")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_thiamin")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_riboflavin")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_niacin")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_pantothenicAcid")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminB6")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_folate")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminB12")));
            dris.Add(0); //omega 6 total
            dris.Add(0); //omega 3 total
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_netCarbs")));
            dris.Add(0); //total sugars
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminE")));
            dris.Add(Convert.ToDouble(db.getDRIValue("dri_vitaminK")));
            dris.Add(0); //omega 3/6 ratio

        }

        protected override void OnDisappearing()
        {
        }


        /// <summary>
        /// Where you can add nutrients to the list of progress bars shown
        /// </summary>
        private void initializeItems()
        {
            var db = DataAccessor.getDataAccessor();
            //DRI not currently being used in Visualizing

            //retreiving the thresholds from the database
            var sugarThresh = db.getDRIThresholds("dri_sugar");
            var omega63Thresh = db.getDRIThresholds("dri_omega6/3 ratio");
            var caloriesThresh = db.getDRIThresholds("dri_calories");
            var netCarbsThresh = db.getDRIThresholds("dri_netCarbs");
            var fiberThresh = db.getDRIThresholds("dri_dietaryFiber");
            var zincThresh = db.getDRIThresholds("dri_zinc");
            var totCarbsThresh = db.getDRIThresholds("dri_totalCarbs");

            //quantity = 0, dri value = 1, low threshold = 2, high threshold = 3

            double[] QDRI12 = { 0, Convert.ToDouble(db.getDRIValue("dri_sugar")), Convert.ToDouble(sugarThresh[0].lowThresh), Convert.ToDouble(sugarThresh[0].highThresh) };
            double[] QDRI15 = { 0, Convert.ToDouble(db.getDRIValue("dri_omega6/3 ratio")), Convert.ToDouble(omega63Thresh[0].lowThresh), Convert.ToDouble(omega63Thresh[0].highThresh) };
            double[] QDRI14 = { 0, Convert.ToDouble(db.getDRIValue("dri_calories")), Convert.ToDouble(caloriesThresh[0].lowThresh), Convert.ToDouble(caloriesThresh[0].highThresh) };
            double[] QDRI2 = { 0, Convert.ToDouble(db.getDRIValue("dri_netCarbs")), Convert.ToDouble(netCarbsThresh[0].lowThresh), Convert.ToDouble(netCarbsThresh[0].highThresh) };
            double[] QDRI13 = { 0, Convert.ToDouble(db.getDRIValue("dri_dietaryFiber")), Convert.ToDouble(fiberThresh[0].lowThresh), Convert.ToDouble(fiberThresh[0].highThresh) };
            double[] QDRI9 = { 0, Convert.ToDouble(db.getDRIValue("dri_zinc")), Convert.ToDouble(zincThresh[0].lowThresh), Convert.ToDouble(zincThresh[0].highThresh) };
            double[] QDRI16 = { 0, Convert.ToDouble(db.getDRIValue("dri_totalCarbs")), Convert.ToDouble(totCarbsThresh[0].lowThresh), Convert.ToDouble(totCarbsThresh[0].highThresh) };

            //double[] QDRI1 = { 0, Convert.ToDouble(db.getDRIValue("dri_protein"))};
            //double[] QDRI3 = { 0, Convert.ToDouble(db.getDRIValue("dri_calcium")) };
            //double[] QDRI4 = { 0, Convert.ToDouble(db.getDRIValue("dri_iron")) };
            //double[] QDRI5 = { 0, Convert.ToDouble(db.getDRIValue("dri_magnesium")) };
            //double[] QDRI6 = { 0, Convert.ToDouble(db.getDRIValue("dri_phosphorus")) };
            //double[] QDRI7 = { 0, Convert.ToDouble(db.getDRIValue("dri_potassium")) };
            //double[] QDRI8 = { 0, Convert.ToDouble(db.getDRIValue("dri_sodium")) };
            //double[] QDRI10 = { 0, Convert.ToDouble(db.getDRIValue("dri_copper")) };
            //double[] QDRI11 = { 0, Convert.ToDouble(db.getDRIValue("dri_manganese")) };

            /////////////////////////////////////////////////////////////////////
            //creating the items to be displayed in the progress bars
            items.Add("Total Sugars(g)", QDRI12);
            items.Add("Omega6/3 Ratio", QDRI15);
            items.Add("Calories", QDRI14);
            items.Add("Carbohydrates(g)", QDRI16);
            //items.Add("Net Carbohydrates(g)",QDRI2);
            items.Add("Total Dietary Fiber(g)", QDRI13);
            items.Add("Zinc(mg)", QDRI9);


            //items.Add("Protein(g)", QDRI1);
            //items.Add("Iron(mg)", QDRI4);
            //items.Add("Calcium(mg)", QDRI3);
            //items.Add("Magnesium(mg)", QDRI5);
            //items.Add("Phosphorus(mg)", QDRI6);
            //items.Add("Potassium(mg)", QDRI7);
            //items.Add("Sodium(mg)", QDRI8);
            //items.Add("Copper(mg)", QDRI10);
            //items.Add("Magnanese(mg)", QDRI11);




        }


    }
}