using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MIX10Xml;
namespace Monospace11
{
	public class TimeslotViewController : WebViewControllerBase
	{
		Timeslot _slot;
		public TimeslotViewController (Timeslot slot) : base()
		{
			_slot = slot;
		}
		public void Update (Timeslot slot)
		{
			_slot = slot;
			LoadHtmlString(FormatText());
		}
		protected override string FormatText()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(StyleHtmlSnippet);
			sb.Append("<h2>"+ _slot.Title+"</h2>"+ Environment.NewLine);
			sb.Append("<b style='color:#666666'>"+_slot.StartTime.ToUniversalTime().ToString("HH:mm")+" - " 
			          +_slot.EndTime.ToUniversalTime().ToString("HH:mm")+"</b><br/>"+ Environment.NewLine);
			//sb.Append("<i style='color:#666666'>"+_session.Location+"</i><br/>"+ Environment.NewLine);
			sb.Append("<span class='body'>"+_slot.Brief+"</span>"+ Environment.NewLine);
			return sb.ToString();
		}
	}
}