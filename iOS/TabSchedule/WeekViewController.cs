using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

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
    public class WeekViewController : UIViewController
    {
        private UITableView tableView;
		private List<MIX10Xml.Day> _days;

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			
			_days = AppDelegate.ConferenceData.Days;
			
			UIImageView imageView = new UIImageView(UIImage.FromFile("Background.png"));
			imageView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
			imageView.UserInteractionEnabled = true;
			
			// no XIB !
			tableView = new UITableView()
			{
			    Source = new TableViewSource(this, _days),
			    AutoresizingMask = UIViewAutoresizing.FlexibleHeight|
			                       UIViewAutoresizing.FlexibleWidth,
			    BackgroundColor = UIColor.Clear,
				Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height),
			};
			// Set the table view to fit the width of the app.
			tableView.SizeToFit();
			// Reposition and resize the receiver
			tableView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
			// Add the table view as a subview
			this.View.AddSubview(tableView);
			
			
			imageView.AddSubview(tableView);
			this.View.AddSubview(imageView);
        }

        private class TableViewSource : UITableViewSource
        {
			private WeekViewController _dvc;
			private List<MIX10Xml.Day> _dates;
            public TableViewSource(WeekViewController controller, List<MIX10Xml.Day> dates)
            {
				_dvc = controller;
				_dates = dates;
            }

			/// <summary>
			/// If there are subsections in the hierarchy, navigate to those
			/// ASSUMES there are _never_ Categories hanging off the root in the hierarchy
			/// </summary>
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
				string date = _dates[indexPath.Row].Date.ToString("dd-MMM dddd");
				DayViewController sessionsView = new DayViewController(_dates[indexPath.Row].Timeslots);
				sessionsView.Title = date;
				_dvc.NavigationController.PushViewController(sessionsView,true);
				tableView.DeselectRow(indexPath,true);
			}
      
            static NSString kCellIdentifier = new NSString ("MyDateIdentifier");

            public override int RowsInSection (UITableView tableview, int section)
            {
                return _dates.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell (kCellIdentifier);
                if (cell == null)
                {
                    cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
                }
				string date = _dates[indexPath.Row].Date.ToString("dd-MMM dddd");
				
                cell.TextLabel.Text = date;
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                return cell;
            }
        }
    }
}