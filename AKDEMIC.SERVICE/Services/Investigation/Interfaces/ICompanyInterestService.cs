using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface ICompanyInterestService
    {
        Task<DataTablesStructs.ReturnedData<CompanyInterest>> GetCompanyInterestDatatable(DataTablesStructs.SentParameters sentParameters, string company, string startDate, string endDate, Guid? lineId);
        Task<DataTablesStructs.ReturnedData<CompanyInterest>> GetCompanyInterestProjectDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId);
        Task<CompanyInterest> GetByCompanyIdAndProjectId(Guid companyId, Guid projectId);
        Task Insert(CompanyInterest entity);
    }
}
