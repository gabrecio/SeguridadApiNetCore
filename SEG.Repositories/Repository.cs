
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace SEG.Repositories
{
    public class Repository<TEntity>: IRepository<TEntity>
        where TEntity : class
    {
       //  protected readonly ILog log = LogManager.GetLogger("ErrorLog","");
      
      

        internal SeguridadEntities dbContext;
        internal DbSet<TEntity> dbSet;

        public Repository(SeguridadEntities context)
        {           
            if (context == null) throw new ArgumentNullException("dbContext");
            this.dbContext = context;            
            this.dbSet = context.Set<TEntity>();
        }

        public SeguridadEntities DbContext
        {
            get { return dbContext; }
        }

        protected IQueryable<TEntity> PrepareQuery(
           IQueryable<TEntity> query,
           Expression<Func<TEntity, bool>> predicate = null,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           int? take = null
       )
        {
            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (take.HasValue)
                query = query.Take(Convert.ToInt32(take));

            return query;
        }

        public virtual IEnumerable<TEntity> GetAll(         
           Expression<Func<TEntity, bool>> predicate = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           int? take = null,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null
       )
        {
            var query = dbContext.Set<TEntity>().AsQueryable();

            query = PrepareQuery(query, predicate, include, orderBy, take);

            return query.ToList();
        }

        /// <summary>
        /// Get method
        /// </summary>   orderBy: x => x.OrderByDescending(y => y.Id)
        /// <param name="filter">uses lambda expressions to allow the calling code to specify a filter condition. Eg: if the repository is instantiated for the User entity type, the code in the calling method might specify u => u.UserName == "leoh" for the filter parameter.</param>
        /// <param name="orderBy">uses lambda expressions to allow the calling code to specify a column to order the results by. Eg if the repository is instantiated for the User entity type, the code in the calling method might specify u => q.OrderBy(u => u.UserName) for the orderBy parameter.</param>
        /// <param name="includeProperties">a string parameter that lets the caller provide a comma-delimited list of navigation properties for eager loading</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var property in dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations())
                query = query.Include(property.Name);

         /*   foreach (var includeProperty in includeProperties.Split
                  (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
              {
                  query = query.Include(includeProperty);
              }*/

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /*  public virtual IQueryable<TEntity> Query(bool eager = false)
          {
              IQueryable<TEntity> query = dbSet;
              if (eager)
              {
                  foreach (var property in dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations())
                      query = query.Include(property.Name);
              }
              return query;
          }*/

        /*  public virtual TEntity GetById(int id)
          {
               return dbSet.Find(id);               
          }*/
        //ver https://code-maze.com/net-core-web-development-part4/
        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entidad = this.dbContext.Set<TEntity>().Where(expression).AsNoTracking();
            foreach (var property in dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations())
                entidad = entidad.Include(property.Name);
            return entidad;
        }

        public virtual void Create(TEntity entity)
        {
           
            if (entity == null) throw new ArgumentNullException("entity");
            dbSet.Add(entity);
        }

        public virtual void Delete(int id)
        {
          
                TEntity entityToDelete = dbSet.Find(id);
                Delete(entityToDelete);
              
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (dbContext.Entry(entityToDelete).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            dbContext.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            dbSet.Attach(entity);
            dbContext.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
           
        }

       


    }
}
