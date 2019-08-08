using System;
using System.Collections.Generic;
using GL_SP_Demo_BLL.Abstracts.Messages;

namespace GL_SP_Demo_BLL.Route
{
    public enum MessageLabels
    {
        MessageRequestError,
        Code3FormatError,
        SrcDstMatchFailure,
    }

    public interface IRouteMessages : IMessages<string,MessageLabels>
    {
        new string GetMessage(MessageLabels label);
    }

    public interface IRouteMessageHandler : IMessageHandler<string,MessageLabels>
    {
        new string GetMessage(MessageLabels labels);
    }

    public class Messages_English : GL_SP_Abstract_Messages<string,MessageLabels>,IRouteMessages
    {
        private readonly Dictionary<MessageLabels, string> Messages = new Dictionary<MessageLabels, string>()
        {
            {MessageLabels.MessageRequestError, "Unfortunately the SP Route message requested is unavailable."},
            {MessageLabels.Code3FormatError, "Please make your request in the code3 format. ie. YYZ" },
            {MessageLabels.SrcDstMatchFailure,"The src and dst match, so you're already where you want to go."}
        };

        public override string GetMessage(MessageLabels label) 
        {
            if (Messages.ContainsKey(label))
            {
                return Messages[label];
            }
            else
            {
                return Messages[MessageLabels.MessageRequestError];
            }
        }
    }

    public class GL_SP_Route_MessageHandler : GL_SP_Abstract_MessageHandler<string,MessageLabels>,IRouteMessageHandler
    {
        IRouteMessages Messages;

        public GL_SP_Route_MessageHandler(IRouteMessages messages)
        {
            Messages = messages;
        }

        public override string GetMessage(MessageLabels label)
        {
            return Messages.GetMessage(label);
        }    
    }
}
