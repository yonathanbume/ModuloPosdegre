using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates.CulturalActivity;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class CulturalActivityRespository : Repository<CulturalActivity>, ICulturalActivityRespository
    {
        public CulturalActivityRespository(AkdemicContext context) : base(context) { }
        public async Task<List<CulturalActivityTemplate>> GetEventsSiscoToHome()
        {
            List<CulturalActivityTemplate> events = await _context.CulturalActivities
                .OrderByDescending(x => x.Date)
                .Take(9)
                .Select(x => new CulturalActivityTemplate
                {
                    Id = x.Id,
                    UrlPicture = x.Image,
                    Date = x.Date.ToDefaultTimeZone().ToString("dd MMMM yyyy", new CultureInfo("es")),
                    Time = x.Date.ToLocalTimeFormat()
                })
                .ToListAsync();
            return events;
        }

        public async Task<List<CulturalActivityTemplate>> GetEventsSiscoAllToHome()
        {
            List<CulturalActivityTemplate> novelties = await _context.CulturalActivities
                .Select(x => new CulturalActivityTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Place = x.Place,
                    Description = x.Description,
                    Date = x.Date.ToDefaultTimeZone().ToString("dddd, dd MMMM yyyy", new CultureInfo("es-PE")),
                    Time = x.Date.ToLocalTimeFormat(),
                    UrlPicture = x.Image
                })
                .ToListAsync();
            return novelties;
        }

        public async Task<CulturalActivityHomeTemplate> GetCulturalActivity(Guid id)
        {
            var nowDate = DateTime.UtcNow.ToDefaultTimeZone().Date;

            var query = await _context.CulturalActivities
                .Include(x => x.Career)
                .Include(x => x.CulturalActivityFiles)
                .Where(x => x.Id == id)
                .Where(x => x.Date >= nowDate)
                .ToListAsync();

            var course = query
                .Select(x => new CulturalActivityHomeTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Date = x.Date.ToDefaultTimeZone().ToLongDateString(),
                    Time = $"{x.Date.ToString("hh:mm t", CultureInfo.CreateSpecificCulture("es-PE"))}M",
                    Price = x.Price.HasValue ? x.Price.Value.ToString("0.00") : "",
                    Image = x.Image,
                    Place = x.Place,
                    Objective = x.Objective,
                    Strategy = x.Strategy,
                    Activities = x.Activities,
                    Users = x.Users,
                    Competencies = x.Competencies,
                    Career = x.CareerId.HasValue ? x.Career.Name : "",
                    IsPrivate = x.IsPrivate,
                    Files = x.CulturalActivityFiles.Select(y => new ActivityFile
                    {
                        Id = y.Id,
                        Name = y.FileName
                    }).ToList()
                }).First();
            return course;
        }

        public async Task<List<CulturalActivityHomeTemplate>> GetCulturalActivities(int page)
        {
            var dateNow = DateTime.UtcNow.ToDefaultTimeZone().Date;

            var query = _context.CulturalActivities.AsNoTracking()
                .Where(x => x.Date >= dateNow)
                .OrderBy(x => x.Date)
                .Skip(5 * page)
                .Take(5).AsQueryable();

            var result = await query.ToListAsync();

            var list = result.Select(x => new CulturalActivityHomeTemplate
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Date = x.Date.ToDefaultTimeZone().ToLongDateString(),
                Time = $"{x.Date.ToString("hh:mm t", CultureInfo.CreateSpecificCulture("es-PE"))}M",
                Price = x.Price.HasValue ? x.Price.Value.ToString("0.00") : "",
                Image = x.Image,
                Place = x.Place
            }).ToList();

            return list;
        }

        public async Task<bool> HasMembers(Guid id)
        {
            return (await _context.RegisterCulturalActivities.FirstOrDefaultAsync(x => x.CulturalActivityId == id)) == null ? false : true;
        }

        public async Task<bool> ExistCulturalActivityName(string name, Guid? id)
        {
            IQueryable<CulturalActivity> query = _context.CulturalActivities.AsQueryable();
            if (id.HasValue)
            {
                return await query.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()) && x.Id != id);
            }
            else
            {
                return await query.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()));
            }
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCulturalActivitiesDataTable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<CulturalActivity, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Date); break;
                case "2":
                    orderByPredicate = ((x) => x.Price); break;
                case "3":
                    orderByPredicate = ((x) => x.Registers.Count() * x.Price); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            IQueryable<CulturalActivity> query = _context.CulturalActivities.AsNoTracking();
            int filteredcount = await query.CountAsync();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Type,
                    x.Name,
                    Price = x.Price.HasValue ? $"S/. {x.Price.Value.ToString("0.00")}" : "",
                    Total = x.Price.HasValue ? $"S/. {(x.Registers.Count() * x.Price.Value).ToString("0.00")}" : "",
                    Date = x.Date.ToString("dd/MM/yyyy hh:mm t").ToUpper() + "M",
                    x.IsPrivate
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCulturalActivitiesByYearDataTable(DataTablesStructs.SentParameters sentParameters, int? year = null)
        {
            var query = _context.CulturalActivities.AsNoTracking();

            if (year != null)
            {
                query = query.Where(x => x.Date.Year == year);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Type,
                    x.Name,
                    Participants = x.Registers.Where(x => x.UserId != null).Count(),
                    Date = x.Date.ToString("dd/MM/yyyy hh:mm t").ToUpper() + "M"
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetCulturalActivitiesByYearChart(int? year = null)
        {
            var query = _context.CulturalActivities
                                    .AsQueryable();

            //Total de la comunidad universitaria, por definir
            int totalComunity = 2000;

            if (year != null)
            {
                query = query.Where(x => x.Date.Year == year);
            }

            var preData = await query
                .Select(x => new
                {
                    Activity = x.Name,
                    Year = x.Date.Year,
                    Participants = x.Registers.Where(x => x.UserId != null).Count()
                }).ToListAsync();

            var data = preData
                .GroupBy(x => x.Year)
                .Select(x => new
                {
                    Year = x.Key,
                    Accepted = x.Sum(y => y.Participants)
                })
                .ToList();

            var result = new
            {
                categories = data.Select(x => x.Year).ToList(),
                //data = data.Select(x => x.Accepted).ToList(),
                data = data.Select(x => Math.Round((x.Accepted * 100.0) / totalComunity * 1.0, 2, MidpointRounding.AwayFromZero)).ToList()
            };

            return result;
        }

        public async Task<object> GetCulturalActivitiesYear()
        {
            var query = _context.CulturalActivities.AsNoTracking();

            var result = await query
                .OrderByDescending(x => x.Date.Year)
                .Select(x => new
                {
                    id = x.Date.Year,
                    text = x.Date.Year,
                })
                .Distinct()
                .ToListAsync();

            return result;
        }

        public async Task<object> GetActivitiesPerCareer()
        {
            var total = _context.CulturalActivities
                .Where(x => x.CareerId != null)
                .Select(x => new
                {
                    x.Career.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .OrderBy(x => x.Key)
                .ToList();

            var categories = total.Select(x => x.Key).ToList();
            var data = total.Select(x => x.Count()).ToList();
            return new { categories, data };
        }
        public async Task<object> GetActivityTypesPerCareer()
        {
            var total = _context.CulturalActivities
                .Select(x => new
                {
                    x.Type.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .OrderBy(x => x.Key)
                .ToList();

            var categories = total.Select(x => x.Key).ToList();
            var data = total.Select(x => x.Count()).ToList();
            return new { categories, data };
        }
    }
}
