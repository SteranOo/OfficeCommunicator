using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using OfficeCommunicator.Dal.EF;
using OfficeCommunicator.Dal.Entities;
using OfficeCommunicator.Dal.Repositories;
using OfficeCommunicator.Dal.UoW;
using OfficeCommunicator.Middleware.Network;
using OfficeCommunicator.Middleware.Utils;

namespace OfficeCommunicator.Server.Services
{
    public class AuthorizationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGenericRepository<User> _userRepository;
        public readonly ConcurrentDictionary<IClientCallback, User> Clients;

        public AuthorizationService()
        {
            _uow = new UnitOfWork(new ApplicationDbContext());
            _userRepository = _uow.GetRepository<User>();
            Clients = new ConcurrentDictionary<IClientCallback, User>();
        }
        private IStringEncoder Encoder
        {
            get
            {
                var setting = ConfigurationManager.AppSettings["PasswordEncoding"];
                IStringEncoder encoder = null;
                switch (setting)
                {
                    case "Md5":
                        encoder = new Md5StringEncoder();
                        break;
                    case "Sha256":
                        encoder = new Sha256StringEncoder();
                        break;
                }
                return encoder;
            }
        }

        public bool Register(string login, string password)
        {
            var user =  _userRepository.GetBy(x => x.Login == login).FirstOrDefault();
            if (user != null)
                return false;

            var encodedPassword = Encoder.EncodeString(password);
            _userRepository.Add(new User { Login = login, Password = encodedPassword });
            _uow.SaveAsync();
            return true;
        }

        public bool Login(string login, string passwordHash, IClientCallback client)
        {
            var user = _userRepository.GetBy(x => x.Login == login && x.Password == passwordHash).FirstOrDefault();
            if (user == null)
                return false;

            return Clients.Values.FirstOrDefault(x => x.Login == login) == null && Clients.TryAdd(client, user);
        }

        public User Logout(IClientCallback client)
        {
            Clients.TryRemove(client, out var user);
            return user;
        }

        public User GetUser(string login)
        {
            return _userRepository.GetBy(x => x.Login == login).FirstOrDefault();
        }

        public User GetUser(IClientCallback client)
        {
            Clients.TryGetValue(client, out var user);
            return user;
        }

        public bool IsAuthorized(IClientCallback client)
        {
            return Clients.ContainsKey(client);
        }
    }
}
