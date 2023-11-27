using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class CafeteriaServiceTermRepository: Repository<CafeteriaServiceTerm>, ICafeteriaServiceTermRepository
    {
        public CafeteriaServiceTermRepository(AkdemicContext context) : base(context) { }

        public async Task<CafeteriaServiceTerm> FirstOrDefaultById(Guid id)
        {
            var result = await _context.CafeteriaServiceTerms.Where(x => x.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<CafeteriaServiceTerm> GetActiveCafeteriaServiceTerm()
        {
            return await _context.CafeteriaServiceTerms.Where(x => (x.DateBegin <= DateTime.UtcNow && x.DateEnd > DateTime.UtcNow)).FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaServiceTermDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query =  _context.CafeteriaServiceTerms.OrderByDescending(x => x.CreatedAt).AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Description.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    dateBegin = x.DateBegin.ToLocalDateFormat(),
                    dateEnd = x.DateEnd.ToLocalDateFormat(),
                    x.Year,
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaServiceTermByStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, string searchValue = null)
        {
            var query = _context.CafeteriaServiceTerms.OrderByDescending(x => x.CreatedAt).AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Description.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    dateBegin = x.DateBegin.ToLocalDateFormat(),
                    dateEnd = x.DateEnd.ToLocalDateFormat(),
                    IsPostulated = x.CafeteriaPostulations.Where(y=>y.StudentId == studentId).Any(),
                    PostulationStatus = x.CafeteriaPostulations.Where(y => y.StudentId == studentId).Select(z=>z.Status).FirstOrDefault(),
                    PostulationStatusText = ConstantHelpers.CAFETERIA_POSTULATION.VALUES
                    .ContainsKey(x.CafeteriaPostulations.Where(y => y.StudentId == studentId).Select(z => z.Status).FirstOrDefault()) ?
                        ConstantHelpers.CAFETERIA_POSTULATION.VALUES[x.CafeteriaPostulations.Where(y => y.StudentId == studentId).Select(z => z.Status).FirstOrDefault()] : "",
                    x.Year,
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

        public async Task<object> GetCustom(Guid id)
        {
            var result = await _context.CafeteriaServiceTerms.Where(x => x.Id == id).Select(x => new
            {
                x.Id,
                DateBegin = x.DateBegin.ToLocalDateFormat(),
                DateEnd = x.DateEnd.ToLocalDateFormat(),
                x.Description,
                x.Year,
                x.DaysPerWeek,
                x.WeeksPerTerm

            }).FirstOrDefaultAsync();
            return result;
        }
    }
}
