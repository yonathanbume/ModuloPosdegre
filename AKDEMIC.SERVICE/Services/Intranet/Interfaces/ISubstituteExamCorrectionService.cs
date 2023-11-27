using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ISubstituteExamCorrectionService
    {
        Task<IEnumerable<SubstituteExamCorrection>> GetAll(string teacherId = null, Guid? termId = null);
        Task<SubstituteExamCorrection> Get(Guid id);
        Task Insert(SubstituteExamCorrection substituteExamCorrection);
        Task Update(SubstituteExamCorrection substituteExamCorrection);
        Task DeleteById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null);
        Task<SubstituteExamCorrection> GetByTeacherStudent(string techearId, Guid studentId);
    }
}
