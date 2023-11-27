using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryTermService : IPreuniversitaryTermService
    {
        private readonly IPreuniversitaryTermRepository _preuniversitaryTermRepository;

        public PreuniversitaryTermService(IPreuniversitaryTermRepository preuniversitaryTermRepository)
        {
            _preuniversitaryTermRepository = preuniversitaryTermRepository;
        }

        public async Task<bool> AnyByDateTimeConflict(DateTime parsedStartDate, DateTime parsedEndDate, Guid? ignoredId = null)
            => await _preuniversitaryTermRepository.AnyByDateTimeConflict(parsedStartDate, parsedEndDate, ignoredId);

        public async Task Delete(PreuniversitaryTerm entity)
            => await _preuniversitaryTermRepository.Delete(entity);

        public async Task<PreuniversitaryTerm> Get(Guid id)
            => await _preuniversitaryTermRepository.Get(id);

        public async Task<PreuniversitaryTerm> GetActivePreUniversitaryApplicationTerm()
            => await _preuniversitaryTermRepository.GetActivePreUniversitaryApplicationTerm();

        public async Task<object> GetCurrentPreuniversitaryTerm()
            => await _preuniversitaryTermRepository.GetCurrentPreuniversitaryTerm();

        public async Task<object> GetPreunviersitaryTermSelect2ClientSide()
            => await _preuniversitaryTermRepository.GetPreunviersitaryTermSelect2ClientSide();

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue = null)
            => await _preuniversitaryTermRepository.GetReportDatatable(sentParameters, preuniversitaryTermId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTermDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _preuniversitaryTermRepository.GetTermDatatable(sentParameters, searchValue);

        public async Task Insert(PreuniversitaryTerm entity)
            => await _preuniversitaryTermRepository.Insert(entity);

        public async Task Update(PreuniversitaryTerm entity)
            => await _preuniversitaryTermRepository.Update(entity);
    }
}
 