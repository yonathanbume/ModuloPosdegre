using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class InvestigationParticipationTypeService : IInvestigationParticipationTypeService
    {
        private readonly IInvestigationParticipationTypeRepository _investigationParticipationTypeRepository;

        public InvestigationParticipationTypeService(IInvestigationParticipationTypeRepository investigationParticipationTypeRepository)
        {
            _investigationParticipationTypeRepository = investigationParticipationTypeRepository;
        }

        public async Task Delete(InvestigationParticipationType investigationParticipationType)
            => await _investigationParticipationTypeRepository.Delete(investigationParticipationType);

        public async Task<InvestigationParticipationType> Get(Guid id)
            => await _investigationParticipationTypeRepository.Get(id);

        public async Task<IEnumerable<InvestigationParticipationType>> GetAll()
            => await _investigationParticipationTypeRepository.GetAll();

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _investigationParticipationTypeRepository.AnyByName(name,id);
        public async Task<DataTablesStructs.ReturnedData<object>> GetAllParticipationTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _investigationParticipationTypeRepository.GetAllParticipationTypeDatatable(sentParameters,searchValue);

        public async Task Insert(InvestigationParticipationType investigationParticipationType)
            => await _investigationParticipationTypeRepository.Insert(investigationParticipationType);

        public async Task Update(InvestigationParticipationType investigationParticipationType)
            => await _investigationParticipationTypeRepository.Update(investigationParticipationType);
    }
}
