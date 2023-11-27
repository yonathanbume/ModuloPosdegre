using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Classifier;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ClassifierService : IClassifierService
    {
        private readonly IClassifierRepository _classifierRepository;

        public ClassifierService(IClassifierRepository classifierRepository)
        {
            _classifierRepository = classifierRepository;
        }

        public async Task Insert(Classifier classifier)
            => await _classifierRepository.Insert(classifier);
        public async Task InsertRange(List<Classifier> classifiers)
            => await _classifierRepository.InsertRange(classifiers);
        public async Task<Classifier> Get(Guid id)
        {
            return await _classifierRepository.Get(id);
        }

        public async Task<IEnumerable<Classifier>> GetAll()
        {
            return await _classifierRepository.GetAll();
        }

        public async Task<IEnumerable<Classifier>> GetClassifiers()
        {
            return await _classifierRepository.GetClassifiers();
        }

        public async Task<DataTablesStructs.ReturnedData<Classifier>> GetClassifiersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _classifierRepository.GetClassifiersDatatable(sentParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetClassifiersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _classifierRepository.GetClassifiersSelect2(requestParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassifiersDatatableEconomic(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _classifierRepository.GetClassifiersDatatableEconomic(sentParameters, searchValue);

        public async Task<object> GetClassifiersEconomicBy(Guid id)
            => await _classifierRepository.GetClassifiersEconomicBy(id);

        public async Task<object> GetClassifiersEdit(Guid id)
            => await _classifierRepository.GetClassifiersEdit(id);

        public async Task Delete(Classifier classifier)
            => await _classifierRepository.Delete(classifier);

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassifiersDatatableReport(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null, bool showAll = false)
            => await _classifierRepository.GetClassifiersDatatableReport(sentParameters, search, startDate, endDate, type, showAll);
        public async Task<decimal> GetClassifiersDatatableReportTotalAmount(string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null)
            => await _classifierRepository.GetClassifiersDatatableReportTotalAmount(search, startDate, endDate, type);
        
        public async Task<List<ClassifierTemplate>> GetClassifiersReportData(string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null, bool showAll = false)
            => await _classifierRepository.GetClassifiersReportData(search, startDate, endDate, type, showAll);

        public async Task<object> GetAreaOrCareerJson()
            => await _classifierRepository.GetAreaOrCareerJson();
        public async Task<Classifier> GetByCode(string code)
            => await _classifierRepository.GetByCode(code);

        public async Task Update(Classifier classifier)
        {
            await _classifierRepository.Update(classifier);
        }

        public async Task UpdateClassifierRelationsJob()
        {
            await _classifierRepository.UpdateClassifierRelationsJob();
        }

        public async Task<Classifier> Add(Classifier classifier)
            => await _classifierRepository.Add(classifier);

        public async Task Update()
            => await _classifierRepository.Update();

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptsByClassifierDatatable(DataTablesStructs.SentParameters sentParameters, Guid classifierId, string search)
            => await _classifierRepository.GetConceptsByClassifierDatatable(sentParameters, classifierId, search);

        public async Task<List<PaymentTemplate>> GetPaymentsByClassifierId(Guid classifierId, DateTime? startDate = null, DateTime? endDate = null, int? type = null)
            => await _classifierRepository.GetPaymentsByClassifierId(classifierId, startDate, endDate, type);
    }
}
