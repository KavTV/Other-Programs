using System;
using System.Text;
using Xamarin.Forms;
using SQLite;

namespace ShoppingList.Models
{
    [Serializable]
    public class Store
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string SetColor { get; set; }
        public Color GetColor { get { return Color.FromHex(SetColor); } }

        //public Store(string name, Color color)
        //{
        //    this.Name = name;
        //    this.SetColor = color.ToHex();
        //}
    }
}
