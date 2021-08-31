using GlobalView;
using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LocalViewModels
{
    public class PermisosCentroCostoViewModel
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string descripcionCompleta { get; set; }
        public List<ForaneaViewModel> Permisos { get; set; }    

    }
}
