using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatWCF
{
    [ServiceContract(CallbackContract = typeof(IServerChatCallback))]
    public interface IChatService
    {
        [OperationContract]
        int Connect(string name);

        [OperationContract]
        void Disonnect(int id);

        [OperationContract(IsOneWay = true)]
        void SendMessage(string message, int id);
    }

    public interface IServerChatCallback
    {
        [OperationContract (IsOneWay = true)]
        void MessageCallback(string message);
    }
}
