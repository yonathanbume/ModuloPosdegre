using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Matricula;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class TeachingHoursRangeService: ITeachingHoursRangeService
    {
        private readonly ITeachingHoursRangeRepository _teachingHoursRangeRepository;
        public TeachingHoursRangeService(ITeachingHoursRangeRepository teachingHoursRangeRepository)
        {
            _teachingHoursRangeRepository = teachingHoursRangeRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _teachingHoursRangeRepository.DeleteById();
        }

        public async Task<TeachingHoursRange> Get(Guid? id)
        {
            return await _teachingHoursRangeRepository.Get(id);
        }

        public async Task<IEnumerable<TeachingHoursRange>> GetAll()
        {
            return await _teachingHoursRangeRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachingHoursRangeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? id = null)
            => await _teachingHoursRangeRepository.GetTeachingHoursRangeDatatable(sentParameters, id);

        public async Task Insert(TeachingHoursRange obj)
        {
            await _teachingHoursRangeRepository.Insert(obj);
        }
    }
}
