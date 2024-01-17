using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Implementations
{
    public  class AsignaturaService:IAsignaturaService
    {
        public readonly IAsignaturaRepository _asignaturaRepository;
        public AsignaturaService(IAsignaturaRepository asignaturaRepository)
        {
            _asignaturaRepository = asignaturaRepository ;
        }
      
        public async Task Delete(Master entity)
        {
           // await _masterRepository.Delete(entity);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetAsignaturaDataTable(DataTablesStructs.SentParameters parameters1, string search)
       => _asignaturaRepository.GetAsignaturaDataTable(parameters1, search);

        public async  Task Insert(Asignatura entity)
        {
            await _asignaturaRepository.Insert(entity);
        }

        /* public Task<List<Master>> GetAllMaster()
         {
             //return _masterRepository.GetAll();
         }*/
    }
}