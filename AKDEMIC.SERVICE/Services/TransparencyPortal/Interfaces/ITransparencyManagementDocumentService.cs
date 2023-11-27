using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyManagementDocumentService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters);
        Task Insert(TransparencyManagementDocument entity);
        Task Update(TransparencyManagementDocument entity);
        Task Delete(TransparencyManagementDocument entity);
        Task<TransparencyManagementDocument> Get(Guid id);
        Task<IEnumerable<TransparencyManagementDocument>> GetAll();
    }
}
