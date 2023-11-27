using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class ScholarshipRepository : Repository<Scholarship>, IScholarshipRepository
    {
        public ScholarshipRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, byte? program = null, string startDate = null, byte? status = null, string search = null)
        {
            Expression<Func<Scholarship, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Program);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.StartDate);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.EndDate);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.Scholarships.AsQueryable();

            if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                query = query.Where(x => x.Target == 1);
            else if (User.IsInRole(ConstantHelpers.ROLES.TEACHERS))
                query = query.Where(x => x.Target == 2);
            else if (!User.IsInRole(ConstantHelpers.ROLES.SUPERADMIN))
                query = query.Where(x => x.Target == 3);

            if (program.HasValue)
                query = query.Where(x => x.Program == program);

            if (!string.IsNullOrEmpty(startDate))
            {
                var datepicker = ConvertHelpers.DatepickerToUtcDateTime(startDate);
                query = query.Where(x => x.StartDate == datepicker);
            }

            if (status.HasValue)
            {
                switch (status)
                {
                    case 1:
                        query = query.Where(x => x.EndDate > DateTime.UtcNow);
                        break;
                    case 2:
                        query = query.Where(x => x.StartDate > DateTime.UtcNow);
                        break;
                    case 3:
                        query = query.Where(x => x.EndDate.Date > DateTime.UtcNow.Date);
                        break;
                    case 4:
                        query = query.Where(x => x.EndDate.Date <= DateTime.UtcNow.Date);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new Scholarship
                {
                    Id = x.Id,
                    Name = x.Name,
                    Program = x.Program,
                    StartDate = x.StartDate,
                    Target = x.Target,
                    CareerName = x.Career.Name,
                    FacultyName = x.Career.Faculty.Name,
                    EndDate = x.EndDate
                })
                .ToListAsync();

            if (status == 3)
                data = data.Where(x => ((x.EndDate - DateTime.UtcNow).TotalDays < 15)).ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetScholarshipSelect2ClientSide(Guid? selectedId = null)
        {
            var result = await _context.Scholarships
                .Select(x => new
                {
                    x.Id,
                    Text = x.Name,
                    Selected = x.Id == selectedId
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<bool> HasPostulants(Guid scholarshipId)
        {
            return await _context.QuestionnaireAnswerByUsers.AnyAsync(x => x.Postulation.Questionnaire.ScholarshipId == scholarshipId);
        }

        public async Task<bool> HasPostulations(Guid scholarshipId)
        {
            return await _context.Postulations.AnyAsync(y => y.Questionnaire.ScholarshipId == scholarshipId);
        }
    }
}
