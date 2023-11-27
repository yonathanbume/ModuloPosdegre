using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class AeContactMessageRepository : Repository<AeContactMessage>, IAeContactMessageRepository
    {
        public AeContactMessageRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            var query = _context.AeContactMessages
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.User.UserName.Trim().Contains(search.ToLower().Trim()) || x.User.FullName.Trim().Contains(search.ToLower().Trim()));

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    x.Email,
                    x.PhoneNumber,
                    x.Description
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<object> GetDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            var query = _context.AeContactMessages
                .OrderByDescending(x => x.CreatedAt)
                .Where(x=>x.UserId == userId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    x.Email,
                    x.PhoneNumber,
                    x.Description
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

    }
}
