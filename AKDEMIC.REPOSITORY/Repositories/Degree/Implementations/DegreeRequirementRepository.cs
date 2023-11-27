using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Implementations
{
    public class DegreeRequirementRepository : Repository<DegreeRequirement>, IDegreeRequirementRepository
    {
        public DegreeRequirementRepository(AkdemicContext context) : base(context)
        {

        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetDegreeRequirementDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {        

            var query = _context.DegreeRequirements.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query                
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    Type = ConstantHelpers.GRADE_INFORM.DegreeType.VALUES.ContainsKey(x.Type) ? ConstantHelpers.GRADE_INFORM.DegreeType.VALUES[x.Type] : "--"
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<DegreeRequirement>> GetDegreeRequirementsByType(int type)
        {
            var result = await _context.DegreeRequirements.Where(x => x.Type == type).ToListAsync();
            return result;
        }
    }
}
