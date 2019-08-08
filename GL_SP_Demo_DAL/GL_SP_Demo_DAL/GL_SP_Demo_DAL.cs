using System;
using GL_SP_Demo_Graph;

namespace GL_SP_Demo_DAL.Route
{
    public interface IGL_DAL
    {
        string Read(string src, string dst);
        string ReadGraph(string src, string dst);
    }

    public class GL_SP_Route_DAL : IGL_DAL
    {
        private readonly IGL_Graph _GL_Graph;

        public GL_SP_Route_DAL(IGL_Graph gl_graph)
        {
            _GL_Graph = gl_graph;
        }

        public string ReadGraph(string src, string dst)
        {
            try
            {
                return _GL_Graph.ReadGraph(src, dst);
            }
            catch (Exception exception)
            {
                return $"GL_Graph Exception {exception.Message} {exception.Source} {exception.StackTrace}";
            }
        }

        public string Read(string src, string dst)
        {
            try
            {
                return _GL_Graph.Read(src, dst);
            }
            catch (Exception exception)
            {
                return $"GL_Graph Exception {exception.Message} {exception.Source} {exception.StackTrace}";
            }
        }
    }
}
