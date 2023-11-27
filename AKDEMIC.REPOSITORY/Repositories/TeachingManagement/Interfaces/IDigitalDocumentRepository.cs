using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IDigitalDocumentRepository : IRepository<DigitalDocument>
    {
        Task<object> GetAllAsModelA();
        Task<object> GetAsModelB(Guid? id = null, string userToVerify = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDigitalDocumentToTeacherDatatable(DataTablesStructs.SentParameters parameters, string teacherId);
    }
}