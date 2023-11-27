using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ProcedureRoleService : IProcedureRoleService
    {
        private readonly IProcedureRoleRepository _procedureRoleRepository;

        public ProcedureRoleService(IProcedureRoleRepository procedureRoleRepository)
        {
            _procedureRoleRepository = procedureRoleRepository;
        }

        public async Task<bool> ToggleProcedureRole(Guid procedureId, string roleId)
        {
            return await _procedureRoleRepository.ToggleProcedureRole(procedureId, roleId);
        }

        public async Task<ProcedureRole> Get(Guid id)
        {
            return await _procedureRoleRepository.Get(id);
        }

        public async Task<ProcedureRole> GetProcedureRole(Guid id)
        {
            return await _procedureRoleRepository.GetProcedureRole(id);
        }

        public async Task Delete(ProcedureRole procedureRole)
        {
            await _procedureRoleRepository.Delete(procedureRole);
        }

        public async Task Insert(ProcedureRole procedureRole)
        {
            await _procedureRoleRepository.Insert(procedureRole);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProcedureRolesAssignedDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureId)
            => await _procedureRoleRepository.GetProcedureRolesAssignedDatatable(sentParameters, procedureId);

        public async Task DeleteRange(IEnumerable<ProcedureRole> entities)
            => await _procedureRoleRepository.DeleteRange(entities);

        public async Task<List<ProcedureRole>> GetProcedureRolesByProcedureId(Guid procedureId)
            => await _procedureRoleRepository.GetProcedureRolesByProcedureId(procedureId);

        public async Task<bool> ValidRoleToProcedure(Guid procedureId, string roleId)
            => await _procedureRoleRepository.ValidRoleToProcedure(procedureId, roleId);
    }
}
