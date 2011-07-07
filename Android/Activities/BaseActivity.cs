
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;

namespace Conf.Activities
{
    public class BaseActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        /// <summary>
        /// http://mgroves.com/monodroid-creating-an-options-menu/ 
        /// </summary>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var item = menu.Add(Android.Views.MenuConsts.None, 1, 1, new Java.Lang.String("What's On"));
            item.SetIcon(Resource.Drawable.calendar);

            item = menu.Add(Android.Views.MenuConsts.None, 2, 2, new Java.Lang.String("Speakers"));  // HACK: todo - add 'using' statement around Java.Lang.Strings for GC (as per novell hint)
            item.SetIcon(Resource.Drawable.microphone);

            item = menu.Add(Android.Views.MenuConsts.None, 3, 3, new Java.Lang.String("Sessions"));
            item.SetIcon(Resource.Drawable.bullhorn);

            item = menu.Add(Android.Views.MenuConsts.None, 4, 4, new Java.Lang.String("My Schedule"));
            item.SetIcon(Resource.Drawable.star);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var intent = new Intent();
            switch (item.TitleFormatted.ToString())
            {
                case "What's On":

                    intent.SetClass(this, typeof(HomeActivity));
                    intent.AddFlags(ActivityFlags.ClearTop);            // http://developer.android.com/reference/android/content/Intent.html#FLAG_ACTIVITY_CLEAR_TOP
                    StartActivity(intent);
                    return true;

                case "Speakers": 

                    intent.SetClass(this, typeof(SpeakersActivity));
                    intent.AddFlags(ActivityFlags.ClearTop);            // http://developer.android.com/reference/android/content/Intent.html#FLAG_ACTIVITY_CLEAR_TOP
                    StartActivity(intent);
                    return true;

                case "Sessions": 

                    intent.SetClass(this, typeof(TagsActivity));
                    intent.AddFlags(ActivityFlags.ClearTop);            // http://developer.android.com/reference/android/content/Intent.html#FLAG_ACTIVITY_CLEAR_TOP
                    StartActivity(intent);
                    return true;

                case "My Schedule":

                    intent.SetClass(this, typeof(FavouritesActivity));
                    StartActivity(intent);
                    return true;

                default:
                    // generally shouldn't happen...
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}