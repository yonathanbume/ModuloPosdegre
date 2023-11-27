using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class ProcedureRoleRepository : Repository<ProcedureRole>, IProcedureRoleRepository
    {
        public ProcedureRoleRepository(AkdemicContext context) : base(context) { }

        public async Task<ProcedureRole> GetProcedureRole(Guid id)
        {
            return await _context.ProcedureRoles
                .SelectProcedureRole()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ValidRoleToProcedure(Guid procedureId, string roleId)
        {
            var role = await _context.Roles.Where(x => x.Id == roleId).FirstOrDefaultAsync();
            var procedure = await _context.Procedures.Where(x => x.Id == procedureId).FirstOrDefaultAsync();

            if(
                procedure.Type == ConstantHelpers.PROCEDURES.TYPE.CAREER || procedure.Type == ConstantHelpers.PROCEDURES.TYPE.FACULTY || 
                procedure.Score == ConstantHelpers.PROCEDURES.SCORE.AUTOMATIC || procedure.Score == ConstantHelpers.PROCEDURES.SCORE.SEMIAUTOMATIC
                )
            {
                if (role.Name != ConstantHelpers.ROLES.STUDENTS)
                    return false;
            }

            return true;
        }

        public async Task<bool> ToggleProcedureRole(Guid procedureId, string roleId)
        {
            var procedureRole = await _context.ProcedureRoles
                .Where(x => x.ProcedureId == procedureId && x.RoleId == roleId)
                .FirstOrDefaultAsync();

            var role = await _context.Roles.Where(x => x.Id == roleId).FirstOrDefaultAsync();

            var procedureRoleAny = procedureRole != null;

            if (procedureRoleAny)
            {
                await Delete(procedureRole);

                if(role.Name == ConstantHelpers.ROLES.STUDENTS)
                {
                    var procedureAdmissionTypes = await _context.ProcedureAdmissionTypes.Where(x=>x.ProcedureId == procedureId).ToListAsync();
                    _context.ProcedureAdmissionTypes.RemoveRange(procedureAdmissionTypes);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                procedureRole = new ProcedureRole
                {
                    ProcedureId = procedureId,
                    RoleId = roleId
                };

                await Insert(procedureRole);

                if (role.Name == ConstantHelpers.ROLES.STUDENTS)
                {
                    var admissionTypes = await _context.AdmissionTypes.ToListAsync();
                    var procedureAdmissionTypes = admissionTypes.Select(x => new ProcedureAdmissionType
                    {
                        ProcedureId = procedureId,
                        AdmissionTypeId = x.Id
                    }).ToList();
                    await _context.ProcedureAdmissionTypes.AddRangeAsync(procedureAdmissionTypes);
                    await _context.SaveChangesAsync();
                }
            }

            return !procedureRoleAny;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProcedureRolesAssignedDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureId)
        {
            var query = _context.ProcedureRoles.Where(x => x.ProcedureId == procedureId).AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name
                })
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<ProcedureRole>> GetProcedureRolesByProcedureId(Guid procedureId)
            => await _context.ProcedureRoles.Where(x => x.ProcedureId == procedureId).ToListAsync();

    }
}
