using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IScholarshipStudentService
    {
        Task<ScholarshipStudent> Get(Guid id);
        Task<bool> IsApplicant(Guid studentId, Guid scholarshipId);
        Task<ScholarshipStudent> GetScholarshipStudent(Guid studentId, Guid scholarshipId);
        Task Insert(ScholarshipStudent entity);
        Task Update(ScholarshipStudent entity);
        Task<List<ScholarshipStudentRequirement>> GetScholarshipStudentRequirements(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetScholarshipStudentDatatable(DataTablesStructs.SentParameters parameters, Guid scholarshipId, string searchValue, byte? status = null);
    }
}
