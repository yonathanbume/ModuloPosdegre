using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IScholarshipFileService
    {
        Task<IEnumerable<ScholarshipFile>> GetAllByScholarshipId(Guid scholarshipId);
        Task DeleteRange(IEnumerable<ScholarshipFile> files);
        Task<ScholarshipFile> Get(Guid id);
        Task Delete(ScholarshipFile file);
        Task Insert(ScholarshipFile file);

    }
}
