using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ResearchLineHistoricService : IResearchLineHistoricService
    {
        private readonly IResearchLineHistoricRepository _researchLineHistoricRepository;
        public async Task UpdateLine(Guid lineId) {
            await _researchLineHistoricRepository.UpdateLine(lineId);
        }

        public async Task DeleteHistoric(Guid lineId)
        {
            await _researchLineHistoricRepository.DeleteHistoric(lineId);
        }
        public ResearchLineHistoricService(IResearchLineHistoricRepository researchLineHistoricRepository)
        {
            _researchLineHistoricRepository = researchLineHistoricRepository;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetHistoricDatatable(DataTablesStructs.SentParameters sentParameters, Guid lineId)
        {
            return await _researchLineHistoricRepository.GetHistoricDatatable(sentParameters, lineId);
        }
        public async Task DeleteById(Guid id)
        {
            await _researchLineHistoricRepository.DeleteById(id);
        }
        public async Task Insert(ResearchLineHistoric researchLineHistoric)
        {
            await _researchLineHistoricRepository.Insert(researchLineHistoric);
        }
        public async Task Update(ResearchLineHistoric researchLineHistoric)
        {
            await _researchLineHistoricRepository.Update(researchLineHistoric);
        }
    }
}
