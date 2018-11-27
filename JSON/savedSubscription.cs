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
    class savedSubscription
    {
        private int i;
        savedInfo[] myInfos;

        public void Add(savedInfo myInfo)
        {
            myInfos[i] = myInfo;
            i++;
        }

        public void Reset()
        {
            i = 0;
        }

        public void AdaptSize(int myTaille)
        {
            myInfos = new savedInfo[myTaille];
        }
    }
}