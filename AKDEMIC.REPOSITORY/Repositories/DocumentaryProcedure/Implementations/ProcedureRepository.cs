using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class ProcedureRepository : Repository<Procedure>, IProcedureRepository
    {
        public ProcedureRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? procedureCategoryId, Guid? procedureSubcategoryId, string userId, Expression<Func<Procedure, Procedure>> selectPredicate = null, Expression<Func<Procedure, dynamic>> orderByPredicate = null, Func<Procedure, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Procedures.AsNoTracking();

            //if (facultyId != null)
            //{
            //    query = query.Where(x => x.FacultyId == facultyId);
            //}

            if (procedureCategoryId != null)
            {
                query = query.Where(x => x.ProcedureCategoryId == procedureCategoryId);
            }

            if (procedureSubcategoryId != null)
            {
                query = query.Where(x => x.ProcedureSubcategoryId == procedureSubcategoryId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue));
            }
            if (userId != null)
            {
                var roles = await GetRolesByUserRoleUser(userId);
                var student = await GetStudentByUser(userId);
                query = query.Where(x => x.ProcedureRoles.Any(y => roles.Contains(y.Role)) /*&& (student == null || x.FacultyId == null || x.FacultyId == student.Career.FacultyId)*/);
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<Procedure>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private async Task<IEnumerable<ApplicationRole>> GetRolesByUserRoleUser(string userRoleUserId)
        {
            var query = _context.UserRoles
                .Where(x => x.UserId == userRoleUserId)
                .Select(x => new ApplicationRole
                {
                    Id = x.Role.Id
                });

            return await query.ToListAsync();
        }

        private async Task<Student> GetStudentByUser(string studentUserId)
        {
            var query = _context.Students
                .Where(x => x.UserId == studentUserId)
                .Select(x => new Student
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
                    }
                });

            return await query.FirstOrDefaultAsync();
        }

        #endregion

        #region PUBLIC

        public async Task<bool> AnyProcedureByCategory(Guid procedureCategoryId)
        {
            var query = _context.Procedures.Where(x => x.ProcedureCategoryId == procedureCategoryId);

            return await query.AnyAsync();
        }

        public async Task<bool> AnyProcedureBySubcategory(Guid procedureSubcategoryId)
        {
            var query = _context.Procedures.Where(x => x.ProcedureSubcategoryId == procedureSubcategoryId);

            return await query.AnyAsync();
        }

        public async Task<Procedure> GetProcedure(Guid id)
        {
            var query = _context.Procedures
                .Where(x => x.Id == id)
                .SelectProcedure()
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Procedure>> GetProcedures()
        {
            var query = _context.Procedures.SelectProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Procedure>> GetProceduresByProcedureCategory(Guid procedureCategoryId)
        {
            var query = _context.Procedures
                .Where(x => x.ProcedureCategoryId == procedureCategoryId)
                .SelectProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Procedure>> GetProceduresByProcedureCategory(Guid procedureCategoryId, Guid procedureSubcategoryId)
        {
            var query = _context.Procedures
                .Where(x => x.ProcedureCategoryId == procedureCategoryId)
                .Where(x => x.ProcedureSubcategoryId == procedureSubcategoryId)
                .SelectProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Procedure>> GetProceduresByProcedureCategory(Guid? procedureCategoryId, Guid? procedureSubcategoryId, string roleId)
        {
            var query = _context.Procedures.AsNoTracking();

            if (procedureCategoryId != null)
                query = query = query.Where(x => x.ProcedureCategoryId == procedureCategoryId);

            if (procedureSubcategoryId != null)
                query = query.Where(x => x.ProcedureSubcategoryId == procedureSubcategoryId);

            if (!string.IsNullOrEmpty(roleId))
                query = query.Where(x => x.ProcedureRoles.Any(y => y.RoleId == roleId));

            query = query.SelectProcedure();

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<Procedure>> GetProceduresByUser(string userId)
        {
            var confi = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DocumentaryProcedureManagement.TUPA_READONLY).FirstOrDefaultAsync();

            if (confi == null)
            {
                confi = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.DocumentaryProcedureManagement.TUPA_READONLY,
                    Value = ConstantHelpers.Configuration.DocumentaryProcedureManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.DocumentaryProcedureManagement.TUPA_READONLY]
                };

                await _context.Configurations.AddAsync(confi);
                await _context.SaveChangesAsync();
            }

            var roles = await GetRolesByUserRoleUser(userId);
            var student = await GetStudentByUser(userId);

            var query = _context.Procedures
                .Where(x => x.ProcedureRoles.Any(y => roles.Contains(y.Role)))
                .AsNoTracking();

            if (student != null)
            {
                query = query.Where(x => x.Dependency.CareerId == student.CareerId || x.Dependency.Faculty.Careers.Any(y => y.Id == student.CareerId) || !x.DependencyId.HasValue);
            }

            if (!bool.Parse(confi.Value))
                query = query.Where(x => x.ConceptId.HasValue);

            var result = await query.SelectProcedure().ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Procedure>> GetProceduresByUser(string userId, Guid procedureCategoryId)
        {
            var roles = await GetRolesByUserRoleUser(userId);
            var student = await GetStudentByUser(userId);
            var query = _context.Procedures
                .Where(x => x.ProcedureRoles.Any(y => roles.Contains(y.Role)) /*&& (student == null || x.FacultyId == null || x.FacultyId == student.Career.FacultyId)*/)
                .Where(x => x.ProcedureCategoryId == procedureCategoryId)
                .SelectProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Procedure>> GetProceduresByUser(string userId, Guid procedureCategoryId, Guid procedureSubcategoryId)
        {
            var roles = await GetRolesByUserRoleUser(userId);
            var student = await GetStudentByUser(userId);
            var query = _context.Procedures
                .Where(x => x.ProcedureRoles.Any(y => roles.Contains(y.Role)) /*&& (student == null || x.FacultyId == null || x.FacultyId == student.Career.FacultyId)*/)
                .Where(x => x.ProcedureCategoryId == procedureCategoryId && x.ProcedureSubcategoryId == procedureSubcategoryId)
                .SelectProcedure();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Procedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.ProcedureCategory.Name);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.ProcedureSubcategory.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Duration);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Score);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProceduresDatatable(sentParameters, null, null, null, null, ExpressionHelpers.SelectProcedure(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByFaculty(DataTablesStructs.SentParameters sentParameters, Guid facultyId, string searchValue = null)
        {
            Expression<Func<Procedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.ProcedureCategory.Name);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.ProcedureSubcategory.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Duration);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Score);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProceduresDatatable(sentParameters, facultyId, null, null, null, ExpressionHelpers.SelectProcedure(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByFaculty(DataTablesStructs.SentParameters sentParameters, Guid facultyId, Guid procedureCategoryId, string searchValue = null)
        {
            Expression<Func<Procedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.ProcedureCategory.Name);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.ProcedureSubcategory.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Duration);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Score);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProceduresDatatable(sentParameters, facultyId, procedureCategoryId, null, null, ExpressionHelpers.SelectProcedure(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByFaculty(DataTablesStructs.SentParameters sentParameters, Guid facultyId, Guid procedureCategoryId, Guid procedureSubcategoryId, string searchValue = null)
        {
            Expression<Func<Procedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.ProcedureCategory.Name);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.ProcedureSubcategory.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Duration);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Score);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProceduresDatatable(sentParameters, facultyId, procedureCategoryId, procedureSubcategoryId, null, ExpressionHelpers.SelectProcedure(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, string searchValue = null)
        {
            Expression<Func<Procedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.ProcedureCategory.Name);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.ProcedureSubcategory.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Duration);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Score);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProceduresDatatable(sentParameters, null, procedureCategoryId, null, null, ExpressionHelpers.SelectProcedure(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, Guid procedureSubcategoryId, string searchValue = null)
        {
            Expression<Func<Procedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.ProcedureCategory.Name);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.ProcedureSubcategory.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Duration);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Score);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProceduresDatatable(sentParameters, null, procedureCategoryId, procedureSubcategoryId, null, ExpressionHelpers.SelectProcedure(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            Expression<Func<Procedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.ProcedureCategory.Name);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.ProcedureSubcategory.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Duration);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Score);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProceduresDatatable(sentParameters, null, null, null, userId, ExpressionHelpers.SelectProcedure(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        #endregion

        public async Task<List<Procedure>> GetProceduresBySearchValue(string searchValue)
        {
            if (!string.IsNullOrWhiteSpace(searchValue))
                searchValue = searchValue.ToLower();

            var query = _context.Procedures
                .Where(x => string.IsNullOrWhiteSpace(searchValue) ||
                            x.Code.Contains(searchValue) ||
                            x.Name.Contains(searchValue))
                .SelectProcedure()
                .OrderBy(x => x.Name)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<decimal> GetEnrollmentFeeCost(Guid procedureId)
        {
            var result = await _context.Procedures
                                .Where(x => x.Id == procedureId)
                                .SumAsync(x => x.ProcedureRequirements.Sum(p => p.Cost));

            return (decimal)result;
        }


        public async Task<object> GetProcedureJson(string term)
        {
            var qry = _context.Procedures.AsQueryable();

            if (term != null) qry = qry.Where(x => x.Name.Contains(term));

            var procedures = await qry
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).OrderBy(x => x.text).ToListAsync();

            return procedures;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters sentParameters, string code, string search)
        {
            Expression<Func<Procedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.ProcedureCategory.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.ProcedureRequirements.Sum(pr => pr.Cost);
                    break;
                default:
                    orderByPredicate = (x) => x.ProcedureCategory.Name;
                    break;
            }


            var query = _context.Procedures
                .Where(x => x.ConceptId.HasValue)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Code.ToUpper().Contains(search.ToUpper()) || x.Name.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    category = x.ProcedureCategory.Name,
                    name = x.Name,
                    totalamount = x.ProcedureRequirements.Sum(pr => pr.Cost)
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

        public async Task<Procedure> GetByStaticType(int staticType)
            => await _context.Procedures.Where(x => x.StaticType == staticType).FirstOrDefaultAsync();

        public async Task<Procedure> GetProcedureByConceptId(Guid conceptId)
        {
            var result = await _context.Procedures
                .Where(x => x.ConceptId.HasValue && x.ConceptId == conceptId)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAvailableProceduresByUserDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, string search)
        {
            Expression<Func<Procedure, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var roles = await _context.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync();

            var now = DateTime.UtcNow.ToDefaultTimeZone();

            var query = _context.Procedures
                .Where(x => 
                    x.Enabled && 
                    x.ProcedureRoles.Any(y => roles.Contains(y.RoleId)) && 
                    ((x.EnabledStartDate.HasValue && x.EnabledEndDate.HasValue && now >= x.EnabledStartDate && now <= x.EnabledEndDate) || !x.EnabledStartDate.HasValue)
                )
                .AsNoTracking();

            var onlyManualTupa = Convert.ToBoolean(await GetConfigurationValue(ConstantHelpers.Configuration.DocumentaryProcedureManagement.ONLY_MANUAL_TUPA_PROCEDURE));

            if (onlyManualTupa)
            {
                query = query.Where(x => x.Score == ConstantHelpers.PROCEDURES.SCORE.MANUAL);
            }

            if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
            {
                var student = await _context.Students.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                query = query.Where(x => x.Dependency.CareerId == student.CareerId || x.Dependency.Faculty.Careers.Any(y => y.Id == student.CareerId) || !x.DependencyId.HasValue);
                query = query.Where(x => x.ProcedureAdmissionTypes.Any(y => y.AdmissionTypeId == student.AdmissionTypeId));
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(search.ToLower().Trim()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
               .OrderByCondition(parameters.OrderDirection, orderByPredicate)
               .Skip(parameters.PagingFirstRecord)
               .Take(parameters.RecordsPerDraw)
               .Select(x => new
               {
                   x.Id,
                   x.Code,
                   x.Name,
                   amount = x.ConceptId.HasValue ? $"S/. {x.Concept.Amount.ToString("0.00")}" : "Sin costo",
               }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<ProcedureValidationResult> ValidateSystemRequirements(Guid procedureId, ClaimsPrincipal user)
        {
            var result = new ProcedureValidationResult();
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var procedure = await _context.Procedures.Where(x => x.Id == procedureId).FirstOrDefaultAsync();
            var requirements = await _context.ProcedureRequirements.Where(x => x.ProcedureId == procedure.Id && x.Type == ConstantHelpers.PROCEDURE_REQUIREMENTS.TYPE.SYSTEM_VALIDATION).ToListAsync();

            var student = await _context.Students.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            foreach (var requirement in requirements)
            {
                switch (requirement.SystemValidationType)
                {
                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.NO_DEBTS:
                        var anyPaymentDebs = await _context.Payments.AnyAsync(x => x.UserId == userId && x.Type == ConstantHelpers.PAYMENT.STATUS.PENDING);
                        if (anyPaymentDebs)
                        {
                            result.Message = "Se encontraron deudas pendientes.";
                            return result;
                        }
                        break;

                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.ENROLLED_IN_THE_ACTIVE_TERM:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();

                            if (term is null)
                            {
                                result.Message = "No se encontró un periodo activo.";
                                return result;
                            }

                            var anyEnrollment = await _context.StudentSections.AnyAsync(x => x.Section.CourseTerm.TermId == term.Id);

                            if (!anyEnrollment)
                            {
                                result.Message = "No se encontraron secciones matriculadas en el periodo actual.";
                                return result;
                            }
                        }
                        break;

                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_GRADUATE:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            if (student.Status != ConstantHelpers.Student.States.GRADUATED)
                            {
                                result.Message = "El trámite requiere que el alumno sea egresado.";
                                return result;
                            }

                        }
                        break;

                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_BACHELOR:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            if (student.Status != ConstantHelpers.Student.States.BACHELOR)
                            {
                                result.Message = "El trámite requiere que el alumno sea bachiller.";
                                return result;
                            }

                        }
                        break;

                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_QUALIFIED:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            if (student.Status != ConstantHelpers.Student.States.QUALIFIED)
                            {
                                result.Message = "El trámite requiere que el alumno sea titulado.";
                                return result;
                            }
                        }
                        break;

                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_ACTIVE_STUDENT:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            if (
                                student.Status != ConstantHelpers.Student.States.TRANSFER &&
                                student.Status != ConstantHelpers.Student.States.ENTRANT &&
                                student.Status != ConstantHelpers.Student.States.REGULAR &&
                                student.Status != ConstantHelpers.Student.States.IRREGULAR &
                                student.Status != ConstantHelpers.Student.States.REPEATER &
                                student.Status != ConstantHelpers.Student.States.REPEATER &&
                                student.Status != ConstantHelpers.Student.States.UNBEATEN
                                )
                            {
                                result.Message = "El trámite requiere que el alumno se encuentre activo.";
                                return result;
                            }
                        }
                        break;

                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_NOT_SANCTIONED:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            if (student.Status == ConstantHelpers.Student.States.SANCTIONED)
                            {
                                result.Message = "El trámite requiere que el alumno no se encuentre sancionado.";
                                return result;
                            }
                        }
                        break;

                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_UNBEATEN:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            if (student.Status == ConstantHelpers.Student.States.UNBEATEN)
                            {
                                result.Message = "El trámite requiere que el alumno se encuentre invicto.";
                                return result;
                            }
                        }
                        break;

                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_LAST_TERM_APPROVED:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            var lastAcademicSummary = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id)
                                .OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number)
                                .Select(x => new
                                {
                                    x.Term.Name,
                                    x.Term.MinGrade,
                                    x.WeightedAverageGrade
                                })
                                .FirstOrDefaultAsync();

                            if (lastAcademicSummary.MinGrade > lastAcademicSummary.WeightedAverageGrade)
                            {
                                result.Message = $"No cuentas con un promedio aprobatorio en el periodo académico {lastAcademicSummary.Name}";
                                return result;
                            }
                        }
                        break;
                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_NO_PENDING_GRADES:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            var studentSections = await _context.StudentSections.Where(x => x.StudentId == student.Id && x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                                .Select(x => new
                                {
                                    x.Id,
                                    course = x.Section.CourseTerm.Course.Name,
                                    evaluations = x.Section.CourseTerm.Evaluations.Count(),
                                    grades = x.Grades.Count()
                                })
                                .ToListAsync();

                            if (!studentSections.Any())
                            {
                                var lastSummary = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id)
                                    .OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number)
                                    .Select(x => new
                                    {
                                        x.TermHasFinished,
                                        Term = x.Term.Name
                                    })
                                    .FirstOrDefaultAsync();

                                if (lastSummary != null && !lastSummary.TermHasFinished)
                                {
                                    result.Message = $"El estudiante tiene notas pendientes en el periodo ${lastSummary.Term}";
                                    return result;
                                }
                            }
                            else
                            {
                                var pendingCourses = studentSections.Where(y => y.evaluations > y.grades).Select(x=>x.course).ToList();

                                if (pendingCourses != null && pendingCourses.Any())
                                {
                                    result.Message = $"El estudiante tiene notas pendientes en el(los) curso(s) ${string.Join("; ", pendingCourses)}";
                                    return result;
                                }
                            }
                        }
                        break;
                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_UPPER_THIRD:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            var lastAcademicSummary = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id)
                                .OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number).FirstOrDefaultAsync();

                            if(lastAcademicSummary == null || lastAcademicSummary.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH)
                            {
                                result.Message = $"El estudiante no se encuentra en el Tercio Superior.";
                                return result;
                            }
                        }
                        break;
                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_HIGHER_FIFTH:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            var lastAcademicSummary = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id)
                                .OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number).FirstOrDefaultAsync();

                            if (lastAcademicSummary == null || lastAcademicSummary.MeritType >= ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH)
                            {
                                result.Message = $"El estudiante no se encuentra en el Quinto Superior.";
                                return result;
                            }
                        }
                        break;
                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_MERIT_ORDER:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            var lastAcademicSummary = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id)
                                .OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number).FirstOrDefaultAsync();

                            if (lastAcademicSummary == null || lastAcademicSummary.MeritType == ConstantHelpers.ACADEMIC_ORDER.NONE)
                            {
                                result.Message = $"El estudiante no tiene asignado un orden mérito.";
                                return result;
                            }
                        }
                        break;
                    case ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALIDATE_NOT_SANCTIONED_HISTORY:
                        if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                        {
                            var academicSummaries = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id).ToListAsync();
                            if(academicSummaries.Any(y=>y.StudentStatus == ConstantHelpers.Student.States.SANCTIONED))
                            {
                                result.Message = $"El estudiante no tiene asignado un orden mérito.";
                                return result;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            result.Success = true;
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConsolidatedReportDatatable(DataTablesStructs.SentParameters parameters, Guid? categoryId, int? year, string roleId)
        {
            var query = _context.Procedures.AsNoTracking();

            if (categoryId.HasValue && categoryId != Guid.Empty)
                query = query.Where(x => x.ProcedureCategoryId == categoryId);

            if (!string.IsNullOrEmpty(roleId))
                query = query.Where(x => x.ProcedureRoles.Any(y => y.RoleId == roleId));

            var recordsFiltered = await query.CountAsync();

            var data = await query
               .Skip(parameters.PagingFirstRecord)
               .Take(parameters.RecordsPerDraw)
               .Select(x => new
               {
                   x.Id,
                   x.Name,
                   january = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 1),
                   febraury = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 2),
                   march = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 3),
                   april = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 4),
                   may = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 5),
                   june = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 6),
                   july = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 7),
                   august = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 8),
                   september = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 9),
                   october = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 10),
                   november = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 11),
                   december = x.UserProcedures.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == year).Count(x => x.CreatedAt.Value.Month == 12),
               }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }
    }
}
