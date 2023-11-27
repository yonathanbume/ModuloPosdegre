using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;

        public BuildingService(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }

        public async Task<IEnumerable<Building>> GetAll()
            => await _buildingRepository.GetAllWithData();

        public async Task<IEnumerable<Building>> GetAllByCampusId(Guid campusId)
            => await _buildingRepository.GetAllByCampusId(campusId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _buildingRepository.GetDataDatatable(sentParameters, searchValue, null);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataByCampusDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid campus)
            => await _buildingRepository.GetDataDatatable(sentParameters, searchValue, campus);

        public Task Delete(Building building) => _buildingRepository.Delete(building);

        public Task DeleteById(Guid id) => _buildingRepository.DeleteById(id);

        public Task Insert(Building building) => _buildingRepository.Insert(building);

        public Task Update(Building building) => _buildingRepository.Update(building);

        public Task<Building> Get(Guid id) => _buildingRepository.Get(id);

        public async Task<IEnumerable<Select2Structs.Result>> GetBuildingsSelect2ClientSide(Guid? campusId = null)
            => await _buildingRepository.GetBuildingsSelect2ClientSide(campusId);

        public async Task<object> GetBuildingsJson(Guid id)
            => await _buildingRepository.GetBuildingsJson(id);
    }
}
