using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Classifier;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IClassifierService
    {
        Task Insert(Classifier classifier);
        Task<Classifier> Add(Classifier classifier);
        Task InsertRange(List<Classifier> classifiers);
        Task<Classifier> Get(Guid id);
        Task<IEnumerable<Classifier>> GetAll();
        Task<IEnumerable<Classifier>> GetClassifiers();
        Task<DataTablesStructs.ReturnedData<object>> GetConceptsByClassifierDatatable(DataTablesStructs.SentParameters sentParameters, Guid classifierId, string search);
        Task<DataTablesStructs.ReturnedData<Classifier>> GetClassifiersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetClassifiersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetClassifiersDatatableEconomic(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<object> GetClassifiersEconomicBy(Guid id);
        Task<object> GetClassifiersEdit(Guid id);
        Task Delete(Classifier classifier);
        Task<DataTablesStructs.ReturnedData<object>> GetClassifiersDatatableReport(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null, bool showAll = false);
        Task<decimal> GetClassifiersDatatableReportTotalAmount(string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null);
        Task<List<ClassifierTemplate>> GetClassifiersReportData(string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null, bool showAll = false);
        Task<object> GetAreaOrCareerJson();
        Task<Classifier> GetByCode(string code);
        Task Update(Classifier classifier);
        Task Update();
        Task UpdateClassifierRelationsJob();
        Task<List<PaymentTemplate>> GetPaymentsByClassifierId(Guid classifierId, DateTime? startDate = null, DateTime? endDate = null, int? type = null);
    }
}
