using System;
using System.Threading.Tasks;
using OfficeCommunicator.Dal.EF;
using OfficeCommunicator.Dal.Entities;
using OfficeCommunicator.Dal.Repositories;

namespace OfficeCommunicator.Dal.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        private bool _isDisposed;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity: BaseEntity
        {
            return new GenericRepository<TEntity>(_dbContext);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                _dbContext.Dispose();
            }
            _isDisposed = true;
        }
    }
}
