using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalView
{
    public class ForaneaExtViewModel
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string descripcionCompleta { get; set; }

        public static ForaneaExtViewModel Mapper(int id, string codigo, string descripcion)
        {
            return new ForaneaExtViewModel()
            {
                id = id,
                codigo = codigo,
                descripcion = descripcion,
                descripcionCompleta = codigo + " " + descripcion
            };
        }
    }

    public class ForaneaExt2ViewModel
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public decimal valor { get; set; }

        public static ForaneaExt2ViewModel Mapper(int id, string codigo, string descripcion, decimal valor)
        {
            return new ForaneaExt2ViewModel()
            {
                id = id,
                codigo = codigo,
                descripcion = descripcion,
                valor = valor
            };
        }
    }
}
