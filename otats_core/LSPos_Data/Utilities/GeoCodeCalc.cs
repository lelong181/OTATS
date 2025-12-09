using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GeoCodeCalc
/// </summary>
public static class GeoCodeCalc
{
    public const double EarthRadiusInMiles = 3956.0;
    public const double EarthRadiusInKilometers = 6367.0;

    public static double ToRadian(double val) { return val * (Math.PI / 180); }
    public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

    public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
    {
        return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Miles);
    }

    public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
    {
        double radius = GeoCodeCalc.EarthRadiusInMiles;

        if (m == GeoCodeCalcMeasurement.Kilometers) { radius = GeoCodeCalc.EarthRadiusInKilometers; }
        return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
    }

     
    public static double GetAzimuth(double inputlat1, double inputlng1, double inputlat2, double inputlng2)
    {
        double deltaY = inputlng2 - inputlng1;
        double deltaX = inputlat2 - inputlat1;
        return ((Math.Atan2(deltaY, deltaX) * 180) / Math.PI);
    }
    public static double DegToRad(double deg) { return (deg / 180.0 * Math.PI); }
    public static double RadToDeg(double rad) { return (rad / Math.PI * 180.0); }
}

public enum GeoCodeCalcMeasurement : int
{
    Miles = 0,
    Kilometers = 1
}