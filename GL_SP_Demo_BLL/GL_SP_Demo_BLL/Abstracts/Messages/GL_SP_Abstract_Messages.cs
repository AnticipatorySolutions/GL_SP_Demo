using System.Collections.Generic;

namespace GL_SP_Demo_BLL.Abstracts.Messages
{
    public interface IMessages<J,K>
    {
        J GetMessage(K label);
    }

    public interface IMessageHandler<J,K>
    {
        J GetMessage(K label);
    }

    public abstract class GL_SP_Abstract_Messages<J, K> : IMessages<J, K>
    {
        private readonly Dictionary<K, J> Messages = new Dictionary<K, J>();
        public abstract J GetMessage(K label);
    }

    public abstract class GL_SP_Abstract_MessageHandler<J, K> : IMessageHandler<J, K>
    {
        public abstract J GetMessage(K label);
    }
}
