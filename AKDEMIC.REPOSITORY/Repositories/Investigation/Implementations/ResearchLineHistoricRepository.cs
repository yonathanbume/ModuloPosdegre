using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class ResearchLineHistoricRepository : Repository<ResearchLineHistoric>, IResearchLineHistoricRepository
    {
        public ResearchLineHistoricRepository(AkdemicContext context) : base(context) { }

        public async Task UpdateLine(Guid lineId)
        {
            var line = await _context.ResearchLines.Where(x => x.Id == lineId).Select(x => new ResearchLineHistoric
            {
                Active = x.Active,
                Name = x.Name,
                ReseachSubArea = x.ResearchDiscipline.ResearchSubArea.Name,
                ResearchArea = x.ResearchDiscipline.ResearchSubArea.ResearchArea.Name,
                ResearchCategory = x.ResearchCategory.Name,
                ResearchDiscipline = x.ResearchDiscipline.Name,
                ResearchLineId = x.Id,
                DateTime = DateTime.UtcNow.ToDefaultTimeZone(),
                Career = x.Career.Name
            }).FirstAsync();
            await _context.ResearchLineHistorics.AddAsync(line);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHistoric(Guid lineId)
        {
            var historics = await _context.ResearchLineHistorics.Where(x => x.ResearchLineId == lineId).ToListAsync();
            _context.ResearchLineHistorics.RemoveRange(historics);
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetHistoricDatatable(DataTablesStructs.SentParameters sentParameters, Guid lineId)
        {
            var query =await  _context.ResearchLineHistorics.Where(x => x.ResearchLineId == lineId).ToListAsync();

            int recordsFiltered = query.Count();
            var data = query
                .OrderByDescending(x => x.DateTime)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    datetime = x.DateTime.ToString("dd/MM/yyyy"),
                    name = x.Name,
                    career = x.Career,
                    category = x.ResearchCategory,
                    discipline = x.ResearchDiscipline,
                    subarea = x.ReseachSubArea,
                    area = x.ResearchArea
                }).ToList();

            int recordsTotal = data.Count;

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
