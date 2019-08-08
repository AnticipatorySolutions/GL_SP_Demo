using System;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using GL_SP_Demo_API.Controllers;
using GL_SP_Demo_BLL.Route;

namespace GL_SP_Demo_API.Tests
{
    public class PathControllerTests
    {
        readonly PathController Controller;
        readonly IGL_Route_BLL ServiceMock;

        public PathControllerTests()
        {
            ServiceMock = new BLL_Mock();
            Controller = new PathController(ServiceMock);
        }

        [Fact]
        public void GetPath_ProvideAllParams_ConfirmReturn()
        {
            ActionResult<string> okResult;
            okResult = Controller.Get("Source", "Destination");
            Assert.IsType<string>(okResult.Value);
            Assert.True(okResult.Value == "Success");
        }

        [Fact]
        public void GetPath_NullSrcParams_ConfirmReturn()
        {
            ActionResult<string> okResult;
            okResult = Controller.Get(null, "Destination");
            Assert.IsType<string>(okResult.Value);
            Assert.True(okResult.Value == "Please include src Code 3 value in your query.");
        }

        [Fact]
        public void GetPath_NullDstParams_ConfirmReturn()
        {
            ActionResult<string> okResult;
            okResult = Controller.Get("Source", null);
            Assert.IsType<string>(okResult.Value);
            Assert.True(okResult.Value == "Please include a dst Code 3 value in your query.");
        }

        [Fact]
        public void GetPath_NullParams_ConfirmReturn()
        {
            ActionResult<string> okResult;
            okResult = Controller.Get(null, null);
            Assert.IsType<string>(okResult.Value);
            Assert.True(okResult.Value == "Please include src and dst Code 3 values in your query.");
        }
    }
}