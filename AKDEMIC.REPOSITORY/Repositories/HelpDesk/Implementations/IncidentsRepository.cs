using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.HelpDesk.Implementations
{
    public class IncidentsRepository : Repository<Incident>, IIncidentsRepository
    {
        public IncidentsRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<IncidentTemplate, dynamic>> GetIncidentsDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Code);
                case "1":
                    return ((x) => x.Name);
                case "2":
                    return ((x) => x.Description);
                case "3":
                    return ((x) => x.UserReporting);
                case "4":
                    return ((x) => x.AssignedTechnician);
                case "5":
                    return ((x) => x.Date);
                default:
                    return ((x) => x.Code);
            }
        }

        private Func<IncidentTemplate, string[]> GetIncidentsDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Code + "",
                x.Name + "",
                x.UserReporting+ "",
                x.AssignedTechnician+ "",
                x.Description+"",
                x.Date+"",
                x.Dependency+"",
            };
        }

        private async Task<DataTablesStructs.ReturnedData<IncidentTemplate>> GetIncidentsDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<IncidentTemplate, IncidentTemplate>> selectPredicate = null, Expression<Func<IncidentTemplate, dynamic>> orderByPredicate = null, Func<IncidentTemplate, string[]> searchValuePredicate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var query = _context.Incidents
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                query = query.Where(x => x.UserReportingIncidentId == userId);
            }

            var result = query
                .Select(s => new IncidentTemplate
                {
                    Id = s.Id,
                    Code = s.CodeForUsers,
                    //Name = s.Name,
                    Description = s.Description,
                    StatusId = s.Status,
                    UserReporting = string.IsNullOrEmpty(s.UserReportingIncident.FullName) ? "No Especificado" : s.UserReportingIncident.FullName,
                    AssignedTechnician = string.IsNullOrEmpty(s.AssignedTechnician.FullName) ? "No Asignado" : s.AssignedTechnician.FullName,
                    Date = s.IncidentDate.ToString("dd/MM/yyyy"),
                    Dependency = s.Dependency.Name,
                    Solutions = s.IncidentSolutions.Count()
                }, searchValue)
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }


        private async Task<DataTablesStructs.ReturnedData<IncidentTemplate>> GetIncidentByTechIdDatatable(DataTablesStructs.SentParameters sentParameters, string techId, Expression<Func<IncidentTemplate, IncidentTemplate>> selectPredicate = null, Expression<Func<IncidentTemplate, dynamic>> orderByPredicate = null, Func<IncidentTemplate, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Incidents
                .AsNoTracking();


            var result = query
                .Where(x => x.AssignedTechnicianId == techId)
                .Select(s => new IncidentTemplate
                {
                    Id = s.Id,
                    Code = s.CodeForUsers,
                    //Name = s.Name,
                    Description = s.Description,
                    StatusId = s.Status,
                    UserReporting = string.IsNullOrEmpty(s.UserReportingIncident.FullName) ? "No Especificado" : s.UserReportingIncident.FullName,
                    AssignedTechnician = string.IsNullOrEmpty(s.AssignedTechnician.FullName) ? "No Asignado" : s.AssignedTechnician.FullName,
                    Date = s.IncidentDate.ToString("dd/MM/yyyy"),
                    Dependency = s.Dependency.Name,
                    Solutions = s.IncidentSolutions.Count()
                }, searchValue)
                //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC
        public async Task<DataTablesStructs.ReturnedData<IncidentTemplate>> GetIncidentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
        {
            return await GetIncidentsDatatable(sentParameters, null,/* ExpressionHelpers.SelectIncident(),*/ GetIncidentsDatatableOrderByPredicate(sentParameters), GetIncidentsDatatableSearchValuePredicate(), searchValue, user);
        }

        public async Task<IEnumerable<IncidentType>> GetTypes(Guid id)
        {
            var types = await _context.IncidentTypes.Where(x => x.IncidentId == id).ToListAsync();

            return types;
        }
        public async override Task DeleteById(Guid id)
        {
            var incident = await _context.Incidents.Include(x => x.IncidentTypes).FirstOrDefaultAsync(x => x.Id == id);
            _context.IncidentTypes.RemoveRange(incident.IncidentTypes);
            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();
        }
        public async override Task<Incident> Get(Guid id)
        {
            return await _context.Incidents.Include(x => x.IncidentTypes).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task DeleteTypes(Guid id)
        {
            var incident = await _context.Incidents.Include(x => x.IncidentTypes).FirstOrDefaultAsync(x => x.Id == id);
            _context.IncidentTypes.RemoveRange(incident.IncidentTypes);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Incident>> GetIncidentsByDates(DateTime start, DateTime end)
        {
            return await _context.Incidents.Where(x => x.IncidentDate >= start && x.IncidentDate <= end).ToListAsync();
        }

        public async Task<TypeChartTemplate> GetTypesChart()
        {
            var parents = await _context.IncidentTypes
                        .GroupBy(x => x.ParentId)
                        .Select(x => new TypeChartItem
                        {
                            Key = x.Key,
                            Count = x.Count()
                        })
                        .ToListAsync();
            var childs = new List<TypeChartChildItem>();

            parents.ForEach(item =>
            {
                item.Childs = _context.IncidentTypes
                                       .Where(x => x.ParentId == item.Key)
                                       .GroupBy(x => x.ChildId)
                                       .Select(x => new TypeChartChildItem

                                       {
                                           Key = x.Key,
                                           Count = x.Count(),
                                           parentKey = item.Key
                                       })
                                       .ToList();
            });

            TypeChartTemplate template = new TypeChartTemplate();
            template.Parents = parents;
            return template;
        }

        public async Task<TechsChartTemplate> GetTechsChart()
        {
            const int ClosedIncident = 2;

            //tipos de incidentes
            var groupedIncidentTypes = await _context.Incidents.Include(x => x.IncidentTypes).ToListAsync();

            //tecnico + cant. incidentes resueltos
            var techs = await _context.Incidents
                    .Where(x => x.Status == ClosedIncident)
                    .GroupBy(x => x.AssignedTechnicianId)
                    .Select(x => new TechsChartItem
                    {
                        Key = x.Key,
                        Count = x.Count()
                    }).ToListAsync();

            var IncidentsTypeListItem = new List<IncidentTypesByTechs>();
            techs.ForEach(tech =>
            {
                var incidentclose = _context.IncidentTypes
                                        .Where(x => x.Incident.AssignedTechnicianId == tech.Key)
                                        .GroupBy(x => x.ParentId)
                                        .Select(x => new IncidentTypeItem
                                        {
                                            IncidentTypeId = x.Key,
                                            Qty = x.Count()
                                        })
                                        .ToList();

                incidentclose.ForEach(f =>
                {
                    var inc = _context.IncidentTypes
                                      .Where(x => x.Incident.AssignedTechnicianId == tech.Key && f.IncidentTypeId == x.ParentId)
                                      .Select(x => new { x.ParentId, x.IncidentId })
                                      .Distinct();
                    f.Qty = inc.Count();
                });

                IncidentsTypeListItem.Add(new IncidentTypesByTechs { incidentTypes = incidentclose, techId = tech.Key });
            });

            TechsChartTemplate template = new TechsChartTemplate();
            template.Techs = techs;
            template.IncidentsTypeListItem = IncidentsTypeListItem;
            return template;
        }

        public async Task<MonthlyChartTemplate> GetMonthlyChart()
        {
            var grouped = await _context.Incidents.Where(x => x.IncidentDate >= DateTime.UtcNow.AddYears(-1) && x.IncidentDate <= DateTime.UtcNow)
                            .GroupBy(x => new { date = new { x.IncidentDate.Month, x.IncidentDate.Year }, x.Status })
                            .Select(x => new MonthlyChartItem
                            {
                                Year = x.Key.date.Year,
                                Key = x.Key.date.Month,
                                Count = x.Count(),
                                Status = x.Key.Status,
                            })
                            .OrderBy(x => x.Year)
                            .ThenBy(x => x.Key)
                            .ToListAsync();
            var result = new MonthlyChartTemplate();
            result.list = grouped;
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<IncidentTemplate>> GetIncidentByTechIdDatatable(DataTablesStructs.SentParameters sentParameters, string techId, string searchValue = null)
        {
            return await GetIncidentByTechIdDatatable(sentParameters,techId,null,null,null,searchValue);
        }
        #endregion
    }
}
