using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Conf.Activities
{
    [Activity(Label = "Tags")]
    public class TagsActivity : BaseActivity
    {
        List<ConfXml.Tag2> _tags;
        ListView _list;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.CustomTitle); // BETTER: http://www.anddev.org/my_own_titlebar_backbutton_like_on_the_iphone-t4591.html
            SetContentView(Resource.Layout.Tags);
            Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.WindowTitle); // http://www.londatiga.net/it/how-to-create-custom-window-title-in-android/

            _list = FindViewById<ListView>(Resource.Id.List);
            _list.ItemClick += new EventHandler<ItemEventArgs>(_list_ItemClick);

            _tags = Conf.Current.ConfItem.Tags;
        }
        protected override void OnResume()
        {
            base.OnResume();
            refreshTags();
        }
        private void refreshTags()
        {
            _list.Adapter = new TagsAdapter(this, _tags);
        }

        private void _list_ItemClick(object sender, ItemEventArgs e)
        {
            var tag = ((TagsAdapter)_list.Adapter).GetRow(e.Position);

            var intent = new Intent();
            intent.SetClass(this, typeof(SessionsActivity));
            intent.PutExtra("Tag", tag.Value);

            StartActivity(intent);
        }
    }
}