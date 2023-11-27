using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ExtracurricularAreaService : IExtracurricularAreaService
    {
        private readonly IExtracurricularAreaRepository _extracurricularAreaRepository;

        public ExtracurricularAreaService(IExtracurricularAreaRepository extracurricularAreaRepository)
        {
            _extracurricularAreaRepository = extracurricularAreaRepository;
        }

        public async Task DeleteById(Guid id)
            => await _extracurricularAreaRepository.DeleteById(id);

        public async Task<ExtracurricularArea> Get(Guid id)
            => await _extracurricularAreaRepository.Get(id);

        public async Task<IEnumerable<ExtracurricularArea>> GetAll()
            => await _extracurricularAreaRepository.GetAll();

        public async Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _extracurricularAreaRepository.GetDataDatatable(sentParameters, search);

        public async Task Insert(ExtracurricularArea extracurricularArea)
            => await _extracurricularAreaRepository.Insert(extracurricularArea);


        public async Task Update(ExtracurricularArea extracurricularArea)
            => await _extracurricularAreaRepository.Update(extracurricularArea);

    }
}
