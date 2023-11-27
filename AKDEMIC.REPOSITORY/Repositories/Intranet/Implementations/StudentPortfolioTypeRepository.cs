using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class StudentPortfolioTypeRepository : Repository<StudentPortfolioType>, IStudentPortfolioTypeRepository
    {
        public StudentPortfolioTypeRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<StudentPortfolioType, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.StudentPortfolioTypes
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    dependencyId = x.DependencyId,
                    dependency = x.DependencyId.HasValue ? x.Dependency.Name : "---",
                    x.Type,
                    x.CanUploadStudent,
                    TypeName = ConstantHelpers.Intranet.STUDENT_PORTFOLIO_TYPE.NAMES[x.Type]
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<StudentPortfolioType>> GetStudentPortfolioTypes(byte? type, bool? canUploadStudent)
        {
            var query = _context.StudentPortfolioTypes.AsNoTracking();

            if (type.HasValue)
                query = query.Where(x => x.Type == type);

            if (canUploadStudent.HasValue && canUploadStudent.Value)
                query = query.Where(x => x.CanUploadStudent);

            return await query.ToListAsync();
        }
    }
}
