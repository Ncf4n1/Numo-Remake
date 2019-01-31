using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace NuMo_Test.ViewModels
{
    public class AddFoodViewModel : BaseViewModel
    {
        public AddFoodViewModel()
        {
            Title = "Add Food";
        }

        public ICommand OpenWebCommand { get; }
    }
}
