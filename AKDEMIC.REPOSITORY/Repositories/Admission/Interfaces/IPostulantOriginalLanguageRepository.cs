using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IPostulantOriginalLanguageRepository : IRepository<PostulantOriginalLanguage>
    {
        Task<List<PostulantOriginalLanguage>> GetAllByPostulant(Guid postulantId);
    }
}
