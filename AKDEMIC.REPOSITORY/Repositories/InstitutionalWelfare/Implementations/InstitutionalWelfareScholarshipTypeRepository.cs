using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareScholarshipTypeRepository : Repository<InstitutionalWelfareScholarshipType>, IInstitutionalWelfareScholarshipTypeRepository
    {
        public InstitutionalWelfareScholarshipTypeRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _context.InstitutionalWelfareScholarshipTypes.AnyAsync(x => x.Name.Trim().ToLower() == name.Trim().ToLower() && x.Id != ignoredId);

        public async Task<bool> AnyScholarship(Guid id)
            => await _context.InstitutionalWelfareScholarships.AnyAsync(y => y.InstitutionalWelfareScholarshipTypeId == id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetScholarshipTypesDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
        {
            var query = _context.InstitutionalWelfareScholarshipTypes.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Name,
                      scholarshipCount = x.InstitutionalWelfareScholarships.Count()
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetScholarshipTypeSelect2()
        {
            var result = await _context.InstitutionalWelfareScholarshipTypes
                .Select(x => new
                {
                    x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return result;
        }
    }
}
