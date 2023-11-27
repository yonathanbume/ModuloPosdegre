
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IFavoriteCompanyService
    {
        Task<FavoriteCompany> GetByCompanyAndUser(Guid companyId, string userId);
        Task<IEnumerable<FavoriteCompany>> GetAll();
        Task Insert(FavoriteCompany favoriteCompany);
        Task Update(FavoriteCompany favoriteCompany);
        Task Delete(FavoriteCompany favoriteCompany);
    }
}
