using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtracurricularActivityStudentRepository : IRepository<ExtracurricularActivityStudent>
    {
        Task<List<ExtracurricularActivityStudent>> GetAllByStudent(Guid studentId);
        Task<object> GetDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, string search);
    }
}
