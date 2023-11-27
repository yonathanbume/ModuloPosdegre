using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Matricula;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ITeachingHoursRangeRepository:IRepository<TeachingHoursRange>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTeachingHoursRangeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? id = null);
    }
}
