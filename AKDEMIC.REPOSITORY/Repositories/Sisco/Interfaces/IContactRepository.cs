using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<Contact> GetFirstContact();
    }
}
