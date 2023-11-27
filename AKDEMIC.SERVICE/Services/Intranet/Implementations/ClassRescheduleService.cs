using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ClassRescheduleService : IClassRescheduleService
    {
        private readonly IClassRescheduleRepository _classRescheduleRepository;

        public ClassRescheduleService(IClassRescheduleRepository classRescheduleRepository)
        {
            _classRescheduleRepository = classRescheduleRepository;
        }

        public Task<bool> AnyByClass(Guid classId, int? status = null)
            => _classRescheduleRepository.AnyByClass(classId, status);

        public Task Delete(ClassReschedule classReschedule)
            => _classRescheduleRepository.Delete(classReschedule);

        public Task DeleteById(Guid id)
            => _classRescheduleRepository.DeleteById(id);

        public async Task DeleteRange(IEnumerable<ClassReschedule> entities)
            => await _classRescheduleRepository.DeleteRange(entities);

        public Task<ClassReschedule> Get(Guid id)
            => _classRescheduleRepository.Get(id);

        public Task<IEnumerable<ClassReschedule>> GetAll(string userId = null, int? status = null)
            => _classRescheduleRepository.GetAll(userId, status);

        public async Task<object> GetClassReschedule(Guid id)
        {
            return await _classRescheduleRepository.GetClassReschedule(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassRescheduleDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user , int? status = null, string startSearchDate = null, string endSearchDate = null, string search = null)
            => await _classRescheduleRepository.GetClassRescheduleDatatable(sentParameters, user,status,startSearchDate,endSearchDate, search);

        public Task Insert(ClassReschedule classReschedule)
            => _classRescheduleRepository.Insert(classReschedule);

        public Task Update(ClassReschedule classReschedule)
            => _classRescheduleRepository.Update(classReschedule);
    }
}
