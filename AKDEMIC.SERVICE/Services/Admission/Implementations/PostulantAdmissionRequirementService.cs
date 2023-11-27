using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class PostulantAdmissionRequirementService : IPostulantAdmissionRequirementService
    {
        private readonly IPostulantAdmissionRequirementRepository _postulantAdmissionRequirementRepository;

        public PostulantAdmissionRequirementService(IPostulantAdmissionRequirementRepository postulantAdmissionRequirementRepository)
        {
            _postulantAdmissionRequirementRepository = postulantAdmissionRequirementRepository;
        }

        public async Task Add(PostulantAdmissionRequirement postulantAdmissionRequirement)
            => await _postulantAdmissionRequirementRepository.Add(postulantAdmissionRequirement);
        public async Task InsertPostulantAdmissionRequirement(PostulantAdmissionRequirement postulantAdmissionRequirement) =>
            await _postulantAdmissionRequirementRepository.Insert(postulantAdmissionRequirement);

        public async Task UpdatePostulantAdmissionRequirement(PostulantAdmissionRequirement postulantAdmissionRequirement) =>
            await _postulantAdmissionRequirementRepository.Update(postulantAdmissionRequirement);

        public async Task DeletePostulantAdmissionRequirement(PostulantAdmissionRequirement postulantAdmissionRequirement) =>
            await _postulantAdmissionRequirementRepository.Delete(postulantAdmissionRequirement);

        public async Task<IEnumerable<PostulantAdmissionRequirement>> GetAllPostulantAdmissionRequirements() =>
            await _postulantAdmissionRequirementRepository.GetAll();

        public async Task<PostulantAdmissionRequirement> GetPostulantAndAdmissionRequirement(Guid postulantId, Guid requirementId)
            => await _postulantAdmissionRequirementRepository.GetPostulantAndAdmissionRequirement(postulantId, requirementId);

        public async Task InsertRange(List<PostulantAdmissionRequirement> postulantAdmissionRequirements)
            => await _postulantAdmissionRequirementRepository.InsertRange(postulantAdmissionRequirements);

        public async Task RemoveByPostulantId(Guid postulantId) => await _postulantAdmissionRequirementRepository.RemoveByPostulantId(postulantId);

        public async Task<IEnumerable<PostulantAdmissionRequirement>> GetAllByPostulant(Guid postulantId) 
            => await _postulantAdmissionRequirementRepository.GetAllByPostulant(postulantId);
    }
}
