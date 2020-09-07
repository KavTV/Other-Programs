using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShoppingList.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Essentials;
using ShoppingList.Pages;

namespace ShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopListPage : ContentPage
    {
        ItemInformation iteminfo = ItemInformation.Instance();
        ObservableCollection<ShopList> shopList { get; set; }

        public ShopListPage()
        {
            InitializeComponent();
            shopList = new ObservableCollection<ShopList>();
            listView.ItemsSource = shopList;

        }
        protected override void OnAppearing()
        {

            UpdateList();

        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            ShopList selectedList = e.SelectedItem as ShopList;
            await Navigation.PushAsync(new ItemsPage(selectedList),true);
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            var menuitem = sender as MenuItem;
            var shop = menuitem.BindingContext as ShopList;
            iteminfo.DeleteShopList(shop);
            UpdateList();
        }

        /// <summary>
        /// Asks user to make a new list
        /// </summary>
        private async void OnAddItemClicked(object sender, EventArgs e)
        {
            var result = await DisplayPromptAsync("Ny Liste", "Hvad vil du kalde din liste?", "Add", "Cancel", "Butik", -1, Keyboard.Text,"");
            if (result != null)
            {
                foreach (var item in iteminfo.GetShopList())
                {
                    if (item.Name == result)
                    {
                        await DisplayAlert("Fejl", "Dette navn findes allerede","Cancel");
                        return;
                    }
                }
                ShopList newShopList = new ShopList(result);
                iteminfo.Save(newShopList);
                UpdateList();
            }
        }

        private void UpdateList()
        {
            shopList.Clear();
            foreach (var shop in iteminfo.GetShopList())
            {
                shopList.Add(shop);
            }


        }

        private async void AllLists_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllItemsPage());
        }
    }
}