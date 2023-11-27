using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IRoleService
    {
        Task<ApplicationRole> GetByName(string roleName);
        Task<IEnumerable<ApplicationRole>> GetAll();
        Task<ApplicationRole> Get(Guid id);
        Task<IEnumerable<ApplicationRole>> GetAllByName(IEnumerable<string> roleNames);
        Task<IEnumerable<ApplicationRole>> GetAllById(IEnumerable<string> roleIds);
        Task<IEnumerable<ApplicationRole>> GetProcedureRoles(Guid procedureId);
        Task<DataTablesStructs.ReturnedData<ApplicationRole>> GetProcedureRolesDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null, List<string> rolesFiltered = null);
        Task<Select2Structs.ResponseParameters> GetRolesSelect2(Select2Structs.RequestParameters requestParameters, string search);
        Task<ApplicationRole> Get(string id);
        Task<object> GetAllAsSelect2ClientSide();
        Task<object> GetAllAsSelect2ClientSide(IEnumerable<string> exceptionNames);
        Task<object> GetAllAsSelect2ByStudentAndEnterpriseClientSide();
        Task<ApplicationRole> GetRolByName(string name);
        Task<object> GetUnits();
        Task<object> GetUnitMark();
        Task<object> GetUnitsPurchase();
        Task<object> GetAdqui(int UserRequirementIndex);
        Task<object> GetMarketResearchProgramiming();
    }
}