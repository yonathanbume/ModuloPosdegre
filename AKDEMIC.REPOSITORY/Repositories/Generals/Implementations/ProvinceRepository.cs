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
    public class ProvinceRepository : Repository<Province>, IProvinceRepository
    {
        public ProvinceRepository(AkdemicContext context) : base(context) { }

        public async Task<Tuple<int, List<Tuple<string, string, int>>>> GetProvincesQuantityReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            var baseQuery = _context.Provinces
                .Where(x => string.IsNullOrWhiteSpace(paginationParameter.SearchValue) || x.Name.Contains(paginationParameter.SearchValue) || 
                            x.Department.Name.Contains(paginationParameter.SearchValue))
                .AsQueryable();

            var records = await baseQuery.CountAsync();

            var query = baseQuery.Select(x => new
            {
                department = x.Department.Name,
                province = x.Name,
                quantity = _context.WorkerLaborInformation.Count(y => y.ResidenceProvinceId == x.Id)
            }).AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.department) : query.OrderBy(q => q.department);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.province) : query.OrderBy(q => q.province);
                    break;
                case "2":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.quantity) : query.OrderBy(q => q.quantity);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.department) : query.OrderBy(q => q.department);
                    break;
            }

            var data = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage).ToListAsync();

            var result = data.Select(x => new Tuple<string, string, int>(x.department, x.province, x.quantity)).ToList();

            return new Tuple<int, List<Tuple<string, string, int>>>(records, result);
        }

        public async Task<List<Tuple<string, string, int>>> GetProvincesTypeQuantityReport(string search)
        {
            var data = await _context.Provinces
                .Where(x => string.IsNullOrWhiteSpace(search) || x.Name.Contains(search) || x.Department.Name.Contains(search))
                .Select(x => new
                {
                    department = x.Department.Name,
                    province = x.Name,
                    quantity = _context.WorkerLaborInformation.Count(y => y.ResidenceProvinceId == x.Id)
                })
                .AsQueryable()
                .ToListAsync();

            var result = data.Select(x => new Tuple<string, string, int>(x.department, x.province, x.quantity)).ToList();

            return result;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetProvincesSelect2ClientSide(Guid deparmentId)
        {
            var result = await _context.Provinces
                .Where(x => x.DepartmentId == deparmentId)
                .OrderBy(x => x.Name)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetProvincesJson(Guid did, string q)
        {
            var result = _context.Provinces
                .Where(p => p.DepartmentId.Equals(did))
                .Select(p => new
                {
                    id = p.Id,
                    text = p.Name
                }).AsQueryable();

            if (!string.IsNullOrEmpty(q))
                result = result.Where(p => p.text.Contains(q));

            var model = await result.OrderBy(p => p.text).ToListAsync();

            return model;
        }

        public async Task<Province> GetByNameAndDepartmentId(Guid departmentId, string cell)
        {
            var province = await _context.Provinces.Where(x => x.DepartmentId == departmentId && x.Name == cell).FirstOrDefaultAsync();

            return province;
        }

        public async Task<Guid> GetGuidByName(string name)
        {
            var result = await _context.Provinces.FirstOrDefaultAsync(x => x.Name.Equals(name));

            return result.Id;
        }
        public async Task<object> GetProvinces()
        {
            var result = await _context.Provinces
                .Select(x => new
                {
                    id = x.Id,
                    departmentId = x.DepartmentId,
                    name = x.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {
            return await _context.Provinces.AnyAsync(x => x.Code.ToUpper() == code.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllProvinceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? countryId = null, Guid? departmentId = null)
        {
            Expression<Func<Province, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Department.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Department.Country.Name);
                    break;
            }

            var query = _context.Provinces.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (countryId != null)
            {
                query = query.Where(x => x.Department.CountryId == countryId);
            }

            if (departmentId != null)
            {
                query = query.Where(x => x.DepartmentId == departmentId);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    DepartmentName = x.Department.Name,
                    CountryName = x.Department.Country.Name
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

        public async Task<object> GetAllAsSelect2ClientSide(Guid? departmentId = null)
        {
            var query = _context.Provinces.AsNoTracking();

            if (departmentId != null)
                query = query.Where(x => x.DepartmentId == departmentId);

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return result;
        }
    }
}
