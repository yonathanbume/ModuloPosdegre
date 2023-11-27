using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class SedeService : ISedeService
    {
        private readonly ISedeRepository _sedeRepository;
        public SedeService(ISedeRepository sedeRepository)
        {
            _sedeRepository = sedeRepository;
        }

        public async Task DeleteRange(IEnumerable<Sede> sedes)
        {
            await _sedeRepository.DeleteRange(sedes);
        }

        public async Task<IEnumerable<Sede>> GetListSedesByCompany(Guid companyId)
        {
            return await _sedeRepository.GetListSedesByCompany(companyId);
        }

        public async Task<object> GetSedesByCompany(Guid companyId)
        {
            return await _sedeRepository.GetSedesByCompany(companyId);
        }

        public Task<object> GetSedesByUser(string userId)
            => _sedeRepository.GetSedesByUser(userId);


        public async Task InsertRange(IEnumerable<Sede> sedes)
        {
            await _sedeRepository.InsertRange(sedes);
        }
    }
}
