using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareScholarshipTypeService
    {
        Task Insert(InstitutionalWelfareScholarshipType entity);
        Task Update(InstitutionalWelfareScholarshipType entity);
        Task<InstitutionalWelfareScholarshipType> Get(Guid id);
        Task Delete(InstitutionalWelfareScholarshipType entity);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task<bool> AnyScholarship(Guid id);
        Task<object> GetScholarshipTypeSelect2();
        Task<DataTablesStructs.ReturnedData<object>> GetScholarshipTypesDatatable(DataTablesStructs.SentParameters parameters, string searchValue);
    }
}
