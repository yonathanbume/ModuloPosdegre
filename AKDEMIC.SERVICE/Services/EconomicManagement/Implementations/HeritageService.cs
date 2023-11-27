using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class HeritageService : IHeritageService
    {
        private readonly IHeritageRepository _heritageRepository;

        public HeritageService(IHeritageRepository heritageRepository)
        {
            _heritageRepository = heritageRepository;
        }

        public async Task<Heritage> Get(Guid catalogItemId, Guid dependencyId)
            => await _heritageRepository.Get(catalogItemId, dependencyId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetHeritageDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, string search)
            => await _heritageRepository.GetHeritageDatatable(parameters, user, search);

        public async Task<List<Heritage>> GetHeritages(ClaimsPrincipal user)
            => await _heritageRepository.GetHeritages(user);

        public async Task Update(Heritage entity)
            => await _heritageRepository.Update(entity);
    }
}
