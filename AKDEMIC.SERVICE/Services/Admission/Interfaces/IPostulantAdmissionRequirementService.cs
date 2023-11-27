using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IPostulantAdmissionRequirementService
    {
        Task Add(PostulantAdmissionRequirement postulantAdmissionRequirement);
        Task InsertRange(List<PostulantAdmissionRequirement> postulantAdmissionRequirements);
        Task InsertPostulantAdmissionRequirement(PostulantAdmissionRequirement postulantAdmissionRequirement);
        Task UpdatePostulantAdmissionRequirement(PostulantAdmissionRequirement postulantAdmissionRequirement);
        Task DeletePostulantAdmissionRequirement(PostulantAdmissionRequirement postulantAdmissionRequirement);
        Task<IEnumerable<PostulantAdmissionRequirement>> GetAllPostulantAdmissionRequirements();
        Task<IEnumerable<PostulantAdmissionRequirement>> GetAllByPostulant(Guid postulantId);
        Task RemoveByPostulantId(Guid postulantId);
        Task<PostulantAdmissionRequirement> GetPostulantAndAdmissionRequirement(Guid postulantId, Guid requirementId);

    }
}
