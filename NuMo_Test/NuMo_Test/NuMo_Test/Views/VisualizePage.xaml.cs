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


namespace NuMo_Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualizePage : ContentPage
    { 
        DateTime date;
        List<IMyDayViewItem> ViewItemList;

        int daysToLoad = 1;

        SKCanvas canvas1;
        int width1 = 0;
        int height1 = 0;

        // lists of values for each DRI nutrient
        List<string> nutNames = new List<string>();
        List<string> nutDRINames = new List<string>();
        List<double> DRIlow = new List<double>();
        List<double> DRImed = new List<double>();
        List<double> DRIhigh = new List<double>();
        List<double> nutConsumed = new List<double>();
        List<bool> DRIlimit = new List<bool>(); // true if the nutrient is harmless beyond DRI
        List<bool> displayNut = new List<bool>(); // which nutrients should be displayed

        // light shades used for unfilled visualize bar drawings
        SKColor LIGHTRED = new SKColor(255, 181, 181);
        SKColor LIGHTGREEN = new SKColor(211, 255, 188);
        SKColor LIGHTYELLOW = new SKColor(255, 255, 171);
        SKColor BASEBACKGROUND = new SKColor(33, 150, 243);

        public VisualizePage(List<Nutrient> nutrientList)
        {
            InitializeComponent();
            InitializeDRIs();

            ViewItemList = new List<IMyDayViewItem>();
            var timeArr = new String[] { "Today", "This Week", "This Month" };
            timePicker.ItemsSource = timeArr;
            timePicker.SelectedItem = timeArr[0];

            AnimateBar(0, height1, width1, canvas1, new SKPaint());

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

            //TODO: Remove this, add consumed info
            nutConsumed.Add(2500);
            nutConsumed.Add(130);

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
        }

        private void SetBaseCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            width1 = e.Info.Width - 60;
            height1 = e.Info.Height - 30;
            canvas1 = e.Surface.Canvas;
            DrawBaseVisual(e);
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

                var width = e.Info.Width - 60;
                var height = e.Info.Height - 30;

                // The left rounded edge of the nutrition visual
                // Starts Red to indicate that it could fill up
                SKRoundRect firstRound = new SKRoundRect(new SKRect(15f, 15f, 45f, height + 15f), 15f, 15f);
                paint.Color = Color.Red.ToSKColor();
                canvas.DrawRoundRect(firstRound, paint);

                // The right rounded edge
                SKRoundRect secondRound = new SKRoundRect(new SKRect(width + 15f, 15f, width + 45f, height + 15f), 15f, 15f);
                paint.Color = LIGHTRED;
                canvas.DrawRoundRect(secondRound, paint);

                // Rectangle for consumed DRI 0 tp Min Threshold
                SKRect firstRect = new SKRect(30, 15, (width / 10) + 25f + 30, height + 15);
                paint.Color = LIGHTRED;
                canvas.DrawRect(firstRect, paint);

                // Rectangle for consumed DRI from Min Threshold to half way between Min and DRI
                SKRect secRect = new SKRect((width / 10) + 30, 15, (width / 10) * 3 + 30, height + 15);
                paint.Color = LIGHTYELLOW;
                canvas.DrawRect(secRect, paint);

                // Rectangle for consumed DRI from 
                // half way between low thresh and DRI to half way from DRI to max threshhold
                SKRect thirdRect = new SKRect((width / 10) * 3 + 30, 15, (width / 10) * 7 + 30, height + 15);
                paint.Color = LIGHTGREEN;
                canvas.DrawRect(thirdRect, paint);

                // Rectangle for consumed DRI from 
                // halfway between DRI and max threshhold to max threshold
                SKRect fourRect = new SKRect((width / 10) * 7 + 30, 15, (width / 10) * 9 + 30, height + 15);
                paint.Color = LIGHTYELLOW;
                canvas.DrawRect(fourRect, paint);

                // Rectangle for consumed DRI from max threshold to twice the regular DRI
                SKRect fiveRect = new SKRect(((width / 10) * 9) - 25f + 30, 15, width + 30, height + 15);
                paint.Color = LIGHTRED;
                canvas.DrawRect(fiveRect, paint);

            }
        }

        async Task AnimateBar(int DRIcount, double barHeight, double barWidth, SKCanvas canvas, SKPaint paint)
        {
            float height = (float) barHeight;
            float width = (float) barWidth;

            // Stopwatch to animate bars
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double t = stopwatch.Elapsed.TotalSeconds;
            while (t <= 1)
            {
                var fill = Math.Sin(Math.PI / 2 * t);
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

                if(currentFill <= .1) // less than min DRI consumed
                {
                    paint.Color = Color.Red.ToSKColor();
                    canvas.DrawRect(new SKRect(30f, 15f,  currentFill*width+30f, height + 15f), paint);
                }
                else if(currentFill <= .3) // less than half way from min DRI to DRI
                {
                    paint.Color = Color.Red.ToSKColor();
                    canvas.DrawRect(new SKRect(30, 15, (width / 10) + 25f + 30, height + 15), paint);

                    paint.Color = Color.Yellow.ToSKColor();
                    canvas.DrawRect(new SKRect(width / 10 + 30f, 15f, currentFill * width + 30f, height + 15f), paint);
                }
                else if (currentFill <= .5) // less than DRI
                {
                    paint.Color = Color.Yellow.ToSKColor();
                    canvas.DrawRect(new SKRect((width / 10) + 30, 15, (width / 10) * 3 + 30, height + 15), paint);

                    paint.Color = Color.Green.ToSKColor();
                    canvas.DrawRect(new SKRect(3 * width / 10 + 30f, 15f, currentFill * width + 30f, height + 15f), paint);
                }
                else if (currentFill <= .7) // less than half way from DRI to max DRI 
                {
                    paint.Color = Color.Green.ToSKColor();
                    canvas.DrawRect(new SKRect(3 * width / 10 + 30f, 15f, 5 * width / 10 + 30f, height + 15f), paint);

                    paint.Color = Color.Green.ToSKColor();
                    canvas.DrawRect(new SKRect(5 * width / 10 + 30f, 15f, currentFill * width + 30f, height + 15f), paint);
                }
                else if (currentFill <= .9) // less than max DRI
                {
                    paint.Color = Color.Green.ToSKColor();
                    canvas.DrawRect(new SKRect(5 * width / 10 + 30f, 15f, 7 * width / 10 + 30f, height + 15f), paint);

                    paint.Color = Color.Yellow.ToSKColor();
                    canvas.DrawRect(new SKRect(7 * width / 10 + 30f, 15f, currentFill * width + 30f, height + 15f), paint);
                }
                else if (currentFill < 1) // less than twice DRI
                {
                    paint.Color = Color.Yellow.ToSKColor();
                    canvas.DrawRect(new SKRect(7 * width / 10 + 30f, 15f, 9 * width / 10 + 30f, height + 15f), paint);

                    paint.Color = Color.Red.ToSKColor();
                    canvas.DrawRect(new SKRect(9 * width / 10 + 30f, 15f, currentFill * width + 30f, height + 15f), paint);
                }
                else // above twice DRI
                {
                    paint.Color = Color.Red.ToSKColor();
                    canvas.DrawRect(9 * width / 10 + 30f, 15f, width + 30f, height + 15f, paint);
                    canvas.DrawRoundRect(width-15f, 15f, 30f, height+15f, 15f, 15f, paint);
                }

                t = stopwatch.Elapsed.TotalSeconds;
                Task.Delay(16).Wait();
                await Task.Delay(0);
            }

            stopwatch.Stop();

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

    }
}