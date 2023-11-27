using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyProjectByName(string name, Guid? id)
        {
            if (id == null)
            {
                return await _context.InvestigationProjects.Where(x => x.Name == name).AnyAsync();
            }
            else
            {
                return await _context.InvestigationProjects.Where(x => x.Name == name && x.Id != id).AnyAsync();
            }
        }

        public async Task<object> GetProject(Guid id)
        {
            try
            {
                var query = _context.InvestigationProjects.Where(x => x.Id == id).Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    researchLineId = x.ResearchLineId,
                    careerId = x.CareerId,
                    academicProgramId = x.AcademicProgramId,
                    executeLocation = x.ExecuteLocation,
                    startDateString = x.StartDate.ToString("dd/MM/yyyy"),
                    endDateString = x.EndDate.ToString("dd/MM/yyyy"),
                    objective = x.Objective,
                    advanceRubricId = x.AdvanceRubricId,
                    finalRubricId = x.FinalRubricId,
                    budget = x.Budget,
                    sabatical = x.Sabatical,
                    sabaticalFileUrl = x.SabaticalFile,
                    fileUrl = x.File,
                    evaluationFileUrl = x.EvaluationFile,
                    coordinator = new
                    {
                        x.CoordinatorId,
                        fullName = $"{x.Coordinator.UserName} - {x.Coordinator.FullName}"
                    },
                    members = x.Members.Select(y => new
                    {
                        y.MemberId,
                        fullName = $"{y.Member.UserName} - {y.Member.FullName}"
                    }).ToList(),
                    resolution = x.Resolution,
                    finalAdvance = x.ProjectAdvances.Where(y => y.IsFinal).Select(y => y.ProjectAdvanceHistorics.Select(z => z.File).FirstOrDefault()).FirstOrDefault()
                });
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProjectTemplate> GetProjectTemplate(Guid id)
        {
            var project = await _context.InvestigationProjects.Where(x => x.Id == id)
                .Include(x=>x.ResearchLine)
                .Include(x => x.AcademicProgram)
                .Include(x=>x.Career)
                .Include(x=>x.Coordinator)
                .Include(x => x.ProjectEvaluators).ThenInclude(x => x.Evaluator)
                .Include(x => x.Members).ThenInclude(x => x.Member)
                .Include(x => x.ProjectAdvances).FirstOrDefaultAsync();

            var result = new ProjectTemplate
            {
                Sabatical = project.Sabatical,
                AcademicProgram = $"{project.AcademicProgram.Code} - {project.AcademicProgram.Name}",
                ResearchLine = project.ResearchLine.Name,
                Budget = project.Budget.ToString("0.00"),
                Career = project.Career.Name,
                CoordinatorCode = project.Coordinator.UserName,
                Coordinator = $"{project.Coordinator.FullName}",
                EndDate = project.EndDate.ToString("yyyy-MM-dd"),
                Evaluators = project.ProjectEvaluators.Select(y => $"{y.Evaluator.UserName} - {y.Evaluator.FullName}").OrderBy(y => y.ToString()).ToList(),
                ExecuteLocation = project.ExecuteLocation,
                Members = project.Members.Select(y => $"{y.Member.UserName} - {y.Member.FullName}").OrderBy(y => y.ToString()).ToList(),
                Name = project.Name,
                Objective = project.Objective,
                StartDate = project.StartDate.ToString("yyyy-MM-dd"),
                AdvanceTemplates = project.Status < 4 ? new List<AdvanceTemplate>() : project.ProjectAdvances.Select(y => new AdvanceTemplate
                {
                    Name = y.Name,
                    Description = y.Description,
                    Qualification = y.Qualification.ToString("0.00")
                }).ToList()
            };

            return result;
        }

        public async Task<IEnumerable<object>> GetProjects()
        {
            var query = _context.InvestigationProjects.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                researchLineId = x.ResearchLine.Name,
                career = x.Career.Name,
                scheduleFile = x.ProjectSchedules.OrderByDescending(y => y.DateTime).Select(y => y.File).FirstOrDefault(),
                status = x.Status
            });
            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId, string searchValue = null)
        {
            Expression<Func<Project, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            bool teacher = false;

            if ((await _context.Teachers.FirstOrDefaultAsync(x => x.UserId == userId)) != null)
                teacher = true;

            var query = _context.InvestigationProjects.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue));
            }

            if (careerId.HasValue)
            {
                query = query.Where(x => x.CareerId == careerId.Value);
            }

            if (userId != null)
            {
                query = query.Where(x => x.CoordinatorId == userId);
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    type = teacher,
                    status = x.Status,
                    careerId = x.CareerId,
                    career = x.Career.Name,
                    sabatical = x.Sabatical,
                    lineId = x.ResearchLineId,
                    evaluationFile = x.EvaluationFile,
                    researchLine = x.ResearchLine.Name,
                    deletedDescription = x.DeletedDescription,
                    endDate = x.EndDate.ToString("dd/MM/yyyy"),
                    evalauatorStatus = x.ProjectEvaluators.Count() == 0 ? false : true,
                    scheduleFile = x.ProjectSchedules.OrderByDescending(y => y.DateTime).Select(y => y.File).FirstOrDefault(),
                    course = x.Section == null ? "-" : $"{x.Section.CourseTerm.Course.Code} {x.Section.CourseTerm.Course.Name}",
                    evaluators = String.Join(", ", x.ProjectEvaluators.Select(y => $"{y.Evaluator.UserName} - {y.Evaluator.FullName}").ToList().ToArray()),
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectEvaluatorsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, int type, string searchValue = null)
        {
            Expression<Func<ProjectEvaluator, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Project.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Project.Career.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.Project.Name);
                    break;
            }

            var query = _context.InvestigationProjectEvaluators.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Project.Name.ToUpper().Contains(searchValue));
            }

            if (userId != null)
            {
                query = query.Where(x => x.EvaluatorId == userId);
            }

            if (type != 0)
            {
                if (type == 1)
                {
                    query = query.Where(x => !x.Project.Sabatical && !x.Project.IsFormative);
                }
                else if (type == 2)
                {
                    query = query.Where(x => x.Project.Sabatical);
                }
                else
                {
                    query = query.Where(x => x.Project.IsFormative);
                }
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.ProjectId,
                    name = x.Project.Name,
                    researchLine = x.Project.ResearchLine.Name,
                    career = x.Project.Career.Name,
                    scheduleFile = x.Project.ProjectSchedules.OrderByDescending(y => y.DateTime).Select(y => y.File).FirstOrDefault(),
                    evaluationFile = x.Project.EvaluationFile,
                    status = x.Project.Status,
                    sabatical = x.Project.Sabatical,
                    deletedDescription = x.Project.DeletedDescription
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPublishedProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId, Guid? areaId, Guid? lineId, int type, string searchValue = null)
        {
            Expression<Func<Project, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.ResearchLine.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.InvestigationProjects.AsNoTracking();

            query = query.Where(x => x.Status == 4);

            if (type != 0)
            {
                if (type == 1)
                {
                    query = query.Where(x => !x.Sabatical && !x.IsFormative);
                }
                else if (type == 2)
                {
                    query = query.Where(x => x.Sabatical);
                }
                else
                {
                    query = query.Where(x => x.IsFormative);
                }
            }

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue));
            }

            if (careerId.HasValue)
            {
                query = query.Where(x => x.CareerId == careerId.Value);
            }

            if (lineId.HasValue)
            {
                query = query.Where(x => x.ResearchLineId == lineId.Value);
            }

            if (!String.IsNullOrEmpty(userId))
            {
                List<Guid> linesId = await _context.UserResearchLines.Where(x => x.UserId == userId).Select(x => x.ResearchLineId).ToListAsync();
                query = query.Where(x => linesId.Any(y => y == x.ResearchLineId));
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
                    line = x.ResearchLine.Name,
                    career = x.Career.Name,
                    finalAdvance = x.Status != 4 ? "" : x.ProjectAdvances.First(y => y.IsFinal).ProjectAdvanceHistorics.First().File
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsDatatable(DataTablesStructs.SentParameters sentParameters, int type, int year, Guid? facultyId, Guid? careerId, Guid? lineId, string searchValue = null)
        {
            Expression<Func<Project, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.ResearchLine.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

           var query = _context.InvestigationProjects.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue));
            }

            if (type != 0)
            {
                if (type == 1)
                {
                    query = query.Where(x => !x.Sabatical && !x.IsFormative);
                }
                else if (type == 2)
                {
                    query = query.Where(x => x.Sabatical);
                }
                else
                {
                    query = query.Where(x => x.IsFormative);
                }
            }

            if (year != 0)
            {
                query = query.Where(x => x.StartDate.Year == year);
            }

            if (careerId.HasValue)
            {
                query = query.Where(x => x.CareerId == careerId.Value);
            }

            if (lineId.HasValue)
            {
                query = query.Where(x => x.ResearchLineId == lineId.Value);
            }

            if (facultyId.HasValue)
            {
                query = query.Where(x => x.Career.FacultyId == facultyId.Value);
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.Code,
                    name = x.Name,
                    status = x.Status,
                    career = x.Career.Name,
                    sabatical = x.Sabatical,
                    researchLine = x.ResearchLine.Name,
                    id = x.Status == 5 ? "" : x.Id.ToString(),
                    deletedDescription = x.DeletedDescription,
                    scheduleFile = x.ProjectSchedules.OrderByDescending(y => y.DateTime).Select(y => y.File).FirstOrDefault()
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                DrawCounter = sentParameters.DrawCounter
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetFormativeProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? sectionId, string searchValue = null)
        {
            Expression<Func<ProjectEvaluator, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Project.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.Project.Name);
                    break;
            }

            var query = _context.InvestigationProjectEvaluators.Where(x => x.Project.IsFormative).AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Project.Name.ToUpper().Contains(searchValue));
            }

            if (sectionId.HasValue)
            {
                query = query.Where(x => x.Project.SectionId == sectionId);
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
                    id = x.Project.Status == 5 ? "" : x.Project.Id.ToString(),
                    name = x.Project.Name,
                    researchLine = x.Project.ResearchLine.Name,
                    scheduleFile = x.Project.ProjectSchedules.OrderByDescending(y => y.DateTime).Select(y => y.File).FirstOrDefault(),
                    status = x.Project.Status,
                    deletedDescription = x.Project.DeletedDescription
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsReportDatatable(DataTablesStructs.SentParameters sentParameters, int status, int type, string name, Guid? areaId, Guid? lineId, Guid? careerId)
        {
            Expression<Func<Project, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Resolution);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.ResearchLine.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.ExecuteLocation);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.InvestigationProjects.AsNoTracking();

            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(name));
            }
            if (lineId.HasValue)
            {
                query = query.Where(x => x.ResearchLineId == lineId.Value);
            }
            if (careerId.HasValue)
            {
                query = query.Where(x => x.CareerId == careerId.Value);
            }
            if (type != 0)
            {
                if (type == 1)
                {
                    query = query.Where(x => x.Sabatical);
                }
                else
                {
                    query = query.Where(x => !x.Sabatical);
                }
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
                    resolution = x.Resolution == null ? "" : x.Resolution,
                    area = x.ResearchLine.ResearchDiscipline.ResearchSubArea.ResearchArea.Name,
                    line = x.ResearchLine.Name,
                    career = x.Career.Name,
                    location = x.ExecuteLocation,
                    duration = $"{x.EndDate.Subtract(x.StartDate).Days} días",
                    startDate = x.StartDate.ToString("dd/MM/yyyy"),
                    endDate = x.EndDate.ToString("dd/MM/yyyy"),
                    budget = $"S/. {x.Budget.ToString("0.00")}",
                    status = x.Status
                }).ToListAsync();

            if (status != -1)
            {
                data = data.Where(x => x.status == status).ToList();
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

        public async Task<object> GetProjectPerStatusStatisticalBox(int status, string startDate, string endDate, List<Guid> careers, Guid? areaId, Guid? lineId, List<Guid> programs)
        {
            var query = _context.InvestigationProjects.AsQueryable();

            if (status != -1)
            {
                query = query.Where(x => x.Status == status);
            }
            if (!String.IsNullOrEmpty(startDate))
            {
                query = query.Where(x => x.StartDate.Date >= ConvertHelpers.DatepickerToDatetime(startDate));
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                query = query.Where(x => x.EndDate.Date <= ConvertHelpers.DatepickerToDatetime(endDate));
            }
            if (careers.Count > 0)
            {
                query = query.Where(x => careers.Any(y => y == x.CareerId));
            }
            if (lineId.HasValue)
            {
                query = query.Where(x => x.ResearchLineId == lineId.Value);
            }
            if (programs.Count > 0)
            {
                query = query.Where(x => programs.Any(y => y == x.AcademicProgramId));
            }

            var result = await query.Select(x => new
            {
                status =
                x.Status == 0 ? "Enviado" :
                x.Status == 1 ? "Aprobado" :
                x.Status == 2 ? "En revisión" :
                x.Status == 3 ? "Finalizado" :
                x.Status == 4 ? "Publicado" : "Desactivado"
            }).ToListAsync();
            var total = result.GroupBy(x => x.status).OrderBy(x => x.Key).ToList();
            var categories = total.Select(x => x.Key).ToList();
            var data = total.Select(x => x.Count()).ToList();
            return new { categories, data };
        }

        public async Task<object> GetProjectPerCareerStatisticalBox(int status, string startDate, string endDate, List<Guid> careers, Guid? areaId, Guid? lineId, List<Guid> programs)
        {
            var query = _context.InvestigationProjects.AsQueryable();

            if (status != -1)
            {
                query = query.Where(x => x.Status == status);
            }
            if (!String.IsNullOrEmpty(startDate))
            {
                query = query.Where(x => x.StartDate >= ConvertHelpers.DatepickerToDatetime(startDate));
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                query = query.Where(x => x.EndDate <= ConvertHelpers.DatepickerToDatetime(endDate));
            }
            if (careers.Count > 0)
            {
                query = query.Where(x => careers.Any(y => y == x.CareerId));
            }
            if (lineId.HasValue)
            {
                query = query.Where(x => x.ResearchLineId == lineId.Value);
            }
            if (programs.Count > 0)
            {
                query = query.Where(x => programs.Any(y => y == x.AcademicProgramId));
            }

            var total = query
                .Select(x => new
                {
                    x.Career.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .OrderBy(x => x.Key)
                .ToList();
            var categories = total.Select(x => x.Key).ToList();
            var data = total.Select(x => x.Count()).ToList();
            return new { categories, data };
        }

        public async Task<object> GetBudgetPerCareerStatisticalBox(int status, string startDate, string endDate, List<Guid> careers, Guid? areaId, Guid? lineId, List<Guid> programs)
        {
            var query = _context.InvestigationProjects.AsQueryable();

            if (status != -1)
            {
                query = query.Where(x => x.Status == status);
            }
            if (!String.IsNullOrEmpty(startDate))
            {
                query = query.Where(x => x.StartDate >= ConvertHelpers.DatepickerToDatetime(startDate));
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                query = query.Where(x => x.EndDate <= ConvertHelpers.DatepickerToDatetime(endDate));
            }
            if (careers.Count > 0)
            {
                query = query.Where(x => careers.Any(y => y == x.CareerId));
            }
            if (lineId.HasValue)
            {
                query = query.Where(x => x.ResearchLineId == lineId.Value);
            }
            if (programs.Count > 0)
            {
                query = query.Where(x => programs.Any(y => y == x.AcademicProgramId));
            }

            var total = query
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
        
        public async Task<object> GetOnGoingInvestigationChart(int investigationType, Guid? academicProgramId = null)
        {
            var query = _context.InvestigationProjects.AsQueryable();

            if (academicProgramId != null)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (investigationType == 0) //Ambos
            {
                query = query.Where(x => x.IsFormative && x.Sabatical);
            }
            else if (investigationType == 1) //Sabatico
            {
                query = query.Where(x => x.Sabatical);
            }
            else if (investigationType == 2) //Formativo
            {
                query = query.Where(x => x.IsFormative);
            }

            var chartData = await _context.Faculties
                .Select(
                    x => new
                    {
                        x.Name,
                        y = query.Count(y => y.Career.FacultyId == x.Id && y.Status == 2)
                    }
                )
                .ToListAsync();

            return chartData;
        }
    }
}
