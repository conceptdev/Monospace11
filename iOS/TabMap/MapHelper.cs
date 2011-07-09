using System;
using MonoTouch.CoreLocation;

namespace Monospace11
{
	public static class MapExtensions{
		
		public static double ToRadian(this double d)
		{
			return d * (Math.PI /180);
		}
		public static double ToDegree(this double r)
		{
			return r * (180 / Math.PI);
		}
	}
	public enum UnitsOfLength
	{Miles,Kilometer,NauticalMiles }

	
	/// <remarks>
	/// Code from
	/// http://bryan.reynoldslive.com/post/Latitude2c-Longitude2c-Bearing2c-Cardinal-Direction2c-Distance2c-and-C.aspx
	/// </remarks>
	public class MapHelper
	{
		private static double _MilesToKilometers = 1.609344 ;
		private static double _MilesToNautical = 0.868976242;
		public MapHelper ()
		{}
		
		/// <summary>
		/// Calculates the distance between two points of latitude and longitude.
		/// Great Link - http://www.movable-type.co.uk/scripts/latlong.html
		/// </summary>
		/// <param name="coordinate1">First coordinate.</param>
		/// <param name="coordinate2">Second coordinate.</param>
		/// <param name="unitsOfLength">Sets the return value unit of length.</param>
		public static Double Distance(Coordinate coordinate1, Coordinate coordinate2, UnitsOfLength unitsOfLength)
		{
		   var theta = coordinate1.Longitude - coordinate2.Longitude;
		   var distance = Math.Sin(coordinate1.Latitude.ToRadian()) * Math.Sin(coordinate2.Latitude.ToRadian()) +
		                  Math.Cos(coordinate1.Latitude.ToRadian()) * Math.Cos(coordinate2.Latitude.ToRadian()) *
		                  Math.Cos(theta.ToRadian());
		
		   distance = Math.Acos(distance);
		   distance = distance.ToDegree();
		   distance = distance * 60 * 1.1515;
		
		   if (unitsOfLength == UnitsOfLength.Kilometer)
		       distance = distance * _MilesToKilometers;
		   else if (unitsOfLength == UnitsOfLength.NauticalMiles)
		       distance = distance * _MilesToNautical;
		
		   return (distance);
		}
		
	}
	public class Coordinate
      {
          private double latitude, longitude;
    
			public Coordinate(){}
			
			public Coordinate(CLLocationCoordinate2D mapCoord)
			{
				latitude = mapCoord.Latitude;
				longitude = mapCoord.Longitude;
			}
			
          /// <summary>
          /// Latitude in degrees. -90 to 90
          /// </summary>
          public Double Latitude
          {
              get { return latitude; }
              set
              {
                  if (value > 90) throw new ArgumentOutOfRangeException("value", "Latitude value cannot be greater than 90.");
                  if (value < -90) throw new ArgumentOutOfRangeException("value", "Latitude value cannot be less than -90.");
                  latitude = value;
              }
          }
   
          /// <summary>
          /// Longitude in degree. -180 to 180
         /// </summary>
          public Double Longitude
          {
              get { return longitude; }
              set
             {
                  if (value > 180) throw new ArgumentOutOfRangeException("value", "Longitude value cannot be greater than 180.");
                  if (value < -180) throw new ArgumentOutOfRangeException("value", "Longitude value cannot be less than -180.");
                  longitude = value;
              }
          }
      }
}
