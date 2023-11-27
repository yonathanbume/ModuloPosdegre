using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class StudentCertificateService: IStudentCertificateService
    {
        private readonly IStudentCertificateRepository _studentCertificateRepository;

        public StudentCertificateService(IStudentCertificateRepository studentCertificateRepository)
        {
            _studentCertificateRepository = studentCertificateRepository;
        }

        public async Task DeleteRange(IEnumerable<StudentCertificate> studentCertificates)
        {
            await _studentCertificateRepository.DeleteRange(studentCertificates);
        }

        public Task<StudentCertificate> Get(Guid id)
            => _studentCertificateRepository.Get(id);

        public async Task<IEnumerable<StudentCertificate>> GetAllByStudent(Guid studentId)
        {
            return await _studentCertificateRepository.GetAllByStudent(studentId);
        }

        public async Task<List<ProfileDetailTemplate.CertificateDate>> GetAllByStudentTemplate(Guid studentId)
        {
            return await _studentCertificateRepository.GetAllByStudentTemplate(studentId);
        }

        public async Task InsertRange(IEnumerable<StudentCertificate> studentCertificates)
        {
            await _studentCertificateRepository.InsertRange(studentCertificates);
        }
    }
}
