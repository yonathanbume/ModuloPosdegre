using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class IncubationCallService: IIncubationCallService
    {
        private readonly IIncubationCallRepository _incubationCallRepository;

        public IncubationCallService(IIncubationCallRepository incubationCallRepository)
        {
            _incubationCallRepository = incubationCallRepository;
        }

        public async Task<IncubationCall> Get(Guid id)
        {
            return await _incubationCallRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAceptedDatatable(DataTablesStructs.SentParameters sentParameters, string rolId, string searchValue = null)
        {
            return await _incubationCallRepository.GetIncubationCallAceptedDatatable(sentParameters,rolId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAdmin2Datatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _incubationCallRepository.GetIncubationCallAdmin2Datatable(sentParameters,searchValue);
        }

        public async Task Delete(IncubationCall incubationCall)
        {
            await _incubationCallRepository.Delete(incubationCall);
        }

        public async Task<IncubationCall> GetByUser(string userId)
        {
            return await _incubationCallRepository.GetByUser(userId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAdminDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _incubationCallRepository.GetIncubationCallAdminDatatable(sentParameters,searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallEnterpriseDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _incubationCallRepository.GetIncubationCallEnterpriseDatatable(sentParameters,searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallNotAdminDatatable(DataTablesStructs.SentParameters sentParameters, string rolId, string searchValue = null)
        {
            return await _incubationCallRepository.GetIncubationCallNotAdminDatatable(sentParameters,rolId,searchValue);
        }

        public async Task<IncubationCall> IncubationCallAdmin()
        {
            return await _incubationCallRepository.IncubationCallAdmin();
        }

        public async Task Insert(IncubationCall incubationCall)
        {
            await _incubationCallRepository.Insert(incubationCall);
        }

        public async Task InsertRange(IEnumerable<IncubationCall> incubationCalls)
        {
            await _incubationCallRepository.InsertRange(incubationCalls);
        }

        public async Task Update(IncubationCall incubationCall)
        {
            await _incubationCallRepository.Update(incubationCall);
        }
    }
}
