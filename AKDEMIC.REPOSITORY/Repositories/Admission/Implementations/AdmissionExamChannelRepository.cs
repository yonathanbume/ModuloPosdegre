using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class AdmissionExamChannelRepository : Repository<AdmissionExamChannel>, IAdmissionExamChannelRepository
    {
        public AdmissionExamChannelRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<Channel>> GetChannelsByAdmissionExamId(Guid admissionExamId)
        {
            var channels = await _context.AdmissionExamChannels
                .Where(x => x.AdmissionExamId == admissionExamId)
                .Select(x => x.Channel)
                .ToListAsync();
                
            return channels;
        }
    }
}
