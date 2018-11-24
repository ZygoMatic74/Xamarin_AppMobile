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
    class ServiceF : Fragment
    {
        myJsonInterface recupJson;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.layout_Service, container, false);

            Context myCtxt = rootView.Context;
            Android.Content.Res.AssetManager assets = this.Context.Assets;

            recupJson = new myJsonInterface("service.json", assets);

            LinearLayout myLinear = rootView.FindViewById<LinearLayout>(Resource.Id.listServiceJson);
            recupJson.generateJsonServiceList(myLinear, myCtxt);

            return rootView;

        }
    }
}