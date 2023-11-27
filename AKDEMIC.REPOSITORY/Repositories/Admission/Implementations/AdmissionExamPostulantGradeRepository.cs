using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class AdmissionExamPostulantGradeRepository : Repository<AdmissionExamPostulantGrade>, IAdmissionExamPostulantGradeRepository
    {
        public AdmissionExamPostulantGradeRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByIdAndPostulant(Guid admissionExamId, Guid postulantId)
        {
            return await _context.AdmissionExamPostulantGrades.AnyAsync(x => x.AdmissionExamId==admissionExamId&& x.PostulantId == postulantId);
        }
    }
}
