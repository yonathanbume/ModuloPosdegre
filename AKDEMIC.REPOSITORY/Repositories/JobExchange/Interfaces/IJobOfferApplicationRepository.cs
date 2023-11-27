using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IJobOfferApplicationRepository : IRepository<JobOfferApplication>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetByJobOfferDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, Guid? jobOfferId, DateTime startDate, DateTime endDate, string searchValue = null);
        Task<List<JobOfferApplicationTemplate>> GetByJobOfferData(Guid? jobOfferId, DateTime startDate, DateTime endDate, string searchValue = null);
        Task<List<JobOfferApplicationAgreementReportTemplate>> GetApplicationWithAgreementReportData();
        Task<JobOfferApplication> GetByJobOfferAndStudent(Guid jobOfferId, Guid studentId);
        Task<List<JobOfferApplication>> GetJobOfferApplications(Guid jobOfferId);
        Task<IEnumerable<JobOfferApplication>> GetAllByStudent(Guid studentId);
        Task<IEnumerable<JobOfferApplication>> GetAllWithIncludesByStudent(Guid studentId);
        Task<object> GetJobOfferApplicationSelect2ClientSide(Guid companyId, Guid? careerId = null);
        Task<object> GetJobExchangeStudentWorkTypeReportChart(Guid? facultyId = null, List<Guid> careers = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentWorkTypeReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, List<Guid> careers = null);
        Task<object> GetJobExchangeJobOfferApplicationCareerReportChart(List<Guid> careers = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferApplicationCareerReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers = null);
    }
}
