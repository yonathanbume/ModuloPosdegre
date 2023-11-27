using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class ScholarshipFileService : IScholarshipFileService
    {
        private readonly IScholarshipFileRepository _scholarshipFileRepository;

        public ScholarshipFileService(IScholarshipFileRepository scholarshipFileRepository)
        {
            _scholarshipFileRepository = scholarshipFileRepository;
        }

        public async Task Delete(ScholarshipFile file)
            => await _scholarshipFileRepository.Delete(file);

        public async Task DeleteRange(IEnumerable<ScholarshipFile> files)
            => await _scholarshipFileRepository.DeleteRange(files);

        public async Task<ScholarshipFile> Get(Guid id)
            => await _scholarshipFileRepository.Get(id);

        public async Task<IEnumerable<ScholarshipFile>> GetAllByScholarshipId(Guid scholarshipId)
            => await _scholarshipFileRepository.GetAllByScholarshipId(scholarshipId);

        public async Task Insert(ScholarshipFile file)
            => await _scholarshipFileRepository.Insert(file);
    }
}
