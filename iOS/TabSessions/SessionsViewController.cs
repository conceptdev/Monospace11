using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Monospace11
{
    [Register]
    public class SessionsViewController : UIViewController
    {
        private UITableView tableView;
		public List<MIX10Xml.Session> Sessions;
		public List<string> FavoriteCodes;
		private int _timeslot = -1;
		private string _tag = "";
		private bool didViewDidLoadJustRun = true;

		public SessionsViewController ()
		{
			_tag = "";
			_timeslot = -1;
		}
		
		public SessionsViewController (int timeslot)
		{
			_timeslot = timeslot;
			_tag = "";
		}
		public SessionsViewController (string tag)
		{
			_tag = tag;
			_timeslot = -1;
		}

		public override void ViewWillAppear (bool animated)
		{
			if (didViewDidLoadJustRun == true) {didViewDidLoadJustRun = false; return;}
			FavoriteCodes = AppDelegate.UserData.GetFavoriteCodes();
			tableView.ReloadData();
			base.ViewWillAppear (animated);
		}
		
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			if (_timeslot > 0)
			{	// filter by timeslot
				Sessions = AppDelegate.ConferenceData.Sessions.Take(4).ToList();
			}
			else if (!String.IsNullOrEmpty(_tag))
			{	// filter by tag
				var x = from s in AppDelegate.ConferenceData.Sessions
						where s.HasTag(_tag)
						select s;
				Sessions = x.ToList();
			}
			else
			{	// just show all
				Sessions = AppDelegate.ConferenceData.Sessions;
			}
			FavoriteCodes = AppDelegate.UserData.GetFavoriteCodes();

			UIImageView imageView = new UIImageView(UIImage.FromFile("Background.png"));
			imageView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
			imageView.UserInteractionEnabled = true;

			// no XIB !
			tableView = new UITableView()
			{
			    Source = new TableViewSource(this),
			    AutoresizingMask = UIViewAutoresizing.FlexibleHeight|
			                       UIViewAutoresizing.FlexibleWidth,
			    BackgroundColor = UIColor.Clear,
				Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height-100)
			};
			imageView.AddSubview(tableView);
			this.View.AddSubview(imageView);
			didViewDidLoadJustRun = true;
        }

        private class TableViewSource : UITableViewSource
        {
			private SessionsViewController _svc;
			private UIImage starImage;
            public TableViewSource(SessionsViewController controller)
            {
				_svc = controller;
				starImage =  UIImage.FromFile("Images/star.png");
            }
			private SessionViewController sessVC;
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
				MIX10Xml.Session s = _svc.Sessions[indexPath.Row];
				if (sessVC == null)
					sessVC = new SessionViewController(s);
				else
					sessVC.Update(s);
				sessVC.Title = s.Title;
				_svc.NavigationController.PushViewController(sessVC, true);
				tableView.DeselectRow(indexPath,true);
			}

            public override int RowsInSection (UITableView tableview, int section)
            {
                return _svc.Sessions.Count;
            }
            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
				MIX10Xml.Session s = _svc.Sessions[indexPath.Row];
				bool star = _svc.FavoriteCodes.Contains(s.Code);
				UITableViewCell cell = null;
				if (star)
				{
					cell = tableView.DequeueReusableCell ("star");
					if (cell == null)
					{
						cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "star");
					}
					cell.ImageView.Image = starImage;
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				}
				else
				{
					cell = tableView.DequeueReusableCell ("plain");
					if (cell == null)
					{
						cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "plain");
					}
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				}
				try
				{
					cell.TextLabel.Text = s.Title;
					cell.DetailTextLabel.Text = s.GetSpeakerList();
				} catch (Exception){}
                return cell;
            }
        }
    }
}