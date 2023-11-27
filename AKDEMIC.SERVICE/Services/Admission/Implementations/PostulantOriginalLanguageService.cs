using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class PostulantOriginalLanguageService : IPostulantOriginalLanguageService
    {
        private readonly IPostulantOriginalLanguageRepository _postulantOriginalLanguageRepository;

        public PostulantOriginalLanguageService(IPostulantOriginalLanguageRepository postulantOriginalLanguageRepository)
        {
            _postulantOriginalLanguageRepository = postulantOriginalLanguageRepository;
        }

        public async Task Delete(PostulantOriginalLanguage entity)
        {
            await _postulantOriginalLanguageRepository.Delete(entity);
        }

        public async Task DeleteAllByPostulant(Guid postulantId)
        {
            var languages = await _postulantOriginalLanguageRepository.GetAllByPostulant(postulantId);
            await _postulantOriginalLanguageRepository.DeleteRange(languages);
        }

        public async Task<PostulantOriginalLanguage> Get(Guid id)
        {
           return await _postulantOriginalLanguageRepository.Get(id);
        }

        public async Task<List<PostulantOriginalLanguage>> GetAllByPostulant(Guid postulantId)
        {
            return await _postulantOriginalLanguageRepository.GetAllByPostulant(postulantId);
        }

        public async Task Insert(PostulantOriginalLanguage entity)
        {
            await _postulantOriginalLanguageRepository.Insert(entity);
        }

        public async Task InsertRange(List<PostulantOriginalLanguage> languages)
        {
            await _postulantOriginalLanguageRepository.InsertRange(languages);
        }

        public async Task Update(PostulantOriginalLanguage entity)
        {
            await _postulantOriginalLanguageRepository.Update(entity);
        }
    }
}
