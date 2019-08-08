using System;
using System.Collections.Generic;
using System.Linq;
// using GeoCoordinatePortable;  // - Recruiter Discouraged Use of Libraries 
using GL_SP_Demo_Graph.Utilities;

namespace GL_SP_Demo_Graph
{
    public class Edge
    {
        public Node Source;
        public Node Destination;
        public int Distance = 0; //Weight = distance in km
        public Edge(Node src, Node dst, IHaversine haversine)
        {

            Source = src;
            Destination = dst;
            //Distance = (int)(src.Location.GetDistanceTo(dst.Location)); // Put it back later! GeoCoordinates is fun, look into course as heuristic during search.
            //Casting because the extra precision isn't necessary... it might be now... 
            Distance = (int)haversine.Distance(src.Location,dst.Location, DistanceType.Kilometers); 
        }
    }
}


