using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using OfficeCommunicator.Dal.Entities;
using OfficeCommunicator.Middleware.Dto;
using OfficeCommunicator.Middleware.Network;
using OfficeCommunicator.Server.Services;

namespace OfficeCommunicator.Server.Network
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ServerEngine : IServerEngine
    {
        private readonly AuthorizationService _authorizationService;
        private readonly ChatService _chatService;

        private IClientCallback CurrentClient => OperationContext.Current.GetCallbackChannel<IClientCallback>();

        public ServerEngine()
        {
            _chatService = new ChatService();
            _authorizationService = new AuthorizationService();
        }

        private void NotifyUserLoggedIn(string login)
        {
            foreach (var client in _authorizationService.Clients)
            {
                if (client.Value.Login != login)
                    client.Key.UserLoggedIn(login);
            }
        }

        private void NotifyUserLoggedOut(string login)
        {
            foreach (var client in _authorizationService.Clients)
            {
                if (client.Value.Login != login)
                    client.Key.UserLoggedOut(login);
            }
        }

        public bool Register(string login, string password)
        {
            return _authorizationService.Register(login, password);
        }

        public bool Login(string login, string passwordHash)
        {
            var isLoggedIn = _authorizationService.Login(login, passwordHash, CurrentClient);
            if (!isLoggedIn)
                return false;

            Task.Run(() => NotifyUserLoggedIn(login));
            return true;
        }

        public void Logout()
        {
            var user = _authorizationService.Logout(CurrentClient);
            if (user == null)
                return;

            Task.Run(() => NotifyUserLoggedOut(user.Login));
        }

        public List<string> GetUsers()
        {
            return !_authorizationService.IsAuthorized(CurrentClient) ?
                null : _authorizationService.Clients.Where(x => x.Key != CurrentClient).Select(x => x.Value.Login).ToList();
        }

        public List<ChatMessageDto> GetMessages(string companionLogin)
        {
            if (!_authorizationService.IsAuthorized(CurrentClient))
                return null;

            var user = _authorizationService.GetUser(CurrentClient);
            var result = new List<ChatMessageDto>();
            _chatService.GetMessages(user.Login, companionLogin).ForEach(
                x => result.Add(new ChatMessageDto
                {
                    Text = x.Text,
                    Time = x.Time,
                    Sender = x.Sender.Login,
                    Recipient = x.Recipient.Login
                }));
            return result;
        }

        public void SendMessage(ChatMessageDto message)
        {
            if (!_authorizationService.IsAuthorized(CurrentClient))
                return;

            var entity = new ChatMessage
            {
                Text = message.Text,
                Time = message.Time,
                SenderId = _authorizationService.GetUser(message.Sender).Id,
                RecipientId = _authorizationService.GetUser(message.Recipient).Id
            };
            _chatService.SaveMessage(entity);
        }
    }
}
