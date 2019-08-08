using System;
using System.Collections.Generic;

namespace GL_SP_Demo_BLL.Abstracts.Transform
{
    public interface ITransformHandler<J, K>
    {
        J Run(K transformer, J item);
    }

    public interface ITransformStore<J>{ }

    public abstract class GL_SP_Abstract_TransformHandler<J, K> : ITransformHandler<J, K>
    {
        public abstract J Run(K transformer, J item);
    }

    public abstract class GL_SP_Abstract_TransformStore<J> : ITransformStore<J>
    {
    }
}
