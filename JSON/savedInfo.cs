using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AppXamarin.JSON
{
    class savedInfo
    {
        string myLabel;
        string myValue;

        public savedInfo(string label, string value)
        {
            myLabel = label;
            myValue = value;
        }
    }
}