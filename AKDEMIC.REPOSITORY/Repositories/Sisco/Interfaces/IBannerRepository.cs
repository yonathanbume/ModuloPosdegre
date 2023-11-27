using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces
{
    public interface IBannerRepository : IRepository<Banner>
    {
        Task<DataTablesStructs.ReturnedData<BannerTemplate>> GetAllBannerDatatable(DataTablesStructs.SentParameters sentParameters, string headline = null, byte? status = null);
        Task<BannerTemplate> GetAvailableOrdersAndListSequenceOrder();
        Task<BannerTemplate> GetBannerById(Guid id);
        Task<List<BannerTemplate>> GetBannersToHome();
        Task<List<Banner>> GetAllBannersActive();
        Task<IEnumerable<object>> GetAvailableOrdersAndListSequenceOrderSelect2();
    }
}
