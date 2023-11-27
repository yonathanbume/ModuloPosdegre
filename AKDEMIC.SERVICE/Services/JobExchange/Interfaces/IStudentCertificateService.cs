using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IStudentCertificateService
    {
        Task<StudentCertificate> Get(Guid id);
        Task<IEnumerable<StudentCertificate>> GetAllByStudent(Guid studentId);
        Task DeleteRange(IEnumerable<StudentCertificate> studentCertificates);
        Task InsertRange(IEnumerable<StudentCertificate> studentCertificates);
        Task<List<CertificateDate>> GetAllByStudentTemplate(Guid studentId);
    }
}
