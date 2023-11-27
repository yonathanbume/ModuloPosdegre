using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IGalleryService
    {
        Task<bool> ExistName(string name, Guid? id = null);
        Task Insert(Gallery newItem);
        Task DeleteById(Guid id);
        Task<IEnumerable<Gallery>> GetAllByPublicationId(Guid id, string search);
        Task<IEnumerable<Gallery>> GetAllByAcademicAgreementId(Guid academicAgreementId, string search);
        Task<IEnumerable<Gallery>> GetAllByScholarshipId(Guid scholarshipId);
        Task DeleteRange(IEnumerable<Gallery> entites);
        Task<GalleryTemplate> GetAllServerSideByScholarshipId(int page, Guid scholarshipId, string search);
        Task<Gallery> Get(Guid id);
    }
}
