using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using static AKDEMIC.CORE.Helpers.ConstantHelpers.DATATABLE.SERVER_SIDE;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class PerformanceEvaluationTemplateRepository : Repository<PerformanceEvaluationTemplate>, IPerformanceEvaluationTemplateRepository
    {
        public PerformanceEvaluationTemplateRepository(AkdemicContext context) : base(context) { }
        public async Task<object> GetPerformanceEvaluationTemplate(Guid id)
        {
            var query = _context.PerformanceEvaluationTemplates.Where(x => x.Id == id).Select(x => new
            {
                id = x.Id,
                max = x.Max,
                name = x.Name,
                roleId = x.RoleId,
                isActive = x.IsActive,
                questions = x.Questions.Count(),
                x.Target,
                x.Instructions,
                x.EnabledCriterions
            }).FirstAsync();

            return await query;
        }

        public async Task<PerformanceEvaluationTemplate> GetPerformanceEvaluationWithQuestions(Guid templateId)
            => await _context.PerformanceEvaluationTemplates.Where(x => x.Id == templateId).Include(x => x.Questions).FirstOrDefaultAsync();

        public async Task<bool> AnyPerformanceEvaluationTemplateByName(string name, Guid? id)
        {
            IQueryable<PerformanceEvaluationTemplate> query = _context.PerformanceEvaluationTemplates.AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name == name);
            }
            if (id.HasValue)
            {
                query = query.Where(x => x.Id != id);
            }

            PerformanceEvaluationTemplate result = await query.FirstOrDefaultAsync();
            return result != null;
        }

        public async Task<ReturnedData<object>> GetPerformanceEvaluationTemplatesDatatable(SentParameters sentParameters, string searchValue = null, Guid? performanceEvaluationId = null)
        {
            Expression<Func<PerformanceEvaluationTemplate, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Role.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Max); break;
                case "3":
                    orderByPredicate = ((x) => x.IsActive); break;
                case "4":
                    orderByPredicate = ((x) => x.Questions.Count); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            IQueryable<PerformanceEvaluationTemplate> query = _context.PerformanceEvaluationTemplates.AsNoTracking();

            if (performanceEvaluationId.HasValue && performanceEvaluationId != Guid.Empty)
            {
                query = query.Where(x => x.RelatedPerformanceEvaluationTemplates.Any(y => y.PerformanceEvaluationId == performanceEvaluationId));
            }


            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Role.Name.ToUpper().Contains(searchValue.ToUpper()));
            }


            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    role = x.Role.Name == "Director de Carrera" ? "Director de Escuela" : x.Role.Name,
                    users = x.Users.Count,
                    hasRaitingScale = x.RelatedPerformanceEvaluationTemplates.Any(y=>y.PerformanceEvaluation.PerformanceEvaluationRubrics.Any()),
                    isActive = x.IsActive,
                    max = $"De 1 a {x.Max}",
                    questions = x.Questions.Count
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

        public async Task<PerformanceEvaluationTemplate> GetActiveTemplateByRole(string roleName)
        {
            var today = DateTime.UtcNow.ToDefaultTimeZone().Date;

            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE && x.StartDate.Date <= today && x.EndDate.Date >= today).FirstOrDefaultAsync();

            var performanceEvaluationTemplateId = await _context.RelatedPerformanceEvaluationTemplates
                .Where(x => x.PerformanceEvaluationId == evaluation.Id && x.PerformanceEvaluationTemplate.Role.Name == roleName)
                .Select(x => x.PerformanceEvaluationTemplateId).FirstOrDefaultAsync();

            IQueryable<PerformanceEvaluationTemplate> query = _context.PerformanceEvaluationTemplates.Include(x => x.Questions).AsNoTracking();

            query = query.Where(x => x.Id == performanceEvaluationTemplateId);

            PerformanceEvaluationTemplate result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task Active(Guid id)
        {
            PerformanceEvaluationTemplate template = await _context.PerformanceEvaluationTemplates.FirstAsync(x => x.Id == id);

            List<PerformanceEvaluationTemplate> nonActives = await _context.PerformanceEvaluationTemplates.Where(x => x.IsActive && x.RoleId == template.RoleId).ToListAsync();

            foreach (PerformanceEvaluationTemplate item in nonActives)
            {
                item.IsActive = false;
            }
            template.IsActive = true;

            await _context.SaveChangesAsync();
        }

        public async Task<string> ValidateActiveTemplates(byte target)
        {

            List<string> list = new List<string>();

            if (target == ConstantHelpers.PERFORMANCE_EVALUATION.TARGET.STUDENTS)
            {
                var template = await _context.PerformanceEvaluationTemplates.Where(x => x.Role.Name == ConstantHelpers.ROLES.STUDENTS && x.IsActive && x.Questions.Any()).FirstOrDefaultAsync();
                if (template is null)
                {
                    list.Add(ConstantHelpers.ROLES.STUDENTS);
                }
            }
            else
            {
                List<string> roles = await _context.Roles.Where(x =>
                    x.Name == ConstantHelpers.ROLES.DEAN ||
                    x.Name == ConstantHelpers.ROLES.STUDENTS ||
                    x.Name == ConstantHelpers.ROLES.CAREER_DIRECTOR ||
                    x.Name == ConstantHelpers.ROLES.RESEARCH_COORDINATOR ||
                    x.Name == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR ||
                    x.Name == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR ||
                    x.Name == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Select(x => x.Name).ToListAsync();


                foreach (var role in roles)
                {
                    var template = await _context.PerformanceEvaluationTemplates.Where(x => x.Role.Name == role && x.IsActive && x.Questions.Any()).FirstOrDefaultAsync();
                    if (template is null)
                    {
                        list.Add(role);
                    }
                }
            }

            return list.Count == 0 ? null : string.Join(", ", list);

        }

        public async Task<List<PerformanceEvaluationTemplate>> GetPerformanceEvaluationTemplateActive(byte target)
        {
            if (target == ConstantHelpers.PERFORMANCE_EVALUATION.TARGET.STUDENTS)
            {
                var result = await _context.PerformanceEvaluationTemplates.Where(x => x.IsActive && x.Role.Name == ConstantHelpers.ROLES.STUDENTS).ToListAsync();
                return result;
            }
            else
            {
                var result = await _context.PerformanceEvaluationTemplates.Where(x => x.IsActive).ToListAsync();
                return result;
            }
        }

        public async Task<Select2Structs.ResponseParameters> GetTemplatesSelet2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            var query = _context.PerformanceEvaluationTemplates.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(searchValue.Trim().ToLower()));

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<PerformanceEvaluationTemplate> ImportEvaluationPerformanceTemplate(Guid oldTemplateId, string newName)
        {
            var oldTemplate = await _context.PerformanceEvaluationTemplates.Where(x => x.Id == oldTemplateId).FirstOrDefaultAsync();
            var questions = await _context.PerformanceEvaluationQuestions.Where(x => x.PerformanceEvaluationTemplateId == oldTemplateId).ToListAsync();
            var criterions = await _context.PerformanceEvaluationCriterions.Where(x => x.PerformanceEvaluationTemplateId == oldTemplateId).ToListAsync();

            var newTemplate = new PerformanceEvaluationTemplate
            {
                IsActive = false,
                Max = oldTemplate.Max,
                Name = newName,
                Target = oldTemplate.Target,
                EnabledCriterions = oldTemplate.EnabledCriterions,
                Instructions = oldTemplate.Instructions,
                RoleId = oldTemplate.RoleId,
                Questions = questions.Select(x => new PerformanceEvaluationQuestion
                {
                    Description = x.Description
                })
                .ToList(),
                Criterions = criterions.Select(x => new PerformanceEvaluationCriterion
                {
                    Name = x.Name
                })
                .ToList()
            };

            return newTemplate;
        }

        public async Task<List<PerformanceEvaluationTemplate>> GetPerformanceEvaluationTemplates(Guid evaluationId)
            => await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluationId).Select(x => x.PerformanceEvaluationTemplate).ToListAsync();
    }
}
