using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IAdmissionExamPostulantGradeService
    {
        Task InsertRange(List<AdmissionExamPostulantGrade> list);
        Task<bool> AnyByIdAndPostulant(Guid admissionExamId, Guid postulantId);
    }
}
