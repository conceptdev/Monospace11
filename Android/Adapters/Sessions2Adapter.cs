using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;

namespace Conf.Activities
{
    public class Sessions2Adapter : BaseAdapter
    {
        private List<ConfXml.Session2> _sessions;
        private Activity _context;

        public Sessions2Adapter(Activity context, List<ConfXml.Session2> sessions)
        {
            _context = context;
            _sessions = sessions;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = (convertView
                            ?? _context.LayoutInflater.Inflate(
                                    Resource.Layout.Sessions2Item, parent, false)
                        ) as LinearLayout;
            var row = _sessions.ElementAt(position);

            view.FindViewById<TextView>(Resource.Id.Time).Text = row.DateTimeDisplay;

            view.FindViewById<TextView>(Resource.Id.Title).Text = row.Title;

            if (row.Room == "")
                view.FindViewById<TextView>(Resource.Id.Room).Text = row.SpeakerList;
            else
                view.FindViewById<TextView>(Resource.Id.Room).Text = row.Room + " room; " + row.SpeakerList;
            

            return view;
        }

        public override int Count
        {
            get { return _sessions.Count(); }
        }

        public ConfXml.Session2 GetRow(int position)
        {
            return _sessions.ElementAt(position);
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