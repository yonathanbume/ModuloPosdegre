using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.WorkingDay;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public sealed class WorkingDayService : IWorkingDayService
    {
        private readonly IWorkingDayRepository _workingDayRepository;
        public WorkingDayService(IWorkingDayRepository workingDayRepository)
        {
            _workingDayRepository = workingDayRepository;
        }

        public async Task AddWorkingDaysRange(IEnumerable<WorkingDay> entities)
            => await _workingDayRepository.AddRange(entities);

        public async Task<WorkingDay> Get(Guid Id)
        {
            return await _workingDayRepository.Get(Id);
        }

        public async Task<IEnumerable<WorkingDay>> GetWorkingDayByDateAndUser(DateTime registerDate, string userId)
            => await _workingDayRepository.GetWorkingDayByDateAndUser(registerDate, userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkingDaysDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? initDatetime, DateTime? finishDatetime, string academicCoordinatorId = null, string searchValue = null)
        {
            return await _workingDayRepository.GetWorkingDaysDatatable(sentParameters, initDatetime, finishDatetime, academicCoordinatorId, searchValue);
        }

        public async Task Update(WorkingDay entity)
        {
            await _workingDayRepository.Update(entity);
        }

        public async Task UpdateWorkingDays(IEnumerable<WorkingDay> entites)
            => await _workingDayRepository.UpdateRange(entites);

        Task<WorkingDay> IWorkingDayService.GetByFilter(DateTime? date, string userId)
        {
            return _workingDayRepository.GetByFilter(date, userId);
        }

        Task IWorkingDayService.InsertRange(ICollection<WorkingDay> workingDays)
        {
            return _workingDayRepository.InsertRange(workingDays);
        }

        Task IWorkingDayService.UpdateRange(ICollection<WorkingDay> workingDays)
        {
            return _workingDayRepository.UpdateRange(workingDays);
        }

        public async Task<object> GetReportTeachearAssistenace(DateTime initDatetime, DateTime finishDatetime)
        {
            return await _workingDayRepository.GetReportTeachearAssistenace(initDatetime, finishDatetime);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetWorkingDaySelect2ClientSide(Guid termId, string userId, bool? isAbsent = false)
        {
            return await _workingDayRepository.GetWorkingDaySelect2ClientSide(termId, userId, isAbsent);
        }

        public async Task Insert(WorkingDay ta)
        {
            await _workingDayRepository.Insert(ta);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersDailyAssitance(DataTablesStructs.SentParameters sentParameters, string teacherId, DateTime startDate, string search, ClaimsPrincipal user)
        {
            return await _workingDayRepository.GetTeachersDailyAssitance(sentParameters, teacherId, startDate, search, user);
        }

        public async Task<IEnumerable<TeacherPlusAssitance>> GetTeachersDailyAssitance(string teacherId, DateTime startDate, string search, ClaimsPrincipal user)
        {
            return await _workingDayRepository.GetTeachersDailyAssitance(teacherId, startDate, search, user);
        }

        public async Task<TeacherPlusMonthlyAssitance> GetMonthlyAssistance(string teacherId, string startDate, string search, ClaimsPrincipal user = null)
        {
            return await _workingDayRepository.GetMonthlyAssistance(teacherId, startDate, search, user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersToTakeAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null)
            => await _workingDayRepository.GetTeachersToTakeAssistanceDatatable(sentParameters, user, searchValue);

        public async Task<IEnumerable<WorkingDayConsolited>> GetConsolidatedWorkingDayByMonth(DateTime startDate, DateTime endDate, List<string> usersId = null)
            => await _workingDayRepository.GetConsolidatedWorkingDayByMonth(startDate,endDate, usersId);
    }
}