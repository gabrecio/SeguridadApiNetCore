using SEG.Repositories.Common;
using SEG.Repositories.DataContext;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {
    }
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly SeguridadEntities _dbContext;
        public MenuRepository(SeguridadEntities dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}