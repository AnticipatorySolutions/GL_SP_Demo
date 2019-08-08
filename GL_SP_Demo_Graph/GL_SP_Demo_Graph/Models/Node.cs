using System;
using System.Collections.Generic;
using System.Linq;
// using GeoCoordinatePortable; //Recruiter Discouraged the Use of Libraries
using GL_SP_Demo_Graph.Utilities;

namespace GL_SP_Demo_Graph
{
    public class Node
    {
        public string Name;
        public string Code2;
        public string Code3;
        public bool Inboud = false;
        public bool Outbound = false;
        public int Region = 0;
        //public GeoCoordinate Location;
        public Position Location;
        public Dictionary<string, Edge> Edges = new Dictionary<string, Edge>();
        public Dictionary<string, List<string>> ShortestPath = new Dictionary<string, List<string>>();
        private IHaversine Haversine;

        public Node(string code3, IHaversine haversine)
        {
            Code3 = code3;
            Haversine = haversine;
        }

        public bool AddEdge(Node src, Node dst)
        {
            if (!Edges.ContainsKey(dst.Code3))
            {
                Edges.Add(dst.Code3, new Edge(src, dst, Haversine));
                Outbound = true;
                Edges[dst.Code3].Destination.Inboud = true;

                return true;
            }
            return false;
        }

        public void SetRegion(int region) { Region = region; }
    }
}
