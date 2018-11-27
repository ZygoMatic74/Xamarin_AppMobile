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
    class savedElement
    {
        public View myView;
        public Object myElement;

        public savedElement(View linkedView, Object linkedElement)
        {
            myView = linkedView;
            myElement = linkedElement;
        }
    }
}