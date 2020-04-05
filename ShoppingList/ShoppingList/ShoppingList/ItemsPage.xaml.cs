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
            var result = await DisplayPromptAsync("Ændre Navn", "Hvad vil du ændre navnet på varen til?","OK","Cancel","Mælk",-1,Keyboard.Text);
            if (result != "")
            {
                item.Text = result;
                iteminfo.AddItem(item.Key, item.Text, item.Price, item.IsSelected, item.Store);
                iteminfo.Save();
                UpdateList();
            }
        }
        private async void EditPrice_Clicked(object sender, EventArgs e) //Not done
        {
            var menuitem = sender as MenuItem;
            var item = menuitem.BindingContext as Item;

        }
        private async void EditStore_Clicked(object sender, EventArgs e)//Not done
        {
            var menuitem = sender as MenuItem;
            var item = menuitem.BindingContext as Item;

        }
    }
}