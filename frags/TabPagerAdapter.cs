using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace AppXamarin.frags
{
    class TabPagerAdapter : FragmentPagerAdapter
    {
        JavaList<Fragment> fragments;

        public TabPagerAdapter(FragmentManager fm, JavaList<Fragment> fragments) : base(fm)
        {
            this.fragments = fragments;
        }

        public override int Count
        {

            get
            {
                return fragments.Size();
            }
        }


        public override Fragment GetItem(int position)
        {
            return fragments[position];
        }
    }
}