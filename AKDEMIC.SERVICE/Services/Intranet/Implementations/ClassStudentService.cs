using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassStudent;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ClassStudentService : IClassStudentService
    {
        private readonly IClassStudentRepository _classStudentRepository;

        public ClassStudentService(IClassStudentRepository classStudentRepository)
        {
            _classStudentRepository = classStudentRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<SectionAbsenceDetailDataTableTemplate>> GetSectionAbsenceDetailDataTable(DataTablesStructs.SentParameters parameters, Guid sid, Guid aid, int filter)
            => await _classStudentRepository.GetSectionAbsenceDetailDataTable(parameters, sid, aid, filter);
        
        public async Task Delete(ClassStudent classStudent)
            => await _classStudentRepository.Delete(classStudent);

        public async Task<ClassStudent> Get(Guid classStudentId)
            => await _classStudentRepository.Get(classStudentId);

        public async Task<IEnumerable<object>> GetStudentAssistances(Class @class, string teacherId, Guid? classId = null)
           => await _classStudentRepository.GetStudentAssistances(@class, teacherId, classId);

        public async Task<IEnumerable<ClassStudent>> GetAll()
            => await _classStudentRepository.GetAll();

        public async Task<IEnumerable<ClassStudent>> GetAll(Guid? sectionId = null, Guid? studentId = null, DateTime? startTime = null, DateTime? endTime = null, bool? absent = null, Guid? classId = null, string teacherId = null)
            => await _classStudentRepository.GetAll(sectionId, studentId, startTime, endTime, absent, classId, teacherId);

        public async Task Insert(ClassStudent classStudent)
            => await _classStudentRepository.Insert(classStudent);

        public async Task Update(ClassStudent classStudent)
            => await _classStudentRepository.Update(classStudent);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesAssistanceDetailByStudentAndSectionDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid sectionId)
            => await _classStudentRepository.GetCoursesAssistanceDetailByStudentAndSectionDatatable(parameters, studentId, sectionId);

        public async Task<IEnumerable<Select2Structs.Result>> GetClassStudentSelect2ClientSide(string userId, Guid sectionId, bool isAbsent = false)
            => await _classStudentRepository.GetClassStudentSelect2ClientSide(userId, sectionId, isAbsent);

        public async Task<List<ClassStudent>> GetClassesBySectionId(Guid sectionId)
            => await _classStudentRepository.GetClassesBySectionId(sectionId);

        public async Task CreateClassStudentsJob(Guid? sectionId, Guid id)
            => await _classStudentRepository.CreateClassStudentsJob(sectionId, id);

        public async Task<bool> AnyAssistanceByClassScheduleId(Guid classScheduleId, Guid sectionId)
            => await _classStudentRepository.AnyAssistanceByClassScheduleId(classScheduleId, sectionId);

        public async Task<object> GetClassStudentsOldAssistance(Guid classId, string teacherId = null)
            => await _classStudentRepository.GetClassStudentsOldAssistance(classId, teacherId);

        public async Task AssignDPI(Guid sectionId)
            => await _classStudentRepository.AssignDPI(sectionId);

        public async Task<List<ClassStudentReportTemplate>> GetClassStudentReport(Guid sectionId)
            => await _classStudentRepository.GetClassStudentReport(sectionId);

        public async Task<List<ClassStudent>> GetClassStudentsByStudentSection(Guid studentId, Guid sectionId)
            => await _classStudentRepository.GetClassStudentsByStudentSection(studentId, sectionId);

    }
}
