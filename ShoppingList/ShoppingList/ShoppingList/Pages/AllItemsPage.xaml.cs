using ShoppingList.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllItemsPage : ContentPage
    {
        ObservableCollection<ShopList> shopLists { get; set; }
        public ObservableCollection<Item> itemList { get; set; } = new ObservableCollection<Item>();


        ItemInformation iteminfo = ItemInformation.Instance();

        string sortingMethod = "";
        public AllItemsPage()
        {
            InitializeComponent();
            MyListView.ItemsSource = itemList;
        }

        protected override void OnAppearing()
        {
            UpdateList();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var selectedItem = e.Item as Item;
            //Make the item selected
            foreach (var shop in shopLists)
            {

                foreach (var item in shop.ItemList)
                {
                    if (item == selectedItem)
                    {
                        if (item.IsSelected == false)
                        {
                            item.IsSelected = true;

                        }
                        else
                        {
                            item.IsSelected = false;

                        }

                        iteminfo.Save(shop);
                        UpdateList();

                        //Deselect Item
                        ((ListView)sender).SelectedItem = null;
                        return;
                    }
                }

            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        void UpdateList()
        {

            itemList.Clear();
            shopLists = iteminfo.GetAllShops();
            foreach (var shop in shopLists)
            {
                foreach (var item in shop.ItemList)
                {
                    itemList.Add(item);
                }
            }
            //Sorting method
            switch (sortingMethod)
            {
                case "SortByCheapest":
                    ShowCheapestItems();
                    break;
                case "SortSameItem":
                    SortSameItem();
                    break;
                case "SortPriceLowHigh":
                    SortPriceLowHigh();
                    break;
                case "SortPriceHighLow":
                    SortPriceHighLow();
                    break;
                default:
                    MyListView.ItemsSource = itemList.OrderBy(d => d.Text).ThenBy(d => d.FromList);
                    break;
            }


        }

        void ShowCheapestItems()
        {
            //Compares the items from the same list and if 2 items got the same name, the most expensive is removed
            //Also if the item is selected remove it from the list
            foreach (var item in itemList.ToList())
            {
                if (item.IsSelected)
                {
                    itemList.Remove(item);
                }
                foreach (var thing in itemList.ToList())
                {
                    if (item.Text.ToLower() == thing.Text.ToLower())
                    {

                        if (!item.IsSelected && !thing.IsSelected)
                        {


                            //Delete the item with highest price
                            if (item.Price > thing.Price)
                            {
                                itemList.Remove(item);
                            }
                            else if (item.Price < thing.Price)
                            {
                                itemList.Remove(thing);
                            }
                        }
                    }
                }
            }
            MyListView.ItemsSource = itemList.OrderBy(d => d.Text).ThenBy(d => d.FromList);
        }

        void SortSameItem()
        {
            MyListView.ItemsSource = itemList.OrderBy(d => d.Text).ThenBy(d => d.Price);
        }

        void SortPriceLowHigh()
        {
            MyListView.ItemsSource = itemList.OrderBy(d => d.Price);
        }
        void SortPriceHighLow()
        {
            MyListView.ItemsSource = itemList.OrderByDescending(d => d.Price);
        }

        private async void Sort_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Hvilket filter vil du bruge?", "Cancel", null, "Sorter efter butik", "Vis kun billigste varer", "Samme varer", "Pris Lav/Høj", "Pris Høj/Lav");
            switch (action)
            {
                case "Sorter efter butik":
                    sortingMethod = "";
                    break;
                case "Vis kun billigste varer":
                    sortingMethod = "SortByCheapest";
                    break;
                case "Samme varer":
                    sortingMethod = "SortSameItem";
                    break;
                case "Pris Lav/Høj":
                    sortingMethod = "SortPriceLowHigh";
                    break;
                case "Pris Høj/Lav":
                    sortingMethod = "SortPriceHighLow";
                    break;
                default:
                    break;
            }
            UpdateList();
            
        }

        private void ToolbarRemove_Clicked(object sender, EventArgs e)
        {
            //Deletes all the marked items
            foreach (var shop in shopLists.ToList())
            {
                bool isSomethingDeleted = false;
                foreach (var item in shop.ItemList.ToList())
                {
                    if (item.IsSelected)
                    {
                        shop.ItemList.Remove(item);
                        isSomethingDeleted = true;
                    }
                }
                if (isSomethingDeleted)
                {
                    iteminfo.Save(shop);
                }
            }
            UpdateList();

        }
    }
}
