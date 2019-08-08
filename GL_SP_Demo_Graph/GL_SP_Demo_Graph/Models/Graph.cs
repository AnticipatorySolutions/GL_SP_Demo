using System;
using System.Collections.Generic;
using System.Linq;
// using GeoCoordinatePortable; //Recruite Discouraged the Use of Libraries 
using GL_SP_Demo_Graph.Utilities;

namespace GL_SP_Demo_Graph
{
    public interface IGraph
    {
        string Read(string src, string dst);
        string ReadGraph(string src, string dst);
    }

    public class Graph : IGraph
    {
        public GraphHandler GraphHandler;
        public Graph(IHaversine haversine)
        {            
            GraphHandler = new GraphHandler(haversine);
        }
        public string Read(string src, string dst)
        {
            return GraphHandler.Read_Repo(src, dst);
        }
        public string ReadGraph(string src, string dst)
        {
            return GraphHandler.Read_Graph(src, dst);
        }
    } 

    public class GraphHandler
    {
        public FlowData Data;
        public ReferenceData RefData;
        public MetaData Meta;
        public GraphCore GraphCore;

        public GraphHandler(IHaversine haversine)
        {
            DataParser x = new DataParser();
            Data = x.Flow();

            ReferenceParser refParser = new ReferenceParser();
            RefData = refParser.Flow();
            string directoryPath = @"C:\netProjects\AirlineGraph\shortestPaths";
            var data = System.IO.Directory.GetFiles(directoryPath);

            MetaParser metaParser = new MetaParser();
            Meta = metaParser.Flow();

            GraphCore = new GraphCore(Data, RefData, haversine);
        }

        public string Read_Graph(string src, string dst)
        {
            if (!GraphCore.HasNode(src))
            {
                return "Unfortunately the src node wasn't found.";
            }
            if (!GraphCore.HasNode(dst))
            {
                return "Unfortunately the dst node wasn't found.";
            }

            return GraphCore.GetShortestPath(src,dst);
        }

        public string Read_Repo(string src, string dst)
        {
            if (!Meta.ShortestPath.ContainsKey(src))
            {
                return "Unfortunately the src node wasn't found.";
            }
            if (!Meta.ShortestPath[src].ContainsKey(dst))
            {
                return "Unfortunately the dst node wasn't found.";
            }

            var shortPath = Meta.ShortestPath[src][dst];
            
            var shortPathSplit = shortPath.Split("|");
            
            var invertPath = shortPathSplit.Reverse();
            var returnValue = "|";
            foreach (string path in invertPath)
            {
                returnValue += $"{path}|";
            }
            return returnValue;             
        }
    }


    public class GraphCore
    {
        public Dictionary<string, Node> Nodes = new Dictionary<string, Node>(); //this should be private at some point

        private FlowData fd;
        private ReferenceData rd;
        private IHaversine Haversine;

        public GraphCore(FlowData data, ReferenceData refData, IHaversine haversine)
        {
            Haversine = haversine;
            fd = data;
            rd = refData;
            HydrateGraph();
        }

        public bool HasNode(string code3)
        {
            if (!Nodes.ContainsKey(code3)) { return false; }
            return true;
        }

        public string GetShortestPath(string src, string dst)
        {
            GraphConverter GraphConverter = new GraphConverter();
            string ReturnValue = "|";
            var ShortestPath =  GraphConverter.Convert(src, dst, Nodes);

            if (ShortestPath.Count > 1)
            {
                for (var i = ShortestPath.Count - 1; i >= 0; i--)
                {
                    ReturnValue += ShortestPath[i] + "|";
                }
            }
            else {
                ReturnValue = $"Unfortunately you cannot reach {dst} from {src}";
            }
            return ReturnValue;
        }

        private void HydrateGraph()
        {
            AddNodes();
            AddEdges();
        }

        private void AddNodes()
        {
            foreach (var entry in fd.AllNodes)
            {
                if (!Nodes.ContainsKey(entry.Key))
                {
                    Nodes.Add(entry.Key, new Node(entry.Key, Haversine));
                    double.TryParse(rd.Latitutes[entry.Key], out double lat);
                    double.TryParse(rd.Longitudes[entry.Key], out double lon);
                    Nodes[entry.Key].Location = new Position(lat, lon);
                }
            }
        }

        private void AddEdges()
        {
            foreach (var entry in fd.Edges)
            {
                var x = entry.Key.Split('|');
                Nodes[x[0]].AddEdge(Nodes[x[0]], Nodes[x[1]]);
            }
        }
    }
}
