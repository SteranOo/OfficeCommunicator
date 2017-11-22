using System;
using OfficeCommunicator.Middleware.Network;

namespace OfficeCommunicator.Client.Network
{
    public class ServerListener : IClientCallback
    {
        public event Action<string> OnUserLoggedIn;
        public event Action<string> OnUserLoggedOut;

        public void UserLoggedIn(string login)
        {
            OnUserLoggedIn?.Invoke(login);
        }

        public void UserLoggedOut(string login)
        {
            OnUserLoggedOut?.Invoke(login);
        }
    }
}
