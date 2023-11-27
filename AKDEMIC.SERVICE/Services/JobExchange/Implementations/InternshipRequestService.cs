using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class InternshipRequestService : IInternshipRequestService
    {
        private readonly IInternshipRequestRepository _internshipRequestRepository ;

        public InternshipRequestService(IInternshipRequestRepository internshipRequestRepository)
        {
            _internshipRequestRepository = internshipRequestRepository;
        }

        public Task<bool> AnyByStudentExperience(Guid studentExperienceId)
            => _internshipRequestRepository.AnyByStudentExperience(studentExperienceId);

        public async Task<InternshipRequest> Get(Guid id)
        {
            return await _internshipRequestRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternshipEmployeeDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId = null, string searchValue = null)
            => await _internshipRequestRepository.GetInternshipEmployeeDatatable(sentParameters,userId, careerId,searchValue);

        public Task<List<InternshipRequestTemplate>> GetInternshipRequestData(List<Guid> careers)
            => _internshipRequestRepository.GetInternshipRequestData(careers);

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternshipRequestDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            return await _internshipRequestRepository.GetInternshipRequestDatatable(sentParameters, userId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternshipRequestDatatableTotal(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, int ConvalidationType = 0, string searchValue = null)
        {
            return await _internshipRequestRepository.GetInternshipRequestDatatableTotal(sentParameters, careers, ConvalidationType, searchValue);
        }

        public Task<List<InternshipRequestTemplate>> GetInternshipRequestStudentData(Guid studentId)
            => _internshipRequestRepository.GetInternshipRequestStudentData(studentId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentExperienceDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            return await _internshipRequestRepository.GetStudentExperienceDatatable(sentParameters, userId, searchValue);
        }

        public async Task Insert(InternshipRequest model)
        {
            await _internshipRequestRepository.Insert(model);
        }

        public async Task Update(InternshipRequest model)
        {
            await _internshipRequestRepository.Update(model);
        }
    }
}
