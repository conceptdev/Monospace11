using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
namespace Monospace11
{
	public class WebViewControllerBase : UIViewController
	{
		#region respond to shaking (OS3+)
		// also requires you to put
		// UIApplication.SharedApplication.ApplicationSupportsShakeToEdit = true;
		// in Main.cs : FinishedLaunching()
		public override bool CanBecomeFirstResponder {
			get {
				return true;
			}
		}
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.BecomeFirstResponder();
		}
		public override void ViewWillDisappear (bool animated)
		{
			this.ResignFirstResponder();
			base.ViewWillDisappear (animated);
		}
		public override void MotionEnded (UIEventSubtype motion, UIEvent evt)
		{
			Console.WriteLine("Motion detected");
			if (motion ==  UIEventSubtype.MotionShake)
			{
				Console.WriteLine("and was a shake");
				//labelLastUpdated.Text = "All shook up! Updating..."; // never appears
				// Do your application-specific shake response here...
				var ice = new ICEViewController();
				this.PresentModalViewController (ice, true);

			}
		}
		#endregion

		protected string basedir;
		protected UIWebView webView;
		/// <summary>
		/// Shared Css styles
		/// </summary>
		public string StyleHtmlSnippet
		{
			get 
			{  // http://jonraasch.com/blog/css-rounded-corners-in-all-browsers
				return "<style>" +
				"body {background-image:url('Background.png'); background-color:#F0F0F0; }"+
				"body,b,i,p,h2{font-family:Helvetica;}" +
				"h1,h2{color:#F09402;}" +
				"h1,h2{margin-bottom:0px;}" +
				".footnote{font-size:small;}" +
				".sessionspeaker{color:#444444;font-weight:bold;}" +
				".sessionroom{color:#666666;}" +
				".sessiontime{color:#666666;}" +
				".sessiontag{color:#800020;}" +
				"div.sessionspeaker { -webkit-border-radius:12px; background:white; width:285; color:black; padding:8 10 10 8;  }" +
				"a.sessionspeaker {color:black; text-decoration:none;}"+
				"</style>";
			}
		}
		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			basedir = Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			basedir = basedir.Replace("Documents", "Monospace11.app");
			// no XIB !
			webView = new UIWebView()
			{
				ScalesPageToFit = false,
			};
			LoadHtmlString(FormatText());
            webView.SizeToFit();
            webView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height-93);
            // Add the table view as a subview
            this.View.AddSubview(webView);
		}
		protected virtual string FormatText()
		{ return ""; }

		protected void LoadHtmlString (string s)
		{
			webView.LoadHtmlString(s, new NSUrl(basedir, true));
		}
	}
}