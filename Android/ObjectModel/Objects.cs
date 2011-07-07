using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Android.Runtime;

namespace ConfXml
{
    [Preserve]
    public class DataVersion
    {
        public DataVersion() { }
        public int Number { get; set; }
        public string DataUrl { get; set; }
    }

    [Preserve]
    [System.Diagnostics.DebuggerDisplay("Conference {Name} {Speakers.Count}")]
    public class Conference : ConferenceBase
    {
        public Conference()
        {
            Speakers = new List<Speaker>();
            Sessions = new List<Session2>();
            Tags = new List<Tag>();
            Days = new List<Day>();
        }
        public Conference(ConferenceBase cb)
            : base(cb)
        {
            Speakers = new List<Speaker>();
            Sessions = new List<Session2>();
            Tags = new List<Tag>();
            Days = new List<Day>();
        }
        [XmlElement("da")]
        public List<Day> Days { get; set; }
        [XmlElement("sp")]
        public List<Speaker> Speakers { get; set; }
        [XmlElement("se")]
        public List<Session2> Sessions { get; set; }
        [XmlElement("ta")]
        public List<Tag> Tags { get; set; }

    }
    [Preserve]
    public class Day
    {
        public Day()
        {
            Timeslots = new List<Timeslot>();
        }
        [XmlAttribute("d")]
        public DateTime Date { get; set; }
        [XmlElement("ts")]
        public List<Timeslot> Timeslots { get; set; }
    }
    [Preserve]
    public class Timeslot
    {
        public Timeslot()
        {
            Sessions = new List<Session>();
        }
        public Timeslot(Day day, int startHour, int startMinute, int endHour, int endMinute, string title, string brief)
            : this()
        {
            StartTime = new DateTime(day.Date.Year, day.Date.Month, day.Date.Day, startHour, startMinute, 00);
            EndTime = new DateTime(day.Date.Year, day.Date.Month, day.Date.Day, endHour, endMinute, 00);
            Title = title;
            Brief = brief;
            HasSessions = false;
        }
        public Timeslot(Day day, int startHour, int startMinute, int endHour, int endMinute, string title, string brief, bool hasSessions)
            : this(day, startHour, startMinute, endHour, endMinute, title, brief)
        {
            HasSessions = hasSessions;
        }
        [XmlAttribute("s")]
        public DateTime StartTime { get; set; }
        [XmlAttribute("e")]
        public DateTime EndTime { get; set; }
        [XmlAttribute("t")]
        public string Title { get; set; }
        [XmlElement("b")]
        public string Brief { get; set; }
        [XmlElement("se")]
        public List<Session> Sessions { get; set; }
        [XmlAttribute("h")]
        public bool HasSessions { get; set; }
    }

    [Preserve]
    [System.Diagnostics.DebuggerDisplay("Speaker {Name}")]
    public class Speaker
    {
        public Speaker()
        {
            Sessions = new List<Session>();
        }
        public Speaker(Speaker2 s1)
        {
            this.Name = s1.Name;
            this.Bio = s1.Bio;
            this.Twitter = s1.Twitter;
        }
        [XmlAttribute("n")]
        public string Name { get; set; }
        [XmlAttribute("b")]
        public string Bio { get; set; }
        [XmlAttribute("t")]
        public string Twitter { get; set; }
        [XmlElement("se")]
        public List<Session> Sessions { get; set; }
    }

    [Preserve]
    [System.Diagnostics.DebuggerDisplay("Session {Title}")]
    public class Session
    {
        public Session()
        {
            Speakers = new List<Speaker>();
            Tags = new List<Tag>();
        }
        [XmlAttribute("c")]
        public string Code { get; set; }
        [XmlAttribute("sd")]
        public DateTime Start;
        [XmlAttribute("ed")]
        public DateTime End;
        [XmlAttribute("r")]
        public string Room { get; set; }
        [XmlAttribute("t")]
        public string Title { get; set; }
        [XmlAttribute("b")]
        public string Brief { get; set; }
        [XmlElement("sp")]
        public List<Speaker> Speakers { get; set; }
        [XmlElement("t")]
        public List<Tag> Tags { get; set; }
        /// <summary>Clone does NOT include Speakers or Tags</summary>
        public Session Clone()
        {
            return new Session { Title = Title, Brief = Brief, Code = Code, Room = Room, Start = Start, End = End };
        }



        public string DateTimeDisplay
        {
            get
            {
                if (End > DateTime.MinValue)
                    return Start.ToString("ddd MMMM d, H:mm") + " - " + End.ToString("H:mm");
                else
                    return Start.ToString("ddd MMMM d, H:mm");
            }
        }
        public string DateTimeQuickJumpDisplay
        {
            get
            {
                return Start.ToString("ddd MMM d, H:mm");
            }
        }
        public string TimeQuickJumpDisplay
        {
            get
            {
                return Start.ToString("ddd H:mm");
            }
        }
        public string DateTimeQuickJumpSubtitle
        {
            get
            {
                string output = "";
                if (!String.IsNullOrEmpty(Room)) output = Room + "; ";

                return output + SpeakerList;
            }
        }

        public string FavoritesSubtitle
        {
            get
            {
                string output = "";
                if (!String.IsNullOrEmpty(Room)) output = Room + "; ";

                return output + Start.ToString("ddd H:mm");
            }
        }

        public string GetSpeakerList()
        {
            string speakers = "";
            foreach (var s in Speakers)
            {
                speakers += ", " + s.Name;
            }
            return speakers.Trim(',', ' ');
        }
        public string SpeakerList
        {
            get { return GetSpeakerList(); }
        }
        public bool HasTag(string tag)
        {
            foreach (var t in Tags)
                if (t.Value == tag) return true;
            return false;
        }
        public string GetTagList()
        {
            string tags = "";
            foreach (var t in Tags)
            {
                tags += ", " + t.Value;
            }
            return tags.Trim(',', ' ');
        }
        public string TagList
        {
            get { return GetTagList(); }
        }
    }

    [Preserve]
    [System.Diagnostics.DebuggerDisplay("Tag {Value}")]
    public class Tag
    {
        public Tag()
        {
            Sessions = new List<Session>();
        }
        [XmlAttribute("v")]
        public string Value { get; set; }
        [XmlElement("se")]
        public List<Session> Sessions { get; set; }
    }

    [Preserve]
    [System.Diagnostics.DebuggerDisplay("MapLocation {Title} {Location}")]
    public class MapLocation
    {
        [XmlAttribute("t")]
        public string Title { get; set; }
        [XmlAttribute("s")]
        public string Subtitle { get; set; }
        [XmlElement("l")]
        public Point Location { get; set; }
    }

    [Preserve]
    public class Point
    {
        public Point() { }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
