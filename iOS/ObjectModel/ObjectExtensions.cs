using System;
using MonoTouch.CoreLocation;
using MIX10Xml;

namespace Monospace11
{
	public static class ObjectExtensions
	{
		public static CLLocationCoordinate2D To2D (this System.Drawing.PointF point)
		{
			return new CLLocationCoordinate2D(point.Y, point.X);
		}
		public static CLLocationCoordinate2D To2D (this MIX10Xml.Point point)
		{
			return new CLLocationCoordinate2D(point.Y, point.X);
		}
		public static string ToLL (this CLLocationCoordinate2D point)
		{
			return string.Format("{0},{1}", point.Latitude, point.Longitude);
		}
	}
}
