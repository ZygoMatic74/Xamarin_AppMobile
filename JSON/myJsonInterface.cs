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
using AlertDialog = Android.App.AlertDialog;
using Java.Util;
using System.Collections.Generic;
using AppXamarin.JSON;

namespace AppXamarin
{
  class myJsonInterface
  {
        myServicesListe myServices = new myServicesListe();
        String serviceSelected = "null";
        List<savedElement> myElements = new List<savedElement>();
        savedSubscription mySubscription = new savedSubscription();

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
            Object[] tabElements = myServices.services[indiceService].elements;

            mySubscription.Reset();
            int elementToSave = 0;
            myElements.Clear();

            for(int i = 0; i < tabElements.Length; i++)
            {
                switch (tabElements[i].type)
                {
                    case "image" :
                        addImageToView(tabElements[i], myView, ctxt);
                        break;

                    case "edit":
                        addEditTextToView(tabElements[i], myView, ctxt);
                        elementToSave++;
                        break;

                    case "radioGroup":
                        addRadioGroupToView(tabElements[i], myView, ctxt);
                        elementToSave++;
                        break;

                    case "label":
                        addLabelToView(tabElements[i], myView, ctxt);
                        break;

                    case "switch":
                        addSwitchToView(tabElements[i], myView, ctxt);
                        elementToSave++;
                        break;

                    case "button":
                        addCheckBoxToView(tabElements[i], myView, ctxt);
                        elementToSave++;
                        break;

                    default:
                        break;
                }
            }

            mySubscription.AdaptSize(elementToSave);

            Button validateButton = new Button(ctxt);
            validateButton.Text = "Valider";
            validateButton.Click += (sender, e) => {
                string errorMessage = Checker(myElements, ctxt);
                if (errorMessage != "OK")
                {
                    mySubscription.Reset();
                    AlertDialog.Builder alert = new AlertDialog.Builder(ctxt);
                    alert.SetNegativeButton("Fermer", (senderAlert, args) => { });
                    alert.SetMessage("Vous avez oublié des informations ! \n" + errorMessage);
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {
                    mySubscription.Reset();
                }
            };

            myView.AddView(validateButton);

        }

        private String Checker(List<savedElement> mySavedElements, Context ctxt)
        {
            string currentSection = "";
            string value = "";
            string result = "";

            foreach(savedElement item in mySavedElements)
            {
                switch (item.myElement.type)
                {
                    case "edit":
                        TextView tempTextView = item.myView.JavaCast<TextView>();

                        if (tempTextView.Text != "" || !item.myElement.mandatory)
                        {
                            mySubscription.Add(new savedInfo(tempTextView.Hint, tempTextView.Text));
                        }
                        else
                        {
                            result = result + " " + tempTextView.Hint;
                        }
                        break;

                    case "radioGroup":
                        RadioGroup tempRadioGroup = item.myView.JavaCast<RadioGroup>();
                        RadioButton isChecked = tempRadioGroup.GetChildAt(tempRadioGroup.CheckedRadioButtonId).JavaCast<RadioButton>();
                        if(!(isChecked == null))
                        {
                            mySubscription.Add(new savedInfo(currentSection, isChecked.Text));
                        }
                        else
                        {
                            result = result + " " + currentSection + " " + tempRadioGroup.CheckedRadioButtonId;
                        }
                        break;

                    case "label":
                        TextView tempLabel = item.myView.JavaCast<TextView>();
                        currentSection = tempLabel.Text;
                        break;

                    case "switch":
                        Switch tempSwitch = item.myView.JavaCast<Switch>();

                        if (tempSwitch.Checked)
                        {
                            value = "true";
                        }
                        else
                        {
                            value = "false";
                        };

                        mySubscription.Add(new savedInfo(currentSection, value));
                        break;

                    case "button":
                        CheckBox tempCheckBox = item.myView.JavaCast<CheckBox>();
                        currentSection = tempCheckBox.Text;

                        if (tempCheckBox.Checked)
                        {
                            value = "true";
                        }
                        else
                        {
                            value = "false";
                        };

                        mySubscription.Add(new savedInfo(currentSection + item.myElement.section, value ));
                        break;

                    default:
                        break;
                }
            }

            if (result == "") { return "OK"; } else { return result; }
        }

        private int getIndiceForService(String myService)
        {
            for(int i = 0; i < myServices.services.Length; i++)
            {
                if(myServices.services[i].title == myService) { return i; }
            }
            return -1;
        }

        private void addImageToView(Object myElement, LinearLayout myView, Context ctxt)
        {
            ImageView myImage = new ImageView(ctxt);
            var uri = GetImageBitmapFromUrl(myElement.value[0]);
            myImage.SetImageBitmap(uri);
            myImage.SetScaleType(ImageView.ScaleType.FitCenter);
            myView.AddView(myImage);
        }

        private void addEditTextToView(Object myElement, LinearLayout myView, Context ctxt)
        {
            EditText myEdit = new EditText(ctxt);
            myEdit.Hint = myElement.value[0];

            myElements.Add(new savedElement(myEdit,myElement));
            myView.AddView(myEdit);
        }

        private void addRadioGroupToView(Object myElement, LinearLayout myView, Context ctxt)
        {
            RadioGroup myRadioGroup = new RadioGroup(ctxt);


            for(int i = 0; i < myElement.value.Length; i++)
            {
                RadioButton myRadioButton = new RadioButton(ctxt);
                String myValue = myElement.value[i];
                myRadioButton.Text = myValue;
                myRadioButton.Id = i;
                myRadioGroup.AddView(myRadioButton);
            }

            myElements.Add(new savedElement(myRadioGroup,myElement));
            myView.AddView(myRadioGroup);
        }

        private void addLabelToView(Object myElement, LinearLayout myView, Context ctxt)
        {
            TextView myText = new TextView(ctxt);
            myText.Text = myElement.value[0];

            myElements.Add(new savedElement(myText,myElement));
            myView.AddView(myText);
        }

        private void addSwitchToView(Object myElement, LinearLayout myView, Context ctxt)
        {
            Switch mySwitch = new Switch(ctxt);

            if (myElement.value[0] == "true")
            {
                mySwitch.Checked = true;
            }
            else
            {
                mySwitch.Checked = false;
            }

            myElements.Add(new savedElement(mySwitch,myElement));
            myView.AddView(mySwitch);
        }

        private void addCheckBoxToView(Object myElement, LinearLayout myView, Context ctxt)
        {
            CheckBox myCheckBox = new CheckBox(ctxt);
            myCheckBox.Text = myElement.value[0];
            myCheckBox.Checked = true;

            myElements.Add(new savedElement(myCheckBox, myElement));
            myView.AddView(myCheckBox);
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
