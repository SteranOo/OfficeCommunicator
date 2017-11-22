using System;
using System.Threading.Tasks;
using OfficeCommunicator.Dal.Entities;
using OfficeCommunicator.Dal.Repositories;

namespace OfficeCommunicator.Dal.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

        Task SaveAsync();
    }
}
