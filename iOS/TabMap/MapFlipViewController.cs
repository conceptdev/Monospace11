using MIX10Xml;
using MonoTouch.UIKit;
using System;
using System.Drawing;

namespace Monospace11
{
	public class MapFlipViewController : UIViewController
	{
		MapViewController mapView;
		MapLocationViewController locationView;
		
		public override void ViewDidLoad ()
        {
			base.ViewDidLoad ();
			mapView = new MapViewController(this);
			mapView.View.Frame = new RectangleF(0,0,this.View.Frame.Width, this.View.Frame.Height);
			this.View.AddSubview(mapView.View);
		}
		
		public void Flip(MapLocation toLocation)
		{
			mapView.SetLocation(toLocation); // assume not null, since it's created in ViewDidLoad ??
			
			Flip();
		}
		
		public void Flip()
		{
			// lazy load the non-default view
			if (locationView == null)
			{
				locationView = new MapLocationViewController(this); 
				locationView.View.Frame = new RectangleF(0,0,this.View.Frame.Width, this.View.Frame.Height);
			}
			UIView.BeginAnimations("Flipper");
			UIView.SetAnimationDuration(1.25);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
			if (mapView.View.Superview == null)
			{	// to map
				UIView.SetAnimationTransition (UIViewAnimationTransition.FlipFromRight, this.View, true);
				locationView.ViewWillAppear(true);
				mapView.ViewWillDisappear(true);
				
				locationView.View.RemoveFromSuperview();
				this.View.AddSubview(mapView.View);
				
				mapView.ViewDidDisappear(true);
				locationView.ViewDidAppear(true);
			}
			else
			{	// to list
				UIView.SetAnimationTransition (UIViewAnimationTransition.FlipFromLeft, this.View, true);
				mapView.ViewWillAppear(true);
				locationView.ViewWillDisappear(true);
				
				mapView.View.RemoveFromSuperview();
				this.View.AddSubview(locationView.View);
				
				locationView.ViewDidDisappear(true);
				mapView.ViewDidAppear(true);
			}
			UIView.CommitAnimations();
		}
	}
}
