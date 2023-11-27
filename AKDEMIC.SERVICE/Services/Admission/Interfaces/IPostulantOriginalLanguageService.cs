using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IPostulantOriginalLanguageService
    {
        Task Insert(PostulantOriginalLanguage entity);
        Task InsertRange(List<PostulantOriginalLanguage> languages);
        Task<PostulantOriginalLanguage> Get(Guid id);
        Task Update(PostulantOriginalLanguage entity);
        Task Delete(PostulantOriginalLanguage entity);
        Task DeleteAllByPostulant(Guid postulantId);
        Task<List<PostulantOriginalLanguage>> GetAllByPostulant(Guid postulantId);
    }
}
