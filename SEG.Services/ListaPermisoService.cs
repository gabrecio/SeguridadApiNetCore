using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEG.LocalViewModels;
using SEG.Repositories.DataContext;
using SEG.Repositories;

namespace SEG.Services
{ 
    public interface IListaPermisoService
    {
        ListaPermiso GetListaPermisoById(int id);
        ListaPermiso ViewModelToModel(Permission viewModel);
    }
    public class ListaPermisoService : IListaPermisoService
    {
          private IListaPermisoRepository listaPermisoRepository;

          public ListaPermisoService(IListaPermisoRepository listaPermisoRepository)
        {
            this.listaPermisoRepository = listaPermisoRepository;
        }
          public ListaPermiso GetListaPermisoById(int id)
          {
            // return listaPermisoRepository.GetById(id);
            return listaPermisoRepository.FindByCondition(owner => owner.Id.Equals(id)).FirstOrDefault();
        }
          public ListaPermiso ViewModelToModel(Permission viewModel)
          {
              var lp = GetListaPermisoById(viewModel.ListaPermisoId);
              var model = new ListaPermiso
              {
                  Id = viewModel.ListaPermisoId,
                  MenuId = lp.MenuId,
                  OperacionId = lp.OperacionId
              };
              return model;
        }

       



    }
}
