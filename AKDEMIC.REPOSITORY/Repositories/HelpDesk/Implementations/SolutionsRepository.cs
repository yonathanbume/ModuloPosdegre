using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.HelpDesk.Implementations
{
    public class SolutionsRepository : Repository<Solution>, ISolutionsRepository
    {
        public SolutionsRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE
        //SOLUTION DATA TABLE
        private Expression<Func<SolutionTemplate, dynamic>> GetSolutionsDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Code);
                case "1":
                    return ((x) => x.Name);
                case "2":
                    return ((x) => x.Description);
                default:
                    return ((x) => x.Code);
            }
        }

        private Func<SolutionTemplate, string[]> GetSolutionsDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Code + "",
                x.Name + "",
                x.Description+"",
            };
        }

        private async Task<DataTablesStructs.ReturnedData<SolutionTemplate>> GetSolutionsDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<SolutionTemplate, SolutionTemplate>> selectPredicate = null, Expression<Func<SolutionTemplate, dynamic>> orderByPredicate = null, Func<SolutionTemplate, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Solutions
                .AsNoTracking();

            var result = query
                .Select(x => new SolutionTemplate
                {
                    Id = x.Id,
                    Code = x.CodeForUsers,
                    Name = x.Name,
                    Description = x.Description
                }, searchValue)
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }

        //INCIDENT SOLUTION TABLE
        private Expression<Func<IncidentSolutionTemplate, dynamic>> GetIncidentSolutionsTableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Code);
                case "1":
                    return ((x) => x.Name);
                case "2":
                    return ((x) => x.Description);
                //case "2":
                //    return ((x) => x.);
                default:
                    return ((x) => x.Code);
            }
        }

        private async Task<DataTablesStructs.ReturnedData<IncidentSolutionTemplate>> GetIncidentSolutionsTable(Guid incidentId, DataTablesStructs.SentParameters sentParameters, Expression<Func<IncidentSolutionTemplate, IncidentSolutionTemplate>> selectPredicate = null, Expression<Func<IncidentSolutionTemplate, dynamic>> orderByPredicate = null, Func<IncidentSolutionTemplate, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.IncidentSolutions
                    .Where(x => x.IncidentId == incidentId)
                .AsNoTracking();

            var result = query
                .Select(s => new IncidentSolutionTemplate
                {
                    Id = s.Id,
                    Code = s.Solution.CodeForUsers,
                    Description = s.Solution.Description,
                    StatusId = s.Status,
                    IncidentId = s.IncidentId,
                    SolutionId = s.SolutionId,
                    Name = s.Solution.Name,
                    Date = s.SolutionDate.ToString("dd/MM/yyyy")
                }, searchValue)
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }

        //INCIDENT TABLE
        private Expression<Func<IncidentSolutionTemplate, dynamic>> GetIncidentsTableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Code);
                case "1":
                    return ((x) => x.Name);
                case "2":
                    return ((x) => x.Description);
                //case "2":
                //    return ((x) => x.);
                default:
                    return ((x) => x.Code);
            }
        }

        private async Task<DataTablesStructs.ReturnedData<IncidentSolutionTemplate>> GetIncidentsTable(Guid solutionId, DataTablesStructs.SentParameters sentParameters, Expression<Func<IncidentSolutionTemplate, IncidentSolutionTemplate>> selectPredicate = null, Expression<Func<IncidentSolutionTemplate, dynamic>> orderByPredicate = null, Func<IncidentSolutionTemplate, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.IncidentSolutions
                    .Where(x => x.SolutionId == solutionId)
                .AsNoTracking();

            var result = query
                .Select(s => new IncidentSolutionTemplate
                {
                    Id = s.Id,
                    Code = s.Incident.CodeForUsers,
                    StatusId = s.Status,
                    IncidentId = s.IncidentId,
                    SolutionId = s.SolutionId,
                    //Name = s.Incident.Name,
                    Description=s.Incident.Description,
                    Date = s.SolutionDate.ToString("dd/MM/yyyy")
                }, searchValue)
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }

        //SOLUTION TABLE
        private Expression<Func<SolutionTemplate2, dynamic>> GetSolutionsTableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Code);
                case "1":
                    return ((x) => x.Name);
                case "2":
                    return ((x) => x.Description);
                //case "2":
                //    return ((x) => x.);
                default:
                    return ((x) => x.Code);
            }
        }

        private async Task<DataTablesStructs.ReturnedData<SolutionTemplate2>> GetSolutionsTable(Guid incidenteId, DataTablesStructs.SentParameters sentParameters, Expression<Func<SolutionTemplate2, SolutionTemplate2>> selectPredicate = null, Expression<Func<SolutionTemplate2, dynamic>> orderByPredicate = null, Func<SolutionTemplate2, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var qry2 = _context.IncidentSolutions
                    .Where(x => x.IncidentId == incidenteId)
                .AsNoTracking();

            var qry1 = _context.Solutions.AsNoTracking();

            var result = qry1
                .Select(y => new SolutionTemplate2
                {
                    Name = y.Name,
                    Id = y.Id,
                    Code = y.CodeForUsers,
                    Description = y.Description,
                    Selected = qry2.Any(x => x.SolutionId == y.Id)
                }, searchValue)
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC
        public async Task InsertIncidentSolution(IncidentSolution incidentSolution)
        {
            await _context.IncidentSolutions.AddAsync(incidentSolution);
            await _context.SaveChangesAsync();
        }
        public async Task InsertNewIncidentsSolution(Guid incidentId, List<Guid> idsSols)
        {
            var listIncidentSolutions = new List<IncidentSolution>();

            var oldincidents = _context.IncidentSolutions.Where(x => x.IncidentId == incidentId);
            //elimino los anteriores
            foreach (var item in oldincidents)
            {
                _context.Entry(item).State = EntityState.Deleted;
            }
            //agregar nuevos
            foreach (var item in idsSols)
            {
                var newincident = new IncidentSolution()
                {
                    IncidentId = incidentId,
                    SolutionDate = DateTime.Today,
                    SolutionId = item,
                    Status = 1,
                };
                //verificar si existe
                _context.IncidentSolutions.Add(newincident);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<IncidentSolution> GetIncidentSolution(Guid incidentId)
        {
            return await _context.IncidentSolutions
                                .Include(x=>x.Solution)
                                .Include(x=>x.Incident)
                                .SingleOrDefaultAsync(x => x.Id == incidentId);
        }
        public async Task Delete(Guid incidentId)
        {
            _context.Remove(await _context.IncidentSolutions.SingleOrDefaultAsync(x => x.Id == incidentId));
            await _context.SaveChangesAsync();
        }

        //DATA TABLES
        public async Task<DataTablesStructs.ReturnedData<SolutionTemplate>> GetSolutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetSolutionsDatatable(sentParameters, null, GetSolutionsDatatableOrderByPredicate(sentParameters), GetSolutionsDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<IncidentSolutionTemplate>> GetIncidentSolutionsTable(DataTablesStructs.SentParameters sentParameters, Guid incidenteId)
        {
            return await GetIncidentSolutionsTable(incidenteId, sentParameters, null, GetIncidentSolutionsTableOrderByPredicate(sentParameters), null);
        }

        public async Task<DataTablesStructs.ReturnedData<IncidentSolutionTemplate>> GetIncidentsTable(DataTablesStructs.SentParameters sentParameters, Guid solutionId)
        {
            return await GetIncidentsTable(solutionId,sentParameters, null, GetIncidentsTableOrderByPredicate(sentParameters), null);
        }

        public async Task<DataTablesStructs.ReturnedData<SolutionTemplate2>> GetSolutionsTable(DataTablesStructs.SentParameters sentParameters, Guid incidenteId)
        {
            return await GetSolutionsTable(incidenteId,sentParameters, null, GetSolutionsTableOrderByPredicate(sentParameters), null);
        }
        #endregion
    }
    public class SolutionTemplate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class SolutionTemplate2 : SolutionTemplate
    {
        public bool Selected { get; set; }
    }
    public class IncidentSolutionTemplate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte StatusId { get; set; }
        public string UserReporting { get; set; }
        public string AssignedTechnician { get; set; }
        public string Date { get; set; }
        public Guid IncidentId { get; set; }
        public Guid SolutionId { get; set; }
    }
}