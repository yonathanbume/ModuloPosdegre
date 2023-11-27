using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ProcedureAdmissionTypeService : IProcedureAdmissionTypeService
    {
        private readonly IProcedureAdmissionTypeRepository _procedureAdmissionTypeRepository;

        public ProcedureAdmissionTypeService(IProcedureAdmissionTypeRepository procedureAdmissionTypeRepository)
        {
            _procedureAdmissionTypeRepository = procedureAdmissionTypeRepository;
        }

        public async Task Delete(ProcedureAdmissionType entity)
            => await _procedureAdmissionTypeRepository.Delete(entity);

        public async Task<ProcedureAdmissionType> Get(Guid procedureId, Guid admissionTypeId)
            => await _procedureAdmissionTypeRepository.Get(procedureId, admissionTypeId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetProcedureAdmissionTypeToAssignDatatable(DataTablesStructs.SentParameters parameters, Guid procedureId, string search)
            => await _procedureAdmissionTypeRepository.GetProcedureAdmissionTypeToAssignDatatable(parameters, procedureId, search);

        public async Task Insert(ProcedureAdmissionType entity)
            => await _procedureAdmissionTypeRepository.Insert(entity);
    }
}
