using System;
using System.IO;
using Xamarin.Forms;
using ShoppingList.Data;
using ShoppingList.Models;
using ShoppingList.Pages;

namespace ShoppingList
{
    public partial class App : Application
    {
        public static string FolderPath { get; private set; }
        
        private static ShopDatabase database;

        public static ShopDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ShopDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            
            MainPage = new NavigationPage(new ShopListPage());
            
        }
        // ...
    }
}