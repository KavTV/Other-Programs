using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace ShoppingList.Models
{
    [Serializable]
    public class ShopList
    {
        public string Name { get; set; }
        public ObservableCollection<Item> ItemList { get { return itemList; } set { itemList = value; } }
        private ObservableCollection<Item> itemList;

        public ShopList(string name)
        {
            this.Name = name;
            itemList = new ObservableCollection<Item>();
        }

        


    }
}
