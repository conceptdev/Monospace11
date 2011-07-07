using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Conf.Activities
{
    [Preserve]
    [Activity(Label = "Monospace11", MainLauncher = true, Icon="@drawable/icon")]
    public class HomeActivity : BaseActivity
    {
        ListView _list;
        List<ObjectModel.DayConferenceViewModel> _schedule;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.CustomTitle); // BETTER: http://www.anddev.org/my_own_titlebar_backbutton_like_on_the_iphone-t4591.html
            SetContentView(Resource.Layout.Home);
            Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.WindowTitle); // http://www.londatiga.net/it/how-to-create-custom-window-title-in-android/

            _schedule = Conf.Current.ScheduleItems;

            _list = FindViewById<ListView>(Resource.Id.List);
            _list.ItemClick += new EventHandler<ItemEventArgs>(_list_ItemClick);
        }
        protected override void OnResume()
        {
            base.OnResume();
            refreshSchedule();
        }
        private void refreshSchedule()
        {
            _schedule = Conf.Current.ScheduleItems;
            _list.Adapter = new HomeAdapter(this, _schedule);
        }

        private void _list_ItemClick(object sender, ItemEventArgs e)
        {
            var bm = ((HomeAdapter)_list.Adapter).GetRow(e.Position);
            var intent = new Intent();
            switch (bm.SortOrder)
            { 
                case 0:
                    if (!String.IsNullOrEmpty(bm.Day))
                    {   // use date
                        intent.SetClass(this, typeof(SessionsActivity));
                        intent.PutExtra("SelectedDateTime", bm.Day);
                    }
                    else
                    {   // use session
                        intent.SetClass(this, typeof(SessionActivity));
                        intent.PutExtra("Code", bm.SessCode);
                    }
                    StartActivity(intent);
                    break;
                case 1:
                    intent.SetClass(this, typeof(SessionsActivity));
                    intent.PutExtra("SelectedDate", bm.Day);

                    StartActivity(intent);
                    break;
                case 2:
                    // for future use
                    break;
            }     
        }
    }
}