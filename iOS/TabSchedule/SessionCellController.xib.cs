
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Monospace11
{
	public partial class SessionCellController : UIViewController
	{
		#region Constructors

		// The IntPtr and NSCoder constructors are required for controllers that need 
		// to be able to be created from a xib rather than from managed code

		public SessionCellController (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public SessionCellController (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public SessionCellController ()
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
		
		#endregion
		
		
		public string SessionTitle
		{
		get { return labelTitle.Text; }
		set { labelTitle.Text = value; }
		}
		public string Subtitle
		{
		get { return labelSubtitle.Text; }
		set { labelSubtitle.Text = value; }
		}
		
		public string Time
		{
		get { return labelTime.Text; }
		set { labelTime.Text = value; }
		}
		public string EndTime
		{
		get { return labelTimeEnd.Text; }
		set { labelTimeEnd.Text = value; }
		}
		public UITableViewCell Cell
		{
		get { return cell; }
		}
		
	}
}
