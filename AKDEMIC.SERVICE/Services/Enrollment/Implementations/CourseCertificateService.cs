using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CourseCertificateService : ICourseCertificateService
    {
        private readonly ICourseCertificateRepository _courseCertificateRepository;
        public CourseCertificateService(ICourseCertificateRepository courseCertificateRepository)
        {
            _courseCertificateRepository = courseCertificateRepository;
        }

        public async Task DeleteById(Guid id) => await _courseCertificateRepository.DeleteById(id);

        public async Task<CourseCertificate> Get(Guid id) => await _courseCertificateRepository.Get(id);

        public async Task<IEnumerable<CourseCertificate>> GetAll()
            => await _courseCertificateRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue) 
            => await _courseCertificateRepository.GetDatatable(sentParameters, searchValue);

        public async Task<object> GetSelect2ClientSide() => await _courseCertificateRepository.GetSelect2ClientSide();

        public async Task Insert(CourseCertificate courseCertificate) => await _courseCertificateRepository.Insert(courseCertificate);

        public async Task Update(CourseCertificate courseCertificate) => await _courseCertificateRepository.Update(courseCertificate);
    }
}
