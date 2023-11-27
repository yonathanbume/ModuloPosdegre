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
    public class DidacticalMaterialFileService: IDidacticalMaterialFileService
    {
        private readonly IDidacticalMaterialFileRepository _didacticalMaterialFileRepository;

        public DidacticalMaterialFileService(IDidacticalMaterialFileRepository didacticalMaterialFileRepository)
        {
            _didacticalMaterialFileRepository = didacticalMaterialFileRepository;
        }

        public Task Delete(DidacticalMaterialFile didacticalMaterialFile)
            => _didacticalMaterialFileRepository.Delete(didacticalMaterialFile);

        public Task<DidacticalMaterialFile> Get(Guid id)
            => _didacticalMaterialFileRepository.Get(id);

        public Task<IEnumerable<DidacticalMaterialFile>> GetAll()
            => _didacticalMaterialFileRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDidacticalMaterialFilesDatatable(DataTablesStructs.SentParameters sentParameters, Guid didacticalMaterialId, string searchValue = null)
            => _didacticalMaterialFileRepository.GetAllDidacticalMaterialFilesDatatable(sentParameters, didacticalMaterialId, searchValue);

        public Task Insert(DidacticalMaterialFile didacticalMaterialFile)
            => _didacticalMaterialFileRepository.Insert(didacticalMaterialFile);

        public Task Update(DidacticalMaterialFile didacticalMaterialFile)
            => _didacticalMaterialFileRepository.Update(didacticalMaterialFile);
    }
}
