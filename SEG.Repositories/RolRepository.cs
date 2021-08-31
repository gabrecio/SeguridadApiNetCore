/************************************************************************************************************
 * Descripción: Repositorio Entidad rol. 
 * Observaciones: 
 * Creado por: Gabriel Recio.
 * Historial de Revisiones: 
 * ----------------------------------------------------------------------------------------------- 
 * Fecha        Evento / Método Autor   Descripción 
 * ----------------------------------------------------------------------------------------------- 
 * 04/10/2016 18:24			Implementación inicial. 
 * ----------------------------------------------------------------------------------------------- 
*/
using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SEG.Repositories
{
    public interface IRolRepository : IRepository<Rol>
    {
        int RolUpdate(Rol rol);
    }
    public class RolRepository : Repository<Rol>, IRolRepository
    { 
        private readonly SeguridadEntities _dbContext;
        public RolRepository(SeguridadEntities dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public int RolUpdate(Rol rol)
        {


            //no alterar el orden de las sentencias.. asi funciona...

            // Apply many-to-many link modifications
            dbContext.Set<RolPermiso>().UpdateLinks(pc => pc.RolId, rol.Id,
                pc => pc.ListaPermisoId, rol.RolPermiso.Select(pc => pc.ListaPermisoId));
            _dbContext.SaveChanges();


            //var currentRol = this.FindByCondition(owner => owner.Id.Equals(rol.Id)).First();
            var currentRol = _dbContext.Rol.Where(p => p.Id == rol.Id).First();

            if (currentRol == null)
                {
                    // Handle the invalid call
                    return 0;
                }

                // Apply primitive property modifications
                 _dbContext.Entry(currentRol).CurrentValues.SetValues(rol);

                _dbContext.Rol.Attach(currentRol);
                _dbContext.Entry(currentRol).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _dbContext.SaveChanges();
            
                return 1;
           
           
        }

    }
}
