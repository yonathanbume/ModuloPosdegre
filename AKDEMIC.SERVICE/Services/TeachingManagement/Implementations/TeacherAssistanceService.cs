using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class TeacherAssistanceService : ITeacherAssistanceService
    {
        private readonly ITeacherAssistanceRepository _teacherAssistanceRepository;
        public TeacherAssistanceService(ITeacherAssistanceRepository teacherAssistanceRepository)
        {
            _teacherAssistanceRepository = teacherAssistanceRepository;
        }

        public async Task<List<TeacherAssistance>> GetByFilter(DateTime? time)
        {
            return await _teacherAssistanceRepository.GetByFilter(time);
        }

        public async Task<DataTablesStructs.ReturnedData<WorkingDay>> GetReport(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, DateTime? starttime, DateTime? endtime)
        {
            return await _teacherAssistanceRepository.GetReport(sentParameters, facultyId, careerId, starttime,endtime);
        }

        public async Task<List<WorkingDay>> GetReportData(Guid? facultyId, Guid? careerId, DateTime? starttime,DateTime? endtime)
        {
            return await _teacherAssistanceRepository.GetReportData(facultyId, careerId, starttime,endtime);
        }

        public async Task InsertRangeAsync(List<TeacherAssistance> listassistance)
        {
            await _teacherAssistanceRepository.AddRange(listassistance);
        }

        Task<TeacherAssistance> ITeacherAssistanceService.GetByFilter(string userId, DateTime? time)
        {
            return _teacherAssistanceRepository.GetByFilter(userId, time);
        }

        Task ITeacherAssistanceService.InsertAsync(TeacherAssistance teacherAssistance)
        {
            return _teacherAssistanceRepository.Insert(teacherAssistance);
        }

        Task ITeacherAssistanceService.UpdateAsync(TeacherAssistance teacherAssistance)
        {
            return _teacherAssistanceRepository.Update(teacherAssistance);
        }
    }
}