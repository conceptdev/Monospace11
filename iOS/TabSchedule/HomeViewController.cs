using System;
using System.Linq;
using System.Collections.Generic;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Text;
using System.Drawing;
using System.Globalization;

namespace Monospace11
{
	/// <summary>
	/// Yep, there is some dodgy hardcoding going on in here... date-wise...
	/// </summary>
	public partial class HomeViewController : DialogViewController {
		const int days = 3;
		static DateTime [] DayStarts;
		
		static HomeViewController ()
		{
			DayStarts = new DateTime [days+1];
			
			for (int i = 0; i <= days; i++)
			{
				DayStarts [i] = new DateTime (2011, 7, 23+i);
			}
		}
		
		// Used to format a date
		static string FormatDate (DateTime date, DateTime now)
		{	
			if (date.Year == now.Year && date.Month == now.Month){
				if (date.Day == now.Day)
					return "Today";
				if (date.AddDays (1).Day == now.Day)
					return "Yesterday";
				if (date.AddDays (-1).Day == now.Day)
					return "Tomorrow";
			}
			return date.ToString ("dddd");
		}
		
		// Pretifies a caption to show a nice time relative to the current time.
		public static string MakeCaption (string caption, DateTime start)
		{
			string date;
			var now = DateTime.Now;
			
			if (start.Year == now.Year && start.Month == now.Month)
			{
				if (start.Day == now.Day)
					date = ""; 
				else if (start.Day == now.Day+1)
					date = "tomorrow at";
				else
					date = start.ToString ("MMM dd");
			} else
				date = start.ToString ("MMM dd");
			
			return String.Format ("{0}{1} {2} {3}", caption, caption != "" ? " " : "", date, start.ToString ("H:mm"));
		}
		
		// Fills out the schedule for a given day
		public Element MakeSchedule (DateTime d)
		{	// Added .ToString/Convert.ToDateTime 
			// to avoid System.ExecutionEngineException: Attempting to JIT compile method 'System.Collections.Generic.GenericEqualityComparer`1<System.DateTime>:.ctor ()' while running with --aot-only.
		
			var sections = from s in AppDelegate.ConferenceData.Sessions
				where s.Start.Day == d.Day 
				orderby s.Start ascending
				group s by s.Start.ToString() into g
				select new Section (MakeCaption ("", Convert.ToDateTime(g.Key))) {
					from hs in g
					   select (Element) new SessionElement (hs)
			};

			var root = new RootElement (FormatDate (d, DateTime.Now));
			foreach (var s in sections)
				root.Add (s);
			return root;
		}
		
		// Appends the favorites
		void AppendFavorites (Section section, IEnumerable<MIX10Xml.Session> list)
		{
			var favs = AppDelegate.UserData.GetFavoriteCodes ();
			var favsessions = from s in list 
				where favs.Contains (s.Code) 
				select (Element) new SessionElement (s);
			
			section.Add (favsessions);
		}
		
		// Appends a section to the list.
		public bool AppendSection (RootElement root, IEnumerable<MIX10Xml.Session> list, string title)
		{
			if (list == null || list.Count () == 0)
				return false;
			
			var sschedule = new Section ();
			var schedule = new RootElement ("Sessions") { sschedule };
			
			foreach (var s in list)
				sschedule.Add (new SessionElement (s));
			
			var section = new Section (title);
			section.Add (schedule);
			
			AppendFavorites (section, list);
			
			root.Add (section);
			
			return true;
		}
		
		UIBarButtonItem item2;
		
		public HomeViewController () : base (null)
		{

		}

		
		public override void ViewWillAppear (bool animated)
		{
			Root = GenerateRoot ();
			base.ViewWillAppear (animated);
		}

		RootElement GenerateRoot ()
		{
			var now = DateTime.Now;

			var nowStart = now.AddMinutes (now.Minute<30?-now.Minute:-(now.Minute-30)); // also helps (within 30 minutes bug)
			// The shortest session appears to be 30 minutes that I could see
			var nextStart = nowStart.AddMinutes (29); // possible fix to (within 30 minutes bug)
			nextStart = nextStart.AddSeconds(-nextStart.Second);
			
			var happeningNow = from s in AppDelegate.ConferenceData.Sessions
				where s.Start <= now && now < s.End && (s.Start.Minute == 0 || s.Start.Minute == 30) // fix for short-sessions (which start at :05 after the hour)
				select s;
			 
			var root = new RootElement ("Monospace11");

			// Commented out because of post-conference no-upcoming-data-bug
			if (DateTime.Now < new DateTime(2011,7,25,16,0,0))
			{
				// Added .ToString/Convert.ToDateTime 
				// to avoid System.ExecutionEngineException: Attempting to JIT compile method 'System.Collections.Generic.GenericEqualityComparer`1<System.DateTime>:.ctor ()' while running with --aot-only.
				var allUpcoming = from s in AppDelegate.ConferenceData.Sessions
					where s.Start >= nextStart && 
						(s.Start.Minute == 0 || s.Start.Minute ==30 || s.Start.Minute == 15 || s.Start.Minute == 45)
					orderby s.Start.Ticks
					group s by s.Start.Ticks into g
					select new { Start = g.Key, Sessions = g };
	
				var upcoming = allUpcoming.FirstOrDefault ();
				
				var haveNow = AppendSection (root, happeningNow, MakeCaption ("On Now", nowStart));
				AppendSection (root, upcoming.Sessions, MakeCaption ("Up Next", new DateTime (upcoming.Start)));
	
				// Get also the next slot
				if (!haveNow){
					upcoming = allUpcoming.Skip (1).FirstOrDefault ();
					AppendSection (root, upcoming.Sessions, "Afterwards");
				}
			}
			
			var full = new Section ("Full Schedule");
			for (int i = 1; i < DayStarts.Length; i++)
			{
//				if (DateTime.Now <= new DateTime(2011,7,23))
//					if (now < DayStarts [i]);// REMOVED for post-conference display of entire schedule [CD]

				full.Add (MakeSchedule (DayStarts [i-1]));
			}
			
			root.Add (full);
			
			return root;
		}
	}
}