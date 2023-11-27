using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class JobOfferCareerRepository : Repository<JobOfferCareer>, IJobOfferCareerRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public JobOfferCareerRepository(AkdemicContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<JobOfferCareer>> GetAllByJobOfferId(Guid jobOfferId)
        {
            var query = _context.JobOfferCareers
                .Where(x => x.JobOfferId == jobOfferId);

            return await query.ToListAsync();
        }

        public async Task<object> JobOfferReport1(bool isCoordinator, List<Guid> careers)
        {
            var total = new List<string>();

            var query = _context.JobOfferCareers.Include(x => x.Career.Faculty)
                      .Include(x => x.JobOffer)
                      .OrderBy(x => x.Career.Name)
                      .AsQueryable();

            if (careers.Count > 0 && (!careers.Contains(Guid.Empty)))
            {
                total = await query
                         .Where(x => careers.Contains(x.CareerId))
                         .Select(x => x.Career.Faculty.Name)
                         .ToListAsync();
            }
            else
            {
                total = await query.Select(x => x.Career.Faculty.Name)
                      .ToListAsync();
            }





            List<string> categories = new List<string>();
            List<int> data = new List<int>();

            for (int i = 0; i < total.Count; i++)
            {
                if (categories.FirstOrDefault(x => x == total[i]) == null)
                {
                    categories.Add(total[i]);
                    data.Add(1);
                }
                else
                {
                    int index = categories.IndexOf(total[i]);
                    data[index]++;
                }
            }

            return new { categories, data };
        }

        public async Task<object> JobOfferReport2(bool isCoordinator, List<Guid> careers)
        {
            var query = _context.JobOffers.Include(x => x.JobOfferCareers)
                .Include(x => x.JobOfferApplications)
                .Include(x => x.Company.User).AsQueryable();


            if (careers.Count > 0 && (!careers.Contains(Guid.Empty)))
            {
                query = query
                       .Where(x => x.JobOfferCareers
                       .Any(y => careers.Contains(y.CareerId)))
                       .AsQueryable();
            }

            var queryList = await query.Select(x => new
            {
                companyName = x.Company.User.Name,
                x.Position,
                x.JobOfferApplications,
                careers = x.JobOfferCareers.Select(y => y.CareerId).ToList()
            }).ToListAsync();

            if (isCoordinator)
            {
                queryList = queryList
                    .Where(x => x.careers.Any(y => careers.Contains(y)))
                    .Select(x => new
                    {
                        companyName = x.companyName,
                        x.Position,
                        x.JobOfferApplications,
                        careers = new List<Guid>()
                    })
                    .ToList();
            }



            var data = queryList
                .Select(x => new
                {
                    name = $"{x.companyName}: {x.Position}",
                    y = x.JobOfferApplications.Count()
                }).OrderBy(x => x.name).ToList();

            return new { data };
        }

        public async Task<object> GetJobOfferByCareersAsData(Guid? careerId = null, string startSearchDate = null, string endSearchDate = null , ClaimsPrincipal user = null)
        {
            var query = _context.JobOfferCareers.AsQueryable();
            var careersQuery = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            var group = await query
                .GroupBy(x => new { x.CareerId })
                .Select(x => new
                {
                    x.Key.CareerId,
                    Count = x.Count()
                }).ToListAsync();

            var careers = await careersQuery
                .Select(x => new
                {
                    x.Name,
                    x.Id
                }).ToListAsync();

            var data = careers
                .Select(x => new
                {
                    x.Name,
                    Count = group.Where(y => y.CareerId == x.Id).Select(y => y.Count).FirstOrDefault()
                }).ToList();

            var result = new
            {
                categories = data.Select(x => x.Name).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task JobOfferReport2Excel(IXLWorksheet worksheet, ClaimsPrincipal user, List<Guid> careers)
        {
            var query = _context.JobOffers.Include(x => x.JobOfferCareers)
               .Include(x => x.JobOfferApplications)
               .Include(x => x.Company.User).AsQueryable();

            var userId = _userManager.GetUserId(user);

            //var userApplication = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            var coordinatorCareers = await _context.CoordinatorCareers
               .Include(x => x.Career)
               .Where(x => x.UserId == userId).Select(x => x.CareerId).ToListAsync();

            if (coordinatorCareers.Count > 0)
            {
                if (careers.Contains(Guid.Empty))
                {
                    query = query
                       .Where(x => x.JobOfferCareers
                       .Any(y => coordinatorCareers.Contains(y.CareerId)))
                       .AsQueryable();
                }
                if (careers.Count > 0)
                {
                    query = query
                    .Where(x => x.JobOfferCareers
                    .Any(y => careers.Contains(y.CareerId)))
                    .AsQueryable();
                }
            }

            const int position = 1;
            var column = 0;

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "EMPRESA", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "OFERTA LABORAL", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "N° DE POSTULACIONES", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "FECHA INICIO DE PUBLICACIÓN DE LA OFERTA LABORAL", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "FECHA FIN DE PUBLICACIÓN DE LA OFERTA LABORAL", column);



            worksheet.SheetView.FreezeRows(position);


            var row = 2;

            var queryList = await query.ToListAsync();

            foreach (var item in queryList)
            {
                SetInformationStyle(worksheet, row, 1, item.Company.User.Name ?? "--");
                SetInformationStyle(worksheet, row, 2, item.Position ?? "--");
                SetInformationStyle(worksheet, row, 3, item.JobOfferApplications.Count().ToString());
                SetInformationStyle(worksheet, row, 4, item.StartDate.ToLocalDateFormat() ?? "--");
                SetInformationStyle(worksheet, row, 5, item.EndDate.ToLocalDateFormat() ?? "--");

                row++;
            }


        }

        protected static void SetHeaderRowStyle2(IXLWorksheet worksheet, string headerName, int column)
        {
            const int position = 1;
            var fillColor = XLColor.FromArgb(0x0c618c);
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);
            var fontColor = XLColor.White;
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            const XLAlignmentHorizontalValues alignmentHorizontal = XLAlignmentHorizontalValues.Left;

            worksheet.Column(column).Style.Alignment.Horizontal = alignmentHorizontal;
            worksheet.Cell(position, column).Value = headerName;
            worksheet.Cell(position, column).Style.Font.Bold = true;
            worksheet.Cell(position, column).Style.Font.FontColor = fontColor;
            worksheet.Cell(position, column).Style.Fill.BackgroundColor = fillColor;
            worksheet.Cell(position, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(position, column).Style.Border.OutsideBorderColor = outsideBorderColor;
        }

        protected static void SetInformationStyle(IXLWorksheet worksheet, int row, int column, string data, bool requireDateFormat = false)
        {
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);

            if (requireDateFormat)
            {
                worksheet.Cell(row, column).Style.DateFormat.Format = "dd/MM/yyyy";
            }

            worksheet.Cell(row, column).Value = data;
            worksheet.Cell(row, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(row, column).Style.Border.OutsideBorderColor = outsideBorderColor;
        }

        public async Task<List<JobOfferApplicationExcelReportTemplate>> JobOfferReport2ExcelData(ClaimsPrincipal user, List<Guid> careers)
        {
            var query = _context.JobOffers.Include(x => x.JobOfferCareers).AsQueryable();

            var userId = _userManager.GetUserId(user);


            var coordinatorCareers = await _context.CoordinatorCareers
               .Where(x => x.UserId == userId)
               .Select(x => x.CareerId)
               .ToListAsync();

            if (coordinatorCareers.Count > 0)
            {
                if (careers.Contains(Guid.Empty))
                {
                    query = query
                       .Where(x => x.JobOfferCareers
                       .Any(y => coordinatorCareers.Contains(y.CareerId)))
                       .AsQueryable();
                }
                if (careers.Count > 0)
                {
                    query = query
                    .Where(x => x.JobOfferCareers
                    .Any(y => careers.Contains(y.CareerId)))
                    .AsQueryable();
                }
            }

            var result = await query
                .Select(x => new JobOfferApplicationExcelReportTemplate
                {
                    Company = x.Company.User.Name ?? "--",
                    Position = x.Position ?? "--",
                    ApplicationsCount = x.JobOfferApplications.DefaultIfEmpty().Count(),
                    JobOfferStartDate = x.StartDate.ToLocalDateFormat() ?? "--",
                    JobOfferEndDate = x.EndDate.ToLocalDateFormat() ?? "--",
                })
                .ToListAsync();

            return result;
        }
    }
}
