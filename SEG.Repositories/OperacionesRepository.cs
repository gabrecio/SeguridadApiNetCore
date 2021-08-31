using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEG.Repositories.Common;
using SEG.Repositories.DataContext;


namespace SEG.Repositories
{
    public interface IOperacionesRepository : IRepository<Operacion>
    {
    }
    public class OperacionesRepository : Repository<Operacion>, IOperacionesRepository
    {
        private readonly SeguridadEntities _dbContext;
        public OperacionesRepository(SeguridadEntities dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}