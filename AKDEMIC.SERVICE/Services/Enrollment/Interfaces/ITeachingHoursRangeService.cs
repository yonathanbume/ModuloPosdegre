using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Matricula;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ITeachingHoursRangeService
    {
        Task<IEnumerable<TeachingHoursRange>> GetAll();
        Task<TeachingHoursRange> Get(Guid? id);
        Task Insert(TeachingHoursRange obj);
        Task DeleteById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachingHoursRangeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? id = null);

    }
}
