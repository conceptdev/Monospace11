using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MIX10
{

	public class RssSessionsEntryViewController: UIViewController
	{
		/// <summary>
		/// RssViewController 
		/// </summary>
		//RssViewController rootmvc;
		string _html;//,_title;
		
		public RssSessionsEntryViewController (string title, string html) : base()
		{
			_html = html;
			//_title = title;
		}
		
		public UITextView textView;
		public UIWebView webView;
		
		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			// no XIB !
			webView = new UIWebView()
			{
				ScalesPageToFit = false
			};
			webView.LoadHtmlString(FormatText(), new NSUrl());
			
			// Set the web view to fit the width of the app.
            webView.SizeToFit();

            // Reposition and resize the receiver
            webView.Frame = new RectangleF (
                0, 0, this.View.Frame.Width, this.View.Frame.Height);

            // Add the table view as a subview
            this.View.AddSubview(webView);
			
		}		
		/// <summary>
		/// Format the parts-of-speech text for UIWebView
		/// </summary>
		private string FormatText()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.Append("<style>body,b,p{font-family:Helvetica;}</style>");
			sb.Append(_html);
			sb.Append("<br/>");
			return sb.ToString();
		}
	}
}
