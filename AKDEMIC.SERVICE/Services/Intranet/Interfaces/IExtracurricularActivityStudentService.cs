using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtracurricularActivityStudentService
    {
        Task<ExtracurricularActivityStudent> Get(Guid id);
        Task<IEnumerable<ExtracurricularActivityStudent>> GetAll();
        Task<object> GetDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, string search);
        Task Insert(ExtracurricularActivityStudent extracurricularActivityStudent);
        Task Update(ExtracurricularActivityStudent extracurricularActivityStudent);
        Task DeleteById(Guid id);
        Task<List<ExtracurricularActivityStudent>> GetAllByStudent(Guid studentId);
    }
}
