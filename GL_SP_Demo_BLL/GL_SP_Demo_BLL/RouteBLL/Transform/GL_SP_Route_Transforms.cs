using System;
using System.Collections.Generic;
using GL_SP_Demo_BLL.Abstracts.Transform;

namespace GL_SP_Demo_BLL.Route
{
    public interface IRouteTransformationHandler : ITransformHandler<string, GL_SP_Route_Transformer_Del> 
    {
        new string Run(GL_SP_Route_Transformer_Del transformer, string item);
    }

    public interface IRouteTransformationStore : ITransformStore<string>
    {
        string ToUpper(string item);
    }

    public delegate string GL_SP_Route_Transformer_Del(string text);

    public class RouteTransformationHandler : GL_SP_Abstract_TransformHandler<string, GL_SP_Route_Transformer_Del>, IRouteTransformationHandler
    {
        public override string Run(GL_SP_Route_Transformer_Del transformer, string item)
        {
            return transformer(item);
        }
    }

    public class RouteTransformationStore : GL_SP_Abstract_TransformStore<string>, IRouteTransformationStore
    {
        public string ToUpper(string text)
        {
            return text.ToUpper();
        }
    }
}
