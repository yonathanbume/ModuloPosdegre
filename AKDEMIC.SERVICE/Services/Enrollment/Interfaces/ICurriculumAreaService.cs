using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CurriculumArea;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICurriculumAreaService
    {
        Task<IEnumerable<CurriculumArea>> GetAll();
        Task<CurriculumArea> Get(Guid id);
        Task<CurriculumArea> GetWithFacultiesId(Guid id);
        Task Insert(CurriculumArea curriculumArea);
        Task Update(CurriculumArea curriculumArea);
        Task DeleteById(Guid id);
        Task<object> GetCurriculumAreasJson(string q);
        Task<List<CurriculumAreaTemplate>> GetCurriculumsForTemary();
    }
}
