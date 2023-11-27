using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Implementations
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(AkdemicContext context) : base(context) { }

        public async Task<Contact> GetFirstContact()
        {
            var model = await _context.Contacts
                .Select(x => new Contact
                {
                    Id = x.Id,
                    Phone = x.Phone,
                    Celphone = x.Celphone,
                    Description = x.Description,
                    Email = x.Email,
                    Location = x.Location,
                    Url = x.Url
                }).FirstOrDefaultAsync();

            return model;
        }
    }
}
