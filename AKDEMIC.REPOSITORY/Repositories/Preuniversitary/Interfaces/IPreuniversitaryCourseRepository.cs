using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryCourseRepository : IRepository<PreuniversitaryCourse>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task<List<PreuniversitaryCourse>> GetAllByCareerId(Guid careerId);
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesToScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchvalue = null);
    }
}
