using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using Conf.ObjectModel;

namespace Conf.Activities
{
    public class HomeAdapter : BaseAdapter
    {
        private List<DayConferenceViewModel> _schedule;
        private Activity _context;
        int _fullScheduleHeadingRow = -1;

        public HomeAdapter(Activity context, List<DayConferenceViewModel> schedule)
        {
            _context = context;
            _schedule = schedule;

            for (int i = 0; i < _schedule.Count; i++)
            {
                if (_schedule[i].SortOrder == 1)
                {
                    _fullScheduleHeadingRow = i;
                    break;
                }
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = (convertView
                            ?? _context.LayoutInflater.Inflate(
                                    Resource.Layout.HomeItem, parent, false)
                        ) as LinearLayout;
            var row = _schedule.ElementAt(position);

            if ((row.SortOrder == 1 && position > _fullScheduleHeadingRow)
                || (!String.IsNullOrEmpty(row.SessCode)))
            {   // hide all except the first
                view.FindViewById<TextView>(Resource.Id.Title).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.Title).Visibility = ViewStates.Visible;
            }


            view.FindViewById<TextView>(Resource.Id.Title).Text = row.Section;
            view.FindViewById<TextView>(Resource.Id.Subtitle).Text = row.LineOne;
            if (String.IsNullOrEmpty(row.LineTwo))
            {
                view.FindViewById<TextView>(Resource.Id.SpeakerList).Text = "";
                view.FindViewById<TextView>(Resource.Id.SpeakerList).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.SpeakerList).Text = row.LineTwo;
                view.FindViewById<TextView>(Resource.Id.SpeakerList).Visibility = ViewStates.Visible;
            }

            return view;
        }

        public override int Count
        {
            get { return _schedule.Count(); }
        }

        public DayConferenceViewModel GetRow(int position)
        {
            return _schedule.ElementAt(position);
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}