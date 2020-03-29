using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ShoppingList.Models
{
    class Store
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public Store(string name, Color color)
        {
            this.Name = name;
            this.Color = color;
        }
    }
}
