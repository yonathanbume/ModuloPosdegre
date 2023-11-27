using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface IScholarshipStudentRepository : IRepository<ScholarshipStudent>
    {
        Task<bool> IsApplicant(Guid studentId, Guid scholarshipId);
        Task<ScholarshipStudent> GetScholarshipStudent(Guid studentId, Guid scholarshipId);
        Task<List<ScholarshipStudentRequirement>> GetScholarshipStudentRequirements(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetScholarshipStudentDatatable(DataTablesStructs.SentParameters parameters, Guid scholarshipId, string searchValue, byte? status = null);
    }
}
