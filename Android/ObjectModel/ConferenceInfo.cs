using System;

namespace Conf
{
	public class ConferenceInfo
	{
		public ConferenceInfo ()
		{
		}

		public string DisplayName {get;set;}
		public string DisplayLocation {get;set;}
		
		public string Code {get;set;}
		public DateTime StartDate {get;set;}
		public DateTime EndDate {get;set;}
	}
}

