using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareScholarshipFormatService : IInstitutionalWelfareScholarshipFormatService
    {
        private readonly IInstitutionalWelfareScholarshipFormatRepository _repository;

        public InstitutionalWelfareScholarshipFormatService(IInstitutionalWelfareScholarshipFormatRepository repository)
        {
            _repository = repository;
        }

        public async Task Add(InstitutionalWelfareScholarshipFormat entity)
            => await _repository.Add(entity);

        public async Task Delete(InstitutionalWelfareScholarshipFormat entity)
            => await _repository.Delete(entity);

        public async Task DeleteRange(IEnumerable<InstitutionalWelfareScholarshipFormat> entity)
            => await _repository.DeleteRange(entity);

        public async Task<InstitutionalWelfareScholarshipFormat> Get(Guid id)
            => await _repository.Get(id);

        public async Task<List<InstitutionalWelfareScholarshipFormat>> GetAllByScholarship(Guid scholarshipId)
            => await _repository.GetAllByScholarship(scholarshipId);

        public async Task Insert(InstitutionalWelfareScholarshipFormat entity)
            => await _repository.Insert(entity);

        public async Task Update(InstitutionalWelfareScholarshipFormat entity)
            => await _repository.Update(entity);
    }
}
