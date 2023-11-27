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
    public class EnrollmentMessageService : IEnrollmentMessageService
    {
        private readonly IEnrollmentMessageRepository _enrollmentMessageRepository;
        public EnrollmentMessageService(IEnrollmentMessageRepository enrollmentMessageRepository)
        {
            _enrollmentMessageRepository = enrollmentMessageRepository;
        }

        public async Task<EnrollmentMessage> Get(Guid id)
        {
            return await _enrollmentMessageRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid careerId, string search)
        {
            return await _enrollmentMessageRepository.GetDataDatatable(sentParameters, careerId, search);
        }

        public Task Insert(EnrollmentMessage message) => _enrollmentMessageRepository.Insert(message);

        public async Task Update(EnrollmentMessage enrollment)
        {
            await _enrollmentMessageRepository.Update(enrollment);
        }
    }
}
