using Android.Text.Method;
using ShoppingList;
using ShoppingList.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

    [assembly: ExportRenderer(typeof(CusomEntry), typeof(CustomEntryRenderer))]
namespace ShoppingList.Droid
{


        public class CustomEntryRenderer : EntryRenderer
        {

            public CustomEntryRenderer(Android.Content.Context context)
                      : base(context)
            {
            }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;

           

            //  this.Control.KeyListener = Android.Text.Method.DigitsKeyListener.GetInstance(Resources.Configuration.Locale, true, true);
            this.Control.KeyListener = Android.Text.Method.DigitsKeyListener.GetInstance(string.Format("1234567890{0}", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            this.Control.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal;
        }




    }
    
}