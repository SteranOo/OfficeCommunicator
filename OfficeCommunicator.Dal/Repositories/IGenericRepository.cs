using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OfficeCommunicator.Dal.Entities;

namespace OfficeCommunicator.Dal.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        void Add(TEntity entity);

        void Delete(TEntity entity);

        TEntity Get(int id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate);

        void Update(TEntity entity);
    }
}