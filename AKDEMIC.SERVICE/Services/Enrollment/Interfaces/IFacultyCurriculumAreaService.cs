using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IFacultyCurriculumAreaService
    {
        Task InsertRange(IEnumerable<FacultyCurriculumArea> facultyCurriculumAreas);
        Task DeleteByCurriculumAreaId(Guid curriculumAreaId);
        Task<object> GetByCurriculumAreaId(Guid id);
    }
}