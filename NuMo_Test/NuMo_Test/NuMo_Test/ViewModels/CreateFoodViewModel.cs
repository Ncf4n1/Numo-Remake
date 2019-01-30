using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace NuMo_Test.ViewModels
{
    public class CreateFoodViewModel : BaseViewModel
    {
        public CreateFoodViewModel()
        {
            Title = "Create Food";
        }

        public ICommand OpenWebCommand { get; }
    }
}