using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryTemaryService : IPreuniversitaryTemaryService
    {
        private readonly IPreuniversitaryTemaryRepository _preuniversitaryTemaryRepository;

        public PreuniversitaryTemaryService(IPreuniversitaryTemaryRepository preuniversitaryTemaryRepository)
        {
            _preuniversitaryTemaryRepository = preuniversitaryTemaryRepository;
        }

        public async Task Delete(PreuniversitaryTemary entity)
            => await _preuniversitaryTemaryRepository.Delete(entity);

        public async Task<PreuniversitaryTemary> Get(Guid id)
            => await _preuniversitaryTemaryRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTemariesDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, string searchValue = null)
            => await _preuniversitaryTemaryRepository.GetTemariesDatatable(sentParameters, courseId, termId, searchValue);

        public async Task<object> GetTemariesListByCourseIdAndTermId(Guid courseId, Guid termId)
            => await _preuniversitaryTemaryRepository.GetTemariesListByCourseIdAndTermId(courseId, termId);

        public async Task Insert(PreuniversitaryTemary entity)
            => await _preuniversitaryTemaryRepository.Insert(entity);
    }
}
