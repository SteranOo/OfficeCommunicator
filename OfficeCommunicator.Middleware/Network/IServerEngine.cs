using System.Collections.Generic;
using System.ServiceModel;
using OfficeCommunicator.Middleware.Dto;

namespace OfficeCommunicator.Middleware.Network
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IClientCallback))]
    public interface IServerEngine
    {
        [OperationContract]
        bool Register(string login, string password);

        [OperationContract]
        bool Login(string login, string passwordHash);

        [OperationContract(IsOneWay = true)]
        void Logout();

        [OperationContract]
        List<string> GetUsers();

        [OperationContract]
        List<ChatMessageDto> GetMessages(string companionLogin);

        [OperationContract]
        void SendMessage(ChatMessageDto message);
    }
}
