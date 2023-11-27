using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ExternalProcedureService : IExternalProcedureService
    {
        private readonly IExternalProcedureRepository _externalProcedureRepository;

        public ExternalProcedureService(IExternalProcedureRepository externalProcedureRepository)
        {
            _externalProcedureRepository = externalProcedureRepository;
        }

        public async Task<bool> AnyExternalProcedureByCode(string code)
        {
            return await _externalProcedureRepository.AnyExternalProcedureByCode(code);
        }

        public async Task<ExternalProcedure> Get(Guid id)
        {
            return await _externalProcedureRepository.Get(id);
        }

        public async Task<ExternalProcedure> GetExternalProcedure(Guid id)
        {
            return await _externalProcedureRepository.GetExternalProcedure(id);
        }

        public async Task<IEnumerable<ExternalProcedure>> GetExternalProcedures()
        {
            return await _externalProcedureRepository.GetExternalProcedures();
        }

        public async Task<IEnumerable<ExternalProcedure>> GetExternalProceduresByDependency(Guid dependencyId)
        {
            return await _externalProcedureRepository.GetExternalProceduresByDependency(dependencyId);
        }

        public async Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _externalProcedureRepository.GetExternalProceduresDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatableByClassifier(DataTablesStructs.SentParameters sentParameters, Guid classifierId, string searchValue = null)
        {
            return await _externalProcedureRepository.GetExternalProceduresDatatableByClassifier(sentParameters, classifierId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatableByClassifier(DataTablesStructs.SentParameters sentParameters, Guid classifierId, Guid dependencyId, string searchValue = null)
        {
            return await _externalProcedureRepository.GetExternalProceduresDatatableByClassifier(sentParameters, classifierId, dependencyId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatableByDependency(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string searchValue = null)
        {
            return await _externalProcedureRepository.GetExternalProceduresDatatableByDependency(sentParameters, dependencyId, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _externalProcedureRepository.GetExternalProceduresSelect2(requestParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2ByClassifier(Select2Structs.RequestParameters requestParameters, Guid classifierId, string searchValue = null)
        {
            return await _externalProcedureRepository.GetExternalProceduresSelect2ByClassifier(requestParameters, classifierId, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2ByClassifier(Select2Structs.RequestParameters requestParameters, Guid classifierId, Guid dependencyId, string searchValue = null)
        {
            return await _externalProcedureRepository.GetExternalProceduresSelect2ByClassifier(requestParameters, classifierId, dependencyId, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2ByDependency(Select2Structs.RequestParameters requestParameters, Guid dependencyId, string searchValue = null)
        {
            return await _externalProcedureRepository.GetExternalProceduresSelect2ByDependency(requestParameters, dependencyId, searchValue);
        }

        public async Task Delete(ExternalProcedure externalProcedure)
        {
            await _externalProcedureRepository.Delete(externalProcedure);
        }

        public async Task Insert(ExternalProcedure externalProcedure)
        {
            await _externalProcedureRepository.Insert(externalProcedure);
        }

        public async Task Update(ExternalProcedure externalProcedure)
        {
            await _externalProcedureRepository.Update(externalProcedure);
        }


















        public async Task<bool> HasAnyUserExternalProcedure(Guid externalProcedureId)
        {
            return await _externalProcedureRepository.HasAnyUserExternalProcedure(externalProcedureId);
        }

        public async Task<bool> IsCodeDuplicated(string code)
        {
            return await _externalProcedureRepository.IsCodeDuplicated(code);
        }

        public async Task<bool> IsCodeDuplicated(string code, Guid externalProcedureId)
        {
            return await _externalProcedureRepository.IsCodeDuplicated(code, externalProcedureId);
        }

        public async Task<Tuple<int, List<ExternalProcedure>>> GetExternalProcedures(DataTablesStructs.SentParameters sentParameters)
        {
            return await _externalProcedureRepository.GetExternalProcedures(sentParameters);
        }

        public async Task<Tuple<int, List<ExternalProcedure>>> GetExternalProceduresByDependencyId(Guid dependencyId, DataTablesStructs.SentParameters sentParameters)
        {
            return await _externalProcedureRepository.GetExternalProceduresByDependencyId(dependencyId, sentParameters);
        }







        public async Task<List<ExternalProcedure>> GetExternalProceduresBySearchValue(string searchValue)
        {
            return await _externalProcedureRepository.GetExternalProceduresBySearchValue(searchValue);
        }
    }
}
