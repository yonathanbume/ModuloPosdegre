using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class DigitalResourceRepository : Repository<DigitalResource>, IDigitalResourceRepository
    {
        public DigitalResourceRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<DigitalResource>> GetDigitalResourceDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, string searchValue)
        {
            Expression<Func<DigitalResource, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Tipo); break;
                case "1":
                    orderByPredicate = ((x) => x.Tipo); break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                case "3":
                    orderByPredicate = ((x) => x.Sorter); break;
                //default:
                //    orderByPredicate = ((x) => x.Career.Name); break;
            }

            var query = _context.DigitalResources.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
                {
                    query = query.Where(x => x.DigitalResourceCareers.Any(y=>y.Career.CareerDirectorId == userId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN)||user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.DigitalResourceCareers.Any(y => y.Career.Faculty.DeanId == userId || y.Career.Faculty.SecretaryId == userId));
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => 
                x.Title.ToLower().Contains(searchValue) || 
                x.Sorter.ToLower().Contains(searchValue) || 
                x.Tipo.ToLower().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new DigitalResource
                {
                    Id = x.Id,
                    Sorter = x.Sorter,
                    Tipo = x.Tipo,
                    Title = x.Title,
                    Careers = x.DigitalResourceCareers.Any() ? string.Join(", ", x.DigitalResourceCareers.Select(y=>y.Career.Name).ToList()) : "TODAS",
                    FileUrl = x.FileUrl,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<DigitalResource>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<DigitalResource>> GetDigitalResourceToTeacherDatatable(DataTablesStructs.SentParameters parameters, string teacherId ,string searchValue)
        {
            Expression<Func<DigitalResource, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Tipo); break;
                case "1":
                    orderByPredicate = ((x) => x.Tipo); break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                case "3":
                    orderByPredicate = ((x) => x.Sorter); break;
                //default:
                    //orderByPredicate = ((x) => x.Career.Name); break;
            }

            var careers = await _context.Teachers.Where(x => x.UserId == teacherId).Select(x => x.AcademicDepartment.Faculty.Careers).FirstOrDefaultAsync();
            var careerId = careers.Select(x => x.Id).ToHashSet();

            var query = _context.DigitalResources.Where(x => x.DigitalResourceCareers.Any(y=> careerId.Contains(y.CareerId)) || !x.DigitalResourceCareers.Any()).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x =>
                x.Title.ToLower().Contains(searchValue) ||
                x.Sorter.ToLower().Contains(searchValue) ||
                x.Tipo.ToLower().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new DigitalResource
                {
                    Id = x.Id,
                    Sorter = x.Sorter,
                    Tipo = x.Tipo,
                    Title = x.Title,
                    Careers = x.DigitalResourceCareers.Any() ? string.Join(", ", x.DigitalResourceCareers.Select(y => y.Career.Name).ToList()) : "TODAS",
                    FileUrl = x.FileUrl,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<DigitalResource>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<DigitalResourceCareer>> GetDigitalResourceCareers(Guid digitalResourceId)
            => await _context.DigitalResourceCareers.Where(x => x.DigitalResourceId == digitalResourceId).Include(x => x.Career).ToListAsync();

        public async Task DeleteDigitalResourceCareer(Guid digitalResourceId)
        {
            var entities = await _context.DigitalResourceCareers.Where(x => x.DigitalResourceId == digitalResourceId).ToListAsync();
            _context.DigitalResourceCareers.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}