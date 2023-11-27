using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class GalleryService : IGalleryService
    {
        private readonly IGalleryRepository _galleryRepository;
        public GalleryService(IGalleryRepository galleryRepository)
        {
            _galleryRepository = galleryRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _galleryRepository.DeleteById(id);
        }

        public async Task DeleteRange(IEnumerable<Gallery> entites)
            => await _galleryRepository.DeleteRange(entites);

        public async Task<bool> ExistName(string name, Guid? id = null)
        {
            return await _galleryRepository.ExistName(name, id);
        }

        public async Task<Gallery> Get(Guid id)
            => await _galleryRepository.Get(id);

        public async Task<IEnumerable<Gallery>> GetAllByAcademicAgreementId(Guid academicAgreementId, string search)
        {
            return await _galleryRepository.GetAllByAcademicAgreementId(academicAgreementId, search);
        }

        public async Task<IEnumerable<Gallery>> GetAllByPublicationId(Guid id, string search)
        {
            return await _galleryRepository.GetAllByPublicationId(id, search);
        }

        public async Task<IEnumerable<Gallery>> GetAllByScholarshipId(Guid scholarshipId)
            => await _galleryRepository.GetAllByScholarshipId(scholarshipId);

        public async Task<GalleryTemplate> GetAllServerSideByScholarshipId(int page, Guid scholarshipId, string search)
            => await _galleryRepository.GetAllServerSideByScholarshipId(page, scholarshipId, search);

        public async Task Insert(Gallery newItem)
            => await _galleryRepository.Insert(newItem);
    }
}
