using System;
using GL_SP_Demo_DAL.Route;
using GL_SP_Demo_BLL.Abstracts;

namespace GL_SP_Demo_BLL.Route
{
    public interface IGL_Route_BLL : IGL_BLL<string,MessageLabels>
    {
        string GetShortestPath(string src, string dst);
        string GetShortestRoute(string src, string dst);
    }

    public class GL_SP_Route_BLL : GL_SP_Abstract_BLL<string,MessageLabels>,IGL_Route_BLL
    {
        private readonly IGL_DAL GL_DAL;
        private readonly IRouteMessages MessageHandler;
        private readonly IRouteValidatorStore ValidatorStore;
        private readonly IRouteValidationHandler ValidationHandler;
        private readonly IRouteTransformationStore TransformationStore;
        private readonly IRouteTransformationHandler TransformationHandler;
        
        public GL_SP_Route_BLL(
            IGL_DAL gl_dal,
            IRouteMessages messages,
            IRouteValidatorStore validatorStore,
            IRouteValidationHandler validatorHandler,
            IRouteTransformationStore transformationStore,
            IRouteTransformationHandler transformationHandler
            )
        {
            GL_DAL = gl_dal;
            MessageHandler = messages;
            ValidatorStore = validatorStore;
            ValidationHandler = validatorHandler;
            TransformationStore = transformationStore;
            TransformationHandler = transformationHandler;
        }

        public string GetShortestPath(string src, string dst)
        {
            try
            {
                if (ValidationHandler.Run(ValidatorStore.Match, src, dst))
                {
                    return MessageHandler.GetMessage(MessageLabels.SrcDstMatchFailure);
                };
                if (!ValidationHandler.Run(ValidatorStore.Code3Validation, src, dst))
                {
                    return MessageHandler.GetMessage(MessageLabels.Code3FormatError);
                }

                src = TransformationHandler.Run(TransformationStore.ToUpper, src);
                dst = TransformationHandler.Run(TransformationStore.ToUpper, dst);

                return GL_DAL.ReadGraph(src, dst);
            }
            catch (Exception exception)
            {
                return $"{exception.Message} {exception.Source} {exception.StackTrace}";
            }
        }

        public string GetShortestRoute(string src, string dst)
        {
            try
            {
                if (ValidationHandler.Run(ValidatorStore.Match, src, dst))
                {
                    return MessageHandler.GetMessage(MessageLabels.SrcDstMatchFailure);
                };
                if (!ValidationHandler.Run(ValidatorStore.Code3Validation, src, dst))
                {
                    return MessageHandler.GetMessage(MessageLabels.Code3FormatError);
                }

                src = TransformationHandler.Run(TransformationStore.ToUpper, src);
                dst = TransformationHandler.Run(TransformationStore.ToUpper, dst);

                return GL_DAL.Read(src, dst);
            }
            catch (Exception exception)
            {
                return $"{exception.Message} {exception.Source} {exception.StackTrace}";
            }
        }
    }
}
