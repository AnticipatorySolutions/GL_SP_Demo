using System;
using GL_SP_Demo_Graph.Utilities;

namespace GL_SP_Demo_Graph
{
    public interface IGL_Graph
    {
        string Read(string src, string dst);
        string ReadGraph(string src, string dst);
    }

    public class GL_SP_Route_Graph : IGL_Graph
    {
        IGraph Graph;
        public GL_SP_Route_Graph(IHaversine haversine)
        {
            Graph = new Graph(haversine);
        }

        public string Read(string src, string dst)
        {
            try
            {
                return Graph.Read(src, dst);
            }
            catch (Exception error)
            {
                return $"{error.Source} - {error.Message} - {error.StackTrace}";
            }
        }

        public string ReadGraph(string src, string dst)
        {
            try
            {
                return Graph.ReadGraph(src, dst);
            }
            catch (Exception error)
            {
                return $"{error.Source} - {error.Message} - {error.StackTrace}";
            }
        }

    }
}
