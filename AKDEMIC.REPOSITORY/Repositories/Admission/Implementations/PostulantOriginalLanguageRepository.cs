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
    public class PostulantOriginalLanguageRepository : Repository<PostulantOriginalLanguage>, IPostulantOriginalLanguageRepository
    {
        public PostulantOriginalLanguageRepository(AkdemicContext context) : base(context) { }

        public async Task<List<PostulantOriginalLanguage>> GetAllByPostulant(Guid postulantId)
        {
            var languages = await _context.PostulantOriginalLanguages.Where(x => x.PostulantId == postulantId).ToListAsync();
            return languages;
        }
    }
}
