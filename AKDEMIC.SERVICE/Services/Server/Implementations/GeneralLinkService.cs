using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Server;
using AKDEMIC.REPOSITORY.Repositories.Server.Interfaces;
using AKDEMIC.SERVICE.Services.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Server.Implementations
{
    public class GeneralLinkService : IGeneralLinkService
    {
        private readonly IGeneralLinkRepository _repository;

        public GeneralLinkService(IGeneralLinkRepository repository)
        {
            _repository = repository;
        }

        public async Task Delete(GeneralLink entity)
            => await _repository.Delete(entity);

        public async Task<GeneralLink> Get(Guid id)
            => await _repository.Get(id);

        public async Task<IEnumerable<GeneralLink>> GetAll(byte? type = null, ClaimsPrincipal user = null)
            => await _repository.GetAll(type, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, byte? type)
            => await _repository.GetDatatable(parameters, type);

        public async Task Insert(GeneralLink entity)
            => await _repository.Insert(entity);

        public async Task Update(GeneralLink entity)
            => await _repository.Update(entity);
    }
}
