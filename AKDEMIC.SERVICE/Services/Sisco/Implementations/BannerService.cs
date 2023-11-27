using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using AKDEMIC.SERVICE.Services.Sisco.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Implementations
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }
        
        public async Task InsertBanner(Banner banner) =>
            await _bannerRepository.Insert(banner);

        public async Task UpdateBanner(Banner banner) =>
            await _bannerRepository.Update(banner);

        public async Task DeleteBanner(Banner banner) =>
            await _bannerRepository.Delete(banner);

        public async Task<Banner> GetBannerById(Guid id) =>
            await _bannerRepository.Get(id);

        public async Task<IEnumerable<Banner>> GetAllBanners() =>
            await _bannerRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<BannerTemplate>> GetAllBannerDatatable(DataTablesStructs.SentParameters sentParameters, string headline, byte status) =>
            await _bannerRepository.GetAllBannerDatatable(sentParameters, headline, status);

        public async Task<BannerTemplate> GetAvailableOrdersAndListSequenceOrder() =>
            await _bannerRepository.GetAvailableOrdersAndListSequenceOrder();

        public async Task<BannerTemplate> GetBannerTemplateById(Guid id) =>
            await _bannerRepository.GetBannerById(id);

        public async Task<List<BannerTemplate>> GetBannersToHome() =>
            await _bannerRepository.GetBannersToHome();
        public async Task<List<Banner>> GetAllBannersActive()
            => await _bannerRepository.GetAllBannersActive();
        public async Task<IEnumerable<object>> GetAvailableOrdersAndListSequenceOrderSelect2()
            => await _bannerRepository.GetAvailableOrdersAndListSequenceOrderSelect2();

    }
}
