using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class AcademicAgreementRepository : Repository<AcademicAgreement>, IAcademicAgreementRepository
    {
        public AcademicAgreementRepository(AkdemicContext context) : base(context) { }

        /// <summary>
        /// Obtiene el reporte de 
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetAcademicAgreementChart()
        {
            var query = _context.AcademicAgreements.AsQueryable();
            var students = await _context.Students.CountAsync();
            var all = await query.CountAsync();
            var international = await query
                //.Where(x => x.AcademicAgreementTypes.Any(y => y.Type == 1))
                .CountAsync();


            var studentData = new List<dynamic>
            {
                new { name = "Internacional" , count = international},
                new { name = "Nacional", count = all - international}
            };

            var result = new
            {
                categories = studentData.Select(x => x.name).ToList(),
                data = studentData.Select(x => x.count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcademicAgreementDataTable(DataTablesStructs.SentParameters sentParameters, Guid? type, int statusId, string startDate, bool? onlyActive, string search, Guid? careerId = null, Guid? facultyId = null)
        {
            Expression<Func<AcademicAgreement, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ResolutionNumber); break;
                case "1":
                    orderByPredicate = ((x) => x.AcademicAgreementTypes.Select(y => y.AgreementType.Code).FirstOrDefault()); break;
                case "2":
                    orderByPredicate = ((x) => x.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.StartDate); break;
                default:
                    orderByPredicate = ((x) => x.ResolutionNumber); break;
            }

            var query = _context.AcademicAgreements
                .Include(x => x.AcademicAgreementTypes)
                .Where(x => !x.HasBeenRenovated)
                .AsQueryable();

            if (statusId > 0)
            {
                switch (statusId)
                {
                    case 1:
                        query = query.Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.UtcNow);
                        break;
                    case 2:
                        query = query.Where(x => x.StartDate > DateTime.UtcNow);
                        break;
                    case 3:
                        query = query.Where(x => x.EndDate.HasValue && x.EndDate.Value.Date > DateTime.UtcNow.Date);
                        break;
                    case 4:
                        query = query.Where(x => x.EndDate.HasValue && x.EndDate.Value.Date <= DateTime.UtcNow.Date);
                        break;
                }

            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (type.HasValue && type != Guid.Empty)
                query = query.Where(x => x.AcademicAgreementTypes.Any(y => y.AgreementTypeId == type));

            if (!string.IsNullOrEmpty(startDate))
            {
                var formatedDate = ConvertHelpers.DatepickerToUtcDateTime(startDate);
                query = query.Where(x => x.StartDate.Date == formatedDate.Date || x.EndDate == formatedDate);
            }

            if (onlyActive.HasValue)
                query = query.Where(x => x.Status != 4);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search) ||
                                x.Description.Trim().ToLower().Contains(search) ||
                                x.ResolutionNumber.Trim().ToLower().Contains(search)
                                );
            }

            var recordsFiltered = await query.CountAsync();
            var dataDb = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(s => new
                  {
                      id = s.Id,
                      resolutionNumber = s.ResolutionNumber,
                      types = s.AcademicAgreementTypes.Select(y => y.AgreementType.Code).ToList(),
                      name = s.Name,
                      startDate = s.StartDate.ToLocalDateFormat(),
                      endDateformatted = s.EndDate.ToLocalDateFormat(),
                      endDate = s.EndDate,
                      status = s.Status,
                      ending = s.IsEndingSoon,
                      Career = s.Career.Name,
                      Faculty = s.Faculty.Name
                  })
                  .ToListAsync();

            if (statusId == 3)
                dataDb = dataDb.Where(x => ((x.endDate - DateTime.UtcNow).Value.TotalDays < 15)).ToList();

            var data = dataDb
                .Select(x => new
                {
                    x.id,
                    x.resolutionNumber,
                    type = string.Join(", ", x.types),
                    x.name,
                    x.startDate,
                    x.status,
                    x.Career,
                    x.Faculty,
                    x.ending,
                    x.endDateformatted
                }).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetAcademicAgreementSelect2ClientSide(byte? type)
        {
            var query = _context.AcademicAgreements.AsQueryable();

            //if (type.HasValue)
            //    query = query.Where(x => x.Type == type);

            var result = await query
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<bool> IsAnotherWithResolutionNumber(string resolutionNumber, Guid? id)
        {
            var query = _context.AcademicAgreements.AsQueryable();
            if (id.HasValue)
                return await query.AnyAsync(x => x.ResolutionNumber.ToLower().Trim() == resolutionNumber.ToLower().Trim() && id.Value != x.Id && !x.HasBeenRenovated);
            return await query.AnyAsync(x => x.ResolutionNumber.ToLower().Trim() == resolutionNumber.ToLower().Trim() && !x.HasBeenRenovated);
        }

        public async Task<object> GetAcademicAgreementActiveAndInactiveChart()
        {
            var data = await _context.AcademicAgreements.Where(x => !x.HasBeenRenovated).GroupBy(x => x.Status)
                .Select(x => new
                {
                    name = x.Key,
                    count = x.Count()
                })
                .ToArrayAsync();

            //1 - Activo , 4 - Inactivo
            var result = data
                .Where(x => x.name == 1 || x.name == 4)
                .Select(x => new
                {
                    name = x.name == 1 ? "Activo" : "Inactivo",
                    y = x.count
                })
                .ToArray();

            return result;
        }
    }
}
