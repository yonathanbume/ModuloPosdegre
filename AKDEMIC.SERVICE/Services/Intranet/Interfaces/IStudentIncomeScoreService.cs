using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IStudentIncomeScoreService
    {
        Task<StudentIncomeScore> GetByStudent(Guid studentId);
        Task Update(StudentIncomeScore entity);
        Task Insert(StudentIncomeScore entity);
        Task<StudentIncomeScore> Get(Guid id);
    }
}
