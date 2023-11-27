using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ITeacherDedicationRepository : IRepository<TeacherDedication>
    {
        Task<IEnumerable<TeacherDedication>> GetAllWithIncludes(int? status = null);

        Task<object> GetAllAsSelect2ClientSideAsync(bool includeTitle = false);
        Task<object> GetTeacherDedicationSelect();
        Task<double> GetTeacherLessonsHours(string teacherId, Guid termId, TimeSpan startTime, TimeSpan endTime);
        Task<object> GetTeacherDedicationChart(Guid? id = null);
        Task<IEnumerable<TeacherDedication>> GetAll(string search, bool? onlyActive = false);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDedicationDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherDedicationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null);
    }
}