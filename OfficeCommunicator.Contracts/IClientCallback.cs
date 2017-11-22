using System.ServiceModel;

namespace OfficeCommunicator.Contracts
{
    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void UserConnected(string login); 
    }
}
