using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringMessageRepository : IRepository<TutoringMessage>
    {
        Task<IEnumerable<TutoringMessage>> GetAllByTutoringStudentIdAndTutorId(Guid? tutoringStudentId = null, string tutorId = null);
        Task<IEnumerable<TutoringMessage>> GetAllByTutor(string tutorId);
    }
}
