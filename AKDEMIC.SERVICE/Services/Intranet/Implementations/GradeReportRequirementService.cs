using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class GradeReportRequirementService : IGradeReportRequirementService
    {
        private readonly IGradeReportRequirementRepository _gradeReportRequirementRepository;

        public GradeReportRequirementService(IGradeReportRequirementRepository gradeReportRequirementRepository)
        {
            _gradeReportRequirementRepository = gradeReportRequirementRepository;
        }

        public async Task Delete(GradeReportRequirement entity)
        {
            await _gradeReportRequirementRepository.Delete(entity);
        }

        public async Task<GradeReportRequirement> Get(Guid id)
        {
            return await _gradeReportRequirementRepository.Get(id);
        }

        public async Task<List<GradeReportRequirement>> GetRequirementsByGradeReport(Guid gradeReportId)
        {
            return await _gradeReportRequirementRepository.GetRequirementsByGradeReport(gradeReportId);
        }

        public async Task Insert(GradeReportRequirement entity)
        {
            await _gradeReportRequirementRepository.Insert(entity);
        }

        public async Task InsertRange(IEnumerable<GradeReportRequirement> gradeReportRequirements)
        {
            await _gradeReportRequirementRepository.InsertRange(gradeReportRequirements);
        }

        public async Task Update(GradeReportRequirement entity)
        {
            await _gradeReportRequirementRepository.Update(entity);
        }
    }
}
