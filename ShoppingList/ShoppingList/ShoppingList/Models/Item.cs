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
        public string StoreName { get { return Store.Name; } }
        public bool IsSelected { get; set; }
        public Store Store { get; set; }
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
                return Store.GetColor;
            }
        }

        public Item(string key, string text, double price, bool isselected, Store store)
        {
            //this.Filename = filename;
            this.Text = text;
            this.Price = price;
            this.IsSelected = isselected;
            this.Key = key;
            this.Store = store;
        }
        

    }
}
