using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Interfaces
{
    public interface IBannerService
    {
        Task InsertBanner(Banner banner);
        Task UpdateBanner(Banner banner);
        Task DeleteBanner(Banner banner);
        Task<Banner> GetBannerById(Guid id);
        Task<IEnumerable<Banner>> GetAllBanners();
        Task<DataTablesStructs.ReturnedData<BannerTemplate>> GetAllBannerDatatable(DataTablesStructs.SentParameters sentParameters, string headline, byte status);
        Task<BannerTemplate> GetAvailableOrdersAndListSequenceOrder();
        Task<BannerTemplate> GetBannerTemplateById(Guid id);
        Task<List<BannerTemplate>> GetBannersToHome();
        Task<List<Banner>> GetAllBannersActive();
        Task<IEnumerable<object>> GetAvailableOrdersAndListSequenceOrderSelect2();
    }
}
