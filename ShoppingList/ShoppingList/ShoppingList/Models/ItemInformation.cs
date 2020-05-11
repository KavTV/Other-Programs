using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ShoppingList.Models
{
    class ItemInformation
    {
        private static ItemInformation itemInformation;
        private string DATA_FILENAME = Path.Combine(App.FolderPath, ".items.txt");
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



        public void AddItem(string key, string text, double price, bool isselected, Store store)
        {
            try
            {
                // If we already had added a friend with this name
                if (key != null)
                {
                    this.itemDictionary.Remove(key);
                    this.itemDictionary.Add(key, new Item(key, text, price, isselected, store));
                    Console.WriteLine("You had already added " + text + ", Updating info..");
                }
                // Else if we do not have this item details 
                // in our dictionary
                else
                {
                    // Add him in the dictionary
                    key = Path.GetRandomFileName();
                    this.itemDictionary.Add(key, new Item(key, text, price, isselected, store));
                    Console.WriteLine("Item added successfully.");
                } // end if
            }
            catch (Exception)
            {

                Console.WriteLine("Something went wrong");
            }
        } // end public bool AddFriend(string key,string item, int price, string store, bool isselected)

        public void RemoveItem(string key)
        {
            try
            {

                // If we do not have a friend with this name
                if (!this.itemDictionary.ContainsKey(key))
                {
                    Console.WriteLine(key + " had not been added before.");
                }
                // Else if we have a friend with this name
                else
                {
                    if (this.itemDictionary.Remove(key))
                    {
                        Console.WriteLine(key + " had been removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Unable to remove " + key);
                    } // end if
                } // end if
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
            }
        } // end public bool RemoveFriend(string name)


        public void Save()
        {
            // Gain code access to the file that we are going
            // to write to
            try
            {
                // Create a FileStream that will write data to file.
                FileStream writerFileStream =
                    new FileStream(DATA_FILENAME, FileMode.Create, FileAccess.Write);
                // Save our dictionary of friends to file
                this.formatter.Serialize(writerFileStream, this.itemDictionary);

                // Close the writerFileStream when we are done.
                writerFileStream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to save item");
            } // end try-catch
        } // end public bool Load()

        public void Load()
        {

            // Check if we had previously Save information of items
            // previously
            if (File.Exists(DATA_FILENAME))
            {

                try
                {
                    // Create a FileStream will gain read access to the 
                    // data file.
                    FileStream readerFileStream = new FileStream(DATA_FILENAME,
                        FileMode.Open, FileAccess.Read);
                    // Reconstruct information of our item from file.
                    this.itemDictionary = (Dictionary<String, Item>)
                        this.formatter.Deserialize(readerFileStream);
                    // Close the readerFileStream when we are done
                    readerFileStream.Close();

                }
                catch (Exception)
                {
                    Console.WriteLine("There seems to be a file that contains " +
                        "item information but somehow there is a problem " +
                        "with reading it.");
                } // end try-catch

            } // end if

        } // end public bool Load()

        public List<Item> GetItems()
        {
            if (this.itemDictionary.Count > 0)
            {

                List<Item> itemList = new List<Item>();
                foreach (var item in itemDictionary)
                {
                    itemList.Add(item.Value);
                }
                return itemList;
            }
            return null;
        }

        public async void ShareItems()
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Del Liste",
                File = new ShareFile(DATA_FILENAME)
            });
        }
        
        public async void OpenUri()
        {
            var supportsUri = await Launcher.CanOpenAsync("txt");
            if (supportsUri)
            {
                await Launcher.OpenAsync("");
            }
            {

            }
        }

    }
}
