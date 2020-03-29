using System;
using Xamarin.Forms;

namespace ShoppingList.Models
{
    [Serializable]
    public class Item 
    {
        //public string Filename { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public double Price { get; set; }
        public string Store { get; set; }
        public bool IsSelected { get; set; }
        public TextDecorations Decoration
        {
            get
            {
                if (IsSelected == true)
                {
                    return TextDecorations.Strikethrough;
                }
                return TextDecorations.None;
            }
        }
        public Color TextColor
        {
            get
            {
                if (Store == "Bilka" || Store == "Føtex")
                {
                    return Color.Blue;
                }
                else if (Store == "Aldi")
                {
                    return Color.DodgerBlue;
                }
                else if (Store == "Brugsen" || Store == "Kvickly")
                {
                    return Color.Red;
                }
                else if (Store == "Fakta")
                {
                    return Color.DarkRed;
                }
                else if (Store == "Lidl")
                {
                    return Color.YellowGreen;
                }
                else if (Store == "Meny")
                {
                    return Color.OrangeRed;
                }
                else if (Store == "Netto")
                {
                    return Color.FromRgb(230, 192, 53);
                }
                else if (Store == "Rema 1000" || Store == "Jysk")
                {
                    return Color.DarkBlue;
                }
                else if (Store == "Jem & Fix" || Store == "XL")
                {
                    return Color.Black;
                }

                return Color.Gray;
            }
        }

        public Item(string key, string text, double price, string store, bool isselected)
        {
            //this.Filename = filename;
            this.Text = text;
            this.Price = price;
            this.Store = store;
            this.IsSelected = isselected;
            this.Key = key;
        }
        

    }
}
