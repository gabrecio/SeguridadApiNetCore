using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LocalViewModels
{
    public class AplicacionViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public string Observaciones { get; set; }
        public static AplicacionViewModel Mapper(Aplicacion model)
        {

            if (model != null)
            {
                var app = new AplicacionViewModel()
                {
                    Id = model.Id,
                    Codigo = model.Codigo,
                    Descripcion = model.Descripcion,
                    Activo = model.Activo,
                    Observaciones = model.Observaciones
                };
                return app;
            }
            else
                return null;
        }
        public static Aplicacion Mapper(AplicacionViewModel viewModel)
        {
            var app = new Aplicacion()
            {
                Id = viewModel.Id,
                Codigo = viewModel.Codigo,
                Descripcion = viewModel.Descripcion,
                Activo = viewModel.Activo,
                Observaciones = viewModel.Observaciones
            };
            return app;
        }
    }
}
