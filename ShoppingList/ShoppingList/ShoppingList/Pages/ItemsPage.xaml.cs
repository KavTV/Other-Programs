using System;
using System.Linq;
using Xamarin.Forms;
using ShoppingList.Models;
using Xamarin.Forms.Xaml;

namespace ShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ShopList shopList;
        ItemInformation iteminfo = ItemInformation.Instance();

        public ItemsPage(ShopList localShopList)
        {
            InitializeComponent();
            shopList = iteminfo.GetShop(localShopList);
            
            listView.ItemsSource = shopList.ItemList.OrderBy(d => d.Text);
            this.Title = shopList.Name;
        }
        

        protected override void OnAppearing()
        {
            UpdateList();

        }

        async void OnItemAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateItemPage(shopList));
        }

        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e) //add to list 
        {

            if (e.SelectedItem != null)
            {

                var selectedItem = e.SelectedItem as Item;
                if (selectedItem.IsSelected == false) // If item is not selected, make it selected and save it.
                {
                    selectedItem.IsSelected = true;
                    //iteminfo.AddItem(selectedItem.Key, selectedItem.Text, selectedItem.Price, selectedItem.IsSelected, selectedItem.Store);
                }
                else // else make it unselected
                {
                    selectedItem.IsSelected = false;
                    //iteminfo.AddItem(selectedItem.Key, selectedItem.Text, selectedItem.Price, selectedItem.IsSelected, selectedItem.Store);
                    
                }
                iteminfo.Save(shopList);
                UpdateList();
            }
        }

        private void UpdateList()
        {

            shopList.ItemList.Clear();
            foreach (var shop in iteminfo.GetShop(shopList).ItemList.ToList())
            {
                shopList.ItemList.Add(shop);
            }
            if (shopList.ItemList != null)
            {
                listView.ItemsSource = shopList.ItemList
                    .OrderBy(d => d.Text);
            }
            else
            {
                //listView.ItemsSource = null;
            }

        }

        private void RemoveSelectedItems()
        {
            
            if (shopList.ItemList != null)
            {
                foreach (var item in shopList.ItemList.ToList())
                {
                    if (item.IsSelected == true)
                    {
                        shopList.ItemList.Remove(item);
                    }
                }
                iteminfo.Save(shopList);
                UpdateList();
            }
        }

        private void RemoveAllItems()
        {
            
            if (shopList.ItemList != null)
            {
                foreach (var item in shopList.ItemList.ToList())
                {
                    shopList.ItemList.Remove(item);
                }
                iteminfo.Save(shopList);
                UpdateList();
            }
        }
        
        private void ToolbarRemove_Clicked(object sender, EventArgs e)
        {
            RemoveSelectedItems();
        }

        private async void ToolbarDeleteAll_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Slet alt?", "Er du sikker på du vil slette alt på din liste?", "Ja", "Nej");
            if (answer)
            {
                RemoveAllItems();
            }
        }

        private async void EditName_Clicked(object sender, EventArgs e)
        {

            var menuitem = sender as MenuItem;
            var item = menuitem.BindingContext as Item;
            var result = await DisplayPromptAsync("Ændre Navn", $"Hvad vil du ændre navnet på {item.Text} til?", "OK", "Cancel", $"{item.Text}", -1, Keyboard.Text);
            if (result != null)
            {
                item.Text = result;
                //iteminfo.AddItem(item.Key, item.Text, item.Price, item.IsSelected, item.Store);
                iteminfo.Save(shopList);
                UpdateList();
            }
        }

        private async void EditPrice_Clicked(object sender, EventArgs e) // Edits the selected items price
        {
            
            var menuitem = sender as MenuItem;
            var item = menuitem.BindingContext as Item;
            var result = await DisplayPromptAsync("Ændre Pris", $"Hvad vil du ændre prisen på {item.Text} til?", "OK", "Cancel", $"{item.Price}", -1, Keyboard.Numeric);
            if (result != null)
            {
                item.Price = Int32.Parse(result);
                //iteminfo.AddItem(item.Key, item.Text, item.Price, item.IsSelected, item.Store);
                iteminfo.Save(shopList);
                UpdateList();
            }
        }



    }
}