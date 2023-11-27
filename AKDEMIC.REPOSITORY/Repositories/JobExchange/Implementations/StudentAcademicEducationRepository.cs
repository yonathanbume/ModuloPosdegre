using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class StudentAcademicEducationRepository: Repository<StudentAcademicEducation> , IStudentAcademicEducationRepository
    {
        public StudentAcademicEducationRepository(AkdemicContext context): base(context) { }

        public async Task<IEnumerable<StudentAcademicEducation>> GetAllByStudent(Guid studentId)
        {
            var query = _context.StudentAcademicEducations
                .Where(x => x.StudentId == studentId);

            return await query.ToListAsync();
        }

        public async Task<List<AcademicFormationDate>> GetAllByStudentTemplate(Guid studentId)
        {
            var result = _context.StudentAcademicEducations.Where(x => x.StudentId == studentId).Select(x => new AcademicFormationDate
            {
                AcademicLevel = x.Type,
                Description = x.Description,
                IsStudying = x.IsStudying,
                RangeDate = (x.IsStudying) ? $"{x.StartYear} - {x.EndYear}" : $"{x.StartYear} - Hasta la actualidad"
            });
            return await result.ToListAsync();
        }
    }
}
