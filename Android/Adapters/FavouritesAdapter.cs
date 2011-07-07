using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;

namespace Conf.Activities
{
    public class FavouritesAdapter : BaseAdapter
    {
        private List<ConfXml.Session2> _favourites;
        private Activity _context;

        public FavouritesAdapter(Activity context, List<ConfXml.Session2> sessions)
        {
            _context = context;
            _favourites = sessions;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = (convertView
                            ?? _context.LayoutInflater.Inflate(
                                    Resource.Layout.FavouritesItem, parent, false)
                        ) as LinearLayout;
            var row = _favourites.ElementAt(position);

            view.FindViewById<TextView>(Resource.Id.Time).Text = row.DateTimeQuickJumpDisplay;
            view.FindViewById<TextView>(Resource.Id.Title).Text = row.Title;
            if (row.Room != "")
                view.FindViewById<TextView>(Resource.Id.Room).Text = row.Room + " room";
            

            return view;
        }

        public override int Count
        {
            get { return _favourites.Count(); }
        }

        public ConfXml.Session2 GetRow(int position)
        {
            return _favourites.ElementAt(position);
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