using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Diagnostics;

namespace Monospace11
{
    [Register]
    public class FavoritesViewController : DialogViewController
    {
		public FavoritesViewController () : base (null)
		{
		}
		
		public override void ViewWillAppear (bool animated)
		{
			Root = GenerateRoot ();
			
			Debug.WriteLine ("Summary " + Root.Summary() );
		}
		
		RootElement GenerateRoot ()
		{
			var favs = AppDelegate.UserData.GetFavoriteCodes();
			var root = 	new RootElement ("Favorites") {
						from s in AppDelegate.ConferenceData.Sessions
							where favs.Contains(s.Code)
							group s by s.Start.Ticks into g
							orderby g.Key
							select new Section (HomeViewController.MakeCaption ("", new DateTime (g.Key))) {
							from hs in g
							   select (Element) new SessionElement (hs)
			}};	
			
			if(favs.Count == 0)
			{
				var section = new Section("Whoops, Star a few sessions first!");
				root.Add(section);
			}
			return root;
        }
    }
}
