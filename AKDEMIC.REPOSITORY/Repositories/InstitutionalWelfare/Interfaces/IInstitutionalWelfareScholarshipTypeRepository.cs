using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareScholarshipTypeRepository : IRepository<InstitutionalWelfareScholarshipType>
    {
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task<bool> AnyScholarship(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetScholarshipTypesDatatable(DataTablesStructs.SentParameters parameters, string searchValue);
        Task<object> GetScholarshipTypeSelect2();
    }
}
