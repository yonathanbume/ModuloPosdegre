using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface ICompanyInterestRepository : IRepository<CompanyInterest>
    {
        Task<DataTablesStructs.ReturnedData<CompanyInterest>> GetCompanyInterestDatatable(DataTablesStructs.SentParameters sentParameters, string company, string startDate, string endDate, Guid? lineId);
        Task<DataTablesStructs.ReturnedData<CompanyInterest>> GetCompanyInterestProjectDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId);
        Task<CompanyInterest> GetByCompanyIdAndProjectId(Guid companyId, Guid projectId);
    }
}
