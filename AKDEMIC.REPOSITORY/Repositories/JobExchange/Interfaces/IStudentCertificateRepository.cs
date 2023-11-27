using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IStudentCertificateRepository:IRepository<StudentCertificate>
    {
        Task<IEnumerable<StudentCertificate>> GetAllByStudent(Guid studentId);
        Task<List<CertificateDate>> GetAllByStudentTemplate(Guid studentId);
    }
}
