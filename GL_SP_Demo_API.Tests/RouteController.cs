using System;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using GL_SP_Demo_API.Controllers;
using GL_SP_Demo_BLL.Route;

namespace GL_SP_Demo_API.Tests
{
    public class BLL_Mock : IGL_Route_BLL
    {
        public string GetShortestRoute(string src, string dst)
        {
            if (src == null&&dst == null) { return "src and dst null"; }
            if (src == null) { return "src null"; }
            if (dst == null) { return "dst null"; } 
            return "Success";
        }

        public string GetShortestPath(string src, string dst)
        {
            if (src == null && dst == null) { return "src and dst null"; }
            if (src == null) { return "src null"; }
            if (dst == null) { return "dst null"; }
            return "Success";
        }

    }

    public class RouteControllerTests
    {
        readonly RouteController Controller;
        readonly IGL_Route_BLL ServiceMock;

        public RouteControllerTests()
        {
            ServiceMock = new BLL_Mock();
            Controller = new RouteController(ServiceMock);            
        }        
        
        [Fact]
        public void GetRoute_ProvideAllParams_ConfirmReturn()
        {
            ActionResult<string> okResult;
            okResult = Controller.Get("Source", "Destination");
            Assert.IsType<string>(okResult.Value);
            Assert.True(okResult.Value == "Success");
        }

        [Fact]
        public void GetRoute_NullSrcParams_ConfirmReturn()
        {
            ActionResult<string> okResult;
            okResult = Controller.Get(null, "Destination");
            Assert.IsType<string>(okResult.Value);
            Assert.True(okResult.Value == "Please include src Code 3 value in your query.");
        }

        [Fact]
        public void GetRoute_NullDstParams_ConfirmReturn()
        {
            ActionResult<string> okResult;
            okResult = Controller.Get("Source", null);
            Assert.IsType<string>(okResult.Value);
            Assert.True(okResult.Value == "Please include a dst Code 3 value in your query.");
        }

        [Fact]
        public void GetRoute_NullParams_ConfirmReturn()
        {
            ActionResult<string> okResult;
            okResult = Controller.Get(null, null);
            Assert.IsType<string>(okResult.Value);
            Assert.True(okResult.Value == "Please include src and dst Code 3 values in your query.");
        }
    }
}