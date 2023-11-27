using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareScholarshipService : IInstitutionalWelfareScholarshipService
    {
        private readonly IInstitutionalWelfareScholarshipRepository _institutionalWelfareScholarshipRepository;

        public InstitutionalWelfareScholarshipService(IInstitutionalWelfareScholarshipRepository institutionalWelfareScholarshipRepository)
        {
            _institutionalWelfareScholarshipRepository = institutionalWelfareScholarshipRepository;
        }

        public async Task<bool> AnyStudent(Guid scholarshipId)
            => await _institutionalWelfareScholarshipRepository.AnyStudent(scholarshipId);

        public async Task Delete(InstitutionalWelfareScholarship entity)
            => await _institutionalWelfareScholarshipRepository.Delete(entity);

        public async Task<InstitutionalWelfareScholarship> Get(Guid id)
            => await _institutionalWelfareScholarshipRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters parameters, Guid? scholarshipTypeId, string searchValue, ClaimsPrincipal user = null)
            => await _institutionalWelfareScholarshipRepository.GetScholarshipDatatable(parameters, scholarshipTypeId, searchValue, user);

        public async Task<List<ScholarshipStudent>> GetScholarshipStudents(Guid scholarshipId, byte status)
            => await _institutionalWelfareScholarshipRepository.GetScholarshipStudents(scholarshipId, status);

        public async Task Insert(InstitutionalWelfareScholarship entity)
            => await _institutionalWelfareScholarshipRepository.Insert(entity);

        public async Task Update(InstitutionalWelfareScholarship entity)
            => await _institutionalWelfareScholarshipRepository.Update(entity);
    }
}
