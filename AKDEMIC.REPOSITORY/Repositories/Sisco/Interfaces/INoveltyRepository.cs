using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces
{
    public interface INoveltyRepository : IRepository<Novelty>
    {
        Task<DataTablesStructs.ReturnedData<NoveltyTemplate>> GetAllNoveltyDatatable(DataTablesStructs.SentParameters sentParameters, string name = null);
        Task<List<NoveltyTemplate>> GetNoveltyToHome();
        Task<List<NewTemplate>> GetNewToHome();
    }
}
