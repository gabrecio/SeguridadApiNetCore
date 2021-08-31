using SEG.Repositories.Common;
using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Repositories
{
    public interface IUsuarioRolRepository : IRepository<UsuarioRol>
    {
    }
    public class UsuarioRolRepository : Repository<UsuarioRol>, IUsuarioRolRepository
    {
        private readonly SeguridadEntities _dbContext;
        public UsuarioRolRepository(SeguridadEntities dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
