using System;
using Xunit;
using Xunit.Abstractions;
using GL_SP_Demo_BLL.Route;
using GL_SP_Demo_DAL.Route;


namespace GL_SP_Demo_BLL.Tests.Route
{
    public class DAL_Mock : IGL_DAL
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

    //Integration Test
    public class RouteBLLTests
    {
    
        private readonly ITestOutputHelper output;

        private readonly IGL_DAL GL_DAL;
        private readonly IRouteMessages Messages;
        private readonly IRouteMessageHandler MessageHandler;
        private readonly IRouteValidatorStore ValidatorStore;
        private readonly IRouteValidationHandler ValidationHandler;
        private readonly IRouteTransformationStore TransformationStore;
        private readonly IRouteTransformationHandler TransformationHandler;

        private readonly IGL_Route_BLL BLL;

        public RouteBLLTests(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
            GL_DAL = new DAL_Mock();
            Messages = new Messages_English();
            MessageHandler = new GL_SP_Route_MessageHandler(Messages);
            ValidatorStore = new RouteValidatorStore();
            ValidationHandler = new RouteValidationHandler();
            TransformationStore = new RouteTransformationStore();
            TransformationHandler = new RouteTransformationHandler();
            BLL = new GL_SP_Route_BLL(
                GL_DAL,
                Messages,
                ValidatorStore,
                ValidationHandler,
                TransformationStore,
                TransformationHandler
           );
        }

        [Theory]
        [InlineData(null, "DEN")]
        [InlineData("YYZ",null)]
        public void ValidatorStore_Match_ConfirmFalseReturn(string src, string dst)
        {
            Assert.False(ValidatorStore.Match(src, dst));
         }

        [Theory]
        [InlineData("DEN", "DEN")]
        [InlineData(null, null)]
        public void ValidatorStore_Match_ConfirmTrueReturn(string src, string dst)
        {
            Assert.True(ValidatorStore.Match(src, dst));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("YYZ", null)]
        [InlineData(null, "DEN")]
        [InlineData("222", "DEN")]
        [InlineData("YYZ", "555")]
        [InlineData("A", "DEN")]
        [InlineData("YYZ", "A")]
        [InlineData("AA", "DEN")]
        [InlineData("YYZ", "AA")]
        [InlineData("AAAA", "DEN")]
        [InlineData("YYZ", "AAAA")]
        public void ValidatorStore_Code3_ConfirmFalseReturn(string src, string dst)
        {
            Assert.False(ValidatorStore.Code3Validation(src, dst));
        }

        [Theory]
        [InlineData("YYZ", "DEN")]
        [InlineData("yyz", "den")]
        [InlineData("YYZ", "den")]
        [InlineData("yyz", "DEN")]
        public void ValidatorStore_Code3_ConfirmTrueReturn(string src, string dst)
        {
            output.WriteLine($"{src} {dst}");
            Assert.True(ValidatorStore.Code3Validation(src, dst));
        }

        [Fact]
        public void ValidationHandler_RunDelegate_ConfirmTrueReturn()
        {
            bool mockDelegate(string a, string b)
            {
                return true;
            }
            Assert.True(ValidationHandler.Run(mockDelegate,"a", "b"));
        }

        [Theory]
        [InlineData("YYZ","YYZ")]
        [InlineData("yyz","YYZ")]
        public void TransformationStore_ToUpper_ConfirmReturn(string a, string b)
        {
            Assert.True(TransformationStore.ToUpper(a) == b);
        }

        [Theory]
        [InlineData(MessageLabels.Code3FormatError, "Please make your request in the code3 format. ie. YYZ")]
        [InlineData(MessageLabels.MessageRequestError, "Unfortunately the SP Route message requested is unavailable.")]
        [InlineData(MessageLabels.SrcDstMatchFailure, "The src and dst match, so you're already where you want to go.")]
        public void MessagesEnglish_GetMessage_ConfirmReturn(MessageLabels label, string msg)
        {
            output.WriteLine($"{label} {msg}");
            Assert.True(Messages.GetMessage(label) == msg);
        }

        [Theory]
        [InlineData(MessageLabels.Code3FormatError, "Please make your request in the code3 format. ie. YYZ")]
        [InlineData(MessageLabels.MessageRequestError, "Unfortunately the SP Route message requested is unavailable.")]
        [InlineData(MessageLabels.SrcDstMatchFailure, "The src and dst match, so you're already where you want to go.")]
        public void MessageHandler_GetMessage_ConfirmReturn(MessageLabels label, string msg)
        {
            Assert.True(Messages.GetMessage(label) == msg);
        }

        //Integration Tests;
        [Theory]
        [InlineData("YYZ", "DEN")]
        [InlineData("YYZ", "den")]
        [InlineData("yyz", "DEN")]
        [InlineData("Yyz", "DeN")]
        public void GetShortestRoute_ProvideAllParams_ConfirmReturn(string src, string dst)
        {
            string result;
            result = BLL.GetShortestRoute(src,dst);
            Assert.IsType<string>(result);
            Assert.True(result == "Success");
        }

        [Theory]
        [InlineData("YYZ","YYZ")]
        [InlineData(null, null)]
        public void GetShortestRoute_ProvideMatchingParams_ConfirmReturn(string src, string dst)
        {
            string result;
            result = BLL.GetShortestRoute(src, dst);
            Assert.True(result == MessageHandler.GetMessage(MessageLabels.SrcDstMatchFailure));
            Assert.True(result == "The src and dst match, so you're already where you want to go.");
        }

        [Theory]
        [InlineData("YYZ", null)]
        [InlineData(null, "YYZ")]
        [InlineData("Source", "Destination")]
        [InlineData("Source", "YYZ")]
        [InlineData("YYZ", "Destination")]
        [InlineData("DE", "YYZ")]
        [InlineData("YYZ", "DE")]
        [InlineData("123", "YYZ")]
        [InlineData("YYZ", "123")]
        [InlineData("DE", "***")]
        [InlineData("***", "YYZ")]
        public void GetShortestRoute_ProvideInvaildCode3Params_ConfirmReturn(string src, string dst)
        {
            string result;
            result = BLL.GetShortestRoute(src, dst);
            Assert.True(result == MessageHandler.GetMessage(MessageLabels.Code3FormatError));
            Assert.True(result == "Please make your request in the code3 format. ie. YYZ");
        }

        [Theory]
        [InlineData("YYZ", "DEN")]
        [InlineData("YYZ", "den")]
        [InlineData("yyz", "DEN")]
        [InlineData("Yyz", "DeN")]
        public void GetShortestPath_ProvideAllParams_ConfirmReturn(string src, string dst)
        {
            string result;
            result = BLL.GetShortestPath(src, dst);
            Assert.IsType<string>(result);
            Assert.True(result == "Success");
        }

        [Theory]
        [InlineData("YYZ", "YYZ")]
        [InlineData(null, null)]
        public void GetShortestPath_ProvideMatchingParams_ConfirmReturn(string src, string dst)
        {
            string result;
            result = BLL.GetShortestPath(src, dst);
            Assert.True(result == MessageHandler.GetMessage(MessageLabels.SrcDstMatchFailure));
            Assert.True(result == "The src and dst match, so you're already where you want to go.");
        }

        [Theory]
        [InlineData("YYZ", null)]
        [InlineData(null, "YYZ")]
        [InlineData("Source", "Destination")]
        [InlineData("Source", "YYZ")]
        [InlineData("YYZ", "Destination")]
        [InlineData("DE", "YYZ")]
        [InlineData("YYZ", "DE")]
        [InlineData("123", "YYZ")]
        [InlineData("YYZ", "123")]
        [InlineData("DE", "***")]
        [InlineData("***", "YYZ")]
        public void GetShortestPath_ProvideInvaildCode3Params_ConfirmReturn(string src, string dst)
        {
            string result;
            result = BLL.GetShortestPath(src, dst);
            Assert.True(result == MessageHandler.GetMessage(MessageLabels.Code3FormatError));
            Assert.True(result == "Please make your request in the code3 format. ie. YYZ");
        }

    }
}
