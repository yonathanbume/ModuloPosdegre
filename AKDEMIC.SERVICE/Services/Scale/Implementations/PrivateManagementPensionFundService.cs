using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Management.Automation.Language;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class PrivateManagementPensionFundService: IPrivateManagementPensionFundService
    {
        private readonly IPrivateManagementPensionFundRepository _privateManagementPensionFundRepository;

        public PrivateManagementPensionFundService(IPrivateManagementPensionFundRepository privateManagementPensionFundRepository)
        {
            _privateManagementPensionFundRepository = privateManagementPensionFundRepository;
        }

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _privateManagementPensionFundRepository.AnyByName(name,id);
        public async Task Delete(PrivateManagementPensionFund privateManagementPensionFund)
            => await _privateManagementPensionFundRepository.Delete(privateManagementPensionFund);

        public async Task<PrivateManagementPensionFund> Get(Guid id)
            => await _privateManagementPensionFundRepository.Get(id);

        public async Task<IEnumerable<PrivateManagementPensionFund>> GetAll()
            => await _privateManagementPensionFundRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetPrivateManagementPensionDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _privateManagementPensionFundRepository.GetPrivateManagementPensionDatatable(sentParameters,searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPayrollPrivateManagementPensionDatatable(DataTablesStructs.SentParameters sentParameters, Guid? conceptTypeId = null, string searchValue = null)
            => await _privateManagementPensionFundRepository.GetPayrollPrivateManagementPensionDatatable(sentParameters, conceptTypeId, searchValue);

        public async Task Insert(PrivateManagementPensionFund privateManagementPensionFund)
            => await _privateManagementPensionFundRepository.Insert(privateManagementPensionFund);

        public async Task Update(PrivateManagementPensionFund privateManagementPensionFund)
            => await _privateManagementPensionFundRepository.Update(privateManagementPensionFund);
    }
}
