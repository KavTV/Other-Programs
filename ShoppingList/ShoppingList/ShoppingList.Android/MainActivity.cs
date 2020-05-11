using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ShoppingList.Droid
{
    [Activity(Label = "Din Indkøbsliste", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    [IntentFilter(new[] { Intent.ActionView, Intent.ActionEdit, Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataMimeType = "text/plain")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var _mainForms = new App();

            LoadApplication(_mainForms);


            if (Intent.Action == Intent.ActionView)
            {
                //Find way to get the path from intent and save the file

                //var uriFromExtras = Intent.GetParcelableExtra(Intent.ExtraStream) as Android.Net.Uri;
                //var subject = Intent.GetStringExtra(Intent.ExtraSubject);

                //// Get the info from ClipData 
                //var path = Intent.Data.Path;
                
                //// Open a stream from the URI 


                //// Save it over 
                //FileStream fs = new FileStream(path,FileMode.Open);
                //StreamReader sr = new StreamReader(fs);
                //string ok = "";
                //while (!sr.EndOfStream)
                //{
                //    ok += sr.ReadLine();
                //}

                
            }

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}