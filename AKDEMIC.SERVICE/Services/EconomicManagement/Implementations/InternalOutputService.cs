using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class InternalOutputService : IInternalOutputService
    {
        private readonly IInternalOutputRepository _internalOutputRepository;

        public InternalOutputService(IInternalOutputRepository internalOutputRepository)
        {
            _internalOutputRepository = internalOutputRepository;
        }

        public async Task Delete(InternalOutput entity)
            => await _internalOutputRepository.Delete(entity);

        public async Task<InternalOutput> Get(Guid id)
            => await _internalOutputRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternalOuputsDatatable(DataTablesStructs.SentParameters parameters, string search)
            => await _internalOutputRepository.GetInternalOuputsDatatable(parameters, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternalOutputItemsDatatable(DataTablesStructs.SentParameters parameters, Guid internalOuputId ,string search)
            => await _internalOutputRepository.GetInternalOutputItemsDatatable(parameters, internalOuputId,search);

        public async Task Insert(InternalOutput entity)
            => await _internalOutputRepository.Insert(entity);

        public async Task Update(InternalOutput entity)
            => await _internalOutputRepository.Update(entity);
    }
}
