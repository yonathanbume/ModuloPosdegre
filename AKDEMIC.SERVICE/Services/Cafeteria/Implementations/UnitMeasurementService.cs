using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class UnitMeasurementService: IUnitMeasurementService
    {
        private readonly IUnitMeasurementRepository _unitMeasurementRepository;

        public UnitMeasurementService(IUnitMeasurementRepository unitMeasurementRepository)
        {
            _unitMeasurementRepository = unitMeasurementRepository;
        }

        public async Task Delete(UnitMeasurement unitMeasurement)
        {
            await _unitMeasurementRepository.Delete(unitMeasurement);
        }

        public async Task<UnitMeasurement> Get(Guid id)
        {
            return await _unitMeasurementRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUnitMeasurementDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _unitMeasurementRepository.GetUnitMeasurementDatatable(sentParameters,searchValue);
        }

        public async Task<object> GetUnitMeasurementSelect()
        {
            return await _unitMeasurementRepository.GetUnitMeasurementSelect();
        }

        public async Task Insert(UnitMeasurement unitMeasurement)
        {
            await _unitMeasurementRepository.Insert(unitMeasurement);
        }

        public async Task Update(UnitMeasurement unitMeasurement)
        {
            await _unitMeasurementRepository.Update(unitMeasurement);
        }
    }
}
