using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CurriculumArea;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICurriculumAreaRepository : IRepository<CurriculumArea>
    {
        Task<CurriculumArea> GetWithFacultiesId(Guid id);
        Task<object> GetCurriculumAreasJson(string q);
        Task<List<CurriculumAreaTemplate>> GetCurriculumsForTemary();
    }
}
