using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerFamilyInformationRepository : Repository<WorkerFamilyInformation>, IWorkerFamilyInformationRepository
    {
        public WorkerFamilyInformationRepository(AkdemicContext context) : base(context){ }

        public async Task<List<WorkerFamilyInformation>> GetAllByUserId(string userId)
        {
            var data = await _context.WorkerFamilyInformations.Where(x => x.WorkerLaborInformation.UserId == userId).ToListAsync();
            return data;
        }

        public async Task<object> GetFamilyMembers(Guid workerLaborInformationId)
        {
            var data = await _context.WorkerFamilyInformations
                .Where(x => x.WorkerLaborInformationId == workerLaborInformationId)
                .Select(x => new
                {
                    x.Name,
                    x.PaternalName,
                    x.MaternalName,
                    x.Sex,
                    x.Relationship,
                    x.IsAlive,
                    x.HasDiscapacity,
                    BirthDate = x.BirthDate != null ? x.BirthDate.Value.ToLocalDateFormat(): null,
                    x.DniOtherDoc
                })
                .ToListAsync();

            return data;
        }

        public async Task RemoveRangeByUserId(string userId)
        {
            var workerFamilyInformations = await _context.WorkerFamilyInformations.Where(x => x.WorkerLaborInformation.UserId == userId).ToListAsync();

            if (workerFamilyInformations.Count > 0) _context.WorkerFamilyInformations.RemoveRange(workerFamilyInformations);
        }
    }
}
