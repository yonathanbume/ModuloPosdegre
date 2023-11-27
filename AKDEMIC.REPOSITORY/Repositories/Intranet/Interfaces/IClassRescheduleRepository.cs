using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IClassRescheduleRepository : IRepository<ClassReschedule>
    {
        Task<IEnumerable<ClassReschedule>> GetAll(string userId = null, int? status = null);
        Task<bool> AnyByClass(Guid classId, int? status = null);
        Task<object> GetClassReschedule(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetClassRescheduleDatatable(DataTablesStructs.SentParameters sentParameters,ClaimsPrincipal user , int? status = null, string startSearchDate = null, string endSearchDate = null, string search = null);
    }
}
