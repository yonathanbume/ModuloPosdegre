using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class FavoriteCompanyService : IFavoriteCompanyService
    {
        private readonly IFavoriteCompanyRepository _favoriteCompanyRepository;

        public FavoriteCompanyService(IFavoriteCompanyRepository favoriteCompanyRepository)
        {
            _favoriteCompanyRepository = favoriteCompanyRepository;
        }

        public Task Delete(FavoriteCompany favoriteCompany)
            => _favoriteCompanyRepository.Delete(favoriteCompany);

        public Task<FavoriteCompany> GetByCompanyAndUser(Guid companyId, string userId)
            => _favoriteCompanyRepository.GetByCompanyAndUser(companyId, userId);

        public Task<IEnumerable<FavoriteCompany>> GetAll()
            => _favoriteCompanyRepository.GetAll();

        public Task Insert(FavoriteCompany favoriteCompany)
            => _favoriteCompanyRepository.Insert(favoriteCompany);

        public Task Update(FavoriteCompany favoriteCompany)
            => _favoriteCompanyRepository.Update(favoriteCompany);
    }
}
