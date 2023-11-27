using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class ChannelContactRepository : Repository<ChannelContact>, IChannelContactRepository
    {
        public ChannelContactRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetSelect2TypeChannels(Guid companyId)
        {
            var result = _context.ChannelContacts.Where(x => x.CompanyId == companyId).Select(x => new
            {
                x.Channel,
                x.Description
            });
            return await result.ToListAsync();
        }       
            

        public async Task<IEnumerable<ChannelContact>> GetTypeChannelsByCompany(Guid companyId)
        {
            var result = _context.ChannelContacts.Where(x => x.CompanyId == companyId);
            return await result.ToListAsync();
        }
    }
    
}
