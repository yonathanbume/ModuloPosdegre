using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class ImageCompanyService: IImageCompanyService
    {
        private readonly IImageCompanyRepository _imageCompanyRepository;

        public ImageCompanyService(IImageCompanyRepository imageCompanyRepository)
        {
            _imageCompanyRepository = imageCompanyRepository;
        }

        public async Task Delete(ImageCompany imageCompany)
        {
            await _imageCompanyRepository.Delete(imageCompany);
        }

        public async Task<ImageCompany> GetFirstOrDefaultById(Guid id)
        {
            return await _imageCompanyRepository.GetFirstOrDefaultById(id);
        }

        public async Task<object> GetImageCompanies(Guid companyId)
        {
            return await _imageCompanyRepository.GetImageCompanies(companyId);
        }

        public async Task InsertRange(IEnumerable<ImageCompany> imageCompanies)
        {
            await _imageCompanyRepository.InsertRange(imageCompanies);
        }
    }
}
