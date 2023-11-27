using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface ICourseConferenceRespository : IRepository<CourseConference>
    {
        Task<List<CourseConferenceTemplate>> GetCourseConferenceSiscoToHome();
        Task<CourseConferenceTemplate> DetailCourseConferenceSiscoToHome(Guid id);
        Task<CourseConferenceHomeTemplate> GetCourseConference(Guid id);
        Task<List<CourseConferenceHomeTemplate>> GetCourseConferences(int page);
        Task<DataTablesStructs.ReturnedData<object>> GerCourseConferenceDataTable(DataTablesStructs.SentParameters parameters, string search, Guid? careerId = null);
        Task<bool> ExistCourseConferenceName(string name, Guid? id = null);
    }
}
