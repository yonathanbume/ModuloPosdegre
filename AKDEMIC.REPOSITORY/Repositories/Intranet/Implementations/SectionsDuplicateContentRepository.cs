using AKDEMIC.CORE.Extensions;
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
using System.Threading;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class SectionsDuplicateContentRepository : Repository<SectionsDuplicateContent>, ISectionsDuplicateContentRepository
    {
        public SectionsDuplicateContentRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsDuplicateContentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId)
        {
            Expression<Func<SectionsDuplicateContent, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.SectionA.CourseTerm.Course.Career.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.SectionA.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().AcademicYear;
                    break;
                case "3":
                    orderByPredicate = (x) => x.SectionA.CourseTerm.Course.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.SectionA.Code;
                    break;
                case "5":
                    orderByPredicate = (x) => x.SectionB.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().AcademicYear;
                    break;
                case "6":
                    orderByPredicate = (x) => x.SectionB.CourseTerm.Course.Name;
                    break;
                case "7":
                    orderByPredicate = (x) => x.SectionB.Code;
                    break;
                default:
                    orderByPredicate = (x) => x.SectionA.CourseTerm.Course.Career.Name;
                    break;
            }


            var query = _context.SectionsDuplicateContents
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (termId.HasValue)
                query = query.Where(q => q.SectionA.CourseTerm.TermId == termId);
            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(q => q.SectionA.CourseTerm.Course.CareerId == careerId);

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    sectionAId = x.SectionAId,
                    sectionBId = x.SectionBId,
                    career = x.SectionA.CourseTerm.Course.Career.Name,
                    academicYearA = x.SectionA.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().Curriculum.Name,
                    courseA = x.SectionA.CourseTerm.Course.Name,
                    sectionA = x.SectionA.Code,
                    academicYearB = x.SectionB.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().Curriculum.Name,
                    courseB = x.SectionB.CourseTerm.Course.Name,
                    sectionB = x.SectionB.Code
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<SectionsDuplicateContent> GetBySectionAandB(Guid sectionAid, Guid sectionBid)
            => await _context.SectionsDuplicateContents.Where(x => x.SectionAId == sectionAid && x.SectionBId == sectionBid).FirstOrDefaultAsync();
        public async Task<bool> AnySectionASectionB(Guid sectionAid, Guid sectionBid)
            => await _context.SectionsDuplicateContents.AnyAsync(x => x.SectionAId == sectionAid && x.SectionBId == sectionBid);
    }
}
