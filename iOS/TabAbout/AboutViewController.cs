using System.Text;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Monospace11
{
	public class AboutViewController : WebViewControllerBase
	{
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			webView.ShouldStartLoad = delegate(UIWebView webViewParam, NSUrlRequest request, UIWebViewNavigationType navigationType) {
				// view links in a new 'webbrowser' window like session & twitter
				if (navigationType == UIWebViewNavigationType.LinkClicked) {
					this.NavigationController.PushViewController (new WebViewController (request), true);
					return false;
				}
				return true;
			};
		}

		protected override string FormatText ()
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append (StyleHtmlSnippet);
			sb.Append (AppDelegate.ConferenceData.AboutHtml);
			return sb.ToString ();
		}
	}
}












