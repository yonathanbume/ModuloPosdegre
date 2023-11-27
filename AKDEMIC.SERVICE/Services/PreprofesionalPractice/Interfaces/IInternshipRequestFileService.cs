using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PreprofesionalPractice.Interfaces
{
    public interface IInternshipRequestFileService
    {
        Task Insert(InternshipRequestFile entity);
        Task Delete(InternshipRequestFile entity);
        Task<InternshipRequestFile> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid internshipRequestId, byte type);
    }
}
