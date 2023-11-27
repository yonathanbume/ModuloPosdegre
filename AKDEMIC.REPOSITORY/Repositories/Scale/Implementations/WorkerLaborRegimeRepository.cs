using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerLaborRegimeRepository : Repository<WorkerLaborRegime>, IWorkerLaborRegimeRepository
    {
        public WorkerLaborRegimeRepository(AkdemicContext context) : base(context) { }

        public async Task<Tuple<int, List<Tuple<string, int>>>> GetWorkerLaborRegimeQuantityReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            var baseQuery = _context.WorkerLaborRegimes
                .Where(x => string.IsNullOrWhiteSpace(paginationParameter.SearchValue) || x.Name.Contains(paginationParameter.SearchValue))
                .AsQueryable();

            var records = await baseQuery.CountAsync();

            var query = baseQuery.Select(x => new
            {
                name = x.Name,
                quantity = x.WorkerLaborInformation.Count
            })
            .AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.name) : query.OrderBy(q => q.name);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.quantity) : query.OrderBy(q => q.quantity);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.name) : query.OrderBy(q => q.name);
                    break;
            }

            var data = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage).ToListAsync();

            var result = data.Select(x => new Tuple<string, int>(x.name, x.quantity)).ToList();

            return new Tuple<int, List<Tuple<string, int>>>(records, result);
        }

        public async Task<List<Tuple<string, int>>> GetWorkerLaborRegimeQuantityReport(string search)
        {
            var data = await _context.WorkerLaborRegimes
                .Where(x => string.IsNullOrWhiteSpace(search) || x.Name.Contains(search))
                .Select(x => new
                {
                    name = x.Name,
                    quantity = x.WorkerLaborInformation.Count()
                })
                .AsQueryable()
                .ToListAsync();

            var result = data.Select(x => new Tuple<string, int>(x.name, x.quantity)).ToList();

            return new List<Tuple<string, int>>(result);
        }

        public async Task<List<Tuple<string, int>>> GetRetirementSystemReport(List<Tuple<string, byte>> retirementSystems)
        {
            var data = new List<Tuple<string, int>>();

            foreach (var item in retirementSystems)
                data.Add(new Tuple<string, int>(item.Item1, await _context.WorkerRetirementSystemHistories.Where(x => x.Active).CountAsync(x => x.RetirementSystem == item.Item2)));
                //data.Add(new Tuple<string, int>(item.Item1, await _context.WorkerLaborInformation.CountAsync(x => x.RetirementSystem == item.Item2)));

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborRegimeDatatable(DataTablesStructs.SentParameters sentParameters,int status,string searchValue = null)
        {
            Expression<Func<WorkerLaborRegime, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name; break;
                case "1":
                    orderByPredicate = (x) => x.LegalRegulation; break;
                case "2":
                    orderByPredicate = (x) => x.PublicationDateRegulation; break;
                case "3":
                    orderByPredicate = (x) => x.Status; break;
            }

            var query = _context.WorkerLaborRegimes.AsQueryable();

            if (status != -1)// Si es -1 ignorar el estado
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    name = x.Name,
                    status = x.Status,
                    legalRegulation = x.LegalRegulation,
                    publicationDateRegulation = x.PublicationDateRegulation.ToLocalDateFormat(),
                })
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

        public async Task<IEnumerable<WorkerLaborRegime>> GetAll(string search, bool? onlyActive)
        {
            var query = _context.WorkerLaborRegimes.AsQueryable();

            if (onlyActive.HasValue && onlyActive.Value)
                query = query.Where(x => x.Status == ConstantHelpers.STATES.ACTIVE);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));

            return await query.ToArrayAsync();

        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _context.WorkerLaborRegimes.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != ignoredId);

        public async Task<Tuple<bool,string>> TryDelete(Guid id)
        {
            var entity = _context.WorkerLaborRegimes
                   .Include(x => x.WorkerLaborCategories)
                   .Include(x => x.WorkerLaborConditions)
                   .FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return new Tuple<bool, string>(false, "No se encontró el registro seleccionado.");

            if (entity.WorkerLaborCategories.Count > 0)
                return new Tuple<bool, string>(false, "Existen Categorias Laborales Relacionadas");

            if (entity.WorkerLaborConditions.Count > 0)
                return new Tuple<bool, string>(false, "Existen Condiciones Laborales Relacionadas");

            _context.WorkerLaborRegimes.Remove(entity);
            await _context.SaveChangesAsync();
            return new Tuple<bool, string>(true, "Registro eliminado satisfactoriamente");
        }

        public async Task<object> GetSelect2()
        {
            var result = await _context.WorkerLaborRegimes
                .Where(x => x.Status == ConstantHelpers.STATES.ACTIVE)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            return result;
        }
    }
}
