using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

namespace ShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateItemPage : ContentPage
    {
        public CreateItemPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await Task.Run(() => ItemName.Focus());
        }

        private async void OnContinueBtnClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ItemName.Text))
            {
                await Navigation.PushAsync(new SelectPricePage(ItemName.Text));
            }
            else
            {
                await DisplayAlert("Vare", "Du har ikke givet din vare et navn", "OK");
            }
        }
    }
}