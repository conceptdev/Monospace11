using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
namespace Monospace11
{
    public class TableViewControllerBase : UIViewController
    {
        public UITableView tableView;
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

			UIImageView imageView = new UIImageView(UIImage.FromFile("Background.png"));
			imageView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
			imageView.UserInteractionEnabled = true;

			// no XIB !
			tableView = new UITableView()
			{
			    AutoresizingMask = UIViewAutoresizing.FlexibleHeight|
			                       UIViewAutoresizing.FlexibleWidth,
			    BackgroundColor = UIColor.Clear,
				Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height)
			};
			imageView.AddSubview(tableView);
			this.View.AddSubview(imageView);
        }
    }
}
