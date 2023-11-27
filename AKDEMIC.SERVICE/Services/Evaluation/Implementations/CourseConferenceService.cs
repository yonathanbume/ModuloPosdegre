using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class CourseConferenceService : ICourseConferenceService
    {
        private readonly ICourseConferenceRespository _courseConferenceRespository;

        public CourseConferenceService(ICourseConferenceRespository courseConferenceRespository)
        {
            _courseConferenceRespository = courseConferenceRespository;
        }

        public async Task<List<CourseConferenceTemplate>> GetCourseConferenceSiscoToHome()
        {
            return await _courseConferenceRespository.GetCourseConferenceSiscoToHome();
        }

        public async Task<CourseConferenceTemplate> DetailCourseConferenceSiscoToHome(Guid id)
        {
            return await _courseConferenceRespository.DetailCourseConferenceSiscoToHome(id);
        }

        public async Task<CourseConferenceHomeTemplate> GetCourseConference(Guid id)
        {
            return await _courseConferenceRespository.GetCourseConference(id);
        }

        public async Task<List<CourseConferenceHomeTemplate>> GetCourseConferences(int page)
        {
            return await _courseConferenceRespository.GetCourseConferences(page);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GerCourseConferenceDataTable(DataTablesStructs.SentParameters parameters, string search, Guid? careerId = null)
        {
            return await _courseConferenceRespository.GerCourseConferenceDataTable(parameters, search, careerId);
        }

        public async Task<CourseConference> Get(Guid id)
        {
            return await _courseConferenceRespository.Get(id);
        }

        public async Task<bool> ExistCourseConferenceName(string name, Guid? id = null)
        {
            return await _courseConferenceRespository.ExistCourseConferenceName(name, id);
        }

        public async Task Insert(CourseConference courseConference)
        {
            await _courseConferenceRespository.Insert(courseConference);
        }

        public async Task Update(CourseConference courseConference)
        {
            await _courseConferenceRespository.Update(courseConference);
        }

        public async Task DeleteById(Guid id)
        {
            await _courseConferenceRespository.DeleteById(id);
        }

        public async Task<IEnumerable<CourseConference>> GetAll()
        {
            return await _courseConferenceRespository.GetAll();
        }
    }
}
