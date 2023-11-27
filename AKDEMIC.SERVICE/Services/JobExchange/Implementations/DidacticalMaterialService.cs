using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class DidacticalMaterialService : IDidacticalMaterialService
    {
        private readonly IDidacticalMaterialRepository _didacticalMaterialRepository;

        public DidacticalMaterialService(IDidacticalMaterialRepository didacticalMaterialRepository)
        {
            _didacticalMaterialRepository = didacticalMaterialRepository;
        }

        public Task Delete(DidacticalMaterial didacticalMaterial)
            => _didacticalMaterialRepository.Delete(didacticalMaterial);

        public Task<DidacticalMaterial> Get(Guid id)
            => _didacticalMaterialRepository.Get(id);

        public Task<IEnumerable<DidacticalMaterial>> GetAll()
            => _didacticalMaterialRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDidacticalMaterialsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _didacticalMaterialRepository.GetAllDidacticalMaterialsDatatable(sentParameters, searchValue);

        public Task Insert(DidacticalMaterial didacticalMaterial)
            => _didacticalMaterialRepository.Insert(didacticalMaterial);

        public Task Update(DidacticalMaterial didacticalMaterial)
            => _didacticalMaterialRepository.Update(didacticalMaterial);
    }
}
