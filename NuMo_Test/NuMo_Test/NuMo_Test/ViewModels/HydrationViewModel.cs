using System;
using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Forms;

namespace NuMo_Test.ViewModels
{
    public class HydrationViewModel : BaseViewModel
    {

        public HydrationViewModel()
        {
            Title = "Hydration Page";
        }

        public ICommand OpenWebCommand { get; }
    }
}