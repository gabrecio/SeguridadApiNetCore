using SEG.LocalViewModels;
using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.Mappers
{
    public class CentroCostoMapper
    {
        public static CentroCostoViewModel Mapper(CentroCosto model)
        {

            
            
            var cc = new CentroCostoViewModel()
            {
                Id = model.Id,
                Codigo = model.Codigo,
                Nombre = model.Nombre,
                Descripcion = model.Codigo + " " + model.Nombre,
                CentroCostoPadreId = model.CentroCostoPadreId,
                Activo = model.Activo,
                ManejaPresupuesto = model.ManejaPresupuesto

            };            
            return cc;

        }

      

        public static CentroCosto Mapper(CentroCostoViewModel viewModel)
        {        
         
            var cc = new CentroCosto()
            {
                Id = viewModel.Id,
                Codigo = viewModel.Codigo,
                Nombre = viewModel.Nombre,
                CentroCostoPadreId = viewModel.CentroCostoPadreId,
                Activo = viewModel.Activo,
                ManejaPresupuesto = viewModel.ManejaPresupuesto,
            };
            return cc;
        }
    }
}
