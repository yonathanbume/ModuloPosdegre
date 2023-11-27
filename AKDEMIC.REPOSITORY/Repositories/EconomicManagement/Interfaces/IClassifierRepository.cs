using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Classifier;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IClassifierRepository : IRepository<Classifier>
    {
        Task<IEnumerable<Classifier>> GetClassifiers();
        Task<DataTablesStructs.ReturnedData<Classifier>> GetClassifiersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetClassifiersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetClassifiersDatatableEconomic(DataTablesStructs.SentParameters sentParameters, string search);
        Task<object> GetClassifiersEconomicBy(Guid id);
        Task<object> GetClassifiersEdit(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetConceptsByClassifierDatatable(DataTablesStructs.SentParameters sentParameters, Guid classifierId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetClassifiersDatatableReport(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null, bool showAll = false);
        Task<decimal> GetClassifiersDatatableReportTotalAmount(string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null);
        Task<List<ClassifierTemplate>> GetClassifiersReportData(string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null, bool showAll = false);
        Task<object> GetAreaOrCareerJson();
        Task<Classifier> GetByCode(string code);
        Task<List<PaymentTemplate>> GetPaymentsByClassifierId(Guid classifierId, DateTime? startDate = null, DateTime? endDate = null, int? type = null);
        Task UpdateClassifierRelationsJob();
        Task Update();
    }
}
