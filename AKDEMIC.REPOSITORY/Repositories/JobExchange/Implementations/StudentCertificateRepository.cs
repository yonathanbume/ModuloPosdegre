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
    public class StudentCertificateRepository:Repository<StudentCertificate> , IStudentCertificateRepository
    {
        public StudentCertificateRepository(AkdemicContext context) : base (context){ }

        public async Task<IEnumerable<StudentCertificate>> GetAllByStudent(Guid studentId)
        {
            var query = _context.StudentCertificates
                    .Where(x => x.StudentId == studentId);

            return await query.ToListAsync();
        }

        public async Task<List<CertificateDate>> GetAllByStudentTemplate(Guid studentId)
        {
            var result = await _context.StudentCertificates
                .Where(x => x.StudentId == studentId)
                .Select(x => new CertificateDate
                {
                    Description = x.Description,
                    Institution = x.Institution
                })
                .ToListAsync();

            return result;
        }
    }
}
