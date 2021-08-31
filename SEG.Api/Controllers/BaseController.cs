using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
//using System.Web.Http;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace SEG.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class BaseController :ControllerBase
    {
        //protected readonly ILog log = LogManager.GetLogger("ErrorLog","");
      
        /// <summary>
        /// 
        /// </summary>
        public BaseController()
        {
            //log4net.Config.XmlConfigurator.Configure();
          
        }

      
         /// <summary>
         /// rol del usuario actual
         /// </summary>
        public string UsuarioRol
        {
            set
            {
              //  HttpContext.Current.Session["rol"] = value;
            }
            get
            {
                var identity = User.Identity as ClaimsIdentity;
                var identitySesion = identity.Claims.Select(c => new
                {
                    Type = c.Type,
                    Value = c.Value
                });

                return (identitySesion.Where(a => a.Type == "rol").Select(b => b.Value).FirstOrDefault());
            }
        }

        /// <summary>
        /// recupera el id del usuario que esta en sesion
        /// </summary>
        public string GetUser
        {
           
            get
            {
                var identity = User.Identity as ClaimsIdentity;
                var identitySesion = identity.Claims.Select(c => new
                {
                    Type = c.Type,
                    Value = c.Value
                });

                return (identitySesion.Where(a => a.Type == "userId").Select(b => b.Value).FirstOrDefault());
            }
        }

        /// <summary>
       

    }
}
