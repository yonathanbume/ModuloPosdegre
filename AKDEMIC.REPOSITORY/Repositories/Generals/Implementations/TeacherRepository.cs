using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Templates;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherDedication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;


namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(AkdemicContext context) : base(context)
        {
        }

        #region PRIVATE
        //No Eliminar :c
        private async Task<DataTablesStructs.ReturnedData<object>> GetDoctorDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, bool onlyTeachers = true, ClaimsPrincipal user = null, string expeditionDate = null, Guid? countryId = null, Guid? institutionId = null, Guid? termId = null)
        {
            var query = _context.WorkerDoctoralDegrees
                        .AsNoTracking();

            Expression<Func<WorkerDoctoralDegree, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "3":
                    orderByPredicate = ((x) => x.Specialty); break;
                case "4":
                    orderByPredicate = ((x) => x.ExpeditionDate); break;
                case "5":
                    orderByPredicate = ((x) => x.StudyCountry.Name); break;
                case "6":
                    orderByPredicate = ((x) => x.Institution.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.InstitutionType); break;
            }

            if (onlyTeachers)
            {
                var teahers = _context.Teachers.AsNoTracking();

                if (user != null)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                    {
                        teahers = teahers.Where(x => x.Career.QualityCoordinatorId == userId);
                    }
                }

                query = query.Where(x => teahers.Any(y => y.UserId == x.UserId));
            }

            if (!string.IsNullOrEmpty(expeditionDate))
                query = query.Where(x => x.ExpeditionDate >= ConvertHelpers.DatepickerToUtcDateTime(expeditionDate));

            if (countryId != null)
                query = query.Where(x => x.StudyCountryId == countryId);

            if (institutionId != null)
                query = query.Where(x => x.InstitutionId == institutionId);

            if(termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
            }


            if (!string.IsNullOrEmpty(searchValue))
            {
                var search = searchValue.Trim();

                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                            || x.User.FullName.ToUpper().Contains(search.ToUpper())
                            || x.User.Name.ToUpper().Contains(search.ToUpper())
                            || x.User.PaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.User.MaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.Specialty.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.UserId,
                    username = x.User.UserName,
                    fullname = x.User.FullName,
                    email = x.User.Email,
                    specialty = x.Specialty,
                    expeditionDate = x.ExpeditionDate.ToLocalDateFormat() ?? "-",
                    country = x.StudyCountry.Name,
                    institution = x.Institution.Name,
                    institutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        private async Task<DataTablesStructs.ReturnedData<object>> GetMasterDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, bool onlyTeachers = true, ClaimsPrincipal user = null, string expeditionDate = null, Guid? countryId = null, Guid? institutionId = null, Guid? termId = null)
        {
            var query = _context.WorkerMasterDegrees
                        .AsNoTracking();

            Expression<Func<WorkerMasterDegree, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "3":
                    orderByPredicate = ((x) => x.Specialty); break;
                case "4":
                    orderByPredicate = ((x) => x.ExpeditionDate); break;
                case "5":
                    orderByPredicate = ((x) => x.StudyCountry.Name); break;
                case "6":
                    orderByPredicate = ((x) => x.Institution.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.InstitutionType); break;
            }

            if (onlyTeachers)
            {
                var teahers = _context.Teachers.AsNoTracking();
                if (user != null)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                    {
                        teahers = teahers.Where(x => x.Career.QualityCoordinatorId == userId);
                    }
                }
                query = query.Where(x => teahers.Any(y => y.UserId == x.UserId));
            }


            if (!string.IsNullOrEmpty(expeditionDate))
                query = query.Where(x => x.ExpeditionDate >= ConvertHelpers.DatepickerToUtcDateTime(expeditionDate));

            if (countryId != null)
                query = query.Where(x => x.StudyCountryId == countryId);

            if (institutionId != null)
                query = query.Where(x => x.InstitutionId == institutionId);

            if(termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
            }


            if (!string.IsNullOrEmpty(searchValue))
            {
                var search = searchValue.Trim();

                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                            || x.User.FullName.ToUpper().Contains(search.ToUpper())
                            || x.User.Name.ToUpper().Contains(search.ToUpper())
                            || x.User.PaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.User.MaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.Specialty.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.UserId,
                    username = x.User.UserName,
                    fullname = x.User.FullName,
                    email = x.User.Email,
                    specialty = x.Specialty,
                    expeditionDate = x.ExpeditionDate.ToLocalDateFormat() ?? "-",
                    country = x.StudyCountry.Name,
                    institution = x.Institution.Name,
                    institutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }


        private async Task<DataTablesStructs.ReturnedData<object>> GetProfessionalDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, bool onlyTeachers = true, ClaimsPrincipal user = null, string expeditionDate = null, Guid? countryId = null, Guid? institutionId = null, Guid? termId = null)
        {
            var query = _context.WorkerProfessionalTitles
                        .AsNoTracking();

            Expression<Func<WorkerProfessionalTitle, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "3":
                    orderByPredicate = ((x) => x.Specialty); break;
                case "4":
                    orderByPredicate = ((x) => x.ExpeditionDate); break;
                case "5":
                    orderByPredicate = ((x) => x.StudyCountry.Name); break;
                case "6":
                    orderByPredicate = ((x) => x.Institution.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.InstitutionType); break;
            }

            if (onlyTeachers)
            {
                var teahers = _context.Teachers.AsNoTracking();
                if (user != null)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                    {
                        teahers = teahers.Where(x => x.Career.QualityCoordinatorId == userId);
                    }
                }
                query = query.Where(x => teahers.Any(y => y.UserId == x.UserId));
            }


            if (!string.IsNullOrEmpty(expeditionDate))
                query = query.Where(x => x.ExpeditionDate >= ConvertHelpers.DatepickerToUtcDateTime(expeditionDate));

            if (countryId != null)
                query = query.Where(x => x.StudyCountryId == countryId);

            if (institutionId != null)
                query = query.Where(x => x.InstitutionId == institutionId);

            if (termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                var search = searchValue.Trim();

                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                            || x.User.FullName.ToUpper().Contains(search.ToUpper())
                            || x.User.Name.ToUpper().Contains(search.ToUpper())
                            || x.User.PaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.User.MaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.Specialty.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.UserId,
                    username = x.User.UserName,
                    fullname = x.User.FullName,
                    email = x.User.Email,
                    expeditionDate = x.ExpeditionDate.ToLocalDateFormat() ?? "-",
                    country = x.StudyCountry.Name,
                    specialty = x.Specialty,
                    institution = x.Institution.Name,
                    institutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        private async Task<DataTablesStructs.ReturnedData<object>> GetBachelorDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, bool onlyTeachers = true, ClaimsPrincipal user = null, string expeditionDate = null, Guid? countryId = null, Guid? institutionId = null, Guid? termId = null)
        {
            var query = _context.WorkerBachelorDegrees
                        .AsNoTracking();

            Expression<Func<WorkerBachelorDegree, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "3":
                    orderByPredicate = ((x) => x.Specialty); break;
                case "4":
                    orderByPredicate = ((x) => x.ExpeditionDate); break;
                case "5":
                    orderByPredicate = ((x) => x.StudyCountry.Name); break;
                case "6":
                    orderByPredicate = ((x) => x.Institution.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.InstitutionType); break;
            }

            if (onlyTeachers)
            {
                var teahers = _context.Teachers.AsNoTracking();
                if (user != null)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                    {
                        teahers = teahers.Where(x => x.Career.QualityCoordinatorId == userId);
                    }
                }
                query = query.Where(x => teahers.Any(y => y.UserId == x.UserId));
            }


            if (!string.IsNullOrEmpty(expeditionDate))
                query = query.Where(x => x.ExpeditionDate >= ConvertHelpers.DatepickerToUtcDateTime(expeditionDate));

            if (countryId != null)
                query = query.Where(x => x.StudyCountryId == countryId);

            if (institutionId != null)
                query = query.Where(x => x.InstitutionId == institutionId);


            if (termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
            }


            if (!string.IsNullOrEmpty(searchValue))
            {
                var search = searchValue.Trim();

                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                            || x.User.FullName.ToUpper().Contains(search.ToUpper())
                            || x.User.Name.ToUpper().Contains(search.ToUpper())
                            || x.User.PaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.User.MaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.Specialty.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.UserId,
                    username = x.User.UserName,
                    fullname = x.User.FullName,
                    email = x.User.Email,
                    expeditionDate = x.ExpeditionDate.ToLocalDateFormat() ?? "-",
                    country = x.StudyCountry.Name,
                    specialty = x.Specialty,
                    institution = x.Institution.Name,
                    institutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        private async Task<DataTablesStructs.ReturnedData<object>> GetTechnicalDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, bool onlyTeachers = true, ClaimsPrincipal user = null, string expeditionDate = null, Guid? countryId = null, Guid? institutionId = null, Guid? termId = null)
        {
            var query = _context.WorkerTechnicalStudies
                        .AsNoTracking();

            Expression<Func<WorkerTechnicalStudy, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "3":
                    orderByPredicate = ((x) => x.Specialty); break;
                case "4":
                    orderByPredicate = ((x) => x.ExpeditionDate); break;
                case "5":
                    orderByPredicate = ((x) => x.StudyCountry.Name); break;
                case "6":
                    orderByPredicate = ((x) => x.Institution.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.InstitutionType); break;
            }

            if (onlyTeachers)
            {
                var teahers = _context.Teachers.AsNoTracking();
                if (user != null)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                    {
                        teahers = teahers.Where(x => x.Career.QualityCoordinatorId == userId);
                    }
                }
                query = query.Where(x => teahers.Any(y => y.UserId == x.UserId));
            }


            if (!string.IsNullOrEmpty(expeditionDate))
                query = query.Where(x => x.ExpeditionDate >= ConvertHelpers.DatepickerToUtcDateTime(expeditionDate));

            if (countryId != null)
                query = query.Where(x => x.StudyCountryId == countryId);

            if (institutionId != null)
                query = query.Where(x => x.InstitutionId == institutionId);


            if (termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                var search = searchValue.Trim();

                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                            || x.User.FullName.ToUpper().Contains(search.ToUpper())
                            || x.User.Name.ToUpper().Contains(search.ToUpper())
                            || x.User.PaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.User.MaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.Specialty.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.UserId,
                    username = x.User.UserName,
                    fullname = x.User.FullName,
                    email = x.User.Email,
                    specialty = x.Specialty,
                    expeditionDate = x.ExpeditionDate.ToLocalDateFormat() ?? "-",
                    country = x.StudyCountry.Name,
                    institution = x.Institution.Name,
                    institutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        private async Task<DataTablesStructs.ReturnedData<object>> GetSchoolDegreesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, bool onlyTeachers = true, ClaimsPrincipal user = null, string expeditionDate = null, Guid? countryId = null, Guid? institutionId = null, Guid? termId = null)
        {
            var query = _context.WorkerSchoolDegrees
                        .AsNoTracking();

            Expression<Func<WorkerSchoolDegree, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "3":
                    orderByPredicate = ((x) => x.Specialty); break;
                case "4":
                    orderByPredicate = ((x) => x.ExpeditionDate); break;
                case "5":
                    orderByPredicate = ((x) => x.StudyCountry.Name); break;
                case "6":
                    orderByPredicate = ((x) => x.Institution.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.InstitutionType); break;
            }

            if (onlyTeachers)
            {
                var teahers = _context.Teachers.AsNoTracking();
                if (user != null)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                    {
                        teahers = teahers.Where(x => x.Career.QualityCoordinatorId == userId);
                    }
                }
                query = query.Where(x => teahers.Any(y => y.UserId == x.UserId));
            }


            if (!string.IsNullOrEmpty(expeditionDate))
                query = query.Where(x => x.ExpeditionDate >= ConvertHelpers.DatepickerToUtcDateTime(expeditionDate));

            if (countryId != null)
                query = query.Where(x => x.StudyCountryId == countryId);

            if (institutionId != null)
                query = query.Where(x => x.InstitutionId == institutionId);


            if (termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                var search = searchValue.Trim();

                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                            || x.User.FullName.ToUpper().Contains(search.ToUpper())
                            || x.User.Name.ToUpper().Contains(search.ToUpper())
                            || x.User.PaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.User.MaternalSurname.ToUpper().Contains(search.ToUpper())
                            || x.Specialty.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.UserId,
                    username = x.User.UserName,
                    fullname = x.User.FullName,
                    email = x.User.Email,
                    expeditionDate = x.ExpeditionDate.ToLocalDateFormat() ?? "-",
                    country = x.StudyCountry.Name,
                    specialty = ConstantHelpers.STUDY_TYPES.VALUES.ContainsKey(x.StudyType) ?
                            ConstantHelpers.STUDY_TYPES.VALUES[x.StudyType] : "-",
                    institution = x.Institution.Name,
                    institutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        #endregion

        #region PUBLIC
        public async Task<DataTablesStructs.ReturnedData<ConditionAndDedicationTemplate>> GetAllByConditionAndDedicationDataTable(DataTablesStructs.SentParameters sentParameters, Guid? condition, Guid? dedication, Guid? careerId, Guid? category, string search, string coordinatorId = null, Guid? academicDepartmentId = null, Guid? regimeId = null, int? maxStudyLevel = null)
        {
            var query = _context.Teachers
                .Where(x=>x.User.IsActive)
                .AsQueryable();

            if (maxStudyLevel.HasValue)
                query = query.Where(x => x.User.WorkerLaborInformation.MaxStudyLevel == maxStudyLevel);

            if (condition.HasValue && condition != Guid.Empty)
                query = query.Where(x => x.User.WorkerLaborInformation.WorkerLaborConditionId == condition.Value);

            if (dedication.HasValue && dedication != Guid.Empty)
                query = query.Where(x => x.TeacherDedicationId == dedication.Value);

            if (category.HasValue && category != Guid.Empty)
                query = query.Where(x => x.User.WorkerLaborInformation.WorkerLaborCategoryId == category);

            if (regimeId.HasValue && regimeId != Guid.Empty)
                query = query.Where(x => x.User.WorkerLaborInformation.WorkerLaborRegimeId == regimeId);

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                query = query.Where(x => x.CareerId == careerId.Value);
            }
            else
            {
                if (!string.IsNullOrEmpty(coordinatorId))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.AcademicCoordinatorId == coordinatorId ||
                    x.AcademicDepartment.Career.CareerDirectorId == coordinatorId ||
                    x.AcademicDepartment.Career.AcademicSecretaryId == coordinatorId ||
                    x.AcademicDepartment.AcademicDepartmentDirectorId == coordinatorId ||
                    x.AcademicDepartment.AcademicDepartmentCoordinatorId == coordinatorId ||
                    x.AcademicDepartment.AcademicDepartmentSecretaryId == coordinatorId);
                }
            }

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            var recordsFiltered = await query
                .Select(x => new ConditionAndDedicationTemplate
                {
                    id = x.User.Id,
                    names = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    maternalSurname = x.User.MaternalSurname,
                    userName = x.User.UserName,
                    career = x.Career.Name,
                    fullname = x.User.FullName,
                    condition = x.User.WorkerLaborInformation.WorkerLaborCondition.Name,
                    dedication = x.TeacherDedication.Name,
                    email = x.User.Email,
                    phoneNumber = x.User.PhoneNumber,
                    createdAt = x.CreatedAt,
                    category = x.User.WorkerLaborInformation.WorkerLaborCategory.Name,
                    AcademicDepartment = x.AcademicDepartment.Name
                }, search)
                .CountAsync();

            var data = await query
                .OrderBy(x => x.Career.Name)
                .ThenBy(x => x.User.FullName)
                .Select(x => new ConditionAndDedicationTemplate
                {
                    id = x.User.Id,
                    names = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    maternalSurname = x.User.MaternalSurname,
                    userName = x.User.UserName,
                    career = x.Career.Name,
                    fullname = x.User.FullName,
                    condition = x.User.WorkerLaborInformation.WorkerLaborCondition.Name,
                    dedication = x.TeacherDedication.Name,
                    email = x.User.Email,
                    phoneNumber = x.User.PhoneNumber,
                    createdAt = x.CreatedAt,
                    category = x.User.WorkerLaborInformation.WorkerLaborCategory.Name,
                    AcademicDepartment = x.AcademicDepartment.Name
                }, search)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ConditionAndDedicationTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<Select2Structs.ResponseParameters> GetTeachersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? careerId = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
        {
            var query = _context.Teachers
                .Include(x => x.User)
                //.Where(x => x.User.FullName.SplitContains(searchValue.Split(' ', StringSplitOptions.RemoveEmptyEntries)))
                //.WhereSearchValue((x) => new[] { x.User.FullName })
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.CareerDirectorId == userId || x.AcademicDepartment.Career.AcademicCoordinatorId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    query = query.Where(x => x.AcademicDepartment.AcademicDepartmentDirectorId == userId || x.AcademicDepartment.AcademicDepartmentSecretaryId == userId);
                }
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var academicDepartments = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartmentId).ToArrayAsync();
                    query = query.Where(x => x.AcademicDepartmentId.HasValue && academicDepartments.Contains(x.AcademicDepartmentId.Value));
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.ToLower().Contains(searchValue.Trim().ToLower()));
            }

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.UserId,
                Text = x.User.FullName
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<Select2Structs.ResponseParameters> GetFacultyTachersByAcademicProgramId(Select2Structs.RequestParameters requestParameters, Guid academicProgramId, string searchValue = null)
        {
            var facultyId = await _context.Faculties.Where(x => x.Careers.Any(y => y.AcademicPrograms.Any(z => z.Id == academicProgramId))).Select(x => x.Id).FirstOrDefaultAsync();
            var query = _context.Teachers
                           .Where(x => x.Career.FacultyId == facultyId)
               //.Where(x => x.User.FullName.SplitContains(searchValue.Split(' ', StringSplitOptions.RemoveEmptyEntries)))
               //.WhereSearchValue((x) => new[] { x.User.FullName })
               .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.Trim().ToLower().Contains(searchValue.Trim().ToLower()));
                //query = query.WhereSearchValue((x) => new[] { x.User.FullName }, searchValue);
            }

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.UserId,
                Text = x.User.FullName
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetTeachersSelect2ClientSide(Guid? facultyId = null, Guid? careerId = null)
        {
            var query = _context.Teachers
                .Include(x => x.User)
                .AsQueryable();

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            var result = await query.
                Select(x => new Select2Structs.Result
                {
                    Id = x.User.Id,
                    Text = x.User.FullName
                })
                .OrderBy(x => x.Text)
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetTeacherSelectByAcademicDepartment(Guid? facultyId = null, Guid? academicDepartmentId = null)
        {
            var query = _context.Teachers.AsNoTracking();

            if (facultyId != null)
                query = query.Where(x => x.AcademicDepartment.FacultyId == facultyId);

            if (academicDepartmentId != null)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            var result = await query
                .Select(x => new
                {
                    id = x.User.Id,
                    text = x.User.FullName
                })
                .OrderBy(x => x.text)
                .ToArrayAsync();

            return result;
        }



        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherDatatableByLevelStudy(DataTablesStructs.SentParameters sentParameters, int? levelStudy, string search = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.Email);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.TeacherDedication.Name);
                    break;
            }

            var query = _context.Teachers.AsQueryable();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.UserId,
                    x.User.FullName,
                    x.User.Email,
                    x.User.Dni,
                    teacherDedication = x.TeacherDedication.Name ?? "",
                    //levelStudy = string.Join(", " , x.TeacherInformation.TeacherInstructions.Select(x => x.LevelStudy).ToList())

                }, search)
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

        public async Task<object> CountByLevelStudy()
        {
            //var result = await _context.TeacherInstructions
            //    .GroupBy(x => x.LevelStudy)
            //    .Select(x => new
            //    {
            //        name = ConstantHelpers.TEACHER_LEVEL_STUDIE.VALUES[x.Key],
            //        total = x.Count()
            //    }).ToListAsync();
            return new { };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherDatatableByAgeSex(DataTablesStructs.SentParameters sentParameters, int? ageRange = null, int? sex = null, string search = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.Email);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);
                    break;
            }

            var today = DateTime.UtcNow.AddHours(-5);

            var query = _context.Teachers
                .AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (ageRange != null)
            {
                int minAge = ConstantHelpers.AGE_RANGES.VALUES[ageRange.Value].MinAge;
                int maxAge = ConstantHelpers.AGE_RANGES.VALUES[ageRange.Value].MaxAge;

                var minDateTime = today.AddYears(-maxAge);
                var maxDateTime = today.AddYears(-minAge);

                query = query.Where(q => q.User.BirthDate >= minDateTime && q.User.BirthDate <= maxDateTime);
            }

            if (sex != null)
                query = query.Where(q => q.User.Sex == sex.Value);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.UserId,
                    x.User.FullName,
                    x.User.Email,
                    x.User.Dni,
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.User.Sex) ? ConstantHelpers.SEX.VALUES[x.User.Sex] : "",
                    Age = x.User.BirthDate.AddYears((today.Year - x.User.BirthDate.Year)) > today ? (today.Year - x.User.BirthDate.Year) - 1 : (today.Year - x.User.BirthDate.Year)
                }, search)
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

        public async Task<object> CountByAgeRangeAndSex(int? ageRange = null, int? sex = null)
        {
            var query = _context.Teachers.AsQueryable();
            var teachers = await _context.Teachers
                .Select(x => new
                {
                    x.User.Sex,
                    x.User.Age
                })
                .ToListAsync();


            if (!sex.HasValue && !ageRange.HasValue)
            {
                var result = teachers
                    .GroupBy(q => new
                    {
                        Age = ConstantHelpers.AGE_RANGES.VALUES.FirstOrDefault(c =>
                            c.Value.MinAge <= q.Age
                            && c.Value.MaxAge > q.Age).Key,
                        q.Sex
                    })
                    .Select(q => new
                    {
                        ageRange = ConstantHelpers.AGE_RANGES.VALUES[q.Key.Age].Name,
                        sex = ConstantHelpers.SEX.VALUES[q.Key.Sex],
                        total = q.Count()
                    }).ToList();
                return result;
            }

            if (!sex.HasValue && ageRange.HasValue)
            {
                if (ageRange.Value > 0)
                {
                    var minAge = ConstantHelpers.AGE_RANGES.VALUES[ageRange.Value].MinAge;
                    var maxAge = ConstantHelpers.AGE_RANGES.VALUES[ageRange.Value].MaxAge;

                    var result = teachers
                        .Where(q => q.Age >= minAge
                        && q.Age < maxAge)
                        .GroupBy(q => q.Sex)
                        .Select(q => new
                        {
                            name = ConstantHelpers.SEX.VALUES[q.Key],
                            total = q.Count()
                        }).ToList();
                    return result;
                }
                else
                {
                    var result = teachers
                        .GroupBy(q => q.Sex)
                        .Select(q => new
                        {
                            name = ConstantHelpers.SEX.VALUES[q.Key],
                            total = q.Count()
                        }).ToList();
                    return result;
                }

            }

            if (sex.HasValue && !ageRange.HasValue)
            {
                if (sex.Value > 0)
                    query = query.Where(q => q.User.Sex == sex.Value);

                var result = teachers
                    .GroupBy(q =>
                        ConstantHelpers.AGE_RANGES.VALUES.First(c =>
                            c.Value.MinAge <= q.Age
                            && c.Value.MaxAge > q.Age).Key)
                    .Select(q => new
                    {
                        name = ConstantHelpers.AGE_RANGES.VALUES[q.Key].Name,
                        total = q.Count()
                    }).ToList();
                return result;
            }

            return teachers.Count(q =>
                q.Sex == sex.Value &&
                q.Age < ConstantHelpers.AGE_RANGES.VALUES[ageRange.Value].MaxAge &&
                q.Age >= ConstantHelpers.AGE_RANGES.VALUES[ageRange.Value].MinAge);
        }

        public async Task<IEnumerable<Teacher>> GetPerformanceByCareerAndTermAndWeekAndDedication(Guid termId,
            int week, Guid? facultyId = null, Guid? teacherDedicationId = null)
        {
            var query = _context.Teachers.Where(q => q.TeacherDedicationId.HasValue)
                .AsQueryable();

            if (teacherDedicationId.HasValue)
                query = query.Where(q => q.TeacherDedicationId == teacherDedicationId.Value);

            if (facultyId.HasValue)
                query = query.Where(q => q.Career.FacultyId == facultyId.Value);

            var term = await _context.Terms.FindAsync(termId);

            var result = await query.Select(x => new Teacher
            {
                UserId = x.UserId,
                User = new ApplicationUser
                {
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    Dni = x.User.Dni,
                    Email = x.User.Email,
                    FullName = x.User.FullName
                },
                CareerId = x.CareerId,
                Career = new Career
                {
                    Faculty = new Faculty
                    {
                        Id = x.Career.FacultyId,
                        Name = x.Career.Faculty.Name
                    },
                },
                TeacherDedicationId = x.TeacherDedicationId,
                TeacherDedication = new TeacherDedication
                {
                    Name = x.TeacherDedication.Name,
                    MinLessonHours = x.TeacherDedication.MinLessonHours
                },
                ValidatedHours = x.User.Tutorials.Where(t =>
                        t.Section.CourseTerm.TermId == termId && t.IsDictated &&
                        t.StartTime >= term.ClassStartDate.AddDays(7 * (week - 1))
                        && t.EndTime <= term.ClassStartDate.AddDays(7 * week))
                    .Sum(t => t.EndTime.Subtract(t.StartTime).TotalHours)
            }).ToListAsync();

            return result;
        }

        public async Task<object> CountPerformanceByCareerAndTermAndWeekAndDedication(Guid termId,
            int? week = null, Guid? facultyId = null, Guid? teacherDedicationId = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            var query = _context.Teachers.Where(q => q.TeacherDedicationId.HasValue)
                .AsQueryable();

            if (teacherDedicationId.HasValue)
                query = query.Where(q => q.TeacherDedicationId == teacherDedicationId.Value);

            if (facultyId.HasValue)
                query = query.Where(q => q.Career.FacultyId == facultyId.Value);

            if (week.HasValue)
            {
                var result = await query.GroupBy(x => new
                {
                    x.TeacherDedication,
                    x.Career.Faculty
                })
                    .Select(x => new
                    {
                        dedication = x.Key.TeacherDedication,
                        faculty = x.Key.Faculty,
                        validated = x.Count(g => g.User.Tutorials.Where(t =>
                                                         t.Section.CourseTerm.TermId == termId && t.IsDictated &&
                                                         t.StartTime >= term.ClassStartDate.AddDays(7 * (week.Value - 1))
                                                         && t.EndTime <= term.ClassStartDate.AddDays(7 * week.Value))
                                                     .Sum(t => t.EndTime.Subtract(t.StartTime).TotalHours) >=
                                                 (double)g.TeacherDedication.MinLessonHours),
                        total = x.Count()
                    }).ToListAsync();

                return result;
            }
            else
            {
                var maxWeeks = 0;
                if (DateTime.UtcNow >= term.ClassStartDate && DateTime.UtcNow <= term.ClassEndDate)
                    maxWeeks = (int)Math.Floor((DateTime.UtcNow - term.ClassStartDate).TotalDays / 7);
                else if (DateTime.UtcNow > term.ClassEndDate)
                    maxWeeks = (int)Math.Floor((term.ClassEndDate - term.ClassStartDate).TotalDays / 7);
                var weeks = Enumerable.Range(1, maxWeeks).ToArray();
                var result = await query
                    .GroupBy(x => new
                    {
                        x.TeacherDedication,
                        x.Career.Faculty
                    })
                    .Select(x => new
                    {
                        dedication = x.Key.TeacherDedication,
                        faculty = x.Key.Faculty,
                        weeks = weeks,
                        validated = weeks.Select(w => x.Count(g => g.User.Tutorials.Where(t =>
                                                         t.Section.CourseTerm.TermId == termId && t.IsDictated &&
                                                         t.StartTime >= term.ClassStartDate.AddDays(7 * (w - 1))
                                                         && t.EndTime <= term.ClassStartDate.AddDays(7 * w))
                                                     .Sum(t => t.EndTime.Subtract(t.StartTime).TotalHours) >=
                                                 (double)g.TeacherDedication.MinLessonHours)),
                        total = x.Count()
                    }).ToListAsync();

                return result;
            }
        }

        public async Task<Teacher> GetTeacherWithData(string userId)
        {
            return await _context.Teachers
                .Include(x => x.User)
                .ThenInclude(x => x.WorkerLaborInformation)
                .ThenInclude(x => x.WorkerLaborCondition)
                .Include(x => x.User)
                .ThenInclude(x => x.WorkerProfessionalTitles)
                .Include(x => x.TeacherDedication)
                .Include(x => x.AcademicDepartment)
                .Include(x => x.Career)
                    .ThenInclude(x => x.Faculty)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Teacher>> GetAllWithUser()
        {
            return await _context.Teachers.Include(x => x.User).ToListAsync();
        }

        public async Task<Select2Structs.ResponseParameters> GetTeachersSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Teacher, Select2Structs.Result>> selectPredicate = null, Func<Teacher, string[]> searchValuePredicate = null, string searchValue = null, int? status = null)
        {
            var query = _context.Teachers
                            .WhereSearchValue(searchValuePredicate, searchValue)
                            .AsNoTracking();

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                }
                else
                    query = query.WhereSearchValue((x) => new[] { x.User.FullName }, searchValue);
            }

            return await query.ToSelect2(requestParameters, selectPredicate);
        }

        public async Task<Select2Structs.ResponseParameters> GetTeachersByFacultySelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? facultyId = null, bool? isActive = null)
        {
            var query = _context.Teachers.AsNoTracking();

            if (isActive.HasValue)
                query = query.Where(x => x.User.IsActive == isActive);

            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
            //    {
            //        searchValue = $"\"{searchValue}*\"";
            //        query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
            //    }
            //    else
            //        query = query.Where(x => x.User.FullName.ToLower().Contains(searchValue.Trim().ToLower()) || x.User.UserName.ToLower().Contains(searchValue.Trim().ToLower()));
            //    //query = query.WhereSearchValue((x) => new[] { x.User.FullName }, searchValue);
            //}

            Expression<Func<Teacher, Select2Structs.Result>> selectPredicate = (x) => new Select2Structs.Result
            {
                Id = x.UserId,
                Text = x.User.FullName
            };

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetTeachersByCareer(Select2Structs.RequestParameters requestParameters, Guid id, Expression<Func<Teacher, Select2Structs.Result>> selectPredicate = null, Func<Teacher, string[]> searchValuePredicate = null, string searchValue = null, int? status = null)
        {
            var test = await _context.TeacherSections.Where(x => x.Section.CourseTerm.Course.CareerId == id).Select(x => x.TeacherId).ToListAsync();

            var query = _context.Teachers.Where(x => test.Any(y => y == x.UserId) && x.CareerId == id)
                .Include(x => x.TeacherDedication).Include(x => x.Career)
                .Select(x => x, searchValue)
                            .AsNoTracking();


            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
            //    {
            //        searchValue = $"\"{searchValue}*\"";
            //        query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
            //    }
            //    else
            //        query = query.WhereSearchValue((x) => new[] { x.User.FullName }, searchValue);
            //}
            return await query.ToSelect2(requestParameters, selectPredicate);
        }
        public async Task<IEnumerable<TeacherTemplateA>> GetTeachersModelA(Guid? facultyId, string coordinatorId = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
        {
            var query = _context.Teachers.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.CareerDirectorId == userId || x.AcademicDepartment.Career.AcademicCoordinatorId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    query = query.Where(x => x.AcademicDepartment.AcademicDepartmentDirectorId == userId || x.AcademicDepartment.AcademicDepartmentSecretaryId == userId);
                }
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
            }

            if (facultyId.HasValue)
                query = query.Where(x => x.AcademicDepartment.FacultyId == facultyId);

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (!string.IsNullOrEmpty(coordinatorId))
                query = query.Where(x => x.AcademicDepartment.Career.AcademicCoordinatorId == coordinatorId || x.AcademicDepartment.Career.CareerDirectorId == coordinatorId || x.AcademicDepartment.AcademicDepartmentDirectorId == coordinatorId || x.AcademicDepartment.AcademicDepartmentSecretaryId == coordinatorId);

            var result = await query.Select(x => new TeacherTemplateA
            {
                UserId = x.UserId,
                Code = x.User.UserName,
                Name = x.User.FullName,
                FacultyId = x.AcademicDepartment.FacultyId,
                Faculty = x.AcademicDepartment.Faculty == null ? "---" : x.AcademicDepartment.Faculty.Name,
            }).ToListAsync();

            return result;
        }

        public async Task<string> GetTeacherFullNameById(string userId)
        {
            return await _context.Teachers.Where(x => x.UserId == userId).Select(x => x.User.FullName).FirstOrDefaultAsync();
        }

        public async Task<List<ConditionAndDedicationTemplate>> GetAllByConditionAndDedication(Guid? condition, Guid? dedication, Guid? careerId, Guid? category, string search, string coordinatorId = null, Guid? academicDepartmentId = null, Guid? regimeId = null)
        {
            var query = _context.Teachers
                .AsQueryable();

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (regimeId.HasValue && regimeId != Guid.Empty)
                query = query.Where(x => x.User.WorkerLaborInformation.WorkerLaborRegimeId == regimeId);

            if (condition.HasValue && condition != Guid.Empty)
                query = query.Where(x => x.User.WorkerLaborInformation.WorkerLaborConditionId == condition.Value);

            if (dedication.HasValue && dedication != Guid.Empty)
                query = query.Where(x => x.TeacherDedicationId == dedication.Value);

            if (category.HasValue && category != Guid.Empty)
                query = query.Where(x => x.User.WorkerLaborInformation.WorkerLaborCategoryId == category);

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                query = query.Where(x => x.CareerId == careerId.Value);
            }
            else
            {
                if (!string.IsNullOrEmpty(coordinatorId))
                {
                    query = query.Where(x => x.Career.AcademicCoordinatorId == coordinatorId || x.Career.CareerDirectorId == coordinatorId || x.Career.AcademicDepartmentDirectorId == coordinatorId || x.Career.AcademicSecretaryId == coordinatorId);
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToLower().Contains(search.ToLower().Trim()));
            }
            //if (!string.IsNullOrEmpty(search))
            //    query = query.Where(x => x.User.FullName.RemoveDiacritics().ToLower().Contains(search.RemoveDiacritics().ToLower()));

            var result = await query
                .OrderBy(x => x.Career.Name)
                .ThenBy(x => x.User.FullName)
                .Select(x => new ConditionAndDedicationTemplate
                {
                    id = x.User.Id,
                    names = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    maternalSurname = x.User.MaternalSurname,
                    userName = x.User.UserName,
                    career = x.Career.Name,
                    fullname = x.User.FullName,
                    DNI = x.User.Document,
                    //condition = ConstantHelpers.TEACHER.TEACHER_CONDITIONS.VALUES[x.Condition],
                    condition = x.User.WorkerLaborInformation.WorkerLaborCondition.Name,
                    dedication = x.TeacherDedication.Name,
                    email = x.User.Email,
                    phoneNumber = x.User.PhoneNumber,
                    createdAt = x.CreatedAt,
                    category = x.User.WorkerLaborInformation.WorkerLaborCategory.Name,
                    AcademicDepartment = x.AcademicDepartment.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<object> GetAllAsSelect2ClientSide(Guid? facultyId = null, string keyWord = null, Guid? careerId = null, string coordinatorId = null, Guid? academicDepartmentId = null)
        {
            var query = _context.Teachers.AsQueryable();

            if (facultyId.HasValue)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (!string.IsNullOrEmpty(keyWord))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    keyWord = $"\"{keyWord}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, keyWord));
                }
                else
                    query = query.WhereSearchValue((x) => new[] { x.User.FullName }, keyWord);
            }
            //if (!string.IsNullOrEmpty(keyWord))
            //{
            //    foreach (var filtro in keyWord.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries))
            //        query = query.Where(x => x.User.Name.ToUpper().Contains(filtro.ToUpper()));
            //}

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                //var testt = await _context.TeacherSections.Where(y => y.Section.CourseTerm.Course.CareerId == careerId.Value).Select(x => x.TeacherId).ToListAsync();
                //query = query.Where(x => testt.Any(y => y == x.UserId));
                query = query.Where(x => x.CareerId == careerId.Value);
            }

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (!string.IsNullOrEmpty(coordinatorId))
            {
                var careeers = GetCoordinatorCareers(coordinatorId);
                //query = query.Where(x => x.TeacherSections.Any(y => careeers.Any(z => z == y.Section.CourseTerm.Course.CareerId)));
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.Course.Career.AcademicCoordinatorId == coordinatorId ||
                                                                    y.Section.CourseTerm.Course.Career.AcademicDepartmentDirectorId == coordinatorId ||
                                                                    y.Section.CourseTerm.Course.Career.AcademicSecretaryId == coordinatorId ||
                                                                    y.Section.CourseTerm.Course.Career.CareerDirectorId == coordinatorId));
            }

            var result = await query
                .OrderBy(x => x.User.FullName)
                .Select(x => new
                {
                    id = x.User.Id,
                    text = $"{x.User.Name} {x.User.PaternalSurname} {x.User.MaternalSurname}",
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<TeacherPlusAssitance>> GetAllPlusAssitance(Guid? careerId = null, string secretaryId = null)
        {
            var assists = await _context.WorkingDays.Where(y => y.RegisterDate.Date == DateTime.UtcNow.Date).ToListAsync();
            var query = _context.Teachers.AsQueryable();
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId.Value);

            if (!string.IsNullOrEmpty(secretaryId))
                query = query.Where(x => x.Career.AcademicSecretaryId == secretaryId);

            var data = await query.Select(
                    x => new
                    {
                        x.UserId,
                        x.User.FullName,
                        x.User.UserName
                    }
                ).ToListAsync();

            var result = data.Select(
                    x => new TeacherPlusAssitance
                    {
                        TeacherId = x.UserId,
                        FullName = x.FullName,
                        UserName = x.UserName,
                        Status = assists.Where(y => y.UserId == x.UserId).Select(y => y.Status).FirstOrDefault(),
                        StartDate = assists.Where(y => y.UserId == x.UserId && y.StartTime.HasValue).Select(y => y.StartTime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                        EndDate = assists.Where(y => y.UserId == x.UserId && y.Endtime.HasValue).Select(y => y.Endtime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                        Time = DateTime.UtcNow
                    }
                ).ToList();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, string search = null, Guid? termId = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Faculty.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Name); break;
                default:
                    orderByPredicate = ((x) => x.User.UserName); break;
            }

            var query = _context.Teachers.Where(x => x.User.IsActive).AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    query = query.Where(x => x.AcademicDepartment.AcademicDepartmentDirectorId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.CareerDirectorId == userId || x.AcademicDepartment.Career.AcademicCoordinatorId == userId);
                }
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.Faculty.DeanId == userId || x.AcademicDepartment.Career.Faculty.SecretaryId == userId);
                }
            }

            if (facultyId.HasValue)
            {
                query = query.Where(x => x.Career.FacultyId == facultyId.Value);
            }
            if (careerId.HasValue)
            {
                query = query.Where(x => x.CareerId == careerId.Value);
            }
            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
            {
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);
            }
            if (termId.HasValue)
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId));
            }

            int recordsFiltered = await query.Select(x => new
            {
                userId = x.UserId,
                username = x.User.UserName,
                name = x.User.FullName,
                faculty = x.Career.Faculty.Name,
                career = x.Career.Name,
                academicDepartment = x.AcademicDepartment.Name,
                sections = x.TeacherSections.Where(y => y.Section.CourseTerm.TermId == termId).Count()
            }, search).CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)

                .Select(x => new
                {
                    userId = x.UserId,
                    username = x.User.UserName,
                    name = x.User.FullName,
                    faculty = x.Career.Faculty.Name,
                    career = x.Career.Name,
                    academicDepartment = x.AcademicDepartment.Name,
                    sections = x.TeacherSections.Where(y => y.Section.CourseTerm.TermId == termId).Count()
                }, search)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var lockedUsers = _context.LockedUsers
                .OrderByDescending(x => x.DateTime)
                .AsEnumerable()
                .Where(x => data.Any(y => y.userId == x.UserId))
                .ToList();

            var _data = data.Select(x => new
            {
                id = x.userId,
                x.career,
                x.faculty,
                x.name,
                x.username,
                x.academicDepartment,
                x.sections,
                lockedUser = lockedUsers.FirstOrDefault(y => y.UserId == x.userId)
            }).ToList();

            var _data2 = _data.Select(x => new
            {
                x.id,
                x.career,
                x.faculty,
                x.name,
                x.username,
                x.sections,
                x.academicDepartment,
                text = x.lockedUser == null ? null : x.lockedUser.Status ? x.lockedUser.Description : null,
                locked = x.lockedUser == null ? false : x.lockedUser.Status ? true : false
            });

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = _data2,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetAllByTermandCareer(Guid termId, Guid? careerId, string coordinatorId = null, byte? status = null, ClaimsPrincipal user = null, Guid? academicDepartmentId = null)
        {
            var query = _context.Teachers.AsQueryable();


            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
            }

            if (careerId.HasValue && careerId.Value != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId.Value);

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (!string.IsNullOrEmpty(coordinatorId))
            {
                query = query.Where(x => x.Career.AcademicCoordinatorId == coordinatorId || x.Career.CareerDirectorId == coordinatorId);
            }

            var tecSchedules = await _context.TeacherSchedules.Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId)
                .Select(
                    x => new
                    {
                        x.TeacherId,
                        ClassSchedule = new
                        {
                            x.ClassSchedule.SessionType,
                            x.ClassSchedule.EndTimeText,
                            x.ClassSchedule.StartTimeText
                        }
                    }
                )
                .ToListAsync();

            var pedagogical_hour_time_configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME).FirstOrDefaultAsync();
            int.TryParse(pedagogical_hour_time_configuration.Value, out var pedagogical_hour_time);

            var resultDB = await query.Select(x => new
            {
                id = x.User.Id,
                names = x.User.Name,
                paternalSurname = x.User.PaternalSurname,
                maternalSurname = x.User.MaternalSurname,
                userName = x.User.UserName,
                condition = x.TeacherInformation != null && x.TeacherInformation.LaborCondition != 0 ? ConstantHelpers.USER_LABOR_INFORMATION.TEACHER_CONDITIONS.VALUES[x.TeacherInformation.LaborCondition] : string.Empty,
                dedication = x.TeacherDedication.Name,
                email = x.User.Email,
                phoneNumber = x.User.PhoneNumber,
            }).ToListAsync();


            var result = resultDB
                .Select(x => new
                {
                    x.id,
                    x.names,
                    x.paternalSurname,
                    x.maternalSurname,
                    x.userName,
                    fullname = $"{x.names} {x.paternalSurname} {x.maternalSurname}",
                    x.condition,
                    x.dedication,
                    x.email,
                    x.phoneNumber,
                    academicCharge = _context.TeacherAcademicCharges.Where(y => y.TeacherId == x.id && y.TermId == termId).Select(y => new { y.Id, y.IsValidated, y.Observation }).FirstOrDefault(),
                    hours = ((tecSchedules
                                .Where(y => y.TeacherId == x.id)
                                .Select(s => new
                                {
                                    teoricHours = s.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.THEORY ? DateTime.ParseExact(s.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(s.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalMinutes : 0,
                                    practicalHours = s.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE ? DateTime.ParseExact(s.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(s.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalMinutes : 0,
                                    seminarHours = s.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.SEMINAR ? DateTime.ParseExact(s.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(s.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalMinutes : 0,
                                }).Sum(y => y.teoricHours + y.practicalHours + y.seminarHours)) / pedagogical_hour_time).ToString("0.0")

                }).ToList();

            if (status.HasValue)
            {
                switch (status)
                {
                    case 1:
                        result = result.Where(x => x.academicCharge != null && x.academicCharge.IsValidated).ToList();
                        break;

                    case 2:
                        result = result.Where(x => x.academicCharge == null || (x.academicCharge != null && !x.academicCharge.IsValidated)).ToList();
                        break;
                }
            }

            return result;
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByTermandCareer2(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId, string coordinatorId = null, byte? status = null, string searchValue = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null, byte? sex = null, bool? viewAll = false)
        {
            var query = _context.Teachers.AsQueryable();

            if (careerId.HasValue && careerId.Value != Guid.Empty)
                query = query.Where(x => x.AcademicDepartment.CareerId == careerId);

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (sex.HasValue && sex != 0)
                query = query.Where(x => x.User.Sex == sex);

            if (!string.IsNullOrEmpty(coordinatorId))
            {
                query = query.Where(
                    x =>
                    x.AcademicDepartment.Career.AcademicCoordinatorId == coordinatorId ||
                    x.AcademicDepartment.Career.CareerDirectorId == coordinatorId ||
                    x.AcademicDepartment.Career.AcademicSecretaryId == coordinatorId ||
                    x.AcademicDepartment.Career.SocialResponsabilityCoordinatorId == coordinatorId ||
                    x.AcademicDepartment.AcademicDepartmentDirectorId == coordinatorId ||
                    x.AcademicDepartment.AcademicDepartmentSecretaryId == coordinatorId);
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.Faculty.DeanId == userId || x.AcademicDepartment.Career.Faculty.SecretaryId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_COORDINATOR))
                {
                    query = query.Where(x => x.AcademicDepartment.AcademicDepartmentDirectorId == userId || x.AcademicDepartment.AcademicDepartmentSecretaryId == userId || x.AcademicDepartment.AcademicDepartmentCoordinatorId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.AcademicCoordinatorId == userId || x.AcademicDepartment.Career.CareerDirectorId == userId);
                }
            }

            var tecSchedules = await _context.TeacherSchedules.Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId)
                .Select(
                    x => new
                    {
                        x.TeacherId,
                        ClassSchedule = new
                        {
                            x.ClassSchedule.SessionType,
                            x.ClassSchedule.EndTimeText,
                            x.ClassSchedule.StartTimeText
                        }
                    }
                )
                .ToListAsync();

            var nonTeachingSchedules = await _context.NonTeachingLoadSchedules.Where(x => x.NonTeachingLoad.TermId == termId)
                .Select(x => new
                {
                    x.NonTeachingLoad.TeacherId,
                    x.StartTime,
                    x.EndTime
                })
                .ToListAsync();

            if (viewAll.HasValue && !viewAll.Value)
                query = query.Where(x => x.TeacherSchedules.Any(y => y.ClassSchedule.Section.CourseTerm.TermId == termId) || x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId && y.Section.IsDirectedCourse));


            var pedagogical_hour_time_configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME).FirstOrDefaultAsync();
            if (pedagogical_hour_time_configuration is null)
            {
                pedagogical_hour_time_configuration = new Configuration
                {
                    Key = ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME,
                    Value = ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME]
                };

                await _context.AddAsync(pedagogical_hour_time_configuration);
                await _context.SaveChangesAsync();
            }

            int.TryParse(pedagogical_hour_time_configuration.Value, out var pedagogical_hour_time);

            var recordsFiltered = 0;
            recordsFiltered = await query
                .Select(x => new
                {
                    id = x.User.Id,
                    names = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    academicDepartment = x.AcademicDepartment.Name ?? "-",
                    maternalSurname = x.User.MaternalSurname,
                    userName = x.User.UserName,
                    fullname = x.User.FullName,
                    condition = x.User.WorkerLaborInformation.WorkerLaborCondition.Name ?? "",
                    dedication = x.TeacherDedication.Name,
                    email = x.User.Email,
                    phoneNumber = x.User.PhoneNumber,
                    academicCharge = _context.TeacherAcademicCharges.Where(y => y.TeacherId == x.UserId && y.TermId == termId).Select(y => new { y.Id, y.IsValidated, y.Observation }).FirstOrDefault(),
                    directedCourseSum = x.TeacherSections.Where(y => y.Section.IsDirectedCourse && y.Section.CourseTerm.TermId == termId).Sum(y => (y.Section.CourseTerm.Course.TheoreticalHours + y.Section.CourseTerm.Course.PracticalHours)) / 2M
                }, searchValue)
                .CountAsync();

            var dataDB = await query
                .Select(x => new
                {
                    id = x.User.Id,
                    names = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    academicDepartment = x.AcademicDepartment.Name ?? "-",
                    maternalSurname = x.User.MaternalSurname,
                    userName = x.User.UserName,
                    fullname = x.User.FullName,
                    condition = x.User.WorkerLaborInformation.WorkerLaborCondition.Name ?? "",
                    dedication = x.TeacherDedication.Name,
                    email = x.User.Email,
                    phoneNumber = x.User.PhoneNumber,
                    academicCharge = _context.TeacherAcademicCharges.Where(y => y.TeacherId == x.UserId && y.TermId == termId).Select(y => new { y.Id, y.IsValidated, y.Observation }).FirstOrDefault(),
                    //directedCourseSum = x.TeacherSections.Where(y => y.Section.IsDirectedCourse && y.Section.CourseTerm.TermId == termId).Sum(y => (y.Section.CourseTerm.Course.TheoreticalHours + y.Section.CourseTerm.Course.PracticalHours)) / 2M
                    directedCourseList = x.TeacherSections.Where(y => y.Section.IsDirectedCourse && y.Section.CourseTerm.TermId == termId).Select(y => y.Section.CourseTerm.Course.TheoreticalHours + y.Section.CourseTerm.Course.PracticalHours).ToList()
                }, searchValue)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.id,
                    x.names,
                    x.paternalSurname,
                    x.academicDepartment,
                    x.maternalSurname,
                    x.userName,
                    x.fullname,
                    x.condition,
                    x.dedication,
                    x.email,
                    x.phoneNumber,
                    x.academicCharge,
                    hours = (((tecSchedules
                                    .Where(y => y.TeacherId == x.id)
                                    .Select(s => new
                                    {
                                        teoricHours = s.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.THEORY ? DateTime.ParseExact(s.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(s.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalMinutes : 0,
                                        practicalHours = s.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE ? DateTime.ParseExact(s.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(s.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalMinutes : 0,
                                        seminarHours = s.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.SEMINAR ? DateTime.ParseExact(s.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(s.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalMinutes : 0,
                                    }).Sum(y => y.teoricHours + y.practicalHours + y.seminarHours)) / pedagogical_hour_time) 
                                    + 
                                    Convert.ToDouble(x.directedCourseList.DefaultIfEmpty(0).Sum()/2M)
                                    +
                                    nonTeachingSchedules.Where(y=>y.TeacherId == x.id).Sum(y=>y.EndTime.ToLocalTimeSpanUtc().Subtract(y.StartTime.ToLocalTimeSpanUtc()).TotalHours)
                                    ).ToString("0.0")
                })
                .ToList();



            if (status.HasValue)
            {
                switch (status)
                {
                    case 1:
                        data = data.Where(x => x.academicCharge != null && x.academicCharge.IsValidated).ToList();
                        break;

                    case 2:
                        data = data.Where(x => x.academicCharge == null || (x.academicCharge != null && !x.academicCharge.IsValidated)).ToList();
                        break;
                }
            }


            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<Teacher>> GetAllByCareers(List<Guid> careers)
        {
            var query = _context.Teachers.AsQueryable();
            if (careers.Count > 0)
            {
                var carreras = _context.Careers.Where(x => careers.Contains(x.Id));

                query = query.Where(x => carreras.Any(y => y.Id == x.CareerId));
            }

            return await query.Include(x => x.User).ToListAsync();
        }

        public async Task<int> CountByCareers(List<Guid> careers)
        {
            var query = _context.Teachers.AsQueryable();
            if (careers.Count > 0)
            {
                var carreras = _context.Careers.Where(x => careers.Contains(x.Id));
                query = query.Where(x => carreras.Any(y => y.Id == x.CareerId));
            }

            return await query.CountAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersByCareersDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.UserId);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.UserId);
                    break;
            }

            var query = _context.Teachers.AsQueryable();
            if (careers.Count > 0)
            {
                var carreras = _context.Careers.Where(x => careers.Contains(x.Id));

                query = query.Where(x => carreras.Any(y => y.Id == x.CareerId));
            }
            query = query
               .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate);

            Expression<Func<Teacher, object>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                Fullname = x.User.FullName,
                Faculty = x.Career.Faculty.Name,
                career = x.Career.Name,
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> GetTeacherSelect2Report()
        {
            var teachers = await _context.Teachers
                .Include(x => x.User)
                .Select(x => new
                {
                    id = x.User.Id,
                    text = x.User.FullName
                })
                .ToListAsync();

            return teachers;
        }

        public async Task<TeacherTemplateA> GetAsTemplateAById(string teacherId)
        {
            var teacher = await _context.Teachers
                                  .Select(x => new TeacherTemplateA
                                  {
                                      UserId = x.UserId,
                                      FullName = $"{x.User.Name}, {x.User.PaternalSurname} {x.User.MaternalSurname}",
                                      Dedication = x.TeacherDedication != null ? x.TeacherDedication.Name : "",
                                      LaborCondition = x.TeacherInformation != null ? x.TeacherInformation.LaborCondition : 3,
                                      Faculty = x.Career.Faculty != null ? x.Career.Faculty.Name : "",
                                      Career = x.Career != null ? x.Career.Name : "",
                                      UserName = x.User.UserName
                                  })
                                  .FirstOrDefaultAsync(x => x.UserId == teacherId || x.UserName == teacherId);

            return teacher;
        }

        public async Task<object> GetAllAsModelC()
        {
            var result = await _context.Teachers
                .Select(x => new
                {
                    Id = x.UserId,
                    Code = x.User.UserName,
                    Name = x.User.FullName,
                    Faculty = x.Career == null ? "-" : x.Career.Faculty.Name,
                    Career = x.Career == null ? "-" : x.Career.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return result;
        }

        public async Task<Teacher> GetWithTeacherDedication(string id)
        {
            var teacher = await _context.Teachers.Include(x => x.TeacherDedication).Where(x => x.UserId == id).FirstOrDefaultAsync();
            return teacher;
        }

        public async Task<int> GetTeachersEnrolledCountByTermIdAndLevelStudie(Guid termId, int levelStudy)
        {
            int result = 0;
            if (levelStudy == ConstantHelpers.STUDIES_LEVEL.MASTER)
            {
                result = await _context.Teachers
                .Where(x => x.User.WorkerMasterDegrees.Any(y => y.UserId == x.UserId) &&
                    x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId)).CountAsync();

            }
            else if (levelStudy == ConstantHelpers.STUDIES_LEVEL.DOCTORAL)
            {
                result = await _context.Teachers
                .Where(x => x.User.WorkerDoctoralDegrees.Any(y => y.UserId == x.UserId) &&
                    x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId)).CountAsync();
            }

            return result;
        }

        public async Task<int> GetTeachersEnrolledCountByTermId(Guid termId, ClaimsPrincipal user = null)
        {
            var query = _context.Teachers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            return await query.Where(X => X.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId)).CountAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetByCategoryAndTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? categoryId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TeacherSections
                .IgnoreQueryFilters()
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Teacher.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (categoryId != null) query = query.Where(x => x.Teacher.User.WorkerLaborInformation.WorkerLaborCategoryId == categoryId);

            int recordsFiltered = await query
                .Select(x => new { x.Section.CourseTerm.TermId, x.Teacher.User.WorkerLaborInformation.WorkerLaborCategoryId })
                .Distinct()
                .CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.TeacherId,
                    Term = x.Section.CourseTerm.Term.Name,
                    x.Section.CourseTerm.TermId,
                    Category = x.Teacher.User.WorkerLaborInformation.WorkerLaborCategory.Name,
                    Regime = x.Teacher.User.WorkerLaborInformation.WorkerLaborCategory.WorkerLaborRegime.Name,
                    x.Teacher.User.WorkerLaborInformation.WorkerLaborCategoryId
                })
                .Distinct()
                .GroupBy(x => new { x.TermId, x.Term, x.Category, x.Regime, x.WorkerLaborCategoryId })
                .Select(x => new
                {
                    x.Key.Term,
                    x.Key.Category,
                    x.Key.Regime,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Category)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetByCategoryAndTermChart(Guid? termId = null, Guid? categoryId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TeacherSections
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Teacher.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (categoryId != null) query = query.Where(x => x.Teacher.User.WorkerLaborInformation.WorkerLaborCategoryId == categoryId);


            var data = await _context.WorkerLaborCategories
                .Select(x => new
                {
                    Category = x.Name,
                    Regime = x.WorkerLaborRegime.Name,
                    Count = query.Where(y => y.Teacher.User.WorkerLaborInformation.WorkerLaborCategoryId == x.Id).Select(y => new { y.TeacherId, y.Section.CourseTerm.TermId }).Distinct().Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Category)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => $"{x.Category} - Reg. {x.Regime}").ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherByCountryDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? countryId = null)
        {
            var query = _context.TeacherSections.AsNoTracking();

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (countryId != null) query = query.Where(x => x.Teacher.User.Department.CountryId == countryId);

            int recordsFiltered = await query
                .Select(x => new { x.Section.CourseTerm.TermId, x.Teacher.User.Department.CountryId })
                .Distinct()
                .CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.TeacherId,
                    Term = x.Section.CourseTerm.Term.Name,
                    x.Section.CourseTerm.TermId,
                    Country = x.Teacher.User.Department.Country.Name,
                    x.Teacher.User.Department.CountryId
                })
                .Distinct()
                .GroupBy(x => new { x.TermId, x.Term, x.Country, x.CountryId })
                .Select(x => new
                {
                    x.Key.Term,
                    x.Key.Country,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Country)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetTeacherByCountryChart(Guid? termId = null, Guid? countryId = null)
        {
            var query = _context.TeacherSections.AsNoTracking();

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (countryId != null) query = query.Where(x => x.Teacher.User.Department.CountryId == countryId);

            var data = await _context.Countries
                .Select(x => new
                {
                    Country = x.Name,
                    Count = query.Where(y => y.Teacher.User.Department.CountryId == x.Id).Select(y => new { y.TeacherId, y.Section.CourseTerm.TermId }).Distinct().Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Country)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Country).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<object> GetTeachersJson(string searchValue)
        {
            var query = _context.Teachers.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                }
                else
                    query = query.WhereSearchValue((x) => new[] { x.User.FullName }, searchValue);
            }

            var model = await query.OrderBy(x => x.User.PaternalSurname).Select(x => new
            {
                id = x.UserId,
                text = x.User.FullName
            }).ToListAsync();

            return model;
        }

        public async Task<Teacher> GetByUserId(string userId)
        {
            var result = await _context.Teachers
                .Include(x => x.Career)
                .Include(x => x.User)
                .Include(x => x.AcademicDepartment)
                .Where(t => t.UserId == userId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<object> GetTeacherJsonByUserId(string userId)
        {
            var result = await _context.Teachers
                .Where(t => t.UserId == userId)
                .Select(t => new
                {
                    id = t.UserId,
                    text = t.User.FullName
                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<object> GetTeachersByIdJson(Guid id)
        {
            var teachers = await _context.Teachers
                .Where(c => c.Career.FacultyId == id)
                .Select(c => new
                {
                    id = c.UserId,
                    text = c.User.FullName
                }).OrderBy(x => x.text).ToListAsync();

            teachers.Insert(0, new { id = "", text = "Todos" });

            return teachers;
        }

        public async Task<int> GetRegisteredTeachersCountByTermId(Guid termId)
        {
            return await _context.Teachers.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId)).CountAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersWithInvestigationProjectDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
        {
            var teachers = _context.Teachers.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    teachers = teachers.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }


            var query = _context.InvestigationProjectMembers.Where(x => teachers.Any(y => y.UserId == x.MemberId)).AsNoTracking();

            int recordsFiltered = await query
                .Select(x => x.MemberId)
                .Distinct()
                .CountAsync();


            var data = await query
                .GroupBy(x => new { x.Member.UserName, x.Member.FullName, x.Member.Dni, x.Member.Email })
                .Select(x => new
                {
                    x.Key.FullName,
                    x.Key.UserName,
                    x.Key.Dni,
                    x.Key.Email,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.FullName)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllEscalafonReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null, Guid? conditionId = null, Guid? dedicationId = null, Guid? categoryId = null, Guid? capPositionId = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.Dni;
                    break;
                case "4":
                    orderByPredicate = (x) => x.User.Email;
                    break;
                case "5":
                    orderByPredicate = (x) => x.User.PhoneNumber;
                    break;
                case "6":
                    orderByPredicate = (x) => x.User.BirthDate;
                    break;
                case "7":
                    orderByPredicate = (x) => x.AcademicDepartment.Name;
                    break;
                case "8":
                    orderByPredicate = (x) => x.User.WorkerLaborInformation.WorkerLaborCondition.Name;
                    break;
                case "9":
                    orderByPredicate = (x) => x.TeacherDedication.Name;
                    break;
            }

            var query = _context.Teachers
                .AsQueryable();

            if (academicDepartmentId != null)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (conditionId != null)
            {
                var laborInformations = _context.WorkerLaborInformation
                    .Where(x => x.WorkerLaborConditionId == conditionId)
                    .AsQueryable();

                query = query.Where(x => laborInformations.Any(y => y.UserId == x.UserId));
            }

            if (dedicationId != null)
            {
                query = query.Where(x => x.TeacherDedicationId == dedicationId);
            }

            if (categoryId != null)
            {
                var laborInformations = _context.WorkerLaborInformation
                    .Where(x => x.WorkerLaborCategoryId == categoryId)
                    .AsQueryable();

                query = query.Where(x => laborInformations.Any(y => y.UserId == x.UserId));
            }

            if (capPositionId != null)
            {
                var laborInformations = _context.WorkerLaborInformation
                    .Where(x => x.WorkerCapPositionId == capPositionId)
                    .AsQueryable();

                query = query.Where(x => laborInformations.Any(y => y.UserId == x.UserId));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.User.Name,
                    x.User.MaternalSurname,
                    x.User.PaternalSurname,
                    x.User.Dni,
                    x.User.BirthDateString,
                    x.User.Email,
                    x.User.PhoneNumber,
                    AcademicDepartment = x.AcademicDepartment.Name ?? "-",
                    Dedication = x.TeacherDedication.Name ?? "-",
                    CapPosition = x.User.WorkerLaborInformation.WorkerCapPosition.Name ?? "-",
                    Condition = x.User.WorkerLaborInformation.WorkerLaborCondition.Name ?? "-" /*_context.WorkerLaborInformation.Where(y => y.UserId == x.UserId).Select(y => y.WorkerLaborCondition.Name ?? "-")*/
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherByDedicationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? dedicationId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TeacherSections
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Teacher.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (dedicationId != null) query = query.Where(x => x.Teacher.TeacherDedicationId == dedicationId);

            int recordsFiltered = await query
                .Select(x => new { x.Section.CourseTerm.TermId, x.Teacher.TeacherDedicationId })
                .Distinct()
                .CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.TeacherId,
                    Term = x.Section.CourseTerm.Term.Name,
                    x.Section.CourseTerm.TermId,
                    Dedication = x.Teacher.TeacherDedication.Name,
                    Regime = x.Teacher.TeacherDedication.WorkerLaborRegime.Name,
                    x.Teacher.TeacherDedicationId
                })
                .Distinct()
                .GroupBy(x => new { x.TermId, x.Term, x.Dedication, x.Regime, x.TeacherDedicationId })
                .Select(x => new
                {
                    x.Key.Term,
                    x.Key.Dedication,
                    x.Key.Regime,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Dedication)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            //Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            //switch (sentParameters.OrderColumn)
            //{
            //    case "0":
            //        orderByPredicate = ((x) => x.User.UserName);
            //        break;
            //    case "1":
            //        orderByPredicate = ((x) => x.User.FullName);
            //        break;
            //    default:
            //        orderByPredicate = ((x) => x.User.UserName);
            //        break;
            //}

            //var query = _context.Teachers
            //    .Where(x => x.User.IsActive && (x.User.State == ConstantHelpers.USER_STATES.ACTIVE || x.User.State == ConstantHelpers.USER_STATES.TEMPORARY))
            //    .AsQueryable();

            //if (dedicationId != null)
            //    query = query.Where(x => x.TeacherDedicationId == dedicationId);

            //int recordsFiltered = await query.CountAsync();

            //var data = await query
            //    .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
            //    .Select(x => new
            //    {
            //        x.User.UserName,
            //        x.User.Name,
            //        x.User.MaternalSurname,
            //        x.User.PaternalSurname,
            //        x.User.Email,
            //        Dedication = x.TeacherDedication.Name ?? "-",
            //    })
            //    .Skip(sentParameters.PagingFirstRecord)
            //    .Take(sentParameters.RecordsPerDraw)
            //    .ToListAsync();

            //var recordsTotal = data.Count();

            //return new DataTablesStructs.ReturnedData<object>
            //{
            //    Data = data,
            //    DrawCounter = sentParameters.DrawCounter,
            //    RecordsFiltered = recordsFiltered,
            //    RecordsTotal = recordsTotal
            //};
        }

        public async Task<object> GetTeacherByDedicationChart(Guid? termId = null, Guid? dedicationId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TeacherSections
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Teacher.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (dedicationId != null) query = query.Where(x => x.Teacher.TeacherDedicationId == dedicationId);

            var data = await _context.TeacherDedication
                .Select(x => new
                {
                    Dedication = x.Name,
                    Regime = x.WorkerLaborRegime.Name,
                    Count = query.Where(y => y.Teacher.TeacherDedicationId == x.Id).Select(y => new { y.TeacherId, y.Section.CourseTerm.TermId }).Distinct().Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Dedication)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => $"{x.Dedication} - Reg. {x.Regime}").ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;

            //var query = _context.Teachers
            //                .Where(x => x.User.IsActive && (x.User.State == ConstantHelpers.USER_STATES.ACTIVE || x.User.State == ConstantHelpers.USER_STATES.TEMPORARY))
            //                .AsQueryable();

            //if (dedicationId != null)
            //    query = query.Where(x => x.TeacherDedicationId == dedicationId);


            //var dedications = await _context.TeacherDedication
            //    .Select(x => new
            //    {
            //        Dedication = x.Name,
            //        Accepted = query.Where(y => y.TeacherDedicationId == x.Id).Count()
            //    })
            //    .OrderByDescending(x => x.Accepted)
            //    .ThenBy(x => x.Dedication)
            //    .ToListAsync();

            //var result = new
            //{
            //    categories = dedications.Select(x => x.Dedication).ToList(),
            //    data = dedications.Select(x => x.Accepted).ToList()
            //};

            //return result;
        }

        public async Task<DataTablesStructs.ReturnedData<Teacher>> GetTeacherByClaimDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid? facultyId = null, string search = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Faculty.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
            }

            var query = _context.Teachers.AsQueryable();
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                var academicDepartment = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartmentId).ToArrayAsync();
                query = query.Where(x => x.AcademicDepartmentId.HasValue && academicDepartment.Contains(x.AcademicDepartmentId.Value));
            }

            if (facultyId.HasValue)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search));
                }
                else
                    query = query.WhereSearchValue((x) => new[] { x.User.FullName }, search);
            }

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<Teacher, Teacher>> selectPredicate = (x) => new Teacher
            {
                User = new ApplicationUser
                {
                    UserName = x.User.UserName,
                    FullName = x.User.FullName,
                    Email = x.User.Email
                },
                Career = new Career
                {
                    Name = x.Career.Name,
                    Faculty = new Faculty
                    {
                        Name = x.Career.Faculty.Name
                    }
                }
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachingPlanDataTable(DataTablesStructs.SentParameters paginationParameter, Guid termId, string search)
        {
            var query = _context.Teachers
                .Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (paginationParameter.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Document);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.WorkerLaborInformation.WorkerLaborCondition.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.TeacherDedication.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
            }


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper())
                                        || x.User.WorkerLaborInformation.WorkerLaborCondition.Name.ToUpper().Contains(search.ToUpper())
                                        || x.TeacherDedication.Name.ToUpper().Contains(search.ToUpper()));
            }

            var filterRecordsAsync = await query.CountAsync();

            var pagedList = await query
                        .Skip(paginationParameter.PagingFirstRecord)
                        .Take(paginationParameter.RecordsPerDraw)
                        .Select(x => new
                        {
                            id = x.UserId,
                            name = x.User.FullName,
                            condition = x.User.WorkerLaborInformation.WorkerLaborCondition.Name ?? "-",
                            dedication = x.TeacherDedication.Name ?? "-",
                            dni = x.User.Dni
                        })
                        .ToListAsync();

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = pagedList,
                DrawCounter = paginationParameter.DrawCounter,
                RecordsFiltered = filterRecordsAsync,
                RecordsTotal = filterRecordsAsync
            };
            return result;
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersToAssignCourses(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null, Guid? academicDepartmentId = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.TeacherDedication.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.User.WorkerLaborInformation.WorkerLaborCondition.Name); break;
                default:
                    orderByPredicate = ((x) => x.User.UserName); break;
            }

            var query = _context.Teachers.Where(x => x.User.IsActive).AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_COORDINATOR))
                {
                    query = query.Where(x => x.AcademicDepartment.AcademicDepartmentDirectorId == userId || x.AcademicDepartment.AcademicDepartmentSecretaryId == userId || x.AcademicDepartment.AcademicDepartmentCoordinatorId == userId);
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
                {
                    query = query.Where(x => x.Career.CareerDirectorId == userId);
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            //if (!string.IsNullOrEmpty(searchValue))
            //    query = query.Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue));

            int recordsFiltered = await query
                .Select(x => new
                {
                    x.UserId,
                    x.User.UserName,
                    x.User.FullName,
                    dedication = x.TeacherDedication.Name,
                    condition = x.User.WorkerLaborInformation.WorkerLaborCondition.Name,
                }, searchValue)
                .CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                  .Select(x => new
                  {
                      x.UserId,
                      x.User.UserName,
                      x.User.FullName,
                      dedication = x.TeacherDedication.Name,
                      condition = x.User.WorkerLaborInformation.WorkerLaborCondition.Name,
                  }, searchValue)
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

        public async Task<object> SearchTeacherByTerm(string term)
        {
            var query = _context.Teachers
                         .AsNoTracking();

            if (!string.IsNullOrEmpty(term))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    term = $"\"{term}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, term) || EF.Functions.Contains(x.User.UserName, term));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(term.ToUpper()) || x.User.UserName.ToUpper().Contains(term.ToUpper()));
            }

            var teachers = await query
                .OrderBy(x => x.User.UserName)
                .Select(x => new
                {
                    id = x.UserId,
                    text = $"{x.User.UserName} - {x.User.FullName}"
                }).Take(5).ToListAsync();

            return teachers;
        }

        public async Task<object> SearchTeacherByTerm(string term, List<string> filteredUsers)
        {
            var query = _context.Teachers
                .Include(x => x.User)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(term))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    term = $"\"{term}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, term) || EF.Functions.Contains(x.User.UserName, term));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(term.ToUpper()) || x.User.UserName.ToUpper().Contains(term.ToUpper()));
            }

            var teachers = await query
                .OrderBy(x => x.User.UserName)
                .Select(x => new
                {
                    id = x.UserId,
                    text = $"{x.User.UserName} - {x.User.FullName}"
                }).Take(5).ToListAsync();

            teachers = teachers.Where(x => !filteredUsers.Contains(x.id)).ToList();
            return teachers;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetOlderTeacherDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            //Mayores de 70
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.PaternalSurname);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.MaternalSurname);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.User.BirthDate.Year == 1 ? 0 : x.User.Age);
                    break;
            }
            var today = DateTime.UtcNow.AddHours(-5);

            var query = _context.Teachers
                .Where(x => (x.User.BirthDate.AddYears((today.Year - x.User.BirthDate.Year)) > today ? (today.Year - x.User.BirthDate.Year) - 1 : (today.Year - x.User.BirthDate.Year)) > 70)
                .AsQueryable();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.Name,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    Age = x.User.BirthDate.AddYears((today.Year - x.User.BirthDate.Year)) > today ? (today.Year - x.User.BirthDate.Year) - 1 : (today.Year - x.User.BirthDate.Year)
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDismissalTeacherDatatable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.CareerId);
                    break;
            }

            var query = _context.Teachers
                .Where(x => x.User.ScaleResolutions.Any(y => y.Observation.ToLower() == "destitución" || y.Observation.ToLower() == "destitucion"))
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.FullName.ToLower().Contains(search.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    username = x.User.UserName,
                    fullName = x.User.FullName,
                    dni = x.User.Dni,
                    career = x.Career.Name
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<Select2Structs.ResponseParameters> GetTeachersByCareerSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, int? status = null)
        {


            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            
            var query = _context.Teachers.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == term.Id)).AsNoTracking();
            
            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue));
                //query = query.WhereSearchValue((x) => new[] { x.User.FullName }, searchValue);
            }

            Expression<Func<Teacher, Select2Structs.Result>> selectPredicate = (x) => new Select2Structs.Result
            {
                Id = x.UserId,
                Text = $"{x.User.FullName} - {x.TeacherDedication.Name}"
            };

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public void Remove(Teacher teacher)
        {
            _context.Teachers.Remove(teacher);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersBirthdayThisMonth(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.BirthDate.DayOfYear);
                    break;
            }

            var query = _context.Teachers.AsNoTracking();
            var today = DateTime.UtcNow.AddHours(-5);

            query = query.Where(x => x.User.BirthDate.Month == today.Month);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.FullName,
                    x.User.Email,
                    BirthDate = x.User.BirthDate.ToString("dd 'de' MMMM")
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersContractThisMonth(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<ScaleResolution, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.EndDate.Value);
                    break;
            }

            var query = _context.ScaleResolutions
                .Where(x => x.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos" && x.User.Teachers.Any(y => y.UserId == x.UserId))
                .AsNoTracking();

            var today = DateTime.UtcNow.AddHours(-5);
            var endDate = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            var statDate = new DateTime(today.Year, today.Month, 1);

            query = query.Where(x => x.EndDate.HasValue);

            query = query.Where(x => x.EndDate.Value.AddHours(-5) >= statDate && x.EndDate.Value.AddHours(-5) <= endDate);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.FullName,
                    x.User.Email,
                    contractEndDate = x.EndDate.Value.ToString("dd 'de' MMMM")
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


        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersLicenseThisMonth(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<ScaleLicenseAuthorization, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.EndDate);
                    break;
            }

            var query = _context.ScaleLicenseAuthorizations.AsNoTracking();

            var today = DateTime.UtcNow.AddHours(-5);
            var endDate = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            var statDate = new DateTime(today.Year, today.Month, 1);

            query = query.Where(x => x.EndDate.AddHours(-5) >= statDate && x.EndDate.AddHours(-5) <= endDate);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.FullName,
                    x.User.Email,
                    licenseEndDate = x.EndDate.ToString("dd 'de' MMMM")
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherByAgeRange(DataTablesStructs.SentParameters sentParameters, string search, int minAge, int maxAge)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.BirthDate);
                    break;
            }

            var query = _context.Teachers.AsNoTracking();

            var today = DateTime.UtcNow.AddHours(-5);

            var maxDate = today.AddYears(-minAge);
            var minDate = today.AddYears(-maxAge);

            query = query.Where(x => x.User.BirthDate >= minDate && x.User.BirthDate <= maxDate);

            int recordsFiltered = await query
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.FullName,
                    x.User.Email
                }, search)
                .CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.FullName,
                    x.User.Email,
                    Age = x.User.BirthDate.AddYears((today.Year - x.User.BirthDate.Year)) > today ? (today.Year - x.User.BirthDate.Year) - 1 : (today.Year - x.User.BirthDate.Year)
                }, search)
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetIntranetTeachersDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null, string search = null, ClaimsPrincipal user = null, Guid? termId = null)
        {

            Expression<Func<Teacher, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni); break;
                case "3":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "4":
                    orderByPredicate = ((x) => x.User.PhoneNumber); break;
            }

            var query = _context.Teachers.AsNoTracking();

            if (termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId));
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.CareerDirectorId == userId || x.AcademicDepartment.Career.AcademicCoordinatorId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    query = query.Where(x => x.AcademicDepartment.AcademicDepartmentDirectorId == userId || x.AcademicDepartment.AcademicDepartmentSecretaryId == userId);
                }
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
            }

            if (academicDepartmentId != null)
            {
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);
            }


            var recordsFiltered = await query
                .Select(x => new
                {
                    id = x.User.Id,
                    dni = x.User.Dni,
                    paternalSurName = x.User.PaternalSurname,
                    maternalSurName = x.User.MaternalSurname,
                    name = x.User.Name,
                    fullName = x.User.FullName,
                    phoneNumber = x.User.PhoneNumber,
                    picture = x.User.Picture,
                    userName = x.User.UserName
                }, search).CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.User.Id,
                    dni = x.User.Dni,
                    paternalSurName = x.User.PaternalSurname,
                    maternalSurName = x.User.MaternalSurname,
                    name = x.User.Name,
                    fullName = x.User.FullName,
                    phoneNumber = x.User.PhoneNumber,
                    picture = x.User.Picture,
                    userName = x.User.UserName
                }, search)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherForAsistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? academicDepartmentId = null, ClaimsPrincipal user = null, string searchValue = null)
        {
            var query = _context.Teachers
                            .AsNoTracking();

            Expression<Func<Teacher, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.AcademicDepartment.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.AcademicDepartment.Faculty.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "5":
                    orderByPredicate = ((x) => x.User.PhoneNumber); break;
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    query = query.Where(x => x.AcademicDepartment.AcademicDepartmentDirectorId == userId);
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.CareerDirectorId == userId || x.AcademicDepartment.Career.AcademicCoordinatorId == userId);
                }
            }

            if (facultyId != null)
                query = query.Where(x => x.AcademicDepartment.FacultyId == facultyId);

            if (academicDepartmentId != null)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.User.UserName.ToUpper().Contains(searchValue.ToUpper()) || x.User.FullName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.UserId,
                    username = x.User.UserName,
                    name = x.User.FullName,
                    academicDepartment = x.AcademicDepartment.Name ?? "--",
                    faculty = x.AcademicDepartment.Faculty.Name ?? "--",
                    email = x.User.Email,
                    phoneNumber = x.User.PhoneNumber
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudiesDatatableByUser(DataTablesStructs.SentParameters sentParameters, string UserId)
        {
            var data = new List<GenericStudiesTemplate>();

            var schoolStudies = await _context.WorkerSchoolDegrees.Where(x => x.UserId == UserId)
                            .Select(x => new GenericStudiesTemplate
                            {
                                Specialty = x.Specialty,
                                StudyType = "Primaria/Secundaria",
                                Institution = x.Institution.Name ?? "-",
                                InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                                    ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : ""
                            })
                            .ToListAsync();
            var technicalStudies = await _context.WorkerTechnicalStudies.Where(x => x.UserId == UserId)
                            .Select(x => new GenericStudiesTemplate
                            {
                                Specialty = x.Specialty,
                                StudyType = "Estudio Técnico",
                                Institution = x.Institution.Name ?? "-",
                                InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                                    ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : ""
                            })
                            .ToListAsync();
            var bachelorStudies = await _context.WorkerBachelorDegrees.Where(x => x.UserId == UserId)
                            .Select(x => new GenericStudiesTemplate
                            {
                                Specialty = x.Specialty,
                                StudyType = "Bachiller",
                                Institution = x.Institution.Name ?? "-",
                                InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                                    ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : ""
                            })
                            .ToListAsync();
            var professionalTitlesStudies = await _context.WorkerProfessionalTitles.Where(x => x.UserId == UserId)
                            .Select(x => new GenericStudiesTemplate
                            {
                                Specialty = x.Specialty,
                                StudyType = "Titulo Profesional",
                                Institution = x.Institution.Name ?? "-",
                                InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                                    ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : ""
                            })
                            .ToListAsync();
            var professionalSchoolsStudies = await _context.WorkerProfessionalSchools.Where(x => x.UserId == UserId)
                            .Select(x => new GenericStudiesTemplate
                            {
                                Specialty = x.Specialty,
                                StudyType = "Colegiatura",
                                Institution = x.Institution.Name ?? "-",
                                InstitutionType = "-"
                            })
                            .ToListAsync();
            var masterStudies = await _context.WorkerMasterDegrees.Where(x => x.UserId == UserId)
                            .Select(x => new GenericStudiesTemplate
                            {
                                Specialty = x.Specialty,
                                StudyType = "Maestria",
                                Institution = x.Institution.Name ?? "-",
                                InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                                    ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : ""
                            })
                            .ToListAsync();
            var doctoralStudies = await _context.WorkerDoctoralDegrees.Where(x => x.UserId == UserId)
                            .Select(x => new GenericStudiesTemplate
                            {
                                Specialty = x.Specialty,
                                StudyType = "Doctorado",
                                Institution = x.Institution.Name ?? "-",
                                InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                                    ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : ""
                            })
                            .ToListAsync();

            data.AddRange(doctoralStudies);
            data.AddRange(masterStudies);
            data.AddRange(professionalSchoolsStudies);
            data.AddRange(professionalTitlesStudies);
            data.AddRange(bachelorStudies);
            data.AddRange(technicalStudies);
            data.AddRange(schoolStudies);

            var recordsTotal = data.Count();

            var resultData = data
                    .Select(x => new
                    {
                        x.Specialty,
                        x.StudyType,
                        x.Institution,
                        x.InstitutionType
                    }).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = resultData,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudiesByStudyLevelDatatable(DataTablesStructs.SentParameters sentParameters, string search, int? studyLevel = 0, ClaimsPrincipal user = null, string expeditionDate = null, Guid? countryId = null, Guid? institutionId = null, Guid? termId = null)
        {
            switch (studyLevel)
            {
                case ConstantHelpers.STUDIES_LEVEL.DOCTORAL:
                    return await GetDoctorDatatable(sentParameters, search, true, user, expeditionDate, countryId, institutionId, termId);
                case ConstantHelpers.STUDIES_LEVEL.MASTER:
                    return await GetMasterDatatable(sentParameters, search, true, user, expeditionDate, countryId, institutionId, termId);
                case ConstantHelpers.STUDIES_LEVEL.PROFESSIONAL:
                    return await GetProfessionalDatatable(sentParameters, search, true, user, expeditionDate, countryId, institutionId, termId);
                case ConstantHelpers.STUDIES_LEVEL.BACHELOR:
                    return await GetBachelorDatatable(sentParameters, search, true, user, expeditionDate, countryId, institutionId, termId);
                case ConstantHelpers.STUDIES_LEVEL.TECHNICAL:
                    return await GetTechnicalDatatable(sentParameters, search, true, user, expeditionDate, countryId, institutionId, termId);
                case ConstantHelpers.STUDIES_LEVEL.ELEMENTARYSCHOOL:
                    return await GetSchoolDegreesDatatable(sentParameters, search, true, user, expeditionDate, countryId, institutionId, termId);
                default:
                    return await GetSchoolDegreesDatatable(sentParameters, search, true, user, expeditionDate, countryId, institutionId, termId);
            }
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassesWithoutAttendance(DataTablesStructs.SentParameters parameters, Guid termId, Guid? academicDepartmentId, string search, DateTime endTime, ClaimsPrincipal user)
        {
            var query = _context.Teachers.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId)).AsNoTracking();

            query = query.Where(x => x.TeacherSchedules.Any(y => y.ClassSchedule.Classes.Any(z => !z.IsDictated && z.EndTime <= endTime)));

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.FullName.ToLower().Contains(search.Trim().ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.UserId,
                    username = x.User.UserName,
                    name = x.User.FullName,
                    academicDepartment = x.AcademicDepartment.Name ?? "-"
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<List<ScheduleTemplate>> GetTeacherCompleteSchedule(Guid termId, string teacherId, DateTime start, DateTime end)
        {
            var model = new List<ScheduleTemplate>();

            //C. Lectiva
            var wordLoad = await _context.Classes.Where(x => x.Section.CourseTerm.TermId == termId && x.ClassSchedule.TeacherSchedules.Any(y => y.TeacherId == teacherId) && x.StartTime >= start && x.EndTime <= end)
                .Select(x => new ScheduleTemplate
                {
                    Id = x.Id,
                    Title = $"{x.ClassSchedule.Section.CourseTerm.Course.Code}-{x.ClassSchedule.Section.CourseTerm.Course.Name} ({x.ClassSchedule.Section.Code})",
                    Description = x.Classroom.Description,
                    AllDay = false,
                    Start = x.StartTime.ToDefaultTimeZone().ToString("yyyy-MM-dd HH:mm:ss"),
                    End = x.EndTime.ToDefaultTimeZone().ToString("yyyy-MM-dd HH:mm:ss"),
                    Type = 1
                })
                .ToListAsync();

            model.AddRange(wordLoad);

            //c. No Lectiva
            var nonTeachingLoad = await _context.NonTeachingLoads.Where(x => x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE && x.TeacherId == teacherId)
                .Where(x => (start >= x.StartDate && end <= x.StartDate) ||
                (start >= x.StartDate && x.EndDate >= end) ||
                (end <= x.EndDate && x.StartDate <= end))
                .ToListAsync();

            var nonTeachingLoadIds = nonTeachingLoad.Select(x => x.Id).ToList();
            if (nonTeachingLoad.Any())
            {
                var dates = new List<DateTime>();

                for (DateTime date = start.Date; date <= end.Date; date = date.AddDays(1))
                    dates.Add(date);

                var schedules = await _context.NonTeachingLoadSchedules.Where(x => nonTeachingLoadIds.Contains(x.NonTeachingLoadId)).ToListAsync();

                var nonTeachingLoadSchedules = schedules
                    .Select(x => new ScheduleTemplate
                    {
                        Id = x.Id,
                        //Title =  nonTeachingLoad.Where(y=>y.Id == x.NonTeachingLoadId).Select(x=>x.Name).FirstOrDefault(),
                        Title = "Carga N.L.",
                        Description = nonTeachingLoad.Where(y => y.Id == x.NonTeachingLoadId).Select(x => x.Name).FirstOrDefault(),
                        AllDay = false,
                        Start = dates.Where(y => y.DayOfWeek == ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay)).FirstOrDefault().Date.Add(x.StartTimeLocal).ToString("yyyy-MM-dd HH:mm:ss"),
                        End = dates.Where(y => y.DayOfWeek == ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay)).FirstOrDefault().Date.Add(x.EndTimeLocal).ToString("yyyy-MM-dd HH:mm:ss"),
                        Type = 2
                    })
                    .ToList();

                model.AddRange(nonTeachingLoadSchedules);
            }

            return model;
        }

        public async Task<object> GetMagisterByAcademicDepartmentChart(Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
        {
            var query = _context.WorkerMasterDegrees.AsNoTracking();

            var teachers = _context.Teachers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    teachers = teachers.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (academicDepartmentId != null) teachers = teachers.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            teachers = teachers.Where(x => query.Any(y => y.UserId == x.UserId));

            var data = await _context.AcademicDepartments
                .Select(x => new
                {
                    AcademicDepartment = x.Name,
                    Count = teachers.Where(y => y.AcademicDepartmentId == x.Id).Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.AcademicDepartment)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.AcademicDepartment).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<object> GetDoctorsByAcademicDepartmentChart(Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
        {
            var query = _context.WorkerDoctoralDegrees.AsNoTracking();

            var teachers = _context.Teachers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    teachers = teachers.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (academicDepartmentId != null) teachers = teachers.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            teachers = teachers.Where(x => query.Any(y => y.UserId == x.UserId));

            var data = await _context.AcademicDepartments
                .Select(x => new
                {
                    AcademicDepartment = x.Name,
                    Count = teachers.Where(y => y.AcademicDepartmentId == x.Id).Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.AcademicDepartment)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.AcademicDepartment).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<object> GetSecondSpecialityByAcademicDepartmentChart(Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
        {
            var query = _context.WorkerSecondSpecialties.AsNoTracking();

            var teachers = _context.Teachers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    teachers = teachers.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (academicDepartmentId != null) teachers = teachers.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            teachers = teachers.Where(x => query.Any(y => y.UserId == x.UserId));

            var data = await _context.AcademicDepartments
                .Select(x => new
                {
                    AcademicDepartment = x.Name,
                    Count = teachers.Where(y => y.AcademicDepartmentId == x.Id).Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.AcademicDepartment)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.AcademicDepartment).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<List<UserGenericStudiesTemplate>> GetAllUserGenericStudies(int? studyLevel = 0, ClaimsPrincipal user = null, Guid? termId = null)
        {
            var teachers = _context.Teachers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    teachers = teachers.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            var doctoral = _context.WorkerDoctoralDegrees.Where(x => teachers.Any(y => y.UserId == x.UserId)).AsNoTracking();
            var master = _context.WorkerMasterDegrees.Where(x => teachers.Any(y => y.UserId == x.UserId)).AsNoTracking();
            var professional = _context.WorkerProfessionalTitles.Where(x => teachers.Any(y => y.UserId == x.UserId)).AsNoTracking();
            var bachelor = _context.WorkerBachelorDegrees.Where(x => teachers.Any(y => y.UserId == x.UserId)).AsNoTracking();
            var technical = _context.WorkerTechnicalStudies.Where(x => teachers.Any(y => y.UserId == x.UserId)).AsNoTracking();
            var elementary = _context.WorkerSchoolDegrees.Where(x => teachers.Any(y => y.UserId == x.UserId)).AsNoTracking();

            if(termId.HasValue && termId != Guid.Empty)
            {
                doctoral = doctoral.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
                master = master.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
                professional = professional.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
                bachelor = bachelor.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
                technical = technical.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
                elementary = elementary.Where(x => x.User.Teachers.Any(y => y.TeacherSections.Any(z => z.Section.CourseTerm.TermId == termId)));
            }

            switch (studyLevel)
            {
                case ConstantHelpers.STUDIES_LEVEL.DOCTORAL:
                    return await doctoral
                        .Select(x => new UserGenericStudiesTemplate
                        {
                            UserName = x.User.UserName,
                            FullName = x.User.FullName,
                            Email = x.User.Email,
                            Specialty = x.Specialty,
                            Institution = x.Institution.Name,
                            InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                        }).ToListAsync();
                case ConstantHelpers.STUDIES_LEVEL.MASTER:
                    return await master
                        .Select(x => new UserGenericStudiesTemplate
                        {
                            UserName = x.User.UserName,
                            FullName = x.User.FullName,
                            Email = x.User.Email,
                            Specialty = x.Specialty,
                            Institution = x.Institution.Name,
                            InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                        }).ToListAsync();
                case ConstantHelpers.STUDIES_LEVEL.PROFESSIONAL:
                    return await professional
                        .Select(x => new UserGenericStudiesTemplate
                        {
                            UserName = x.User.UserName,
                            FullName = x.User.FullName,
                            Email = x.User.Email,
                            Specialty = x.Specialty,
                            Institution = x.Institution.Name,
                            InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                        }).ToListAsync();
                case ConstantHelpers.STUDIES_LEVEL.BACHELOR:
                    return await bachelor
                        .Select(x => new UserGenericStudiesTemplate
                        {
                            UserName = x.User.UserName,
                            FullName = x.User.FullName,
                            Email = x.User.Email,
                            Specialty = x.Specialty,
                            Institution = x.Institution.Name,
                            InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                        }).ToListAsync();
                case ConstantHelpers.STUDIES_LEVEL.TECHNICAL:
                    return await technical
                        .Select(x => new UserGenericStudiesTemplate
                        {
                            UserName = x.User.UserName,
                            FullName = x.User.FullName,
                            Email = x.User.Email,
                            Specialty = x.Specialty,
                            Institution = x.Institution.Name,
                            InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                        }).ToListAsync();
                case ConstantHelpers.STUDIES_LEVEL.ELEMENTARYSCHOOL:
                    return await elementary
                        .Select(x => new UserGenericStudiesTemplate
                        {
                            UserName = x.User.UserName,
                            FullName = x.User.FullName,
                            Email = x.User.Email,
                            Specialty = ConstantHelpers.STUDY_TYPES.VALUES.ContainsKey(x.StudyType) ?
                                ConstantHelpers.STUDY_TYPES.VALUES[x.StudyType] : "-",
                            Institution = x.Institution.Name,
                            InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                        }).ToListAsync();
                default:
                    return await elementary
                        .Select(x => new UserGenericStudiesTemplate
                        {
                            UserName = x.User.UserName,
                            FullName = x.User.FullName,
                            Email = x.User.Email,
                            Specialty = ConstantHelpers.STUDY_TYPES.VALUES.ContainsKey(x.StudyType) ?
                                ConstantHelpers.STUDY_TYPES.VALUES[x.StudyType] : "-",
                            Institution = x.Institution.Name,
                            InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(x.InstitutionType) ?
                            ConstantHelpers.INSTITUTION_TYPES.VALUES[x.InstitutionType] : "-"
                        }).ToListAsync();
            }
        }

        public async Task<List<TeacherReportTemplate>> GetAllTeacherBySexAge(int? ageRange = null, int? sex = null, ClaimsPrincipal user = null)
        {
            var today = DateTime.UtcNow.AddHours(-5);

            var query = _context.Teachers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (ageRange != null)
            {
                int minAge = ConstantHelpers.AGE_RANGES.VALUES[ageRange.Value].MinAge;
                int maxAge = ConstantHelpers.AGE_RANGES.VALUES[ageRange.Value].MaxAge;

                var minDateTime = today.AddYears(-maxAge);
                var maxDateTime = today.AddYears(-minAge);

                query = query.Where(q => q.User.BirthDate >= minDateTime && q.User.BirthDate <= maxDateTime);
            }

            if (sex != null)
                query = query.Where(q => q.User.Sex == sex.Value);


            var data = await query
                .Select(x => new TeacherReportTemplate
                {
                    UserName = x.User.UserName,
                    FullName = x.User.FullName,
                    Email = x.User.Email,
                    Dni = x.User.Dni,
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.User.Sex) ? ConstantHelpers.SEX.VALUES[x.User.Sex] : "",
                    BirthDate = x.User.BirthDate.ToString("dd 'de' MMMM 'del' yyyy"),
                    Age = x.User.BirthDate.AddYears((today.Year - x.User.BirthDate.Year)) > today ? (today.Year - x.User.BirthDate.Year) - 1 : (today.Year - x.User.BirthDate.Year)
                })
                .OrderBy(x => x.FullName)
                .ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassAttendanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? academicDepartmentId = null, string search = null)
        {
            var query = _context.Classes.AsNoTracking();

            var teacherSchedules = _context.TeacherSchedules.AsNoTracking();

            var teacherSections = _context.TeacherSections.Where(x=>x.Section.Classes.Any(Z=>Z.ClassSchedule.TeacherSchedules.Any(ts=>ts.TeacherId == x.TeacherId))).AsNoTracking();

            if (academicDepartmentId != null)
            {
                teacherSchedules = teacherSchedules.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);
                teacherSections = teacherSections.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);
            }

            if (termId != null)
            {
                teacherSchedules = teacherSchedules.Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId);
                teacherSections = teacherSections.Where(x => x.Section.CourseTerm.TermId == termId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                string searchValue = search.ToUpper().Trim();
                teacherSections = teacherSections.Where(x => x.Teacher.User.UserName.ToUpper().Contains(searchValue)
                                    || x.Teacher.User.Name.ToUpper().Contains(searchValue)
                                    || x.Teacher.User.PaternalSurname.ToUpper().Contains(searchValue)
                                    || x.Teacher.User.MaternalSurname.ToUpper().Contains(searchValue)
                                    || x.Teacher.User.FullName.ToUpper().Contains(searchValue)
                                    || x.Section.CourseTerm.Course.Name.ToUpper().Contains(searchValue));
            }

            int recordsFiltered = await teacherSections
                .Select(x => new { x.TeacherId, x.SectionId })
                .Distinct()
                .CountAsync();

            var data = await teacherSections
                .GroupBy(x => new
                {
                    x.Teacher.User.UserName,
                    x.Teacher.User.FullName,
                    x.TeacherId,
                    AcademicDepartment = x.Teacher.AcademicDepartment.Name,
                    Term = x.Section.CourseTerm.Term.Name,
                    Section = x.Section.Code,
                    x.SectionId,
                    Course = x.Section.CourseTerm.Course.Name
                })
                .Select(x => new
                {
                    x.Key.AcademicDepartment,
                    x.Key.UserName,
                    x.Key.FullName,
                    x.Key.Term,
                    x.Key.Course,
                    x.Key.Section,
                    scheduled = query.Where(y => y.SectionId == x.Key.SectionId && teacherSchedules.Any(z => z.ClassScheduleId == y.ClassScheduleId && z.TeacherId == x.Key.TeacherId)).Count(),
                    dictated = query.Where(y => y.SectionId == x.Key.SectionId && y.IsDictated && teacherSchedules.Any(z => z.ClassScheduleId == y.ClassScheduleId && z.TeacherId == x.Key.TeacherId)).Count()
                })
                .OrderBy(x => x.AcademicDepartment)
                .ThenBy(x => x.FullName)
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherContractsDatatable(DataTablesStructs.SentParameters sentParameters, int year, string search = null)
        {
            var teachers = _context.Teachers.AsNoTracking();

            var today = DateTime.UtcNow.AddHours(-5);

            var query = _context.ScaleResolutions
                        .Where(x => x.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos")
                        .AsNoTracking();

            query = query.Where(x => x.EndDate == null || x.EndDate > today);

            query = query.Where(x => x.BeginDate.HasValue && x.BeginDate.Value.AddYears(year) < today);

            query = query.Where(x => teachers.Any(y => y.UserId == x.UserId));

            if (!string.IsNullOrEmpty(search))
            {
                var searchvalue = search.ToUpper().Trim();

                query = query.Where(x => x.User.UserName.ToUpper().Contains(searchvalue)
                            || x.User.FullName.ToUpper().Contains(searchvalue)
                            || x.User.Name.ToUpper().Contains(searchvalue)
                            || x.User.PaternalSurname.ToUpper().Contains(searchvalue)
                            || x.User.MaternalSurname.ToUpper().Contains(searchvalue));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    x.User.Name,
                    x.User.FullName,
                    BeginDate = x.BeginDate.Value.ToLocalDateFormat(),
                    EndDate = x.EndDate.HasValue ? x.EndDate.ToLocalDateFormat() : "Indefinido"
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

        public async Task<Teacher> GetIgnoreQueryFilter(string id)
        {
            return await _context.Teachers.IgnoreQueryFilters().Where(x => x.UserId == id).FirstOrDefaultAsync();
        }


        public async Task<DataTablesStructs.ReturnedData<TeacherInformationTemplate>> GetQuantityOfAllTeachersByTermIdDatatable(DataTablesStructs.SentParameters parameters, Guid? termId = null, Guid? academicDepartmentId = null, Guid? dedicationId = null, Guid? categoryId = null, Guid? conditionId = null)
        {
            Expression<Func<TeacherInformationTemplate, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.AcademicDepartment);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Dedication);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Condition);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Category);
                    break;
            }

            var teacherSections = _context.TeacherSections.AsNoTracking();

            if (termId != null)
                teacherSections = teacherSections.Where(x => x.Section.CourseTerm.TermId == termId);

            if (academicDepartmentId != null)
                teacherSections = teacherSections.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);

            if (dedicationId != null)
                teacherSections = teacherSections.Where(x => x.Teacher.TeacherDedicationId == dedicationId);

            if (conditionId != null)
                teacherSections = teacherSections.Where(x => x.Teacher.User.WorkerLaborInformation.WorkerLaborConditionId == conditionId);

            if (categoryId != null)
                teacherSections = teacherSections.Where(x => x.Teacher.User.WorkerLaborInformation.WorkerLaborCategoryId == categoryId);

            var teachers = _context.Teachers.AsNoTracking();

            teachers = teachers.Where(x => teacherSections.Any(y => y.TeacherId == x.UserId));

            var recordsFiltered = await teachers
                .GroupBy(x => new { x.AcademicDepartmentId, x.TeacherDedicationId, x.User.WorkerLaborInformation.WorkerLaborConditionId, x.User.WorkerLaborInformation.WorkerLaborCategoryId })
                .Select(x => x.Key.AcademicDepartmentId)
                .CountAsync();

            //no usar string interpolation por alguna razon el query no me funciona con eso, y cambia la estrucutra del mismo por eso que se cae
            var data = await teachers
                .Select(x => new
                {
                    AcademicDepartment = x.AcademicDepartment.Name,
                    x.AcademicDepartmentId,
                    Dedication = x.TeacherDedication.Name,
                    x.TeacherDedicationId,
                    Condition = "Reg. " + x.User.WorkerLaborInformation.WorkerLaborCondition.WorkerLaborRegime.Name + " - " + x.User.WorkerLaborInformation.WorkerLaborCondition.Name,
                    x.User.WorkerLaborInformation.WorkerLaborConditionId,
                    Category = "Reg. " + x.User.WorkerLaborInformation.WorkerLaborCategory.WorkerLaborRegime.Name + " - " + x.User.WorkerLaborInformation.WorkerLaborCategory.Name,
                    x.User.WorkerLaborInformation.WorkerLaborCategoryId
                })
                .GroupBy(x => new { x.AcademicDepartment, x.AcademicDepartmentId, x.Dedication, x.TeacherDedicationId, x.Condition, x.WorkerLaborConditionId, x.Category, x.WorkerLaborCategoryId })
                .Select(x => new TeacherInformationTemplate
                {
                    AcademicDepartment = x.Key.AcademicDepartment,
                    Dedication = x.Key.Dedication,
                    Condition = x.Key.Condition,
                    Category = x.Key.Category,
                    Total = x.Count(),
                })
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<TeacherInformationTemplate>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<object> GetQuantityOfAllTeachersByTermIdChart(Guid? termId = null, Guid? academicDepartmentId = null, Guid? dedicationId = null, Guid? categoryId = null, Guid? conditionId = null)
        {
            var teacherSections = _context.TeacherSections.AsQueryable();

            if (termId != null)
                teacherSections = teacherSections.Where(x => x.Section.CourseTerm.TermId == termId);

            if (academicDepartmentId != null)
                teacherSections = teacherSections.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);

            if (dedicationId != null)
                teacherSections = teacherSections.Where(x => x.Teacher.TeacherDedicationId == dedicationId);

            if (conditionId != null)
                teacherSections = teacherSections.Where(x => x.Teacher.User.WorkerLaborInformation.WorkerLaborConditionId == conditionId);

            if (categoryId != null)
                teacherSections = teacherSections.Where(x => x.Teacher.User.WorkerLaborInformation.WorkerLaborCategoryId == categoryId);


            var teachers = _context.Teachers.AsNoTracking();

            teachers = teachers.Where(x => teacherSections.Any(y => y.TeacherId == x.UserId));

            var academicDepartments = await _context.AcademicDepartments
                .Select(x => new
                {
                    AcademicDepartment = x.Name,
                    Teachers = teachers.Where(y => y.AcademicDepartmentId == x.Id).Select(y => y.UserId).Count()
                }).ToListAsync();

            var result = new
            {
                categories = academicDepartments.Select(x => x.AcademicDepartment).ToList(),
                data = academicDepartments.Select(x => x.Teachers).ToList()
            };

            return result;
        }

        public async Task<object> GetQuantityOfAllHiredTeachersByTermIdChart(Guid termId)
        {
            var query = _context.TeacherSections
              .Where(x => x.Section.CourseTerm.TermId == termId)
              .AsNoTracking();

            var academicDepartments = await _context.AcademicDepartments
                .Select(x => new
                {
                    x.Name,
                    Count = query.Where(y => y.Teacher.AcademicDepartmentId == x.Id).Select(y => y.TeacherId).Distinct().Count()
                }).ToListAsync();

            var result = new
            {
                categories = academicDepartments.Select(x => x.Name).ToList(),
                data = academicDepartments.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<object> GetQuantityOfAllTeachersByAcademicDegreeChart(Guid? termId = null, Guid? academicDepartmentId = null)
        {
            var academicDegrees = ConstantHelpers.USER_LABOR_INFORMATION.TEACHER_LEVEL_STUDIE.VALUES.OrderByDescending(x => ConstantHelpers.USER_LABOR_INFORMATION.TEACHER_LEVEL_STUDIE.HIERARCHICAL_ORDER[x.Key]).ToList();

            var query = _context.TeacherSections.AsNoTracking();

            if (termId != null)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (academicDepartmentId != null)
                query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);

            var teachersId = await query.Select(x => x.TeacherId).Distinct().ToListAsync();

            var doctoralCount = await _context.WorkerDoctoralDegrees.CountAsync(x => teachersId.Any(y => y == x.UserId));
            var masterCount = await _context.WorkerMasterDegrees.CountAsync(x => teachersId.Any(y => y == x.UserId));
            var professionalCount = await _context.WorkerProfessionalTitles.CountAsync(x => teachersId.Any(y => y == x.UserId));
            var bachelorCount = await _context.WorkerBachelorDegrees.CountAsync(x => teachersId.Any(y => y == x.UserId));
            var technicalCount = await _context.WorkerTechnicalStudies.CountAsync(x => teachersId.Any(y => y == x.UserId));
            var elementaryCount = await _context.WorkerSchoolDegrees.CountAsync(x => teachersId.Any(y => y == x.UserId));

            var studies = new List<string>
            {
                "Doctorado", "Maestría", "Titutlo Profesional" , "Bachiller" , "Estudios Tecnicos", "Secundaria/Primaria"
            };

            var studiesCount = new List<int>
            {
                doctoralCount , masterCount , professionalCount , bachelorCount , technicalCount , elementaryCount
            };


            var result = new
            {
                categories = studies,
                data = studiesCount
            };


            return result;
        }

        public async Task<object> GetAllTeacherByDedicationChart(Guid? academicDepartmentId = null, Guid? regimeId = null)
        {
            var teachers = _context.Teachers.AsNoTracking();
            var query = _context.TeacherDedication.AsNoTracking();

            if (academicDepartmentId != null)
                teachers = teachers.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (regimeId != null)
            {
                teachers = teachers.Where(x => x.TeacherDedication.WorkerLaborRegimeId == regimeId);
                query = query.Where(x => x.WorkerLaborRegimeId == regimeId);
            }


            var data = await query
                .Where(x => x.Status == ConstantHelpers.STATES.ACTIVE)
                .Select(x => new
                {
                    x.Name,
                    Count = teachers.Count(y => y.TeacherDedicationId == x.Id)
                }).ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Name).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherByAcademicDepartmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.User.PaternalSurname); break;
                case "3":
                    orderByPredicate = ((x) => x.User.MaternalSurname); break;
                case "4":
                    orderByPredicate = ((x) => x.AcademicDepartment.Name); break;
            }

            var query = _context.Teachers
                .AsQueryable();

            if (academicDepartmentId != null)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    userName = x.User.UserName,
                    name = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    maternalSurname = x.User.MaternalSurname,
                    academicDepartment = x.AcademicDepartment.Name,
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<object> GetTeacherByAcademicDepartmentChart(Guid? academicDepartmentId = null)
        {
            var query = _context.Teachers
                            .AsQueryable();

            if (academicDepartmentId != null)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            var teachers = await query
                .GroupBy(x => x.AcademicDepartmentId)
                .Select(x => new
                {
                    AcademicDepartmentId = x.Key,
                    Count = x.Count()
                })
                .ToListAsync();

            var academicDepartments = await _context.AcademicDepartments
                .Select(x => new
                {
                    x.Name,
                    x.Id
                }).ToListAsync();

            var data = academicDepartments
                .Select(x => new
                {
                    AcademicDepartment = x.Name,
                    Teachers = teachers.Where(y => y.AcademicDepartmentId == x.Id).Select(x => x.Count).FirstOrDefault()
                }).ToList();

            var result = new
            {
                categories = data.Select(x => x.AcademicDepartment).ToList(),
                data = data.Select(x => x.Teachers).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherForeignDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, bool? foreign = null, ClaimsPrincipal user = null)
        {
            var query = _context.TeacherSections.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Teacher.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (foreign != null)
            {
                if (foreign.Value)
                {
                    query = query.Where(x => x.Teacher.User.Department.Country.Code != ConstantHelpers.CountryCode.Current);
                }
                else
                {
                    query = query.Where(x => x.Teacher.User.Department.Country.Code == ConstantHelpers.CountryCode.Current);
                }
            }


            int recordsFiltered = await query
                .Select(x => new { x.Section.CourseTerm.TermId })
                .Distinct()
                .CountAsync();

            var preData = await query
                .Select(x => new
                {
                    x.TeacherId,
                    Term = x.Section.CourseTerm.Term.Name,
                    TermYear = x.Section.CourseTerm.Term.Year,
                    TermNumber = x.Section.CourseTerm.Term.Number,
                    x.Section.CourseTerm.TermId,
                    Country = x.Teacher.User.Department.Country.Code,
                    x.Teacher.User.Department.CountryId
                })
                .Distinct()
                .GroupBy(x => new { x.TermId, x.Term, x.TermYear, x.TermNumber, x.Country, x.CountryId })
                .Select(x => new
                {
                    x.Key.TermYear,
                    x.Key.TermNumber,
                    x.Key.Term,
                    x.Key.Country,
                    x.Key.CountryId,
                    Count = x.Count()
                })
                .ToListAsync();

            //Nacional y extranjero
            var data = preData
                .OrderByDescending(x => x.TermYear)
                .ThenByDescending(x => x.TermNumber)
                .GroupBy(x => new { x.Term })
                .Select(x => new
                {
                    x.Key.Term,
                    National = x.Where(y => y.Country == ConstantHelpers.CountryCode.Current).Sum(y => y.Count),
                    Foreign = x.Where(y => y.Country != ConstantHelpers.CountryCode.Current).Sum(y => y.Count)
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetTeacherForeignChart(Guid? termId = null, bool? foreign = null, ClaimsPrincipal user = null)
        {
            var query = _context.TeacherSections.AsNoTracking();


            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Teacher.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            var totalTeachers = await query.Select(x => x.TeacherId).Distinct().CountAsync();

            if (foreign != null)
            {
                if (foreign.Value)
                {
                    query = query.Where(x => x.Teacher.User.Department.Country.Code != ConstantHelpers.CountryCode.Current);
                }
                else
                {
                    query = query.Where(x => x.Teacher.User.Department.Country.Code == ConstantHelpers.CountryCode.Current);
                }
            }

            var nationalTeachers = await query
                    .Where(x => x.Teacher.User.Department.Country.Code == ConstantHelpers.CountryCode.Current)
                    .Select(x => x.TeacherId).Distinct().CountAsync();
            var nationalTeachersPercentage = totalTeachers == 0 ? 0.0 : Math.Round((nationalTeachers * 100.0) / totalTeachers * 1.0, 2, MidpointRounding.AwayFromZero);

            var foreignTeachers = await query
                    .Where(x => x.Teacher.User.Department.Country.Code != ConstantHelpers.CountryCode.Current)
                    .Select(x => x.TeacherId).Distinct().CountAsync();

            var foreignTeachersPercentage = totalTeachers == 0 ? 0.0 : Math.Round((foreignTeachers * 100.0) / totalTeachers * 1.0, 2, MidpointRounding.AwayFromZero);


            var categoriesData = new List<string>
            {
                "Nacional", "Extranjero"
            };

            var countData = new List<double>
            {
                nationalTeachersPercentage , foreignTeachersPercentage
            };


            var result = new
            {
                categories = categoriesData,
                data = countData
            };

            return result;
        }

        public async Task<TeacherLaborInformationTemplate> GetTeacherLaborTransparencyInformation(string userId)
        {
            var teacherStudies = await _context.WorkerDoctoralDegrees
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    x.User.FullName,
                    x.Specialty,
                    x.InstitutionType,
                    InstitutionName = x.Institution.Name,
                    ExpeditionDate = x.ExpeditionDate.ToLocalDateFormat(),
                    Country = x.StudyCountry.Name,
                    Department = x.StudyDepartment.Name,
                    Province = x.StudyProvince.Name,
                    District = x.StudyDistrict.Name
                })
                .FirstOrDefaultAsync();

            //no tiene doctorado , seguimos maestria
            if (teacherStudies == null)
            {
                teacherStudies = await _context.WorkerMasterDegrees
                    .Where(x => x.UserId == userId)
                    .Select(x => new
                    {
                        x.User.FullName,
                        x.Specialty,
                        x.InstitutionType,
                        InstitutionName = x.Institution.Name,
                        ExpeditionDate = x.ExpeditionDate.ToLocalDateFormat(),
                        Country = x.StudyCountry.Name,
                        Department = x.StudyDepartment.Name,
                        Province = x.StudyProvince.Name,
                        District = x.StudyDistrict.Name
                    })
                    .FirstOrDefaultAsync();
            }

            //no tiene maestria , seguimos Titutlo
            if (teacherStudies == null)
            {
                teacherStudies = await _context.WorkerProfessionalTitles
                    .Where(x => x.UserId == userId)
                    .Select(x => new
                    {
                        x.User.FullName,
                        x.Specialty,
                        x.InstitutionType,
                        InstitutionName = x.Institution.Name,
                        ExpeditionDate = x.ExpeditionDate.ToLocalDateFormat(),
                        Country = x.StudyCountry.Name,
                        Department = x.StudyDepartment.Name,
                        Province = x.StudyProvince.Name,
                        District = x.StudyDistrict.Name
                    })
                    .FirstOrDefaultAsync();
            }

            //no tiene titulo , seguimos Bechiller
            if (teacherStudies == null)
            {
                teacherStudies = await _context.WorkerBachelorDegrees
                    .Where(x => x.UserId == userId)
                    .Select(x => new
                    {
                        x.User.FullName,
                        x.Specialty,
                        x.InstitutionType,
                        InstitutionName = x.Institution.Name,
                        ExpeditionDate = x.ExpeditionDate.ToLocalDateFormat(),
                        Country = x.StudyCountry.Name,
                        Department = x.StudyDepartment.Name,
                        Province = x.StudyProvince.Name,
                        District = x.StudyDistrict.Name
                    })
                    .FirstOrDefaultAsync();
            }

            //no tiene bachiller , seguimos Tecnico
            if (teacherStudies == null)
            {
                teacherStudies = await _context.WorkerTechnicalStudies
                    .Where(x => x.UserId == userId)
                    .Select(x => new
                    {
                        x.User.FullName,
                        x.Specialty,
                        x.InstitutionType,
                        InstitutionName = x.Institution.Name,
                        ExpeditionDate = x.ExpeditionDate.ToLocalDateFormat(),
                        Country = x.StudyCountry.Name,
                        Department = x.StudyDepartment.Name,
                        Province = x.StudyProvince.Name,
                        District = x.StudyDistrict.Name
                    })
                    .FirstOrDefaultAsync();
            }

            //no tiene Tecnico , seguimos Colegio
            if (teacherStudies == null)
            {
                teacherStudies = await _context.WorkerSchoolDegrees
                    .Where(x => x.UserId == userId)
                    .Select(x => new
                    {
                        x.User.FullName,
                        x.Specialty,
                        x.InstitutionType,
                        InstitutionName = x.Institution.Name,
                        ExpeditionDate = x.ExpeditionDate.ToLocalDateFormat(),
                        Country = x.StudyCountry.Name,
                        Department = x.StudyDepartment.Name,
                        Province = x.StudyProvince.Name,
                        District = x.StudyDistrict.Name
                    })
                    .FirstOrDefaultAsync();
            }
            var fullName = await _context.Users.Where(x => x.Id == userId).Select(x => x.FullName).FirstOrDefaultAsync();

            var workerLaborInformation = await _context.WorkerLaborInformation.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            var teacherInfo = new TeacherLaborInformationTemplate
            {
                FullName = fullName,
                Institution = "",
                InstitutionType = "",
                Specialty = "",
                ExpeditionDate = "",
                Country = "",
                Department = "",
                Province = "",
                District = ""
            };

            string law30220Config = await GetConfigurationValue(ConstantHelpers.Configuration.ScaleConfiguration.LAW_30220);

            if (workerLaborInformation != null)
            {
                DateTime workerlaw30220Date = DateTime.UtcNow;

                var converterResult = DateTime.TryParse(law30220Config, out workerlaw30220Date);
                if (converterResult && workerLaborInformation.EntryDate.HasValue)
                {
                    teacherInfo.JoinedBeforeLaw30220 = workerLaborInformation.EntryDate.Value < workerlaw30220Date;
                }
            }

            if (teacherStudies != null)
            {
                teacherInfo.Institution = teacherStudies.InstitutionName;
                teacherInfo.InstitutionType = ConstantHelpers.INSTITUTION_TYPES.VALUES.ContainsKey(teacherStudies.InstitutionType) ? ConstantHelpers.INSTITUTION_TYPES.VALUES[teacherStudies.InstitutionType] : "-";
                teacherInfo.Specialty = teacherStudies.Specialty;
                teacherInfo.ExpeditionDate = teacherStudies.ExpeditionDate;
                teacherInfo.Country = teacherStudies.Country;
                teacherInfo.Department = teacherStudies.Department ?? "-";
                teacherInfo.Province = teacherStudies.Province ?? "-";
                teacherInfo.District = teacherStudies.District ?? "-";
            }

            return teacherInfo;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPreProfessionalInternshipSupervisorDatatable(DataTablesStructs.SentParameters parameters, Guid? academicDepartmentId, string search)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.INTERNSHIP_SUPERVISOR)); break;
                default:
                    orderByPredicate = ((x) => x.User.FullName); break;
            }

            var query = _context.Teachers.AsNoTracking();

            if (academicDepartmentId != Guid.Empty && academicDepartmentId.HasValue)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                query = query.Where(x => x.User.FullName.ToLower().Trim().Contains(search) || x.User.UserName.Trim().ToLower().Contains(search));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.FullName,
                    academicDepartment = x.AcademicDepartment.Name,
                    isSupervisor = x.User.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.INTERNSHIP_SUPERVISOR)
                })

                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<Select2Structs.ResponseParameters> GetTeachersByAcademicDepartmentSelect2(Select2Structs.RequestParameters requestParameters, Guid? id = null, string searchValue = null, int? status = null)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var query = _context.Teachers.AsNoTracking();

            //Teacher
            var tutorTeacherWithoutAcademicChargeConfig = await _context.Configurations
                .Where(x => x.Key == ConstantHelpers.Configuration.Tutoring.TUTOR_TEACHER_WITHOUT_ACADEMICCHARGE)
                .FirstOrDefaultAsync();

            bool tutorTeacherWithoutAcademicChargeEnabled = false;

            if (tutorTeacherWithoutAcademicChargeConfig != null)
            {
                bool isValid = bool.TryParse(tutorTeacherWithoutAcademicChargeConfig.Value, out tutorTeacherWithoutAcademicChargeEnabled);
            }

            //Si no esta permitido los docentes sin carga académica
            //Entonces el docente será filtrado por solo los que estan asignados a una seccion en el periodo actual
            if (!tutorTeacherWithoutAcademicChargeEnabled)
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == term.Id)).AsNoTracking();

            //Si se especifica un departamento academico
            if (id != null)
                query = query.Where(x => x.AcademicDepartmentId == id).AsNoTracking();

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue));
                //query = query.WhereSearchValue((x) => new[] { x.User.FullName }, searchValue);
            }

            Expression<Func<Teacher, Select2Structs.Result>> selectPredicate = (x) => new Select2Structs.Result
            {
                Id = x.UserId,
                Text = $"{x.User.FullName} - {x.TeacherDedication.Name}"
            };

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersManagementDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null, string search = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "3":
                    orderByPredicate = ((x) => x.User.Document); break;
                case "4":
                    orderByPredicate = ((x) => x.AcademicDepartmentId.HasValue ? x.AcademicDepartment.Name : ""); break;
                case "5":
                    orderByPredicate = ((x) => x.User.PhoneNumber); break;
                case "6":
                    orderByPredicate = ((x) => x.User.UserLogins.Select(y => y.LastLogin));
                    break;
                default:
                    break;
            }

            var query = _context.Teachers.AsNoTracking();

            if (academicDepartmentId != null && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                || x.User.FullName.ToUpper().Contains(search.ToUpper())
                || x.User.Document.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.UserId,
                    picture = x.User.Picture,
                    username = x.User.UserName,
                    fullname = x.User.FullName,
                    document = x.User.Document,
                    academicDepartment = x.AcademicDepartmentId.HasValue ? x.AcademicDepartment.Name : "-",
                    lastLogin = x.User.UserLogins.Any() ? x.User.UserLogins.OrderByDescending(y => y.LastLogin).Select(y => y.LastLogin.ToLocalDateFormat()).FirstOrDefault()
                    : "-",
                    roles = x.User.UserRoles.Select(y => y.Role.Name).ToList()
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


        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersWithNonTeachingLoadDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string search, Guid? academicDepartmentId = null, bool? viewAll = false, ClaimsPrincipal user = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.AcademicDepartment.Name); break;
                default:
                    orderByPredicate = ((x) => x.User.FullName); break;
            }

            var query = _context.Teachers.AsNoTracking();

            if(user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    query = query.Where(x => x.AcademicDepartment.AcademicDepartmentDirectorId == userId);
                }
            }

            if (viewAll.HasValue && !viewAll.Value)
            {
                query = query.Where(x => x.NonTeachingLoads.Any(x => x.TermId == termId));
            }

            if (academicDepartmentId.HasValue)
                query = query.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                query = query.Where(x => x.User.FullName.ToLower().Trim().Contains(search) || x.User.UserName.Trim().ToLower().Contains(search));
            }

            var recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.UserId,
                    x.User.UserName,
                    x.User.FullName,
                    AcademicDepartment = x.AcademicDepartment.Name,
                })
                .ToListAsync();

            var teachersHashId = dataDB.Select(x => x.UserId).ToHashSet();

            var nonTeacherSchedules = await _context.NonTeachingLoadSchedules.Where(x => x.NonTeachingLoad.TermId == termId && teachersHashId.Contains(x.NonTeachingLoad.TeacherId))
                .Select(x => new
                {
                    starTime = x.StartTime.ToLocalTimeSpanUtc(),
                    endTime = x.EndTime.ToLocalTimeSpanUtc(),
                    x.NonTeachingLoad.TeacherId
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.UserId,
                    x.UserName,
                    x.FullName,
                    x.AcademicDepartment,
                    hours = nonTeacherSchedules.Where(y=>y.TeacherId == x.UserId).Sum(y=>y.endTime.Subtract(y.starTime).TotalHours).ToString("0.0")
                })
                .ToList();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPercentageTeachersInvestigationProjectDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
        {
            var teachers = _context.Teachers.AsQueryable();

            int totalTeachers = await teachers.CountAsync();

            int totalInvestigationTeachers = await _context.InvestigationProjectMembers
                .Where(x => teachers.Any(y => y.UserId == x.MemberId))
                .Select(x => x.MemberId)
                .Distinct()
                .CountAsync();

            //if (user != null)
            //{
            //    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
            //    {
            //        teachers = teachers.Where(x => x.Career.QualityCoordinatorId == userId);
            //    }
            //}

            //Report
            var data = new List<MemberProjectInvestigationTemplate>
            {
                new MemberProjectInvestigationTemplate { DataReport = "Total de docentes", ValueData = totalTeachers },
                new MemberProjectInvestigationTemplate { DataReport = "Docentes con proyecto de investigación", ValueData = totalInvestigationTeachers },
                new MemberProjectInvestigationTemplate { DataReport = "Docentes sin proyecto de investigación", ValueData = totalTeachers -  totalInvestigationTeachers},
            };

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetPercentageTeachersInvestigationProjectChart(ClaimsPrincipal user = null)
        {
            var teachers = _context.Teachers.AsQueryable();

            int totalTeachers = await teachers.CountAsync();

            int totalInvestigationTeachers = await _context.InvestigationProjectMembers
                .Where(x => teachers.Any(y => y.UserId == x.MemberId))
                .Select(x => x.MemberId)
                .Distinct()
                .CountAsync();

            int totalNotInvestigationTeachers = totalTeachers - totalInvestigationTeachers;

            //if (user != null)
            //{
            //    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
            //    {
            //        teachers = teachers.Where(x => x.Career.QualityCoordinatorId == userId);
            //    }
            //}

            //Report
            var dataReport = new List<StudentReportDoubleTemplate>
            {
                new StudentReportDoubleTemplate { DataReport = "Docentes con proyecto de investigación", ValueData = totalInvestigationTeachers },
                new StudentReportDoubleTemplate { DataReport = "Docentes sin proyecto de investigación", ValueData = totalNotInvestigationTeachers }
            };

            var data = dataReport.Select(x => new
            {
                name = x.DataReport,
                y = x.ValueData
            }).ToList();

            return new { data };
        }

        #endregion
    }
}