using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ITeacherDedicationService
    {
        Task<IEnumerable<TeacherDedication>> GetAll();
        Task<IEnumerable<TeacherDedication>> GetAllWithIncludes(int? status = null);

        Task<TeacherDedication> GetByIdAsync(Guid id);
        Task InsertAsync(TeacherDedication teacherDedication);
        Task UpdateAsync(TeacherDedication teacherDedication);
        Task DeleteAsync(TeacherDedication teacherDedication);

        Task<object> GetAllAsSelect2ClientSideAsync(bool includeTitle = false);
        Task<object> GetTeacherDedicationSelect();

        Task<double> GetTeacherLessonsHours(string teacherId, Guid termId, TimeSpan startTime, TimeSpan endTime);
        Task<object> GetTeacherDedicationChart(Guid? id = null);
        Task<IEnumerable<TeacherDedication>> GetAll(string search, bool? onlyActive = false);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDedicationDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherDedicationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null);
    }
}