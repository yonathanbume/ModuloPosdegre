using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IPostulantAdmissionRequirementRepository : IRepository<PostulantAdmissionRequirement>
    {
        Task<PostulantAdmissionRequirement> GetPostulantAndAdmissionRequirement(Guid postulantId, Guid requirementId);
        Task RemoveByPostulantId(Guid postulantId);
        Task<IEnumerable<PostulantAdmissionRequirement>> GetAllByPostulant(Guid postulantId);
    }
}
