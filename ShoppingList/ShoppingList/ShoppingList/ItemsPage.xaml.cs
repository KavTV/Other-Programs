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

            UpdateListAsync();

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
                    iteminfo.AddItem(selectedItem.Key, selectedItem.Text, selectedItem.Price, selectedItem.Store, selectedItem.IsSelected);
                    iteminfo.Save();
                    UpdateListAsync();
                }
                else
                {
                    selectedItem.IsSelected = false;
                    iteminfo.AddItem(selectedItem.Key, selectedItem.Text, selectedItem.Price, selectedItem.Store, selectedItem.IsSelected);
                    iteminfo.Save();
                    UpdateListAsync();
                }

            }
        }

        private void UpdateListAsync()
        {

            iteminfo.Load();

            var currentItems = iteminfo.GetItems();

            if (currentItems != null)
            {
                listView.ItemsSource = currentItems
                    .OrderBy(d => d.Store)
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
                UpdateListAsync();
            }
        }

        private void ToolbarRemove_Clicked(object sender, EventArgs e)
        {
            RemoveSelectedItems();
        }
    }
}