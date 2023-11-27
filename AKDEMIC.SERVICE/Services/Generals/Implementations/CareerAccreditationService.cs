using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class CareerAccreditationService : ICareerAccreditationService
    {
        private readonly ICareerAccreditationRepository _careerAccreditationRepository;

        public CareerAccreditationService(ICareerAccreditationRepository careerAccreditationRepository)
        {
            _careerAccreditationRepository = careerAccreditationRepository;
        }

        public async Task Delete(CareerAccreditation entity)
            => await _careerAccreditationRepository.Delete(entity);

        public async Task<CareerAccreditation> Get(Guid id)
            => await _careerAccreditationRepository.Get(id);

        public Task<object> GetCareerAccreditationChart(Guid? careerId = null, DateTime? EndDate = null)
            => _careerAccreditationRepository.GetCareerAccreditationChart(careerId,EndDate);

        public Task<DataTablesStructs.ReturnedData<object>> GetCareerAccreditationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, DateTime? EndDate = null)
            => _careerAccreditationRepository.GetCareerAccreditationDatatable(sentParameters, careerId,EndDate);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareerAcreditationsHistoryDatatable(DataTablesStructs.SentParameters parameters, Guid careerId, string searchValue)
            => await _careerAccreditationRepository.GetCareerAcreditationsHistoryDatatable(parameters, careerId, searchValue);

        public async Task<List<CareerAccreditation>> GetDataList()
            => await _careerAccreditationRepository.GetDataList();

        public async Task Insert(CareerAccreditation entity)
            => await _careerAccreditationRepository.Insert(entity);
    }
}
