using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryExamClassroomService
    {
        Task<bool> AnyClassroomByExam(Guid classroomId, Guid preuniversitaryExamId, Guid? ignoredId = null);
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryExamId);
        Task Insert(PreuniversitaryExamClassroom entity);
        Task Update(PreuniversitaryExamClassroom entity);
        Task Delete(PreuniversitaryExamClassroom entity);
        Task<PreuniversitaryExamClassroom> Get(Guid id);
    }
}
