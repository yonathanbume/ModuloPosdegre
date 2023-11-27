using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Faculty;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class FacultyRepository : Repository<Faculty>, IFacultyRepository
    {
        public FacultyRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<Faculty>> GetFacultiesDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<Faculty, Faculty>> selectPredicate = null, Expression<Func<Faculty, dynamic>> orderByPredicate = null, string searchValue = null)
        {
            var query = _context.Faculties
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            if (sentParameters.RecordsPerDraw == 0)
            {
                List<Faculty> data1 = await query.OrderByCondition(sentParameters.OrderDirection, orderByPredicate).Select(selectPredicate).ToListAsync();

                int recordsTotal1 = data1.Count;

                return new DataTablesStructs.ReturnedData<Faculty>
                {
                    Data = data1,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal1
                };
            }

            List<Faculty> data = await query.OrderByCondition(sentParameters.OrderDirection, orderByPredicate).Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw).Select(selectPredicate).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<Faculty>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetFacultiesSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Faculty, Select2Structs.Result>> selectPredicate = null, string searchValue = null)
        {
            IQueryable<Faculty> query = _context.Faculties
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            List<Select2Structs.Result> results = await query.Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE).Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE).Select(selectPredicate).ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetFacultiesByAcademicCoordinatorSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Faculty, Select2Structs.Result>> selectPredicate = null, string searchValue = null, string academicCoordinatorId = null)
        {
            var query = _context.Faculties
                .Where(x => x.Careers.Any(y => y.AcademicCoordinatorId == academicCoordinatorId || y.CareerDirectorId == academicCoordinatorId))
                .OrderBy(x => x.Name)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;

            var results = await query.Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE).Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE).Select(selectPredicate).ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<Faculty>> GetFacultiesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Faculty, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                default:
                    //orderByPredicate = ((x) => x.Name);
                    break;
            }

            return await GetFacultiesDatatable(sentParameters, ExpressionHelpers.SelectFaculty(), orderByPredicate, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetFacultiesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetFacultiesSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, searchValue);
        }

        public async Task<object> GetFacultiesSelect2ClientSide(bool hasAll = false, ClaimsPrincipal user = null)
        {
            IQueryable<Faculty> query = _context.Faculties.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        query = query.Where(x => x.DeanId == userId || x.SecretaryId == userId);
                    }
                }
                else
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    var faculties = _context.AcademicDepartments
                        .Where(x => x.AcademicDepartmentDirectorId == userId)
                        .Select(x => x.FacultyId).ToHashSet();
                    query = query.Where(x => faculties.Contains(x.Id));
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId).AsNoTracking();
                        var faculties = qryCareers.Select(x => x.FacultyId).ToHashSet();
                        query = query.Where(x => faculties.Contains(x.Id));
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    Guid[] faculties = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.FacultyId).ToArrayAsync();
                    query = query.Where(x => faculties.Any(y => y == x.Id));
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers.Where(x => x.QualityCoordinatorId == userId).AsNoTracking();
                        var faculties = qryCareers.Select(x => x.FacultyId).ToHashSet();
                        query = query.Where(x => faculties.Contains(x.Id));
                    }
                }
            }

            var result = await query
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            if (hasAll)
            {
                result.Insert(0, new { id = new Guid(), text = "Todas" });
            }

            return result;
        }

        public async Task<Select2Structs.ResponseParameters> GetFacultiesByAcademicCoordinatorSelect2(Select2Structs.RequestParameters requestParameters, string academicCoordinatorId, string searchValue = null)
        {
            return await GetFacultiesByAcademicCoordinatorSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, searchValue, academicCoordinatorId);
        }

        public async Task<Faculty> GetFacultyByCareerId(Guid careerId)
        {
            IQueryable<Faculty> faculty = _context.Faculties.Where(x => x.Careers.Any(s => s.Id == careerId));
            return await faculty.FirstOrDefaultAsync();
        }
        public async Task<object> GetAllAsSelect2ClientSide(bool hasAll = false, ClaimsPrincipal user = null)
        {
            IQueryable<Faculty> query = _context.Faculties.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    IQueryable<Career> qryCareers = _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId).AsNoTracking();
                    HashSet<Guid> faculties = qryCareers.Select(x => x.FacultyId).ToHashSet();
                    query = query.Where(x => faculties.Contains(x.Id));
                }
            }

            var result = await query
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            if (hasAll)
            {
                result.Insert(0, new { id = new Guid(), text = "Todas" });
            }

            return result;
        }

        public async Task<object> GetAllAsSelect2ClientSide2(bool includeTitle = false)
        {
            var faculties = await _context.Faculties.OrderBy(x => x.Name).Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).ToListAsync();

            if (includeTitle)
            {
                faculties.Insert(0, new { id = new Guid(), text = "Todas" });
            }

            return faculties;
        }

        public async Task<object> GetFaculty(Guid id)
        {
            return await _context.Faculties.Where(x => x.Id == id).Select(x => new
            {
                id = x.Id,
                name = x.Name,
                isValid = x.IsValid,
                resolution = x.DecanalResolutionNumber,
                abbreviation = x.Abbreviation,
                resolutionFile = x.DecanalResolutionFile
            }).FirstAsync();
        }

        public async Task<object> GetFaculties3()
        {
            return await _context.Faculties.OrderBy(x => x.Name).Select(x => new
            {
                id = x.Id,
                name = x.Name
            }).ToListAsync();
        }

        public async Task<ModelFacultyTemplate> GetFacultiesAdmitted()
        {
            var result = await _context.Faculties
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();
            var result2 = result.FirstOrDefault();

            ModelFacultyTemplate model = new ModelFacultyTemplate
            {
                Result = result,
                Active = result2.id
            };
            return model;
        }

        public async Task<object> GetFacultiesJson(string q, ClaimsPrincipal user = null)
        {
            IQueryable<Faculty> query = _context.Faculties.AsNoTracking();

            if (user != null)
            {
                string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var facultiesId = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.FacultyId).ToListAsync();
                    query = query.Where(x => facultiesId.Contains(x.Id));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {

                    if (!string.IsNullOrEmpty(userId))
                    {
                        IQueryable<ENTITIES.Models.Generals.Career> qryCareers = _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId).AsNoTracking();
                        HashSet<Guid> faculties = qryCareers.Select(x => x.FacultyId).ToHashSet();
                        query = query.Where(x => faculties.Contains(x.Id));
                    }
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        query = query.Where(x => x.DeanId == userId || x.SecretaryId == userId || x.AdministrativeAssistantId == userId);
                    }
                }
            }


            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(q.ToUpper()));
            }

            var result = await query
                .OrderBy(x => x.Name)
                .Select(f => new
                {
                    id = f.Id,
                    text = f.Name
                }).ToListAsync();

            return result;
        }

        public async Task<object> GetFacultiesAllJson()
        {
            var faculties = await _context.Faculties
                .OrderBy(x => x.Name)
                .Select(f => new
                {
                    id = f.Id,
                    text = f.Name
                }).ToListAsync();

            faculties.Insert(0, new { id = new Guid(), text = "Todas" });

            return faculties;
        }

        public async Task<List<Faculty>> GetAllWithCareers()
        {
            return await _context.Faculties.Include(x => x.Careers).ToListAsync();
        }

        public async Task<string> GetDeanById(Guid facultyId)
        {
            var dean = await _context.Faculties.Where(x => x.Id == facultyId).Select(x => x.DeanGrade + " " + x.Dean.FullName).FirstOrDefaultAsync();
            return dean;
        }

        public async Task<Faculty> GetWithHistory(Guid id)
        {
            var faculty = await _context.Faculties.Where(x => x.Id == id).Include(x => x.FacultyHistories).FirstOrDefaultAsync();
            return faculty;
        }

        public async Task<List<Faculty>> GetFacultiesByDean(string deanId)
        {
            var faculties = await _context.Faculties
                .Where(x => x.DeanId == deanId)
                .Include(x => x.Careers)
                .ToListAsync();

            return faculties;
        }

        public async Task<List<Faculty>> GetFacultiesBySecretary(string secretaryId)
        {
            var faculties = await _context.Faculties
                .Where(x => x.SecretaryId == secretaryId)
                .Include(x => x.Careers)
                .ToListAsync();

            return faculties;
        }


        public async Task<object> GetFacultyIncomesDatatable(DataTablesStructs.SentParameters sentParameters, int? year = null, int? type = null)
        {
            Expression<Func<Faculty, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    //orderByPredicate = ((x) => x.Name);
                    break;
            }

            var currentYear = DateTime.Now.Year;
            if (year.HasValue && year > 0) currentYear = year.Value;

            var startDate = (new DateTime(currentYear, 1, 1)).ToUtcDateTime();
            var endDate = (new DateTime(currentYear, 12, 31)).ToUtcDateTime().AddDays(1).AddTicks(-1);
            
            var qryPayments = _context.Payments
                .IgnoreQueryFilters()
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID
                && x.ConceptId.HasValue
                && x.PaymentDate.HasValue
                && startDate <= x.PaymentDate && x.PaymentDate <= endDate
                && x.User.Students.Any())
                .AsNoTracking();

            if (type.HasValue && type != 0)
            {
                switch (type)
                {
                    case 1: qryPayments = qryPayments.Where(x => x.IsBankPayment || (x.InvoiceId.HasValue && x.Invoice.PaymentType != ConstantHelpers.Treasury.Invoice.PaymentType.CASH)); break;
                    case 2: qryPayments = qryPayments.Where(x => x.InvoiceId.HasValue && x.Invoice.PaymentType == ConstantHelpers.Treasury.Invoice.PaymentType.CASH); break;
                    default:
                        break;
                }
            }

            var dataPayments = await qryPayments
                .Select(x => new
                {
                    //x.UserId,
                    PaymentDate = x.PaymentDate.Value,
                    x.Total,
                    FacultyId = x.User.Students.Select(y => y.Career.FacultyId).FirstOrDefault()
                })
                .ToListAsync();

            var payments = dataPayments
                .Select(x => new
                {
                    PaymentDate = x.PaymentDate.ToDefaultTimeZone(),
                    x.Total,
                    x.FacultyId
                })
                .ToList();

            var recordsFiltered = await _context.Faculties.CountAsync();

            var faculties = await _context.Faculties
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name
                })
                .ToListAsync();

            var data = faculties
                .Select(x => new
                {
                    code = x.Code,
                    name = x.Name,
                    january = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 1).Sum(p => p.Total), // x.Concepts.Sum(c => c.Payments.Where(p => p.PaymentDate.HasValue && p.PaymentDate.Value.Year == currentYear && p.PaymentDate.Value.Month == 1).Sum(p => p.Total)),
                    february = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 2).Sum(p => p.Total),
                    march = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 3).Sum(p => p.Total),
                    april = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 4).Sum(p => p.Total),
                    may = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 5).Sum(p => p.Total),
                    june = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 6).Sum(p => p.Total),
                    july = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 7).Sum(p => p.Total),
                    august = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 8).Sum(p => p.Total),
                    september = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 9).Sum(p => p.Total),
                    october = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 10).Sum(p => p.Total),
                    november = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 11).Sum(p => p.Total),
                    december = payments.Where(p => p.FacultyId == x.Id && p.PaymentDate.Month == 12).Sum(p => p.Total),
                    totalAmount = payments.Where(p => p.FacultyId == x.Id).Sum(p => p.Total)
                })
                .ToList();


            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
        #endregion
    }
}
