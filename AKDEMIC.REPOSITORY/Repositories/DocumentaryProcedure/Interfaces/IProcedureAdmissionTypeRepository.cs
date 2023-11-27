using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IProcedureAdmissionTypeRepository : IRepository<ProcedureAdmissionType>
    {
        Task<ProcedureAdmissionType> Get(Guid procedureId, Guid admissionTypeId);
        Task<DataTablesStructs.ReturnedData<object>> GetProcedureAdmissionTypeToAssignDatatable(DataTablesStructs.SentParameters parameters, Guid procedureId, string search);
    }
}
