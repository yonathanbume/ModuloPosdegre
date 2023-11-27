using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionExamPostulantGradeRepository : IRepository<AdmissionExamPostulantGrade>
    {
        Task<bool> AnyByIdAndPostulant(Guid admissionExamId, Guid postulantId);
    }
}
