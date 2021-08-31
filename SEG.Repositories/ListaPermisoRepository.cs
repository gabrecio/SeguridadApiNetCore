using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SEG.Repositories.Common;
using SEG.Repositories.DataContext;

namespace SEG.Repositories
{
    public interface IListaPermisoRepository : IRepository<ListaPermiso>
    {

    }
    public class ListaPermisoRepository : Repository<ListaPermiso>, IListaPermisoRepository
    {
        private readonly SeguridadEntities _dbContext;

        public ListaPermisoRepository(SeguridadEntities dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
