using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IStudentIncomeScoreRepository : IRepository<StudentIncomeScore>
    {
        Task<StudentIncomeScore> GetByStudent(Guid studentId);
    }
}
