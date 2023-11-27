using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IStudentAcademicEducationService
    {
        Task<IEnumerable<StudentAcademicEducation>> GetAllByStudent(Guid studentId);
        Task<List<AcademicFormationDate>> GetAllByStudentTemplate(Guid studentId);
        Task DeleteRange(IEnumerable<StudentAcademicEducation> studentAcademicEducations);
        Task InsertRange(IEnumerable<StudentAcademicEducation> studentAcademicEducations);
    }
}
