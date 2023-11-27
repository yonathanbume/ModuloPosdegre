using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class FacultyCurriculumAreaService : IFacultyCurriculumAreaService
    {
        private readonly IFacultyCurriculumAreaRepository _facultyCurriculumAreaRepository;

        public FacultyCurriculumAreaService(IFacultyCurriculumAreaRepository facultyCurriculumAreaRepository)
        {
            _facultyCurriculumAreaRepository = facultyCurriculumAreaRepository;
        }

        public Task InsertRange(IEnumerable<FacultyCurriculumArea> facultyCurriculumAreas) =>
            _facultyCurriculumAreaRepository.InsertRange(facultyCurriculumAreas);

        public Task DeleteByCurriculumAreaId(Guid curriculumAreaId) =>
            _facultyCurriculumAreaRepository.DeleteByCurriculumAreaId(curriculumAreaId);

        public async Task<object> GetByCurriculumAreaId(Guid id)
        {
            return await _facultyCurriculumAreaRepository.GetByCurriculumAreaId(id);
        }
    }
}
