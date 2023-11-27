using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionExamPostulantGradeService : IAdmissionExamPostulantGradeService
    {
        private readonly IAdmissionExamPostulantGradeRepository _admissionExamPostulantGradeRepository;
        public AdmissionExamPostulantGradeService(IAdmissionExamPostulantGradeRepository admissionPostulantGradeRepository)
        {
            _admissionExamPostulantGradeRepository=admissionPostulantGradeRepository;
        }

        public async  Task<bool> AnyByIdAndPostulant(Guid admissionExamId, Guid postulantId)
        {
           return await _admissionExamPostulantGradeRepository.AnyByIdAndPostulant(admissionExamId, postulantId);
        }

        public async Task InsertRange(List<AdmissionExamPostulantGrade> list)
        {
            await _admissionExamPostulantGradeRepository.InsertRange(list);
        }
    }
}
