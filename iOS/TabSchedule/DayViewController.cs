using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MIX10Xml;
using System.Diagnostics;
using Monospace11;
namespace Monospace11
{
	/// <summary>
	/// First view that users see - lists the top level of the hierarchy xml
	/// </summary>
	/// <remarks>
	/// LOADS data from the xml files into public properties (deserialization)
	/// then we pass around references to the MainViewController so other
	/// ViewControllers can access the data.</remarks>
	[Register]
	public class DayViewController : UIViewController
	{
		private UITableView tableView;
		private string _date;

		private List<MIX10Xml.Timeslot> _slots;

		public DayViewController (string date)
		{
			_date = date;
		}
		public DayViewController (List<MIX10Xml.Timeslot> slots)
		{
			_slots = slots;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			UIImageView imageView = new UIImageView (UIImage.FromFile ("Background.png"));
			imageView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
			imageView.UserInteractionEnabled = true;
			
			// no XIB !
			tableView = new UITableView { Source = new TableViewSource (this, _date, _slots), AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth, BackgroundColor = UIColor.Clear, Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height - 100) };
			imageView.AddSubview (tableView);
			this.View.AddSubview (imageView);
		}

		private class TableViewSource : UITableViewSource
		{
			private DayViewController _svc;
			private List<Timeslot> _slots;

			public TableViewSource (DayViewController controller, string date, List<Timeslot> slots)
			{
				_svc = controller;
				_slots = slots;
				controllers = new Dictionary<int, SessionCellController> ();
			}

			private TimeslotViewController slotVC;
			/// <summary>
			/// If there are subsections in the hierarchy, navigate to those
			/// ASSUMES there are _never_ Categories hanging off the root in the hierarchy
			/// </summary>
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var s = _slots[indexPath.Row];
				if (slotVC == null)
					slotVC = new TimeslotViewController (s);
				else
					slotVC.Update (s);
				slotVC.Title = s.Title;
				_svc.NavigationController.PushViewController (slotVC, true);
			}

			public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
			{
				var s = _slots[indexPath.Row];
				SessionsViewController sessionsView = new SessionsViewController ();
				// TODO: timeslot
				sessionsView.Title = s.Title;
				_svc.NavigationController.PushViewController (sessionsView, true);
			}
			public override NSIndexPath WillSelectRow (UITableView tableView, NSIndexPath indexPath)
			{
				var s = _slots[indexPath.Row];
				if (String.IsNullOrEmpty (s.Brief))
					return null;
				return indexPath;
			}

			static NSString kCellIdentifier = new NSString ("MySessionIdentifier");

			private Dictionary<int, SessionCellController> controllers = null;


			public override int RowsInSection (UITableView tableview, int section)
			{
				return _slots.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (kCellIdentifier);
				var s = _slots[indexPath.Row];
				SessionCellController cellController = null;
				
				if (cell == null) {
					cellController = new SessionCellController ();
					NSBundle.MainBundle.LoadNib ("SessionCellController", cellController, null);
					cell = cellController.Cell;
					cell.Tag = Environment.TickCount;
					//HACK: fix this crappy hack
					controllers.Add (cell.Tag, cellController);
				} else {
					cellController = controllers[cell.Tag];
				}
				if (s.Title == "Workshops" || s.Title == "Sessions")
					cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton; else if (!String.IsNullOrEmpty (s.Brief))
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				else
					cell.Accessory = UITableViewCellAccessory.None;
				try {
					Debug.WriteLine ("Original time: " + s.StartTime.ToString ());
					Debug.WriteLine ("Universal time: " + s.StartTime.ToUniversalTime ().ToString ());
					Debug.WriteLine ("Local time: " + s.StartTime.ToLocalTime ().ToString ());
					cellController.SessionTitle = s.Title;
					cellController.Subtitle = "";
					// TODO: chosen one?
					cellController.Time = s.StartTime.ToUniversalTime ().ToString ("HH:mm");
					if (s.StartTime.ToUniversalTime () == s.EndTime.ToUniversalTime ())
						cellController.EndTime = "";
					else
						cellController.EndTime = s.EndTime.ToUniversalTime ().ToString ("HH:mm");
				} catch (Exception) {
				}
				return cell;
			}


			public void ShowAlert (string title, string message)
			{
				using (var alert = new UIAlertView (title, message, null, "OK", null)) {
					alert.Show ();
				}
			}
		}
	}
}
