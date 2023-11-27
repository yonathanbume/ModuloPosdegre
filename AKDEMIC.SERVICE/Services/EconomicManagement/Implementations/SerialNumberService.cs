using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class SerialNumberService : ISerialNumberService
    {
        private readonly ISerialNumberRepository _serialNumberRepository;

        public SerialNumberService(ISerialNumberRepository serialNumberRepository)
        {
            _serialNumberRepository = serialNumberRepository;
        }

        public async Task<SerialNumber> GetSerialNumber(Guid id) => await _serialNumberRepository.Get(id);
        public async Task Delete(SerialNumber serialNumber)
             => await _serialNumberRepository.Delete(serialNumber);
        public async Task<IEnumerable<SerialNumber>> GetUserSerialNumbers(string id) =>
            await _serialNumberRepository.GetUserSerialNumbers(id);

        public async Task<bool> ValidateSerialNumber(string userId, int number) =>
            await _serialNumberRepository.ValidateSerialNumber(userId, number);

        public async Task InsertSerialNumber(SerialNumber serialNumber)
        {
            await _serialNumberRepository.Insert(serialNumber);
            if (serialNumber.IsBankSerialNumber)
                await _serialNumberRepository.RemovePreviousBankSerialNumbers(serialNumber.Id);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string term)
            => await _serialNumberRepository.GetDatatable(sentParameters, term);
        public async Task<bool> isRepeatedAsync(string series, byte documentType)
            => await _serialNumberRepository.isRepeatedAsync(series, documentType);
        public async Task<bool> haveOneAlreadyAsync(byte documentType, string userId, Guid? id = null)
            => await _serialNumberRepository.haveOneAlreadyAsync(documentType, userId, id);
        public async Task<List<SerialNumber>> GetSerialNumberIncludeUser(Guid id)
            => await _serialNumberRepository.GetSerialNumberIncludeUser(id);
        public async Task<bool> isRepeatedAsyncById(Guid id, string series, byte documentType)
            => await _serialNumberRepository.isRepeatedAsyncById(id, series, documentType);
        public async Task<SerialNumber> GetUserSerialNumbersByUserIdAndDocumentType(string userId, int documentType)
            => await _serialNumberRepository.GetUserSerialNumbersByUserIdAndDocumentType(userId, documentType);

        public async  Task Update(SerialNumber serialNumber)
        {
            await _serialNumberRepository.Update(serialNumber);
            if (serialNumber.IsBankSerialNumber)
                await _serialNumberRepository.RemovePreviousBankSerialNumbers(serialNumber.Id);
        }
    }
}
