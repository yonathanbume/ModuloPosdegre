using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyCompetitionService : ITransparencyCompetitionService
    {
        private readonly ITransparencyCompetitionRepository _transparencyCompetitionRepository;
        public TransparencyCompetitionService(ITransparencyCompetitionRepository transparencyCompetitionRepository)
        {
            _transparencyCompetitionRepository = transparencyCompetitionRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _transparencyCompetitionRepository.DeleteById(id);
        }

        public async  Task<TransparencyCompetition> Get(Guid id)
        {
            return await _transparencyCompetitionRepository.Get(id);
        }

        public async Task<IEnumerable<TransparencyCompetition>> GetAll()
        {
            return await _transparencyCompetitionRepository.GetAll();
        }

        public async  Task<DataTablesStructs.ReturnedData<object>> GetListCompetition(DataTablesStructs.SentParameters paginationParameter, int year)
        {
            return await _transparencyCompetitionRepository.GetListCompetition(paginationParameter, year);
        }

        public async Task Insert(TransparencyCompetition regulation)
        {
            await _transparencyCompetitionRepository.Insert(regulation);
        }

        public async Task Update(TransparencyCompetition regulation)
        {
            await _transparencyCompetitionRepository.Update(regulation);
        }
    }
}
