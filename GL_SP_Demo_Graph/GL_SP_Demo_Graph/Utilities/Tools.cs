using System;
using System.Collections.Generic;
using System.Linq;

namespace GL_SP_Demo_Graph
{

    public class FlowData
    {
        public string MaxOut = "YYZ";
        public Dictionary<string, int> Inbound = new Dictionary<string, int>();
        public Dictionary<string, int> Outbound = new Dictionary<string, int>();
        public Dictionary<string, int> AllNodes = new Dictionary<string, int>();
        public Dictionary<string, int> Edges = new Dictionary<string, int>();
    }

    public class ReferenceData
    {
        //Name,City,Country,IATA 3,Latitute,Longitude
        public Dictionary<string, string> Names = new Dictionary<string, string>();
        public Dictionary<string, string> City = new Dictionary<string, string>();
        public Dictionary<string, string> Countries = new Dictionary<string, string>();
        public Dictionary<string, string> Longitudes = new Dictionary<string, string>();
        public Dictionary<string, string> Latitutes = new Dictionary<string, string>();
        public Dictionary<string, string> Code3 = new Dictionary<string, string>();

    }

    

    public class MetaData
    {
        public Dictionary<string, Dictionary<string,string>> ShortestPath = new Dictionary<string, Dictionary<string,string>>();
    }

    public class GraphConverter
    {
        public List<string> Convert(string start, string finish, Dictionary<string, Node> vertices)
        {
            var previous = new Dictionary<string, string>();
            var distances = new Dictionary<string, int>();
            var nodes = new List<string>();

            List<string> path = new List<string>();


            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 1;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }
                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest].Edges)
                {
                    var alt = distances[smallest] + neighbor.Value.Distance;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }
            path.Add(start);

            return path;
        }
    }

    public class MetaParser
    {

        string hardFileName = @"..\GL_SP_Demo_Graph\GL_SP_Demo_Graph\Data\ShortestPaths\";

        public MetaData Flow()
        {
            MetaData tracking = new MetaData();
            Reader fileReader = new Reader();
            ReaderStore readerStore = new ReaderStore();

            var pathArray = System.IO.Directory.GetFiles(hardFileName);
            foreach (string path in pathArray)
            {
                var r = fileReader.Read(readerStore.CSV_Reader_Headless, path);
                var s = r.FileValues["H1"][0].Remove(3);
                tracking.ShortestPath.Add(s, new Dictionary<string, string>());

                for (var i = 0; i < r.FileValues["H1"].Count; i++)
                {
                    tracking.ShortestPath[s].Add(r.FileValues["H1"][i].Substring(4), r.FileValues["H2"][i]);
                }
            }
            return tracking;
        }
    }

    public class ReferenceParser
    {
        string hardFileName = @"..\GL_SP_Demo_Graph\GL_SP_Demo_Graph\Data\airports.csv"; //Will Not Deploy!!!

        public ReferenceData Flow()
        {
            ReferenceData tracking = new ReferenceData();
            Reader fileReader = new Reader();
            ReaderStore csvReaderStore = new ReaderStore();

            var r = fileReader.Read(csvReaderStore.CSV_Reader_FullHead, hardFileName);
            for (var i = 0; i < r.FileValues[r.Headers[3]].Count; i++)
            {
                if (!tracking.Names.ContainsKey(r.FileValues[r.Headers[3]][i]))
                {
                    tracking.Names.Add(r.FileValues[r.Headers[3]][i], r.FileValues[r.Headers[0]][i]);
                    tracking.City.Add(r.FileValues[r.Headers[3]][i], r.FileValues[r.Headers[1]][i]);
                    tracking.Countries.Add(r.FileValues[r.Headers[3]][i], r.FileValues[r.Headers[2]][i]);
                    tracking.Code3.Add(r.FileValues[r.Headers[3]][i], r.FileValues[r.Headers[3]][i]);
                    tracking.Latitutes.Add(r.FileValues[r.Headers[3]][i], r.FileValues[r.Headers[4]][i]);
                    tracking.Longitudes.Add(r.FileValues[r.Headers[3]][i], r.FileValues[r.Headers[5]][i]);
                }
            }
            return tracking;
        }
    }

    public class DataParser
    {
        string hardFileName = @"..\GL_SP_Demo_Graph\GL_SP_Demo_Graph\Data\routes.csv";

        public FlowData Flow()
        {
            FlowData tracking = new FlowData();
            Reader fileReader = new Reader();
            ReaderStore csvReaderStore = new ReaderStore();

            var r = fileReader.Read(csvReaderStore.CSV_Reader_FullHead, hardFileName);
            for (var i = 0; i < r.FileValues[r.Headers[0]].Count; i++)
            {
                var airline = r.FileValues[r.Headers[0]][i];
                var outbound = r.FileValues[r.Headers[1]][i];
                var inbound = r.FileValues[r.Headers[2]][i];

                if (!tracking.Outbound.ContainsKey(outbound))
                {
                    tracking.Outbound.Add(outbound, 1);
                }
                else
                {
                    tracking.Outbound[outbound]++;
                }

                if (!tracking.Inbound.ContainsKey(inbound))
                {
                    tracking.Inbound.Add(inbound, 1);
                }
                else
                {
                    tracking.Inbound[inbound]++;
                }

                if (!tracking.Edges.ContainsKey($"{outbound}|{inbound}"))
                {
                    tracking.Edges.Add($"{outbound}|{inbound}", 1);
                }
                else
                {
                    tracking.Edges[$"{outbound}|{inbound}"]++;
                }
            }

            foreach (var entry in tracking.Inbound)
            {
                if (!tracking.AllNodes.ContainsKey(entry.Key))
                {
                    tracking.AllNodes.Add(entry.Key, entry.Value);
                }
            }

            foreach (var entry in tracking.Outbound)
            {
                if (!tracking.AllNodes.ContainsKey(entry.Key))
                {
                    tracking.AllNodes.Add(entry.Key, entry.Value);
                }
                else
                {
                    tracking.AllNodes[entry.Key] += entry.Value;
                }
            }

            var src = tracking.Outbound.OrderByDescending(x => x.Value);
            tracking.MaxOut = src.First().Key;
            return tracking;
        }
    }


}
