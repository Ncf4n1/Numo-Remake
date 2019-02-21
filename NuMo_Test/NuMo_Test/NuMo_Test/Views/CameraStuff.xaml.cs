using NuMo_Test.ItemViews;
using Plugin.Media;
using Plugin.Media.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


/* Camera page for uploading or taking pictures to save as reminders
 */

namespace NuMo_Test.Views
{
    public partial class CameraStuff : ContentPage
    {
        Image saveImage = new Image();
        String date;
        public CameraStuff(String date)
        {
            // Initialize the toolbar with save button, date, and title
            this.date = date;
            ToolbarItem save = new ToolbarItem();
            save.Text = "Save";
            save.Clicked += saveButtonClicked;
            ToolbarItems.Add(save);
            InitializeComponent();
            this.Title = "Upload Reminder Photo";

        }
        private async void TakePhotoButton_OnClicked(object sender, EventArgs e)
        {
            // If the take picure button is clicked, check if a camera is supported and use it if so
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                });

            if (file == null)
                return;

            // Display photo on page for user to see
            saveImage.Source = ImageSource.FromStream(() => file.GetStream());
            MainImage.Source = saveImage.Source;
        }

        private async void PickPhotoButton_OnClicked(object sender, EventArgs e)
        {
            // If the pick picure button is clicked, check if there is a photo album and access it if so
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Oops", "Pick photo is not supported!", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            // Display photo on page for user to see
            saveImage.Source = ImageSource.FromStream(() => file.GetStream());
            MainImage.Source = saveImage.Source;
        }

        //NOT NEEDED AFTER MARSHMELLO ANDROID
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        //{
        //    Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        private async void saveButtonClicked(object sender, EventArgs args)
        {
            // Upon clicking save button, add photo to the data base as a reminder and add it as a display item on mydaypage
            if (saveImage != null)
            {
                var db = DataAccessor.getDataAccessor();
                var remainder = new MyDayRemainderItem();
                remainder.Date = this.date;
                remainder.RemainderImage = saveImage;
                db.insertRemainder(remainder);
            }
            // Refresh the page
            MessagingCenter.Send(new MyDayFoodItem(), "RefreshMyDay");
            await Navigation.PopAsync();
        }
    }
}
