using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces;
using AKDEMIC.SERVICE.Services.PreprofesionalPractice.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PreprofesionalPractice.Implementations
{
    public class InternshipRequestService : IInternshipRequestService
    {
        private readonly IInternshipRequestRepository _internshipRequestRepository;
        public InternshipRequestService(IInternshipRequestRepository internshipRequestRepository)
        {
            _internshipRequestRepository = internshipRequestRepository;
        }

        public async Task Delete(InternshipRequest internshipValidationRequest)
            => await _internshipRequestRepository.Delete(internshipValidationRequest);

        public async Task DeleteById(Guid id)
            => await _internshipRequestRepository.DeleteById(id);

        public async Task<InternshipRequest> Get(Guid id)
            => await _internshipRequestRepository.Get(id);

        public async Task<IEnumerable<InternshipRequest>> GetAll()
            => await _internshipRequestRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
            => await _internshipRequestRepository.GetDataDatatable(sentParameters, searchValue, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatableBySupervisor(DataTablesStructs.SentParameters sentParameters, byte? status, string supervisorId, string searchValue = null, ClaimsPrincipal user = null)
            => await _internshipRequestRepository.GetDatatableBySupervisor(sentParameters, status, supervisorId, searchValue, user);

        public async Task Insert(InternshipRequest internshipValidationRequest)
            => await _internshipRequestRepository.Insert(internshipValidationRequest);

        public async Task Update(InternshipRequest internshipValidationRequest)
            => await _internshipRequestRepository.Update(internshipValidationRequest);
    }
}
