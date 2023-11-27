using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces;
using AKDEMIC.SERVICE.Services.PreprofesionalPractice.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PreprofesionalPractice.Implementations
{
    public class PresentationLetterService : IPresentationLetterService
    {
        private readonly IPresentationLetterRepository _presentationLetterRepository;

        public PresentationLetterService(IPresentationLetterRepository presentationLetterRepository)
        {
            _presentationLetterRepository = presentationLetterRepository;
        }

        public async Task Delete(PresentationLetter entity)
            => await _presentationLetterRepository.Delete(entity);

        public async Task<PresentationLetter> Get(Guid id)
            => await _presentationLetterRepository.Get(id);

        public async Task<string> GetCode()
            => await _presentationLetterRepository.GetCode();

        public async Task<DataTablesStructs.ReturnedData<object>> GetPresentationLettersDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string search)
            => await _presentationLetterRepository.GetPresentationLettersDatatable(sentParameters, user, search);

        public async Task Insert(PresentationLetter entity)
            => await _presentationLetterRepository.Insert(entity);

        public async Task Update(PresentationLetter entity)
            => await _presentationLetterRepository.Update(entity);
    }
}
