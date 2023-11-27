using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class LanguageService: ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task Delete(Language language)
        {
            await _languageRepository.Delete(language);
        }

        public async Task<Language> Get(Guid id)
        {
            return await _languageRepository.Get(id);
        }

        public async Task<IEnumerable<Language>> GetAll()
        {
            return await _languageRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetLanguageDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _languageRepository.GetLanguageDatatable(sentParameters,searchValue);
        }

        public async Task<object> GetSelect2CLientSide()
        {
            return await _languageRepository.GetSelect2CLientSide();
        }

        public async Task Insert(Language language)
        {
            await _languageRepository.Insert(language);
        }

        public async Task Update(Language language)
        {
            await _languageRepository.Update(language);
        }
    }
}
