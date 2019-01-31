using System;

using NuMo_Test.Models;

namespace NuMo_Test.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = "Item Detail Page";
            //Title = item?.Text;
            Item = item;
        }
    }
}
