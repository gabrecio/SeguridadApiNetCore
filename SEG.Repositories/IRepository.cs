using Microsoft.EntityFrameworkCore.Query;
using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace SEG.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        SeguridadEntities DbContext { get; }

        IEnumerable<TEntity> GetAll(          
           Expression<Func<TEntity, bool>> predicate = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           int? take = null,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        IEnumerable<TEntity> Get(
         Expression<Func<TEntity, bool>> filter = null,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,       
         string includeProperties = "");
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);
        //TEntity GetById(int id);

        void Create(TEntity entity);

        void Delete(int id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entity);
    }
}
