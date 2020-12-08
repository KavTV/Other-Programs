using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using ShoppingList.Models;
using dotMorten.Xamarin.Forms;
using System.Net.Http.Headers;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace ShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateItemPage : ContentPage
    {
        ShopList shopList;

        string SUGGESTION_FILENAME = Path.Combine(App.FolderPath, "Suggestions.txt");
        
        public CreateItemPage(ShopList shopList)
        {
            InitializeComponent();
            this.shopList = shopList;
            //Create a new file
            if (!File.Exists(SUGGESTION_FILENAME))
            {
                FileStream fs = new FileStream(SUGGESTION_FILENAME, FileMode.Create);
                fs.Close();
            }
            ItemName.Focus();
            
        }

        protected override void OnAppearing()
        {
            
            ItemName.Focus();
            
            base.OnAppearing();
        }

        private async void OnContinueBtnClicked(object sender, EventArgs e)
        {
            await NextPage();
        }

        private async Task NextPage()
        {
            if (!string.IsNullOrWhiteSpace(ItemName.Text))
            {
                char[] itemText = ItemName.Text.Trim().ToCharArray();
                itemText[0] = char.ToUpper(itemText[0]);
                string itemNameText = new string(itemText);

                SaveToSuggestions(itemNameText);

                await Navigation.PushAsync(new SelectPricePage(itemNameText, shopList));
            }
            else
            {
                await DisplayAlert("Vare", "Du har ikke givet din ting et navn", "OK");
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;
                if (!string.IsNullOrWhiteSpace(sender.Text))
                {
                    try
                    {

                        var dataset = GetSuggestionList().Where(x => x.ToLower().Contains(sender.Text.ToLower()));
                        sender.ItemsSource = dataset.ToList();
                    }
                    catch (Exception)
                    {

                        //throw;
                    }
                }
                else
                {
                    sender.ItemsSource = null;
                }

            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            // Set sender.Text. You can use e.SelectedItem to build your text string.
            string itemName = e.SelectedItem as string;
            sender.Text = itemName;
        }
        private async void AutoSuggestBox_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
        {

            if (e.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                await NextPage();
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
                await NextPage();
            }
        }
        /// <summary>
        /// Get all the suggestions from the txt file and load to List
        /// </summary>
        /// <returns></returns>
        private List<string> GetSuggestionList()
        {

            try
            {
                List<string> suggestionList = new List<string>();

                StreamReader sr = new StreamReader(SUGGESTION_FILENAME);

                while (!sr.EndOfStream)
                {
                    suggestionList.Add(sr.ReadLine());
                }

                sr.Close();

                return suggestionList;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);

            }
            return null;
        }

        private void SaveToSuggestions(string suggestion)
        {

            List<string> suggestionList = GetSuggestionList();

            //If the suggestion is already in the file, dont do anything
            bool doesSuggestionExist = false;
            if (suggestionList != null)
            {

                foreach (var item in suggestionList.ToList())
                {
                    if (item.ToLower() == suggestion.ToLower())
                    {
                        doesSuggestionExist = true;
                    }
                }
            }
            StreamWriter sw = new StreamWriter(SUGGESTION_FILENAME, true);
            if (!doesSuggestionExist)
            {
                sw.WriteLine(suggestion);
            }
            sw.Close();
           

        }

    }
}