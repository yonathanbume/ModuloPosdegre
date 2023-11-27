using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IStudentComplementaryStudyRepository:IRepository<StudentComplementaryStudy>
    {
        Task<List<StudentComplementaryStudy>> GetByStudentId(Guid studentId, int? type = null);
    }
}
