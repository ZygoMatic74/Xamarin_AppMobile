using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Newtonsoft.Json;
using System.IO;
using Java.IO;
using Android.Content.Res;
using Android.Content;
using Android.Views;
using Android.Graphics;
using System.Net;
using Android.Preferences;

namespace AppXamarin
{
  class myJsonInterface
  {
        myServicesListe myServices = new myServicesListe();

    public myJsonInterface(string nameJsonFile, AssetManager assets)
    {
      string content;
      using (StreamReader sr = new StreamReader(assets.Open(nameJsonFile)))
      {
        content = sr.ReadToEnd();
        myServices = JsonConvert.DeserializeObject<myServicesListe>(content);
      }
    }

    public myServicesListe getServices()
        {
            return myServices;
        }

    public void generateJsonForm(LinearLayout myView, Context ctxt)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ctxt);
            String myString = prefs.GetString("currentService", "null");

            if(myString == myServices.services[0].title) {
                // Définition du composant Text. 
                TextView myTextView = new TextView(ctxt);
                myTextView.Text = myServices.services[0].title;
                myView.AddView(myTextView);
            }

            if (myString == myServices.services[1].title)
            {
                TextView myTextView2 = new TextView(ctxt);
                myTextView2.Text = myServices.services[1].title;
                myView.AddView(myTextView2);
            }

            RadioGroup r = new RadioGroup(ctxt);
            String test = "Test";

            RadioButton r1 = new RadioButton(ctxt);
            r1.Text = test;

            r.AddView(r1);
            myView.AddView(r);
        }

    public void generateJsonServiceList(LinearLayout myList, Context ctxt)
        {
            ImageButton myImage = new ImageButton(ctxt);
            var uri =GetImageBitmapFromUrl("https://img3.telestar.fr/var/telestar/storage/images/3/0/5/6/3056045/netflix-annonce-des-projets-france_width1024.png");
            myImage.SetImageBitmap(uri);
            myImage.SetScaleType(ImageView.ScaleType.FitCenter);
            myImage.Click += (sender, e) =>
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ctxt);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString("currentService", myServices.services[0].title);
                editor.Apply();
            };

            ImageButton myImage2 = new ImageButton(ctxt);
            var uri2 = GetImageBitmapFromUrl("https://images-eu.ssl-images-amazon.com/images/I/51rttY7a%2B9L.png");
            myImage2.SetImageBitmap(uri2);
            myImage2.SetScaleType(ImageView.ScaleType.FitCenter);
            myImage2.Click += (sender, e) =>
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ctxt);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString("currentService", myServices.services[1].title);
                editor.Apply();
            };

            myList.AddView(myImage);
            myList.AddView(myImage2);

        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            if (!(url == "null"))
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }

            return imageBitmap;
        }
    }
}
