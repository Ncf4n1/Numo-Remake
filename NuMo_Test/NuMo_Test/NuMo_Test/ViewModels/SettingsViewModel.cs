using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace NuMo_Test.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            Title = "Settings";
        }

        public ICommand OpenWebCommand { get; }
    }
}