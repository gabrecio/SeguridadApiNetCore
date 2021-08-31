using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalView
{
   public class ForaneaViewModel
    {
        public int id { get; set; }

        public string descripcion { get; set; }
        public static ForaneaViewModel Mapper(int id, string descripcion)
        {
            return new ForaneaViewModel()
            {
                id = id,
                descripcion = descripcion
            };
        }
    }
}
