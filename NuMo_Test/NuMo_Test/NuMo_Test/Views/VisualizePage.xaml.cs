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

        // Stopwatch to animate bars
        Stopwatch stopwatch = new Stopwatch();

        public VisualizePage(List<Nutrient> nutrientList)
        {
            InitializeComponent();
            InitializeDRIs(); 

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
            nutConsumed.Add(2000);
            nutConsumed.Add(50);

            foreach (var DRIname in nutDRINames)
            {
                var DRIthreshholds = db.getDRIThresholds(DRIname);
                var midDRI = Double.Parse(db.getDRIValue(DRIname));

                DRImed.Add(midDRI);

                /* Accessing DRIThreshholds from database was causing null errors.
                 * Current low and high calculations are done with these constant 
                 * multipliers in DRIPage, so base funtionallity is present here
                 * Will need to get updated upon DRI threshold changes                
                 */               
                DRIlow.Add(midDRI * .25);
                DRIhigh.Add(midDRI * 1.25);
            }
        }

        /* Method to create the underlying empty bar for nutritional visualizations       
         * Called for each base cell of each nutrient
         */
        private void drawBaseVisual(object sender, SKPaintSurfaceEventArgs e)
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

        async Task AnimateBar()
        {
            stopwatch.Start();
            double t = stopwatch.Elapsed.TotalSeconds;
            while (t <= 1)
            {

            }
            stopwatch.Stop();

        }
    }
}