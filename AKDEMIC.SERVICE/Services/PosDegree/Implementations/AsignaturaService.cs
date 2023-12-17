using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
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

       /* public Task<List<Master>> GetAllMaster()
        {
            //return _masterRepository.GetAll();
        }*/
    }
}