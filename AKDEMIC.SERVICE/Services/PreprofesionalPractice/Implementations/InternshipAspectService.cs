using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces;
using AKDEMIC.SERVICE.Services.PreprofesionalPractice.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PreprofesionalPractice.Implementations
{
    public class InternshipAspectService : IInternshipAspectService
    {
        private readonly IInternshipAspectRepository _internshipAspectRepository;

        public InternshipAspectService(IInternshipAspectRepository internshipAspectRepository)
        {
            _internshipAspectRepository = internshipAspectRepository;
        }

        public async Task Delete(InternshipAspect entity)
            => await _internshipAspectRepository.Delete(entity);

        public async Task<InternshipAspect> Get(Guid id)
            => await _internshipAspectRepository.Get(id);

        public async Task<IEnumerable<InternshipAspect>> GetAllByType(byte? type = null, bool? ignoreQueryFilters = null)
            => await _internshipAspectRepository.GetAllByType(type, ignoreQueryFilters);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, byte type)
            => await _internshipAspectRepository.GetDatatable(parameters, type);

        public async Task Insert(InternshipAspect entity)
            => await _internshipAspectRepository.Insert(entity);

        public async Task Update(InternshipAspect entity)
            => await _internshipAspectRepository.Update(entity);
    }
}
