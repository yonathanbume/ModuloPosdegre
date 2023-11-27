using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Implementations
{
    public class PresentationLetterRepository : Repository<PresentationLetter>, IPresentationLetterRepository
    {
        public PresentationLetterRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<string> GetCode()
        {
            var datetime = DateTime.UtcNow.ToDefaultTimeZone();
            var year = datetime.Year;
            var count = await _context.PresentationLetters.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).CountAsync() + 1;
            var result = $"{count.ToString("000000")}-{year}-UNAB";
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPresentationLettersDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string search)
        {
            var query = _context.PresentationLetters
                .OrderBy(x => x.CreatedAt)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToLower().Trim().Contains(search.ToLower().Trim()) || x.Student.User.UserName.ToLower().Trim().Contains(search.ToLower().Trim()));

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                {
                    query = query.Where(x => x.Student.UserId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
                {
                    query = query.Where(x => x.Student.Career.CareerDirectorId == userId);
                }
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    x.Workplace,
                    x.Position,
                    x.AddressedTo,
                    x.FinalDocumentUrl,
                    x.Days,
                    x.Code,
                    studentUserName = x.Student.User.UserName,
                    studentFullName = x.Student.User.FullName,
                    createdAt = x.CreatedAt.ToLocalDateTimeFormat()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
    }
}
