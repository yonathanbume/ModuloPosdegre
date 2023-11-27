using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface ISubstituteExamCorrectionRepository : IRepository<SubstituteExamCorrection>
    {
        Task<IEnumerable<SubstituteExamCorrection>> GetAll(string teacherId = null, Guid? termId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null);
        Task<SubstituteExamCorrection> GetByTeacherStudent(string teacherId, Guid studentId);
    }
}
