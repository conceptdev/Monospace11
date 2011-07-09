using System;
using MonoTouch.UIKit;

namespace Monospace11
{
	public class ICEViewController : UIViewController
	{
		UIImageView imageView;
		public ICEViewController ()
		{
			UIImage img = UIImage.FromFile("Images/surprise.png");
			imageView = new UIImageView();
			imageView.Image = img;
			imageView.Frame = new System.Drawing.RectangleF(0,0,320,480);
			this.View.AddSubview(imageView);
		}
		public override void ViewDidAppear (bool animated)
		{
			MonoTouch.Foundation.NSTimer.CreateScheduledTimer (TimeSpan.FromSeconds (10), delegate
			{
				this.ParentViewController.ModalViewController.DismissModalViewControllerAnimated(true);
			});
		}
	}
}