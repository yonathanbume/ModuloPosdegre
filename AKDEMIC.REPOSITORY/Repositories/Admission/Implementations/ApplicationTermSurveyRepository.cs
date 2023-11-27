using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class ApplicationTermSurveyRepository : Repository<ApplicationTermSurvey> , IApplicationTermSurveyRepository
    {
        public ApplicationTermSurveyRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByApplicationTermId(Guid applicationTermId)
            => await _context.ApplicationTermSurveys.AnyAsync(x => x.ApplicationTermId == applicationTermId);

        public async Task<ApplicationTermSurvey> GetByApplicationTermId(Guid applicationTermId)
            => await _context.ApplicationTermSurveys.Where(x => x.ApplicationTermId == applicationTermId).FirstOrDefaultAsync();

        public async Task<bool> AnySurveyUser(Guid surveyId)
            => await _context.ApplicationTermSurveyUsers.AnyAsync(x => x.ApplicationTermSurveyId == surveyId);
    }
}
