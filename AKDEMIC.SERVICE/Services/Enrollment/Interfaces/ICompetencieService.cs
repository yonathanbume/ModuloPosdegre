using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICompetencieService
    {
        Task Insert(Competencie entity);
        Task Delete(Competencie entity);
        Task<Competencie> Get(Guid id);
        Task Update(Competencie entity);
        Task<DataTablesStructs.ReturnedData<object>> GetCompetenciesDatatable(DataTablesStructs.SentParameters parameters, string searchvalue);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task<Select2Structs.ResponseParameters> GetCompetenciesSelect2(Select2Structs.RequestParameters parameters, byte? type, string searchValue, Guid? curriculumId );
    }
}
