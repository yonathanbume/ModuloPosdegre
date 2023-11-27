using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CampusCareerService : ICampusCareerService
    {
        private readonly ICampusCareerRepository _campusCareerRepository;

        public CampusCareerService(ICampusCareerRepository campusCareerRepository)
        {
            _campusCareerRepository = campusCareerRepository;
        }

        public async Task<IEnumerable<CampusCareer>> GetAll()
            => await _campusCareerRepository.GetAll();
        public async Task Delete(CampusCareer area) => await _campusCareerRepository.Delete(area);

        public async Task DeleteById(Guid campus, Guid career) => await _campusCareerRepository.DeleteById(campus, career);

        public async Task<CampusCareer> Get(Guid campus, Guid career) => await _campusCareerRepository.Get(campus, career);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _campusCareerRepository.GetDataDatatable(sentParameters, searchValue, null);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataByCampusDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid campus)
            => await _campusCareerRepository.GetDataDatatable(sentParameters, searchValue, campus);

        public async Task Insert(CampusCareer area) => await _campusCareerRepository.Insert(area);

        public async Task Update(CampusCareer area) => await _campusCareerRepository.Update(area);

        public async Task<CampusCareer> GetCampusByIdAndCareer(Guid careerId, Guid campusId)
            => await _campusCareerRepository.GetCampusByIdAndCareer(careerId, campusId);

        public async Task<object> GetCareerSelect2ByCampusId(Guid campusId)
            => await _campusCareerRepository.GetCareerSelect2ByCampusId(campusId);
    }
}
