using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface ICourseConferenceService
    {
        Task<List<CourseConferenceTemplate>> GetCourseConferenceSiscoToHome();
        Task<CourseConferenceTemplate> DetailCourseConferenceSiscoToHome(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GerCourseConferenceDataTable(DataTablesStructs.SentParameters parameters, string search, Guid? careerId = null);
        Task<CourseConference> Get(Guid id);
        Task<CourseConferenceHomeTemplate> GetCourseConference(Guid id);
        Task<List<CourseConferenceHomeTemplate>> GetCourseConferences(int page);
        Task<bool> ExistCourseConferenceName(string name, Guid? id = null);
        Task Insert(CourseConference courseConference);
        Task Update(CourseConference courseConference);
        Task DeleteById(Guid id);
        Task<IEnumerable<CourseConference>> GetAll();
    }
}
