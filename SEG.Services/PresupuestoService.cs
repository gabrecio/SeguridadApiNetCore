using SEG.Services.Common;
//using PRE.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SEG.LocalViewModels;
using SEG.Services.Helpers;
using Microsoft.Extensions.Options;

namespace SEG.Services
{
    public interface IPresupuestoService
    {
        List<CentroCostoViewModel> GetCentroscosto();
    }
    public class PresupuestoService: IPresupuestoService
    {

       
         public PresupuestoService()
         {
             
         }

     


        public List<CentroCostoViewModel> GetCentroscosto()
        {
            HTTPClientePresupuesto HttpCliente = new HTTPClientePresupuesto();
            var result = HttpCliente.GET("CentroCosto");
            var listaCentrosCosto = JsonConvert.DeserializeObject<List<CentroCostoViewModel>>(result);
            return listaCentrosCosto;
        }
    }
}
