using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringSuggestionService : ITutoringSuggestionService
    {
        private readonly ITutoringSuggestionRepository _tutoringSuggestionRepository;

        public TutoringSuggestionService(ITutoringSuggestionRepository tutoringSuggestionRepository)
        {
            _tutoringSuggestionRepository = tutoringSuggestionRepository;
        }

        public async Task DeleteById(Guid id)
            => await _tutoringSuggestionRepository.DeleteById(id);

        public async Task<TutoringSuggestion> Get(Guid id)
            => await _tutoringSuggestionRepository.Get(id);
        
        public async Task<DataTablesStructs.ReturnedData<object>> GetTutoringSuggestionsDatatable(DataTablesStructs.SentParameters sentParameters, byte? status = null, string searchValue = null, string role = null)
            => await _tutoringSuggestionRepository.GetTutoringSuggestionsDatatable(sentParameters, status, searchValue, role);

        public async Task Insert(TutoringSuggestion tutoringSuggestion)
            => await _tutoringSuggestionRepository.Insert(tutoringSuggestion);

        public async Task Update(TutoringSuggestion tutoringSuggestion)
            => await _tutoringSuggestionRepository.Update(tutoringSuggestion);
    }
}
