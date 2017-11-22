using System.ServiceModel;

namespace OfficeCommunicator.Contracts
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
    }
}
