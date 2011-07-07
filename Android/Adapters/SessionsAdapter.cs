using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;

namespace Conf.Activities
{
    public class SessionsAdapter : BaseAdapter
    {
        private List<ConfXml.Session> _sessions;
        private Activity _context;

        public SessionsAdapter(Activity context, List<ConfXml.Session> sessions)
        {
            _context = context;
            _sessions = sessions;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = (convertView
                            ?? _context.LayoutInflater.Inflate(
                                    Resource.Layout.SessionsItem, parent, false)
                        ) as LinearLayout;
            var row = _sessions.ElementAt(position);

            view.FindViewById<TextView>(Resource.Id.Title).Text = row.Title;
            
            return view;
        }

        public override int Count
        {
            get { return _sessions.Count(); }
        }

        public ConfXml.Session GetRow(int position)
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