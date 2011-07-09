using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SQLite;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
namespace Monospace11
{
	public class TabBarController : UITabBarController
	{
		/// <summary>
		/// One ViewController for each tab
		/// </summary>
		MonoTouch.UIKit.UINavigationController 
				  navSessionController
				, navSpeakerController
				, navScheduleController
				, navFavoritesController;
		
		UIColor HeaderColor = new UIColor(0.94f, 0.58f, 0.02f, 1f);
		
		/// <summary>
		/// Create the ViewControllers that we are going to use for the tabs:
		/// Sessions, Speakers
		/// </summary>
		/// <remarks>
		/// Some icons from glyphish.com -- CCA so DON'T FORGET attribution on the website!!
		/// </remarks>
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var dvc = new HomeViewController ();

			navScheduleController = new MonoTouch.UIKit.UINavigationController();
			navScheduleController.PushViewController(dvc, false);
			navScheduleController.NavigationBar.BarStyle = UIBarStyle.Black;
			navScheduleController.NavigationBar.TintColor = HeaderColor;
			navScheduleController.TopViewController.Title ="What's on";
			navScheduleController.TabBarItem = new UITabBarItem("What's on", UIImage.FromFile("Images/83-calendar.png"), 0);
			
			var svc = new SpeakersViewController();
			navSpeakerController = new MonoTouch.UIKit.UINavigationController();
			navSpeakerController.PushViewController(svc, false);
			navSpeakerController.TopViewController.View.BackgroundColor = new UIColor(65.0f,169.0f,198.0f,255.0f);
			navSpeakerController.NavigationBar.BarStyle = UIBarStyle.Black;
			navSpeakerController.NavigationBar.TintColor = HeaderColor;
			navSpeakerController.TopViewController.Title ="Speakers";
			navSpeakerController.TabBarItem = new UITabBarItem("Speakers", UIImage.FromFile("Images/tabspeaker.png"), 1);
			
			var ssvc = new TagsViewController(); 
			navSessionController = new MonoTouch.UIKit.UINavigationController();
			navSessionController.PushViewController(ssvc, false);
			navSessionController.NavigationBar.BarStyle = UIBarStyle.Black;
			navSessionController.NavigationBar.TintColor = HeaderColor;
			navSessionController.TopViewController.Title ="Sessions by tag";
			navSessionController.TabBarItem = new UITabBarItem("Sessions", UIImage.FromFile("Images/124-bullhorn.png"), 2);
			
			var mvc = new MapFlipViewController();
			mvc.Title = "Map";
			mvc.TabBarItem = new UITabBarItem("Map", UIImage.FromFile("Images/103-map.png"), 5);
			
			var fvc = new FavoritesViewController();
			navFavoritesController = new MonoTouch.UIKit.UINavigationController();
			navFavoritesController.PushViewController(fvc, false);
			navFavoritesController.NavigationBar.BarStyle = UIBarStyle.Black;
			navFavoritesController.NavigationBar.TintColor = HeaderColor;
			navFavoritesController.TopViewController.Title ="My Schedule";
			navFavoritesController.TabBarItem = new UITabBarItem("My Schedule", UIImage.FromFile("Images/28-star.png"), 7);
			
			var u = new UIViewController[]
			{
				  navScheduleController
				, navSpeakerController
				, navSessionController
				, mvc
				, navFavoritesController
			};
			
			this.SelectedIndex = 0;
			this.ViewControllers = u;
			this.MoreNavigationController.NavigationBar.TintColor = HeaderColor;
			this.MoreNavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
			
			var backgroundColor = UIColor.FromPatternImage(UIImage.FromFile("Background.png"));
			this.MoreNavigationController.View.BackgroundColor = backgroundColor;
			
			this.CustomizableViewControllers = new UIViewController[]{};
		}
	}
}