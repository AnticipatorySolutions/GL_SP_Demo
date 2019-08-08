using Xunit;
using Xunit.Abstractions;
using GL_SP_Demo_DAL.Route;
using GL_SP_Demo_Graph;

namespace GL_SP_Demo_DAL.Tests.Route
{
    public class Graph_Mock : IGL_Graph
    {
        public string Read(string src, string dst)
        {
            return "Success";
        }
        public string ReadGraph(string src, string dst)
        {
            return "Success";
        }
    }

    public class RouteDALTests
    {
        private readonly ITestOutputHelper output;

        private readonly IGL_DAL DAL;
        private readonly IGL_Graph Graph;
     
        public RouteDALTests(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
            Graph = new Graph_Mock();
            DAL = new GL_SP_Route_DAL(Graph);
        }

        [Theory]
        [InlineData("YYZ", "DEN")]
        public void GetShortestRoute_ProvideAllParams_ConfirmReturn(string src, string dst)
        {
            string result;
            result = DAL.Read(src,dst);
            Assert.IsType<string>(result);
            Assert.True(result == "Success");
        }

        [Theory]
        [InlineData("YYZ", "DEN")]
        public void GetShortestPath_ProvideAllParams_ConfirmReturn(string src, string dst)
        {
            string result;
            result = DAL.ReadGraph(src, dst);
            Assert.IsType<string>(result);
            Assert.True(result == "Success");
        }
    }
}
