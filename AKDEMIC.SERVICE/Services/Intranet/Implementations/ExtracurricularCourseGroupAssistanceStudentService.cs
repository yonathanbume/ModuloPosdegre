using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ExtracurricularCourseGroupAssistanceStudentService : IExtracurricularCourseGroupAssistanceStudentService
    {
        private readonly IExtracurricularCourseGroupAssistanceStudentRepository _extracurricularCourseGroupAssistanceStudentRepository;

        public ExtracurricularCourseGroupAssistanceStudentService(IExtracurricularCourseGroupAssistanceStudentRepository  extracurricularCourseGroupAssistanceStudentRepository)
        {
            _extracurricularCourseGroupAssistanceStudentRepository = extracurricularCourseGroupAssistanceStudentRepository;
        }

        public Task DeleteById(Guid id)
            => _extracurricularCourseGroupAssistanceStudentRepository.DeleteById(id);

        public Task DeleteRange(IEnumerable<ExtracurricularCourseGroupAssistanceStudent> extracurricularCourseGroupAssistanceStudents)
            => _extracurricularCourseGroupAssistanceStudentRepository.DeleteRange(extracurricularCourseGroupAssistanceStudents);

        public Task<ExtracurricularCourseGroupAssistanceStudent> Get(Guid id)
            => _extracurricularCourseGroupAssistanceStudentRepository.Get(id);

        public Task<IEnumerable<ExtracurricularCourseGroupAssistanceStudent>> GetAllByAssistance(Guid assistanceId)
            => _extracurricularCourseGroupAssistanceStudentRepository.GetAllByAssistance(assistanceId);

        public Task Insert(ExtracurricularCourseGroupAssistanceStudent extracurricularCourseGroupAssistanceStudent)
            => _extracurricularCourseGroupAssistanceStudentRepository.Insert(extracurricularCourseGroupAssistanceStudent);

        public Task InsertRange(IEnumerable<ExtracurricularCourseGroupAssistanceStudent> extracurricularCourseGroupAssistanceStudents)
            => _extracurricularCourseGroupAssistanceStudentRepository.InsertRange(extracurricularCourseGroupAssistanceStudents);

        public Task Update(ExtracurricularCourseGroupAssistanceStudent extracurricularCourseGroupAssistanceStudent)
            => _extracurricularCourseGroupAssistanceStudentRepository.Update(extracurricularCourseGroupAssistanceStudent);

        public Task UpdateRange(IEnumerable<ExtracurricularCourseGroupAssistanceStudent> extracurricularCourseGroupAssistanceStudents)
            => _extracurricularCourseGroupAssistanceStudentRepository.UpdateRange(extracurricularCourseGroupAssistanceStudents);
    }
}
