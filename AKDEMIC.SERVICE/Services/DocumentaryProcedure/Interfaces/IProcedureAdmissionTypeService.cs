using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IProcedureAdmissionTypeService
    {
        Task<ProcedureAdmissionType> Get(Guid procedureId, Guid admissionTypeId);
        Task<DataTablesStructs.ReturnedData<object>> GetProcedureAdmissionTypeToAssignDatatable(DataTablesStructs.SentParameters parameters, Guid procedureId, string search);
        Task Insert(ProcedureAdmissionType entity);
        Task Delete(ProcedureAdmissionType entity);
    }
}
