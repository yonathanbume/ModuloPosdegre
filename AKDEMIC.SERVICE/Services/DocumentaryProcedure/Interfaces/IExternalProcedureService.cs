using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IExternalProcedureService
    {
        Task<bool> AnyExternalProcedureByCode(string code);
        Task<ExternalProcedure> Get(Guid id);
        Task<ExternalProcedure> GetExternalProcedure(Guid id);
        Task<IEnumerable<ExternalProcedure>> GetExternalProcedures();
        Task<IEnumerable<ExternalProcedure>> GetExternalProceduresByDependency(Guid dependencyId);
        Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatableByClassifier(DataTablesStructs.SentParameters sentParameters, Guid classifierId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatableByClassifier(DataTablesStructs.SentParameters sentParameters, Guid classifierId, Guid dependencyId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatableByDependency(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2ByClassifier(Select2Structs.RequestParameters requestParameters, Guid classifierId, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2ByClassifier(Select2Structs.RequestParameters requestParameters, Guid classifierId, Guid dependencyId, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2ByDependency(Select2Structs.RequestParameters requestParameters, Guid dependencyId, string searchValue = null);
        Task Delete(ExternalProcedure externalProcedure);
        Task Insert(ExternalProcedure externalProcedure);
        Task Update(ExternalProcedure externalProcedure);













        Task<bool> HasAnyUserExternalProcedure(Guid externalProcedureId);
        Task<bool> IsCodeDuplicated(string code);
        Task<bool> IsCodeDuplicated(string code, Guid externalProcedureId);
        Task<Tuple<int, List<ExternalProcedure>>> GetExternalProcedures(DataTablesStructs.SentParameters sentParameters);
        Task<Tuple<int, List<ExternalProcedure>>> GetExternalProceduresByDependencyId(Guid dependencyId, DataTablesStructs.SentParameters sentParameters);



        Task<List<ExternalProcedure>> GetExternalProceduresBySearchValue(string searchValue);
    }
}