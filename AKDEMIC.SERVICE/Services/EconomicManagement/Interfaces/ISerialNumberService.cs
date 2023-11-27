using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ISerialNumberService
    {
        Task<SerialNumber> GetSerialNumber(Guid id);
        Task Delete(SerialNumber serialNumber);
        Task<IEnumerable<SerialNumber>> GetUserSerialNumbers(string id);
        Task<bool> ValidateSerialNumber(string userId, int number);
        Task InsertSerialNumber(SerialNumber serialNumber);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string term);
        Task<bool> isRepeatedAsync(string series, byte documentType);
        Task<bool> haveOneAlreadyAsync(byte documentType, string userId, Guid? id = null);
        Task<List<SerialNumber>> GetSerialNumberIncludeUser(Guid id);
        Task<bool> isRepeatedAsyncById(Guid id,string series, byte documentType);
        Task<SerialNumber> GetUserSerialNumbersByUserIdAndDocumentType(string userId, int documentType);
        Task Update(SerialNumber serialNumber);
    }
}
