using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class CafeteriaServiceTermService : ICafeteriaServiceTermService
    {
        private readonly ICafeteriaServiceTermRepository _cafeteriaServiceTermRepository;

        public CafeteriaServiceTermService(ICafeteriaServiceTermRepository cafeteriaServiceTermRepository)
        {
            _cafeteriaServiceTermRepository = cafeteriaServiceTermRepository;
        }

        public async Task Delete(Guid id)
        {
            await _cafeteriaServiceTermRepository.DeleteById(id);
        }

        public async Task<CafeteriaServiceTerm> FirstOrDefaultById(Guid id)
        {
            return await _cafeteriaServiceTermRepository.FirstOrDefaultById(id);
        }

        public async Task<CafeteriaServiceTerm> Get(Guid id)
        {
            return await _cafeteriaServiceTermRepository.Get(id);
        }

        public async Task<CafeteriaServiceTerm> GetActiveCafeteriaServiceTerm()
        {
            return await _cafeteriaServiceTermRepository.GetActiveCafeteriaServiceTerm();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaServiceTermDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _cafeteriaServiceTermRepository.GetCafeteriaServiceTermDatatable(sentParameters,searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaServiceTermByStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId ,string searchValue = null)
        {
            return await _cafeteriaServiceTermRepository.GetCafeteriaServiceTermByStudentDatatable(sentParameters, studentId,searchValue);
        }

        public async Task<object> GetCustom(Guid id)
        {
            return await _cafeteriaServiceTermRepository.GetCustom(id);
        }

        public async Task Insert(CafeteriaServiceTerm cafeteriaServiceTerm)
        {
            await _cafeteriaServiceTermRepository.Insert(cafeteriaServiceTerm);
        }

        public async Task Update(CafeteriaServiceTerm cafeteriaServiceTerm)
        {
            await _cafeteriaServiceTermRepository.Update(cafeteriaServiceTerm);
        }
    }
}
