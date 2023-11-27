using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IJobOfferCareerService
    {
        Task<object> JobOfferReport1(bool isCoordinator, List<Guid> careerId);
        Task<object> JobOfferReport2(bool isCoordinator, List<Guid> careerId);
        Task JobOfferReport2Excel(IXLWorksheet worksheet, ClaimsPrincipal user ,  List<Guid> careers);
        Task<List<JobOfferApplicationExcelReportTemplate>> JobOfferReport2ExcelData(ClaimsPrincipal user, List<Guid> careers);
        Task DeleteRange(IEnumerable<JobOfferCareer> jobOfferCareers);
        Task<IEnumerable<JobOfferCareer>> GetAllByJobOfferId(Guid jobOfferId);
        Task<object> GetJobOfferByCareersAsData(Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, ClaimsPrincipal user = null);
    }
}
