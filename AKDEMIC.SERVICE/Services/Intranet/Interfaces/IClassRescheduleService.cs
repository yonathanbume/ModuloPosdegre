using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IClassRescheduleService
    {
        Task<ClassReschedule> Get(Guid id);
        Task<IEnumerable<ClassReschedule>> GetAll(string userId = null, int? status = null);
        Task<bool> AnyByClass(Guid classId, int? status = null);
        Task Insert(ClassReschedule classReschedule);
        Task Update(ClassReschedule classReschedule);
        Task DeleteById(Guid id);
        Task DeleteRange(IEnumerable<ClassReschedule> entities);
        Task Delete(ClassReschedule classReschedule);
        Task<object> GetClassReschedule(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetClassRescheduleDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, int? status = null, string startSearchDate = null , string endSearchDate = null, string search = null);
    }
}
