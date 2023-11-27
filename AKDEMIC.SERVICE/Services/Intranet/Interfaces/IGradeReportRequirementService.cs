using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IGradeReportRequirementService
    {
        Task Insert(GradeReportRequirement entity);
        Task<GradeReportRequirement> Get(Guid id);
        Task Update(GradeReportRequirement entity);
        Task Delete(GradeReportRequirement entity);
        Task InsertRange(IEnumerable<GradeReportRequirement> gradeReportRequirements);
        Task<List<GradeReportRequirement>> GetRequirementsByGradeReport(Guid gradeReportId);
    }
}
