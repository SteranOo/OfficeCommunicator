using System.Collections.Generic;
using System.Linq;
using OfficeCommunicator.Dal.EF;
using OfficeCommunicator.Dal.Entities;
using OfficeCommunicator.Dal.Repositories;
using OfficeCommunicator.Dal.UoW;

namespace OfficeCommunicator.Server.Services
{
    public class ChatService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGenericRepository<ChatMessage> _messageRepository;

        public ChatService()
        {
            _uow = new UnitOfWork(new ApplicationDbContext());
            _messageRepository = _uow.GetRepository<ChatMessage>();
        }

        public List<ChatMessage> GetMessages(string userLogin, string companionLogin)
        {
            return _messageRepository
                .GetBy(x => x.Recipient.Login == companionLogin || x.Sender.Login == userLogin)
                .ToList();
        }

        public void SaveMessage(ChatMessage message)
        {
            _messageRepository.Add(message);
            _uow.SaveAsync();
        }
    }
}
