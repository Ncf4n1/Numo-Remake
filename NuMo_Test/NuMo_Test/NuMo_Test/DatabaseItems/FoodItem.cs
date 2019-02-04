 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuMo_Test
{
    //The FoodItem class contains logic for organizing a list of individual nutrients. There should be no repeats in the list.
    class FoodItem
    {
        public List<Nutrient> nutrients;

        public FoodItem() {
        }

        //Create a consolodated FoodItem from a list of them.
        public FoodItem(List<FoodItem> foodItems)
        {
            nutrients = new List<Nutrient>();
            foreach(var foodItem in foodItems)
            {
                foreach(var nutrient in foodItem.nutrients)
                {
                    var findResult = nutrients.Find(i => i.name == nutrient.name);
                    if (findResult != null)
                    {
                        findResult.quantity += nutrient.quantity;
                    }
                    else
                    {
                        nutrients.Add(nutrient);
                    }
                }
            }
            //freshly calculate the omega6/3 ratio
            calculate63ratio(nutrients);
        }

        //Create a FoodItem from a series of food data, along with the appropriate multiplie to get to 100g units.
        public FoodItem(List<NutData> values, double multiplier)
        {
            nutrients = new List<Nutrient>();
            foreach (var value in values)
            {
                Nutrient nutrient = new Nutrient();
                nutrient.quantity = value.nutr_value*multiplier/100;
                nutrient.dbNo = value.nutr_no;
                switch (value.nutr_no)
                {
                    case 208:
                        nutrient.name = "Calories";
                        nutrient.quantifier = "kcal";
                        break;
                    case 203:
                        nutrient.name = "Protein(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 205:
                        nutrient.name = "Carbohydrates(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 269:
                        nutrient.name = "Total Sugars(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 291:
                        nutrient.name = "Total Dietary Fiber(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 301:
                        nutrient.name = "Calcium(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 303:
                        nutrient.name = "Iron(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 304:
                        nutrient.name = "Magnesium(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 305:
                        nutrient.name = "Phosphorus(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 306:
                        nutrient.name = "Potassium(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 307:
                        nutrient.name = "Sodium(mg)";
                        nutrient.quantifier = "g";
                        break;
                    case 309:
                        nutrient.name = "Zinc(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 312:
                        nutrient.name = "Copper(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 315:
                        nutrient.name = "Magnanese(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 317:
                        nutrient.name = "Selenium(µg)";
                        nutrient.quantifier = "µg";
                        break;
                    case 320:
                        nutrient.name = "Vitamin A RAE(µg)";
                        nutrient.quantifier = "µg";
                        break;
                    case 323:
                        nutrient.name = "Vitamin E(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 401:
                        nutrient.name = "Vitamin C(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 404:
                        nutrient.name = "Thiamin(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 405:
                        nutrient.name = "Riboflavin(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 406:
                        nutrient.name = "Niacin(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 410:
                        nutrient.name = "Pantothenic Acid(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 415:
                        nutrient.name = "Vitamin B6(mg)";
                        nutrient.quantifier = "mg";
                        break;
                    case 417:
                        nutrient.name = "Folate(µg)";
                        nutrient.quantifier = "µg";
                        break;
                    case 418:
                        nutrient.name = "Vitamin B12(µg)";
                        nutrient.quantifier = "µg";
                        break;
                    case 430:
                        nutrient.name = "Vitamin K(µg)";
                        nutrient.quantifier = "µg";
                        break;
                    case 618:
                        nutrient.name = "Omega 6 1(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 670:
                        nutrient.name = "Omega 6 2(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 672:
                        nutrient.name = "Omega 6 3(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 685:
                        nutrient.name = "Omega 6 4(g)";
                        nutrient.quantifier = "g";
                        nutrients.Add(nutrient);
                        nutrient = new Nutrient();
                        nutrient.name = "Omega 3 7(g)";
                        nutrient.quantifier = "g";
                        nutrient.quantity = value.nutr_value/100;
                        break;
                    case 620:
                        nutrient.name = "Omega 6 5(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 853:
                        nutrient.name = "Omega 6 6(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 619:
                        nutrient.name = "Omega 3 1(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 858:
                        nutrient.name = "Omega 3 2(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 852:
                        nutrient.name = "Omega 3 3(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 621:
                        nutrient.name = "Omega 3 4(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 629:
                        nutrient.name = "Omega 3 5(g)";
                        nutrient.quantifier = "g";
                        break;
                    case 631:
                        nutrient.name = "Omega 3 6(g)";
                        nutrient.quantifier = "g";
                        break;

                    default:
                        break;
                }
                if (nutrient.name != null)
                {
                    nutrients.Add(nutrient);
                }
            }
            //Special logic for totaling up omega 6 nutrient and omega 3 nutrient
            Nutrient omega6 = new Nutrient();
            omega6.name = "Omega 6 total(g)";
            omega6.quantifier = "g";
            omega6.quantity = 0;
            var omega6elements = nutrients.FindAll(i => i.name.Equals("Omega 6 1(g)") || i.name.Equals("Omega 6 2(g)") || i.name.Equals("Omega 6 3(g)") || i.name.Equals("Omega 6 4(g)") || i.name.Equals("Omega 6 5(g)") || i.name.Equals("Omega 6 6(g)"));
            foreach (var element in omega6elements)
            {
                omega6.quantity += element.quantity;
            }
            //This entry is subtracted from our total due to direction from Ed Dratz
            var omega6special = nutrients.Find(i => i.name.Equals("Omega 6 2(g)"));
            if (omega6special != null)
            {
                omega6.quantity -= 2 * omega6special.quantity;
            }
            if(omega6.quantity > 0)
                nutrients.Add(omega6);
            Nutrient omega3 = new Nutrient();
            omega3.name = "Omega 3 total(g)";
            omega3.quantifier = "g";
            omega3.quantity = 0;
            var omega3elements = nutrients.FindAll(i => i.name.Equals("Omega 3 1(g)") || i.name.Equals("Omega 3 2(g)") || i.name.Equals("Omega 3 3(g)") || i.name.Equals("Omega 3 4(g)") || i.name.Equals("Omega 3 5(g)") || i.name.Equals("Omega 3 6(g)") || i.name.Equals("Omega 3 7(g)"));
            foreach(var element in omega3elements)
            {
                omega3.quantity += element.quantity;
            }
            //This entry is subtracted from our total due to direction from Ed Dratz
            var omega3special = nutrients.Find(i => i.name.Equals("Omega 3 7(g)"));
            if(omega3special != null)
            {
                omega3.quantity -= 2 * omega3special.quantity;
            }
            if (omega3.quantity > 0)
                nutrients.Add(omega3);
            calculate63ratio(nutrients);
            
            Nutrient netCarbs = new Nutrient();
            netCarbs.name = "Net Carbohydrates(g)";
            netCarbs.quantifier = "g";
            var totalCarbs = nutrients.Find(i => i.name.Equals("Carbs By Diff(g)"));
            if (totalCarbs != null)
            {
                netCarbs.quantity = totalCarbs.quantity;
                var dietaryFiber = nutrients.Find(i => i.name.Equals("Total Dietary Fiber(g)"));
                if(dietaryFiber != null)
                {
                    netCarbs.quantity -= dietaryFiber.quantity;
                }
                nutrients.Add(netCarbs);
            }
        }

        //Calculates total O6/total O3 if applicable
        public static void calculate63ratio(List<Nutrient> nutrients)
        {
            var omega3 = nutrients.Find(i => i.name.Equals("Omega 3 total(g)"));
            var omega6 = nutrients.Find(i => i.name.Equals("Omega 6 total(g)"));
            if ((omega3 != null) && omega3.quantity > 0)
            {
                Nutrient omega63ratio = new NuMo_Test.Nutrient();
                omega63ratio.quantity = 0;
                omega63ratio.name = "Omega6/3 Ratio";
                omega63ratio.quantifier = "special";
                if(omega6 != null)
                    omega63ratio.quantity = omega6.quantity / omega3.quantity;
                nutrients.RemoveAll(i => i.name.Equals("Omega6/3 Ratio"));
                nutrients.Add(omega63ratio);
            }
        }

        //removes excessive omega entries, we assume users only want totals and ratio.
        public void stripExtraOmegs()
        {
            nutrients.RemoveAll(i => i.name.Contains("Omega ") && !(i.name.Equals("Omega 3 total(g)") || i.name.Equals("Omega 6 total(g)")));
        }

    }
}
