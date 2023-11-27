using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringSuggestionRepository : IRepository<TutoringSuggestion>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTutoringSuggestionsDatatable(DataTablesStructs.SentParameters sentParameters, byte? status = null, string searchValue = null, string role = null);
    }
}
