using System;
using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Forms;

namespace NuMo_Test.ViewModels
{
    public class AddFoodViewModel : BaseViewModel
    {
        List<NumoNameSearch> searchResults { get; set; }

        public AddFoodViewModel()
        {
            Title = "Add Food";
        }

        public ICommand OpenWebCommand { get; }
    }
}
