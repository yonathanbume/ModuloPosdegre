using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IJobOfferCareerRepository:IRepository<JobOfferCareer>
    {
        Task<object> JobOfferReport1(bool isCoordinator, List<Guid> careers);
        Task<object> JobOfferReport2(bool isCoordinator, List<Guid> careers);
        Task<IEnumerable<JobOfferCareer>> GetAllByJobOfferId(Guid jobOfferId);
        Task<object> GetJobOfferByCareersAsData(Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, ClaimsPrincipal user = null);
        Task JobOfferReport2Excel(IXLWorksheet worksheet , ClaimsPrincipal user, List<Guid> careers);
        Task<List<JobOfferApplicationExcelReportTemplate>> JobOfferReport2ExcelData(ClaimsPrincipal user, List<Guid> careers);
    }
}
