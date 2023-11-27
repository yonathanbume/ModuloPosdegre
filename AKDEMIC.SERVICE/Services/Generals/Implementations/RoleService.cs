using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ApplicationRole> Get(Guid id)
            => await _roleRepository.Get(id);

        public async Task<IEnumerable<ApplicationRole>> GetAll()
            => await _roleRepository.GetAll();

        public async Task<IEnumerable<ApplicationRole>> GetAllByName(IEnumerable<string> roleNames)
            => await _roleRepository.GetAllByName(roleNames);

        public async Task<ApplicationRole> GetByName(string roleName)
            => await _roleRepository.GetByName(roleName);

        public async Task<IEnumerable<ApplicationRole>> GetProcedureRoles(Guid procedureId)
            => await _roleRepository.GetProcedureRoles(procedureId);

        public async Task<DataTablesStructs.ReturnedData<ApplicationRole>> GetProcedureRolesDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null, List<string> rolesFiltered = null)
            => await _roleRepository.GetProcedureRolesDatatable(sentParameters, procedureId, searchValue, rolesFiltered );

        public async Task<Select2Structs.ResponseParameters> GetRolesSelect2(Select2Structs.RequestParameters requestParameters, string search)
            => await _roleRepository.GetRolesSelect2(requestParameters, search);

        public async Task<ApplicationRole> Get(string id)
            => await _roleRepository.Get(id);

        public Task<object> GetAllAsSelect2ClientSide()
            => _roleRepository.GetAllAsSelect2ClientSide();

        public async Task<object> GetAllAsSelect2ByStudentAndEnterpriseClientSide()
        {
            return await _roleRepository.GetAllAsSelect2ByStudentAndEnterpriseClientSide();
        }

        public async Task<object> GetAllAsSelect2ClientSide(IEnumerable<string> exceptionNames)
            => await _roleRepository.GetAllAsSelect2ClientSide(exceptionNames);

        public async Task<IEnumerable<ApplicationRole>> GetAllById(IEnumerable<string> roleIds)
            => await _roleRepository.GetAllById(roleIds);


        public async Task<ApplicationRole> GetRolByName(string name)
            => await _roleRepository.GetRolByName(name);
        public async Task<object> GetUnits()
            => await _roleRepository.GetUnits();
        public async Task<object> GetUnitMark()
            => await _roleRepository.GetUnitMark();
        public async Task<object> GetUnitsPurchase()
            => await _roleRepository.GetUnitsPurchase();
        public async Task<object> GetAdqui(int UserRequirementIndex)
            => await _roleRepository.GetAdqui(UserRequirementIndex);
        public async Task<object> GetMarketResearchProgramiming()
            => await _roleRepository.GetMarketResearchProgramiming();
    }
}
