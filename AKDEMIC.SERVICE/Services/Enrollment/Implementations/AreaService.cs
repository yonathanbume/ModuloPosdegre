using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;

        public AreaService(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }

        public async Task Delete(Area area) => await _areaRepository.Delete(area);

        public async Task DeleteById(Guid id) => await _areaRepository.DeleteById(id);

        public async Task<Area> Get(Guid id) => await _areaRepository.Get(id);

        public Task<object> GetAllAsSelect2ClientSide()
            => _areaRepository.GetAllAsSelect2ClientSide();

        public async Task<IEnumerable<Select2Structs.Result>> GetAreasSelect2ClientSide()
            => await _areaRepository.GetAreasSelect2ClientSide();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _areaRepository.GetDataDatatable(sentParameters, searchValue);

        public async Task Insert(Area area) => await _areaRepository.Insert(area);

        public async Task Update(Area area) => await _areaRepository.Update(area);

        public async Task<object> GetAreaJson(Guid id)
            => await _areaRepository.GetAreaJson(id);

        public async Task<object> GetAreaWithDataJson()
            => await _areaRepository.GetAreaWithDataJson();

        public async Task<Area> GetFirst()
        {
            return await _areaRepository.GetFirst();
        }

        public async Task<IEnumerable<Area>> GetAll()
            => await _areaRepository.GetAll();

        public async Task<Area> GetByName(string name)
            => await _areaRepository.GetByName(name);
    }
}
