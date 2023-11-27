using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IGalleryRepository : IRepository<Gallery>
    {
        Task<bool> ExistName(string name, Guid? id);
        Task<IEnumerable<Gallery>> GetAllByPublicationId(Guid id, string search);
        Task<IEnumerable<Gallery>> GetAllByAcademicAgreementId(Guid academicAgreementId, string search);
        Task<IEnumerable<Gallery>> GetAllByScholarshipId(Guid scholarshipId);
        Task<GalleryTemplate> GetAllServerSideByScholarshipId(int page, Guid scholarshipId, string search);
    }
}
