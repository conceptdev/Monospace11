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
    [Activity(Label = "Sessions")]
    public class SessionsActivity : BaseActivity
    {
        string _tag;
        List<ConfXml.Session2> _sessions;
        ListView _list;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.CustomTitle); // BETTER: http://www.anddev.org/my_own_titlebar_backbutton_like_on_the_iphone-t4591.html
            SetContentView(Resource.Layout.Sessions);
            Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.WindowTitle); // http://www.londatiga.net/it/how-to-create-custom-window-title-in-android/

            _list = FindViewById<ListView>(Resource.Id.List);
            _list.ItemClick += new EventHandler<ItemEventArgs>(_list_ItemClick);

            var t = Intent.GetStringExtra("Tag");
            var d = Intent.GetStringExtra("SelectedDate");
            var dt = Intent.GetStringExtra("SelectedDateTime");
            
            if (!String.IsNullOrEmpty(d))
            {
                var date = DateTime.Parse(d);

                FindViewById<TextView>(Resource.Id.Title).Text = date.ToString("dddd, dd MMMM");

                var sess = from s in Conf.Current.ConfItem.Sessions
                            where s.Start.Day == date.Day
                            orderby s.Start
                            select s;

                _sessions = sess.ToList();
            }
            else if (!String.IsNullOrEmpty(dt))
            {
                var datetime = DateTime.Parse(dt);

                FindViewById<TextView>(Resource.Id.Title).Text = datetime.ToString("dddd, dd MMMM");

                var sess = from s in Conf.Current.ConfItem.Sessions
                           where s.Start == datetime
                           orderby s.Start
                           select s;

                _sessions = sess.ToList();
            }
            else
            {
                FindViewById<TextView>(Resource.Id.Title).Text = t;

                var _tag = (from tag in Conf.Current.ConfItem.Tags
                            where tag.Value == t
                            select tag).FirstOrDefault();

                var sessionsWithTag = new List<ConfXml.Session2>();
                foreach (var code in _tag.SessionCodes)
                {
                    var sess = from s in Conf.Current.ConfItem.Sessions
                               where s.Code == code
                               select s;

                    var s1 = sess.FirstOrDefault();
                    if (s1 != null)
                    {
                        sessionsWithTag.Add(s1);
                    }
                }
                _sessions = sessionsWithTag;
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            refreshSessions();
        }
        private void refreshSessions()
        {
            _list.Adapter = new Sessions2Adapter(this, _sessions);
        }

        private void _list_ItemClick(object sender, ItemEventArgs e)
        {
            var session = ((Sessions2Adapter)_list.Adapter).GetRow(e.Position);

            var intent = new Intent();
            intent.SetClass(this, typeof(SessionActivity));
            intent.PutExtra("Code", session.Code);

            StartActivity(intent);
        }
    }
}