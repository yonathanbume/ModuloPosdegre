using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReport;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class GradeReportService : IGradeReportService
    {
        private readonly IGradeReportRepository _gradeReportRepository;

        public GradeReportService(IGradeReportRepository gradeReportRepository)
        {
            _gradeReportRepository = gradeReportRepository;
        }

        public Task<GradeReport> Add(GradeReport model)
            => _gradeReportRepository.Add(model);

        public async Task<bool> ExistGradeReport(Guid studentId, int gradeType)
        {
            return await _gradeReportRepository.ExistGradeReport(studentId, gradeType);
        }

        public Task<GradeReport> Get(Guid id)
            => _gradeReportRepository.Get(id);

        public Task<GradeReport> GetByStudentIdAndGradeType(Guid studentId, int gradeType)
            => _gradeReportRepository.GetByStudentIdAndGradeType(studentId, gradeType);

        public async Task<string> GetEndDateByTermId(Guid termId)
        {
            return await _gradeReportRepository.GetEndDateByTermId(termId);
        }

        public async Task<GradeReport> GetGradeReportBachelor(Guid studentId)
        {
            return await _gradeReportRepository.GetGradeReportBachelor(studentId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeReportByStudentDatatable(DataTablesStructs.SentParameters sentParameters, int gradeType, string userId, string searchValue = null)
            => await _gradeReportRepository.GetGradeReportByStudentDatatable(sentParameters,gradeType,userId,searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, int gradeType, string searchValue = null)
        {
            return await _gradeReportRepository.GetGradeReportDatatable(sentParameters, careerId, gradeType, searchValue);
        }

        public async Task<GradeReport> GetGradeReportWithIncludes(Guid id)
        {
            return await _gradeReportRepository.GetGradeReportWithIncludes(id);
        }

        public async Task<string> GetPromotionGrade(Guid studentId)
        {
            return await _gradeReportRepository.GetPromotionGrade(studentId);
        }

        public async Task<StudentInfoTemplate> GetStudentByStudentId(Guid studentId)
        {
            return await _gradeReportRepository.GetStudentByStudentId(studentId);
        }

        public async Task<object> GetStudentByUserName(string username)
        {
            return await _gradeReportRepository.GetStudentByUserName(username);
        }

        public async Task<object> GetStudentByUserNameBachelor(string username)
        {
            return await _gradeReportRepository.GetStudentByUserNameBachelor(username);
        }

        public async Task Insert(GradeReport model)
        {
            await _gradeReportRepository.Insert(model);
        }

        public Task SaveChanges()
            => _gradeReportRepository.SaveChanges();

        public async Task Update(GradeReport model)
        {
            await _gradeReportRepository.Update(model);
        }
    }
}
