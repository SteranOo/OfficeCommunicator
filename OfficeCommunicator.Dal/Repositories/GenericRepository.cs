using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using OfficeCommunicator.Dal.EF;
using OfficeCommunicator.Dal.Entities;

namespace OfficeCommunicator.Dal.Repositories
{
    internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected ApplicationDbContext Dbcontext;
        protected readonly IDbSet<TEntity> Dbset;

        public GenericRepository(ApplicationDbContext context)
        {
            Dbcontext = context;
            Dbset = context.Set<TEntity>();
        }

        public virtual TEntity Get(int id)
        {
            return Dbset.FirstOrDefault(x => x.Id == id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Dbset.AsEnumerable();
        }

        public virtual IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Dbset.Where(predicate).AsEnumerable();
        }

        public virtual void Add(TEntity entity)
        {
            Dbset.Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            Dbset.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            Dbcontext.Entry(entity).State = EntityState.Modified;
        }
    }
}
