using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using AKDEMIC.SERVICE.Services.Sisco.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Implementations
{
    public class NoveltyService : INoveltyService
    {
        private readonly INoveltyRepository _noveltyRepository;

        public NoveltyService(INoveltyRepository noveltyRepository)
        {
            _noveltyRepository = noveltyRepository;
        }

        public async Task InsertNovelty(Novelty novelty) =>
            await _noveltyRepository.Insert(novelty);

        public async Task UpdateNovelty(Novelty novelty) =>
            await _noveltyRepository.Update(novelty);

        public async Task DeleteNovelty(Novelty novelty) =>
            await _noveltyRepository.Delete(novelty);

        public async Task<Novelty> GetNoveltyById(Guid id) =>
            await _noveltyRepository.Get(id);

        public async Task<IEnumerable<Novelty>> GetAllNovelties() =>
            await _noveltyRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<NoveltyTemplate>> GetAllNoveltyDatatable(DataTablesStructs.SentParameters sentParameters, string name) =>
            await _noveltyRepository.GetAllNoveltyDatatable(sentParameters, name);

        public async Task<List<NoveltyTemplate>> GetNoveltyToHome() =>
            await _noveltyRepository.GetNoveltyToHome();

        public async Task<List<NewTemplate>> GetNewToHome() =>
            await _noveltyRepository.GetNewToHome();
    }
}
