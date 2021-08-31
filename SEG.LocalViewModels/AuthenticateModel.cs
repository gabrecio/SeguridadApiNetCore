using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SEG.LocalViewModels
{
    public  class AuthenticateModel
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
        [Required]
        public string codigoApp { get; set; }

        
    }
}
