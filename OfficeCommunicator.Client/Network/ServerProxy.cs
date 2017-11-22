using System.Collections.Generic;
using System.ServiceModel;
using OfficeCommunicator.Middleware.Dto;
using OfficeCommunicator.Middleware.Network;

namespace OfficeCommunicator.Client.Network
{
    public class ServerProxy : DuplexClientBase<IServerEngine>, IServerEngine
    {
        private static ServerProxy _instance;

        public static ServerProxy Instance
        {
            get
            {
                if (_instance == null)
                {
                    var listener = new ServerListener();
                    _instance = new ServerProxy(new InstanceContext(listener), listener);
                }
                return _instance;
            }
        }

        public ServerListener Listener;

        protected ServerProxy(InstanceContext callback, ServerListener listener) : base(callback)
        {
            Listener = listener;
        }

        public bool Register(string login, string password)
        {
            return Channel.Register(login, password);
        }

        public bool Login(string login, string passwordHash)
        {
            return Channel.Login(login, passwordHash);
        }

        public void Logout()
        {
            Channel.Logout();
        }

        public List<string> GetUsers()
        {
            return Channel.GetUsers();
        }

        public List<ChatMessageDto> GetMessages(string companionLogin)
        {
            return Channel.GetMessages(companionLogin);
        }

        public void SendMessage(ChatMessageDto message)
        {
            Channel.SendMessage(message);
        }
    }
}
