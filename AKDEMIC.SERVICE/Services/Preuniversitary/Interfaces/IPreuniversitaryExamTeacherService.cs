using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryExamTeacherService
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryExamId);
        Task Insert(PreuniversitaryExamTeacher entity);
        Task Update(PreuniversitaryExamTeacher entity);
        Task<PreuniversitaryExamTeacher> Get(Guid id);
        Task Delete(PreuniversitaryExamTeacher entity);
        Task<bool> AnyUserByExam(Guid preuniversitaryExamId, string userId);
        Task InsertRange(List<PreuniversitaryExamTeacher> entities);
        Task AssignTeachersRandomly(Guid preuniversitaryExamId);
    }
}
