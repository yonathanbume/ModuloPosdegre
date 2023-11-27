using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AkdemicContext context) : base(context) { }

        public async Task<Tuple<int, List<Tuple<string, int>>>> GetDepartmentsQuantityReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            var baseQuery = _context.Departments
                .Where(x => string.IsNullOrWhiteSpace(paginationParameter.SearchValue) || x.Name.Contains(paginationParameter.SearchValue))
                .AsQueryable();

            var records = await baseQuery.CountAsync();

            var query = baseQuery.Select(x => new
            {
                name = x.Name,
                quantity = _context.WorkerLaborInformation.Count(y => y.ResidenceDepartmentId == x.Id)
            }).AsQueryable();

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

        public async Task<List<Tuple<string, int>>> GetDepartmentsTypeQuantityReport(string search)
        {
            var data = await _context.Departments
                .Where(x => string.IsNullOrWhiteSpace(search) || x.Name.Contains(search))
                .Select(x => new
                {
                    name = x.Name,
                    quantity = _context.WorkerLaborInformation.Count(y => y.ResidenceDepartmentId == x.Id)
                })
                .AsQueryable()
                .ToListAsync();

            var result = data.Select(x => new Tuple<string, int>(x.name, x.quantity)).ToList();

            return result;
        }

        public async Task<object> GetDepartmentsByCountryCode(string countryCode, string search)
        {
            var query = _context.Departments
                .OrderBy(x => x.CountryId)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    x.Id,
                    CountryCode = (x.Country != null) ? x.Country.Code : null,
                    Text = x.Name
                }).AsQueryable();

            if (!string.IsNullOrEmpty(countryCode))
                query = query.Where(x => x.CountryCode.Equals(countryCode));

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Text.Contains(search));

            var result = await query.ToArrayAsync();
            return result;
        }

        public async Task<object> GetDepartmenstJson(string q, string countryCode)
        {
            var result = _context.Departments
                .Select(d => new
                {
                    id = d.Id,
                    countryCode = (d.Country != null) ? d.Country.Code : null,
                    text = d.Name
                }).AsQueryable();

            if (!string.IsNullOrEmpty(countryCode))
                result = result.Where(d => d.countryCode.Equals(countryCode));

            if (!string.IsNullOrEmpty(q))
                result = result.Where(d => d.text.Contains(q));

            var model = await result.OrderBy(d => d.text).ToListAsync();

            return model;
        }

        public async Task<Department> GetByName(string cell)
        {
            var department = await _context.Departments.Where(x => x.Name == cell).FirstOrDefaultAsync();

            return department;
        }

        public async Task<Guid> GetGuiByName(string name)
        {
            var first  =await _context.Departments.FirstOrDefaultAsync(x => x.Name.Equals(name));

            return first.Id;
        }
        public async Task<object> GetDepartments()
        {
            var result = await _context.Departments
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<object> GetDepartmentSelect2ClientSide()
        {
            var result = await _context.Departments
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {
            return await _context.Departments.AnyAsync(x => x.Code.ToUpper() == code.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDepartmentDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? countryId = null)
        {
            Expression<Func<Department, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Country.Name);
                    break;
            }

            var query = _context.Departments.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (countryId != null)
            {
                query = query.Where(x => x.CountryId == countryId);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    CountryName = x.Country.Name
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
    }
}
