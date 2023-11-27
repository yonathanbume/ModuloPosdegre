using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ExpenditureProvisionService : IExpenditureProvisionService
    {
        private readonly IExpenditureProvisionRepository _expenditureProvisionRepository;

        public ExpenditureProvisionService(IExpenditureProvisionRepository expenditureProvisionRepository)
        {
            _expenditureProvisionRepository = expenditureProvisionRepository;
        }

        public async Task InsertExpenditureProvision(ExpenditureProvision expenditureProvision) =>
            await _expenditureProvisionRepository.Insert(expenditureProvision);

        public async Task UpdateExpenditureProvision(ExpenditureProvision expenditureProvision) =>
            await _expenditureProvisionRepository.Update(expenditureProvision);

        public async Task DeleteExpenditureProvision(ExpenditureProvision expenditureProvision) =>
            await _expenditureProvisionRepository.Delete(expenditureProvision);

        public async Task<ExpenditureProvision> GetExpenditureProvisionById(Guid id) =>
            await _expenditureProvisionRepository.Get(id);

        public async Task<IEnumerable<ExpenditureProvision>> GetAllExpenditureProvisions() =>
            await _expenditureProvisionRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatable(DataTablesStructs.SentParameters sentParameters, string search)
            => await _expenditureProvisionRepository.GetExpenditureProvisionDatatable(sentParameters, search);

        public async Task<object> GetProvisionByDependencyAndStatus(Guid id, int status)
            => await _expenditureProvisionRepository.GetProvisionByDependencyAndStatus(id, status);

        public async Task<List<ExpenditureProvision>> GetExpenditureProvisionList(Guid id, int status)
            => await _expenditureProvisionRepository.GetExpenditureProvisionList(id, status);

        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatableDependency(string userId, DataTablesStructs.SentParameters sentParameters, string search)
            => await _expenditureProvisionRepository.GetExpenditureProvisionDatatableDependency(userId, sentParameters, search);
        public IQueryable<ExpenditureProvision> ProvisionsQry(DateTime date, int status)
            =>  _expenditureProvisionRepository.ProvisionsQry(date, status);
        public async Task<List<ExpenditureProvision>> GetExpenditureProvisionStatusList(int status)
            => await _expenditureProvisionRepository.GetExpenditureProvisionStatusList(status);
        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatableProvision(DataTablesStructs.SentParameters sentParameters, string search)
            => await _expenditureProvisionRepository.GetExpenditureProvisionDatatableProvision(sentParameters, search);
    }
}
