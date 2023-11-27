using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class PostulationService : IPostulationService
    {
        private readonly IPostulationRepository _postulationRepository;

        public PostulationService(IPostulationRepository postulationRepository)
        {
            _postulationRepository = postulationRepository;
        }

        public async Task<Guid> InsertAndReturnId(Postulation questionnaireByUser)
        {
            return await _postulationRepository.InsertAndReturnId(questionnaireByUser);
        }

        public async Task<bool> WasAnsweredByuser(Guid questionnaireId, string userId, string externalUser = null)
        {
            return await _postulationRepository.WasAnsweredByuser(questionnaireId, userId, externalUser);
        }

        public async Task<object> GetPostulantsByscholarship()
        {
            return await _postulationRepository.GetPostulantsByscholarship();
        }

        public async Task<Postulation> Get(Guid id)
            => await _postulationRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<Postulation>> GetPostulationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? scholarshipId = null, byte? state = null, string search = null)
            => await _postulationRepository.GetPostulationsDatatable(sentParameters, scholarshipId, state, search);

        public async Task<List<PostulationTemplate>> GetPostulationsReport(Guid? scholarshipId = null, byte? state = null)
            => await _postulationRepository.GetPostulationsReport(scholarshipId, state);

        public async Task<Postulation> GetQuestionnaireByPostulationId(Guid postulationId)
            => await _postulationRepository.GetQuestionnaireByPostulationId(postulationId);

        public async Task Update(Postulation entity)
            => await _postulationRepository.Update(entity);

        public async Task<Postulation> GetByUserIdAndScholarshipId(Guid scholarshipId, string userId)
            => await _postulationRepository.GetByUserIdAndScholarshipId(scholarshipId, userId);

        public async Task<object> GetAdmittedByProgram()
        {
            return await _postulationRepository.GetAdmittedByProgram();
        }

        public async Task<object> GetPostulantsByProgram()
        {
            return await _postulationRepository.GetPostulantsByProgram();
        }
        public async Task<DataTablesStructs.ReturnedData<Postulation>> GetAdmittedDatatable(DataTablesStructs.SentParameters sentParameters, Guid? scholarshipId = null, byte? state = null, string search = null)
        {
            return await _postulationRepository.GetAdmittedDatatable(sentParameters, scholarshipId, state, search);
        }

        public async Task<object> GetAdmittedByscholarship()
        {
            return await _postulationRepository.GetAdmittedByscholarship();
        }

        public async Task Add(Postulation postulation)
            => await _postulationRepository.Add(postulation);

        public async Task Insert(Postulation postulation)
            => await _postulationRepository.Insert(postulation);

        public async Task<Postulation> GetByUserIdAndQuestionnaireId(string userId, Guid questionnaireId)
            => await _postulationRepository.GetByUserIdAndQuestionnaireId(userId, questionnaireId);

        public async Task<object> GetReportByCareerChart()
            => await _postulationRepository.GetReportByCareerChart();

        public async Task<object> GetReportByTerm(Guid termId)
        {
            return await _postulationRepository.GetReportByTerm(termId);
        }
    }
}
