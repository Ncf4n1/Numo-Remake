using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;

using NuMo_Test.DatabaseItems;

namespace NuMo_Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualizePage : ContentPage
    {
        DateTime date;
        List<IMyDayViewItem> ViewItemList;

        Stopwatch stopwatch = new Stopwatch();
        bool pageIsActive;
        double fill;

        int daysToLoad = 1;

        // lists of values for each DRI nutrient
        List<string> nutNames = new List<string>();
        List<string> nutDRINames = new List<string>();
        List<double> DRIlow = new List<double>();
        List<double> DRImed = new List<double>();
        List<double> DRIhigh = new List<double>();
        List<double> nutConsumed = new List<double>();
        List<bool> DRIlimit = new List<bool>(); // true if the nutrient is harmless beyond DRI
        List<bool> displayNut = new List<bool>(); // which nutrients should be displayed

        List<SKCanvas> visualSKCanvases = new List<SKCanvas>(); // the list of SKCanvases to draw upon

        // light shades used for unfilled visualize bar drawings
        SKColor LIGHTRED = new SKColor(255, 181, 181);
        SKColor LIGHTGREEN = new SKColor(211, 255, 188);
        SKColor LIGHTYELLOW = new SKColor(255, 255, 171);
        SKColor BASEBACKGROUND = new SKColor(33, 150, 243);

        public VisualizePage(List<Nutrient> nutrientList)
        {
            InitializeComponent();

            date = datePicker.Date;
            ViewItemList = new List<IMyDayViewItem>();
            var timeArr = new String[] { "Today", "This Week", "This Month" };
            timePicker.ItemsSource = timeArr;
            timePicker.SelectedItem = timeArr[0];

        }

        /* method to populate the DRI lists and names
         * used as a precurser to creating the visualize elements
         * uses the database accessor methods:
         * GetNutNames, GetDRINames, getDRIValue, DRIthresholds.
         */
        private void InitializeDRIs()
        {
            var db = DataAccessor.getDataAccessor();

            // initalize DRI and readable names
            nutNames = db.GetNutNames();
            nutDRINames = db.GetDRINames();

            foreach (var DRIname in nutDRINames)
            {
                var DRIthreshholds = db.getDRIThresholds(DRIname);
                var midDRI = Double.Parse(db.getDRIValue(DRIname)) * daysToLoad;

                DRImed.Add(midDRI);

                /* Accessing DRIThreshholds from database was causing null errors.
                 * Current low and high calculations are done with these constant 
                 * multipliers in DRIPage, so base funtionallity is present here
                 * Will need to get updated upon DRI threshold changes                
                 */               
                DRIlow.Add(midDRI * .25 * daysToLoad);
                DRIhigh.Add(midDRI * 1.25 * daysToLoad);
            }

            //TODO: Unify order of Nutrients
            foreach (var item in GetNutrients())
            {
                nutConsumed.Add(item.quantity);
            }
        }

        /* Method to create the underlying empty bar for nutritional visualizations       
         * Called for each base cell of each nutrient
         */
        private void DrawBaseVisual(SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;

            using (SKPaint paint = new SKPaint())
            {
                canvas.Clear(BASEBACKGROUND);

                var HORIZONTALPADDING = 30f;
                var VERTICALPADDING = 15f;

                var width = e.Info.Width - 2*HORIZONTALPADDING;
                var height = e.Info.Height - 2*VERTICALPADDING;

                // The left rounded edge of the nutrition visual
                // Starts Red to indicate that it could fill up
                SKRoundRect firstRound = new SKRoundRect(new SKRect(HORIZONTALPADDING-15f, VERTICALPADDING, HORIZONTALPADDING+15f, height + VERTICALPADDING), 15f, 15f);
                paint.Color = Color.Red.ToSKColor();
                canvas.DrawRoundRect(firstRound, paint);

                // The right rounded edge
                SKRoundRect secondRound = new SKRoundRect(new SKRect(width + 15f, VERTICALPADDING, width + 45f, height + VERTICALPADDING), 15f, 15f);
                paint.Color = LIGHTRED;
                canvas.DrawRoundRect(secondRound, paint);

                // Rectangle for consumed DRI 0 tp Min Threshold
                SKRect firstRect = new SKRect(HORIZONTALPADDING, VERTICALPADDING, (width / 10) + HORIZONTALPADDING, height + VERTICALPADDING);
                paint.Color = LIGHTRED;
                canvas.DrawRect(firstRect, paint);

                // Rectangle for consumed DRI from Min Threshold to half way between Min and DRI
                SKRect secRect = new SKRect((width / 10) + HORIZONTALPADDING, VERTICALPADDING, (width / 10) * 3 + HORIZONTALPADDING, height + VERTICALPADDING);
                paint.Color = LIGHTYELLOW;
                canvas.DrawRect(secRect, paint);

                // Rectangle for consumed DRI from 
                // half way between low thresh and DRI to half way from DRI to max threshhold
                SKRect thirdRect = new SKRect((width / 10) * 3 + HORIZONTALPADDING, VERTICALPADDING, (width / 10) * 7 + HORIZONTALPADDING, height + VERTICALPADDING);
                paint.Color = LIGHTGREEN;
                canvas.DrawRect(thirdRect, paint);

                // Rectangle for consumed DRI from 
                // halfway between DRI and max threshhold to max threshold
                SKRect fourRect = new SKRect((width / 10) * 7 + HORIZONTALPADDING, VERTICALPADDING, (width / 10) * 9 + HORIZONTALPADDING, height + VERTICALPADDING);
                paint.Color = LIGHTYELLOW;
                canvas.DrawRect(fourRect, paint);

                // Rectangle for consumed DRI from max threshold to twice the regular DRI
                SKRect fiveRect = new SKRect(((width / 10) * 9) + HORIZONTALPADDING, VERTICALPADDING, width + HORIZONTALPADDING, height + VERTICALPADDING);
                paint.Color = LIGHTRED;
                canvas.DrawRect(fiveRect, paint);

            }
        }

        private List<Nutrient> GetNutrients()
        {
            date = datePicker.Date;
            var db = DataAccessor.getDataAccessor();
            var baseList = new List<FoodHistoryItem>();
            for (int i = 0; i < daysToLoad; i++)
            {
                baseList.AddRange(db.getFoodHistoryList(date.AddDays(-i).ToString()));
            }
            var nutrientList = db.getNutrientsFromHistoryList(baseList);

            foreach (var item in nutrientList)
            {
                if (item.DisplayName != "Omega6/3 Ratio")
                    item.quantity /= daysToLoad;
            }
            return nutrientList;
        }

        //Allow user to update the current date
        void dateClicked(object sender, DateChangedEventArgs e)
        {
            date = e.NewDate;
            this.OnAppearing();
        }

        //Allow user to update their choice of time range
        void OnTimeLengthChoiceChanged(object sender, EventArgs e)
        {
            //this.Title = timeLengthChoice.Items.ElementAt(timeLengthChoice.SelectedIndex);
            if ((String)timePicker.SelectedItem == "Today")
            {
                daysToLoad = 1;
            }
            else if ((String)timePicker.SelectedItem == "This Week")
            {
                daysToLoad = 7;
            }
            else if ((String)timePicker.SelectedItem == "This Month")
            {
                daysToLoad = 30;
            }
            this.OnAppearing();
        }

        //TODO: Implement animation on appearing
        protected override void OnAppearing()
        {
            base.OnAppearing();

            InitializeDRIs();
            pageIsActive = true;
            AnimateBar();
            //AnimationLoop();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageIsActive = false;
        }

        async Task AnimateBar()
        {
            stopwatch.Start();
            double startTime = stopwatch.Elapsed.TotalSeconds;
            double elapsedTime = stopwatch.Elapsed.TotalSeconds - startTime;

            double animationTime = 3;

            while (pageIsActive && elapsedTime <= animationTime)
            {
                elapsedTime = stopwatch.Elapsed.TotalSeconds - startTime;
                fill = Math.Sin(Math.PI / 2 * elapsedTime / animationTime);
                //TODO: for BarCanvasList, InvalidateSurface
                CaloriesBarCanvas.InvalidateSurface();
                await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
            }

            stopwatch.Stop();
        }

        void OnVisualizePaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            DrawBaseVisual(args);
            OnVisualizeAnimateBar(args);
        }

        void OnVisualizeAnimateBar(SKPaintSurfaceEventArgs e)
        {
            var width = e.Info.Width - 60;
            var height = e.Info.Height - 30;
            var canvas = e.Surface.Canvas;

            int DRIcount = nutDRINames.IndexOf("dri_calories");

            using (SKPaint paint = new SKPaint())
            {
                var consumed = nutConsumed.ElementAt(DRIcount) / DRImed.ElementAt(DRIcount);
                double lengthBar = 0; // values should be from 0 to width

                // cases to deturmine the length of the total bar width filled by current consumed nutrients
                if (consumed <= .25) lengthBar = 4 * consumed * width / 10;
                else if (consumed <= .625) lengthBar = width / 10 + (consumed - .25) / .375 * 2 * width / 10;
                else if (consumed <= 1) lengthBar = 3 * width / 10 + (consumed - .625) / .375 * 2 * width / 10;
                else if (consumed <= 1.125) lengthBar = 5 * width / 10 + (consumed - 1) / .125 * 2 * width / 10;
                else if (consumed <= 1.25) lengthBar = 7 * width / 10 + (consumed - 1.125) / .125 * 2 * width / 10;
                else if (consumed <= 2) lengthBar = 9 * width / 10 + (consumed - 1.25) / .75 * width / 10;
                else lengthBar = width;

                var proportion = lengthBar / width;
                var currentFill = (float)(fill * proportion);

                var VERTICALPADDING = 15f;
                var HORIZONTALPADDING = 30f;

                // First red rectangle, representing 0 to minDRI nutrients consumed
                if (currentFill > 0) 
                {
                    paint.Color = Color.Red.ToSKColor();
                    if (currentFill <= .1)
                    {
                        canvas.DrawRect(new SKRect(HORIZONTALPADDING, VERTICALPADDING, currentFill * width + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                    else
                    {
                        canvas.DrawRect(new SKRect(HORIZONTALPADDING, VERTICALPADDING, (width / 10) + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                }

                // first yellow rectangle, representing minDRI to halfway between minDRI and DRI
                if (currentFill > .1)
                {
                    paint.Color = Color.Yellow.ToSKColor();
                    if (currentFill <= .3) // less than half way from min DRI to DRI
                    {
                        canvas.DrawRect(new SKRect((width / 10) + HORIZONTALPADDING, VERTICALPADDING, currentFill * width + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                    else
                    {
                        canvas.DrawRect(new SKRect((width / 10) + HORIZONTALPADDING, VERTICALPADDING, (width / 10) * 3 + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                }

                // first green rectangle, representing halfway between minDRI and DRI to DRI
                if (currentFill > .3)
                {
                    paint.Color = Color.Green.ToSKColor();
                    if (currentFill <= .5)
                    {
                        canvas.DrawRect(new SKRect(3 * width / 10 + HORIZONTALPADDING, VERTICALPADDING, currentFill * width + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                    else
                    {
                        canvas.DrawRect(new SKRect(3 * width / 10 + HORIZONTALPADDING, VERTICALPADDING, 5 * width / 10 + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                }

                // second green rectangle, representing DRI to halfway between DRI and maxDRI
                if (currentFill > .5)
                {
                    paint.Color = Color.Green.ToSKColor();
                    if (currentFill <= .7)
                    {
                        canvas.DrawRect(new SKRect(5 * width / 10 + HORIZONTALPADDING, VERTICALPADDING, currentFill * width + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                    else
                    {
                        canvas.DrawRect(new SKRect(5 * width / 10 + HORIZONTALPADDING, VERTICALPADDING, 7 * width / 10 + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                }

                // second yellow rectangle, representing halfway between DRI and maxDRI to maxDRI
                if (currentFill > .7)
                {
                    paint.Color = Color.Yellow.ToSKColor();
                    if (currentFill <= .9) // less than max DRI
                    {
                        canvas.DrawRect(new SKRect(7 * width / 10 + HORIZONTALPADDING, VERTICALPADDING, currentFill * width + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                    else
                    {
                        canvas.DrawRect(new SKRect(7 * width / 10 + HORIZONTALPADDING, VERTICALPADDING, 9 * width / 10 + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                }

                // second red rectangle, representing maxDRI to twice maxDRI. greater than twice maxDRI doesn't get visualized.
                if (currentFill > .9)
                {
                    paint.Color = Color.Red.ToSKColor();
                    if (currentFill < 2)
                    {
                        canvas.DrawRect(new SKRect(9 * width / 10 + HORIZONTALPADDING, VERTICALPADDING, currentFill * width + HORIZONTALPADDING, height + VERTICALPADDING), paint);
                    }
                    else // above twice DRI
                    {
                        canvas.DrawRect(9 * width / 10 + 30f, VERTICALPADDING, width + 30f, height + VERTICALPADDING, paint);
                        canvas.DrawRoundRect(width - VERTICALPADDING, VERTICALPADDING, 30f, height + VERTICALPADDING, VERTICALPADDING, VERTICALPADDING, paint);
                    }
                }
            }

        }
    }
}