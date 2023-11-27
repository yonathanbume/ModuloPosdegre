using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.SanctionedPostulant;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class SanctionedPostulantService : ISanctionedPostulantService
    {
        private readonly ISanctionedPostulantRepository _sanctionedPostulantRepository;

        public SanctionedPostulantService(ISanctionedPostulantRepository sanctionedPostulantRepository)
        {
            _sanctionedPostulantRepository = sanctionedPostulantRepository;
        }

        public async Task<bool> AnyByDni(string dni, Guid? ignoredId = null)
            => await _sanctionedPostulantRepository.AnyByDni(dni, ignoredId);

        public async Task<bool> AnyByDni(string dni, Guid? applicationTermId, Guid? ignoredId = null)
            => await _sanctionedPostulantRepository.AnyByDni(dni,applicationTermId, ignoredId);

        public async Task Delete(SanctionedPostulant entity)
            => await _sanctionedPostulantRepository.Delete(entity);

        public async Task<SanctionedPostulant> Get(Guid id)
            => await _sanctionedPostulantRepository.Get(id);

        public async Task<IEnumerable<SanctionedPostulant>> GetAll()
            => await _sanctionedPostulantRepository.GetAll();

        public async Task<List<SanctionedPostulantTemplate>> GetSanctionedPostulantData(Guid? applicationTermId, string search)
            => await _sanctionedPostulantRepository.GetSanctionedPostulantData(applicationTermId, search);


        public async Task<DataTablesStructs.ReturnedData<object>> GetSanctionedPostulantDatatable(DataTablesStructs.SentParameters parameters, Guid? applicationTermId, string search)
            => await _sanctionedPostulantRepository.GetSanctionedPostulantDatatable(parameters, applicationTermId, search);

        public async Task Insert(SanctionedPostulant entity)
            => await _sanctionedPostulantRepository.Insert(entity);

        public async Task InsertRange(List<SanctionedPostulant> entities)
            => await _sanctionedPostulantRepository.InsertRange(entities);

        public async Task<bool> SanctionedDNI(string dni, Guid applicationTermId)
            => await _sanctionedPostulantRepository.SanctionedDNI(dni, applicationTermId);

        public async Task Update(SanctionedPostulant entity)
            => await _sanctionedPostulantRepository.Update(entity);
    }
}
