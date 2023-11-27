using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class StudentAcademicEducationService: IStudentAcademicEducationService
    {
        private readonly IStudentAcademicEducationRepository _studentAcademicEducationRepository;

        public StudentAcademicEducationService(IStudentAcademicEducationRepository studentAcademicEducationRepository)
        {
            _studentAcademicEducationRepository = studentAcademicEducationRepository;
        }

        public async Task DeleteRange(IEnumerable<StudentAcademicEducation> studentAcademicEducations)
        {
            await _studentAcademicEducationRepository.DeleteRange(studentAcademicEducations);
        }

        public async Task<IEnumerable<StudentAcademicEducation>> GetAllByStudent(Guid studentId)
        {
            return await _studentAcademicEducationRepository.GetAllByStudent(studentId);
        }

        public async Task<List<AcademicFormationDate>> GetAllByStudentTemplate(Guid studentId)
        {
            return await _studentAcademicEducationRepository.GetAllByStudentTemplate(studentId);
        }

        public async Task InsertRange(IEnumerable<StudentAcademicEducation> studentAcademicEducations)
        {
            await _studentAcademicEducationRepository.InsertRange(studentAcademicEducations);
        }
    }
}
