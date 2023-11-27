using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class VocationalTestAnswerCareerPostulantService : IVocationalTestAnswerCareerPostulantService
    {
        private readonly IVocationalTestAnswerCareerPostulantRepository _vocationalTestAnswerCareerPostulantRepository;

        public VocationalTestAnswerCareerPostulantService(IVocationalTestAnswerCareerPostulantRepository vocationalTestAnswerCareerPostulantRepository)
        {
            _vocationalTestAnswerCareerPostulantRepository = vocationalTestAnswerCareerPostulantRepository;
        }

        public async Task<IEnumerable<VocationalTestAnswerCareerPostulant>> GetAll()
        {
            return await _vocationalTestAnswerCareerPostulantRepository.GetAll();
        }

        public async Task<object> GetChart(Guid applicationTermId)
        {
            return await _vocationalTestAnswerCareerPostulantRepository.GetChart(applicationTermId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetVocationalTestAnswerCareerDataTable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, string search = null)
        {
            return await _vocationalTestAnswerCareerPostulantRepository.GetVocationalTestAnswerCareerDataTable(sentParameters,applicationTermId, search);
        }

        public async Task<object> GetVocationalTestAnswerCareerPostulantsFiltered(Guid postulantId)
        {
            return await _vocationalTestAnswerCareerPostulantRepository.GetVocationalTestAnswerCareerPostulantsFiltered(postulantId);
        }

        public async Task Insert(VocationalTestAnswerCareerPostulant vocationalTestAnswerCareerPostulant)
        {
            await _vocationalTestAnswerCareerPostulantRepository.Insert(vocationalTestAnswerCareerPostulant);
        }
    }
}
