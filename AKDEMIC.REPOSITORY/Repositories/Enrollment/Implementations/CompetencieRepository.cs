using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CompetencieRepository : Repository<Competencie> , ICompetencieRepository
    {
        public CompetencieRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCompetenciesDatatable(DataTablesStructs.SentParameters parameters, string searchvalue)
        {
            Expression<Func<Competencie, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Type;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Type;
                    break;
            }

            var query = _context.Competencies.AsNoTracking();

            if (!string.IsNullOrEmpty(searchvalue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchvalue.Trim().ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    type = ConstantHelpers.COMPETENCIE.VALUES[x.Type]
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _context.Competencies.AnyAsync(x => x.Name.ToLower() == name.ToLower().Trim() && x.Id != ignoredId);

        public async Task<Select2Structs.ResponseParameters> GetCompetenciesSelect2(Select2Structs.RequestParameters parameters, byte? type, string searchValue, Guid? curriculumId)
        {
            var query = _context.Competencies.AsNoTracking();

            if (curriculumId.HasValue)
                query = query.Where(x => x.CurriculumCompetencies.Any(y => y.CurriculumId == curriculumId));

            if (type.HasValue)
                query = query.Where(x => x.Type == type);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(searchValue.Trim().ToLower()));

            var currentPage = parameters.CurrentPage != 0 ? parameters.CurrentPage - 1 : 0;

            var results = await query.Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE).Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
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
    }
}
