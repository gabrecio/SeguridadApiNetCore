using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LocalViewModels
{
    public class CentroCostoViewModel
    {
            #region Propiedades 
            public int Id { get; set; }
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public int? CentroCostoPadreId { get; set; }
            public bool Activo { get; set; }
            public bool ManejaPresupuesto { get; set; }
        #endregion Propiedades -Get/Set

      
    }
    }
