using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace NuMo_Test.ViewModels
{
    public class DRIViewModel : BaseViewModel
    {
        public DRIViewModel()
        {
            Title = "Dietary Reference Intake";
        }

        public ICommand OpenWebCommand { get; }
    }
}