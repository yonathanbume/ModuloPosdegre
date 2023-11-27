using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareScholarshipRequirementService : IInstitutionalWelfareScholarshipRequirementService
    {
        private readonly IInstitutionalWelfareScholarshipRequirementRepository _repository;

        public InstitutionalWelfareScholarshipRequirementService(IInstitutionalWelfareScholarshipRequirementRepository repository)
        {
            _repository = repository;
        }

        public async Task Delete(InstitutionalWelfareScholarshipRequirement entity)
            => await _repository.Delete(entity);

        public async Task DeleteRange(IEnumerable<InstitutionalWelfareScholarshipRequirement> entities)
            => await _repository.DeleteRange(entities);

        public async Task<InstitutionalWelfareScholarshipRequirement> Get(Guid id)
            => await _repository.Get(id);

        public async Task<List<InstitutionalWelfareScholarshipRequirement>> GetAllByScholarship(Guid scholarshipId)
            => await _repository.GetAllByScholarship(scholarshipId);

        public async Task Insert(InstitutionalWelfareScholarshipRequirement entity)
            => await _repository.Insert(entity);

        public async Task Update(InstitutionalWelfareScholarshipRequirement entity)
            => await _repository.Update(entity);
    }
}
