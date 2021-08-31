/************************************************************************************************************
 * Descripción: Servicio Entidad CentroCosto. 
 * Observaciones: 
 * Creado por: Gabriel Recio.
 * Historial de Revisiones: 
 * ----------------------------------------------------------------------------------------------- 
 * Fecha        Evento / Método Autor   Descripción 
 * ----------------------------------------------------------------------------------------------- 
 * 04/10/2017 13:58			Implementación inicial. 
 * ----------------------------------------------------------------------------------------------- 
*/ 
using System;
using System.Collections.Generic;
using System.Linq;
using SEG.Commons;
using SEG.LocalViewModels;
using GlobalView;
using SEG.Repositories;
using SEG.Mappers;

namespace SEG.Services
{

    public interface ICentroCostoService
    {
        CentroCostoViewModel GetById(int id);
        List<CentroCostoViewModel> GetAll(string query);
        //List<CentroCostoViewModel> GetCentroDeCostoHijos(int centroCostoId, List<CentroCostoViewModel> lista);
        List<ForaneaExtViewModel> GetListaAgil();
        bool LimpiarCacheCentroCosto();
    }

    public class CentroCostoService:ICentroCostoService
    { 
        private readonly ICentroCostoRepository _centroCostoRepository;
    public CentroCostoService(ICentroCostoRepository  centroCostoRepository)
    { 
       this._centroCostoRepository =  centroCostoRepository;
    } 
    public  CentroCostoViewModel GetById(int id)
    {
            var SingletonCentroCosto = _centroCostoRepository.GetAll().ToList(); //ListaCentroCostoSingleton.Instance.GetLista();
            return SingletonCentroCosto.Where(p=>p.Id == id).Select(p=> CentroCostoMapper.Mapper(p)).FirstOrDefault();
    } 
    public  List<CentroCostoViewModel> GetAll(string query)
    {
            List<CentroCostoViewModel> viewModels = null;
            var SingletonCentroCosto = _centroCostoRepository.GetAll().ToList();//ListaCentroCostoSingleton.Instance.GetLista();

            if (String.IsNullOrEmpty(query))
            {
                viewModels = SingletonCentroCosto.Where(p=>p.Activo).Select(p => CentroCostoMapper.Mapper(p)).ToList(); 
            }
            else
            {
                var queryFilter = query.ToLower().RemoveDiacritics2();
                string[] terms = queryFilter.Split();
                foreach (string word in terms)
                {

                    if (viewModels == null)
                    {
                        viewModels = SingletonCentroCosto.Where(p => p.Activo && p.Codigo.ToLower().RemoveDiacritics2().Contains(word) || p.Nombre.ToLower().RemoveDiacritics2().Contains(word)).Select(p => CentroCostoMapper.Mapper(p)).ToList();
                    }
                    else
                    {
                        viewModels = viewModels.Where(p => p.Activo && p.Codigo.ToLower().RemoveDiacritics2().Contains(word) || p.Nombre.ToLower().RemoveDiacritics2().Contains(word)).ToList();
                    }
                }
            }       
        return viewModels;
    } 
  
   /* public List<CentroCostoViewModel> GetCentroDeCostoHijos(int centroCostoId, List<CentroCostoViewModel> lista)
    {
          
            lista.Add(_CentroCostoRepository.Get().Where(p => p.Id == centroCostoId).Select(p => CentroCostoViewModel.Mapper(p)).First());            
            foreach (int hijo in _CentroCostoRepository.Get().Where(p => p.CentroCostoPadreId == centroCostoId).Select(p => p.Id).ToList())
            {
                GetCentroDeCostoHijos(hijo, lista);
            }
            return lista;
        }*/


        public List<ForaneaExtViewModel> GetListaAgil()
        {

            List<CentroCostoViewModel> models = null;
            var SingletonCentroCosto = _centroCostoRepository.GetAll();// ListaCentroCostoSingleton.Instance.GetLista();
            models = SingletonCentroCosto.Where(p=>p.Activo).Select(p => CentroCostoMapper.Mapper(p)).ToList();

            return models.Select(p => ForaneaExtViewModel.Mapper(p.Id, p.Codigo, p.Nombre)).ToList();
        }

        public bool LimpiarCacheCentroCosto()
        {
            try
            {
                ListaCentroCostoSingleton.Instance.ResetCache();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
