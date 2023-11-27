using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class DistrictRepository : Repository<District>, IDistrictRepository
    {
        public DistrictRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<Select2Structs.Result>> GetDistrictsSelect2ClientSide(Guid provinceId)
        {
            var result = await _context.Districts
                .Where(x => x.ProvinceId == provinceId)
                .OrderBy(x => x.Name)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetDistrictsJson(Guid pid, string q)
        {
            var result = _context.Districts
                 .Where(d => d.ProvinceId.Equals(pid))
                 .Select(d => new
                 {
                     id = d.Id,
                     text = d.Name
                 }).AsQueryable();

            if (!string.IsNullOrEmpty(q))
                result = result.Where(d => d.text.Contains(q));

            var model = await result.OrderBy(d => d.text).ToListAsync();

            return model;
        }

        public async Task<District> GetByNameAndProvinceId(Guid provinceId, string cell)
        {
            var district = await _context.Districts.Where(x => x.ProvinceId == provinceId && x.Name == cell).FirstOrDefaultAsync();

            return district;
        }

        public async Task<Guid> GetGuidByName(string name)
        {
            var result =await _context.Districts.FirstOrDefaultAsync(x => x.Name.Equals(name));

            return result.Id;
        }
        public async Task<object> GetDistricts()
        {
            var result = await _context.Districts
            .Select(x => new
            {
                id = x.Id,
                provinceId = x.ProvinceId,
                name = x.Name
            })
            .ToListAsync();

            return result;
        }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {
            return await _context.Districts.AnyAsync(x => x.Code.ToUpper() == code.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDistrictDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? countryId = null, Guid? departmentId = null, Guid? provinceId = null)
        {
            Expression<Func<District, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Province.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Province.Department.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Province.Department.Country.Name);
                    break;
            }

            var query = _context.Districts.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (countryId != null)
            {
                query = query.Where(x => x.Province.Department.CountryId == countryId);
            }

            if (departmentId != null)
            {
                query = query.Where(x => x.Province.DepartmentId == departmentId);
            }

            if (provinceId != null)
            {
                query = query.Where(x => x.ProvinceId == provinceId);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    DepartmentName = x.Province.Department.Name,
                    ProvinceName = x.Province.Name,
                    CountryName = x.Province.Department.Country.Name
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
