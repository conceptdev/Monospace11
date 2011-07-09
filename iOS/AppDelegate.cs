using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using ConceptDevelopment;
using MIX10Xml;
using System.Drawing;
using MonoTouch.ObjCRuntime;
using System.Threading;
using System.Diagnostics;

namespace Monospace11
{
	/// <summary>
	/// ROOT of this application; referenced in "Main.cs"
	/// </summary>
	[Register ("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;
		UIImageView splashView;
		
		TabBarController tabBarController;
		public static UserDatabase UserData {get; private set;} 
		public static Conference ConferenceData {get;private set;}
		public static Conference2 ConferenceData2 {get;private set;}
		/// <summary>conf.xml</summary>
		public static string XmlDataFilename = "conf.xml";
		/// <summary>userdata.db</summary>
		public static string SqliteDataFilename = "userdata.db";
		
		/// <summary>
		/// Loads the best conf.xml it can find - first look in SpecialFolder 
		/// (if not there, load the one that was included in the app download)
		/// </summary>
		/// <remarks>
		/// I wonder if there could be a problem with newer app code trying to 
		/// open an older Xml after an upgrade is installed? 
		/// I guess newer apps that aren't backward compatible
		/// should use a different filename eg. conf2.xml...
		/// </remarks>
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
			// setup SQLite for 'starred sessions' database
			var basedir = Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			UserData = new UserDatabase(Path.Combine (basedir, SqliteDataFilename));

			#region Get All Session data...
			
			string xmlPath = XmlDataFilename; // the 'built in' version

				// version 2
				xmlPath = XmlDataFilename; // the 'built in' version
				if (File.Exists(Path.Combine(basedir, XmlDataFilename)))
				{	// load a newer copy
					xmlPath = Path.Combine(basedir, XmlDataFilename);
				}
	
				long start = DateTime.Now.Ticks;
				using (TextReader reader = new StreamReader(xmlPath))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Conference2));
					ConferenceData2 = (Conference2)serializer.Deserialize(reader);
					
					// Version 2 'flat' data structure
					var sessDic2 = (from s2 in ConferenceData2.Sessions
								select s2).ToDictionary(item => item.Code);
					var speaDic2 = (from s3 in ConferenceData2.Speakers
								select s3).ToDictionary(item => item.Name);
//					var tagDic2 = (from s3 in ConferenceData2.Tags
//								select s3).ToDictionary(item => item.Value);
		
					// dictionaries to re-constitute version 1 data structure
					var speaDic1 = new Dictionary<string, MIX10Xml.Speaker>();
					var sessDic1 = new Dictionary<string, MIX10Xml.Session>();
					var tagDic1  = new Dictionary<string, MIX10Xml.Tag>();
					
					// create version 1 speakers
					foreach (var sp2 in speaDic2)
					{
						MIX10Xml.Speaker sp1 = sp2.Value as MIX10Xml.Speaker;
						speaDic1.Add(sp1.Name, sp1);
					}
					// create version 1 sessions
					// add sessions to version 1 tags
					// add sessions to version 1 speakers
					foreach (var se2 in sessDic2.Values)
					{
						MIX10Xml.Session se1 = se2 as MIX10Xml.Session;
						sessDic1.Add(se1.Code, se1);
						foreach (var ta2 in se2.TagStrings)
						{
							if (!tagDic1.Keys.Contains(ta2))
							{
								tagDic1.Add(ta2,new Tag{Value=ta2});
							}
							tagDic1[ta2].Sessions.Add(se1);
							se1.Tags.Add(tagDic1[ta2]);
						}
						// add speakers to version 1 sessions
						foreach (var spn in se2.SpeakerNames)
						{ Console.WriteLine(spn);
							se1.Speakers.Add(speaDic1[spn]);
							speaDic1[spn].Sessions.Add(se1);
						}
					}
					// push into version 1 data structure, which rest of the app uses
					ConferenceData = new Conference(ConferenceData2);
					ConferenceData.Speakers = speaDic1.Values.ToList();
					ConferenceData.Sessions = sessDic1.Values.ToList();
					ConferenceData.Tags = tagDic1.Values.ToList();
				}
			
			#endregion
			
			// Create the tab bar
			tabBarController = new Monospace11.TabBarController ();
			// Create the main window and add the navigation controller as a subview
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.AddSubview(tabBarController.View);
			window.MakeKeyAndVisible ();
			showSplashScreen();
			
            return true;
		}
		
		void showSplashScreen ()
		{
			splashView = new UIImageView(new RectangleF(0f, 0f, 320f, 480f));
			splashView.Image = UIImage.FromFile("Default.png");
			window.AddSubview(splashView);
			window.BringSubviewToFront(splashView);
			UIView.BeginAnimations("SplashScreen");
			UIView.SetAnimationDuration(0.5f);
			UIView.SetAnimationDelegate(this);
			UIView.SetAnimationTransition(UIViewAnimationTransition.None, window, true);
			UIView.SetAnimationDidStopSelector(new Selector("completedAnimation"));
		    splashView.Alpha = 0f;
		    splashView.Frame = new RectangleF(-60f, -60f, 440f, 600f);
		    UIView.CommitAnimations();
		}

		[Export("completedAnimation")]
		void StartupAnimationDone()
		{
			Debug.WriteLine ("Done");
			splashView.RemoveFromSuperview();
			splashView.Dispose();
		}

		
		public static void GetCellSelectedColor(UITableViewCell cell)
		{
			using (var v = new UIView(cell.Frame))
			{
				var LightBlue = new UIColor(0.29f, 0.50f, 0.53f, 255.0f);
				v.BackgroundColor = LightBlue;
				cell.SelectedBackgroundView = v;
			}
		}
		
        // This method is allegedly required in iPhoneOS 3.0
        public override void OnActivated (UIApplication application)
        {
        }
    }
}