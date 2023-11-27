using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringSuggestionService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTutoringSuggestionsDatatable(DataTablesStructs.SentParameters sentParameters, byte? status = null, string searchValue = null, string role = null);
        Task<TutoringSuggestion> Get(Guid id);
        Task Insert(TutoringSuggestion tutoringSuggestion);
        Task Update(TutoringSuggestion tutoringSuggestion);
        Task DeleteById(Guid id);
    }
}
