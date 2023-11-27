using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CampusRepository : Repository<Campus>, ICampusRepository
    {
        public CampusRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<Campus, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.District.Province.Department.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.District.Province.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.District.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Address;
                    break;
                case "6":
                    orderByPredicate = (x) => x.IsPrincipal;
                    break;
                case "7":
                    orderByPredicate = (x) => x.IsValid;
                    break;
                default:
                    orderByPredicate = (x) => x.Code;
                    break;
            }

            var query = _context.Campuses
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                //.Skip(sentParameters.PagingFirstRecord)
                //.Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.Address,
                    x.IsValid,
                    x.IsPrincipal,
                    District = x.District.Name,
                    Province = x.District.Province.Name,
                    Department = x.District.Province.Department.Name
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

        public async Task<object> GetCampus(Guid id)
        {
            return await _context.Campuses.Where(x => x.Id == id).Select(x => new
            {
                id = x.Id,
                code = x.Code,
                name = x.Name,
                inei = x.INEI,
                reniec = x.RENIEC,
                address = x.Address,
                isValid = x.IsValid,
                capacity = x.Capacity,
                comments = x.Comments,
                builtArea = x.BuiltArea,
                telephone = x.Telephone,
                reference = x.Reference,
                districtId = x.DistrictId,
                groundArea = x.GroundArea,
                isPrincipal = x.IsPrincipal,
                serviceType = x.ServiceType,
                provinceId = x.District.ProvinceId,
                otherServiceType = x.OtherServiceType,
                authorizationType = x.AuthorizationType,
                departmentId = x.District.Province.DepartmentId,
                startAuthorization = x.StartAuthorization.HasValue ? x.StartAuthorization.Value.ToString("dd/MM/yyyy") : null,
                endAuthorization = x.EndAuthorization.HasValue ? x.EndAuthorization.Value.ToString("dd/MM/yyyy") : null
            }).FirstAsync();
        }

        public async Task<object> GetAllAsSelect2ClientSide()
        {
            var campuses = await _context.Campuses.Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).ToListAsync();
            return campuses;
        }

        public async Task ClearPrincipal()
        {
            Campus campus = await _context.Campuses.FirstOrDefaultAsync(x => x.IsPrincipal);
            if (campus == null)
                return;
            campus.IsPrincipal = false;
            await _context.SaveChangesAsync();
        }

        public async Task<Select2Structs.ResponseParameters> GetAllSelect2(Select2Structs.RequestParameters requestParameters)
        {
            return await GetAllSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name,
            });
        }

        private async Task<Select2Structs.ResponseParameters> GetAllSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Campus, Select2Structs.Result>> selectPredicate = null)
        {
            IQueryable<Campus> query = _context.Campuses
                .AsNoTracking();

            int currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            List<Select2Structs.Result> results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        public async Task<object> GetCampusJson()
        {
            var result = await _context.Campuses.Select(c => new
            {
                id = c.Id,
                text = c.Name
            }).ToListAsync();
            return result;
        }

        public async Task<object> GetCampusCareerJson(Guid cid)
        {
            var result = await _context.CampusCareers.Where(cc => cc.CampusId == cid).Select(cc => new
            {
                id = cc.CareerId,
                text = cc.Career.Name
            }).ToListAsync();
            return result;
        }

        public async Task<Campus> GetFirstCampus()
        {
            return await _context.Campuses.FirstOrDefaultAsync();
        }

        public async Task<Campus> GetCampusPrincipal()
        {
            return await _context.Campuses.Where(x => x.IsPrincipal == true).FirstOrDefaultAsync();
        }

        public async Task<bool> AnyClassroom(Guid id)
            => await _context.Campuses.AnyAsync(x => x.Buildings.Any(y => y.Classrooms.Any()));
    }
}