using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryCourseService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<PreuniversitaryCourse> Get(Guid id);
        Task Insert(PreuniversitaryCourse entity);
        Task Update(PreuniversitaryCourse entity);
        Task Delete(PreuniversitaryCourse entity);
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task<List<PreuniversitaryCourse>> GetAllByCareerId(Guid careerId);
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesToScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchvalue = null);
    }
}
