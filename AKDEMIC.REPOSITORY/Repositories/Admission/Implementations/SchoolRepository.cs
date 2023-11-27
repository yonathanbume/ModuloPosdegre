using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class SchoolRepository : Repository<School>, ISchoolRepository
    {
        public SchoolRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, byte? type = null, string search = null)
        {
            Expression<Func<School, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.ModularCode;
                    break;
                case "1":
                    orderByPredicate = (x) => x.LocalCode;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.IsSchooled;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Type;
                    break;
                default:
                    break;
            }

            var query = _context.Schools.AsNoTracking();

            if (type.HasValue && type != 0)
                query = query.Where(x => x.Type == type);

            if (!string.IsNullOrEmpty(search))
            {
                var normalizedSearch = search.ToUpper();
                query = query.Where(x => x.Name.ToUpper().Contains(normalizedSearch));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    modularCode = x.ModularCode,
                    localCode = x.LocalCode,
                    name = x.Name,
                    schooled = x.IsSchooled ? "Escolarizado" : "No escolarizado",
                    //isSchooled = x.IsSchooled,
                    //type = x.Type,
                    typeName = ConstantHelpers.Admission.School.Type.VALUES[x.Type],
                    //address = x.Address,
                    //departmentId = x.DepartmentId,
                    //provinceId = x.ProvinceId,
                    //districtId = x.DistrictId,
                    //ubigeoCode = x.UbigeoCode
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<object> GetSelect2ClientSide()
        {
            var result = await _context.Schools
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            return result;
        }

        public async Task<object> GetSelect2ServerSide(string term, int? type = null, Guid? departmentId = null, Guid? provinceId = null)
        {
            var query = _context.Schools.AsNoTracking();

            if (type.HasValue)
            {
                switch (type.Value)
                {
                    case ConstantHelpers.SECONDARY_EDUCATION_TYPE.PUBLIC:
                        query = query.Where(x => x.Type == ConstantHelpers.Admission.School.Type.DIRECT_MANAGEMENT_PUBLIC_SCHOOL
                        || x.Type == ConstantHelpers.Admission.School.Type.PRIVATE_MANAGEMENT_PUBLIC_SCHOOL);
                        break;
                    case ConstantHelpers.SECONDARY_EDUCATION_TYPE.PRIVATE:
                    case ConstantHelpers.SECONDARY_EDUCATION_TYPE.PAROCHIAL:
                    case ConstantHelpers.SECONDARY_EDUCATION_TYPE.OTHER:
                        query = query.Where(x => x.Type == ConstantHelpers.Admission.School.Type.PRIVATE_SCHOOL);
                        break;
                    default:
                        query = query.Where(x => x.Type == type.Value);
                        break;
                }
            }

            if (departmentId.HasValue && departmentId != Guid.Empty)
                query = query.Where(x => x.DepartmentId == departmentId.Value);

            if (provinceId.HasValue && provinceId != Guid.Empty)
                query = query.Where(x => x.ProvinceId == provinceId.Value);

            var schools = await query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.ModularCode,
                    District = x.District.Name
                }, term).ToListAsync();

            var data = schools
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.ModularCode} - {x.Name} - {x.District}"
                })
               .ToList();

            return data;
        }
    }
}
