using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.User;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SurveyUser;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Templates;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.SuneduReport;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        protected readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(
            AkdemicContext context,
            IActionContextAccessor actionContextAccessor,
            UserManager<ApplicationUser> userManager) : base(context)
        {
            _actionContextAccessor = actionContextAccessor;
            _userManager = userManager;
        }

        #region PRIVATE

        private async Task<IQueryable<ApplicationUser>> GetJobExchangeQueryableGeneralSurveyUsers(Guid companyId, int status, string rol, List<Guid> graduationTerms, string searchValue = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            IQueryable<ApplicationUser> query = _context.Users.AsNoTracking();

            IQueryable<Student> students = _context.Students.AsQueryable();
            IQueryable<ENTITIES.Models.JobExchange.Company> companies = _context.Companies.AsQueryable();

            string rolName = "Ninguno";

            if (!string.IsNullOrEmpty(rol))
            {
                rolName = await _context.Roles.Where(x => x.Id == rol).Select(x => x.Name).FirstOrDefaultAsync();
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
                {
                    students = students.Where(x => x.Career.CoordinatorCareers.Any(y => y.UserId == userId));
                }
            }

            if (rolName == ConstantHelpers.ROLES.STUDENTS)
            {
                if (careerId != null)
                    students = students.Where(x => x.CareerId == careerId);

                //Regular { ENTRANT , REGULAR, TRANSFER, IRREGULAR, REPEATER, UNBEATEN, SANCTIONED, OBSERVED} "-1"
                //EGRESADO { GRADUATED , BACHELOR , QUALIFIED} "-2"
                if (status == -2) //Egresados en Select
                {
                    students = students.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED ||
                                            x.Status == ConstantHelpers.Student.States.BACHELOR ||
                                            x.Status == ConstantHelpers.Student.States.QUALIFIED);

                    students = students.Where(x => x.GraduationTermId.HasValue);

                    if (graduationTerms.Count > 0 && !graduationTerms.Contains(Guid.Empty))
                    {
                        students = students.Where(x => graduationTerms.Contains(x.GraduationTermId.Value));
                    }
                }
                else if (status == -1) //Todos
                {
                    students = students.Where(x => x.Status == ConstantHelpers.Student.States.ENTRANT ||
                                     x.Status == ConstantHelpers.Student.States.REGULAR ||
                                     x.Status == ConstantHelpers.Student.States.TRANSFER ||
                                     x.Status == ConstantHelpers.Student.States.IRREGULAR ||
                                     x.Status == ConstantHelpers.Student.States.REPEATER ||
                                     x.Status == ConstantHelpers.Student.States.UNBEATEN ||
                                     x.Status == ConstantHelpers.Student.States.SANCTIONED ||
                                     x.Status == ConstantHelpers.Student.States.OBSERVED ||
                                     x.Status == ConstantHelpers.Student.States.GRADUATED ||
                                     x.Status == ConstantHelpers.Student.States.BACHELOR ||
                                     x.Status == ConstantHelpers.Student.States.QUALIFIED);
                }

                query = query.Where(x => students.Any(y => y.UserId == x.Id));
            }
            else if (rolName == ConstantHelpers.ROLES.ENTERPRISE)
            {
                if (companyId != Guid.Empty)
                {
                    List<string> studensWorkedCompany = await _context.JobOfferApplications
                        .Where(x => x.JobOffer.CompanyId == companyId & x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED)
                        .Select(x => x.Student.UserId).Distinct().ToListAsync();

                    query = query.Where(x => studensWorkedCompany.Any(y => y == x.Id));
                }
                else
                {
                    query = query.Where(x => x.UserRoles.Any(y => y.RoleId == rol));
                    query = query.Where(x => companies.Any(y => y.UserId == x.Id));
                }
            }
            else
            {
                //Rol Ninguno, alumnos y empresas
                query = query.Where(x => x.Students.Any(y => y.UserId == x.Id) || companies.Any(y => y.UserId == x.Id));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.WhereUserFullText(searchValue);
            }
            return query;
        }
        #endregion

        #region PUBLIC

        public async Task<ApplicationUser> GetAdminitrativeByUserId(string userId)
            => await _context.Users.Include(x => x.UserDependencies).ThenInclude(x => x.Dependency)
            .Where(x => !x.UserRoles.Any(r => r.Role.Name == ConstantHelpers.ROLES.STUDENTS || r.Role.Name == ConstantHelpers.ROLES.TEACHERS) && x.Id == userId).FirstOrDefaultAsync();
        public async Task<object> GetOnlyAdministrative(Select2Structs.RequestParameters requestParameters, string searchedValue)
        {
            IQueryable<ApplicationUser> users = _context.Users.AsNoTracking()
                .Where(x => x.Type == ConstantHelpers.USER_TYPES.ADMINISTRATIVE);

            if (!string.IsNullOrEmpty(searchedValue))
            {
                users = users.WhereUserFullText(searchedValue);
            }

            return await users.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = $"{x.Id}",
                Text = x.FullName
            }, pageSize: 8);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllByDni(string dni)
            => await _context.Users.Where(x => x.Document == dni || x.Dni == dni).ToArrayAsync();

        public async Task<IEnumerable<ApplicationUser>> GetDependencyUsers()
        {
            IQueryable<ApplicationUser> query = _context.UserRoles
                .Where(x => x.Role.Name == ConstantHelpers.ROLES.DEPENDENCY)
                .Select(x => new ApplicationUser
                {
                    Id = x.UserId,
                    Address = x.User.Address,
                    Dni = x.User.Dni,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    Sex = x.User.Sex
                })
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<ApplicationUser> GetByUserName(string userName)
        {
            var user = await _context.Users
              .Where(x => x.UserName == userName)
              .FirstOrDefaultAsync();
            return user;
        }

        public async Task<ApplicationUser> GetByUserNameWithoutSpecialChars(string userName)
        {
            userName = userName.Replace(".", string.Empty).Replace("-", string.Empty).Replace("_", string.Empty);

            var user = await _context.Users
              .Where(x => x.UserName.Replace(".", string.Empty).Replace("-", string.Empty).Replace("_", string.Empty) == userName)
              .FirstOrDefaultAsync();

            return user;
        }

        public async Task<ApplicationUser> GetByBankDocument(string document)
        {
            document = document.Substring(7);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Document == document && !x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.STUDENTS));
            return user;
        }

        public async Task<IEnumerable<ApplicationUser>> GetBySearchValue(string searchValue)
        {
            searchValue = searchValue.ToUpper();

            var query = _context.Users
                .Where(x => x.IsActive)
                .WhereUserFullText(searchValue)
                .Select(x => new ApplicationUser
                {
                    Id = x.Id,
                    Dni = x.Dni,
                    Name = x.Name,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    FullName = x.FullName
                })
                .OrderBy(x => x.FullName)
                .AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<Select2Structs.ResponseParameters> GetDependencyUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            //incluendo rol
            IQueryable<ApplicationUser> query = _context.Users
                 .Where(x => _context.UserRoles.Any(y => y.UserId == x.Id && y.Role.Name == ConstantHelpers.ROLES.DEPENDENCY))
                 .WhereUserFullText(searchValue)
                 .AsNoTracking();

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.FullName
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }


        public async Task<Select2Structs.ResponseParameters> GetUsersForInvestigationAndEvalutionSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            //incluyendo rol y que s
            IQueryable<ApplicationUser> query = _context.Users
                .Where(x => _context.Students.Any(y => y.UserId == x.Id && y.Status < 6) ||
                            _context.Teachers.Any(y => y.UserId == x.Id) ||
                            x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.EVALUTION_TEAM_COLLABORATOR))
                .WhereUserFullText(searchValue)
                .AsNoTracking();

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = $"{x.UserName} - {x.FullName}"
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<DataTablesStructs.ReturnedData<ApplicationUser>> GetExternalUsersToInterestGroupDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.FullName);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.UserName);

                    break;
                default:
                    orderByPredicate = ((x) => x.FullName);
                    break;
            }

            IQueryable<ApplicationUser> query = _context.Users
               .Where(x => x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.PROGRAM_PARTICIPANT))
               .WhereUserFullText(searchValue)
               .AsNoTracking();

            return await query.ToDataTables(sentParameters, (x) => new ApplicationUser
            {
                Id = x.Id,
                Name = x.Name,
                PaternalSurname = x.PaternalSurname,
                MaternalSurname = x.MaternalSurname,
                UserName = x.UserName,
                FullName = x.FullName,
            });
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersForEvaluationAndInvestigationDatatable(DataTablesStructs.SentParameters sentParameters, IEnumerable<string> roles, byte? role, byte type, Guid? careerId = null, Guid? facultyId = null, string search = null)
        {
            int recordsFiltered = 0;
            if (type == 1)
            {
                var teachers = await _context.Teachers
                    .Where(x => x.CareerId != null)
                    .Select(x => new InvestigationUsers
                    {
                        Id = x.UserId,
                        Code = x.User.UserName,
                        Name = x.User.FullName,
                        Email = x.User.Email,
                        Type = "Docente",
                        HasRole = false,
                        CareerId = x.CareerId.Value,
                        FacultyId = x.Career.FacultyId,
                        Career = x.Career.Name
                    }).ToListAsync();

                var commitees = await _context.UserRoles
                    .Where(x => x.Role.Name == ConstantHelpers.ROLES.COMMITEE)
                    //.AsEnumerable()
                    .Select(x => new InvestigationUsers
                    {
                        Id = x.UserId,
                        Code = x.User.UserName,
                        Name = string.IsNullOrEmpty(x.User.FullName) ? $"{x.User.PaternalSurname} {x.User.MaternalSurname} {x.User.Name}" : x.User.FullName,
                        Email = x.User.Email,
                        Type = "Usuario Externo",
                        HasRole = true
                    }).ToListAsync();

                var duplicates = teachers.Where(x => commitees.Any(y => y.Id == x.Id)).ToList();

                var union = commitees.Union(teachers).ToList();

                for (int i = 0; i < duplicates.Count; i++)
                {
                    union.Remove(union.First(x => x.Id == duplicates[i].Id));
                    union.Remove(union.First(x => x.Id == duplicates[i].Id));
                }

                var _union = duplicates.Select(x => new InvestigationUsers
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Email = x.Email,
                    Career = x.Career,
                    CareerId = x.CareerId,
                    FacultyId = x.FacultyId,
                    Type = "Docente",
                    HasRole = true
                });
                union = _union.Union(union).ToList();

                recordsFiltered = union.Count();

                if (role != null)
                {
                    if (role == 1)
                        union = union.Where(x => x.Type == "Docente").ToList();

                    else
                        union = union.Where(x => x.Type == "Usuario Externo").ToList();
                }

                if (careerId != null)
                {
                    union = union.Where(x => x.CareerId == careerId.Value).ToList();
                }

                if (facultyId != null)
                {
                    union = union.Where(x => x.FacultyId == facultyId.Value).ToList();
                }

                if (!string.IsNullOrEmpty(search))
                {
                    if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                    {
                        search = $"\"{search}*\"";
                        union = union.Where(x => EF.Functions.Contains(x.Name, search) || EF.Functions.Contains(x.Code, search)).ToList();
                    }
                    else
                        union = union.Where(x => x.Name.ToUpper().Contains(search.ToUpper()) || x.Code.ToUpper().Contains(search.ToUpper())).ToList();
                    //union = union.WhereSearchValue((x) => new[] { x.Dni, x.FullName, x.UserName/*, x.PhoneNumber, x.Email*/ }, search);
                }


                switch (sentParameters.OrderColumn)
                {
                    case "0":
                        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
                            union = union.OrderByDescending(x => x.Code).ToList();
                        else
                            union = union.OrderBy(x => x.Code).ToList();
                        break;
                    case "1":
                        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
                            union = union.OrderByDescending(x => x.Name).ToList();
                        else
                            union = union.OrderBy(x => x.Name).ToList();
                        break;
                    case "2":
                        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
                            union = union.OrderByDescending(x => x.Type).ToList();
                        else
                            union = union.OrderBy(x => x.Type).ToList();
                        break;
                    case "3":
                        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
                            union = union.OrderByDescending(x => x.HasRole).ToList();
                        else
                            union = union.OrderBy(x => x.HasRole).ToList();
                        break;
                    default:
                        break;
                }
                var _data = union
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .ToList();

                var data = _data.Select(x => new
                {
                    x.Code,
                    x.HasRole,
                    x.Id,
                    x.Name,
                    x.Email,
                    x.Type,
                    x.Career,
                }).ToList();

                int recordsTotal = data.Count;

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal
                };
            }
            else
            {
                var teachers = await _context.Users
                    .Where(x => x.Type == ConstantHelpers.USER_TYPES.TEACHER)
                    .Select(x => new
                    {
                        id = x.Id,
                        code = x.UserName,
                        name = x.FullName,
                        type = "Docente",
                        hasRole = false
                    }).ToListAsync();

                var commitees2 = _context.UserRoles
                    .Where(x => x.Role.Name == ConstantHelpers.ROLES.VALIDATOR)
                    .Include(x => x.Role)
                    .Include(x => x.User)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        id = x.User.Id,
                        code = x.User.UserName,
                        name = x.User.FullName,
                        type = "Evaluador",
                        hasRole = true
                    }).ToList();

                var duplicates2 = teachers.Where(x => commitees2.Any(y => y.id == x.id)).ToList();

                var union = commitees2.Union(teachers).ToList();

                for (int i = 0; i < duplicates2.Count; i++)
                {
                    union.Remove(union.First(x => x.id == duplicates2[i].id));
                    union.Remove(union.First(x => x.id == duplicates2[i].id));
                }

                var _union = duplicates2.Select(x => new
                {
                    x.id,
                    x.code,
                    x.name,
                    type = "Docente",
                    hasRole = true
                });

                union = _union.Union(union).ToList();

                recordsFiltered = union.Count();

                if (role != null)
                {
                    if (role == 1)
                        union = union.Where(x => x.type == "Docente").ToList();
                    else
                        union = union.Where(x => x.type == "Evaluador").ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                    {
                        search = $"\"{search}*\"";
                        union = union.Where(x => EF.Functions.Contains(x.name, search) || EF.Functions.Contains(x.code, search)).ToList();
                    }
                    else
                        union = union.Where(x => x.name.ToUpper().Contains(search.ToUpper()) || x.code.ToUpper().Contains(search.ToUpper())).ToList();
                }

                switch (sentParameters.OrderColumn)
                {
                    case "0":
                        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
                            union = union.OrderByDescending(x => x.code).ToList();
                        else
                            union = union.OrderBy(x => x.code).ToList();
                        break;
                    case "1":
                        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
                            union = union.OrderByDescending(x => x.name).ToList();
                        else
                            union = union.OrderBy(x => x.name).ToList();
                        break;
                    case "2":
                        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
                            union = union.OrderByDescending(x => x.type).ToList();
                        else
                            union = union.OrderBy(x => x.type).ToList();
                        break;
                    case "3":
                        if (sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
                            union = union.OrderByDescending(x => x.hasRole).ToList();
                        else
                            union = union.OrderBy(x => x.hasRole).ToList();
                        break;
                    default:
                        break;
                }

                var data = union
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .ToList();

                int recordsTotal = data.Count;

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal
                };
            }
        }

        public async Task<Select2Structs.ResponseParameters> GetUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            IQueryable<ApplicationUser> query = _context.Users
                 .WhereUserFullText(searchValue)
                 .AsNoTracking();

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.FullName
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<Select2Structs.ResponseParameters> GetDependencyUsersSelect2ByDependency(Select2Structs.RequestParameters requestParameters, Guid dependency, string searchValue = null)
        {
            var query = _context.UserDependencies.AsQueryable();

            if (dependency != Guid.Empty)
                query = query.Where(x => x.DependencyId == dependency);

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.User.Id,
                Text = x.User.FullName
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }
        public async Task<Select2Structs.ResponseParameters> GetExternalUsersToInterestGroupSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            //incluyendo rol
            IQueryable<ApplicationUser> query = _context.Users
                           .Where(x => x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.PROGRAM_PARTICIPANT))
                           .WhereUserFullText(searchValue)
               .AsNoTracking();

            return await query.ToSelect2(requestParameters,
                (x) => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.FullName
                },
                ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<int> CountWorkers()
        {
            return await _context.Users.Where(x => !x.UserRoles.Any(ur => ur.Role.Name.Equals(ConstantHelpers.ROLES.STUDENTS))).CountAsync();
        }

        public async Task RemoveFromRoles(ApplicationUser user, IEnumerable<string> roleNames)
        {
            await _userManager.RemoveFromRolesAsync(user, roleNames);
        }

        public async Task RemoveFromRole(ApplicationUser user, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task RemoveFromRoles(ApplicationUser user, IEnumerable<ApplicationRole> roles)
        {
            await _userManager.RemoveFromRolesAsync(user, roles.Select(r => r.Name));
        }

        public async Task RemoveFromRole(ApplicationUser user, ApplicationRole role)
        {
            await _userManager.RemoveFromRoleAsync(user, role.Name);
        }

        public async Task<IEnumerable<string>> GetRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<ApplicationUser> GetByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<ApplicationUser> GetByEmailFirst(string email, string id)
            => await _context.Users.Where(x => x.Email == email && x.Id != id).FirstOrDefaultAsync();

        public async Task<List<ApplicationUser>> SearchByEmail(string email)
            => await _context.Users.Where(x => x.Email.ToLower().Trim().Contains(email.ToLower().Trim())).ToListAsync();

        public async Task<bool> AnyWithSameEmail(string userId, string email)
        {
            var normalizedEmail = _userManager.NormalizeEmail(email);
            return await _context.Users.AnyAsync(x => x.Id != userId && x.NormalizedEmail.ToUpper() == normalizedEmail.ToUpper());
        }

        public async Task<ApplicationUser> GetReniecUserByDni(string dni)
        {
            try
            {
                var query = new AKDEMIC.WEBSERVICE.Services.PIDE.Methods.REST.Query(_actionContextAccessor);
                //var query = new WEBSERVICE.Services.PIDE.Methods.SOAP.Query();

                var queryResult = await query.GetDni(dni);
                //var queryResult = await query.Post(dni);

                if (queryResult.Return.DatosPersona != null && queryResult.Return.CoResultado == "0000")
                {
                    var user = new ApplicationUser
                    {
                        Address = queryResult.Return.DatosPersona.Direccion,
                        MaternalSurname = queryResult.Return.DatosPersona.ApSegundo,
                        Name = queryResult.Return.DatosPersona.Prenombres,
                        PaternalSurname = queryResult.Return.DatosPersona.ApPrimer,
                        Picture = queryResult.Return.DatosPersona.Foto,
                        DocumentType = ConstantHelpers.DOCUMENT_TYPES.DNI,
                    };

                    return user;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
        }

        public async Task AddToRole(ApplicationUser user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task AddToRoles(ApplicationUser user, IEnumerable<string> roleNames)
        {
            await _userManager.AddToRolesAsync(user, roleNames);
        }

        public override async Task Delete(ApplicationUser user)
        {
            await _userManager.DeleteAsync(user);
        }

        public async Task<ApplicationUser> GetUserByClaim(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public string GetUserIdByClaim(ClaimsPrincipal user)
        {
            return _userManager.GetUserId(user);
        }

        public async Task<bool> AnyByUserName(string userName, string ignoredId = null)
        {
            if (string.IsNullOrEmpty(ignoredId))
                return await _context.Users.AnyAsync(x => x.UserName == userName);
            return await _context.Users.AnyAsync(x => x.UserName == userName && x.Id != ignoredId);
        }

        public async Task<bool> AnyByEmail(string email, string ignoredId = null, string dni = null)
        {
            return await _context.Users.AnyAsync(x => x.Email == email && x.Id != ignoredId && x.Dni != dni);
        }

        public async Task<bool> AnyByEmailIgnoreQueryFilter(string email, string ignoredId = null, string dni = null)
        {
            return await _context.Users.IgnoreQueryFilters().AnyAsync(x => x.Email == email && x.Id != ignoredId && x.Dni != dni);
        }


        public async Task<bool> AnyByDni(string dni, string ignoredId = null)
        {
            return await _context.Users.AnyAsync(x => x.Dni == dni && x.Id != ignoredId);
        }
        public async Task<bool> AnyByDniAndUserName(string dni, string username, string ignoredId = null)
        {
            return await _context.Users.AnyAsync(x => x.Dni == dni && x.UserName == username && x.Id != ignoredId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSurveyJobExchangeUsersDatatable(DataTablesStructs.SentParameters sentParameters, Guid companyId, int status, string rol, List<Guid> graduationTerms, string searchValue = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.FullName);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            IQueryable<ApplicationUser> query = await GetJobExchangeQueryableGeneralSurveyUsers(companyId, status, rol, graduationTerms, searchValue, careerId, user);

            int recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            List<object> data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(ExpressionHelpers.JobExchangeUsersSentSurvey())
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetSurveyIntranetUsersDatatable(DataTablesStructs.SentParameters sentParameters, string rol, List<int> academicYears, bool onlyEnrolled = false, Guid? careerId = null, Guid? facultyId = null, Guid? specialtyId = null, Guid? academicDepartmentId = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.FullName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Email);
                    break;
                default:
                    orderByPredicate = ((x) => x.FullName);
                    break;
            }

            IQueryable<ApplicationUser> query = _context.Users.AsQueryable();
            IQueryable<ApplicationUserRole> userroles = _context.UserRoles.AsQueryable();
            IQueryable<Teacher> teachers = _context.Teachers.AsQueryable();
            IQueryable<Student> students = _context.Students.AsQueryable();
            IQueryable<Career> careers = _context.Careers.AsQueryable();

            if (academicDepartmentId != null)
            {
                teachers = teachers.Where(x => x.AcademicDepartmentId == academicDepartmentId);
            }

            if (facultyId != null)
            {
                students = students.Where(x => x.Career.FacultyId == facultyId);
            }
            if (careerId != null)
            {
                students = students.Where(x => x.CareerId == careerId);
            }
            if (specialtyId != null)
            {
                students = students.Where(x => x.Career.AcademicPrograms.Any(y => y.Id == specialtyId));
            }

            if (!string.IsNullOrEmpty(rol))
            {
                ApplicationRole role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == rol);
                if (role.Name == ConstantHelpers.ROLES.STUDENTS)
                {
                    if (onlyEnrolled)
                    {
                        students = students.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE && y.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN));

                        students = students.Where(x => academicYears.Contains(x.CurrentAcademicYear));
                    }
                    query = query.Where(x => students.Any(y => y.UserId == x.Id));
                }
                else if (role.Name == ConstantHelpers.ROLES.TEACHERS)
                {
                    query = query.Where(x => teachers.Any(y => y.UserId == x.Id));
                }
                else
                {
                    query = query.Where(x => x.UserRoles.Any(y => y.RoleId == role.Id));
                }

            }
            else
            {
                //Todos los roles menos estudiantes y profesores
                query = query.Where(x => !x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.STUDENTS || y.Role.Name == ConstantHelpers.ROLES.TEACHERS));
            }

            Expression<Func<ApplicationUser, object>> selectPredicate = ExpressionHelpers.IntranetUsersSentSurvey();

            query = query.OrderByCondition("ASC", orderByPredicate);
            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<int> CountSurveyIntranetUsers(string rol, List<int> academicYears, bool onlyEnrolled = false, Guid? careerId = null, Guid? facultyId = null, Guid? specialtyId = null, Guid? academicDepartmentId = null)
        {
            IQueryable<ApplicationUser> query = _context.Users.AsQueryable();
            IQueryable<ApplicationUserRole> userroles = _context.UserRoles.AsQueryable();
            IQueryable<Teacher> teachers = _context.Teachers.AsQueryable();
            IQueryable<Student> students = _context.Students.AsQueryable();
            IQueryable<Career> careers = _context.Careers.AsQueryable();

            if (academicDepartmentId != null)
            {
                teachers = teachers.Where(x => x.AcademicDepartmentId == academicDepartmentId);
            }

            if (facultyId != null)
            {
                students = students.Where(x => x.Career.FacultyId == facultyId);
            }
            if (careerId != null)
            {
                students = students.Where(x => x.CareerId == careerId);
            }
            if (specialtyId != null)
            {
                students = students.Where(x => x.Career.AcademicPrograms.Any(y => y.Id == specialtyId));
            }

            if (!string.IsNullOrEmpty(rol))
            {
                ApplicationRole role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == rol);
                if (role.Name == ConstantHelpers.ROLES.STUDENTS)
                {
                    if (onlyEnrolled)
                    {
                        students = students.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE));
                        students = students.Where(x => academicYears.Contains(x.CurrentAcademicYear));
                    }

                    query = query.Where(x => students.Any(y => y.UserId == x.Id));
                }
                else if (role.Name == ConstantHelpers.ROLES.TEACHERS)
                {
                    query = query.Where(x => teachers.Any(y => y.UserId == x.Id));
                }
                else
                {
                    query = query.Where(x => x.UserRoles.Any(y => y.RoleId == role.Id));
                }
            }
            else
            {
                //Todos los roles menos estudiantes y profesores
                query = query.Where(x => !x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.STUDENTS || y.Role.Name == ConstantHelpers.ROLES.TEACHERS));
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetSurveyIntranetUsers(string rol, List<int> academicYears, bool onlyEnrolled = false, Guid? careerId = null, Guid? facultyId = null, Guid? specialtyId = null, Guid? academicDepartmentId = null)
        {
            IQueryable<ApplicationUser> query = _context.Users.AsQueryable();
            IQueryable<ApplicationUserRole> userroles = _context.UserRoles.AsQueryable();
            IQueryable<Teacher> teachers = _context.Teachers.AsQueryable();
            IQueryable<Student> students = _context.Students.AsQueryable();
            IQueryable<Career> careers = _context.Careers.AsQueryable();

            if (academicDepartmentId != null)
            {
                teachers = teachers.Where(x => x.AcademicDepartmentId == academicDepartmentId);
            }

            if (facultyId != null)
            {
                students = students.Where(x => x.Career.FacultyId == facultyId);
            }
            if (careerId != null)
            {
                students = students.Where(x => x.CareerId == careerId);
            }
            if (specialtyId != null)
            {
                students = students.Where(x => x.Career.AcademicPrograms.Any(y => y.Id == specialtyId));
            }

            if (!string.IsNullOrEmpty(rol))
            {
                ApplicationRole role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == rol);
                if (role.Name == ConstantHelpers.ROLES.STUDENTS)
                {
                    if (onlyEnrolled)
                    {
                        students = students.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE));
                        students = students.Where(x => academicYears.Contains(x.CurrentAcademicYear));
                    }
                    query = query.Where(x => students.Any(y => y.UserId == x.Id));
                }
                else if (role.Name == ConstantHelpers.ROLES.TEACHERS)
                {
                    query = query.Where(x => teachers.Any(y => y.UserId == x.Id));
                }
                else
                {
                    query = query.Where(x => x.UserRoles.Any(y => y.RoleId == role.Id));
                }
            }
            else
            {
                //Todos los roles menos estudiantes y profesores
                query = query.Where(x => !x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.STUDENTS || y.Role.Name == ConstantHelpers.ROLES.TEACHERS));
            }

            return await query.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        }

        public async Task<ApplicationUser> GetDeletedUserById(string userId)
        {
            ApplicationUser user = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        }

        public async Task<ApplicationUser> GetUserWithDependecies(string userId)
        {
            ApplicationUser user = await _context.Users
                    .Include(x => x.UserDependencies)
                    .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync(x => x.Id == userId);

            return user;
        }

        public IQueryable<ApplicationUser> GetAllIQueryable()
        {
            return _context.Users.AsQueryable();
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetUsersByRolesSelect2ClientSide(IEnumerable<string> roles)
        {
            var users = await _context.UserRoles
                .Where(x => roles.Any(y => y == x.Role.Name))
                .Select(x => new Select2Structs.Result
                {
                    Id = x.User.Id,
                    Text = $"{x.User.UserName} - {x.User.FullName}"
                })
                .ToArrayAsync();

            users = users.OrderBy(x => x.Text).ToArray();
            return users;
        }

        public async Task<bool> AnyUserByEmail(string userId, string email)
        {
            var result = await _context.Users.AnyAsync(x => x.Email == email && x.Id != userId);
            return result;
        }

        public async Task<int> CountSurveyJobExchangeUsersDatatable(Guid companyId, int status, string rol, List<Guid> graduationTerms, string searchValue = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            IQueryable<ApplicationUser> query = await GetJobExchangeQueryableGeneralSurveyUsers(companyId, status, rol, graduationTerms, searchValue, careerId, user);
            return await query.CountAsync();
        }

        public async Task<List<SurveyUserTemplate>> GetSurveyJobExchangeUsers(Guid companyId, int status, string rol, List<Guid> graduationTerms, string searchValue = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var query = await GetJobExchangeQueryableGeneralSurveyUsers(companyId, status, rol, graduationTerms, searchValue, careerId, user);

            var result = await query
                .Select(x => new SurveyUserTemplate
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    IsGradutated = x.Students.Any(x => x.Status == ConstantHelpers.Student.States.GRADUATED || x.Status == ConstantHelpers.Student.States.BACHELOR || x.Status == ConstantHelpers.Student.States.QUALIFIED)
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersBySurveyJobExchangeGeneralDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, Guid careerId, Guid facultyId)
        {
            Expression<Func<SurveyUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.UserId);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }
            IQueryable<SurveyUser> surveyUsersQuery = _context.SurveyUsers
                .Where(x => x.SurveyId == surveyId)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            IQueryable<ApplicationUser> query = _context.Users
                        .AsQueryable();

            IQueryable<Student> students = _context.Students.AsQueryable();
            IQueryable<ENTITIES.Models.JobExchange.Company> companies = _context.Companies.AsQueryable();


            if (facultyId != Guid.Empty)
            {
                students = students.Where(x => x.Career.FacultyId == facultyId);
            }

            if (careerId != Guid.Empty)
            {
                students = students.Where(x => x.CareerId == careerId);
            }

            query = query
                .Where(x => students.Any(y => y.UserId == x.Id) || companies.Any(y => y.UserId == x.Id));

            IQueryable<SurveyUser> result = surveyUsersQuery.Where(x => query.Any(y => y.Id == x.UserId));

            Expression<Func<SurveyUser, object>> selectPredicate = null;

            selectPredicate = (x) => new
            {
                User = x.User.FullName,
                x.User.Email
            };


            return await result.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<ApplicationUser>> GetUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string role = null, IEnumerable<string> exceptionRoles = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Dni);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.FullName);

                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }



            IQueryable<ApplicationUser> query = _context.Users
                .AsNoTracking();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.WhereUserFullText(searchValue);
            }

            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(x => x.UserRoles.Any(ur => ur.Role.Name == role));
            }

            if (exceptionRoles != null && exceptionRoles.Any())
            {
                query = query.Where(x => x.UserRoles.All(ur => !exceptionRoles.Contains(ur.Role.Name)));
            }


            return await query.ToDataTables(sentParameters, (x) => new ApplicationUser
            {
                Id = x.Id,
                Address = x.Address,
                BirthDate = x.BirthDate,
                Campaigns = x.Campaigns,
                Connections = x.Connections,
                Dni = x.Dni,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                FullName = x.FullName,
                //FavoriteCompanies = x.FavoriteCompanies,
                IsActive = x.IsActive,
                MaternalSurname = x.MaternalSurname,
                Name = x.Name,
                PaternalSurname = x.PaternalSurname,
                PhoneNumber = x.PhoneNumber,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                Picture = x.Picture,
                Sex = x.Sex,
                UserName = x.UserName,
                CreatedAt = x.CreatedAt,
                DeletedAt = x.DeletedAt,
                UpdatedAt = x.UpdatedAt
            });
        }

        public async Task<string> GetLastByPrefix(string prefix)
        {
            return await _context.Users.Where(u => u.UserName.StartsWith(prefix)).Select(u => u.UserName).OrderByDescending(u => u).FirstOrDefaultAsync();
        }

        public async Task<string> GetUserWithCodeExist(string userCodePrix)
        {
            string usersWithCodeExist = await _context.Users.Where(u => u.UserName.StartsWith(userCodePrix)).Select(u => u.UserName).OrderByDescending(u => u).FirstOrDefaultAsync();

            return usersWithCodeExist;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserCashierDatatable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.FullName;
                    break;
                case "3":
                    orderByPredicate = (x) => string.Join(", ", x.CashierDependencies.Select(y => y.Dependency.Name).ToList());
                    break;
                default:
                    break;
            }


            IQueryable<ApplicationUser> query = _context.Users
                      .Where(x => x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.CASHIER))
                      .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.WhereUserFullText(search);

            int recordsFiltered = await query.CountAsync();

            var dbdata = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.UserName,
                    name = x.FullName,
                    dependencies = x.CashierDependencies.Select(y => new { y.DependencyId, y.Dependency.Name }).ToList()
                    //dependencies = string.Join(", ", x.CashierDependencies.Select(y => y.Dependency.Name).ToList()),
                    //dependenciesId = x.CashierDependencies.Select(y => y.DependencyId).ToList()
                }).ToListAsync();

            var data = dbdata
                .Select(x => new
                {
                    x.id,
                    x.code,
                    x.name,
                    dependencies = string.Join(", ", x.dependencies.Select(y => y.Name).ToList()).Length > 250
                    ? string.Join(", ", x.dependencies.Select(y => y.Name).ToList()).Substring(0, 250) + "..."
                    : string.Join(", ", x.dependencies.Select(y => y.Name).ToList()),
                    dependenciesId = x.dependencies.Select(y => y.DependencyId).ToList()
                }).ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<ApplicationUser> GetUserByStudent(Guid studentId)
        {
            ApplicationUser result = await _context.Users.Where(x => x.Students.Any(y => y.Id == studentId)).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ApplicationUser> GetDependencyUserByUserDependency(Guid? dependencyId = null)
        {
            return await _context.Users.Where(x => x.UserDependencies.Any(u => u.DependencyId == dependencyId)).FirstOrDefaultAsync();
        }

        public async Task<string> GetNameComplete(string userId)
        {
            var nameAndLastName = await _context.Users.Where(x => x.Id == userId)
                .Select(x => new
                {
                    name = $"{x.PaternalSurname} {x.MaternalSurname}, {x.Name}"
                }).FirstOrDefaultAsync();

            return nameAndLastName.name;
        }

        public async Task<object> GetUserJson(string term)
        {
            var qry = _context.Users
                .AsNoTracking();

            if (!string.IsNullOrEmpty(term))
            {
                //qry = qry.WhereUserFullText(term);
                term = term.ToUpper();
                var searchValues = term.Split(' ');
                var numberValues = searchValues.Where(x => x.Any(y => char.IsDigit(y))).ToArray();
                var textValues = searchValues.Where(x => !x.Any(y => char.IsDigit(y))).ToArray();

                if (numberValues.Length > 0) foreach (var item in numberValues) qry = qry.Where(x => x.UserName.ToUpper().StartsWith(item) || x.Document.ToUpper().StartsWith(item));
                if (textValues.Length > 0) foreach (var item in textValues) qry = qry.Where(x => x.FullName.ToUpper().Contains(item));
            }

            var data = await qry
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.UserName} - {x.FullName}"
                }).ToListAsync();

            var result = new
            {
                items = data
            };

            return result;
        }

        public async Task<List<UserScaleInformation>> GetScaleGeographicalUsersReport()
        {
            var query = _context.Users
                .AsNoTracking();

            query = query.Where(x => !x.Students.Any(y => y.UserId == x.Id));

            var data = await query
                .Select(x => new UserScaleInformation
                {
                    UserName = x.UserName,
                    Name = x.Name,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    DocumentType = ConstantHelpers.DOCUMENT_TYPES.VALUES.ContainsKey(x.DocumentType) ?
                        ConstantHelpers.DOCUMENT_TYPES.VALUES[x.DocumentType] : "",
                    Document = x.Document,
                    PhoneNumber = x.PhoneNumber,
                    CivilStatus = ConstantHelpers.CIVIL_STATUS.VALUES.ContainsKey(x.CivilStatus) ?
                        ConstantHelpers.CIVIL_STATUS.VALUES[x.CivilStatus] : "",
                    BirthDate = x.BirthDate.ToLocalDateFormat(),
                    Country = x.WorkerLaborInformation.ResidenceCountry.Name,
                    Department = x.WorkerLaborInformation.ResidenceDepartment.Name,
                    Province = x.WorkerLaborInformation.ResidenceProvince.Name,
                    District = x.WorkerLaborInformation.ResidenceDistrict.Name,
                    Address = x.Address,
                    Campus = x.WorkerLaborInformation.Campus.Name,
                    Building = x.WorkerLaborInformation.Building.Name
                })
                .ToListAsync();

            return data;

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetScaleGeographicalUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.DocumentType;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Document;
                    break;
                case "6":
                    orderByPredicate = (x) => x.PhoneNumber;
                    break;
                case "7":
                    orderByPredicate = (x) => x.CivilStatus;
                    break;
                case "8":
                    orderByPredicate = (x) => x.BirthDate;
                    break;
                case "9":
                    orderByPredicate = (x) => x.WorkerLaborInformation.ResidenceCountry.Name;
                    break;
                case "10":
                    orderByPredicate = (x) => x.WorkerLaborInformation.ResidenceDepartment.Name;
                    break;
                case "11":
                    orderByPredicate = (x) => x.WorkerLaborInformation.ResidenceProvince.Name;
                    break;
                case "12":
                    orderByPredicate = (x) => x.WorkerLaborInformation.ResidenceDistrict.Name;
                    break;
                case "13":
                    orderByPredicate = (x) => x.Address;
                    break;
                case "14":
                    orderByPredicate = (x) => x.WorkerLaborInformation.Campus.Name;
                    break;
                case "15":
                    orderByPredicate = (x) => x.WorkerLaborInformation.Building.Name;
                    break;
            }

            var query = _context.Users
                .AsNoTracking();

            query = query.Where(x => !x.Students.Any(y => y.UserId == x.Id));


            Expression<Func<ApplicationUser, dynamic>> searchFilter = (x) => new
            {
                userName = x.UserName,
                name = x.Name,
                paternalSurname = x.PaternalSurname,
                maternalSurname = x.MaternalSurname,
                fullName = x.FullName,
            };

            int recordsFiltered = await query
                                .Select(x => new
                                {
                                    userName = x.UserName,
                                    name = x.Name,
                                    paternalSurname = x.PaternalSurname,
                                    maternalSurname = x.MaternalSurname,
                                    fullName = x.FullName
                                }, searchValue, searchFilter).CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    userName = x.UserName,
                    name = x.Name,
                    paternalSurname = x.PaternalSurname,
                    maternalSurname = x.MaternalSurname,
                    documentType = ConstantHelpers.DOCUMENT_TYPES.VALUES.ContainsKey(x.DocumentType) ?
                        ConstantHelpers.DOCUMENT_TYPES.VALUES[x.DocumentType] : "",
                    document = x.Document,
                    fullName = x.FullName,
                    phoneNumber = x.PhoneNumber,
                    civilStatus = ConstantHelpers.CIVIL_STATUS.VALUES.ContainsKey(x.CivilStatus) ?
                        ConstantHelpers.CIVIL_STATUS.VALUES[x.CivilStatus] : "",
                    birthDate = x.BirthDate.ToLocalDateFormat(),
                    country = x.WorkerLaborInformation.ResidenceCountry.Name,
                    department = x.WorkerLaborInformation.ResidenceDepartment.Name,
                    province = x.WorkerLaborInformation.ResidenceProvince.Name,
                    district = x.WorkerLaborInformation.ResidenceDistrict.Name,
                    address = x.Address,
                    campus = x.WorkerLaborInformation.Campus.Name,
                    building = x.WorkerLaborInformation.Building.Name
                }, searchValue, searchFilter)
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetScaleIgnoreQueryFilterUsersDatatable(DataTablesStructs.SentParameters sentParameters, int? state = null, string searchValue = null, int? userType = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Dni;
                    break;
                case "5":
                    orderByPredicate = (x) => x.WorkerLaborInformation.WorkerLaborRegime.Name;
                    break;
                case "6":
                    orderByPredicate = (x) => x.WorkerLaborInformation.WorkerLaborCondition.Name;
                    break;
                case "7":
                    orderByPredicate = (x) => x.WorkerLaborInformation.WorkerLaborCategory.Name;
                    break;
                case "8":
                    orderByPredicate = (x) => x.WorkerLaborInformation.PhysicalFilerCode;
                    break;
                case "9":
                    orderByPredicate = (x) => x.State;
                    break;
            }

            var query = _context.Users
                .IgnoreQueryFilters()
                .AsNoTracking();

            if (state != null)
                query = query.Where(x => x.State == state);

            if (userType != null)
                query = query.Where(x => x.Type == userType);

            query = query.Where(x => !x.Students.Any(y => y.UserId == x.Id));

            Expression<Func<ApplicationUser, dynamic>> searchFilter = (x) => new
            {
                userName = x.UserName,
                name = x.Name,
                paternalSurname = x.PaternalSurname,
                maternalSurname = x.MaternalSurname,
                fullName = x.FullName,
                dni = x.Dni
            };

            int recordsFiltered = await query
                                .Select(x => new
                                {
                                    userName = x.UserName,
                                    name = x.Name,
                                    paternalSurname = x.PaternalSurname,
                                    maternalSurname = x.MaternalSurname,
                                    fullName = x.FullName,
                                    dni = x.Dni
                                }, searchValue, searchFilter).CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    userName = x.UserName,
                    name = x.Name,
                    paternalSurname = x.PaternalSurname,
                    maternalSurname = x.MaternalSurname,
                    fullName = x.FullName,
                    dni = x.Dni,
                    email = x.Email,
                    regimen = x.WorkerLaborInformation.WorkerLaborRegime.Name ?? "-",
                    condition = x.WorkerLaborInformation.WorkerLaborCondition.Name ?? "-",
                    //categoria laboral -> situacion laboral ?
                    category = x.WorkerLaborInformation.WorkerLaborCategory.Name ?? "-",
                    physicalFilerCode = x.WorkerLaborInformation.PhysicalFilerCode ?? "-",
                    status = ConstantHelpers.USER_STATES.VALUES.ContainsKey(x.State) ? ConstantHelpers.USER_STATES.VALUES[x.State] : ""
                }, searchValue, searchFilter)
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

        public async Task<Select2Structs.ResponseParameters> GetUsersByRoleNameSelect2(Select2Structs.RequestParameters requestParameters, string roleName = null, string searchValue = null)
        {
            var query = _context.Users.AsNoTracking();
            ApplicationRole role = null;

            if (roleName != null)
            {
                role = await _context.Roles.Where(x => x.Name == roleName).FirstOrDefaultAsync();
                query = query.Where(x => x.UserRoles.Any(y => y.RoleId == role.Id));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.WhereUserFullText(searchValue);
            }

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .OrderBy(x => x.FullName)
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = (role != null) ? ((role.Name == ConstantHelpers.ROLES.ENTERPRISE) ? x.Name : $"{x.UserName} - {x.FullName}") : $"{x.UserName} - {x.FullName}"
                })
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        public async Task<Select2Structs.ResponseParameters> GetUsersByDependencyIdSelect2(Select2Structs.RequestParameters requestParameters, Guid dependecyId, string searchValue)
        {
            Expression<Func<UserDependency, Select2Structs.Result>> selectPredicate = (x) => new Select2Structs.Result
            {
                Id = x.User.Id,
                Text = x.User.FullName
            };

            IQueryable<UserDependency> query = _context.UserDependencies.Include(x => x.User)
                .Where(x => x.User.FullName.Trim().ToLower().Contains(searchValue.Trim().ToLower()) ||
                x.User.Dni.Trim().ToLower().Contains(searchValue.Trim().ToLower()))
                .Where(x => x.DependencyId == dependecyId)
                .AsNoTracking();

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<ApplicationUser> GetWithData(string userId)
        {
            return await _context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).Where(x => x.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetAllEmails(List<string> userId)
        {
            IQueryable<ApplicationUser> query = _context.Users.Where(x => userId.Any(y => y == x.Id)).AsQueryable();

            return await query.Select(x => x.Email).ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcademicRecordUsers(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Email;
                    break;
                case "3":
                    orderByPredicate = (x) => x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF);
                    break;
                default:
                    orderByPredicate = (x) => x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF);
                    break;
            }

            List<string> roleIgnored = new List<string>
            {
                ConstantHelpers.ROLES.ACADEMIC_RECORD, ConstantHelpers.ROLES.TEACHERS,ConstantHelpers.ROLES.STUDENTS
            };

            IQueryable<ApplicationUser> query = _context.Users
                .Where(x => x.UserRoles.All(ur => !roleIgnored.Contains(ur.Role.Name)))
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.WhereUserFullText(search);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    name = x.FullName,
                    username = x.UserName,
                    email = x.Email,
                    isRecordAcademic = x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF)
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
        #endregion

        public async Task<object> GetUsersTreasurySelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            IQueryable<ApplicationUser> query = _context.Users
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Dni.ToUpper().Contains(searchValue.ToUpper()) || x.Document.ToUpper().Contains(searchValue.ToUpper()) || x.FullName.ToUpper().Contains(searchValue.ToUpper()));

            var result = await query
                   .OrderBy(x => x.Document)
                   .Take(5)
                   .Select(x => new
                   {
                       id = x.UserName,
                       text = $"{x.Document} - {x.FullName}"
                   })
                   .ToListAsync();

            return result;
        }

        public async Task<Select2Structs.ResponseParameters> Select2WithOutStudentRole(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            //excluyendo rol
            IQueryable<ApplicationUser> query = _context.Users.Where(x => x.Type == ConstantHelpers.USER_TYPES.TEACHER || x.Type == ConstantHelpers.USER_TYPES.ADMINISTRATIVE).AsNoTracking();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.WhereUserFullText(searchValue);
            }

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.FullName
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<object> GetNonStudentUsersSelect2(string term, List<string> filteredUsers = null)
        {
            //excluyendo rol
            var query = _context.Users
                .Where(x => !x.Students.Any())
                .AsNoTracking();

            if (!string.IsNullOrEmpty(term))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    term = $"\"{term}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.FullName, term) || EF.Functions.Contains(x.UserName, term));
                }
                else
                    query = query.Where(x => x.FullName.ToUpper().Contains(term.ToUpper()) || x.UserName.ToUpper().Contains(term.ToUpper()));
            }

            if (filteredUsers != null && filteredUsers.Any())
                query = query.Where(x => !filteredUsers.Contains(x.Id));

            var users = await query
                .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.UserName} - {x.FullName}"
                }).Take(5).ToListAsync();

            return users;
        }

        public async Task UpdateUsersPasswordJob(string connectionString)
        {
            List<ApplicationUser> users = await _context.Users
                .Where(x => !x.Email.Contains("enchufate.pe"))
                .Select(x => new ApplicationUser
                {
                    Id = x.Id,
                    Dni = x.Dni,
                    Document = x.Document
                })
                .AsNoTracking()
                .ToListAsync();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"UPDATE {ConstantHelpers.ENTITY_MODELS.DBO.ASPNETUSERS} SET PasswordHash = @PasswordHash WHERE Id = @Id";
                        sqlCommand.Transaction = sqlTransaction;

                        sqlCommand.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, -1);
                        sqlCommand.Parameters.Add("@Id", SqlDbType.NVarChar, -1);
                        sqlCommand.Prepare();

                        for (int i = 0; i < users.Count; i++)
                        {
                            ApplicationUser user = users[i];

                            if (user.Dni != null || user.Document != null)
                            {
                                string passwordHash = _userManager.PasswordHasher.HashPassword(user, user.Dni ?? user.Document);

                                sqlCommand.Parameters["@PasswordHash"].Value = passwordHash;
                                sqlCommand.Parameters["@Id"].Value = user.Id;

                                await sqlCommand.ExecuteNonQueryAsync();
                            }
                        }

                        sqlTransaction.Commit();
                    }
                }
            }
        }

        public async Task UpdateUserFullNameJob()
        {
            List<ApplicationUser> users = await _context.Users.ToListAsync();

            for (int i = 0; i < users.Count; i++)
            {
                ApplicationUser user = users[i];
                string maternalSurname = user.MaternalSurname;
                string name = user.Name;
                string paternalSurname = user.PaternalSurname;
                users[i].FullName = $"{(string.IsNullOrEmpty(paternalSurname) ? "" : $"{paternalSurname} ")}{(string.IsNullOrEmpty(maternalSurname) ? "" : $"{maternalSurname}")}{(string.IsNullOrEmpty(maternalSurname) && string.IsNullOrEmpty(paternalSurname) ? "" : ", ")}{(string.IsNullOrEmpty(name) ? "" : $"{name}")}";
                ;
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task LoadUsersFullNameJob()
        {
            int cont = 0;
            List<ApplicationUser> users = await _context.Users.ToListAsync();

            foreach (ApplicationUser user in users)
            {
                user.FullName = $"{(string.IsNullOrEmpty(user.PaternalSurname) ? "" : $"{user.PaternalSurname} ")}{(string.IsNullOrEmpty(user.MaternalSurname) ? "" : $"{user.MaternalSurname}")}, {(string.IsNullOrEmpty(user.Name) ? "" : $"{user.Name}")}";
                cont++;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<object> GetUSersSelect2CLientSideToSisco(string term)
        {
            IQueryable<ApplicationUser> qry = _context.Users
                   .Where(x => !x.UserRoles.Any(u =>
                u.Role.Name == ConstantHelpers.ROLES.PRE_UNIVERSITARY_STUDENTS
                || u.Role.Name == ConstantHelpers.ROLES.STUDENTS
                || u.Role.Name == ConstantHelpers.ROLES.LANGUAGE_STUDENTS
                || u.Role.Name == ConstantHelpers.ROLES.COMPUTER_STUDENTS
                ));

            if (!string.IsNullOrEmpty(term))
                qry = qry.WhereUserFullText(term);

            var data = await qry
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.UserName} - {x.FullName}"
                })
                .ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPreuniversitaryStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.UserName); break;
                case "3":
                    orderByPredicate = ((x) => x.Email); break;
                case "4":
                    orderByPredicate = ((x) => x.PhoneNumber); break;
                default:
                    orderByPredicate = ((x) => x.FullName); break;
            }

            var preuniversitarygroups = await _context.PreuniversitaryUserGroups
                .Include(x => x.ApplicationUser)
                .Where(pu => pu.PreuniversitaryGroup.PreuniversitaryTermId == preuniversitaryTermId)
                .Select(x => x.ApplicationUserId)
                .ToListAsync();

            var userroles = await _context.UserRoles
                .Include(x => x.User)
                .Where(r => r.Role.Name == ConstantHelpers.ROLES.PRE_UNIVERSITARY_STUDENTS)
                .Select(x => x.UserId)
                .ToListAsync();

            var usersIdList = preuniversitarygroups.Union(userroles).ToList();

            var query = _context.Users.Where(x => usersIdList.Contains(x.Id))
                //.Where(x => x.UserRoles.Any(r => r.Role.Name == ConstantHelpers.ROLES.PRE_UNIVERSITARY_STUDENTS))
                //.Where(x => x.PreuniversitaryUserGroups.Any(pu => pu.PreuniversitaryGroup.PreuniversitaryTermId == preuniversitaryTermId))
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Contains(searchValue) ||
                                            x.PaternalSurname.Contains(searchValue) ||
                                            x.MaternalSurname.Contains(searchValue) ||
                                            x.UserName.Contains(searchValue) ||
                                            x.Email.Contains(searchValue) ||
                                            x.PhoneNumber.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      name = x.FullName,
                      userName = x.UserName,
                      email = x.Email,
                      phoneNumber = x.PhoneNumber
                  })
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersAndStudents(DataTablesStructs.SentParameters sentParameters, Guid? careerId, int type, string searchValue = null)
        {
            var projects = await _context.InvestigationProjects.Select(x => x.CoordinatorId).ToListAsync();

            IQueryable<TeacherStudent> students = _context.Students.AsNoTracking().Select(x => new TeacherStudent
            {
                CareerId = x.CareerId,
                CareerName = x.Career.Name,
                FullName = x.User.FullName,
                UserId = x.UserId,
                UserName = x.User.UserName,
                Type = false
            });

            IQueryable<TeacherStudent> teachers = _context.Teachers.Where(x => x.CareerId != null).AsNoTracking().Select(x => new TeacherStudent
            {
                CareerId = x.Career.Id,
                CareerName = x.Career.Name,
                FullName = x.User.FullName,
                UserId = x.UserId,
                UserName = x.User.UserName,
                Type = true
            });

            IQueryable<TeacherStudent> query = students.Union(teachers);

            int recordsTotal = await query.CountAsync(); // (await _context.Students.CountAsync()) + (await _context.Teachers.CountAsync());

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.FullName, searchValue) || EF.Functions.Contains(x.UserName, searchValue));
                }
                else
                    query = query.Where(X => X.FullName.Trim().ToLower().Contains(searchValue.Trim().ToLower()) || X.UserName.Trim().ToLower().Contains(searchValue.Trim().ToLower()));
            }

            if (type != 0)
            {
                if (type == 1)
                {
                    query = query.Where(x => !x.Type);
                }
                else
                {
                    query = query.Where(x => x.Type);
                }
            }

            if (careerId.HasValue)
            {
                query = query.Where(x => x.CareerId == careerId.Value);
            }

            int recordsFiltered = query.Count();
            var queryclient = await query
                 .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();
            var data = queryclient
                .Select(x => new
                {
                    x.CareerId,
                    x.CareerName,
                    x.FullName,
                    x.UserId,
                    x.UserName,
                    x.Type,
                    haveProjects = projects.FirstOrDefault(y => y == x.UserId)
                }).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<ApplicationUser> GetByFullName(string fullName)
            => await _context.Users.Where(x => x.FullName.ToLower().Trim().Equals(fullName.ToLower().Trim())).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetOlderAdministrativeDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            //Administrativos Mayores de 65
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.PaternalSurname);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.MaternalSurname);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Age);
                    break;
            }
            var query = _context.Users
                .Where(x => x.Type == ConstantHelpers.USER_TYPES.ADMINISTRATIVE && x.Age > 65)
                .AsQueryable();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.UserName,
                    x.Name,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    Age = x.BirthDate.Year == 1 ? 0 : x.Age
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetScaleUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, int? userType = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Dni;
                    break;
            }
            var query = _context.Users.AsQueryable();

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
            {
                if( userType != null)
                {
                    query = query.Where(x => x.Type == userType && !x.Students.Any(y => y.UserId == x.Id));
                }
                else
                {
                    query = query.Where(x => (x.Type == ConstantHelpers.USER_TYPES.ADMINISTRATIVE || x.Type == ConstantHelpers.USER_TYPES.TEACHER) &&
                        !x.Students.Any(y => y.UserId == x.Id));
                }

            }
            else
            {
                query = query.Where(x => !x.Students.Any(y => y.UserId == x.Id));
            }


            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.UserName.ToUpper().Contains(searchValue.ToUpper())
                                        || x.FullName.ToUpper().Contains(searchValue.ToUpper())
                                        || x.Dni.ToUpper().Contains(searchValue.ToUpper())
                                        || x.PaternalSurname.ToUpper().Contains(searchValue.ToUpper())
                                        || x.MaternalSurname.ToUpper().Contains(searchValue.ToUpper())
                                        || x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    userName = x.UserName,
                    name = x.Name,
                    paternalSurname = x.PaternalSurname,
                    maternalSurname = x.MaternalSurname,
                    dni = x.Dni,
                    lastDownload = x.WorkerLaborInformation.LastPdfDownloadDate.HasValue ? x.WorkerLaborInformation.LastPdfDownloadDate.ToLocalDateFormat() : "-"
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetScaleContractUsersDatatable(DataTablesStructs.SentParameters sentParameters, Guid? conditionId = null, Guid? dedicationId = null, string searchValue = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Dni;
                    break;
            }

            var query = _context.Users.AsQueryable();

            if (dedicationId != null)
            {
                var teachers = _context.Teachers.AsQueryable();

                if (dedicationId != null)
                    teachers = teachers.Where(x => x.TeacherDedicationId == dedicationId);

                query = query.Where(x => teachers.Any(y => y.UserId == x.Id));
            }

            if (conditionId != null)
                query = query.Where(x => x.WorkerLaborInformation.WorkerLaborConditionId == conditionId);


            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.UserName.ToUpper().Contains(searchValue.ToUpper())
                                        || x.FullName.ToUpper().Contains(searchValue.ToUpper())
                                        || x.Dni.ToUpper().Contains(searchValue.ToUpper())
                                        || x.PaternalSurname.ToUpper().Contains(searchValue.ToUpper())
                                        || x.MaternalSurname.ToUpper().Contains(searchValue.ToUpper())
                                        || x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    userName = x.UserName,
                    name = x.Name,
                    paternalSurname = x.PaternalSurname,
                    maternalSurname = x.MaternalSurname,
                    dni = x.Dni
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

        public async Task<string> ShowPasswordHint(string userName, string userWeb)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.UserWeb == userWeb);

            if (user == null) return null;

            return user.PasswordHint;
        }

        public async Task<ApplicationUser> GetByUserWeb(string userWeb)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserWeb.ToUpper() == userWeb.ToUpper());
        }

        public async Task<object> GetUsersAuthoritySelect2ServerSide(string search)
        {
            var query = _context.Users
                .Where(x => x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.TEACHERS));
            //.Where(x => x.Type == ConstantHelpers.USER_TYPES.ADMINISTRATIVE || x.Type == ConstantHelpers.USER_TYPES.TEACHER).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.FullName, search) || EF.Functions.Contains(x.UserName, search));
                }
                else
                    query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper()) || x.UserName.ToUpper().Contains(search.ToUpper()));
            }
            var users = await query
                     .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.UserName} - {x.FullName}"
                })
                .Take(5)
                .ToListAsync();
            return users;
        }
        public async Task<object> GetUsersStudentsSelect2ServerSide(string search)
        {
            var query = _context.Users
                    .Where(x => x.Type == ConstantHelpers.USER_TYPES.STUDENT).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.FullName, search) || EF.Functions.Contains(x.UserName, search));
                }
                else
                    query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper()) || x.UserName.ToUpper().Contains(search.ToUpper()));
            }
            var users = await query
                     .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.UserName} - {x.FullName}"
                })
                .Take(5)
                .ToListAsync();
            return users;
        }
        public async Task<object> GetAllUsersSelect2ServerSide(string search)
        {
            var query = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.FullName, search) || EF.Functions.Contains(x.UserName, search));
                }
                else
                    query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper()) || x.UserName.ToUpper().Contains(search.ToUpper()));
            }
            var users = await query
                     .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.UserName} - {x.FullName}"
                })
                .Take(5)
                .ToListAsync();
            return users;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserWithMasterDegreeDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
        {
            var query = _context.Users.Where(x => x.WorkerMasterDegrees.Any())
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.UserName.ToLower().Contains(searchValue) || x.FullName.ToLower().Contains(searchValue));
            }

            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.WorkerMasterDegrees.Count();
                    break;
                default:
                    orderByPredicate = (x) => x.UserName;
                    break;
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.FullName,
                    quantity = x.WorkerMasterDegrees.Count()
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetUserWithDoctoralDegreeDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
        {
            var query = _context.Users.Where(x => x.WorkerDoctoralDegrees.Any())
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.UserName.ToLower().Contains(searchValue) || x.FullName.ToLower().Contains(searchValue));
            }

            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.WorkerDoctoralDegrees.Count();
                    break;
                default:
                    orderByPredicate = (x) => x.UserName;
                    break;
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.FullName,
                    quantity = x.WorkerDoctoralDegrees.Count()
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersWithTrainingDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
        {
            var query = _context.Users.Where(x => x.WorkerMasterDegrees.Any())
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.UserName.ToLower().Contains(searchValue) || x.FullName.ToLower().Contains(searchValue));
            }

            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Dni;
                    break;
                case "2":
                    orderByPredicate = (x) => x.FullName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.UserRoles.Count();
                    break;
                case "4":
                    orderByPredicate = (x) => x.WorkerLaborInformation.WorkerLaborCondition.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.UserName;
                    break;
            }

            int recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.Dni,
                    condition = x.WorkerLaborInformation.WorkerLaborCondition.Name,
                    roles = x.UserRoles.Select(x => x.Role.Name).ToList(),
                    x.FullName,
                    quantity = x.WorkerTrainings.Count()
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.Dni,
                    x.condition,
                    roles = string.Join(", ", x.roles),
                    x.FullName,
                    x.quantity
                })
                .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<string> GetPersonalizedEmailPassword(string username)
        {
            var studentObservation = await _context.StudentObservations
                .Where(x => x.Student.User.UserName == username && x.Type == ConstantHelpers.OBSERVATION_TYPES.EMAIL_INSTITUTIONAL)
                .Select(x => x.Observation).FirstOrDefaultAsync();

            return studentObservation;
        }

        public async Task<ApplicationUser> GetIgnoreQueryFilter(string id)
        {
            return await _context.Users.IgnoreQueryFilters()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetByUserNameIgnoreQueryFilter(string userName)
        {
            var user = await _context.Users.IgnoreQueryFilters()
                .Where(x => x.UserName == userName)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> AnyByUserNameIgnoreQueryFilter(string userName, string ignoredId = null)
        {
            return await _context.Users.IgnoreQueryFilters().AnyAsync(x => x.UserName == userName && x.Id != ignoredId);
        }

        public async Task<List<AdministrativeSuneduReportTemplate>> GetSuneduReportForAdministrative(Guid termId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).Select(x => new { x.Id, x.Name }).FirstOrDefaultAsync();

            var users = _context.Users
                .Where(x => !x.UserRoles.Any(r => r.Role.Name == ConstantHelpers.ROLES.STUDENTS || r.Role.Name == ConstantHelpers.ROLES.TEACHERS))
                .AsNoTracking();

            var today = DateTime.UtcNow.AddHours(-5);

            var data = await users
                .Select(x => new AdministrativeSuneduReportTemplate
                {
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    Name = x.Name,
                    Dni = x.Dni,
                    TermName = term.Name,
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.Sex) ?
                    ConstantHelpers.SEX.VALUES[x.Sex] : "No Especifica",
                    BirthDate = x.BirthDate.ToLocalDateFormat(),
                    Depedencies = string.Join(", ", x.UserDependencies.Select(y => y.Dependency.Name).ToList()),
                    LaborCondition = x.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborCondition == null ? "No Especifica" : y.WorkerLaborCondition.Name).FirstOrDefault(),
                    LaborRegime = x.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborRegime == null ? "No Especifica" : y.WorkerLaborRegime.Name).FirstOrDefault(),
                    CapPositionCode = x.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerCapPosition == null ? "No Especifica" : y.WorkerCapPosition.Code).FirstOrDefault(),
                    CapPositionName = x.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerCapPosition == null ? "No Especifica" : y.WorkerCapPosition.Name).FirstOrDefault(),
                    ElementarySchoolStudies = x.WorkerSchoolDegrees
                        .Where(y => y.StudyType == ConstantHelpers.STUDY_TYPES.ELEMENTARY_SCHOOL)
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty
                        }).FirstOrDefault(),// Primaria
                    HighSchoolStudies = x.WorkerSchoolDegrees
                        .Where(y => y.StudyType == ConstantHelpers.STUDY_TYPES.HIGH_SCHOOL)
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty
                        }).FirstOrDefault(),// Secundaria
                    TechnicalStudies = x.WorkerTechnicalStudies
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty
                        }).FirstOrDefault(),// Estudio Tecnico
                    BachelorDegrees = x.WorkerBachelorDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty
                        }).FirstOrDefault(),// Bachillerato
                    ProfessionalSchools = x.WorkerProfessionalSchools
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty
                        }).FirstOrDefault(),// Colegiatura
                    ProfessionalTitles = x.WorkerProfessionalTitles
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty
                        }).FirstOrDefault(),// Titulo Profesional
                    MasterDegrees = x.WorkerMasterDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty
                        }).FirstOrDefault(),// Maestria
                    DoctoralDegrees = x.WorkerDoctoralDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty
                        }).FirstOrDefault(),// Doctorado
                    SecondSpecialties = x.WorkerSecondSpecialties
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty
                        }).FirstOrDefault(),// Segunda Escpecialidades
                    Diplomates = x.WorkerDiplomates
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty
                        }).FirstOrDefault(),// Diplomados
                    CreatedDate = x.CreatedAt.ToLocalDateFormat(),
                    ActualContract = x.ScaleResolutions
                        .Where(y => y.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos"
                        && today > y.BeginDate && today < y.EndDate)
                        .Select(y => new ContractTemplate
                        {
                            BeginDate = y.BeginDate.ToLocalDateFormat(),
                            EndDate = y.EndDate.ToLocalDateFormat()
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            return data;
        }

        public async Task<List<TeacherSuneduReportTemplate>> GetSuneduReportForTeacher(Guid termId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).Select(x => new { x.Id, x.Name }).FirstOrDefaultAsync();

            var teachers = _context.Teachers
                            .AsNoTracking();

            var today = DateTime.UtcNow.AddHours(-5);

            var data = await teachers
                .Select(x => new TeacherSuneduReportTemplate
                {
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    Dni = x.User.Dni,
                    TermName = term.Name,
                    TeacherDedication = x.TeacherDedication == null ? "No Especifica" : x.TeacherDedication.Name,
                    Faculty = x.Career == null ? "No Especifica" : x.Career.Faculty.Name,
                    Career = x.Career == null ? "No Especifica" : x.Career.Name,
                    JoinedDateString = x.User.WorkerLaborInformation.EntryDate.HasValue ? x.User.WorkerLaborInformation.EntryDate.Value.ToLocalDateFormat() : "",
                    JoinedDate = x.User.WorkerLaborInformation.EntryDate,
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.User.Sex) ?
                    ConstantHelpers.SEX.VALUES[x.User.Sex] : "No Especifica",
                    BirthDate = x.User.BirthDate.ToLocalDateFormat(),
                    Depedencies = string.Join(", ", x.User.UserDependencies.Select(y => y.Dependency.Name).ToList()),
                    LectiveHours = x.TeacherDedication == null ? 0 : x.TeacherDedication.MaxLessonHours,
                    NoLectiveHours = x.TeacherDedication == null ? 0 : x.TeacherDedication.MinLessonHours,
                    LaborCondition = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborCondition == null ? "No Especifica" : y.WorkerLaborCondition.Name).FirstOrDefault(),
                    LaborRegime = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborRegime == null ? "No Especifica" : y.WorkerLaborRegime.Name).FirstOrDefault(),
                    LaborCategory = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborCategory == null ? "No Especifica" : y.WorkerLaborCategory.Name).FirstOrDefault(),
                    CapPositionCode = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerCapPosition == null ? "No Especifica" : y.WorkerCapPosition.Code).FirstOrDefault(),
                    CapPositionName = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerCapPosition == null ? "No Especifica" : y.WorkerCapPosition.Name).FirstOrDefault(),
                    ElementarySchoolStudies = x.User.WorkerSchoolDegrees
                        .Where(y => y.StudyType == ConstantHelpers.STUDY_TYPES.ELEMENTARY_SCHOOL)
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Primaria
                    HighSchoolStudies = x.User.WorkerSchoolDegrees
                        .Where(y => y.StudyType == ConstantHelpers.STUDY_TYPES.HIGH_SCHOOL)
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Secundaria
                    TechnicalStudies = x.User.WorkerTechnicalStudies
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Estudio Tecnico
                    BachelorDegrees = x.User.WorkerBachelorDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Bachillerato
                    ProfessionalSchools = x.User.WorkerProfessionalSchools
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Colegiatura
                    ProfessionalTitles = x.User.WorkerProfessionalTitles
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Titulo Profesional
                    MasterDegrees = x.User.WorkerMasterDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Maestria
                    DoctoralDegrees = x.User.WorkerDoctoralDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Doctorado
                    SecondSpecialties = x.User.WorkerSecondSpecialties
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Segunda Escpecialidades
                    Diplomates = x.User.WorkerDiplomates
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Diplomados
                    CreatedDate = x.User.CreatedAt.ToLocalDateFormat(),
                    ActualContract = x.User.ScaleResolutions
                        .Where(y => y.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos"
                        && today > y.BeginDate && today < y.EndDate)
                        .Select(y => new ContractTemplate
                        {
                            BeginDate = y.BeginDate.ToLocalDateFormat(),
                            EndDate = y.EndDate.ToLocalDateFormat()
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            return data;
        }

        public async Task<List<TeacherSuneduReportTemplate>> GetSuneduC9ReportForTeacher(Guid termId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).Select(x => new { x.Id, x.Name }).FirstOrDefaultAsync();

            var teachers = _context.Teachers
                            .AsNoTracking();

            var today = DateTime.UtcNow.AddHours(-5);

            var data = await teachers
                .Select(x => new TeacherSuneduReportTemplate
                {
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    Dni = x.User.Dni,
                    TermName = term.Name,
                    UserCountry = x.User.Department == null ? "No Especifica" : x.User.Department.Country == null ? "No Especifica" : x.User.Department.Country.Name,
                    Faculty = x.Career == null ? "No Especifica" : x.Career.Faculty.Name,
                    Career = x.Career == null ? "No Especifica" : x.Career.Name,
                    JoinedDate = x.User.WorkerLaborInformation.EntryDate,
                    JoinedDateString = x.User.WorkerLaborInformation.EntryDate.HasValue ? x.User.WorkerLaborInformation.EntryDate.Value.ToLocalDateFormat() : "",
                    TeachPregrado = "Sí",
                    TeachDoctor = "No",
                    TeachMaster = "No",
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.User.Sex) ?
                    ConstantHelpers.SEX.VALUES[x.User.Sex] : "No Especifica",
                    BirthDate = x.User.BirthDate.ToLocalDateFormat(),
                    Depedencies = string.Join(", ", x.User.UserDependencies.Select(y => y.Dependency.Name).ToList()),
                    //Classes
                    LectiveHours = x.TeacherDedication == null ? 0 : x.TeacherDedication.MaxLessonHours,
                    //OtrasActividades
                    NoLectiveHours = x.TeacherDedication == null ? 0 : x.TeacherDedication.MinLessonHours,
                    LaborCondition = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborCondition == null ? "No Especifica" : y.WorkerLaborCondition.Name).FirstOrDefault(),
                    LaborRegime = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborRegime == null ? "No Especifica" : y.WorkerLaborRegime.Name).FirstOrDefault(),
                    LaborCategory = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborCategory == null ? "No Especifica" : y.WorkerLaborCategory.Name).FirstOrDefault(),
                    CapPositionCode = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerCapPosition == null ? "No Especifica" : y.WorkerCapPosition.Code).FirstOrDefault(),
                    CapPositionName = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerCapPosition == null ? "No Especifica" : y.WorkerCapPosition.Name).FirstOrDefault(),
                    ElementarySchoolStudies = x.User.WorkerSchoolDegrees
                        .Where(y => y.StudyType == ConstantHelpers.STUDY_TYPES.ELEMENTARY_SCHOOL)
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Primaria
                    HighSchoolStudies = x.User.WorkerSchoolDegrees
                        .Where(y => y.StudyType == ConstantHelpers.STUDY_TYPES.HIGH_SCHOOL)
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Secundaria
                    TechnicalStudies = x.User.WorkerTechnicalStudies
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Estudio Tecnico
                    BachelorDegrees = x.User.WorkerBachelorDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Bachillerato
                    ProfessionalSchools = x.User.WorkerProfessionalSchools
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Colegiatura
                    ProfessionalTitles = x.User.WorkerProfessionalTitles
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Titulo Profesional
                    MasterDegrees = x.User.WorkerMasterDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Maestria
                    DoctoralDegrees = x.User.WorkerDoctoralDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Doctorado
                    SecondSpecialties = x.User.WorkerSecondSpecialties
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Segunda Escpecialidades
                    Diplomates = x.User.WorkerDiplomates
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Diplomados
                    CreatedDate = x.User.CreatedAt.ToLocalDateFormat(),
                    ActualContract = x.User.ScaleResolutions
                        .Where(y => y.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos"
                        && today > y.BeginDate && today < y.EndDate)
                        .Select(y => new ContractTemplate
                        {
                            BeginDate = y.BeginDate.ToLocalDateFormat(),
                            EndDate = y.EndDate.ToLocalDateFormat()
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            return data;
        }

        public async Task<List<S3TeacherSuneduReportTemplate>> GetSuneduS3ReportForTeacher(Guid termId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).Select(x => new { x.Id, x.Name }).FirstOrDefaultAsync();

            var teachers = _context.Teachers
                            .AsNoTracking();

            var today = DateTime.UtcNow.AddHours(-5);

            var data = await teachers
                .Select(x => new S3TeacherSuneduReportTemplate
                {
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    Dni = x.User.Dni,
                    Document = x.User.Document,
                    DocumentType = ConstantHelpers.DOCUMENT_TYPES.VALUES.ContainsKey(x.User.DocumentType) ?
                        ConstantHelpers.DOCUMENT_TYPES.VALUES[x.User.DocumentType] : "-",
                    TermName = term.Name,
                    UserCountry = x.User.Department == null ? "No Especifica" : x.User.Department.Country == null ? "No Especifica" : x.User.Department.Country.Name,
                    JoinedDate = x.User.WorkerLaborInformation.EntryDate,
                    JoinedDateString = x.User.WorkerLaborInformation.EntryDate.HasValue ? x.User.WorkerLaborInformation.EntryDate.Value.ToLocalDateFormat() : "",
                    TeachPregrado = "Sí",
                    TeachDoctor = "No",
                    TeachMaster = "No",
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.User.Sex) ?
                    ConstantHelpers.SEX.VALUES[x.User.Sex] : "No Especifica",
                    BirthDate = x.User.BirthDate.ToLocalDateFormat(),
                    Depedencies = string.Join(", ", x.User.UserDependencies.Select(y => y.Dependency.Name).ToList()),
                    LaborCondition = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborCondition == null ? "No Especifica" : y.WorkerLaborCondition.Name).FirstOrDefault(),
                    LaborRegime = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborRegime == null ? "No Especifica" : y.WorkerLaborRegime.Name).FirstOrDefault(),
                    LaborCategory = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerLaborCategory == null ? "No Especifica" : y.WorkerLaborCategory.Name).FirstOrDefault(),
                    CapPositionCode = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerCapPosition == null ? "No Especifica" : y.WorkerCapPosition.Code).FirstOrDefault(),
                    CapPositionName = x.User.WorkerLaborTermInformations
                        .Where(y => y.TermId == term.Id)
                        .Select(y => y.WorkerCapPosition == null ? "No Especifica" : y.WorkerCapPosition.Name).FirstOrDefault(),
                    ElementarySchoolStudies = x.User.WorkerSchoolDegrees
                        .Where(y => y.StudyType == ConstantHelpers.STUDY_TYPES.ELEMENTARY_SCHOOL)
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Primaria
                    HighSchoolStudies = x.User.WorkerSchoolDegrees
                        .Where(y => y.StudyType == ConstantHelpers.STUDY_TYPES.HIGH_SCHOOL)
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Secundaria
                    TechnicalStudies = x.User.WorkerTechnicalStudies
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Estudio Tecnico
                    BachelorDegrees = x.User.WorkerBachelorDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Bachillerato
                    ProfessionalSchools = x.User.WorkerProfessionalSchools
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Colegiatura
                    ProfessionalTitles = x.User.WorkerProfessionalTitles
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Titulo Profesional
                    MasterDegrees = x.User.WorkerMasterDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Maestria
                    DoctoralDegrees = x.User.WorkerDoctoralDegrees
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Doctorado
                    SecondSpecialties = x.User.WorkerSecondSpecialties
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Segunda Escpecialidades
                    Diplomates = x.User.WorkerDiplomates
                        .Select(y => new StudiesTemplate
                        {
                            Country = y.StudyCountry.Name,
                            Speciality = y.Specialty,
                            InsitutionName = y.Institution.Name
                        }).FirstOrDefault(),// Diplomados
                    CreatedDate = x.User.CreatedAt.ToLocalDateFormat(),
                    ActualContract = x.User.ScaleResolutions
                        .Where(y => y.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos"
                        && today > y.BeginDate && today < y.EndDate)
                        .Select(y => new ContractTemplate
                        {
                            BeginDate = y.BeginDate.ToLocalDateFormat(),
                            EndDate = y.EndDate.ToLocalDateFormat()
                        })
                        .FirstOrDefault(),
                    //Classes
                    LectiveHours = x.TeacherDedication == null ? 0 : x.TeacherDedication.MaxLessonHours,
                    //OtrasActividades
                    NoLectiveHours = x.TeacherDedication == null ? 0 : x.TeacherDedication.MinLessonHours,
                    NotLectiveAcademicHours = x.TeacherDedication == null ? 0 : x.TeacherDedication.MinLessonHours,
                    NotLectiveOtherHours = 0.0m,
                    NotLectiveResearchHours = 0.0m,

                    CampusCodes = x.TeacherSchedules.Where(y => y.ClassSchedule.Section.CourseTerm.TermId == term.Id).Select(y => y.ClassSchedule.Classroom.Building.Campus.Code).ToList(),
                    AcademicPrograms = x.TeacherSchedules.Where(y => y.ClassSchedule.Section.CourseTerm.TermId == term.Id).Select(y => y.ClassSchedule.Section.CourseTerm.Course.AcademicProgram.Name).ToList(),
                    DoesResearchTeacher = x.User.WorkerLaborInformation == null ? false : x.User.WorkerLaborInformation.HasRenacyt,
                    RenacytTeacher = x.User.WorkerLaborInformation == null ? false : x.User.WorkerLaborInformation.HasRenacyt,
                    ResearcherTeacher = x.User.WorkerLaborInformation == null ? false : x.User.WorkerLaborInformation.HasRenacyt,
                    Observations = "",
                })
                .ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetScaleBenefitDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.Users.AsNoTracking();

            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Dni;
                    break;
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query
                    .Where(x => x.UserName.ToLower().Contains(searchValue)
                        || x.FullName.ToLower().Contains(searchValue)
                        || x.Name.ToLower().Contains(searchValue)
                        || x.PaternalSurname.ToLower().Contains(searchValue)
                        || x.MaternalSurname.ToLower().Contains(searchValue));
            }

            var benefitUsers = await _context.ScaleExtraBenefitFields
                .Where(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Beneficios"
                    || x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Bonificación Familiar"
                    || x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Pensiones"
                    || x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Quinquenios"
                    || x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Reconocimiento por tiempo de servicios")
                .Select(x => x.ScaleResolution.UserId)
                .Distinct()
                .ToListAsync();

            query = query.Where(x => benefitUsers.Contains(x.Id));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Name,
                    x.Dni
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

        public async Task<List<ApplicationUser>> GetAllByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email.ToUpper() == email.ToUpper()).ToListAsync();
        }

        public async Task<object> SearchByTerm(string term, bool showStudents = false, bool showTeachers = false)
        {
            var query = _context.Users
                .AsNoTracking();

            if (!showStudents) query = query.Where(x => !x.Students.Any());
            if (!showTeachers) query = query.Where(x => !x.Teachers.Any());

            if (!string.IsNullOrEmpty(term))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    term = $"\"{term}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.FullName, term) || EF.Functions.Contains(x.UserName, term));
                }
                else
                {
                    query = query
                        .Where(x => x.FullName.ToUpper().Contains(term.ToUpper()) || x.UserName.ToUpper().Contains(term.ToUpper()));
                }
            }

            var students = await query
                .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.UserName} - {x.FullName}"
                })
                .Take(5).ToListAsync();

            return students;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllStudents()
        {
            var query = await _context.Users
                .Where(x => x.Students.Any(y => y.UserId == x.Id))
                .ToListAsync();

            return query;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string rolId = null)
        {
            var query = _context.Users.AsNoTracking();

            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Email;
                    break;
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query
                    .Where(x => x.UserName.ToLower().Contains(searchValue)
                        || x.FullName.ToLower().Contains(searchValue)
                        || x.Name.ToLower().Contains(searchValue)
                        || x.PaternalSurname.ToLower().Contains(searchValue)
                        || x.MaternalSurname.ToLower().Contains(searchValue));
            }

            //Habilitado solo para los siguientes roles
            //Alumnos
            //Empresa
            //Coordinador de seguimiento
            if (rolId != null)
            {
                query = query.Where(x => x.UserRoles.Any(y => y.RoleId == rolId));
            }
            else
            {
                var defaultroles = await _context.Roles
                                        .Where(x => x.Name == ConstantHelpers.ROLES.STUDENTS ||
                                                    x.Name == ConstantHelpers.ROLES.ENTERPRISE ||
                                                    x.Name == ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR)
                                        .Select(x => x.Id)
                                        .ToListAsync();

                query = query.Where(x => x.UserRoles.Any(y => defaultroles.Contains(y.RoleId)));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    username = x.UserName,
                    fullname = x.FullName,
                    email = x.Email,
                    roles = string.Join(", ", x.UserRoles.Select(y => y.Role.Name).ToList())
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

        public async Task<List<string>> GetJobExchangeUsersEmails(string searchValue = null, string rolId = null)
        {
            var query = _context.Users.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query
                    .Where(x => x.UserName.ToLower().Contains(searchValue)
                        || x.FullName.ToLower().Contains(searchValue)
                        || x.Name.ToLower().Contains(searchValue)
                        || x.PaternalSurname.ToLower().Contains(searchValue)
                        || x.MaternalSurname.ToLower().Contains(searchValue));
            }

            //Habilitado solo para los siguientes roles
            //Alumnos
            //Empresa
            //Coordinador de seguimiento
            if (rolId != null)
            {
                query = query.Where(x => x.UserRoles.Any(y => y.RoleId == rolId));
            }
            else
            {
                var defaultroles = await _context.Roles
                                        .Where(x => x.Name == ConstantHelpers.ROLES.STUDENTS ||
                                                    x.Name == ConstantHelpers.ROLES.ENTERPRISE ||
                                                    x.Name == ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR)
                                        .Select(x => x.Id)
                                        .ToListAsync();

                query = query.Where(x => x.UserRoles.Any(y => defaultroles.Contains(y.RoleId)));
            }


            var emails = await query
                .Select(x => x.Email)
                .ToListAsync();

            return emails;
        }

        public async Task<List<SurveyUserTemplate>> GetSurveyUsersToSend(List<string> users)
        {
            var result = await _context.Users
                .Where(x => users.Contains(x.Id))
                .Select(x => new SurveyUserTemplate
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    IsGradutated = x.Students.Any(x => x.Status == ConstantHelpers.Student.States.GRADUATED || x.Status == ConstantHelpers.Student.States.BACHELOR || x.Status == ConstantHelpers.Student.States.QUALIFIED)
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersManagementDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.UserName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.FullName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Document);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.PhoneNumber);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.UserLogins.Select(y => y.LastLogin));
                    break;
                default:
                    //orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.Users
                .Where(x => !x.Students.Any() && !x.Teachers.Any())
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper())
                || x.UserName.ToUpper().Contains(search.ToUpper())
                || x.Document.ToUpper().Contains(search.ToUpper()));


            //if (exceptionRoles != null && exceptionRoles.Any())
            //{
            //    query = query.Where(x => x.UserRoles.All(ur => !exceptionRoles.Contains(ur.Role.Name)));
            //}

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    picture = x.Picture,
                    username = x.UserName,
                    fullname = x.FullName,
                    email = x.Email,
                    document = x.Document,
                    phone = x.PhoneNumber,
                    roles = string.Join(", ", x.UserRoles.Select(y => y.Role.Name).ToList()),
                    lastLogin = x.UserLogins.Any() ? x.UserLogins.OrderByDescending(y => y.LastLogin).Select(y => y.LastLogin.ToLocalDateFormat()).FirstOrDefault()
                    : "-"
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetLockedUsersDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Document);
                    break;
                //case "3":
                //    orderByPredicate = ((x) => x.LockedOutReason);
                //    break;
                case "3":
                    orderByPredicate = ((x) => x.LockoutEnd);
                    break;
                default:
                    break;
            }

            var query = _context.Users
                .Where(x => x.LockoutEnabled && x.LockoutEnd.HasValue)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper())
                || x.UserName.ToUpper().Contains(search.ToUpper())
                || x.Document.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    username = x.UserName,
                    fullname = x.FullName,
                    document = x.Document,
                    lockedoutReason = x.LockedOutReason,
                    lockoutEnd = x.LockoutEnd
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

        public async Task<ApplicationUser> GetWithGeoLocation(string id)
        {
            var result = await _context.Users
                .Include(x => x.Department)
                .Include(x => x.Province)
                .Include(x => x.District)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersDatatableByType(DataTablesStructs.SentParameters sentParameters, byte type, string search, List<string> roles = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Document);
                    break;
                default:
                    orderByPredicate = ((x) => x.UserName);
                    break;
            }

            var query = _context.Users.Where(x => x.Type == type).AsNoTracking();

            if (roles != null && roles.Any())
                query = query.Where(x => x.UserRoles.Any(y => roles.Contains(y.RoleId)));

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.FullName.ToUpper().Contains(search.ToUpper())
                || x.UserName.ToUpper().Contains(search.ToUpper())
                || x.Document.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.Address,
                    phoneNumber = x.PhoneNumber,
                    x.Dni,
                    x.Email,
                    x.Name,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Picture,
                    x.FullName,
                    x.Document
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

        public async Task<Select2Structs.ResponseParameters> GetUsersByTypeSelect2(Select2Structs.RequestParameters requestParameters, int? userType = null, string searchValue = null)
        {
            //Por defecto en bienestar son solo esos tipos, no modificar
            var users = _context.Users
                .Where(x => x.Type == ConstantHelpers.USER_TYPES.STUDENT ||
                    x.Type == ConstantHelpers.USER_TYPES.TEACHER ||
                    x.Type == ConstantHelpers.USER_TYPES.ADMINISTRATIVE)
                .AsNoTracking();

            if (userType != null)
            {
                users = users.Where(x => x.Type == userType.Value);
            }

            return await users.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.FullName
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE, searchValue);
        }

        public async Task<List<FavoriteCompany>> GetAllFavoriteCompaniesFromUser(string userId)
        {
            var result = await _context.FavoriteCompanies
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return result;
        }
        /*implementacion de de datatable para el filtrado de datos */
        public async Task<DataTablesStructs.ReturnedData<object>> GetUserDataTable(DataTablesStructs.SentParameters parameters1, string search)
        {
            var query = _context.Users.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Dni.ToLower().Contains(search.Trim().ToLower()));
            }
            var recorFilter = await  query.CountAsync();

            var data = await query.Skip(parameters1.PagingFirstRecord)
                .Take(parameters1.RecordsPerDraw).Select(x => new {
                    x.Id,
                    x.FullName,
                    x.Dni,
                    x.Email
                    
                }).ToListAsync();
            var recordTotal = data.Count();
            return new DataTablesStructs.ReturnedData<object> {
                Data = data,
                DrawCounter=parameters1.DrawCounter,
                RecordsFiltered=recorFilter,
                RecordsTotal=recordTotal
            };
        }
    }
}
