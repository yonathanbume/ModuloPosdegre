using System;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IFacultyCurriculumAreaRepository : IRepository<FacultyCurriculumArea>
    {
        Task DeleteByCurriculumAreaId(Guid curriculumAreaId);
        Task<object> GetByCurriculumAreaId(Guid id);
    }
}
