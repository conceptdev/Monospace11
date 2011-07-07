using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Conf.Activities
{
    [Activity(Label = "Speaker")]
    public class SpeakerActivity : BaseActivity
    {
        string _name;
        ConfXml.Speaker2 _speaker;
        List<ConfXml.Session> _sessions;
        ListView _list;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.CustomTitle); // BETTER: http://www.anddev.org/my_own_titlebar_backbutton_like_on_the_iphone-t4591.html
            SetContentView(Resource.Layout.Speaker);
            Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.WindowTitle); // http://www.londatiga.net/it/how-to-create-custom-window-title-in-android/

            _name = Intent.GetStringExtra("Name");
            
            var _speaker = (from speaker in Conf.Current.ConfItem.Speakers
                    where speaker.Name == _name
                    select speaker).FirstOrDefault();

            if (_speaker.Name != "")
            {
                FindViewById<TextView>(Resource.Id.Name).Text = _speaker.Name;

                if (!String.IsNullOrEmpty(_speaker.Bio))
                    FindViewById<TextView>(Resource.Id.Bio).Text = _speaker.Bio;
                else
                {
                    var tv = FindViewById<TextView>(Resource.Id.Bio);
                    tv.Text = "no speaker bio available";
                }
                _sessions = _speaker.Sessions;

                _list = FindViewById<ListView>(Resource.Id.SessionList);
                _list.ItemClick += new EventHandler<ItemEventArgs>(_list_ItemClick);
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            refreshSpeaker();
        }
        private void refreshSpeaker()
        {
            _list.Adapter = new SessionsAdapter(this, _sessions);
        }

        private void _list_ItemClick(object sender, ItemEventArgs e)
        {
            var session = ((SessionsAdapter)_list.Adapter).GetRow(e.Position);

            var intent = new Intent();
            intent.SetClass(this, typeof(SessionActivity));
            intent.PutExtra("Code", session.Code);

            StartActivity(intent);
        }
    }
}