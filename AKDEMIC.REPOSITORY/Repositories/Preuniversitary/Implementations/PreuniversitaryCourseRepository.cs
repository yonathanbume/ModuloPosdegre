using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryCourseRepository : Repository<PreuniversitaryCourse>, IPreuniversitaryCourseRepository
    {
        public PreuniversitaryCourseRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<PreuniversitaryCourse, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Name); break;
                default:
                    orderByPredicate = ((x) => x.Code); break;
            }

            var query = _context.PreuniversitaryCourses.Include(x => x.Career).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Code.Contains(searchValue) || x.Name.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      name = x.Name,
                      code = x.Code,
                      career = x.Career.Name
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

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _context.PreuniversitaryCourses.AnyAsync(x => x.Code == code && x.Id != ignoredId);

        public async Task<List<PreuniversitaryCourse>> GetAllByCareerId(Guid careerId)
            => await _context.PreuniversitaryCourses.Where(x => x.CareerId == careerId).ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesToScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchvalue = null)
        {
            Expression<Func<PreuniversitaryCourse, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.Name); break;
                default:
                    orderByPredicate = ((x) => x.Code); break;
            }

            var preuniversitaryTerm = await _context.PreuniversitaryTerms.FindAsync(preuniversitaryTermId);
            var query = _context.PreuniversitaryCourses.AsQueryable();

            if (!string.IsNullOrEmpty(searchvalue))
                query = query.Where(x => x.Code.Contains(searchvalue) ||
                                            x.Name.Contains(searchvalue));

            var queryclient = await query.ToListAsync();
            int recordsFiltered = queryclient.Count();

            var data = queryclient
                //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    name = x.Name,
                    code = x.Code,
                    editable = !preuniversitaryTerm.IsFinished
                })
                .ToList();


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
