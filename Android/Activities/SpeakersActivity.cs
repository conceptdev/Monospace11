using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Conf.Activities
{
    [Activity(Label = "Speakers")]
    public class SpeakersActivity : BaseActivity
    {
        ListView _list;
        List<ConfXml.Speaker2> _speakers;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.CustomTitle); // BETTER: http://www.anddev.org/my_own_titlebar_backbutton_like_on_the_iphone-t4591.html
            SetContentView(Resource.Layout.Speakers);
            Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.WindowTitle); // http://www.londatiga.net/it/how-to-create-custom-window-title-in-android/

            _speakers = Conf.Current.ConfItem.Speakers ;

            _list = FindViewById<ListView>(Resource.Id.List);
            _list.ItemClick += new EventHandler<ItemEventArgs>(_list_ItemClick);
        }
        protected override void OnResume()
        {
            base.OnResume();
            refreshSpeakers();
        }
        private void refreshSpeakers()
        {
            _list.Adapter = new SpeakersAdapter(this, _speakers);
        }

        private void _list_ItemClick(object sender, ItemEventArgs e)
        {
            var speaker = ((SpeakersAdapter)_list.Adapter).GetRow(e.Position);

            var intent = new Intent();
            intent.SetClass(this, typeof(SpeakerActivity));
            intent.PutExtra("Name", speaker.Name);
            
            StartActivity(intent);
        }
    }
}