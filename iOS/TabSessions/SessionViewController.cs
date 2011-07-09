using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Diagnostics;

namespace Monospace11
{
	public class SessionViewController : WebViewControllerBase
	{
		public MIX10Xml.Session DisplaySession;
		public bool IsFromFavoritesView = false;
		public SessionViewController (MIX10Xml.Session session) : base()
		{
			DisplaySession = session;
		}
		public SessionViewController (MIX10Xml.Session session, bool isFromFavs) : this (session)
		{
			IsFromFavoritesView = isFromFavs;
			DisplaySession = session;
		}
		public SessionViewController (string sessionCode) : base()
		{
			foreach (var s in AppDelegate.ConferenceData.Sessions)
			{
				if (s.Code == sessionCode)
				{	
					DisplaySession = s;
				}
			}
			
		}
		public void Update (string sessionCode) 
		{
			foreach (var s in AppDelegate.ConferenceData.Sessions)
			{
				if (s.Code == sessionCode)
				{
					DisplaySession = s;
				}
			}
			if (DisplaySession != null) LoadHtmlString(FormatText()); 
		}

		public void Update (MIX10Xml.Session session) 
		{
			DisplaySession = session;
			LoadHtmlString(FormatText());
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			webView.ShouldStartLoad = delegate (UIWebView webViewParam, NSUrlRequest request, UIWebViewNavigationType navigationType)
			{	// Catch the link click, and process the add/remove favorites
				Debug.WriteLine(request.Url.Host);
				if (navigationType == UIWebViewNavigationType.LinkClicked)
				{
					string path = request.Url.Path.Substring(1);
					if (request.Url.Host == "add.MIX10.app")
					{
						AppDelegate.UserData.AddFavoriteSession(path);
						this.Update(this.DisplaySession);
					}
					else if (request.Url.Host == "remove.MIX10.app")
					{	// "remove.MIX10.app"
						AppDelegate.UserData.RemoveFavoriteSession(path);
						if (this.IsFromFavoritesView)
						{	// once unfavorited, hide and go back to list view
							this.NavigationController.PopViewControllerAnimated(true);
						}
						else
						{
							this.Update(this.DisplaySession);
						}
					}
					else
					{
						this.NavigationController.PushViewController (new WebViewController (request), true);
						return false;
					}
				}
				return true;
			};
		}

		protected override string FormatText()
		{
			string timeFormat = "H:mm";
			StringBuilder sb = new StringBuilder();
			sb.Append(StyleHtmlSnippet);
			sb.Append("<h2>"+DisplaySession.Title+"</h2>"+ Environment.NewLine);
			if (AppDelegate.UserData.IsFavorite(DisplaySession.Code))
			{	// okay this is a little bit of a HACK:
				sb.Append(@"<nobr><a href=""http://remove.MIX10.app/"+DisplaySession.Code+@"""><img src='Images/favorited.png' align='right' border='0'/></a></nobr>");
			}
			else {
				sb.Append(@"<nobr><a href=""http://add.MIX10.app/"+DisplaySession.Code+@"""><img src='Images/favorite.png' align='right' border='0'/></a></nobr>");
			}
			sb.Append("<br/>");
			if (DisplaySession.Speakers.Count > 0)
			{
				sb.Append("<span class='sessionspeaker'>"+DisplaySession.GetSpeakerList() +"</span> "+ Environment.NewLine);
				sb.Append("<br/>");
			}

			sb.Append("<span class='sessiontime'>"
					+ DisplaySession.Start.ToString("ddd MMM dd") + " " 
					+ DisplaySession.Start.ToString(timeFormat)+" - " 
					+ DisplaySession.End.ToString(timeFormat) +"</span><br />"+ Environment.NewLine);

			if (!String.IsNullOrEmpty (DisplaySession.Room))
			{
				sb.Append("<span class='sessionroom'>"+DisplaySession.Room+" room</span><br />"+ Environment.NewLine);
				sb.Append("<br />"+ Environment.NewLine);
			}
			sb.Append("<span class='body'>"+DisplaySession.Brief+"</span>"+ Environment.NewLine);

			if (DisplaySession.Tags.Count > 0)
			{
				sb.Append("<br /><br />"+ Environment.NewLine);
				sb.Append("Tags: <span class='sessiontag'>"+DisplaySession.GetTagList()+"</span>"+ Environment.NewLine);
			}
				
			return sb.ToString();
		}
	}
}