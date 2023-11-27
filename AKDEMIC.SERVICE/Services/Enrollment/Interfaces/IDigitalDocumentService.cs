using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IDigitalDocumentService
    {
        Task<DigitalDocument> GetAsync(Guid id);
        Task DeleteAsync(DigitalDocument digitalDocument);
        Task UpdateAsync(DigitalDocument digitalDocument);
        Task InsertAsync(DigitalDocument digitalDocument);
        Task<object> GetAllAsModelA();
        Task<object> GetAsModelB(Guid? id = null, string userToVerify = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDigitalDocumentToTeacherDatatable(DataTablesStructs.SentParameters parameters, string teacherId);
    }
}