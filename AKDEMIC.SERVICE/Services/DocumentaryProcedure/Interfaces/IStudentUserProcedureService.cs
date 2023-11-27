using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IStudentUserProcedureService
    {
        Task Add(StudentUserProcedure entity);
        Task Insert(StudentUserProcedure entity);
        Task<StudentUserProcedure> Get(Guid id);
        Task Delete(StudentUserProcedure entity);
    }
}
