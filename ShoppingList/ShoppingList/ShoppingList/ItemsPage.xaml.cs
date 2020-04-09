using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using ShoppingList.Models;
using System.Threading.Tasks;

namespace ShoppingList
{
    public partial class ItemsPage : ContentPage
    {

        ItemInformation iteminfo = ItemInformation.Instance();

        public ItemsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateList();

        }

        async void OnItemAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateItemPage());
        }

        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e) //add to list 
        {

            if (e.SelectedItem != null)
            {

                var selectedItem = e.SelectedItem as Item;
                if (selectedItem.IsSelected == false) // If item is not selected, make it selected and save it.
                {
                    selectedItem.IsSelected = true;
                    iteminfo.AddItem(selectedItem.Key, selectedItem.Text, selectedItem.Price, selectedItem.IsSelected, selectedItem.Store);
                    iteminfo.Save();
                    UpdateList();
                }
                else
                {
                    selectedItem.IsSelected = false;
                    iteminfo.AddItem(selectedItem.Key, selectedItem.Text, selectedItem.Price, selectedItem.IsSelected, selectedItem.Store);
                    iteminfo.Save();
                    UpdateList();
                }

            }
        }

        private void UpdateList()
        {

            iteminfo.Load();

            var currentItems = iteminfo.GetItems();

            if (currentItems != null)
            {
                listView.ItemsSource = currentItems
                    .OrderBy(d => d.StoreName)
                    .ToList();
            }
            else
            {
                listView.ItemsSource = null;
            }

        }

        private void RemoveSelectedItems()
        {
            var currentItems = iteminfo.GetItems();
            if (currentItems != null)
            {
                foreach (var item in currentItems)
                {
                    if (item.IsSelected == true)
                    {
                        iteminfo.RemoveItem(item.Key);
                    }
                }
                iteminfo.Save();
                UpdateList();
            }
        }

        private void RemoveAllItems()
        {
            var currentItems = iteminfo.GetItems();
            if (currentItems != null)
            {
                foreach (var item in currentItems)
                {
                    iteminfo.RemoveItem(item.Key);
                }
                iteminfo.Save();
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
                iteminfo.AddItem(item.Key, item.Text, item.Price, item.IsSelected, item.Store);
                iteminfo.Save();
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
                iteminfo.AddItem(item.Key, item.Text, item.Price, item.IsSelected, item.Store);
                iteminfo.Save();
                UpdateList();
            }
        }

        private async void EditStore_Clicked(object sender, EventArgs e)// Edits the selected items Store
        {
            var menuitem = sender as MenuItem; //Get item selected
            var item = menuitem.BindingContext as Item;//Convert to Item

            var getShops = await App.Database.GetShopsAsync(); // Get the shops to select from
            string[] storelist = new string[getShops.Count]; // Make array to put shop labels into.
            int i = 0;
            foreach (var thing in getShops) //Make an array of store names
            {
                storelist[i] = thing.Name;
                i++;
            }

            string result = await DisplayActionSheet("Ændre Butik", "Cancel", null, storelist); // Promt to select store
            if (result != "Cancel" && result != "")//If a store is selected, find the item and update item.
            {
                foreach (var thing in getShops)
                {
                    if (thing.Name == result)
                    {
                        item.Store = thing;
                        iteminfo.AddItem(item.Key, item.Text, item.Price, item.IsSelected, item.Store);
                        iteminfo.Save();
                        UpdateList();
                        return;
                    }
                }



            }
        }
    }
}