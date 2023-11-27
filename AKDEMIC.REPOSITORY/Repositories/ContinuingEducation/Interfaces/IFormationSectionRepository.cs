using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Templates.FormationSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces
{
    public interface IFormationSectionRepository:IRepository<ENTITIES.Models.ContinuingEducation.Section>
    {
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllFormationSectionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? courseId = null, string searchValue = null);

        Task<FormationSectionTemplate> GetSectionTemplateData(Guid id);
        Task<List<FormationSectionTemplate>> GetAllSectionTemplateData(int skip = 0, int take = 0, Guid? courseTypeId = null, string searchValue = null);
    }
}
