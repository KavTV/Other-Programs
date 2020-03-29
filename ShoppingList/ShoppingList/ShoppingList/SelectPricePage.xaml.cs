using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectPricePage : ContentPage
    {
        string itemText;
        public SelectPricePage(string text)
        {
            InitializeComponent();
            itemText = text;
        }

        protected override async void OnAppearing()
        {
            await Task.Run(() => ItemPrice.Focus());
        }
        private async void OnContinueBtnClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ItemPrice.Text))
            {
                try
                {
                    var d = Convert.ToDecimal(ItemPrice.Text, new CultureInfo("en-US"));
                    await Navigation.PushAsync(new SelectStore(itemText, double.Parse(d.ToString())));
                }
                catch (Exception)
                {
                    await DisplayAlert("Hov", "Der skete en fejl", "OK");
                }
            }
            else
            {
                await DisplayAlert("Vare", "Du har ikke givet din vare en pris", "OK");
            }
        }

    }
}