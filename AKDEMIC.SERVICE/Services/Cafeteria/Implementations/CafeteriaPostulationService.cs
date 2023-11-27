using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class CafeteriaPostulationService : ICafeteriaPostulationService
    {
        private ICafeteriaPostulationRepository _cafeteriaPostulationRepository;

        public CafeteriaPostulationService(ICafeteriaPostulationRepository cafeteriaPostulationRepository)
        {
            _cafeteriaPostulationRepository = cafeteriaPostulationRepository;
        }
        public async Task Insert(CafeteriaPostulation cafeteriaPostulation)
        {
            await _cafeteriaPostulationRepository.Insert(cafeteriaPostulation);
        }

        public async Task Update(CafeteriaPostulation cafeteriaPostulation)
        {
            await _cafeteriaPostulationRepository.Update(cafeteriaPostulation);
        }

        public async Task<CafeteriaPostulation> FirstOrDefaultById(Guid id)
        {
            return await _cafeteriaPostulationRepository.FirstOrDefaultById(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllStudentsByIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid serviceTermId,string searchValue = null)
        {
            return await _cafeteriaPostulationRepository.GetAllStudentsByIdDatatable(sentParameters, serviceTermId, searchValue);
        }
    }
}
