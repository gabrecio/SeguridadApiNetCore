using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalView
{
    public class DocumentoViewModel
    {
        public int Identificador { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Observaciones { get; set; }

    }
}
