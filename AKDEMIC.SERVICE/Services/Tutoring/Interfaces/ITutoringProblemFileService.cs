using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringProblemFileService
    {
        Task<IEnumerable<TutoringProblemFile>> GetAllByTutoringProblemId(Guid tutoringProblemId);
        Task<TutoringProblemFile> Get(Guid tutoringProblemFileId);
        Task Insert(TutoringProblemFile tutoringProblemFile);
        Task Update(TutoringProblemFile tutoringProblemFile);
        Task DeleteById(Guid tutoringProblemFileId);
        Task Delete(TutoringProblemFile tutoringProblemFile);
    }
}
