using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICompetencieRepository : IRepository<Competencie>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCompetenciesDatatable(DataTablesStructs.SentParameters parameters, string searchvalue);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task<Select2Structs.ResponseParameters> GetCompetenciesSelect2(Select2Structs.RequestParameters parameters, byte? type, string searchValue, Guid? curriculumId);
    }
}
