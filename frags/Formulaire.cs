using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace AppXamarin.frags
{
    class Formulaire : Fragment
    {
        myJsonInterface recupJson;
        Context myCtxt;
        LinearLayout myLinear;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.layout_Formulaire, container, false);

            myCtxt = rootView.Context;
            Android.Content.Res.AssetManager assets = this.Context.Assets;

            recupJson = new myJsonInterface("service.json",assets);

            myLinear = rootView.FindViewById<LinearLayout>(Resource.Id.jsonGeneratedView);
            recupJson.generateJsonForm(myLinear, myCtxt);

            return rootView;

        }

        public void reload()
        {
            recupJson.generateJsonForm(myLinear, myCtxt);
        }
    }
}