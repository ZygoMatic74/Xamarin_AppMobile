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
            String myServiceString = prefs.GetString("currentService", "null");

            myView.RemoveAllViewsInLayout();

            if(myServiceString != "null")
            {
                if (myServiceString == myServices.services[0].title)
                {
                    // Définition du composant Text. 
                    TextView myTextView = new TextView(ctxt);
                    myTextView.Text = myServices.services[0].title;
                    myView.AddView(myTextView);
                }

                if (myServiceString == myServices.services[1].title)
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
            else
            {
                TextView myTextView = new TextView(ctxt);
                myTextView.Text = "Aucun service sélectionné !";
                myView.AddView(myTextView);
            }

        }

    public void generateJsonServiceList(LinearLayout myList, Context ctxt)
        {
            for(int i = 0; i < myServices.services.Length; i++)
            {
                string service = myServices.services[i].title;

                ImageButton myImage = new ImageButton(ctxt);
                var uri = GetImageBitmapFromUrl(getImageForService(i));
                myImage.SetImageBitmap(uri);
                myImage.SetScaleType(ImageView.ScaleType.FitCenter);

                myImage.Click += (sender, e) =>
                {
                    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ctxt);
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString("currentService", service);
                    editor.Apply();
                };

                myList.AddView(myImage);
            }
        }



        // ***** Fonction permettant de récupérer une image en focntion de son URL et la converti en Bitmap
        // ***** Trouvée sur un forum
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

        // ***** Fonction permettant de renvoyer l'URL de l'image pour le service d'indice l
        private String getImageForService(int l)
        {
            myService currentService = myServices.services[l];

            for (int i = 0; i<currentService.elements.Length;i++)
            {
                if(currentService.elements[i].type == "image") { return currentService.elements[i].value[0]; }
            }
            return "";
        }
    }
}
