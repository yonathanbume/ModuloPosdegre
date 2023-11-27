using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryExamTeacherRepository : IRepository<PreuniversitaryExamTeacher>
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryExamId);
        Task<bool> AnyUserByExam(Guid preuniversitaryExamId, string userId);
        Task AssignTeachersRandomly(Guid preuniversitaryExamId);
    }
}
