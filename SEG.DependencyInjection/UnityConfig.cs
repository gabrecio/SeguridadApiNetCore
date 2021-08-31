using System.Web;
using SEG.Repositories.DataContext;
using SEG.Repositories.Implementations;
using SEG.Repositories.Interfaces;
using SEG.Services.Implementations;
using SEG.Services.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;

namespace SEG.DependencyInjection
{
    public static class UnityConfig
    {
        private static UnityContainer container;

        public static UnityContainer Container { get { return container; } }

        public static UnityContainer RegisterComponents()
        {
            container = new UnityContainer();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<SeguridadEntities>();
            container.RegisterType<ISeguridadEntities, SeguridadEntities>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));          
         

            #region Services

            container.RegisterType<IUsuarioService, UsuarioService>();
            container.RegisterType<IRolService, RolService>();
            container.RegisterType<IListaPermisoService, ListaPermisoService>();
            container.RegisterType<IPresupuestoService, PresupuestoService>();
            container.RegisterType<IAplicacionService, AplicacionService>();
            container.RegisterType<ICentroCostoService, CentroCostoService>();
            container.RegisterType<ITangoSueldoService, TangoSueldoService>();
            #endregion

            #region Repositories
            container.RegisterType<IUsuarioRepository, UsuarioRepository>();
                container.RegisterType<IRolRepository, RolRepository>();
                container.RegisterType<IListaPermisoRepository, ListaPermisoRepository>();
                container.RegisterType<IMenuRepository, MenuRepository>();
                container.RegisterType<IOperacionesRepository, OperacionesRepository>();
             //   container.RegisterType<ICentroCostoRepository, CentroCostoRepository>();
                container.RegisterType<IAplicacionRepository, AplicacionRepository>();
                container.RegisterType<IUsuarioRolRepository, UsuarioRolRepository>();
            #endregion

            return container;
        }

        public static T ResolveDependency<T>()
        {
            if (container == null)
            {
                container = new UnityContainer();
                RegisterComponents();
            }
            return container.Resolve<T>();
        }
    }
}

