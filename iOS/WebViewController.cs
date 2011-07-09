using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Monospace11
{
	public class WebViewController : UIViewController {
		UIWebView webView;
		UIToolbar navBar;
		NSUrlRequest url;
		UIBarButtonItem [] items;
		
		public WebViewController (NSUrlRequest url) : base ()
		{
			this.url = url;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			navBar = new UIToolbar ();
			navBar.Frame = new RectangleF (0, this.View.Frame.Height-130, this.View.Frame.Width, 40);
			
			items = new UIBarButtonItem [] {
				new UIBarButtonItem ("Back", UIBarButtonItemStyle.Bordered, (o, e) => { webView.GoBack (); }),
				new UIBarButtonItem ("Forward", UIBarButtonItemStyle.Bordered, (o, e) => { webView.GoForward (); }),
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace, null),
				new UIBarButtonItem (UIBarButtonSystemItem.Refresh, (o, e) => { webView.Reload (); }),
				new UIBarButtonItem (UIBarButtonSystemItem.Stop, (o, e) => { webView.StopLoading (); })
			};
			navBar.Items = items;
			
			SetNavBarColor();
			
			webView = new UIWebView ();
			webView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height-130);
			
			webView.LoadStarted += delegate {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			};
			webView.LoadFinished += delegate {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			};
			
			webView.ScalesPageToFit = true;
			webView.SizeToFit ();
			webView.LoadRequest (url);
			
			this.View.AddSubview (webView);
			this.View.AddSubview (navBar);
		}
		
		private void SetNavBarColor ()
		{
			navBar.BarStyle = UIBarStyle.Black;
			var frame = new RectangleF(0f, 0f, this.View.Bounds.Width, 40f);
			using (var v = new UIView(frame))
			{
				using(var imageView = new UIImageView(UIImage.FromFile("/Images/TabBarBackground.png")))
				{
					imageView.Frame = frame;
					imageView.Alpha = 0.43f;
					navBar.AddSubview(imageView);	
				}
			}
		}
	}	
}

