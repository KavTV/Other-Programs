using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShoppingList.Models;
using System.IO;

namespace ShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectStore : ContentPage
    {
        string itemText;
        double itemPrice;
        public SelectStore(string text, double price)
        {
            InitializeComponent();
            itemText = text;
            itemPrice = price;

            
        }
        protected override void OnAppearing()
        {
            var StoreNames = new List<Store>();
            StoreNames.Add(new Store("Aldi", Color.DodgerBlue));
            StoreNames.Add(new Store("Bilka", Color.Blue));
            StoreNames.Add(new Store("Brugsen", Color.Red));
            StoreNames.Add(new Store("Fakta", Color.DarkRed));
            StoreNames.Add(new Store("Føtex", Color.Blue));
            StoreNames.Add(new Store("Kvickly", Color.Red));
            StoreNames.Add(new Store("Lidl", Color.YellowGreen));
            StoreNames.Add(new Store("Meny", Color.OrangeRed));
            StoreNames.Add(new Store("Netto", Color.FromRgb(230, 192, 53)));
            StoreNames.Add(new Store("Rema 1000", Color.DarkBlue));
            StoreNames.Add(new Store("Jysk", Color.DarkBlue));
            StoreNames.Add(new Store("Jem & Fix", Color.Black));
            StoreNames.Add(new Store("XL", Color.Black));
            StoreNames.Add(new Store("Andet", Color.Gray));
            StoreList.ItemsSource = StoreNames
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
                    var label = e.SelectedItem as Store;
                    await Task.Run(()=> iteminfo.AddItem(null, itemText, itemPrice, label.Name, false));
                    await Task.Run(() => iteminfo.Save());
                    await Navigation.PopToRootAsync();
                }

                catch (Exception)
                {
                    await DisplayAlert("Oprettelse af vare", "Der gik noget galt", "OK");
                }
            }
        }
    }
}