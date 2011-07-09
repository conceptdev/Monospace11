using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Diagnostics;

namespace Monospace11
{
	/// <summary>
	/// Starting point for our MonoTouch application. Specifies the AppDelegate to load to kick things off
	/// </summary>
	public class Application
    {
        static void Main (string[] args)
        {
			try
			{
            	UIApplication.Main (args, "ConferenceApplication", "AppDelegate");
			}
			catch (Exception ex)
			{	// HACK: this is just here for debugging
				Debug.WriteLine(ex);
			}
        }
    }
	
	[Register ("ConferenceApplication")]
	public class ConferenceApplication : UIApplication {
		public static WeakReference cref;
		
		public static UIViewController CurrentController {
			get {
				if (cref != null && cref.IsAlive)
					return (UIViewController) cref.Target;
				
				return null;
			}
			set {
				cref = new WeakReference (value);
			}
		}
	}
}