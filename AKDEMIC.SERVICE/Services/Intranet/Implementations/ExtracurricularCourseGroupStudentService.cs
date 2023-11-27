using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ExtracurricularCourseGroupStudentService : IExtracurricularCourseGroupStudentService
    {
        private readonly IExtracurricularCourseGroupStudentRepository _extracurricularCourseGroupStudentRepository;

        public ExtracurricularCourseGroupStudentService(IExtracurricularCourseGroupStudentRepository extracurricularCourseGroupStudentRepository)
        {
            _extracurricularCourseGroupStudentRepository = extracurricularCourseGroupStudentRepository;
        }

        public async Task<bool> AnyStudentInCourse(Guid studentId, Guid extracurricularCourseId)
            => await _extracurricularCourseGroupStudentRepository.AnyStudentInCourse(studentId, extracurricularCourseId);

        public async Task Delete(ExtracurricularCourseGroupStudent extracurricularCourseGroupStudent)
            => await _extracurricularCourseGroupStudentRepository.Delete(extracurricularCourseGroupStudent);

        public async Task DeleteById(Guid id)
            => await _extracurricularCourseGroupStudentRepository.DeleteById(id);

        public async Task DeleteRange(IEnumerable<ExtracurricularCourseGroupStudent> extracurricularCourseGroupStudents)
            => await _extracurricularCourseGroupStudentRepository.DeleteRange(extracurricularCourseGroupStudents);

        public async Task<ExtracurricularCourseGroupStudent> Get(Guid id)
            => await _extracurricularCourseGroupStudentRepository.Get(id);

        public async Task<IEnumerable<ExtracurricularCourseGroupStudent>> GetAllByGroup(Guid groupId, byte? paymentStatus = null)
            => await _extracurricularCourseGroupStudentRepository.GetAllByGroup(groupId, paymentStatus);

        public async Task<IEnumerable<ExtracurricularCourseGroupStudent>> GetAllByStudent(Guid studentId)
            => await _extracurricularCourseGroupStudentRepository.GetAllByStudent(studentId);

        public async Task Insert(ExtracurricularCourseGroupStudent extracurricularCourseGroupStudent)
            => await _extracurricularCourseGroupStudentRepository.Insert(extracurricularCourseGroupStudent);

        public async Task InsertRange(IEnumerable<ExtracurricularCourseGroupStudent> extracurricularCourseGroupStudents)
            => await _extracurricularCourseGroupStudentRepository.InsertRange(extracurricularCourseGroupStudents);

        public async Task Update(ExtracurricularCourseGroupStudent extracurricularCourseGroupStudent)
            => await _extracurricularCourseGroupStudentRepository.Update(extracurricularCourseGroupStudent);

        public async Task UpdateRange(IEnumerable<ExtracurricularCourseGroupStudent> extracurricularCourseGroupStudents)
            => await _extracurricularCourseGroupStudentRepository.UpdateRange(extracurricularCourseGroupStudents);
    }
}
