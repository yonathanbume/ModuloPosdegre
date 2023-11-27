using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringMessageService
    {
        Task<IEnumerable<TutoringMessage>> GetAll();
        Task<IEnumerable<TutoringMessage>> GetAllByTutor(string tutorId);
        Task<TutoringMessage> Get(Guid id);
        Task Insert(TutoringMessage tutoringMessage);
        Task Update(TutoringMessage tutoringMessage);
        Task DeleteById(Guid id);
        Task DeleteRange(IEnumerable<TutoringMessage> tutoringMessages);
        Task<IEnumerable<TutoringMessage>> GetAllByTutoringStudentIdAndTutorId(Guid? tutoringStudentId = null, string tutorId = null);
    }
}
