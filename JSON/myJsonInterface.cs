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
        String serviceSelected = "null";

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


        // ***********************************************************************************************
        // *****************   GENERATION DU FORMULAIRE POUR LE SERVICE SELECTIONNE        ***************
        // ***********************************************************************************************

        public void generateJsonForm(LinearLayout myView, Context ctxt)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ctxt);
            String myServiceString = prefs.GetString("currentService", "null");

            if(myServiceString != "null")
            {
                if (myServiceString != serviceSelected)
                {
                    createFormForService(myServiceString, myView, ctxt);
                    serviceSelected = myServiceString;
                }
            }
            else
            {
                myView.RemoveAllViewsInLayout();

                TextView myTextView = new TextView(ctxt);
                myTextView.Text = "Aucun service sélectionné !";
                myView.AddView(myTextView);
            }

        }

        private void createFormForService(String myService, LinearLayout myView, Context ctxt)
        {
            myView.RemoveAllViewsInLayout();

            int indiceService = getIndiceForService(myService);
            myElement[] tabElements = myServices.services[indiceService].elements;

            for(int i = 0; i < tabElements.Length; i++)
            {
                switch (tabElements[i].type)
                {
                    case "image" :
                        addImageToView(tabElements[i].value[0], myView, ctxt);
                        break;
                    case "edit":
                        addEditTextToView(tabElements[i].value[0], myView, ctxt);
                        break;
                    case "radioGroup":
                        addRadioGroupToView(tabElements[i].value, myView, ctxt);
                        break;
                    case "label":
                        addLabelToView(tabElements[i].value[0], myView, ctxt);
                        break;
                    case "switch":
                        addSwitchToView(tabElements[i].value[0], myView, ctxt);
                        break;
                    case "button":
                        addButtonToView(tabElements[i].value[0], myView, ctxt);
                        break;
                    default:
                        break;
                }
            }
        }

        private int getIndiceForService(String myService)
        {
            for(int i = 0; i < myServices.services.Length; i++)
            {
                if(myServices.services[i].title == myService) { return i; }
            }
            return -1;
        }

        private void addImageToView(String myUrl, LinearLayout myView, Context ctxt)
        {
            ImageView myImage = new ImageView(ctxt);
            var uri = GetImageBitmapFromUrl(myUrl);
            myImage.SetImageBitmap(uri);
            myImage.SetScaleType(ImageView.ScaleType.FitCenter);
            myView.AddView(myImage);
        }

        private void addEditTextToView(String myHint, LinearLayout myView, Context ctxt)
        {
            EditText myEdit = new EditText(ctxt);
            myEdit.Hint = myHint;
            myView.AddView(myEdit);
        }

        private void addRadioGroupToView(String[] myValues, LinearLayout myView, Context ctxt)
        {
            RadioGroup myRadioGroup = new RadioGroup(ctxt);

            for(int i = 0; i < myValues.Length; i++)
            {
                RadioButton myRadioButton = new RadioButton(ctxt);

                String myValue = myValues[i];
                myRadioButton.Text = myValue;

                myRadioGroup.AddView(myRadioButton);
            }

            myView.AddView(myRadioGroup);
        }

        private void addLabelToView(String myLabel, LinearLayout myView, Context ctxt)
        {
            TextView myText = new TextView(ctxt);
            myText.Text = myLabel;

            myView.AddView(myText);
        }

        private void addSwitchToView(String myBool, LinearLayout myView, Context ctxt)
        {
            Switch mySwitch = new Switch(ctxt);

            if (myBool == "true")
            {
                mySwitch.Checked = true;
            }
            else
            {
                mySwitch.Checked = false;
            }

            myView.AddView(mySwitch);
        }

        private void addButtonToView(String myText, LinearLayout myView, Context ctxt)
        {
            Button myButton = new Button(ctxt);
            myButton.Text = myText;

            myView.AddView(myButton);
        }

        // ***********************************************************************************************
        // *********************   GENERATION DE LA LISTE DES SERVICES         ***************************
        // ***********************************************************************************************

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
