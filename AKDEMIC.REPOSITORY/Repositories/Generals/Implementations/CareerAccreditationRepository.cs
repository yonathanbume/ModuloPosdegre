using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class CareerAccreditationRepository : Repository<CareerAccreditation>, ICareerAccreditationRepository
    {
        public CareerAccreditationRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetCareerAccreditationChart(Guid? careerId = null, DateTime? EndDate = null)
        {
            var query = _context.CareerAccreditations.AsNoTracking();

            if (careerId != null) query = query.Where(x => x.CareerId == careerId);

            if (EndDate != null) query = query.Where(x => x.EndDate.Date > EndDate.Value.Date);

            var careers = await _context.Careers
                .Select(x => new
                {
                    Career = x.Name,
                    Accepted = query.Where(y => y.CareerId == x.Id).Count()
                }).ToListAsync();

            var result = new
            {
                categories = careers.Select(x => x.Career).ToList(),
                data = careers.Select(x => x.Accepted).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareerAccreditationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, DateTime? EndDate = null)
        {
            Expression<Func<CareerAccreditation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Description);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.StartDate);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.EndDate);
                    break;
            }

            var query = _context.CareerAccreditations.AsNoTracking();

            if (careerId != null) query = query.Where(x => x.CareerId == careerId);

            if (EndDate != null) query = query.Where(x => x.EndDate.Date > EndDate.Value.Date);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    description = x.Description,
                    career = x.Career.Name,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat()
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareerAcreditationsHistoryDatatable(DataTablesStructs.SentParameters parameters,Guid careerId ,string searchValue)
        {
            var query = _context.CareerAccreditations.Where(x => x.CareerId == careerId).AsQueryable();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescending(x=>x.StartDate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    x.ResolutionFile,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    x.Description
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<List<CareerAccreditation>> GetDataList()
        {
            var result = await _context.CareerAccreditations
                .Select(x => new CareerAccreditation
                {
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Career = new Career
                    {
                        Name = x.Career.Name
                    }
                })
                .ToListAsync();

            return result;
        }
    }
}
