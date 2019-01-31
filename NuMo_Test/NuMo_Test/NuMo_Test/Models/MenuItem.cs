using System;
using System.Collections.Generic;
using System.Text;

namespace NuMo_Test.Models
{
    public enum MenuItemType
    {
        Home,
        CreateRecipe,
        CreateFood,
        ItemDetail,
        Settings
    }
    public class MenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
