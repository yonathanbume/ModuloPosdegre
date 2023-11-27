using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<DataTablesStructs.ReturnedData<Document>> GetDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string number = null, string matter = null, Guid? dependencyId = null, string resolutionDate = null, Guid? sorterId = null,Guid? categoryId = null, string startDate = null, string endDate = null, string searchValue = null, ClaimsPrincipal user = null, Guid? facultyId = null, int? year = null, byte? status = null, byte? type = null);
        Task<DataTablesStructs.ReturnedData<Document>> GetDocumentsDatatableByUserId(DataTablesStructs.SentParameters sentParameters, string userId, string numberOfAct = null, string matter = null, Guid? dependencyId = null, string date = null, string searchValue = null);
        Task<IEnumerable<Select2Structs.Result>> GetDependencies();
        Task<Dependency> GetDependencyById(Guid dependencyId);
        Task<IEnumerable<Select2Structs.Result>> GetDependenciesByUserId(string userId);
        Task<IEnumerable<Tuple<int, int>>> GetReportByDependencyId(Guid? dependencyId, Guid? facultyId = null);
        Task<bool> AnyByNumber(string number, Guid? ignoredId = null);
        Task<bool> AnyByResolutionCategoryId(Guid resolutionCategoryId);
        Task<bool> AnyBySorterId(Guid sorterId);
    }
}
