using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.Repositories
{
    public interface ICentroCostoRepository : IRepository<CentroCosto>
    {

    }
    public class CentroCostoRepository : Repository<CentroCosto>, ICentroCostoRepository
    {
        private readonly SeguridadEntities _dbContext;
            public CentroCostoRepository(SeguridadEntities dbContext) : base(dbContext)
            {
                _dbContext = dbContext;
            }
      
    }
}
