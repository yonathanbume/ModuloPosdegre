using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class ScholarshipService : IScholarshipService
    {
        private readonly IScholarshipRepository _scholarshipRepository;

        public ScholarshipService(IScholarshipRepository scholarshipRepository)
        {
            _scholarshipRepository = scholarshipRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters sentParameters,ClaimsPrincipal User, byte? program = null, string startDate = null, byte? status = null, string search = null)
            => await _scholarshipRepository.GetScholarshipDatatable(sentParameters,User, program, startDate, status, search);

        public async Task<Scholarship> Get(Guid id)
            => await _scholarshipRepository.Get(id);

        public async Task Insert(Scholarship entity)
            => await _scholarshipRepository.Insert(entity);

        public async Task Delete(Scholarship entity)
            => await _scholarshipRepository.Delete(entity);

        public async Task Update(Scholarship entity)
            => await _scholarshipRepository.Update(entity);

        public async Task<object> GetScholarshipSelect2ClientSide(Guid? selectedId = null)
            => await _scholarshipRepository.GetScholarshipSelect2ClientSide(selectedId);

        public async Task<IEnumerable<Scholarship>> GetAll()
            => await _scholarshipRepository.GetAll();

        public async Task<bool> HasPostulants(Guid scholarshipId)
            => await _scholarshipRepository.HasPostulants(scholarshipId);

        public async Task<bool> HasPostulations(Guid scholarshipId)
            => await _scholarshipRepository.HasPostulations(scholarshipId);
    }
}
