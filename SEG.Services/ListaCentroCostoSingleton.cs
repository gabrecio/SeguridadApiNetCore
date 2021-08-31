
using Newtonsoft.Json;
using SEG.LocalViewModels;
using SEG.Services.Common;
using System;
using System.Collections.Generic;

namespace SEG.Services
{
    public class ListaCentroCostoSingleton
    {
        
            private List<CentroCostoViewModel> _lista = null;
            private int _cantidad;

        ListaCentroCostoSingleton()
            {
                var HttpCliente = new HTTPClientePresupuesto();
                var rest = HttpCliente.GET("centroCosto");
                var lista = JsonConvert.DeserializeObject<List<CentroCostoViewModel>>(rest);
                if (lista != null)
                {
                    _lista = lista;
                    _cantidad = lista.Count;
                }

            }

            public static ListaCentroCostoSingleton Instance
            {
                get
                {
                    return Nested.listaCentroCostoSingleton;
                }
            }
            public List<CentroCostoViewModel> GetLista()
            {
                return _lista;
            }
            public void SetLista(List<CentroCostoViewModel> l)
            {
                this._lista = l;
            }

            public void ResetCache()
            {
                Nested.listaCentroCostoSingleton = new ListaCentroCostoSingleton();
            }

        public int GetCantidad()
            {
                return _cantidad;
            }

            public void GetCantidad(int cant)
            {
                this._cantidad = cant;
            }

            class Nested
            {
                // Explicit static constructor to tell C# compiler
                // not to mark type as beforefieldinit
                static Nested()
                {

                }
                internal static ListaCentroCostoSingleton listaCentroCostoSingleton = new ListaCentroCostoSingleton();
            }
        }
}
