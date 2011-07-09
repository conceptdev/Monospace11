using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MIX10Xml
{
   public class DataVersion2
   {
      public DataVersion2() {}
      public int Number { get; set; }
      public string DataUrl { get; set; }
      public DateTime LastUpdated { get; set; }
   }
   public class ConferenceBase
   {
      public ConferenceBase()
      {
         Locations = new List<MapLocation>();
      }
      public ConferenceBase(ConferenceBase cb)
      {
         this.Version = cb.Version;
         this.Name = cb.Name;
         this.BaseUrl = cb.BaseUrl;
         this.AboutHtml = cb.AboutHtml;
         this.Location = cb.Location;
         this.Locations = cb.Locations;
      }
      [XmlAttribute("v")]
      public int Version { get; set; }
      [XmlAttribute("n")]
      public string Name { get; set; }
      [XmlAttribute("url")]
      public string BaseUrl { get; set; }
      [XmlAttribute("abt")]
      public string AboutHtml { get; set; }
      [XmlElement("lo")]
      public MapLocation Location { get; set; }
      [XmlElement("locs")]
      public List<MapLocation> Locations { get; set; }


   }
   public class Conference2 : ConferenceBase
   {
      public Conference2() { }
      public Conference2(ConferenceBase cb) :base (cb)
      {
         Speakers = new List<Speaker2>();
         Sessions = new List<Session2>();
         Tags = new List<Tag2>();
      }
      [XmlElement("sp")]
      public List<Speaker2> Speakers { get; set; }
      [XmlElement("se")]
      public List<Session2> Sessions { get; set; }
      [XmlElement("t")]
      public List<Tag2> Tags { get; set; }
   }
   public class Speaker2 : Speaker
   {
      public Speaker2() { }
      [XmlElement("c")]
      public List<string> SessionCodes { get; set; }
   }
   public class Session2 : Session
   {
      public Session2() { }
      public Session2(Session s1)
      {
         this.Code = s1.Code;
         this.Brief = s1.Brief;
         this.Start = s1.Start;
         this.End = s1.End;
         this.Room = s1.Room;
         this.Title = s1.Title;
         this.Url = s1.Url;
         this.Tags = s1.Tags;

         SpeakerNames = new List<string>();
         TagStrings = new List<string>();
      }
      public List<string> SpeakerNames { get; set; }
      public List<string> TagStrings { get; set; }
   }
   public class Tag2
   {
      public Tag2()
      {
         SessionCodes = new List<string>();
      }
      [XmlAttribute("v")]
      public string Value { get; set; }
      [XmlElement("se")]
      public List<string> SessionCodes { get; set; }
   }

}
