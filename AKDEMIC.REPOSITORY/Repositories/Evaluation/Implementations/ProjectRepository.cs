using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyProjectByName(string name, Guid? id)
        {
            if (id == null)
            {
                return await _context.EvaluationProjects.Where(x => x.Name == name).AnyAsync();
            }
            else
            {
                return await _context.EvaluationProjects.Where(x => x.Name == name && x.Id != id).AnyAsync();
            }
        }

        public async Task<object> GetProject(Guid id)
        {
            var query = _context.EvaluationProjects.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                area = x.AreaId,
                careerId = x.CareerId,
                academicProgramId = x.AcademicProgramId,
                executeLocation = x.ExecuteLocation,
                startDateString = x.StartDate.ToString("dd/MM/yyyy"),
                endDateString = x.EndDate.ToString("dd/MM/yyyy"),
                objective = x.Objective,
                budget = x.Budget,
                sabatical = x.Sabatical,
                sabaticalFileUrl = x.SabaticalFile,
                fileUrl = x.File,
                evaluationFileUrl = x.EvaluationFile,
                publicObjective = x.PublicObjective,
                dependencyId = x.DependencyId,
                modality = x.Modality,
                districtId = x.DistrictId,
                provinceId = x.District.ProvinceId,
                departmentId = x.District.Province.DepartmentId,
                coordinator = new
                {
                    x.CoordinatorId,
                    fullName = $"{x.Coordinator.UserName} - {x.Coordinator.FullName}"
                },
                teacherMembers = x.Members.Where(x => x.IsTeacher).Select(y => new
                {
                    y.MemberId,
                    fullName = $"{y.Member.UserName} - {y.Member.FullName}"
                }).ToList(),
                studentMembers = x.Members.Where(x => !x.IsTeacher).Select(y => new
                {
                    y.MemberId,
                    fullName = $"{y.Member.UserName} - {y.Member.FullName}"
                }).ToList(),
                goals = x.ProjectSustainableDevelopmentGoals.Select(y => new
                {
                    goalId = y.SustainableDevelopmentGoalId,
                    name = y.SustainableDevelopmentGoal.Name
                }).ToList()
            });
            return await query.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<ProjectTemplate> GetProjectTemplate(Guid id)
        {
            var project = await _context.EvaluationProjects.Where(x => x.Id == id).Include(x => x.Area).Include(x => x.District).ThenInclude(x => x.Province)
                .ThenInclude(x => x.Department).Include(x => x.AcademicProgram).Include(X => X.Career)
                .Include(x => x.Coordinator).Include(x => x.ProjectEvaluators).ThenInclude(x => x.Evaluator)
                .Include(x => x.Members).ThenInclude(x => x.Member)
                .Include(x => x.Dependency)
                .FirstOrDefaultAsync();

            var result = new ProjectTemplate
            {
                Id = project.Id,
                Sabatical = project.Sabatical,
                District = project.District.Name,
                Province = project.District.Province.Name,
                Department = project.District.Province.Department.Name,
                AcademicPrograms = project.AcademicProgram == null ? "" : project.AcademicProgram.Name,
                Area = project.Area.Name,
                Budget = project.Budget.ToString("0.00"),
                Career = project.Career.Name,
                Coordinator = $"{project.Coordinator.UserName} - {project.Coordinator.FullName}",
                EndDate = project.EndDate.ToString("dd/MM/yyyy"),
                Evaluators = project.ProjectEvaluators.Select(y => $"{y.Evaluator.UserName} - {y.Evaluator.FullName}").OrderBy(y => y.ToString()).ToList(),
                ExecuteLocation = project.ExecuteLocation,
                Members = project.Members.Select(y => $"{y.Member.UserName} - {y.Member.FullName}").OrderBy(y => y.ToString()).ToList(),
                Name = project.Name,
                Objective = project.Objective,
                StartDate = project.StartDate.ToString("dd/MM/yyyy"),
                Dependency = project.Dependency.Name,
                Modality = EVALUATIONHelpers.MODALITY.VALUES[project.Modality],
                PublicObjective = project.PublicObjective,
            };

            return result;
        }


        public async Task<object> GetPublishedProject(Guid id)
        {
            var project = await _context.EvaluationProjects.Where(x => x.Id == id).Select(project => new
            {
                Sabatical = project.Sabatical,
                District = project.District.Name,
                Province = project.District.Province.Name,
                Department = project.District.Province.Department.Name,
                AcademicPrograms = project.AcademicProgram.Name,
                Area = project.Area.Name,
                Budget = project.Budget.ToString("0.00"),
                Career = project.Career.Name,
                Coordinator = $"{project.Coordinator.UserName} - {project.Coordinator.FullName}",
                EndDate = project.EndDate.ToString("dd/MM/yyyy"),
                Evaluators = project.ProjectEvaluators.Select(y => $"{y.Evaluator.UserName} - {y.Evaluator.FullName}").OrderBy(y => y.ToString()).ToList(),
                ExecuteLocation = project.ExecuteLocation,
                Members = project.Members.Select(y => $"{y.Member.UserName} - {y.Member.FullName}").OrderBy(y => y.ToString()).ToList(),
                Name = project.Name,
                Objective = project.Objective,
                StartDate = project.StartDate.ToString("dd/MM/yyyy"),
                Dependency = project.Dependency.Name,
                //Modality = EVALUATIONHelpers.MODALITY.VALUES[project.Modality],
                PublicObjective = project.PublicObjective,
            }).FirstOrDefaultAsync();

            var result = new ProjectTemplate
            {
            };

            return result;
        }

        public async Task<IEnumerable<object>> GetProjects()
        {
            var query = _context.EvaluationProjects.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                area = x.Area.Name,
                career = x.Career.Name,
                scheduleFile = x.ScheduleFile,
                status = x.Status == 5 ? 5 : x.Status == 3 ? 3 : x.Status == 1 ? 1 : x.EndDate < DateTime.UtcNow.ToDefaultTimeZone() && x.StartDate > DateTime.UtcNow.ToDefaultTimeZone() ? 1 : 2
            });
            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null, int modality = -1, string startDate = null, string endDate = null, byte? status = null, Guid? careerId = null)
        {
            Expression<Func<Project, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Area); break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Name); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            IQueryable<Project> query = _context.EvaluationProjects.AsNoTracking();

            if (modality != -1)
            {
                query = query.Where(x => x.Modality == modality);
            }

            if (userId != null)
            {
                query = query.Where(x => x.CoordinatorId == userId);
            }

            if (careerId != null)
            {
                query = query.Where(x => x.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(startDate))
                query = query.Where(x => x.StartDate >= ConvertHelpers.DatepickerToDatetime(startDate));

            if (!string.IsNullOrEmpty(endDate))
                query = query.Where(x => x.StartDate <= ConvertHelpers.DatepickerToDatetime(endDate));

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    modality = x.Modality,
                    area = x.Area.Name,
                    reports = x.ProjectReports.Select(x => x.Type).Count(),
                    career = x.Career.Name,
                    scheduleFile = x.ScheduleFile,
                    startDate = $"{x.StartDate:dd/MM/yyyy}",
                    status = x.Status == 5 ? 5 : x.Status == 3 ? 3 : x.Status == 1 ? 1 : x.EndDate < DateTime.UtcNow.ToDefaultTimeZone() && x.StartDate > DateTime.UtcNow.ToDefaultTimeZone() ? 1 : 2
                }).ToListAsync();

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(x => x.name.Contains(searchValue) || x.career.Contains(searchValue)).ToList();
            }

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsEvaluatorDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null, int modality = -1)
        {
            Expression<Func<ProjectEvaluator, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Project.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Project.Area); break;
                case "2":
                    orderByPredicate = ((x) => x.Project.Career.Name); break;
                default:
                    orderByPredicate = ((x) => x.Project.CreatedAt); break;
            }

            IQueryable<ProjectEvaluator> query = _context.EvaluationProjectEvaluators.AsNoTracking();

            if (modality != -1)
            {
                query = query.Where(x => x.Project.Modality == modality);
            }

            if (userId != null)
            {
                query = query.Where(x => x.EvaluatorId == userId);
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Project.Id,
                    name = x.Project.Name,
                    modality = x.Project.Modality,
                    area = x.Project.Area.Name,
                    career = x.Project.Career.Name,
                    scheduleFile = x.Project.ScheduleFile,
                    evaluationFile = x.Project.EvaluationFile,
                    status = x.Project.Status == 5 ? 5 : x.Project.Status == 3 ? 3 : x.Project.Status == 1 ? 1 : x.Project.EndDate < DateTime.UtcNow.ToDefaultTimeZone() && x.Project.StartDate > DateTime.UtcNow.ToDefaultTimeZone() ? 1 : 2
                }).ToListAsync();

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(x => x.name.Contains(searchValue) ||
                                x.career.Contains(searchValue)).ToList();
            }

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<ProjectReportDataTemplate>> GetProjectsReportDatatable(DataTablesStructs.SentParameters sentParameters, int status, string name, string area, int modality = -1, Guid? careerId = null)
        {
            IQueryable<Project> query = _context.EvaluationProjects.AsNoTracking();

            if (modality != -1)
            {
                query = query.Where(x => x.Modality == modality);
            }

            if (!String.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.ToLower().Contains(name));
            if (!String.IsNullOrEmpty(area))
                query = query.Where(x => x.Area.ToString().Contains(area));
            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);
            int filteredcount = await query.CountAsync();
            query = query.OrderBy(x => x.Name);

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new ProjectReportDataTemplate
                {
                    Name = x.Name,
                    Modality = EVALUATIONHelpers.MODALITY.VALUES[x.Modality],
                    Area = x.AreaId.HasValue ? x.Area.Name : "-",
                    ExecuterUnity = x.Dependency.Name,
                    PublicObjective = x.PublicObjective,
                    AcademicPrograms = _context.AcademicPrograms.Where(y => y.Id == x.AcademicProgramId).Select(y => y.Name).FirstOrDefault(),
                    Career = x.Career.Name,
                    Location = x.ExecuteLocation,
                    Duration = $"{x.EndDate.Subtract(x.StartDate).Days} días",
                    Objective = x.Objective,
                    Budget = $"S/. {x.Budget.ToString("0.00")}",
                    Status = x.Status == 5 ? 5 : x.Status == 3 ? 3 : x.Status == 1 ? 1 : x.EndDate < DateTime.UtcNow.ToDefaultTimeZone() && x.StartDate > DateTime.UtcNow.ToDefaultTimeZone() ? 1 : 2
                }).ToListAsync();

            if (status != -1)
            {
                data = data.Where(x => x.Status == status).ToList();
            }

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ProjectReportDataTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetProjectPerStatusStatisticalBox()
        {

            var result = await _context.EvaluationProjects.Select(x => new
            {
                status = x.Status == 5 ? "Publicado" : x.Status == 3 ? "Finalizado" : x.Status == 1 ? "Desaprobado" : x.EndDate < DateTime.UtcNow.ToDefaultTimeZone() && x.StartDate > DateTime.UtcNow.ToDefaultTimeZone() ? "Desaprobado" : "En progreso"
            }).ToListAsync();
            var total = result.GroupBy(x => x.status).OrderBy(x => x.Key).ToList();
            List<string> categories = total.Select(x => x.Key).ToList();
            List<int> data = total.Select(x => x.Count()).ToList();
            return new { categories, data };
        }
        public async Task<object> GetProjectPerCareerStatisticalBox()
        {
            var total = _context.EvaluationProjects
                .Select(x => new
                {
                    x.Career.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .OrderBy(x => x.Key)
                .ToList();
            List<string> categories = total.Select(x => x.Key).ToList();
            List<int> data = total.Select(x => x.Count()).ToList();
            return new { categories, data };
        }
        public async Task<object> GetBudgetPerCareerStatisticalBox()
        {
            var total = _context.EvaluationProjects
                .Select(x => new
                {
                    x.Career.Name,
                    x.Budget
                })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .OrderBy(x => x.Key)
                .ToList();
            var data = total.Select(x => new
            {
                name = x.Key,
                y = x.Sum(y => y.Budget)
            }).ToList();
            return new { data };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsWithEvaluatorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, int modality = -1, Guid? careerId = null)
        {
            Expression<Func<Project, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.ProjectEvaluators.Count() == 0); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            IQueryable<Project> query = _context.EvaluationProjects.AsNoTracking();

            if (modality != -1)
            {
                query = query.Where(x => x.Modality == modality);
            }

            if (careerId != null)
            {
                query = query.Where(x => x.CareerId == careerId);
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    status = x.ProjectEvaluators.Count() == 0 ? false : true,
                    evaluators = string.Join(", ", x.ProjectEvaluators.Select(y => $"{y.Evaluator.UserName} - {y.Evaluator.FullName}").ToList().ToArray())
                }).ToListAsync();

            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(x => x.name.Contains(searchValue) ||
                                x.evaluators.Contains(searchValue)).ToList();
            }

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<string>> GetEvaluatorsIdByEvaluationProjectId(Guid id, int modality = -1)
        {
            if (modality != -1)
            {
                return await _context.EvaluationProjects.Where(x => x.Id == id && x.Modality == modality).Select(x => x.ProjectEvaluators.Select(y => y.EvaluatorId).ToList()).FirstAsync();
            }
            return await _context.EvaluationProjects.Where(x => x.Id == id).Select(x => x.ProjectEvaluators.Select(y => y.EvaluatorId).ToList()).FirstAsync();
        }

        public async Task<bool> ExistProjectName(string name, Guid? id)
        {
            var query = _context.EvaluationProjects.AsQueryable();
            if (id.HasValue)
            {
                return await query.AnyAsync(x => x.Name.Equals(name) && x.Id != id);
            }
            else
            {
                return await query.AnyAsync(x => x.Name.Equals(name));
            }
        }

        public async Task<ProjectWithRubricsCareersDependenciesTemplate> GetProjectWithRubrics_Careers_Dependencies(Guid id, int modality = -1)
        {
            var project = new Project();

            if (modality != -1)
            {
                project = await _context.EvaluationProjects.Where(x => x.Modality == modality).Include(x => x.Coordinator).Include(x => x.District).ThenInclude(x => x.Province)
                    .Include(x => x.Area).FirstAsync(x => x.Id == id);
            }
            else
            {
                project = await _context.EvaluationProjects.Include(x => x.Coordinator).Include(x => x.District).ThenInclude(x => x.Province).Include(x => x.Area).FirstAsync(x => x.Id == id);
            }

            var viewModel = new ProjectWithRubricsCareersDependenciesTemplate
            {
                Id = project.Id,
                Name = project.Name,
                DistrictId = project.District.Id,
                ProvinceId = project.District.ProvinceId,
                DeparmentId = project.District.Province.DepartmentId,
                Modality = project.Modality,
                Area = project.Area.Name,
                DependencyId = project.DependencyId,
                PublicObjective = project.PublicObjective,
                CareerId = project.CareerId,
                AcademicProgramId = project.AcademicProgramId,
                ExecuteLocation = project.ExecuteLocation,
                StartDateString = project.StartDate.ToString("dd/MM/yyyy"),
                EndDateString = project.EndDate.ToString("dd/MM/yyyy"),
                Objective = project.Objective,
                Budget = project.Budget,
                FileUrl = project.File,
                CoordinatorId = project.Coordinator.FullName,
                TeacherMembers = _context.EvaluationProjectMembers.Where(x => x.ProjectId == id && x.IsTeacher).Select(x => x.Member.FullName).ToList(),
                StudentMembers = _context.EvaluationProjectMembers.Where(x => x.ProjectId == id && !x.IsTeacher).Select(x => x.Member.FullName).ToList(),
                Goals = _context.ProjectSustainableDevelopmentGoals.Where(x => x.ProjectId == id).Select(x => x.SustainableDevelopmentGoal.Name).ToList(),
                //Goals = _context.SustainableDevelopmentGoals
                //.OrderBy(x => x.Name)
                //.Select(x => new Tuple<Guid, string>(x.Id, x.Name)).ToList(),
                ListCareers = _context.Careers
                                                    .OrderBy(x => x.Name)
                                                    .Select(x => new Tuple<Guid, string>(x.Id, x.Name)).ToList(),
                ListDependencies = _context.Dependencies
                                                    .OrderBy(x => x.Name)
                                                    .Select(x => new Tuple<Guid, string>(x.Id, x.Name)).ToList(),
                ListMembers = _context.UserRoles
                                                    .Where(x => x.Role.Name == ConstantHelpers.ROLES.STUDENTS || x.Role.Name == ConstantHelpers.ROLES.TEACHERS)
                                                    .OrderBy(x => x.User.FullName)
                                                    .Select(x => new Tuple<string, string>(x.UserId, x.User.FullName)).ToList()
            };
            return viewModel;
        }

        public async Task<Project> GetProjectByIdAndModality(Guid id, int modality)
        {
            return await _context.EvaluationProjects.Where(x => x.Id == id && x.Modality == modality).FirstOrDefaultAsync();
        }

        public async Task<object> GetProjectsChart()
        {
            var query = _context.InvestigationProjects.AsQueryable();

            var all = await query.CountAsync();

            if (all == 0)
            {
                all = 10;
            }

            var result = new
            {
                others = all,
                indexed = 0
            };

            return result;
        }

        public async Task<List<ProjectTemplate>> GetPublishedProjects(int page)
        {

            var query = _context.EvaluationProjects.AsNoTracking().Where(x => x.Status == 5);

            var result = await query.OrderBy(x => x.StartDate).Select(project => new ProjectTemplate
            {
                Id = project.Id,
                District = project.District.Name,
                Province = project.District.Province.Name,
                Department = project.District.Province.Department.Name,
                Budget = project.Budget.ToString("0.00"),
                Career = project.Career.Name,
                EndDate = project.EndDate.ToString("dd/MM/yyyy"),
                ExecuteLocation = project.ExecuteLocation,
                Name = project.Name,
                Objective = project.Objective,
                StartDate = project.StartDate.ToString("dd/MM/yyyy")
            }).Skip(5 * page).Take(5).ToListAsync();

            return result;
        }

        public async Task<object> GetAllOfProjectsByStatusChart(int year = 0)
        {
            var query = _context.EvaluationProjects.AsQueryable();

            if (year != 0)
                query = query.Where(x => x.StartDate.Year == year);


            var result = ConstantHelpers.INVESTIGATION_PROJECT_MODALITY.VALUES
                .Select(x => new
                {
                    Name = x.Value,
                    Data = ConstantHelpers.INVESTIGATION_PROJECT_STATUS.VALUES
                    .Select(y => query.Count(z => z.Modality == x.Key && z.Status == y.Key))
                    .ToList()
                }).ToList();

            return result;
        }
    }
}
