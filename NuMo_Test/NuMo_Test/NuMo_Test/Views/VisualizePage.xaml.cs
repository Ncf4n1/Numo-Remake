using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Color LIGHTRED = new Color(255, 145, 140);
        Color LIGHTGREEN = new Color(145, 155, 143);
        Color LIGHTYELLOW = new Color(255, 255, 128);


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

            foreach(var DRIname in nutDRINames)
            {
                var DRIthreshholds = db.getDRIThresholds(DRIname);
                var midDRI = Double.Parse(db.getDRIValue(DRIname));

                DRImed.Add(midDRI);
                DRIlow.Add(midDRI*.25);
                DRIhigh.Add(midDRI * 1.25);

            }

        }

        private void OnPaintSample(object sender, SKPaintSurfaceEventArgs e)
        {

            int surfaceWidth = e.Info.Width;
            int surfaceHeight = e.Info.Height;
            float side = Math.Min(surfaceHeight, surfaceWidth) * 0.5f;
            float drawWidth = surfaceWidth - 20;
            SKCanvas canvas = e.Surface.Canvas;

            using (SKPaint paint = new SKPaint())
            {
                canvas.Clear(Color.Black.ToSKColor());
                SKRect calRect = new SKRect(10f, 20f, surfaceWidth - 10, surfaceHeight - 20);
                SKRoundRect calRRect = new SKRoundRect(calRect, 25f, 25f);
                paint.Color = Color.Blue.ToSKColor();
                canvas.DrawRoundRect(calRRect, paint);
            }

        }

        private void OnPaintSample2(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;

            using (SKPaint paint = new SKPaint())
            {
                drawBaseVisual(canvas,paint,e);
            }

        }

        private void drawBaseVisual(SKCanvas canvas, SKPaint paint, SKPaintSurfaceEventArgs e)
        {
            canvas.Clear(Color.Blue.ToSKColor());

            var width = e.Info.Width - 30;
            var height = e.Info.Height - 30;

            SKRect firstRect = new SKRect(15, 15, (width / 10) + 25f + 15, height + 15);
            SKRoundRect firstRRect = new SKRoundRect(firstRect, 15f, 15f);
            paint.Color = Color.Salmon.ToSKColor();
            canvas.DrawRoundRect(firstRRect, paint);

            SKRect secRect = new SKRect((width / 10) + 15, 15, (width / 10) * 3 + 15, height + 15);
            paint.Color = Color.LightGoldenrodYellow.ToSKColor();
            canvas.DrawRect(secRect, paint);

            SKRect thirdRect = new SKRect((width / 10) * 3 + 15, 15, (width / 10) * 7 + 15, height + 15);
            paint.Color = Color.LightGreen.ToSKColor();
            canvas.DrawRect(thirdRect, paint);

            SKRect fiveRect = new SKRect(((width / 10) * 9) - 25f + 15, 15, width + 15, height + 15);
            SKRoundRect fiveRRect = new SKRoundRect(fiveRect, 15f, 15f);
            paint.Color = Color.Salmon.ToSKColor();
            canvas.DrawRoundRect(fiveRRect, paint);

            SKRect fourRect = new SKRect((width / 10) * 7 + 15, 15, (width / 10) * 9 + 15, height + 15);
            paint.Color = Color.LightGoldenrodYellow.ToSKColor();
            canvas.DrawRect(fourRect, paint);

        }
    }
}