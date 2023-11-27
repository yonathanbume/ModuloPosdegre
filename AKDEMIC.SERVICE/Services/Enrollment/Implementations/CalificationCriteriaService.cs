using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CalificationCriteriaService : ICalificationCriteriaService
    {
        private readonly ICalificationCriteriaRepository _calificationCriteriaRepository;

        public CalificationCriteriaService(ICalificationCriteriaRepository calificationCriteriaRepository)
        {
            _calificationCriteriaRepository = calificationCriteriaRepository;
        }

        public async Task InsertCalificationCriteria(CalificationCriteria calificationCriteria) =>
            await _calificationCriteriaRepository.Insert(calificationCriteria);

        public async Task UpdateCalificationCriteria(CalificationCriteria calificationCriteria) =>
            await _calificationCriteriaRepository.Update(calificationCriteria);

        public async Task DeleteCalificationCriteria(CalificationCriteria calificationCriteria) =>
            await _calificationCriteriaRepository.Delete(calificationCriteria);

        public async Task<CalificationCriteria> GetCalificationCriteriaById(Guid id) =>
            await _calificationCriteriaRepository.Get(id);

        public async Task<IEnumerable<CalificationCriteria>> GetAllCalificationCriterias() =>
            await _calificationCriteriaRepository.GetAll();

        public async Task<object> GetCalificationCriteriasObj()
            => await _calificationCriteriaRepository.GetCalificationCriteriasObj();

        public async Task<bool> GetAnyCalificationCriterias(Guid id)
            => await _calificationCriteriaRepository.GetAnyCalificationCriterias(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCalificationsCriteriasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _calificationCriteriaRepository.GetCalificationsCriteriasDatatable(sentParameters, searchValue);
    }
}
