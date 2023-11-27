using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringProblemFileRepository : IRepository<TutoringProblemFile>
    {
        Task<IEnumerable<TutoringProblemFile>> GetAllByTutoringProblemId(Guid tutoringProblemId);
    }
}
