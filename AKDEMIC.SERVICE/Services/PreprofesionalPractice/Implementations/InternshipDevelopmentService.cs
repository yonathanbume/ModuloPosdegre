using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces;
using AKDEMIC.SERVICE.Services.PreprofesionalPractice.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PreprofesionalPractice.Implementations
{
    public class InternshipDevelopmentService : IInternshipDevelopmentService
    {
        private readonly IInternshipDevelopmentRepository _repository;

        public InternshipDevelopmentService(IInternshipDevelopmentRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<InternshipDevelopment>> GetByIntershipValidationRequestId(Guid internshipValidationRequestId, byte? type = null)
            => await _repository.GetByIntershipValidationRequestId(internshipValidationRequestId,type);


        public async Task Insert(InternshipDevelopment entity)
            => await _repository.Insert(entity);

        public async Task InsertRange(List<InternshipDevelopment> entities)
            => await _repository.InsertRange(entities);

        public async Task Update(InternshipDevelopment entity)
            => await _repository.Update(entity);
    }
}
