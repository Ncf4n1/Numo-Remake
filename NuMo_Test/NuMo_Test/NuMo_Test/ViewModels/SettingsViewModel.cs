using System;

using NuMo_Test.Models;

namespace NuMo_Test.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public SettingsViewModel(Item item = null)
        {
            Title = "Settings";
            Item = item;
        }
    }
}