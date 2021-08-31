using log4net;
using SEG.LocalViewModels;
using SEG.Repositories.DataContext;
using SEG.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Services
{

    public interface IAplicacionService
    {
        AplicacionViewModel GetById(int id);
        List<AplicacionViewModel> GetAll(string query);
        int Insert(AplicacionViewModel viewModel);
        int Update(AplicacionViewModel viewModel);
        bool Delete(int id);
    }

    public class AplicacionService: IAplicacionService
    {
        private readonly IAplicacionRepository _appRepository;
        
      //  protected readonly ILog log = LogManager.GetLogger("ErrorLog","");

        public AplicacionService(IAplicacionRepository appRepository)
        {
            this._appRepository = appRepository;
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(log);

        }

        public AplicacionViewModel GetById(int id)
        {
            // var model = _appRepository.GetById(id);
            var model = _appRepository.FindByCondition(owner => owner.Id.Equals(id)).FirstOrDefault();

            var viewModel = AplicacionViewModel.Mapper(model);
            return viewModel;
        }
        public List<AplicacionViewModel> GetAll(string query)
        {
            var viewModels = new List<AplicacionViewModel>();
            List<Aplicacion> models;
            if (String.IsNullOrEmpty(query))
            {
                models = (List<Aplicacion>)_appRepository.Get();
            }
            else
            {
                models = _appRepository.Get().Where(p => p.Codigo.ToLower().Contains(query.ToLower()) || p.Descripcion.ToLower().Contains(query.ToLower())).ToList();
            }
            foreach (Aplicacion model in models)
            {
                viewModels.Add(AplicacionViewModel.Mapper(model));
            }
            return viewModels;
        }
        public int Insert(AplicacionViewModel viewModel)
        {
            try
            {
                var model = AplicacionViewModel.Mapper(viewModel);

                _appRepository.Create(model);
                _appRepository.DbContext.SaveChanges();

                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public int Update(AplicacionViewModel viewModel)
        {
            try
            {

                //var model = _appRepository.GetById(viewModel.Id);
                var model = _appRepository.FindByCondition(owner => owner.Id.Equals(viewModel.Id)).FirstOrDefault();
                model.Activo = viewModel.Activo;
                model.Codigo = viewModel.Codigo;
                model.Descripcion = viewModel.Descripcion;
                model.Observaciones = viewModel.Observaciones;
                _appRepository.Update(model);
                _appRepository.DbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                _appRepository.Delete(id);
                _appRepository.DbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
