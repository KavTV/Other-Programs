using System;

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
    //[IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault },DataScheme ="file", DataHost = "*", DataMimeType = "*/*", DataPathPattern = ".*\\..*\\..*\\..*\\.store")] Open files with .store when you click on them (Does not work)
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
                var pdf = Intent.ClipData.GetItemAt(0);

                var uriFromExtras = Intent.GetParcelableExtra(Intent.ExtraStream) as Android.Net.Uri;
                var subject = Intent.GetStringExtra(Intent.ExtraSubject);

                //var pdfStream = ContentResolver.OpenInputStream(pdf.Uri);

                //var memOfPdf = new System.IO.MemoryStream();
                //pdfStream.CopyTo(memOfPdf);

                //var docsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                //var filePath = System.IO.Path.Combine(docsPath, "temp.pdf");

                //System.IO.File.WriteAllBytes(filePath, memOfPdf.ToArray());

                //_mainForms.DisplayThePDF(filePath);
            }

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}