using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IStudentAcademicEducationRepository: IRepository<StudentAcademicEducation>
    {
        Task<IEnumerable<StudentAcademicEducation>> GetAllByStudent(Guid studentId);
        Task<List<AcademicFormationDate>> GetAllByStudentTemplate(Guid studentId);
    }
}
