using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace ShoppingList.Models
{
    class ItemInformation
    {
        private static ItemInformation itemInformation;
        private string DATA_FILENAME = Path.Combine(App.FolderPath, "ShopLists/"); // default path
        private BinaryFormatter formatter;
        private Dictionary<string, Item> itemDictionary;



        public static ItemInformation Instance()
        {
            if (itemInformation == null)
            {
                itemInformation = new ItemInformation();
            }

            return itemInformation;
        }

        private ItemInformation()
        {
            this.itemDictionary = new Dictionary<string, Item>();
            this.formatter = new BinaryFormatter();
        }


        public void DeleteShopList(ShopList shopList)
        {
            DATA_FILENAME = Path.Combine(App.FolderPath, "ShopLists/", $".{shopList.Name}.txt");
            try
            {
                //Delete the file
                File.Delete(DATA_FILENAME);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
            }
        } // end public bool RemoveFriend(string name)

        /// <summary>
        /// Save and serialize the ShopList object
        /// </summary>
        /// <param name="shopList"></param>
        public void Save(ShopList shopList)
        {
            DATA_FILENAME = Path.Combine(App.FolderPath, "ShopLists/", $".{shopList.Name}.txt");

            try
            {
                // Create a FileStream that will write data to file.
                FileStream writerFileStream =
                    new FileStream(DATA_FILENAME, FileMode.Create, FileAccess.Write);
                // Save our dictionary of items to file
                this.formatter.Serialize(writerFileStream, shopList);

                // Close the writerFileStream when we are done.
                writerFileStream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to save item");
            } // end try-catch
        } // end public bool Load()

        public ShopList GetShop(ShopList shopList)
        {
            DATA_FILENAME = Path.Combine(App.FolderPath, "ShopLists/", $".{shopList.Name}.txt");
            // Check if we had previously Save information of items
            // previously
            if (File.Exists(DATA_FILENAME))
            {

                try
                {
                    // Create a FileStream that will gain read access to the data file.
                    FileStream readerFileStream = new FileStream(DATA_FILENAME,
                    FileMode.Open, FileAccess.Read);
                    // Reconstruct information of our item from file.
                    

                    ShopList shop = (ShopList)this.formatter.Deserialize(readerFileStream);

                    // Close the readerFileStream when we are done
                    readerFileStream.Close();
                    return shop;

                }
                catch (Exception)
                {
                    Console.WriteLine("There seems to be a file that contains " +
                        "item information but somehow there is a problem " +
                        "with reading it.");
                } // end try-catch

            } // end if
            return null;
        } // end public GetShop()

        public ObservableCollection<ShopList> GetShopList()
        {
            DATA_FILENAME = Path.Combine(App.FolderPath, "ShopLists/");
            try
            {

                if (!Directory.Exists(DATA_FILENAME))
                {
                    Directory.CreateDirectory(DATA_FILENAME);
                }
                ObservableCollection<ShopList> shopList = new ObservableCollection<ShopList>();
                //Finds every file in the ShopLists folder
                foreach (var file in Directory.GetFiles(DATA_FILENAME))
                {
                    Console.WriteLine(file);
                    string[] fileSplit = file.Split('.');
                    string fileName = "";
                    for (int i = 4; i < fileSplit.Length - 1; i++)
                    {
                        if (i > 4)
                        {
                            fileName += ".";
                        }
                        fileName += fileSplit[i];
                    }
                    shopList.Add(new ShopList(Path.GetFileName(fileName)));
                }
                return shopList;
            }
            catch (Exception)
            {
                Console.WriteLine("Could not find any shops");
            }
            return null;
        }


        /// <summary>
        /// Gets all the shop lists made
        /// </summary>
        /// <returns>List with deserialized ShopList objects</returns>
        public ObservableCollection<ShopList> GetAllShops()
        {
            ObservableCollection<ShopList> shopLists = GetShopList();
            ObservableCollection<ShopList> newShopList = new ObservableCollection<ShopList>();
            foreach (var shop in shopLists)
            {
                newShopList.Add(GetShop(shop));
            }
            return newShopList;
        }

        /// <summary>
        /// This selects the object if not selected, and deselects it if selected
        /// </summary>
        /// <param name="item"></param>
        public void ChangeIsSelected(Item item)
        {
            if (item.IsSelected == false) // If item is not selected, make it selected and save it.
            {
                item.IsSelected = true;

            }
            else // else make it unselected
            {
                item.IsSelected = false;

            }
        }

        /// <summary>
        /// Removes all the selected items from a ShopList and saves it if anything was deleted
        /// </summary>
        /// <param name="shop">The list you want to check</param>
        public void RemoveSelectedItems(ShopList shopList)
        {
            try
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(50));
            }
            catch (Exception){}
            bool isSomethingDeleted = false;
            foreach (var item in shopList.ItemList.ToList())
            {
                if (item.IsSelected)
                {
                    shopList.ItemList.Remove(item);
                    isSomethingDeleted = true;
                }
            }
            if (isSomethingDeleted)
            {
                Save(shopList);
            }
        }

        /// <summary>
        /// Removes all items from the shopList
        /// </summary>
        /// <param name="shopList"></param>
        public void RemoveAllItems(ShopList shopList)
        {
            try
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(50));
            }
            catch (Exception){}

            shopList.ItemList.Clear();
        }

        
    }
}
