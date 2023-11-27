
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class CafeteriaWeeklyScheduleTurnDetailService : ICafeteriaWeeklyScheduleTurnDetailService
    {
        private ICafeteriaWeeklyScheduleTurnDetailRepository _cafeteriaWeeklyScheduleTurnDetailRepository;
        public CafeteriaWeeklyScheduleTurnDetailService(ICafeteriaWeeklyScheduleTurnDetailRepository cafeteriaWeeklyScheduleTurnDetailRepository)
        {
            _cafeteriaWeeklyScheduleTurnDetailRepository = cafeteriaWeeklyScheduleTurnDetailRepository;
        }

        public async Task Delete(CafeteriaWeeklyScheduleTurnDetail model)
        {
            await _cafeteriaWeeklyScheduleTurnDetailRepository.Delete(model);
        }

        public async Task<CafeteriaWeeklyScheduleTurnDetail> Get(Guid id)
        {
            return await _cafeteriaWeeklyScheduleTurnDetailRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaWeeklyScheduleTurnDetailDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid cafeteriaWeeklyScheduleId)
        {
            return await _cafeteriaWeeklyScheduleTurnDetailRepository.GetCafeteriaWeeklyScheduleTurnDetailDatatable(sentParameters, searchValue, cafeteriaWeeklyScheduleId);
        }

        public async Task Insert(CafeteriaWeeklyScheduleTurnDetail entity)
        {
            await _cafeteriaWeeklyScheduleTurnDetailRepository.Insert(entity);
        }

        public async Task Update(CafeteriaWeeklyScheduleTurnDetail entity)
        {
            await _cafeteriaWeeklyScheduleTurnDetailRepository.Update(entity);
        }

        public async Task<Tuple<bool, string>> ValidateTurnAndHour(int Type, Guid CafeteriaWeeklyScheduleIdParameter, TimeSpan StartTimeParameter, TimeSpan EndTimeParameter, Guid? Id = null)
        {
           return await _cafeteriaWeeklyScheduleTurnDetailRepository.ValidateTurnAndHour(Type, CafeteriaWeeklyScheduleIdParameter, StartTimeParameter, EndTimeParameter, Id);
        }
    }
}
