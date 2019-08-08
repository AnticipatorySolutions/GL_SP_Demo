﻿using System;

namespace GL_SP_Demo_Graph.Utilities
{
    /// <summary>
    /// The distance type to return the results in.
    /// </summary>
    public enum DistanceType { Miles, Kilometers };

    /// <summary>
    /// Specifies a Latitude / Longitude point.
    /// </summary>
    public class Position
    {
        public Position(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }
        public double Latitude;
        public double Longitude;
    }

    public interface IHaversine
    {
        double Distance(Position pos1, Position pos2, DistanceType type);
    }

    public class Haversine : IHaversine
    {
        /// <summary>
        /// Returns the distance in miles or kilometers of any two
        /// latitude / longitude points.
        /// </summary>
        /// <param name=”pos1″></param>
        /// <param name=”pos2″></param>
        /// <param name=”type”></param>
        /// <returns></returns>
        public double Distance(Position pos1, Position pos2, DistanceType type)
        {
            double R = (type == DistanceType.Miles) ? 3960 : 6371;
            double sLat = pos2.Latitude - pos1.Latitude;
            double sLon = pos2.Longitude - pos1.Longitude;
            double dLat = toRadian(sLat);
            double dLon = toRadian(sLon);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(toRadian(pos1.Latitude)) * Math.Cos(toRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d;
        }

        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name=”val”></param>
        /// <returns></returns>
        private double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}