using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Monospace11
{
    public class TagsViewController : DialogViewController
    {
		public TagsViewController () : base (null)
		{
		}
		
		public override void LoadView ()
		{
			Root = GenerateRoot ();
			base.LoadView ();
		}

		RootElement GenerateRoot ()
		{
			// The full list
			var allSessions = new RootElement ("All Sessions") {
			  from s in AppDelegate.ConferenceData.Sessions
				orderby s.Start ascending
				group s by s.Start.Ticks into g
				select new Section (HomeViewController.MakeCaption ("", new DateTime (g.Key))) {
					from hs in g
					   select (Element) new SessionElement (hs)
			}};

			// Per tags
			var stags = new Section ("By Category") {
				from tag in AppDelegate.ConferenceData.Tags
				orderby tag.Value 
				select (Element) new RootElement (tag.Value) {
					from s in tag.Sessions 
						group s by s.Start.Ticks into g
						orderby g.Key
						select new Section (HomeViewController.MakeCaption ("", new DateTime (g.Key))){
						from hs in g 
								select (Element) new SessionElement (hs)
				}}};
			
			var root = new RootElement ("Sessions") { 
				new Section ("By Time") { 
					allSessions
				},
				stags
			};
			
			return root;
		}
	}
}