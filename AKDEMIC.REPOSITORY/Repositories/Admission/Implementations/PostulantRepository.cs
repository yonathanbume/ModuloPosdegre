using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.Postulant;
using Google.Apis.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class PostulantRepository : Repository<Postulant>, IPostulantRepository
    {
        public PostulantRepository(AkdemicContext context) : base(context) { }

        #region Private

        private IQueryable<Postulant> GetPostulantByAdmissionStateQuery(Guid? termId = null, byte? admissionState = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            var postulants = _context.Postulants
                .AsNoTracking();

            if (termId != null) postulants = postulants.Where(x => x.ApplicationTerm.TermId == termId);

            if (admissionState != null) postulants = postulants.Where(x => x.AdmissionState == admissionState);

            if (applicationTermId != null) postulants = postulants.Where(x => x.ApplicationTermId == applicationTermId);

            if (admissionTypeId != null) postulants = postulants.Where(x => x.AdmissionTypeId == admissionTypeId);

            //if (academicProgramId != null)
            //{
            //    postulants = postulants.Where(x => query.Any(y => y.CareerId == x.CareerId));
            //}

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    postulants = postulants.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            //Usaremos Carreras temporalmente
            if (careerId != null)
            {
                postulants = postulants.Where(x => x.CareerId == careerId);
            }

            return postulants;
        }

        #endregion
        public async Task<IEnumerable<Postulant>> GetAllByFacultyAndCareerAndTerm(Guid? facultyId = null, Guid? careerId = null, Guid? termId = null, byte? admissionState = null)
        {
            var query = _context.Postulants.AsQueryable();

            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(q => q.Career.FacultyId == facultyId.Value);

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(q => q.CareerId == careerId.Value);

            if (termId.HasValue && termId != Guid.Empty) query = query.Where(q => q.ApplicationTerm.TermId == termId);

            if (admissionState.HasValue) query = query.Where(x => x.AdmissionState == admissionState);

            var result = await query.Select(x => new Postulant
            {
                Id = x.Id,
                CareerId = x.CareerId,
                Career = new Career
                {
                    Name = x.Career.Name,
                    FacultyId = x.Career.FacultyId,
                    Faculty = new Faculty
                    {
                        Id = x.Career.FacultyId,
                        Name = x.Career.Faculty.Name
                    }
                },
                Name = x.Name,
                MaternalSurname = x.MaternalSurname,
                PaternalSurname = x.PaternalSurname,
                Document = x.Document,
                AdmissionState = x.AdmissionState,
                AdmissionType = new AdmissionType
                {
                    Name = x.AdmissionType.Name
                },
                Student = x.StudentId.HasValue ? new Student
                {
                    User = new ApplicationUser
                    {
                        Name = x.Student.User.Name,
                        PaternalSurname = x.Student.User.PaternalSurname,
                        MaternalSurname = x.Student.User.MaternalSurname,
                        UserName = x.Student.User.UserName,
                        FullName = x.Student.User.FullName
                    }
                } : null,
                ApplicationTerm = new ApplicationTerm
                {
                    Term = new Term
                    {
                        Id = x.ApplicationTerm.TermId,
                        Name = x.ApplicationTerm.Term.Name
                    }
                }
            }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Postulant>> GetAllPostulantsByFilters(Guid? facultyId = null, Guid? careerId = null, Guid? applicationTermId = null, Guid? termId = null, byte? admissionState = null, int? secondaryEducationType = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null, List<Guid> applicationTermsId = null)
        {
            var query = _context.Postulants.AsQueryable();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var queryCareer = _context.Careers
                    .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId);

                if (facultyId.HasValue && facultyId != Guid.Empty) queryCareer = queryCareer.Where(q => q.FacultyId == facultyId.Value);

                if (careerId.HasValue && careerId != Guid.Empty) queryCareer = queryCareer.Where(q => q.Id == careerId.Value);

                var careers = queryCareer.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(q => q.Career.FacultyId == facultyId.Value);

                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(q => q.CareerId == careerId.Value);
            }

            if (termId.HasValue && termId != Guid.Empty) query = query.Where(q => q.ApplicationTerm.TermId == termId);

            if (applicationTermId.HasValue && applicationTermId != Guid.Empty) query = query.Where(q => q.ApplicationTermId == applicationTermId);

            if (admissionState.HasValue) query = query.Where(x => x.AdmissionState == admissionState);

            if (admissionTypeId.HasValue && admissionTypeId != Guid.Empty) query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (secondaryEducationType.HasValue) query = query.Where(x => x.SecondaryEducationType == secondaryEducationType);

            if (applicationTermsId != null && applicationTermsId.Any())
            {
                query = query.Where(x => applicationTermsId.Contains(x.ApplicationTermId));
            }

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<Postulant>> GetReportByFacultyAndCareerAndTerm(Guid? facultyId = null,
            Guid? careerId = null, Guid? termId = null)
        {
            var query = _context.Postulants.AsQueryable();
            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(q => q.Career.FacultyId == facultyId.Value);
            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(q => q.CareerId == careerId.Value);
            if (termId.HasValue && termId != Guid.Empty)
                query = query.Where(q => q.ApplicationTerm.TermId == termId);
            var result = await query.Select(x => new Postulant
            {
                Id = x.Id,
                CareerId = x.CareerId,
                Career = new Career
                {
                    Name = x.Career.Name,
                    FacultyId = x.Career.FacultyId,
                    Faculty = new Faculty
                    {
                        Name = x.Career.Faculty.Name
                    }
                },
                Name = x.Name,
                MaternalSurname = x.MaternalSurname,
                PaternalSurname = x.PaternalSurname,
                Document = x.Document,
                Sex = x.Sex,
                BirthDate = x.BirthDate,
                AdmissionTypeId = x.AdmissionTypeId,
                AdmissionType = new AdmissionType
                {
                    Name = x.AdmissionType.Name
                },
                BirthDepartmentId = x.BirthDepartmentId,
                BirthDepartment = new Department
                {
                    Name = x.BirthDepartment.Name
                },
                BirthProvinceId = x.BirthProvinceId,
                BirthProvince = new Province
                {
                    Name = x.BirthProvince.Name
                },
                BirthDistrictId = x.BirthDistrictId,
                BirthDistrict = new District
                {
                    Name = x.BirthDistrict.Name
                },
                AdmissionState = x.AdmissionState,
                ApplicationTermId = x.ApplicationTermId,
                ApplicationTerm = new ApplicationTerm
                {
                    TermId = x.ApplicationTerm.TermId,
                    Term = new Term
                    {
                        Name = x.ApplicationTerm.Term.Name
                    }
                }
            }).ToListAsync();
            return result;
        }

        public async Task<Postulant> GetPostulantByDniAndTerm(string dni, Guid termId)
        {
            var postulant = await _context.Postulants.Where(x => x.Document == dni && x.ApplicationTermId == termId).Include(x => x.AdmissionType).FirstOrDefaultAsync();

            return postulant;
        }
        public async Task<int> GetStudentCounttByTermAndCareer(Guid admissionTermId, Guid careerId, bool? accepted = null)
        {
            var query = _context.Postulants.Where(x => x.ApplicationTermId == admissionTermId && x.CareerId == careerId);

            if (accepted.HasValue)
            {
                query = query.Where(x => x.Accepted == accepted);
            }

            var studentsCount = await query.CountAsync();

            return studentsCount;
        }

        public async Task<object> GetPostulantsAdmittedByCurrentAppTermId(Guid? currentApplicationTermId = null)
        {
            var result = await _context.Postulants
                .Where(p => p.ApplicationTermId == currentApplicationTermId && p.Accepted)
                .Select(p => new
                {
                    id = p.Id,
                    dni = p.Document,
                    name = p.FullName,
                    career = p.Career.Name,
                    admissionType = p.AdmissionType.Name,
                    accepted = p.Accepted,
                    code = (p.Student != null) ? p.Student.User.UserName : "---",
                }).ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatableAdmitted(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, Guid? typeId = null, Guid? currentApplicationTermId = null)
        {
            var query = _context.Postulants
                .Where(x => x.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED).AsQueryable();

            if (currentApplicationTermId == Guid.Empty)
            {
                var active = await _context.ApplicationTerms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
                if (active != null) currentApplicationTermId = active.Id;
            }

            query = query.Where(p => p.ApplicationTermId == currentApplicationTermId);


            var recordsFiltered = await query.CountAsync();

            if (facultyId != Guid.Empty)
            {
                query = query.Where(x => x.Career.FacultyId == facultyId);
            }

            if (careerId != Guid.Empty)
            {
                query = query.Where(x => x.CareerId == careerId);
            }

            if (typeId != Guid.Empty)
            {
                query = query.Where(x => x.AdmissionTypeId == typeId);
            }

            var data = await query
                //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(post => new
                {
                    id = post.Id,
                    dni = post.Document,
                    name = post.FullName,
                    faculty = post.Career.Faculty.Name,
                    career = post.Career.Name,
                    admissionType = post.AdmissionType.Name,
                    accepted = post.Accepted,
                    code = post.ProofOfIncomeStatus == ConstantHelpers.PROOF_OF_INCOME_STATUS.NON_PICKED_UP ? "---" : (post.Student != null ? post.Student.User.UserName : "---"),
                    proofOfIncomeStatus = post.ProofOfIncomeStatus,
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

        public async Task<List<Postulant>> GetAdmittedPostulantsByCurrentAppTermId(Guid currentApplicationTermId, bool studentNull = false)
        {
            if (currentApplicationTermId == Guid.Empty)
            {
                var active = await _context.ApplicationTerms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).OrderBy(x => x.Term.Name).FirstOrDefaultAsync();
                if (active != null) currentApplicationTermId = active.Id;
            }

            var qry = _context.Postulants
                .Where(p => p.ApplicationTermId == currentApplicationTermId
                && p.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED)
                .AsQueryable();

            if (studentNull)
                qry = qry.Where(x => !x.StudentId.HasValue);

            var postulantsWithStudentNull = await qry
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .Include(x => x.AdmissionType)
                .Include(x => x.Career)
                .Include(x => x.Career.Faculty)
                .ToListAsync();

            return postulantsWithStudentNull;
        }

        public async Task<List<Postulant>> GetAdmittedPostulantsByTermId(Guid termId)
        {
            var qry = _context.Postulants
                .Where(p => p.ApplicationTerm.TermId == termId
                && p.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED)
                .AsQueryable();

            var data = await qry
                .Include(x => x.AdmissionType)
                .Include(x => x.Career)
                .ToListAsync();

            return data;
        }
        public async Task<List<Postulant>> GetAdmittedPostulantsByCurrentAppTermIdOrderByCareer(Guid currentApplicationTermId)
        {
            var postulants = await _context.Postulants
                .Include(x => x.AdmissionType)
                .OrderBy(x => x.Career)
                .Where(p => p.ApplicationTermId == currentApplicationTermId && p.Accepted)
                .ToListAsync();

            return postulants;
        }

        //Por ahora por carrera, ya que no hay relacion directa del postulante con un programa academico
        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantByAdmissionStateDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, byte? admissionState = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            var postulants = GetPostulantByAdmissionStateQuery(termId, admissionState, applicationTermId, careerId, admissionTypeId, user);

            var recordsFiltered = await postulants
                    .Select(x => new
                    {
                        x.CareerId,
                        x.AdmissionTypeId,
                        x.ApplicationTermId,
                    })
                    .Distinct()
                    .CountAsync();

            var data = await postulants
                //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.CareerId,
                    Career = x.Career.Name,
                    x.AdmissionTypeId,
                    AdmissionType = x.AdmissionType.Name,
                    x.ApplicationTermId,
                    ApplicationTerm = x.ApplicationTerm.Name
                })
                .GroupBy(x => new { x.CareerId, x.Career, x.AdmissionTypeId, x.AdmissionType, x.ApplicationTermId, x.ApplicationTerm })
                .Select(x => new
                {
                    x.Key.Career,
                    x.Key.AdmissionType,
                    x.Key.ApplicationTerm,
                    Accepted = x.Count()
                })
                .OrderByDescending(x => x.Accepted)
                .ThenBy(x => x.Career)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<object> GetPostulantAcceptedDatatable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Id;
                    break;
            }

            var postulants = _context.Postulants.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    postulants = postulants.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (applicationTermId.HasValue)
                postulants = postulants.Where(x => x.ApplicationTermId == applicationTermId);

            var recordsFiltered = await postulants
                .Select(x => new
                {
                    x.CareerId,
                    x.ApplicationTerm.TermId
                })
                .Distinct()
                .CountAsync();

            var preData = await postulants
                .Select(x => new
                {
                    Career = x.Career.Name,
                    CareerId = x.CareerId,
                    ApplicationTerm = x.ApplicationTerm.Term.Name,
                    ApplicationTermId = x.ApplicationTerm.TermId,
                    AdmissionState = x.AdmissionState
                })
                .ToListAsync();

            var data = preData
                .GroupBy(x => new { x.Career, x.CareerId, x.ApplicationTerm, x.ApplicationTermId })
                .Select(x => new
                {
                    x.Key.Career,
                    x.Key.ApplicationTerm,
                    Total = x.Count(),
                    Accepted = x.Where(y => y.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED).Count()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<object> GetPostulantByAdmissionStateChart(Guid? termId = null, byte? admissionState = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            var postulants = GetPostulantByAdmissionStateQuery(termId, admissionState, applicationTermId, careerId, admissionTypeId, user);

            var qryCareers = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    qryCareers = qryCareers.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null) qryCareers.Where(x => x.Id == careerId);

            var careers = await qryCareers
                .Select(x => new
                {
                    Career = x.Name,
                    Accepted = postulants.Where(y => y.CareerId == x.Id).Count()
                })
                .OrderByDescending(x => x.Accepted)
                .ThenBy(x => x.Career)
                .ToListAsync();

            var result = new
            {
                categories = careers.Select(x => x.Career).ToList(),
                data = careers.Select(x => x.Accepted).ToList()
            };

            return result;
        }

        public async Task<object> GetPostulantAcceptedPieChartByApplicationTerm(Guid? applicationTermId = null, ClaimsPrincipal user = null)
        {
            var postulants = _context.Postulants
                .AsQueryable();

            if (applicationTermId.HasValue)
                postulants = postulants.Where(x => x.ApplicationTermId == applicationTermId);

            var group = postulants.AsEnumerable().GroupBy(x => x.Accepted).ToList();

            var data = group.Select(x => new
            {
                name = x.Select(y => y.Accepted).FirstOrDefault() ? "Ingresantes" : "No Admitidos/Pendientes",
                y = x.Count()
            }).ToList();

            return new { data };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, string search = null)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Document;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career;
                    break;
                case "4":
                    orderByPredicate = (x) => x.AdmissionType;
                    break;
                default:
                    break;
            }


            var query = _context.Postulants.AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (termId != Guid.Empty) query = query.Where(x => x.ApplicationTermId == termId);

            if (facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(search))
            {
                var normalizedSearch = search.Normalize().ToUpper();

                query = query.Where(x => x.Document.Normalize().ToUpper().Contains(normalizedSearch)
                || x.Name.Normalize().ToUpper().Contains(normalizedSearch));
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    dni = x.Document,
                    name = x.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    paidadmission = x.PaidAdmission,
                    accepted = x.Accepted,
                    admissionType = x.AdmissionType.Name,
                    photo = x.Picture != null
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

        public async Task<List<Postulant>> GetPostulantIncludeApplication(Term term, Guid careerId)
        {
            var postulants = await _context.Postulants
               .Include(x => x.ApplicationTerm)
               .Include(x => x.ApplicationTerm.Term)
               .Where(x => x.ApplicationTerm.Term == term && x.CareerId == careerId)
               .ToListAsync();

            return postulants;
        }

        public async Task<List<Postulant>> GetPostulantsDetail(Guid termId, Guid careerId)
        {
            var postulants = await _context.Postulants
               .Include(x => x.AdmissionType)
               .Include(x => x.Career)
               .Include(x => x.ApplicationTerm)
               .Include(x => x.ApplicationTerm.Term)
               .Where(x => x.ApplicationTerm.TermId == termId && x.CareerId == careerId).ToListAsync();

            return postulants;
        }

        public async Task<Postulant> GetPostulantWithAdmission(string dni, Guid currentApplicationTermId)
        {
            var postulant = await _context.Postulants.Include(x => x.AdmissionType).FirstOrDefaultAsync(p => p.Document == dni && p.ApplicationTermId == currentApplicationTermId);
            return postulant;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcceptedPostulantByResultDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            byte admissionState = ConstantHelpers.POSTULANTS.ApplicationStatus.ADMITTED;

            var postulants = GetPostulantByAdmissionStateQuery(termId, admissionState, applicationTermId, careerId, admissionTypeId, user);

            var recordsFiltered = await postulants
                    .Select(x => new { x.CareerId, x.AdmissionTypeId, x.ApplicationTermId, x.FinalScore })
                    .Distinct()
                    .CountAsync();

            var data = await postulants
                    .Select(x => new
                    {
                        x.CareerId,
                        Career = x.Career.Name,
                        x.AdmissionTypeId,
                        AdmissionType = x.AdmissionType.Name,
                        x.ApplicationTermId,
                        ApplicationTerm = x.ApplicationTerm.Name,
                        x.FinalScore
                    })
                    .GroupBy(x => new { x.CareerId, x.Career, x.AdmissionTypeId, x.AdmissionType, x.ApplicationTermId, x.ApplicationTerm, x.FinalScore })
                    .Select(x => new
                    {
                        x.Key.Career,
                        x.Key.AdmissionType,
                        x.Key.ApplicationTerm,
                        x.Key.FinalScore,
                        Accepted = x.Count()
                    })
                    .OrderByDescending(x => x.Accepted)
                    .ThenBy(x => x.Career)
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
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

        //Agregar filtros por resultados ??
        public async Task<object> GetAcceptedPostulantByResultChart(Guid? termId = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            byte admissionState = ConstantHelpers.POSTULANTS.ApplicationStatus.ADMITTED;

            var postulants = GetPostulantByAdmissionStateQuery(termId, admissionState, applicationTermId, careerId, admissionTypeId, user);

            var queryCareers = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    queryCareers = queryCareers.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            var careers = await queryCareers
                .Select(x => new
                {
                    Career = x.Name,
                    Accepted = postulants.Where(y => y.CareerId == x.Id).Count()
                })
                .OrderByDescending(x => x.Accepted)
                .ThenBy(x => x.Career)
                .ToListAsync();

            var result = new
            {
                categories = careers.Select(x => x.Career).ToList(),
                data = careers.Select(x => x.Accepted).ToList()
            };

            return result;
        }
        //Agregar filtros por resultados ??
        public async Task<object> GetAcceptedPostulantByResult(Guid? applicationTermId = null, Guid? academicProgramId = null, Guid? admissionTypeId = null)
        {
            var postulants = _context.Postulants
                //.Where(x => x.Accepted)
                .AsQueryable();


            if (applicationTermId != Guid.Empty)
            {
                postulants = postulants.Where(x => x.ApplicationTermId == applicationTermId);
            }

            if (admissionTypeId != Guid.Empty)
            {
                postulants = postulants.Where(x => x.AdmissionTypeId == admissionTypeId);
            }

            if (academicProgramId != Guid.Empty)
            {
                postulants = postulants.Where(x => x.CareerId == academicProgramId);
            }

            var postulantresult = await postulants
                .Select(x => new
                {
                    dni = x.Document,
                    name = x.FullName,
                    career = x.Career.Name,
                    admissionType = x.AdmissionType.Name,
                    accepted = x.AdmissionState == 1
                }).ToListAsync();
            return postulantresult;
        }
        //ELIMINAR
        public async Task<IEnumerable<Postulant>> GetAllPostulantsWithDataInclude(Guid careerId, Guid applicationTernId)
        {
            var postulant = await _context.Postulants
                .Include(x => x.Career).ThenInclude(x => x.Faculty)
                .Include(x => x.ApplicationTerm).ThenInclude(x => x.Term)
                .Include(x => x.AdmissionType)
                .ToListAsync();

            return postulant;
        }

        public async Task<List<Postulant>> GetCareerPostulantRegular(Guid careerId, Guid applicationTernId)
        {
            var result = await _context.Postulants
                   .Where(p => p.CareerId == careerId && p.ApplicationTermId == applicationTernId && p.AdmissionType.Type == ConstantHelpers.ADMISSION_MODE.ORDINARY && p.AdmissionState != 0)
                   .OrderBy(p => p.AdmissionTypeId).ThenByDescending(p => p.FinalScore)
                   .ToListAsync();

            return result;
        }

        public async Task<Postulant> GetWithData(Guid id)
        {
            return await _context.Postulants
                .Where(x => x.Id == id)
                    .Include(x => x.ExamCampus)
                    .Include(x => x.Student)
                        .ThenInclude(x => x.User)
                    .Include(x => x.Career)
                        .ThenInclude(x => x.Faculty)
                    .Include(x => x.ApplicationTerm)
                        .ThenInclude(x => x.Term)
                    .Include(x => x.NationalityCountry)
                    .Include(x => x.BirthCountry)
                    .Include(x => x.BirthDepartment)
                    .Include(x => x.BirthProvince)
                    .Include(x => x.BirthDistrict)
                    .Include(x => x.AdmissionType)
                    .Include(x => x.Department)
                    .Include(x => x.Province)
                    .Include(x => x.District)
                    .Include(x => x.SecondaryEducationDistrict)
                    .Include(x => x.SecondaryEducationProvince)
                    .Include(x => x.SecondaryEducationDepartment)
                .FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRegularAdmissionDatatableClientSide(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, string search)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.AdmissionType.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.FinalScore;
                    break;
                case "5":
                    orderByPredicate = (x) => x.OrderMerit;
                    break;
                case "6":
                    orderByPredicate = (x) => x.OrderMeritBySchool;
                    break;
                case "7":
                    orderByPredicate = (x) => x.AdmissionState;
                    break;
                default:
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
            }

            var query = _context.Postulants
                .Where(p => p.PaidAdmission && p.IsVerified)
                .AsNoTracking();

            if (applicationTermId == Guid.Empty)
            {
                var active = await _context.ApplicationTerms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
                if (active != null) applicationTermId = active.Id;
            }

            query = query.Where(p => p.ApplicationTermId == applicationTermId);

            //CEPRE no es considerado en el examen de admision
            query = query.Where(x => !x.AdmissionType.IsCepreAdmissionType);

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.FullName, search) || EF.Functions.Contains(x.Document, search));
                }
                else
                    query = query.Where(x => x.Code.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(p => new
                {
                    id = p.Id,
                    code = p.Code,
                    name = p.FullName,
                    career = p.Career.Name,
                    admissionState = p.AdmissionState,
                    admissionType = p.AdmissionType.Name,
                    score = p.FinalScore,
                    order = p.OrderMerit,
                    orderBySchool = p.OrderMeritBySchool
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

        public async Task<object> GetIrregularAdmissionDatatableClientSide(Guid applicationTermId, Guid? careerId = null, Guid? typeId = null)
        {
            var query = _context.Postulants
                .Where(x => x.ApplicationTermId == applicationTermId)
                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(p => p.CareerId == careerId);

            if (typeId.HasValue && typeId != Guid.Empty) query = query.Where(p => p.AdmissionTypeId == typeId);

            var result = await query
               .Select(p => new
               {
                   id = p.Id,
                   dni = p.Document,
                   name = p.FullName,
                   career = p.Career.Name,
                   accepted = p.Accepted,
                   admissionType = p.AdmissionType.Name
               }).ToListAsync();

            return result;
        }

        public async Task<Postulant> GetWithStudentData(Guid id)
        {
            return await _context.Postulants
                    .Where(x => x.Id == id)
                    .Include(x => x.Student).ThenInclude(x => x.User)
                    .Include(x => x.Career).ThenInclude(x => x.Faculty)
                            .Include(x => x.ApplicationTerm).ThenInclude(x => x.Term)
                        .Include(x => x.NationalityCountry)
                        .Include(x => x.BirthCountry)
                    .Include(x => x.AdmissionType)
                            .Include(x => x.Department)
                            .Include(x => x.Province)
                            .Include(x => x.District)
                    .FirstOrDefaultAsync();
        }

        public async Task<bool> GetPostulantExist(Guid currentApplicationTermId, string cell, Guid admissionTypeId, Guid careerId)
        {
            var postulantExist = await _context.Postulants
                .AnyAsync(p => p.ApplicationTermId == currentApplicationTermId
                && p.Document == cell
                && p.AdmissionTypeId == admissionTypeId
                && p.CareerId == careerId);

            return postulantExist;
        }

        public async Task<bool> GetPostulantExistAd(Guid currentApplicationTermId, string cell, Guid admissionTypeId)
        {
            var postulantExist = await _context.Postulants
                .AnyAsync(p => p.ApplicationTermId == currentApplicationTermId
                && p.Document == cell
                && p.AdmissionTypeId == admissionTypeId);

            return postulantExist;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantResultsDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, string search = null)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Document;
                    break;
                default:
                    break;
            }

            var query = _context.Postulants
                .Where(p => p.ApplicationTermId == applicationTermId && p.IsVerified && p.PaidAdmission)
                .AsNoTracking();

            if (careerId.HasValue && careerId.Value != Guid.Empty) query = query.Where(p => p.CareerId == careerId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper()) || x.Document.Contains(search));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(p => new
                {
                    id = p.Id,
                    dni = p.Document,
                    name = p.FullName,
                    career = p.Career.Name,
                    condition = p.Accepted ? "INGRESO" : "NO INGRESO",
                    admissionType = p.AdmissionType.Name,
                    score = p.FinalScore,
                    meritOrder = (p.OrderMerit == -1) ? "--" : p.OrderMerit.ToString()
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

        public async Task<object> GetPostulantResultByDni(Guid? currentApplicationTermId = null, string dni = null)
        {
            var result = await _context.Postulants
             .Where(p => p.ApplicationTermId == currentApplicationTermId && p.Document == dni)
             .Select(p => new
             {
                 name = p.Name,
                 paternalSurname = p.PaternalSurname,
                 maternalSurname = p.MaternalSurname,
                 Score = p.FinalScore,
                 meritOrder = p.OrderMerit > 0 ? $"{p.OrderMerit:000}" : "---",
                 condition = p.Accepted ? "INGRESO" : "NO INGRESO",
             }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<PostulantResultTemplate>> GetPostulantResultsByDni(string dni, Guid? currentApplicationTermId = null)
        {
            var query = _context.Postulants.AsNoTracking();

            if (currentApplicationTermId != Guid.Empty && currentApplicationTermId != null)
                query = query.Where(p => p.ApplicationTermId == currentApplicationTermId);

            var result = await query
             .Where(p => p.Document == dni)
             .Select(p => new PostulantResultTemplate
             {
                 //datos generales    
                 Id = p.Id,
                 Name = p.Name,
                 PaternalSurname = p.PaternalSurname,
                 MaternalSurname = p.MaternalSurname,
                 Document = p.Document,
                 Sex = ConstantHelpers.SEX.VALUES[p.Sex],
                 MaritalStatus = ConstantHelpers.MARITAL_STATUS.VALUES[p.MaritalStatus],
                 DocumentType = ConstantHelpers.DOCUMENT_TYPES.VALUES[p.DocumentType],
                 BirthDate = p.BirthDate.ToLocalDateFormat(),
                 Phone1 = p.Phone1,
                 Email = p.Email,
                 NationalityCountry = p.NationalityCountry.Name,
                 Image = p.Picture,
                 //postulacion
                 ApplicationTerm = p.ApplicationTerm.Name,
                 ApplicationTermCreatedAt = p.ApplicationTerm.Term.CreatedAt,
                 AdmissionType = p.AdmissionType.Name,
                 Career = p.Career.Name,
                 AdmissionState = ConstantHelpers.Admission.Postulant.AdmissionState.VALUES[p.AdmissionState],
                 PaidAdmission = p.PaidAdmission ? "Pagada" : "Pendiente de pago",
                 PaidAdmissionValue = p.PaidAdmission,
                 IsVerified = p.IsVerified ? "Verificado" : "Por verificar",
                 IsVerifiedValue = p.IsVerified,

                 //examen de admission
                 ClassRoom = p.AdmissionExamClassroomPostulants
                 .Select(x => $"{x.AdmissionExamClassroom.Classroom.Code} - {x.AdmissionExamClassroom.Classroom.Description}")
                 .FirstOrDefault() ?? "",
                 Campus = p.AdmissionExamClassroomPostulants
                 .Select(x => x.AdmissionExamClassroom.Classroom.Building.Campus.Name)
                 .FirstOrDefault() ?? "",

                 //AdmissionExamDate = _context.AdmissionExamClassroomPostulants
                 //            .Where(x => x.AdmissionExamClassroom.AdmissionExam.AdmissionExamApplicationTerms.Any(y => y.ApplicationTermId == p.ApplicationTermId) &&
                 //                            x.Postulant.Document == dni)
                 //            .Select(x => x.AdmissionExamClassroom.AdmissionExam.DateEvaluation.ToLocalDateFormat())
                 //            .FirstOrDefault() ?? "",                 
                 AdmissionExamDate = p.ApplicationTerm.AdmissionExamApplicationTerms.Any()
                 ? p.ApplicationTerm.AdmissionExamApplicationTerms.Select(x => x.AdmissionExam.DateEvaluation.ToLocalDateFormat()).FirstOrDefault()
                 : "",
                 //AdmissionExamDateTime = p.AdmissionExamClassroomPostulants.Select(x => x.AdmissionExamClassroom.AdmissionExam.DateEvaluation).FirstOrDefault(),
                 AdmissionExamDateTime = p.ApplicationTerm.AdmissionExamApplicationTerms.Any()
                 ? p.ApplicationTerm.AdmissionExamApplicationTerms.Select(x => x.AdmissionExam.DateEvaluation).FirstOrDefault()
                 : null,


                 //resultados
                 ResultPublished = p.ApplicationTerm.WasPublished,
                 OrderMerit = p.OrderMerit > 0 ? $"{p.OrderMerit:000}" : "---",
                 Condition = p.Accepted ? "INGRESO" : "NO INGRESO",
                 FinalScore = p.FinalScore,
                 ReinscriptionDays = p.ApplicationTerm.ReinscriptionDays,
                 PublicationDate = p.ApplicationTerm.PublicationDate
             })
             .OrderByDescending(x => x.ApplicationTermCreatedAt)
             .ToListAsync();



            var daysToShowClassroom = await GetIntConfigurationValue(ConstantHelpers.Configuration.AdmissionManagement.POSTULANT_PREVIOUS_DAYS_TO_SHOW_CLASSROOM);
            foreach (var item in result)
            {
                if (item.ResultPublished && item.Condition != "INGRESO")
                {
                    var inRange = item.PublicationDate.AddDays(item.ReinscriptionDays).ToDefaultTimeZone().Date >= DateTime.UtcNow.ToDefaultTimeZone().Date;
                    if (item.ReinscriptionDays > 0 && inRange)
                        item.ShowReinscriptionButton = true;
                }

                if (!item.AdmissionExamDateTime.HasValue)
                    continue;

                var date = item.AdmissionExamDateTime.Value.ToDefaultTimeZone().Date;
                var currentDate = DateTime.UtcNow.ToDefaultTimeZone().Date;
                var showDate = date.AddDays(daysToShowClassroom * -1) <= currentDate;

                if (!showDate)
                {
                    item.ClassRoom = "???";
                }
            }

            return result;
        }

        public async Task<List<Postulant>> GetPostulantsAsync(Guid currentApplicationTermId, Guid careerId, Guid admissionTypeId)
        {
            var result = await _context.Postulants
                .Where(p => p.ApplicationTermId == currentApplicationTermId && p.CareerId == careerId && p.AdmissionTypeId == admissionTypeId).ToListAsync();

            return result;
        }

        public async Task<string> GetUserWithCodeExist(string userCodePrefix, Guid? applicationTermId = null)
        {
            var query = _context.Postulants
                .Where(u => u.Code.StartsWith(userCodePrefix));

            if (applicationTermId.HasValue && applicationTermId != Guid.Empty)
                query = query.Where(x => x.ApplicationTermId == applicationTermId);

            var result = await query
                .Select(u => u.Code)
                .OrderByDescending(u => u)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Postulant>> GetPostulantApplicationTermId(Guid applicationTermId)
        {
            var result = await _context.Postulants
                .Where(p => p.ApplicationTermId == applicationTermId)
                .Include(x => x.Career).ThenInclude(x => x.Faculty)
                .Include(x => x.AdmissionType)
                .Include(x => x.AcademicProgram)
                .ToListAsync();

            return result;
        }

        public async Task<List<PostulantReportTemplate>> GetTopScoresPerCareer(Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? admissionTypeId = null, Guid? applicationTermId = null, byte? admissionState = null, ClaimsPrincipal user = null, List<Guid> applicationTermsIds = null)
        {
            var topScoresQuery = _context.Postulants.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var queryFiler = _context.Careers
                    .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId);

                if (facultyId.HasValue && facultyId != Guid.Empty)
                    queryFiler = queryFiler.Where(x => x.FacultyId == facultyId);

                if (careerId.HasValue && careerId != Guid.Empty)
                    queryFiler = queryFiler.Where(x => x.Id == careerId);

                var careers = queryFiler.Select(x => x.Id).ToHashSet();

                topScoresQuery.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty)
                    topScoresQuery = topScoresQuery.Where(x => x.Career.FacultyId == facultyId);

                if (careerId.HasValue && careerId != Guid.Empty)
                    topScoresQuery = topScoresQuery.Where(x => x.CareerId == careerId);
            }

            if (admissionTypeId.HasValue && admissionTypeId != Guid.Empty)
                topScoresQuery = topScoresQuery.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (termId.HasValue && termId != Guid.Empty)
                topScoresQuery = topScoresQuery.Where(x => x.ApplicationTerm.TermId == termId);
            else
            {
                var termActive = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
                if (termActive == null)
                    termActive = new Term();

                topScoresQuery = topScoresQuery.Where(x => x.ApplicationTerm.TermId == termActive.Id);
            }

            if (applicationTermId.HasValue && applicationTermId != Guid.Empty)
                topScoresQuery = topScoresQuery.Where(x => x.ApplicationTermId == applicationTermId);

            if (applicationTermsIds != null && applicationTermsIds.Any())
                topScoresQuery = topScoresQuery.Where(x => applicationTermsIds.Contains(x.ApplicationTermId));

            //agregar parametro, byte? admissionState = null
            if (admissionState.HasValue) topScoresQuery = topScoresQuery.Where(x => x.AdmissionState == admissionState);

            var topScores = await topScoresQuery
                .OrderByDescending(x => x.FinalScore)
                .Select(
                    x => new PostulantReportTemplate
                    {
                        Faculty = x.Career.Faculty.Name,
                        Career = x.Career.Name,
                        AdmissionType = x.AdmissionType.Name,
                        Score = x.FinalScore,
                        Dni = x.Document,
                        Fullname = x.RawFullName
                    }
                )
                .Take(10)
                .ToListAsync();

            return topScores;
        }

        public async Task<bool> GetPostulantByDniId(Guid id, string dni)
        {
            return await _context.Postulants.AnyAsync(x => x.ApplicationTermId == id && x.Document == dni);
        }
        public async Task<bool> GetPostulantByDniIdAndActiveApplicationTerms(string dni)
        {
            var postulants = await _context.Postulants.Where(x => x.Document == dni).ToListAsync();
            if (postulants != null || postulants.Count() > 0)
            {
                var applicationTerms = await _context.ApplicationTerms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).ToListAsync();
                foreach (var item in applicationTerms)
                {
                    var exist = postulants.Any(x => x.ApplicationTermId == item.Id);
                    if (exist)
                        return true;
                }
            }
            return false;
        }
        public async Task<bool> GetPostulantByDniIdAdmission(Guid id, string dni)
        {
            return await _context.Postulants.AnyAsync(x => x.ApplicationTermId == id && x.Document == dni && !x.PaidAdmission);
        }

        public async Task<object> GetPostulantDebts(Guid id, string dni)
        {
            var result = await _context.Postulants
                .Where(x => x.ApplicationTermId == id && x.Document == dni)
                .Select(x => new
                {
                    id = x.Id,
                    fullname = x.FullName,
                    registerdate = x.RegisterDate.ToLocalDateFormat(),
                    postulantto = x.Career.Name,
                    modality = x.AdmissionType.Name,
                    debt = $"Pub: {x.AdmissionType.PublicSchoolConcept.Amount}/Priv: {x.AdmissionType.PrivateSchoolConcept.Amount}"
                }).FirstOrDefaultAsync();

            return result;
        }
        public async Task<decimal> GetCostPostulantById(Guid id)
        {
            return await _context.Postulants.Where(x => x.Id == id).Select(x => x.SecondaryEducationType == ConstantHelpers.SECONDARY_EDUCATION_TYPE.PUBLIC ? x.AdmissionType.PublicSchoolConcept.Amount : x.AdmissionType.PrivateSchoolConcept.Amount).FirstOrDefaultAsync();
        }

        public async Task<object> GetPostulantsPayment(Guid id)
        {
            var result = await _context.Postulants
             .Where(x => x.ApplicationTermId == id)
             .Select(x => new
             {
                 id = x.Id,
                 dni = x.Document,
                 fullname = x.FullName,
                 registerdate = x.RegisterDate.ToLocalDateFormat(),
                 postulantto = x.Career.Name,
                 modality = x.AdmissionType.Name,
                 wasPaid = x.PaidAdmission
             }).ToListAsync();


            return result;
        }

        public async Task<Postulant> GetPostulantWithAdmissionById(Guid id)
        {
            return await _context.Postulants.Include(x => x.AdmissionType).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tuple<bool, string>> GenerateStudents(UserManager<ApplicationUser> userManager, Guid currentApplicationTermId)
        {
            var applicationTerm = await _context.ApplicationTerms.Where(x => x.Id == currentApplicationTermId).Include(x => x.Term).FirstOrDefaultAsync();

            if (applicationTerm == null)
                applicationTerm = await _context.ApplicationTerms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).OrderBy(x => x.Term.Name).Include(x => x.Term).FirstOrDefaultAsync();

            if (applicationTerm == null) return new Tuple<bool, string>(false, "No hay proceso de admisión activo");

            var postulants = await _context.Postulants
                .Where(x => x.ApplicationTermId == applicationTerm.Id && x.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED && !x.StudentId.HasValue)
                .Include(x => x.AdmissionType)
                .ToListAsync();
            if (!postulants.Any()) return new Tuple<bool, string>(false, "No hay postulantes pendientes de la generación de código");

            //var codeYearDigits = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_MANAGEABLE_CODE_CURRENT_YEAR_DIGITS);
            //var codeCareerFlag = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_MANAGEABLE_CODE_FACULTY_FLAG));
            //var codeCorrelativeDigits = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_MANAGEABLE_CODE_CORRELATIVE_DIGITS);
            var studentCodeFormat = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.NEW_STUDENT_CODE_FORMAT);
            bool.TryParse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.NEW_STUDENT_CODE_USE_DOCUMENT), out var useDocumentAsUserName);

            if (useDocumentAsUserName)
            {
                var postulantsDocuments = postulants.Select(x => x.Document).ToHashSet();
                var usersDocumentDuplicated = await _context.Users.Where(x => postulantsDocuments.Any(y => y == x.UserName))
                    .Select(x => x.Document)
                    .ToListAsync();

                if (usersDocumentDuplicated.Any())
                    return new Tuple<bool, string>(false, $"Existen usuarios con los siguientes documentos {string.Join("; ", usersDocumentDuplicated)}");

            }

            var enrollmentFeeCost = 0.00M;
            var enrollmentFeeSubtotal = 0.00M;
            var enrollmentFeeIgv = 0.00M;
            Guid? enrollmentFeeId = null;
            var enrollmentFeeDescription = "";

            var enrollmentPaymentMethod = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PAYMENT_METHOD);

            var config = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ADMISSION_ENROLLMENT_PROCEDURE);
            var conceptId = string.IsNullOrEmpty(config) ? null : config;
            if (conceptId != null)
            {
                var concept = await _context.Concepts.FindAsync(Guid.Parse(conceptId));
                enrollmentFeeId = Guid.Parse(conceptId);
                enrollmentFeeDescription = concept.Description;
                enrollmentFeeCost = concept.Amount;

                if (concept.IsTaxed)
                {
                    enrollmentFeeSubtotal = enrollmentFeeCost / (1.00M + ConstantHelpers.Treasury.IGV);
                    enrollmentFeeIgv = enrollmentFeeCost - enrollmentFeeSubtotal;
                }
                else
                {
                    enrollmentFeeSubtotal = enrollmentFeeCost;
                }
            }

            var enrollmentConcepts = await _context.EnrollmentConcepts
                .IgnoreQueryFilters()
                .Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ADDITIONAL_CONCEPT
                && (!x.Condition.HasValue || x.Condition == ConstantHelpers.Student.Condition.REGULAR))
                .Select(x => new
                {
                    x.ConceptId,
                    x.Concept.Description,
                    x.Concept.Amount,
                    x.Concept.IsTaxed,
                    x.Concept.CurrentAccountId,
                })
                .ToListAsync();

            var replacementConcepts = await _context.EnrollmentConcepts
                .Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ADDITIONAL_CONCEPT_REPLACEMENT)
                .Select(x => new
                {
                    x.ConceptId,
                    x.Concept.Description,
                    x.Concept.Amount,
                    x.Concept.IsTaxed,
                    x.Concept.CurrentAccountId,
                    x.Condition,
                    x.ConceptToReplaceId,
                    x.CareerId
                })
                .ToListAsync();

            var admissionTyDescounts = await _context.AdmissionTypeDescounts
                .Where(a => a.TermId == applicationTerm.TermId)
                .ToListAsync();

            var year = applicationTerm.Term.Year.ToString();
            var period = applicationTerm.Term?.Number ?? "0";

            var curriculums = await _context.Curriculums.ToListAsync();
            if (studentCodeFormat.Contains('c')) //Se separa por carrera
            {
                var careers = await _context.Careers
                    .Select(x => new
                    {
                        x.Id,
                        x.Code,
                        x.Name,
                        FacultyCode = x.Faculty.Code
                    })
                    .ToListAsync();

                foreach (var career in careers)
                {
                    var careerPostulants = postulants.Where(x => x.CareerId == career.Id).ToList();
                    if (careerPostulants.Count == 0) continue;

                    var curriculum = curriculums.Where(x => x.CareerId == career.Id).OrderByDescending(x => x.IsActive).OrderByDescending(x => x.Year).ThenByDescending(x => x.Code).FirstOrDefault();
                    if (curriculum == null) return new Tuple<bool, string>(false, $"La carrera {career.Name} no posee plan de estudios activo");

                    foreach (var postulant in careerPostulants)
                    {
                        var modality = postulant.AdmissionType.Abbreviation;
                        var correlative = 1;

                        var userCode = "";

                        if (useDocumentAsUserName)
                        {
                            userCode = postulant.Document;
                        }
                        else
                        {
                            userCode = GenerateStudentCode(studentCodeFormat, year, period, career.FacultyCode, career.Code, postulant.AdmissionType.Abbreviation, postulant.RacialIdentity, correlative.ToString());
                            while (await _context.Users.AnyAsync(x => x.UserName.ToUpper() == userCode.ToUpper()))
                            {
                                correlative++;
                                userCode = GenerateStudentCode(studentCodeFormat, year, period, career.FacultyCode, career.Code, postulant.AdmissionType.Abbreviation, postulant.RacialIdentity, correlative.ToString());
                            }
                        }


                        var civilStatus = ConstantHelpers.CIVIL_STATUS.SINGLE;
                        switch (postulant.MaritalStatus)
                        {
                            case ConstantHelpers.MARITAL_STATUS.MARRIED:
                                civilStatus = ConstantHelpers.CIVIL_STATUS.MARRIED; break;
                            case ConstantHelpers.MARITAL_STATUS.DIVORCED:
                                civilStatus = ConstantHelpers.CIVIL_STATUS.DIVORCED; break;
                            case ConstantHelpers.MARITAL_STATUS.WIDOWED:
                                civilStatus = ConstantHelpers.CIVIL_STATUS.WIDOWER; break;
                            default:
                                break;
                        }

                        var user = new ApplicationUser
                        {
                            UserName = userCode,
                            BirthDate = postulant.BirthDate,
                            Address = postulant.Address,
                            DistrictId = postulant.DistrictId,
                            DepartmentId = postulant.DepartmentId,
                            CivilStatus = civilStatus,
                            DocumentType = postulant.DocumentType,
                            IsActive = true,
                            ProvinceId = postulant.ProvinceId,
                            Sex = postulant.Sex,
                            Type = ConstantHelpers.USER_TYPES.STUDENT,
                            CreatedAt = DateTime.UtcNow,
                            Email = postulant.Email,
                            PersonalEmail = postulant.Email,
                            Name = postulant.Name,
                            PaternalSurname = postulant.PaternalSurname,
                            MaternalSurname = postulant.MaternalSurname,
                            Dni = postulant.Document,
                            Document = postulant.Document,
                            PhoneNumber = postulant.Phone1,
                            FullName = $"{(string.IsNullOrEmpty(postulant.PaternalSurname) ? "" : $"{postulant.PaternalSurname} ")}{(string.IsNullOrEmpty(postulant.MaternalSurname) ? "" : $"{postulant.MaternalSurname}")}, {(string.IsNullOrEmpty(postulant.Name) ? "" : $"{postulant.Name}")}"
                        };
                        var password = postulant.Document;

                        var result = await userManager.CreateAsync(user, password);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.STUDENTS);

                            var student = new Student
                            {
                                AcademicProgramId = postulant.AcademicProgramId,
                                CareerId = postulant.CareerId,
                                CurrentAcademicYear = 1,
                                RegisterDate = DateTime.UtcNow,
                                UserId = user.Id,
                                AdmissionTypeId = postulant.AdmissionTypeId,
                                AdmissionTermId = applicationTerm.TermId,
                                Status = postulant.AdmissionType.IsTransfer ? ConstantHelpers.Student.States.TRANSFER : ConstantHelpers.Student.States.ENTRANT,
                                Curriculum = curriculum,
                                CampusId = postulant.CampusId,
                                AdmissionDate = DateTime.UtcNow,
                                RacialIdentity = postulant.RacialIdentity
                            };

                            await _context.Students.AddAsync(student);
                            postulant.Student = student;

                            // Agregar el tramite de derecho de matrícula a los ingresantes
                            if (enrollmentPaymentMethod == 1)
                            {
                                var discount = 0.00M;
                                var total = enrollmentFeeCost;

                                var admissionTyDescount = admissionTyDescounts.FirstOrDefault(a => a.AdmissionTypeId == postulant.AdmissionTypeId);
                                var discountPercentage = admissionTyDescount?.Descount;
                                if (discountPercentage.HasValue)
                                {
                                    discount = enrollmentFeeCost * (decimal)discountPercentage.Value / 100.0M;
                                    total = enrollmentFeeCost - discount;
                                }

                                var payment = new Payment
                                {
                                    Description = enrollmentFeeDescription,
                                    SubTotal = enrollmentFeeSubtotal,
                                    IgvAmount = enrollmentFeeIgv,
                                    Discount = discount,
                                    Total = total,
                                    EntityId = enrollmentFeeId,
                                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = enrollmentFeeId,
                                    TermId = applicationTerm.TermId
                                };
                                await _context.Payments.AddAsync(payment);

                                foreach (var item in enrollmentConcepts)
                                {
                                    var itemTotal = item.Amount;
                                    var subtotal = itemTotal;
                                    var igv = 0.00M;

                                    var isTaxed = item.IsTaxed;
                                    var description = item.Description;
                                    var itemConceptId = item.ConceptId;
                                    var currentAccountId = item.CurrentAccountId;

                                    if (replacementConcepts.Any(x => x.ConceptToReplaceId == item.ConceptId && x.CareerId == student.CareerId))
                                    {
                                        var conceptReplace = replacementConcepts.FirstOrDefault(x => x.ConceptToReplaceId == item.ConceptId && x.CareerId == student.CareerId);

                                        itemTotal = conceptReplace.Amount;
                                        subtotal = itemTotal;
                                        igv = 0.00M;

                                        isTaxed = conceptReplace.IsTaxed;
                                        description = conceptReplace.Description;
                                        itemConceptId = conceptReplace.ConceptId;
                                        currentAccountId = conceptReplace.CurrentAccountId;
                                    }

                                    if (isTaxed)
                                    {
                                        subtotal = itemTotal / (1.00M + ConstantHelpers.Treasury.IGV);
                                        igv = itemTotal - subtotal;
                                    }

                                    await _context.Payments.AddAsync(new Payment
                                    {
                                        Description = description,
                                        SubTotal = subtotal,
                                        IgvAmount = igv,
                                        Discount = 0.00M,
                                        Total = itemTotal,
                                        EntityId = Guid.Empty,
                                        Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                        UserId = student.UserId,
                                        ConceptId = itemConceptId,
                                        TermId = applicationTerm.TermId,
                                        CurrentAccountId = currentAccountId
                                    });
                                }
                            }
                        }

                        //userCodeSufix++;
                    }
                }
            }
            else
            {
                var careers = await _context.Careers
                        .Select(x => new
                        {
                            x.Id,
                            x.Code,
                            x.Name,
                            FacultyCode = x.Faculty.Code
                        })
                        .ToListAsync();

                foreach (var postulant in postulants)
                {
                    var career = careers.FirstOrDefault(x => x.Id == postulant.CareerId);

                    var curriculum = curriculums.Where(x => x.CareerId == postulant.CareerId).OrderByDescending(x => x.IsActive).OrderByDescending(x => x.Year).ThenByDescending(x => x.Code).FirstOrDefault();
                    if (curriculum == null)
                    {
                        //var career = await _context.Careers.FindAsync(postulant.CareerId);
                        return new Tuple<bool, string>(false, $"La carrera {career.Name} no posee plan de estudios activo");
                    }

                    var modality = postulant.AdmissionType.Abbreviation;
                    var correlative = 1;
                    var userCode = "";

                    if (useDocumentAsUserName)
                    {
                        userCode = postulant.Document;
                    }
                    else
                    {
                        userCode = GenerateStudentCode(studentCodeFormat, year, period, career.FacultyCode, "", postulant.AdmissionType.Abbreviation, postulant.RacialIdentity, correlative.ToString());
                        while (await _context.Users.AnyAsync(x => x.UserName.ToUpper() == userCode.ToUpper()))
                        {
                            correlative++;
                            userCode = GenerateStudentCode(studentCodeFormat, year, period, career.FacultyCode, "", postulant.AdmissionType.Abbreviation, postulant.RacialIdentity, correlative.ToString());
                        }
                    }

                    var user = new ApplicationUser
                    {
                        UserName = userCode,
                        Email = postulant.Email,
                        Name = postulant.Name,
                        PaternalSurname = postulant.PaternalSurname,
                        MaternalSurname = postulant.MaternalSurname,
                        Dni = postulant.Document,
                        Document = postulant.Document,
                        PhoneNumber = postulant.Phone1,
                        BirthDate = postulant.BirthDate,
                        Address = postulant.Address,
                        DistrictId = postulant.DistrictId,
                        DepartmentId = postulant.DepartmentId,
                        DocumentType = postulant.DocumentType,
                        IsActive = true,
                        ProvinceId = postulant.ProvinceId,
                        Sex = postulant.Sex,
                        Type = ConstantHelpers.USER_TYPES.STUDENT,
                        CreatedAt = DateTime.UtcNow,
                        PersonalEmail = postulant.Email,
                        FullName = $"{(string.IsNullOrEmpty(postulant.PaternalSurname) ? "" : $"{postulant.PaternalSurname} ")}{(string.IsNullOrEmpty(postulant.MaternalSurname) ? "" : $"{postulant.MaternalSurname}")}, {(string.IsNullOrEmpty(postulant.Name) ? "" : $"{postulant.Name}")}"
                    };

                    var password = postulant.Document;

                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.STUDENTS);

                        var student = new Student
                        {
                            AcademicProgramId = postulant.AcademicProgramId,
                            CareerId = postulant.CareerId,
                            CurrentAcademicYear = 1,
                            RegisterDate = DateTime.UtcNow,
                            UserId = user.Id,
                            AdmissionTypeId = postulant.AdmissionTypeId,
                            AdmissionTermId = applicationTerm.TermId,
                            Status = postulant.AdmissionType.IsTransfer ? ConstantHelpers.Student.States.TRANSFER : ConstantHelpers.Student.States.ENTRANT,
                            Curriculum = curriculum,
                            CampusId = postulant.CampusId,
                            AdmissionDate = DateTime.UtcNow,
                            RacialIdentity = postulant.RacialIdentity
                        };

                        await _context.Students.AddAsync(student);
                        postulant.Student = student;

                        // Agregar el tramite de derecho de matrícula a los ingresantes
                        if (enrollmentPaymentMethod == 1)
                        {
                            var discount = 0.00M;
                            var total = enrollmentFeeCost;

                            var admissionTyDescount = admissionTyDescounts.FirstOrDefault(a => a.AdmissionTypeId == postulant.AdmissionTypeId);
                            var discountPercentage = admissionTyDescount?.Descount;
                            if (discountPercentage.HasValue)
                            {
                                discount = enrollmentFeeCost * (decimal)discountPercentage.Value / 100.0M;
                                total = enrollmentFeeCost - discount;
                            }

                            var payment = new Payment
                            {
                                Description = enrollmentFeeDescription,
                                SubTotal = enrollmentFeeSubtotal,
                                IgvAmount = enrollmentFeeIgv,
                                Discount = discount,
                                Total = total,
                                EntityId = enrollmentFeeId,
                                Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                UserId = student.UserId,
                                ConceptId = enrollmentFeeId,
                                TermId = applicationTerm.TermId
                            };
                            await _context.Payments.AddAsync(payment);

                            foreach (var item in enrollmentConcepts)
                            {
                                var itemTotal = item.Amount;
                                var subtotal = itemTotal;
                                var igv = 0.00M;

                                if (item.IsTaxed)
                                {
                                    subtotal = itemTotal / (1.00M + ConstantHelpers.Treasury.IGV);
                                    igv = itemTotal - subtotal;
                                }

                                await _context.Payments.AddAsync(new Payment
                                {
                                    Description = item.Description,
                                    SubTotal = subtotal,
                                    IgvAmount = igv,
                                    Discount = 0.00M,
                                    Total = itemTotal,
                                    EntityId = Guid.Empty,
                                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = item.ConceptId,
                                    TermId = applicationTerm.TermId
                                });
                            }
                        }
                    }
                    //userCodeSufix++;
                }
            }

            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, "");
        }

        private string GenerateStudentCode(string format, string year, string period, string faculty, string career, string modality, byte racialIdentity, string correlative)
        {
            var array = format.ToCharArray();
            var result = "";

            var prevChar = "";
            var text = "";

            var racialText = racialIdentity == ConstantHelpers.Student.RacialIdentity.OTHER ? ConstantHelpers.Student.RacialIdentity.HALF_BLOOD.ToString()
                : racialIdentity.ToString();

            foreach (var c in array)
            {
                if (prevChar != "")
                {
                    if (prevChar == c.ToString())
                    {
                        text = $"{text}{c}";
                    }
                    else
                    {
                        var length = text.Length;
                        var part = "";
                        switch (prevChar)
                        {
                            case "y":
                                part = year.PadLeft(length, '0');
                                part = part.Substring(part.Length - length);
                                result = $"{result}{part}";
                                break;
                            case "p":
                                part = period.PadLeft(length, '0');
                                part = part.Substring(part.Length - length);
                                result = $"{result}{part}";
                                break;
                            case "f":
                                part = faculty.PadLeft(length, '0');
                                part = part.Substring(part.Length - length);
                                result = $"{result}{part}";
                                break;
                            case "c":
                                part = career.PadLeft(length, '0');
                                part = part.Substring(part.Length - length);
                                result = $"{result}{part}";
                                break;
                            case "m":
                                part = modality.PadLeft(length, '0');
                                part = part.Substring(part.Length - length);
                                result = $"{result}{part}";
                                break;
                            case "r":
                                part = correlative.PadLeft(length, '0');
                                part = part.Substring(part.Length - length);
                                result = $"{result}{part}";
                                break;
                            case "o":
                                part = racialText.PadLeft(length, '0');
                                part = part.Substring(part.Length - length);
                                result = $"{result}{part}";
                                break;
                            default:
                                result = $"{result}{text}";
                                break;
                        }

                        text = c.ToString();
                        prevChar = c.ToString();
                    }
                }
                else
                {
                    text = c.ToString();
                    prevChar = c.ToString();
                }
            }

            if (prevChar != "")
            {
                var length = text.Length;
                string part;
                switch (prevChar)
                {
                    case "y":
                        part = year.PadLeft(length, '0');
                        part = part.Substring(part.Length - length);
                        result = $"{result}{part}";
                        break;
                    case "p":
                        part = period.PadLeft(length, '0');
                        part = part.Substring(part.Length - length);
                        result = $"{result}{part}";
                        break;
                    case "f":
                        part = faculty.PadLeft(length, '0');
                        part = part.Substring(part.Length - length);
                        result = $"{result}{part}";
                        break;
                    case "c":
                        part = career.PadLeft(length, '0');
                        part = part.Substring(part.Length - length);
                        result = $"{result}{part}";
                        break;
                    case "m":
                        part = modality.PadLeft(length, '0');
                        part = part.Substring(part.Length - length);
                        result = $"{result}{part}";
                        break;
                    case "r":
                        part = correlative.PadLeft(length, '0');
                        part = part.Substring(part.Length - length);
                        result = $"{result}{part}";
                        break;
                    case "o":
                        part = racialText.PadLeft(length, '0');
                        part = part.Substring(part.Length - length);
                        result = $"{result}{part}";
                        break;
                    default:
                        result = $"{result}{text}";
                        break;
                }
            }

            return result;
        }

        public async Task<List<PostulantTemplate>> GetPostulantByAdmissionStateDataExcel(byte? admissionState, Guid? applicationTermId = null, Guid? academicProgramId = null, Guid? admissionTypeId = null)
        {
            var postulants = _context.Postulants
                .AsNoTracking();

            if (admissionState != null) postulants = postulants.Where(x => x.AdmissionState == admissionState);

            if (applicationTermId != null) postulants = postulants.Where(x => x.ApplicationTermId == applicationTermId);

            if (admissionTypeId != null) postulants = postulants.Where(x => x.AdmissionTypeId == admissionTypeId);

            //Usaremos Carreras temporalmente
            if (academicProgramId != null)
            {
                postulants = postulants.Where(x => x.CareerId == academicProgramId);
            }

            var postulantsGrouped = postulants
                    .GroupBy(x => new { x.CareerId, x.AdmissionTypeId, x.ApplicationTermId });

            var data = await postulantsGrouped
                    .Select(x => new PostulantTemplate
                    {
                        Career = x.Select(y => y.Career.Name).FirstOrDefault(),
                        AdmissionType = x.Select(y => y.AdmissionType.Name).FirstOrDefault(),
                        ApplicationTerm = x.Select(y => y.ApplicationTerm.Term.Name).FirstOrDefault(),
                        Accepted = x.Count()
                    }).ToListAsync();

            return data;
        }

        public async Task<List<Postulant>> GetPostulantAdmission(AdmissionExam admissionExam)
        {
            var applicationTermsId = admissionExam.AdmissionExamApplicationTerms.Select(x => x.ApplicationTermId).ToList();
            var channels = admissionExam.AdmissionExamChannels.Select(x => x.ChannelId).ToList();

            var query = _context.Postulants
                .Where(x => applicationTermsId.Contains(x.ApplicationTermId) && x.PaidAdmission && x.IsVerified)
                .AsQueryable();

            if (channels.Any())
                query = query.Where(x => x.ChannelId.HasValue && channels.Contains(x.ChannelId.Value));

            var result = await query
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<Postulant>> GetAdmittedApplicants(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? admissionTypeId = null, Guid? applicationTermId = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Career.Faculty.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.AdmissionType);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Document);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.PaternalSurname);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.FinalScore);
                    break;
                default:
                    orderByPredicate = ((x) => x.PaternalSurname);
                    break;
            }

            var query = _context.Postulants
                .Include(x => x.Career).ThenInclude(x => x.Faculty)
                .Where(x => x.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED).AsQueryable();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                query = query.Where(x => careers.Contains(x.CareerId));
            }

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            if (admissionTypeId.HasValue)
                query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (applicationTermId.HasValue)
                query = query.Where(x => x.ApplicationTermId == applicationTermId);
            else
            {
                var applicationTermActive = await _context.ApplicationTerms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
                query = query.Where(x => x.ApplicationTermId == applicationTermActive.Id);
            }

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<Postulant, Postulant>> selectPredicate = (x) => new Postulant
            {
                Career = new Career
                {
                    Name = x.Career.Name,
                    Faculty = new Faculty
                    {
                        Name = x.Career.Faculty.Name
                    }
                },
                AdmissionType = new AdmissionType
                {
                    Name = x.AdmissionType.Name
                },
                FinalScore = x.FinalScore,
                Document = x.Document,
                PaternalSurname = x.PaternalSurname,
                MaternalSurname = x.MaternalSurname,
                Name = x.Name,
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }
        public async Task<List<AdmittedStudentTemplate>> GetAdmittedStudentsExcel(Guid? facultyId = null, Guid? careerId = null, Guid? typeId = null, Guid? currentApplicationTermId = null, ClaimsPrincipal user = null)
        {
            var query = _context.Postulants
                .Where(p => p.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED).AsNoTracking();

            if (currentApplicationTermId.HasValue)
                query = query.Where(x => x.ApplicationTermId == currentApplicationTermId);

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                query = query.Where(x => careers.Contains(x.CareerId));
            }

            if (facultyId != null && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (typeId != null && typeId != Guid.Empty) query = query.Where(x => x.AdmissionTypeId == typeId);

            var data = await query
                .Select(p => new AdmittedStudentTemplate
                {
                    Dni = p.Document,
                    FullName = p.FullName,
                    Faculty = p.Career.Faculty.Name,
                    Career = p.Career.Name,
                    AdmissionType = p.AdmissionType.Name,
                    Code = p.StudentId.HasValue ? p.Student.User.UserName : "---",
                    FinalScore = p.FinalScore,
                    PostulantCode = p.Code
                }).ToListAsync();

            return data;
        }
        public async Task<Postulant> GetByStudent(Guid studentId)
        {
            return await _context.Postulants.Where(x => x.StudentId == studentId).FirstOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<object> GetTopFacultiesPostulants(Guid id)
        {
            return await _context.Postulants
                 .Where(p => p.ApplicationTerm.TermId.Equals(id))
                 .GroupBy(p => p.Career.Faculty.Name)
                 .Select(p => new
                 {
                     faculty = p.Key,
                     careers = _context.Careers.Where(f => f.Faculty.Name.Equals(p.Key)).Count(),
                     postulantsCount = p.Count()
                 }).ToListAsync();
        }
        public async Task<int> CountByDateAndBroadcastMedium(DateTime startDate, int broadcastMedium)
        {
            return await _context.Postulants.Where(x => x.RegisterDate > startDate).CountAsync(x => x.BroadcastMedium == broadcastMedium);
        }

        public async Task<int> CreatePostulants(int count)
        {
            var applicationTerm = await _context.ApplicationTerms
                .Include(x => x.Term)
                .FirstOrDefaultAsync(x => x.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE);

            var admissionTypes = await _context.AdmissionTypes.ToListAsync();

            var district = await _context.Districts.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
            var province = await _context.Provinces.Where(x => x.Id == district.ProvinceId).FirstOrDefaultAsync();
            var department = await _context.Departments.Where(x => x.Id == province.DepartmentId).FirstOrDefaultAsync();
            var country = await _context.Countries.Where(x => x.Id == department.CountryId).FirstOrDefaultAsync();

            var careers = await _context.Careers.ToListAsync();

            var campuses = await _context.Campuses.ToListAsync();

            var userCodePrefix = await _context.ApplicationTerms.Where(t => t.Id == applicationTerm.Id)
            .Select(t => "P" + t.Term.Year.ToString() + t.Term.Number.ToString()).FirstOrDefaultAsync();
            var usersWithCodeExist = await _context.Postulants.Where(u => u.Code.StartsWith(userCodePrefix)).Select(u => u.Code).OrderByDescending(u => u).FirstOrDefaultAsync();
            var userCodeSufix = usersWithCodeExist == null ? 1 : int.Parse(usersWithCodeExist.Substring(usersWithCodeExist.Length - 5)) + 1;

            var birthDate = ConvertHelpers.DatepickerToDatetime("01/01/1990");

            const string emailSuffix = "@akdemic.pe";

            var rnd = new Random();

            var list = new List<Postulant>();
            for (var i = 0; i < count; i++)
            {
                var dni = rnd.Next(10000000, 99999999);
                var admissionType = admissionTypes[rnd.Next(admissionTypes.Count)];
                var career = careers[rnd.Next(careers.Count)];

                var postulantCode = userCodePrefix + userCodeSufix.ToString().PadLeft(5, '0');

                var validateCodeExist = await _context.Postulants.AnyAsync(x => x.Code == postulantCode);
                if (validateCodeExist)
                {
                    while (validateCodeExist)
                    {
                        userCodeSufix++;
                        postulantCode = userCodePrefix + userCodeSufix.ToString().PadLeft(5, '0');
                        validateCodeExist = await _context.Postulants.AnyAsync(x => x.Code == postulantCode);
                    }
                }

                var campus = campuses[rnd.Next(campuses.Count)];

                var postulant = new Postulant
                {
                    Code = postulantCode,

                    AdmissionType = admissionType,
                    ApplicationTerm = applicationTerm,
                    BirthCountry = country,
                    BirthDepartment = department,
                    BirthDistrict = district,
                    BirthProvince = province,

                    SecondaryEducationDepartment = department,
                    SecondaryEducationProvince = province,
                    SecondaryEducationDistrict = district,

                    Career = career,
                    Department = department,
                    District = district,
                    NationalityCountry = country,
                    Province = province,

                    Name = GetRandomName(rnd),
                    PaternalSurname = GetRandomLastName(rnd),
                    MaternalSurname = GetRandomLastName(rnd),

                    Document = dni.ToString(),
                    Sex = CORE.Helpers.ConstantHelpers.SEX.MALE,
                    BirthDate = birthDate,
                    MaritalStatus = ConstantHelpers.MARITAL_STATUS.SINGLE,
                    Address = "---",
                    Email = $"{dni}{emailSuffix}",
                    WorkingCurrently = false,
                    SecondaryEducationName = "---",
                    SecondaryEducationType = ConstantHelpers.SECONDARY_EDUCATION_TYPE.PUBLIC,
                    SecondaryEducationFinished = ConstantHelpers.FINISHED_SECONDARY_EDUCATION.YES,
                    Representative = ConstantHelpers.REPRESENTATIVE_TYPE.NONE,
                    Childrens = 0,
                    BroadcastMedium = ConstantHelpers.BROADCAST_MEDIUM.INTERNET,
                    AdmissionState = 0,
                    RegisterDate = DateTime.UtcNow,
                    HasOtherSuperiorEducation = false,
                    SuperiorEducationStarts1 = DateTime.UtcNow,
                    HasTwoSuperiorEducations = false,
                    SuperiorEducationStarts2 = DateTime.UtcNow,
                    SecondaryEducationStarts = ConvertHelpers.DatepickerToDatetime("01/01/2010"),
                    SecondaryEducationEnds = ConvertHelpers.DatepickerToDatetime("10/12/2014"),
                    Campus = campus
                };

                list.Add(postulant);
                userCodeSufix++;
            }

            await _context.Postulants.AddRangeAsync(list);

            await _context.SaveChangesAsync();
            var caount = list.Count;
            return list.Count;
        }
        private string GetRandomName(Random rnd)
        {
            var names = new[]
            {
                "Rashad",
                "London",
                "Adelyn",
                "Ayana",
                "Destiny",
                "Gideon",
                "Leia",
                "Kendrick",
                "Amiah",
                "Julius",
                "Micah",
                "Issac",
                "Jase",
                "Sophia",
                "Isis",
                "Liliana",
                "Jordan",
                "Jay",
                "Aubrie",
                "Adison",
                "Allen",
                "Todd",
                "Micaela",
                "Julissa",
                "Annika",
                "Peyton",
                "Tiana",
                "Beau",
                "Max",
                "Johnathan",
                "Fiona",
                "Rayan",
                "Amelia",
                "Frank",
                "Gunnar",
                "Adrienne",
                "Miah",
                "Lincoln",
                "Jeremy",
                "Heath",
                "Camila",
                "Pamela",
                "Braxton",
                "Warren",
                "Gavyn",
                "Jazmine",
                "Nikhil",
                "Mekhi",
                "Jamie",
                "Eli"
            };
            return names[rnd.Next(names.Length)];
        }
        private string GetRandomLastName(Random rnd)
        {
            var lastnames = new[]
            {
                "Arellano",
                "Dunn",
                "Friedman",
                "Mclaughlin",
                "Norris",
                "Olson",
                "Santana",
                "Bass",
                "Evans",
                "Ho",
                "Davenport",
                "Marks",
                "Chandler",
                "Osborne",
                "Fuller",
                "Combs",
                "Bradford",
                "Curtis",
                "Green",
                "Vega",
                "Peck",
                "Love",
                "Moreno",
                "Mcdonald",
                "Ramirez",
                "Carter",
                "Spence",
                "Acevedo",
                "Solomon",
                "Schmitt",
                "Hicks",
                "Key",
                "Hampton",
                "Whitehead",
                "Schwartz",
                "Frye",
                "Webster",
                "Wagner",
                "Riddle",
                "Montes",
                "Gamble",
                "Allen",
                "Sparks",
                "Hays",
                "Walsh",
                "Reilly",
                "Ramos",
                "Tucker",
                "Morales",
                "Miles"
            };
            return lastnames[rnd.Next(lastnames.Length)];
        }

        public async Task<Tuple<int, int>> PostulantPaymentJob(Guid currentApplicationTermId)
        {
            var postulants = await _context.Postulants.Where(x => x.ApplicationTermId == currentApplicationTermId && !x.PaidAdmission).OrderBy(x => x.Id).ToListAsync();

            var half = (int)Math.Round(postulants.Count / 2.00M, 0, MidpointRounding.AwayFromZero);

            var postulantsToUpdate = postulants.Take(half);
            foreach (var item in postulantsToUpdate)
            {
                item.PaidAdmission = true;
            }

            await _context.SaveChangesAsync();
            return new Tuple<int, int>(postulantsToUpdate.Count(), postulants.Count());
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetNonOrdinaryAdmissionDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, string search = null)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    break;
            }

            var query = _context.Postulants
                .Where(x => x.ApplicationTermId == applicationTermId
                && x.AdmissionType.Type != ConstantHelpers.ADMISSION_MODE.ORDINARY
                && x.PaidAdmission && x.IsVerified)
                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (typeId.HasValue && typeId != Guid.Empty) query = query.Where(x => x.AdmissionTypeId == typeId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper()) || x.Document.Contains(search));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(p => new
                {
                    id = p.Id,
                    dni = p.Document,
                    name = p.FullName,
                    career = p.Career.Name,
                    admissionState = p.AdmissionState,
                    admissionType = p.AdmissionType.Name,
                    score = p.FinalScore,
                    order = p.OrderMerit,
                    accepted = p.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, bool? hasDiscapacity = null, bool? hasPhoto = null, string search = null)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Document;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "2":
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.AdmissionType.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.IsVerified;
                    break;
                case "6":
                    orderByPredicate = (x) => x.PaidAdmission;
                    break;
                default:
                    break;
            }

            var query = _context.Postulants
                .Where(x => x.ApplicationTermId == applicationTermId)
                .AsNoTracking();

            if (hasDiscapacity.HasValue) query = query.Where(x => x.HasDiscapacity == hasDiscapacity);

            if (hasPhoto.HasValue)
            {
                if (hasPhoto.Value) query = query.Where(x => x.Picture != null);
                else query = query.Where(x => x.Picture == null);
            }

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (typeId.HasValue && typeId != Guid.Empty) query = query.Where(x => x.AdmissionTypeId == typeId);

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.PaternalSurname.ToUpper().Contains(search.ToUpper()) ||
            x.MaternalSurname.ToUpper().Contains(search.ToUpper()) ||
            x.Name.ToUpper().Contains(search.ToUpper()) ||
            x.Document.Contains(search));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(p => new
                {
                    id = p.Id,
                    dni = p.Document,
                    name = p.FullName,
                    career = p.Career.Name,
                    admissionType = p.AdmissionType.Name,
                    accepted = p.Accepted,
                    paidadmission = p.PaidAdmission,
                    photo = p.Picture != null ? true : false,
                    p.ReniecPicture,
                    verified = p.IsVerified,
                    code = p.Code,
                    photoPath = p.Picture,
                    inscriptionDate = p.RegisterDate.ToLocalDateFormat(),
                    verifiedBy = string.IsNullOrEmpty(p.VerifiedBy) ? "---" : p.VerifiedBy,
                    verifiedDate = p.VerifiedDate.HasValue ? p.VerifiedDate.ToLocalDateFormat() : "",
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsAdmittedDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, string search = null)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                //case "0":
                //    orderByPredicate = (x) => x.Document;
                //    break;
                //case "1":
                //    orderByPredicate = (x) => x.FullName;
                //    break;
                //case "2":
                //    orderByPredicate = (x) => x.Career.Name;
                //    break;
                //case "3":
                //    orderByPredicate = (x) => x.AdmissionType.Name;
                //    break;
                //case "4":
                //    orderByPredicate = (x) => x.Accepted;
                //    break;
                //case "5":
                //    orderByPredicate = (x) => x.PaidAdmission;
                //    break;
                //case "6":
                //orderByPredicate = (x) => x.Picture;
                //break;
                default:
                    break;
            }

            var query = _context.Postulants
                .Where(x => x.ApplicationTermId == applicationTermId)
                .AsNoTracking();


            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (typeId.HasValue && typeId != Guid.Empty) query = query.Where(x => x.AdmissionTypeId == typeId);

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper()) || x.Document.Contains(search));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(p => new PostulantAdmittedTemplate
                {
                    id = p.Id,
                    documentType = ConstantHelpers.DOCUMENT_TYPES.VALUES[p.DocumentType],
                    paternalSurname = p.PaternalSurname,
                    maternalSurname = p.MaternalSurname,
                    name = p.Name,
                    document = p.Document,
                    sex = ConstantHelpers.SEX.VALUES[p.Sex],
                    birthDate = p.BirthDate.ToLocalDateFormat(),
                    discapacity = p.HasDiscapacity ? "SI" : "NO",
                    discapacityType = p.DiscapacityType,
                    applicationTerm = $"{p.ApplicationTerm.Term.Name} - {p.ApplicationTerm.Name}",
                    campus = p.Campus.Name,
                    career = p.Career.Name,
                    admissionType = p.AdmissionType.Name,
                    score = p.FinalScore,
                    accepted = p.Accepted ? "SI" : "NO",
                    acceptedcareer = p.Accepted ? p.Career.Name : "",
                    ethnicId = "",
                    email = p.Student != null ? p.Student.User.Email : "",
                    personalemail = p.Email,
                    phone = p.Phone1,
                    phone2 = p.Phone2,
                    //
                    paidadmission = p.PaidAdmission,
                    code = p.Code
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
        public async Task<List<PostulantAdmittedTemplate>> GetPostulantAdmittedData(Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, string search = null)
        {
            var query = _context.Postulants
                  .Where(x => x.ApplicationTermId == applicationTermId)
                  .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (typeId.HasValue && typeId != Guid.Empty) query = query.Where(x => x.AdmissionTypeId == typeId);

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper()) || x.Document.Contains(search));

            var queryclient = await query
                .Select(p => new
                {
                    id = p.Id,
                    documentType = p.DocumentType,
                    paternalSurname = p.PaternalSurname,
                    maternalSurname = p.MaternalSurname,
                    name = p.Name,
                    document = p.Document,
                    sex = p.Sex,
                    birthDate = p.BirthDate,
                    discapacity = p.HasDiscapacity,
                    discapacityType = p.DiscapacityType,
                    //applicationTerm =$"{p.ApplicationTerm.Term.Name} - {p.ApplicationTerm.Name}",
                    campus = p.Campus.Name,
                    career = p.Career.Name,
                    admissionType = p.AdmissionType.Name,
                    score = p.FinalScore,
                    accepted = p.Accepted,
                    acceptedcareer = p.Accepted ? p.Career.Name : "",
                    ethnicId = "",
                    email = p.Student != null ? p.Student.User.Email : "",
                    personalemail = p.Email,
                    phone = p.Phone1,
                    phone2 = p.Phone2,
                    //
                    paidadmission = p.PaidAdmission,
                    code = p.Code
                })
              .ToListAsync();

            var data = queryclient
                .Select(p => new PostulantAdmittedTemplate
                {
                    id = p.id,
                    documentType = ConstantHelpers.DOCUMENT_TYPES.VALUES[p.documentType],
                    paternalSurname = p.paternalSurname,
                    maternalSurname = p.maternalSurname,
                    name = p.name,
                    document = p.document,
                    sex = ConstantHelpers.SEX.VALUES[p.sex],
                    birthDate = p.birthDate.ToLocalDateFormat(),
                    discapacity = p.discapacity ? "SI" : "NO",
                    discapacityType = p.discapacityType,
                    //applicationTerm =p.applicationTerm,
                    campus = p.campus,
                    career = p.career,
                    admissionType = p.admissionType,
                    score = p.score,
                    accepted = p.accepted ? "SI" : "NO",
                    acceptedcareer = p.acceptedcareer,
                    ethnicId = "",
                    email = p.email,
                    personalemail = p.personalemail,
                    phone = p.phone,
                    phone2 = p.phone2,
                    //
                    paidadmission = p.paidadmission,
                    code = p.code
                })
                .ToList();
            return data;
        }
        public async Task<bool> ValidationByVoucher(string voucher)
        {
            return await _context.Postulants.AnyAsync(x => x.Voucher == voucher);
        }

        public async Task<bool> ValidationByDNI(string document)
        {
            return await _context.Postulants.AnyAsync(x => x.Document == document);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsByAppTermIdAndAdmissionExamIdDataTable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid admissionExamId, string search = null)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.ApplicationTerm.Term.Name;
                    break;
                default:
                    break;
            }

            var query = _context.Postulants
                .Where(x => x.ApplicationTerm.AdmissionExamApplicationTerms.Any(y => y.AdmissionExamId == admissionExamId))
                .AsNoTracking();

            var postulantgrades = _context.AdmissionExamPostulantGrades.Where(x => x.AdmissionExamId == admissionExamId);

            if (applicationTermId != Guid.Empty)
                query = query.Where(x => x.ApplicationTermId == applicationTermId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper()) || x.Document.Contains(search));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .OrderBy(x => x.Career.Faculty.Name).ThenBy(x => x.Career.Name).ThenBy(x => x.AdmissionType.Name).ThenBy(x => x.FullName)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(p => new
                {
                    code = p.Code,
                    id = p.Id,
                    dni = p.Document,
                    name = $"{p.Name} {p.PaternalSurname} {p.MaternalSurname}",
                    career = p.Career.Name,
                    faculty = p.Career.Faculty.Name,
                    admissionState = ConstantHelpers.Admission.Postulant.AdmissionState.VALUES[p.AdmissionState],
                    admissionType = p.AdmissionType.Name,
                    grade = postulantgrades.FirstOrDefault(c => c.PostulantId == p.Id) != null ? postulantgrades.FirstOrDefault(c => c.PostulantId == p.Id).Grade : 0.0M
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

        public async Task<Postulant> GetPostulantByCode(string postulantcode, Guid? applicationTermId = null)
        {
            var query = _context.Postulants
                .Where(x => x.Code == postulantcode);

            if (applicationTermId.HasValue && applicationTermId != Guid.Empty) query = query.Where(x => x.ApplicationTermId == applicationTermId);
            else query = query.Where(x => x.ApplicationTerm.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var postulant = await query.Include(x => x.AdmissionType).FirstOrDefaultAsync();
            return postulant;
        }

        public async Task RecalculateFinalScore(Guid applicationTermId)
        {
            var postulants = await _context.Postulants
                .Where(x => x.ApplicationTermId == applicationTermId)
                .ToListAsync();

            var careers = postulants
                .Select(x => x.CareerId)
                .Distinct().ToList();

            var grades = await _context.AdmissionExamPostulantGrades
                .Where(x => x.Postulant.ApplicationTermId == applicationTermId)
                .ToListAsync();

            var exams = await _context.AdmissionExams
                .Where(x => x.AdmissionExamApplicationTerms.Any(y => y.ApplicationTermId == applicationTermId))
                .ToListAsync();

            foreach (var career in careers)
            {
                var admissionTypeOrder = new Dictionary<Guid, int>();
                var acceptedPostulants = new List<Postulant>();

                var vacants = await _context.Vacants
                    .Where(x => x.CareerApplicationTerm.ApplicationTermId == applicationTermId
                    && x.CareerApplicationTerm.CareerId == career)
                    .Select(x => new
                    {
                        x.AdmissionTypeId,
                        x.Number
                    }).ToDictionaryAsync(v => v.AdmissionTypeId, v => v.Number);

                foreach (var postulant in postulants.Where(x => x.CareerId == career).OrderByDescending(x => x.FinalScore).ToList())
                {
                    if (!admissionTypeOrder.ContainsKey(postulant.AdmissionTypeId)) admissionTypeOrder.Add(postulant.AdmissionTypeId, 1);

                    postulant.OrderMerit = admissionTypeOrder[postulant.AdmissionTypeId];

                    //IF TOP DOWN 
                    if (vacants.ContainsKey(postulant.AdmissionTypeId) && vacants[postulant.AdmissionTypeId] > 0)
                    {
                        vacants[postulant.AdmissionTypeId]--;
                        acceptedPostulants.Add(postulant);
                        postulant.AdmissionState = ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED;
                    }
                    else
                    {
                        if (acceptedPostulants.Any(x => x.FinalScore == postulant.FinalScore && x.AdmissionTypeId == postulant.AdmissionTypeId))
                        {
                            var prevPostulants = acceptedPostulants
                                .Where(x => x.FinalScore == postulant.FinalScore && x.AdmissionTypeId == postulant.AdmissionTypeId)
                                .ToList();

                            foreach (var item in prevPostulants) item.AdmissionState = ConstantHelpers.Admission.Postulant.AdmissionState.MANUAL_APPROVAL;

                            postulant.AdmissionState = ConstantHelpers.Admission.Postulant.AdmissionState.MANUAL_APPROVAL;
                        }
                        else postulant.AdmissionState = ConstantHelpers.Admission.Postulant.AdmissionState.NOT_ADMITTED;
                    }

                    admissionTypeOrder[postulant.AdmissionTypeId]++;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Postulant>> GetPostulantsByFilter(Guid applicationTermId, bool? hasDiscapacity, bool? hasPhoto, string searchValue = null)
        {
            var query = _context.Postulants
                .Where(x => x.ApplicationTermId == applicationTermId)
                .AsQueryable();

            if (hasDiscapacity.HasValue)
                query = query.Where(x => x.HasDiscapacity == hasDiscapacity);

            if (hasPhoto.HasValue)
            {
                if (hasPhoto.Value)
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.Picture));
                }
                else
                {
                    query = query.Where(x => string.IsNullOrEmpty(x.Picture));
                }
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //    query = query.Where(x => x.FullName.ToLower().Contains(searchValue.ToLower().Trim()));

            if (!string.IsNullOrEmpty(searchValue)) query = query.Where(x => x.PaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
            x.MaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
            x.Name.ToUpper().Contains(searchValue.ToUpper()) ||
            x.Document.Contains(searchValue));

            var result = await query.Include(x => x.Career.Faculty).Include(x => x.AdmissionType).ToArrayAsync();
            return result;
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetCareersWithManualApprovalDatatable(Guid applicationTermId)
        {
            var postulants = await _context.Postulants
              .Where(x => x.ApplicationTermId == applicationTermId
              && x.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.MANUAL_APPROVAL)
              .Select(x => new
              {
                  applicationTermId = x.ApplicationTermId,
                  applicationTerm = x.ApplicationTerm.Name,
                  careerId = x.CareerId,
                  career = x.Career.Name,
                  admissionTypeId = x.AdmissionTypeId,
                  admissionType = x.AdmissionType.Name
              }).ToListAsync();

            var data = postulants
                .GroupBy(x => new { x.applicationTermId, x.careerId, x.admissionTypeId })
                .Select(x => new
                {
                    x.Key.applicationTermId,
                    x.Key.careerId,
                    x.Key.admissionTypeId,

                    x.FirstOrDefault().applicationTerm,
                    x.FirstOrDefault().career,
                    x.FirstOrDefault().admissionType,

                    postulants = x.Count()
                }).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsWithManualApprovalDatatable(Guid applicationTermId, Guid careerId, Guid admissionTypeId)
        {
            var data = await _context.Postulants
                         .Where(x => x.ApplicationTermId == applicationTermId
                         && x.CareerId == careerId && x.AdmissionTypeId == admissionTypeId
                         && x.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.MANUAL_APPROVAL)
                         .Select(x => new
                         {
                             code = x.Code,
                             name = x.FullName,
                             score = x.FinalScore,
                             id = x.Id,
                             careerId = x.CareerId,
                             career = x.Career.Name
                         }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            };
        }

        public async Task<List<Postulant>> GetAllPostulantsWithFolders()
        {
            var admissionfolders = await _context.Prospects.Where(x => x.Status == ConstantHelpers.STATES.ACTIVE).ToListAsync();
            var postulants = _context.Postulants.Where(x => x.AdmissionFolder.HasValue).AsQueryable();

            var result = postulants.AsEnumerable().Where(x => admissionfolders.Any(y => x.AdmissionFolder.Value >= y.Start && x.AdmissionFolder.Value <= y.End));
            return result.ToList();
        }
        private class RangeItem
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public string MinMax => $"{Min} - {Max}";
            public int Count { get; set; }
        }
        public async Task<object> GetPostulantByAgeRangeChart(Guid? facultyId, Guid termId, Guid? applicationtermId, byte? admissionState = null, List<Guid> applicationTermsId = null)
        {
            var query = _context.Postulants.AsQueryable();

            if (admissionState.HasValue) query = query.Where(x => x.AdmissionState == admissionState);

            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(q => q.Career.FacultyId == facultyId.Value);
            if (termId != Guid.Empty) query = query.Where(q => q.ApplicationTerm.TermId == termId);

            if (applicationtermId.HasValue && applicationtermId != Guid.Empty) query = query.Where(q => q.ApplicationTermId == applicationtermId);

            if (applicationTermsId != null && applicationTermsId.Any())
            {
                query = query.Where(x => applicationTermsId.Contains(x.ApplicationTermId));
            }

            var ranges = new List<RangeItem>() {
                new RangeItem{ Min=0, Max=15 },
                new RangeItem{ Min=16, Max=20},
                new RangeItem{ Min=21, Max=25 },
                new RangeItem{ Min=26, Max=30 },
                new RangeItem{ Min=31, Max=35 },
                new RangeItem{ Min=36, Max=40 },
                new RangeItem{ Min=40, Max=100},
            };

            var postualnts = query.AsEnumerable();

            var result = ranges.Select(x => new RangeItem
            {
                Max = x.Max,
                Min = x.Min,
                Count = postualnts.Where(y => y.Age >= x.Min && y.Age <= x.Max).Count()
            }).ToList();

            var parentResult = result
                .Select(x => new
                {
                    name = x.MinMax,
                    y = x.Count,
                    drilldown = x.MinMax
                }).ToList();

            var details = new List<ReportByCareerTemplate>();
            foreach (var item in result)
            {
                var detail = new ReportByCareerTemplate();
                detail.Id = item.MinMax;
                detail.Name = item.MinMax;
                var objectDetails = new List<object[]>();

                for (int i = item.Min; i <= item.Max; i++)
                {
                    var qty = postualnts.Where(y => y.Age == i).Count();

                    objectDetails.Add(new object[] { $"{i} años", qty });
                }
                detail.Data = objectDetails;
                details.Add(detail);
            }

            return new { parentResult, details };
        }


        public async Task ProcessOrdinaryPostulantsResults(Guid applicationtermId)
        {
            var postulants = await _context.Postulants
                .Where(x => x.ApplicationTermId == applicationtermId /*&& x.AdmissionType.Type == ConstantHelpers.ADMISSION_MODE.ORDINARY*/ && x.IsVerified && x.PaidAdmission)
                .ToListAsync();

            //calculo de orden de merito general
            var prevScore = -99.0M;
            var orderMerit = 0;
            postulants = postulants.OrderByDescending(x => x.FinalScore).ToList();
            foreach (var item in postulants)
            {
                if (item.FinalScore != prevScore)
                {
                    prevScore = item.FinalScore;
                    orderMerit++;
                }

                item.OrderMerit = orderMerit;
            }

            //calculo de orden de merito por escuela
            var careers = postulants.Select(x => x.CareerId).Distinct().ToList();
            foreach (var career in careers)
            {
                var careerPostulants = postulants.Where(x => x.CareerId == career).ToList();
                var careerPrevScore = -99.0M;
                var careerOrderMerit = 0;

                foreach (var item in careerPostulants)
                {
                    if (item.FinalScore != careerPrevScore)
                    {
                        careerPrevScore = item.FinalScore;
                        careerOrderMerit++;
                    }

                    item.OrderMeritBySchool = careerOrderMerit;
                }
            }

            postulants.ForEach(x => x.AdmissionState = ConstantHelpers.Admission.Postulant.AdmissionState.NOT_ADMITTED);

            var vacants = await _context.Vacants
                .Where(x => x.CareerApplicationTerm.ApplicationTermId == applicationtermId)
                .Select(x => new
                {
                    x.CareerApplicationTerm.CareerId,
                    x.AdmissionTypeId,
                    x.Number,
                    x.CareerApplicationTerm.CampusId,
                    x.AcademicProgramId
                }).ToListAsync();

            var tiebreakerModality = await GetIntConfigurationValue(ConstantHelpers.Configuration.AdmissionManagement.ADMISSION_TIEBREAKER_MODALITY);

            foreach (var vacant in vacants)
            {
                var qry = postulants.Where(x => x.CareerId == vacant.CareerId && x.AdmissionTypeId == vacant.AdmissionTypeId).AsQueryable();
                if (vacant.CampusId.HasValue) qry = qry.Where(x => x.CampusId == vacant.CampusId);
                if (vacant.AcademicProgramId.HasValue) qry = qry.Where(x => x.AcademicProgramId == vacant.AcademicProgramId);

                var vacantPostulants = qry.OrderByDescending(x => x.FinalScore).ToList();

                var groupByScore = vacantPostulants
                    .GroupBy(x => x.FinalScore)
                    .Select(x => new
                    {
                        FinalScore = x.Key,
                        Postulants = x.Select(y => y.Id).ToList()
                    })
                    .OrderByDescending(x => x.FinalScore).ToList();

                var count = 0;
                var admittedPostulants = new List<Guid>();
                var manualPostulants = new List<Guid>();

                foreach (var group in groupByScore)
                {
                    if (vacant.Number >= count + group.Postulants.Count)
                    {
                        //si aun hay vacantes para todo el grupo. ingresan todos
                        count += group.Postulants.Count;
                        admittedPostulants.AddRange(group.Postulants);
                    }
                    else if (vacant.Number > count)
                    {
                        //si aun hay vacantes pero el grupo es mayor a la cantidad disponible
                        count += group.Postulants.Count;
                        //se verifica configuracion de desempate
                        if (tiebreakerModality == 1)
                        {
                            //ingresan todos
                            admittedPostulants.AddRange(group.Postulants);
                        }
                        else
                        {
                            //desempate manual
                            manualPostulants.AddRange(group.Postulants);
                        }
                    }
                }

                foreach (var postulant in vacantPostulants)
                {
                    if (admittedPostulants.Contains(postulant.Id))
                        postulant.AdmissionState = ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED;
                    if (manualPostulants.Contains(postulant.Id))
                        postulant.AdmissionState = ConstantHelpers.Admission.Postulant.AdmissionState.MANUAL_APPROVAL;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSiriesResult(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid termId, List<Guid> applicationTermsIds = null)
        {
            var query = _context.Postulants.AsQueryable();

            query = query.Where(x => x.ApplicationTerm.TermId == termId);

            if (applicationTermId != Guid.Empty)
            {
                query = query.Where(x => x.ApplicationTermId == applicationTermId);
            }

            if (applicationTermsIds != null && applicationTermsIds.Any())
            {
                query = query.Where(p => applicationTermsIds.Contains(p.ApplicationTermId));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
            //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
            .Skip(sentParameters.PagingFirstRecord)
            .Take(sentParameters.RecordsPerDraw)
            .Select(post => new
            {
                id = post.Id,
                documentType = ConstantHelpers.DOCUMENT_TYPES.VALUES[post.DocumentType],
                document = post.Document,
                paternalSurName = post.PaternalSurname,
                maternalSurName = post.MaternalSurname,
                fullName = post.Name,
                gender = ConstantHelpers.SEX.VALUES.ContainsKey(post.Sex) ? ConstantHelpers.SEX.VALUES[post.Sex] : "--",
                birthday = post.BirthDate.ToLocalDateFormat(),
                hasDiscapacity = post.HasDiscapacity ? "SI" : "NO",
                discapacityType = post.DiscapacityType ?? "--",
                careerPostulant = post.Career.Name.ToUpper(),
                careerPostulant2 = "",
                campus = post.Campus.Name,
                term = post.ApplicationTerm.Name,
                studyModality = "",
                admissionModalityType = post.AdmissionType.Name,
                score = post.FinalScore,
                admitted = post.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED ? "SI" : "NO",
                careerAdmitted = post.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED ? post.Career.Name : "--",
                email = post.Email,
                institutionalEmail = "",
                ethnicIdentity = ConstantHelpers.Student.RacialIdentity.VALUES.ContainsKey(post.RacialIdentity) ? ConstantHelpers.Student.RacialIdentity.VALUES[post.RacialIdentity] : ConstantHelpers.Student.RacialIdentity.VALUES[ConstantHelpers.Student.RacialIdentity.OTHER],
                phone1 = post.Phone1,
                phone2 = post.Phone2

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

        public async Task<object> GetTotalNumberOfEntrantsOfTheAdmissionExamByCareerAsData(Guid? termId = null, Guid? careerId = null, Guid? admissionTypeId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null)
        {

            var postulants = _context.Postulants
                .Where(x => x.AdmissionState == ConstantHelpers.POSTULANTS.ApplicationStatus.ADMITTED)
                .AsQueryable();

            var careersQuery = _context.Careers.AsNoTracking();

            if (termId != null)
                postulants = postulants.Where(x => x.ApplicationTerm.TermId == termId);

            if (admissionTypeId != null)
                postulants = postulants.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    postulants = postulants.Where(x => x.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }


            if (careerId != null)
                postulants = postulants.Where(x => x.CareerId == careerId);

            if (academicProgramId != null)
                postulants = postulants.Where(x => x.AcademicProgramId == academicProgramId);

            var careers = await careersQuery
                .Select(x => new
                {
                    Career = x.Name,
                    Accepted = postulants.Where(y => y.CareerId == x.Id).Count()
                }).ToListAsync();

            var result = new
            {
                categories = careers.Select(x => x.Career).ToList(),
                data = careers.Select(x => x.Accepted).ToList()
            };

            return result;
        }

        public async Task<object> GetTotalNumberOfPostulantsOfTheAdmissionExamByCareerAsData(Guid? termId = null, Guid? careerId = null, Guid? admissionTypeId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null)
        {
            var postulants = _context.Postulants
                .AsQueryable();

            var careersQuery = _context.Careers.AsNoTracking();

            if (termId != null)
                postulants = postulants.Where(x => x.ApplicationTerm.TermId == termId);

            if (admissionTypeId != null)
                postulants = postulants.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    postulants = postulants.Where(x => x.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }


            if (careerId != null)
                postulants = postulants.Where(x => x.CareerId == careerId);

            if (academicProgramId != null)
                postulants = postulants.Where(x => x.AcademicProgramId == academicProgramId);

            var careers = await careersQuery
                .Select(x => new
                {
                    Career = x.Name,
                    Accepted = postulants.Where(y => y.CareerId == x.Id).Count()
                }).ToListAsync();

            var result = new
            {
                categories = careers.Select(x => x.Career).ToList(),
                data = careers.Select(x => x.Accepted).ToList()
            };

            return result;
        }


        public async Task<List<Postulant>> GetSiriesExcelResult(Guid applicationTermId, Guid termId)
        {
            var query = _context.Postulants
                .Include(x => x.Campus)
                .Include(x => x.Career).ThenInclude(x => x.Faculty)
                .Include(x => x.AdmissionType)
                .Include(x => x.ApplicationTerm)
                .Include(x => x.BirthCountry)
                .Include(x => x.NationalityCountry)
                .Include(x => x.Student.User)
                .AsNoTracking();

            if (termId != Guid.Empty)
                query = query.Where(p => p.ApplicationTerm.TermId == termId);

            if (applicationTermId == Guid.Empty && termId == Guid.Empty)
            {
                var active = await _context.ApplicationTerms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
                if (active != null) applicationTermId = active.Id;
            }

            if (applicationTermId != Guid.Empty)
                query = query.Where(p => p.ApplicationTermId == applicationTermId);


            return await query.ToListAsync();
        }


        public async Task<Postulant> CreatePostulantFromStudent(Guid studentId)
        {
            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.User.Name,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    x.User.Document,
                    x.AdmissionTypeId,
                    x.AdmissionTermId,
                    x.User.DepartmentId,
                    x.User.ProvinceId,
                    x.User.DistrictId,
                    x.CareerId,
                    x.User.Sex,
                    x.User.BirthDate,
                    x.User.CivilStatus,
                    x.User.Address,
                    x.User.Email,
                    x.CampusId
                }).FirstOrDefaultAsync();

            var applicationTerm = await _context.ApplicationTerms
                .FirstOrDefaultAsync(x => x.TermId == student.AdmissionTermId);

            if (applicationTerm == null)
            {
                var term = await _context.Terms.FindAsync(student.AdmissionTermId);

                applicationTerm = new ApplicationTerm
                {
                    Name = "Proceso de Addmisión",
                    TermId = student.AdmissionTermId,

                    StartDate = term.StartDate,
                    EndDate = term.EndDate,
                    InscriptionStartDate = term.StartDate,
                    InscriptionEndDate = term.EndDate,
                    PublicationDate = term.EndDate,
                    Status = ConstantHelpers.TERM_STATES.FINISHED
                };
                await _context.ApplicationTerms.AddAsync(applicationTerm);
                await _context.SaveChangesAsync();
            }

            var district = await _context.Districts.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
            var province = await _context.Provinces.Where(x => x.Id == district.ProvinceId).FirstOrDefaultAsync();
            var department = await _context.Departments.Where(x => x.Id == province.DepartmentId).FirstOrDefaultAsync();
            var country = await _context.Countries.Where(x => x.Id == department.CountryId).FirstOrDefaultAsync();

            var campuses = await _context.Campuses.ToListAsync();

            var postulant = new Postulant
            {
                Code = student.Document,
                AdmissionTypeId = student.AdmissionTypeId,
                ApplicationTerm = applicationTerm,

                BirthCountryId = country.Id,
                BirthDepartmentId = student.DepartmentId,
                BirthDistrictId = student.DistrictId,
                BirthProvinceId = student.ProvinceId,

                SecondaryEducationDepartmentId = student.DepartmentId.HasValue ? student.DepartmentId.Value : department.Id,
                SecondaryEducationProvinceId = student.ProvinceId.HasValue ? student.ProvinceId.Value : province.Id,
                SecondaryEducationDistrictId = student.DistrictId.HasValue ? student.DistrictId.Value : district.Id,

                CareerId = student.CareerId,

                DistrictId = student.DistrictId.HasValue ? student.DistrictId.Value : district.Id,
                ProvinceId = student.ProvinceId.HasValue ? student.ProvinceId.Value : province.Id,
                DepartmentId = student.DepartmentId.HasValue ? student.DepartmentId.Value : department.Id,
                NationalityCountryId = country.Id,

                Name = student.Name,
                PaternalSurname = student.PaternalSurname,
                MaternalSurname = student.MaternalSurname,

                Document = student.Document.ToString(),
                Sex = student.Sex,
                BirthDate = student.BirthDate,
                MaritalStatus = student.CivilStatus,
                Address = student.Address,
                Email = student.Email,
                WorkingCurrently = false,

                SecondaryEducationName = "---",
                SecondaryEducationType = ConstantHelpers.SECONDARY_EDUCATION_TYPE.PUBLIC,
                SecondaryEducationFinished = ConstantHelpers.FINISHED_SECONDARY_EDUCATION.YES,
                Representative = ConstantHelpers.REPRESENTATIVE_TYPE.NONE,

                Childrens = 0,
                BroadcastMedium = ConstantHelpers.BROADCAST_MEDIUM.INTERNET,
                AdmissionState = ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED,
                RegisterDate = DateTime.UtcNow,
                HasOtherSuperiorEducation = false,
                SuperiorEducationStarts1 = DateTime.UtcNow,
                HasTwoSuperiorEducations = false,
                SuperiorEducationStarts2 = DateTime.UtcNow,
                SecondaryEducationStarts = ConvertHelpers.DatepickerToDatetime("01/01/2010"),
                SecondaryEducationEnds = ConvertHelpers.DatepickerToDatetime("10/12/2014"),
                CampusId = student.CampusId.HasValue ? student.CampusId.Value : campuses.FirstOrDefault().Id,

                StudentId = studentId
            };


            await _context.Postulants.AddAsync(postulant);
            await _context.SaveChangesAsync();

            return postulant;
        }

        public async Task Insert(Postulant postulant, bool generateCode)
        {
            if (!generateCode)
            {
                var postulantCodeFormat = await GetConfigurationValue(ConstantHelpers.Configuration.AdmissionManagement.POSTULANT_CODE_FORMAT);

                var applicationTerm = await _context.ApplicationTerms
                    .Where(x => x.Id == postulant.ApplicationTermId)
                    .Include(x => x.Term)
                    .FirstOrDefaultAsync();

                var year = applicationTerm.Term.Year.ToString();
                var period = applicationTerm.Term.Number ?? "0";

                var career = await _context.Careers
                    .Where(x => x.Id == postulant.CareerId)
                    .Select(x => new
                    {
                        x.Code,
                        FacultyCode = x.Faculty.Code
                    })
                    .FirstOrDefaultAsync();

                var admissionType = await _context.AdmissionTypes.Where(x => x.Id == postulant.AdmissionTypeId).FirstOrDefaultAsync();

                var correlative = 1;
                var lastCreatedPostulant = await _context.Postulants
                    .Where(x => x.ApplicationTermId == postulant.ApplicationTermId)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();

                var correlativeStart = postulantCodeFormat.IndexOf('r');
                var correlativeEnd = postulantCodeFormat.LastIndexOf('r');
                var correlativeLength = correlativeStart >= 0 ? correlativeEnd - correlativeStart + 1 : 0;

                if (lastCreatedPostulant != null)
                {
                    var text = lastCreatedPostulant.Code.Substring(correlativeStart, correlativeLength);
                    if (!string.IsNullOrEmpty(text))
                        correlative = int.Parse(lastCreatedPostulant.Code.Substring(correlativeStart, correlativeLength));
                }

                var userCode = GenerateStudentCode(postulantCodeFormat, year, period, career.FacultyCode, career.Code, admissionType.Abbreviation, postulant.RacialIdentity, correlative.ToString());

                //var additionalFormat = "";
                //if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA)
                //{
                //    var characters = "abcdefghijk";
                //    var rand = new Random();
                //    var index = rand.Next(characters.Length);
                //    additionalFormat = characters[index].ToString();
                //}
                //userCode += additionalFormat;

                var currentPostulants = await _context.Postulants.Where(x => x.ApplicationTermId == postulant.ApplicationTermId).Select(x => x.Code).ToListAsync();

                while (currentPostulants.Any(x => int.Parse(x.Substring(correlativeStart, correlativeLength)) == correlative))
                {
                    correlative++;
                    userCode = GenerateStudentCode(postulantCodeFormat, year, period, career.FacultyCode, career.Code, admissionType.Abbreviation, postulant.RacialIdentity, correlative.ToString());
                    //userCode += additionalFormat;
                }

                postulant.Code = userCode;
            }
            else
            {
                postulant.Code = postulant.Document;
            }

            await _context.Postulants.AddAsync(postulant);
            await _context.SaveChangesAsync();
        }

        public async Task<Postulant> GetPostulantByDocument(string document, Guid? applicationTermId = null)
        {
            var qry = _context.Postulants.Where(x => x.Document == document).AsQueryable();
            if (applicationTermId.HasValue && applicationTermId != Guid.Empty)
                qry = qry.Where(x => x.ApplicationTermId == applicationTermId);
            var postulant = await qry.FirstOrDefaultAsync();

            return postulant;
        }
        public async Task<object> GetApplicationTermsActiveByDni(string dni)
        {
            var applicationTerms = await _context.Postulants
                .Where(x => x.Document == dni && x.ApplicationTerm.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                .Select(x => x.ApplicationTermId)
                .ToListAsync();

            var activeApplicationTerms = await _context.ApplicationTerms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                .Select(x => new
                {
                    x.Id,
                    TermName = x.Term.Name,
                    ApplicationTermName = x.Name
                })
                .ToListAsync();

            var result = activeApplicationTerms
                .Where(x => applicationTerms.Any(y => y != x.Id))
                .Select(x => new
                {
                    x.Id,
                    text = $"{x.TermName} - {x.ApplicationTermName}"
                })
                .ToList();

            return result;
        }

        public async Task<bool> ExistPostulantOnApplicationTerm(string dni, Guid applicationTermId)
        {
            var result = await _context.Postulants.AnyAsync(x => x.Document == dni && x.ApplicationTermId == applicationTermId);

            return result;
        }

        public async Task DeletePostulantById(Guid id)
        {
            var requirements = await _context.PostulantAdmissionRequirements
                .Where(x => x.PostulantId == id)
                .ToListAsync();
            _context.PostulantAdmissionRequirements.RemoveRange(requirements);

            var languages = await _context.PostulantOriginalLanguages
                .Where(x => x.PostulantId == id)
                .ToListAsync();
            _context.PostulantOriginalLanguages.RemoveRange(languages);

            var parents = await _context.PostulantFamilies
                .Where(x => x.PostulantId == id)
                .ToListAsync();
            _context.PostulantFamilies.RemoveRange(parents);

            var relatedPayments = await _context.Payments
                .Where(x => x.EntityId == id)
                .ToListAsync();
            foreach (var item in relatedPayments)
            {
                item.WasBankPaymentUsed = false;
            }

            await DeleteById(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, string search = null)
        {
            var payments = await _context.Payments
                .Where(x => (!string.IsNullOrEmpty(x.OperationCodeB) && x.OperationCodeB.Contains(search)) || x.InvoiceId.HasValue)
                .Select(x => new
                {
                    x.EntityId,
                    x.OperationCodeB,
                    Invoice = x.InvoiceId.HasValue ? $"{x.Invoice.Series}-{x.Invoice.Number:00000000}" : "",
                    x.PaymentDate
                }).ToListAsync();

            payments = payments
                .Where(x => !string.IsNullOrEmpty(x.OperationCodeB) || x.Invoice.Contains(search))
                .ToList();

            var postulants = await _context.Postulants
                .Where(x => x.PaidAdmission)
                .Select(x => new PostulantPaymentTemplate
                {
                    ApplicationTerm = x.ApplicationTerm.Name,
                    Id = x.Id,
                    Document = x.Document,
                    Name = x.FullName,
                    AdmissionType = x.AdmissionType.Name
                })
                .ToListAsync();

            var data = new List<PostulantPaymentTemplate>();

            foreach (var item in payments)
            {
                var postulant = postulants.FirstOrDefault(x => x.Id == item.EntityId);

                if (postulant != null)
                {
                    postulant.OperationCode = string.IsNullOrEmpty(item.OperationCodeB) ?
                    item.Invoice : item.OperationCodeB;

                    postulant.PaymentDate = item.PaymentDate.Value;
                    postulant.PaymentDateText = item.PaymentDate.ToLocalDateFormat();

                    data.Add(postulant);
                }
            }

            var recordsFiltered = data.Count;

            //switch (sentParameters.OrderColumn)
            //{
            //    case "0":
            //        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
            //            data = data.OrderByDescending(x => x.ApplicationTerm).ToList();
            //        else data = data.OrderBy(x => x.ApplicationTerm).ToList();
            //        break;
            //    case "1":
            //        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
            //            data = data.OrderByDescending(x => x.Document).ToList();
            //        else data = data.OrderBy(x => x.Document).ToList();
            //        break;
            //    case "2":
            //        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
            //            data = data.OrderByDescending(x => x.Name).ToList();
            //        else data = data.OrderBy(x => x.Name).ToList();
            //        break;
            //    case "3":
            //        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
            //            data = data.OrderByDescending(x => x.AdmissionType).ToList();
            //        else data = data.OrderBy(x => x.AdmissionType).ToList(); 
            //        break;
            //    case "4":
            //        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
            //            data = data.OrderByDescending(x => x.OperationCode).ToList();
            //        else data = data.OrderBy(x => x.OperationCode).ToList();
            //        break;
            //    case "5":
            //        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
            //            data = data.OrderByDescending(x => x.PaymentDate).ToList();
            //        else data = data.OrderBy(x => x.PaymentDate).ToList();
            //        break;
            //    default:
            //        break;
            //}

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantAdmittedDatatable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, int? proofOfIncomeStatus = null, string searchValue = null)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Document;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.AdmissionType.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "6":
                    orderByPredicate = (x) => x.ProofOfIncomeStatus;
                    break;
            }


            var query = _context.Postulants
                            .Where(x => x.AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED)
                            .AsNoTracking();

            if (applicationTermId != null)
            {
                query = query.Where(p => p.ApplicationTermId == applicationTermId);
            }

            if (proofOfIncomeStatus != null)
            {
                query = query.Where(x => x.ProofOfIncomeStatus == proofOfIncomeStatus);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                var search = searchValue.ToUpper().Trim();

                query = query.Where(x => x.PaternalSurname.ToUpper().Contains(search) ||
                                        x.MaternalSurname.ToUpper().Contains(search) ||
                                        x.Name.ToUpper().Contains(search) ||
                                        x.Document.ToUpper().Contains(search) ||
                                        x.Student.User.UserName.ToUpper().Contains(search));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    dni = x.Document,
                    x.FullName,
                    faculty = x.Career.Faculty.Name,
                    career = x.Career.Name,
                    admissionType = x.AdmissionType.Name,
                    accepted = x.Accepted,
                    code = x.ProofOfIncomeStatus == ConstantHelpers.PROOF_OF_INCOME_STATUS.NON_PICKED_UP ? "---" : (x.Student != null ? x.Student.User.UserName : "---"),
                    proofOfIncomeStatusText = ConstantHelpers.PROOF_OF_INCOME_STATUS.VALUES.ContainsKey(x.ProofOfIncomeStatus) ?
                        ConstantHelpers.PROOF_OF_INCOME_STATUS.VALUES[x.ProofOfIncomeStatus] : "-",
                    proofOfIncomeStatus = x.ProofOfIncomeStatus
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

        public async Task<int> GetPostulantTriesByDocument(string document)
        {
            var count = await _context.Postulants
                .Where(x => x.Document == document)
                .CountAsync();

            return count;
        }
    }
}