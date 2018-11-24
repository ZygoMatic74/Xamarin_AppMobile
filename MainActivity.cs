using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using AppXamarin.frags;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.Design.Widget;
using System;
using Android.Support.Design.Internal;

namespace AppXamarin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : FragmentActivity
    {
        public TextView textMessage;
        private ViewPager _viewPager;
        private TabPagerAdapter adapter;
        private Android.Support.V4.App.FragmentManager fm;
        private BottomNavigationView _navigationView;
        private JavaList<Fragment> pages = new JavaList<Fragment>();
        private Formulaire myForm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //Fragment manager
            fm = this.SupportFragmentManager;

            //viewpager

            _viewPager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            _viewPager.PageSelected += _viewPager_PageSelected;

            //Adapter
            adapter = new TabPagerAdapter(fm, getPages());
            //set Adapter

            _viewPager.Adapter = adapter;


            _navigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            RemoveShiftMode(_navigationView);
            //******La fonction cassé
            _navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

        }

        //*** Ajout des fragments dans la liste de fragment
        private JavaList<Fragment> getPages()
        {
            pages.Add(new ServiceF());
            myForm = new Formulaire();
            pages.Add(myForm);
            pages.Add(new Resultat());
            pages.Add(new Dev());

            return pages;
        }

        //****************** Cases visibles ****************************************************
        private void RemoveShiftMode(BottomNavigationView view)
        {
            var menuView = (BottomNavigationMenuView) view.GetChildAt(0);

            try
            {
                var shiftingMode = menuView.Class.GetDeclaredField("mShiftingMode");
                shiftingMode.Accessible = true;
                shiftingMode.SetBoolean(menuView, false);
                shiftingMode.Accessible = false;

                for (int i = 0; i < menuView.ChildCount; i++)
                {
                    var item = (BottomNavigationItemView)menuView.GetChildAt(i);
                    item.SetShiftingMode(false);
                    // set once again checked value, so view will be updated
                    item.SetChecked(item.ItemData.IsChecked);
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine((ex.InnerException ?? ex).Message);
            }
        }


        //**************************************************************************************

        
        //********************* Afficher dans la NavigBar quand on slide************************
        private void _viewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            var item = _navigationView.Menu.GetItem(e.Position);
            _navigationView.SelectedItemId = item.ItemId;
        }
        //**************************************************************************************


        //********************** Afficher le frag quand on clique en dans la NavigBar *********************************************

        private void NavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.Item.ItemId) {

                case Resource.Id.ServiceF :
                    _viewPager.SetCurrentItem(e.Item.Order, true);
                    break;
                case Resource.Id.Formulaire:
                    _viewPager.SetCurrentItem(e.Item.Order + 1, true);
                    myForm.reload();
                    break;
                case Resource.Id.Resultat:
                    _viewPager.SetCurrentItem(e.Item.Order + 2, true);
                    break;
                case Resource.Id.Dev:
                    _viewPager.SetCurrentItem(e.Item.Order + 3, true);
                    break;
            }
        }

        //*************************************************************************************************************************

        protected override void OnDestroy()
        {
            _viewPager.PageSelected -= _viewPager_PageSelected;
            _navigationView.NavigationItemSelected -= NavigationView_NavigationItemSelected;
            base.OnDestroy();
        }
    }
}