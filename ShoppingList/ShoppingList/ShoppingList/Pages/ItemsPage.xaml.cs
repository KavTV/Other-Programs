using System;
using System.Linq;
using Xamarin.Forms;
using ShoppingList.Models;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

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
            //Get the UnSerialized ShopList
            shopList = iteminfo.GetShop(localShopList);
            //Apply it to the listview
            listView.ItemsSource = shopList.ItemList.OrderBy(d => d.Text);
            //Change the page name to the shoplist name
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
                iteminfo.ChangeIsSelected(selectedItem);
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

        private async void ToolbarRemove_Clicked(object sender, EventArgs e)
        {
            try
            {
                var duration = TimeSpan.FromMilliseconds(50);
                Vibration.Vibrate(duration);
            }
            catch (Exception)
            {

            }
            var answer = await DisplayAlert("Slet markerede ting?", "Er du sikker på du vil slette markerede ting på din liste?", "Ja", "Nej");
            if (answer)
            {
                iteminfo.RemoveSelectedItems(shopList);
                UpdateList();
            }
        }

        private async void ToolbarDeleteAll_Clicked(object sender, EventArgs e)
        {
            try
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(50));
            }
            catch (Exception){}

            var answer = await DisplayAlert("Slet alt?", "Er du sikker på du vil slette alt på din liste?", "Ja", "Nej");
            if (answer)
            {

                if (shopList.ItemList != null)
                {
                    iteminfo.RemoveAllItems(shopList);
                    iteminfo.Save(shopList);
                    UpdateList();
                }
            }
        }

        private async void EditName_Clicked(object sender, EventArgs e)
        {

            var menuitem = sender as MenuItem;
            var item = menuitem.BindingContext as Item;
            var result = await DisplayPromptAsync("Ændre Navn", $"Hvad vil du ændre navnet på {item.Text} til?", "OK", "Cancel", $"{item.Text}", -1, Keyboard.Text,$"{item.Text}");
            if (result != null)
            {
                item.Text = result;
                
                iteminfo.Save(shopList);
                UpdateList();
            }
        }

        private async void EditPrice_Clicked(object sender, EventArgs e) // Edits the selected items price
        {

            var menuitem = sender as MenuItem;
            var item = menuitem.BindingContext as Item;
            var result = await DisplayPromptAsync("Ændre Pris", $"Hvad vil du ændre prisen på {item.Text} til?", "OK", "Cancel", $"{item.Price}", -1, Keyboard.Numeric, $"{item.Price}");
            if (result != null)
            {
                item.Price = Int32.Parse(result);
                
                iteminfo.Save(shopList);
                UpdateList();
            }
        }



    }
}