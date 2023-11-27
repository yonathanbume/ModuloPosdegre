using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Interfaces
{
    public interface INoveltyService
    {
        Task InsertNovelty(Novelty novelty);
        Task UpdateNovelty(Novelty novelty);
        Task DeleteNovelty(Novelty novelty);
        Task<Novelty> GetNoveltyById(Guid id);
        Task<IEnumerable<Novelty>> GetAllNovelties();
        Task<DataTablesStructs.ReturnedData<NoveltyTemplate>> GetAllNoveltyDatatable(DataTablesStructs.SentParameters sentParameters, string name);
        Task<List<NoveltyTemplate>> GetNoveltyToHome();
        Task<List<NewTemplate>> GetNewToHome();
    }
}
