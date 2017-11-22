using System.ServiceModel;

namespace OfficeCommunicator.Middleware.Network
{
    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void UserLoggedIn(string login);

        [OperationContract(IsOneWay = true)]
        void UserLoggedOut(string login);
    }
}
