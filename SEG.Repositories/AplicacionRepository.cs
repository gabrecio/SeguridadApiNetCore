using SEG.Repositories.Common;
using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Repositories
{
    public interface IAplicacionRepository : IRepository<Aplicacion>
    {

    }
    public class AplicacionRepository :  Repository<Aplicacion>, IAplicacionRepository
    {
        private readonly SeguridadEntities _dbContext;
        public AplicacionRepository(SeguridadEntities dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
