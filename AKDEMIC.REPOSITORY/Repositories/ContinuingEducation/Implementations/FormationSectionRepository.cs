using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Templates.FormationSection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Implementations
{
    public class FormationSectionRepository:Repository<ENTITIES.Models.ContinuingEducation.Section> , IFormationSectionRepository
    {
        public FormationSectionRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {
            return await _context.FormationSections.AnyAsync(x => x.Code.ToUpper() == code.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllFormationSectionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? courseId = null, string searchValue = null)
        {
            Expression<Func<ENTITIES.Models.ContinuingEducation.Section, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Course.Name;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Vacancies);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.InscriptionStart);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.InscriptionEnd);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.ClassStart);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.ClassEnd);
                    break;
            }

            var query = _context.FormationSections.AsNoTracking();

            if (courseId != null)
            {
                query = query.Where(x => x.CourseId == courseId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    CourseName = x.Course.Name,
                    x.Vacancies,
                    InscriptionStart = x.InscriptionStart.ToLocalDateFormat(),
                    InscriptionEnd = x.InscriptionEnd.ToLocalDateFormat(),
                    ClassStart = x.ClassStart.ToLocalDateFormat(),
                    ClassEnd = x.ClassEnd.ToLocalDateFormat(),
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

        public async Task<List<FormationSectionTemplate>> GetAllSectionTemplateData(int skip = 0, int take = 0, Guid? courseTypeId = null, string searchValue = null)
        {

            var query = _context.FormationSections.AsNoTracking();

            if (courseTypeId != null)
                query = query.Where(x => x.Course.CourseTypeId == courseTypeId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                var trimSearch = searchValue.Trim();
                query = query.Where(x => x.Course.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            var result = await query
                .OrderBy(x => x.Course.Name)
                .Select(x => new FormationSectionTemplate
                {
                    Id = x.Id,
                    Code = x.Code,
                    CourseId = x.CourseId,
                    Vacancies = x.Vacancies,
                    CourseName = x.Course.Name,
                    CourseDescription = x.Course.Description,
                    CourseDuration = x.Course.Duration,
                    CoursePlace = x.Course.Place,
                    CourseModality = x.Course.Modality,
                    CourseImage = x.Course.ImageUrl,
                    CourseRequirements = x.Course.Requirement,
                    CourseArea = x.Course.CourseArea.Name,
                    CourseType = x.Course.CourseType.Name,
                    CourseInvestment = x.Course.Investment,
                    InscriptionStart = x.InscriptionStart.ToLocalDateFormat(),
                    InscriptionEnd = x.InscriptionEnd.ToLocalDateFormat(),
                    ClassStart = x.ClassStart.ToLocalDateFormat(),
                    ClassEnd = x.ClassEnd.ToLocalDateFormat() 
                })
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return result;
        }

        public async Task<FormationSectionTemplate> GetSectionTemplateData(Guid id)
        {

            var result = await _context.FormationSections
                .Where(x => x.Id == id)
                .Select(x => new FormationSectionTemplate
                {
                    Id = x.Id,
                    Code = x.Code,
                    CourseId = x.CourseId,
                    Vacancies = x.Vacancies,
                    CourseName = x.Course.Name,
                    CourseDescription = x.Course.Description,
                    CourseDuration = x.Course.Duration,
                    CoursePlace = x.Course.Place,
                    CourseModality = x.Course.Modality,
                    CourseImage = x.Course.ImageUrl,
                    CourseRequirements = x.Course.Requirement,
                    CourseArea = x.Course.CourseArea.Name,
                    CourseType = x.Course.CourseType.Name,
                    CourseInvestment = x.Course.Investment,
                    InscriptionStart = x.InscriptionStart.ToLocalDateFormat(),
                    InscriptionEnd = x.InscriptionEnd.ToLocalDateFormat(),
                    ClassStart = x.ClassStart.ToLocalDateFormat(),
                    ClassEnd = x.ClassEnd.ToLocalDateFormat()
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
