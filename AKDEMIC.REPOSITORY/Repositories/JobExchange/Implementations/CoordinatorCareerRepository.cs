using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class CoordinatorCareerRepository : Repository<CoordinatorCareer>, ICoordinatorCareerRepository
    {
        public CoordinatorCareerRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE
        protected static void CreateHeaderRow(IXLWorksheet worksheet)
        {
            const int position = 1;
            var column = 0;

            worksheet.Column(++column).Width = 40;
            SetHeaderRowStyle(worksheet, "CÓDIGO", column);

            worksheet.Column(++column).Width = 70;
            SetHeaderRowStyle(worksheet, "NOMBRES COMPLETOS", column);

            worksheet.Column(++column).Width = 70;
            SetHeaderRowStyle(worksheet, "ESCUELA PROFESIONAL", column);

            worksheet.Column(++column).Width = 40;
            SetHeaderRowStyle(worksheet, "TELÉFONO", column);

            worksheet.Column(++column).Width = 70;
            SetHeaderRowStyle(worksheet, "CONDICIÓN DEL TRABAJADOR", column);



            worksheet.SheetView.FreezeRows(position);
        }
        protected static void SetHeaderRowStyle(IXLWorksheet worksheet, string headerName, int column)
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
        private async Task LoadRegistryPatternInformation(IXLWorksheet worksheet)
        {

            var row = 2;
            const int CODE = 1;    //CODUNIV
            const int FULLNAME = 2;    //RAZ_SOC 
            const int CAREER = 3;  //FAC_NOM
            const int PHONENUMBER = 4;    //CARR_PROG
            const int WORKER_CONDITION = 5;  //PROGRAMA_ACADEMICO --
            
            var listAcademicCoordinators = await _context.CoordinatorCareers
                .Select(x => new
                {
                    userName = x.User.UserName,
                    fullName = x.User.FullName,
                    careerName = x.Career.Name,
                    phoneNumber = x.User.PhoneNumber,
                    categoryLaborInformation = x.User.WorkerLaborInformation.WorkerLaborCategory.Name,
                    conditionLaborInformation = x.User.WorkerLaborInformation.WorkerLaborCondition.Name
                })
                .ToListAsync();
            if (listAcademicCoordinators != null && listAcademicCoordinators.Count >0)
            {
                for (int i = 0; i < listAcademicCoordinators.Count; i++)
                {
                    SetInformationStyle(worksheet, row, CODE, listAcademicCoordinators[i].userName);
                    SetInformationStyle(worksheet, row, FULLNAME, listAcademicCoordinators[i].fullName);
                    SetInformationStyle(worksheet, row, CAREER, listAcademicCoordinators[i].careerName);
                    SetInformationStyle(worksheet, row, PHONENUMBER, listAcademicCoordinators[i].phoneNumber);                  
                    SetInformationStyle(worksheet, row, WORKER_CONDITION, listAcademicCoordinators[i].categoryLaborInformation + " - " + listAcademicCoordinators[i].conditionLaborInformation);                   
                    row++;
                }
            }
            else
            {
                throw new Exception("No se pudo procesar la generación de excel");
            }
        }

        #endregion

        #region PUBLIC

        public async Task<List<CoordinatorCareer>> GetCoordinatorCareer(string userId)
        {
            var query = _context.CoordinatorCareers
                .Include(x => x.Career)
                .Where(x => x.UserId == userId);

            return await query.ToListAsync();
        }

        public async Task<bool> AnyByUserId(string userId)
        {
            return await _context.CoordinatorCareers.AnyAsync(x => x.UserId == userId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoordinatorsCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, string searchValue = null)
        {
            var query = _context.CoordinatorCareers.AsNoTracking();

            if (careerId.HasValue)
            {
                query = query.Where(x => x.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                string search = searchValue.Trim().ToUpper();
                query = query.Where(x => x.User.FullName.ToUpper().Contains(search) 
                        || x.Career.Name.ToUpper().Contains(search) 
                        || x.User.PaternalSurname.ToUpper().Contains(search)
                        || x.User.MaternalSurname.ToUpper().Contains(search)
                        || x.User.Name.ToUpper().Contains(search)
                        || x.User.UserName.ToUpper().Contains(search));
            }

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    Career = x.Career.Name,
                    x.User.FullName,
                    x.User.UserName,
                    x.User.PhoneNumber,
                    workerCondition = x.User.WorkerLaborInformation.WorkerLaborCategory.Name ?? "" + " - "+ x.User.WorkerLaborInformation.WorkerLaborCondition.Name ?? "",                                        
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        //Override base repos
        public override async Task Insert(CoordinatorCareer coordinatorCareer)
        {
            var rolCoordinator = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR).FirstOrDefaultAsync();

            await _context.CoordinatorCareers.AddAsync(coordinatorCareer);


            if (!await _context.UserRoles.Where(x=>x.UserId == coordinatorCareer.UserId).AnyAsync(x=>x.Role.Name == ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
            {
                var userRole = new ApplicationUserRole
                {
                    RoleId = rolCoordinator.Id,
                    UserId = coordinatorCareer.UserId
                };
                await _context.UserRoles.AddAsync(userRole);
            }           
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteById(Guid id)
        {
            var rolCoordinator = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR).FirstOrDefaultAsync();

            var coordinatorCareer = await _context.CoordinatorCareers.FirstOrDefaultAsync(x => x.Id == id);

            var countCoordinatorCareers = await _context.CoordinatorCareers.Where(x => x.UserId == coordinatorCareer.UserId).CountAsync();

            if (countCoordinatorCareers - 1 <= 0)
            {
                var userRol = await _context.UserRoles.Where(x => x.RoleId == rolCoordinator.Id && x.UserId == coordinatorCareer.UserId).FirstOrDefaultAsync();

                if (userRol != null)
                {

                    _context.UserRoles.Remove(userRol);
                }
            }

            _context.CoordinatorCareers.Remove(coordinatorCareer);

            await _context.SaveChangesAsync();
        }

        public async Task DownloadExcel(IXLWorksheet worksheet)
        {
            CreateHeaderRow(worksheet);
            await LoadRegistryPatternInformation(worksheet);
        }
        
        public async Task<object> GetCareersByFacultySelect2ClientSide(Guid? facultyId, ClaimsPrincipal user = null)
        {
            var query = _context.Careers.AsQueryable();

            if (facultyId != null)
                query = query.Where(x => x.FacultyId == facultyId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
                {
                    query = query.Where(x => x.CoordinatorCareers.Any(y => y.UserId == userId));
                }            //Para bolsa en caso de Decano y secretaria de Decano
                else if (user.IsInRole(ConstantHelpers.ROLES.DEAN))
                {
                    query = query.Where(x => x.Faculty.DeanId == userId);
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Faculty.SecretaryId == userId);
                }
            }

            var result = await query
                .Select(c => new
                {
                    id = c.Id,
                    text = c.Name
                })
                .OrderBy(x => x.text)
                .ToListAsync();

            Guid? careerSelected = null;


            return new { items = result, selected = careerSelected };
        }

        public async Task<object> GetFacultieSelect2ClientSide(ClaimsPrincipal user = null)
        {
            var query = _context.Faculties.AsQueryable();

            Guid? facultySelected = null;

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
                {
                    query = query.Where(x => x.Careers.Any(y => y.CoordinatorCareers.Any(z => z.UserId == userId)));
                }            //Para bolsa en caso de Decano y secretaria de Decano
                else if (user.IsInRole(ConstantHelpers.ROLES.DEAN))
                {
                    query = query.Where(x => x.DeanId == userId);
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.SecretaryId == userId);
                }
            }

            var result = await query
                .Select(c => new
                {
                    id = c.Id,
                    text = c.Name
                }).OrderBy(x => x.text)
                .ToListAsync();

            return new { items = result, selected = facultySelected };

        }

        #endregion
    }
}
