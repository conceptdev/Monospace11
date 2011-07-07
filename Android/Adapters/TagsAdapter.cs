using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;

namespace Conf.Activities
{
    public class TagsAdapter : BaseAdapter
    {
        private List<ConfXml.Tag2> _tags;
        private Activity _context;

        public TagsAdapter(Activity context, List<ConfXml.Tag2> tags)
        {
            _context = context;
            _tags = tags;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = (convertView
                            ?? _context.LayoutInflater.Inflate(
                                    Resource.Layout.TagsItem, parent, false)
                        ) as LinearLayout;
            var row = _tags.ElementAt(position);

            view.FindViewById<TextView>(Resource.Id.Title).Text = row.Value;
            

            return view;
        }

        public override int Count
        {
            get { return _tags.Count(); }
        }

        public ConfXml.Tag2 GetRow(int position)
        {
            return _tags.ElementAt(position);
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