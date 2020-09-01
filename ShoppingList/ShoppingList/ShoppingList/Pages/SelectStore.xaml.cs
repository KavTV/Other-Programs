using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShoppingList.Models;
using System.IO;
using Xam.Plugin.SimpleColorPicker;

namespace ShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectStore : ContentPage
    {
        ShopList shopList;

        string itemText;
        double itemPrice;
        public SelectStore(string text, double price, ShopList shopList)
        {
            InitializeComponent();
            itemText = text;
            itemPrice = price;
            this.shopList = shopList;
        }
        protected override async void OnAppearing()
        {
            await UpdateList();
        }

        private async Task UpdateList()
        {
            if (App.Database.GetShopsAsync(1).Result == null)
            {
                PopulateShopDatabase();
            }
            var GetShops = await App.Database.GetShopsAsync();
            StoreList.ItemsSource = GetShops
                .OrderBy(d => d.Name)
                    .ToList();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                try
                {
                    ItemInformation iteminfo = ItemInformation.Instance();
                    var selectedStore = e.SelectedItem as Store;
                    Item item = new Item( itemText, 21, false, shopList.Name);
                    shopList.ItemList.Add(item);
                    iteminfo.Save(shopList);
                    //await Task.Run(() => iteminfo.AddItem(null, itemText, itemPrice, false, selectedStore));
                    //await Task.Run(() => iteminfo.Save());
                    await Navigation.PopToRootAsync();
                }

                catch (Exception)
                {
                    await DisplayAlert("Oprettelse af vare", "Der gik noget galt", "OK");
                }
            }
        }

        private void PopulateShopDatabase() // if shoplist is empty, add shops
        {
            AddStoreToDB("Aldi", Color.DodgerBlue);
            AddStoreToDB("Bilka", Color.Blue);
            AddStoreToDB("Brugsen", Color.Red);
            AddStoreToDB("Fakta", Color.DarkRed);
            AddStoreToDB("Føtex", Color.Blue);
            AddStoreToDB("Kvickly", Color.Red);
            AddStoreToDB("Lidl", Color.YellowGreen);
            AddStoreToDB("Meny", Color.Orange);
            AddStoreToDB("Netto", Color.FromRgb(230, 192, 53));
            AddStoreToDB("Rema 1000", Color.DarkBlue);
            AddStoreToDB("Jysk", Color.DarkBlue);
            AddStoreToDB("Jem & Fix", Color.Black);
            AddStoreToDB("XL", Color.Black);
            AddStoreToDB("Andet", Color.Gray);

        }

        private async void AddStoreToDB(string name, Color color)
        {
            Store store = new Store();
            store.Name = name;
            store.SetColor = color.ToHex();
            await App.Database.SaveShopAsync(store);
        }

        private async void TilføjButik_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Tilføj Butik", "Hvad skal navnet på den nye butik være?", "OK", "Cancel", "Meny", -1, Keyboard.Text);
            if (result == null)
            {
                return;
            }
            ColorDialogSettings ColorSettings = new ColorDialogSettings();
            ColorSettings.EditAlfa = false;
            var color = await ColorPickerDialog.Show(gMain, "Vælg Farve", Color.Blue, ColorSettings);

            if (result != "")
            {
                Store store = new Store();
                store.Name = result;
                store.SetColor = color.ToHex();
                await App.Database.SaveShopAsync(store);
                await UpdateList();
            }
        }

        private async void SletButik_Clicked(object sender, EventArgs e)
        {
            var menuitem = sender as MenuItem;
            var item = menuitem.BindingContext as Store;
            await App.Database.DeleteShopAsync(item);
            await UpdateList();
        }
    }
}