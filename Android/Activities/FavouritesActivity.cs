using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Conf.Activities
{
    [Activity(Label = "My Schedule")]
    public class FavouritesActivity : BaseActivity
    {
        List<ConfXml.Session2> _favourites;
        ListView _list;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.CustomTitle); // BETTER: http://www.anddev.org/my_own_titlebar_backbutton_like_on_the_iphone-t4591.html
            SetContentView(Resource.Layout.Favourites);
            Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.WindowTitle); // http://www.londatiga.net/it/how-to-create-custom-window-title-in-android/

            _list = FindViewById<ListView>(Resource.Id.List);
            _list.ItemClick += new EventHandler<ItemEventArgs>(_list_ItemClick);

            _favourites = Conf.Current.FavoriteSessions;
        }
        protected override void OnResume()
        {
            base.OnResume();
            refreshFavourites();
        }
        private void refreshFavourites()
        {
            _list.Adapter = new FavouritesAdapter(this, _favourites);
        }
        private void _list_ItemClick(object sender, ItemEventArgs e)
        {
            var session = ((FavouritesAdapter)_list.Adapter).GetRow(e.Position);

            var intent = new Intent();
            intent.SetClass(this, typeof(SessionActivity));
            intent.PutExtra("Code", session.Code);

            StartActivity(intent);
        }
    }
}