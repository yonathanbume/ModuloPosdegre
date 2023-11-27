using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class ScholarshipStudentService : IScholarshipStudentService
    {
        private readonly IScholarshipStudentRepository _scholarshipStudentRepository;

        public ScholarshipStudentService(IScholarshipStudentRepository scholarshipStudentRepository)
        {
            _scholarshipStudentRepository = scholarshipStudentRepository;
        }

        public async Task<ScholarshipStudent> Get(Guid id)
            => await _scholarshipStudentRepository.Get(id);

        public async Task<ScholarshipStudent> GetScholarshipStudent(Guid studentId, Guid scholarshipId)
            => await _scholarshipStudentRepository.GetScholarshipStudent(studentId, scholarshipId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetScholarshipStudentDatatable(DataTablesStructs.SentParameters parameters, Guid scholarshipId, string searchValue, byte? status = null)
            => await _scholarshipStudentRepository.GetScholarshipStudentDatatable(parameters, scholarshipId, searchValue, status);

        public async Task<List<ScholarshipStudentRequirement>> GetScholarshipStudentRequirements(Guid id)
            => await _scholarshipStudentRepository.GetScholarshipStudentRequirements(id);

        public async Task Insert(ScholarshipStudent entity)
            => await _scholarshipStudentRepository.Insert(entity);

        public async Task<bool> IsApplicant(Guid studentId, Guid scholarshipId)
            => await _scholarshipStudentRepository.IsApplicant(studentId, scholarshipId);

        public async Task Update(ScholarshipStudent entity)
            => await _scholarshipStudentRepository.Update(entity);
    }
}
