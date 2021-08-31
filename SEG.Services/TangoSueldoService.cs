using SEG.Services.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using GlobalView;

namespace SEG.Services
{
    public interface ITangoSueldoService
    {


        #region Departamentos     

        ForaneaExtViewModel GetDepartamentoById(int id);
        List<ForaneaExtViewModel> GetListaDepartamentos(string query, string sortBy, int page, int itemPerPage);

        List<ForaneaExtViewModel> GetListaAgilDepartamentos(string query, string sortBy);

        #endregion
    }
    public class TangoSueldoService: ITangoSueldoService
    {
        HttpClienteTangoSueldo HttpCliente = new HttpClienteTangoSueldo();

        public List<ForaneaExtViewModel> GetListaPersonas(string query, string sortBy)
        {
            var result = HttpCliente.GET("padron/ListaAgil?query=" + query + "&&sortBy=" + sortBy);
            var lista = JsonConvert.DeserializeObject<List<ForaneaExtViewModel>>(result);
            return lista;
        }     

        #region Departamentos     

        public ForaneaExtViewModel GetDepartamentoById(int id)
        {
            var result = HttpCliente.GET("departamento?id=" + id);
            var departamento = JsonConvert.DeserializeObject<ForaneaExtViewModel>(result);
            return departamento;
        }     

        public List<ForaneaExtViewModel> GetListaAgilDepartamentos(string query, string sortBy)
        {
            var result = HttpCliente.GET("Departamento/ListaAgil?query=" + query + "&&sortBy=" + sortBy);            
            var lista = JsonConvert.DeserializeObject<List<ForaneaExtViewModel>>(result);
            return lista;
        }

        public List<ForaneaExtViewModel> GetListaDepartamentos(string query, string sortBy, int page, int itemPerPage)
        {
            var result = HttpCliente.GET("departamento/ListaAgil?query=" + query + "&&sortBy=" + sortBy + "&&page=" + page + "&&itemPerPage=" + itemPerPage);            
            var lista = JsonConvert.DeserializeObject<List<ForaneaExtViewModel>>(result);
            return lista;
        }
        #endregion

    }
}
