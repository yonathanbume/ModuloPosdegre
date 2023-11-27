using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ITeacherAssistanceRepository : IRepository<TeacherAssistance>
    {
        Task<TeacherAssistance> GetByFilter(string userId = null, DateTime? time = null);
        Task<List<TeacherAssistance>> GetByFilter(DateTime? time);
        Task<DataTablesStructs.ReturnedData<WorkingDay>> GetReport(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, DateTime? starttime,DateTime? endtime);
        Task<List<WorkingDay>> GetReportData(Guid? facultyId, Guid? careerId, DateTime? starttime, DateTime? endtime);
    }
}