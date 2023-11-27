using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class EmployeeSurveyRepository: Repository<EmployeeSurvey> , IEmployeeSurveyRepository
    {
        public EmployeeSurveyRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<EmployeeSurveyTemplate>> GetAllTemplateByUser(string userId , string companyId)
        {
            var userExperiences = _context.StudentExperiences
                            .Where(x => x.Student.UserId == userId)
                            .AsQueryable();

            var queryFilter = _context.EmployeeSurveys.Include(x=>x.SurveyUser).AsQueryable();

            if (!String.IsNullOrEmpty(companyId))
            {
                queryFilter = queryFilter.Where(x => x.SurveyUser.UserId == companyId);
            };

            var query = queryFilter
                        .Where(x => x.UserId == userId &&
                        x.SurveyUser.Survey.Type == ConstantHelpers.TYPE_SURVEY.BOSS_REPORT)
                        .Select(x => new EmployeeSurveyTemplate
                        {
                            Id = x.SurveyUserId,
                            Status = x.SurveyUser.DateTime == null ? "Pendiente" : "Enviado",
                            CreatedAt = x.SurveyUser.CreatedAt.Value.ToLocalDateFormat(),
                            Company = userExperiences.Where(y => y.Company.UserId == x.SurveyUser.UserId).Select(y => y.Company.User.Name).FirstOrDefault(),
                            Image = userExperiences.Where(y => y.Company.UserId == x.SurveyUser.UserId).Select(y => y.Company.User.Picture).FirstOrDefault(),
                            Sector = userExperiences.Where(y => y.Company.UserId == x.SurveyUser.UserId).Select(y => y.Company.Sector.Description).FirstOrDefault(),
                            Position = x.SurveyUser.Survey.Name
                        });

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEmployeeSurveyByCompany(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            Expression<Func<EmployeeSurvey, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }
            Expression<Func<EmployeeSurvey, dynamic>> selectPredicate = null;

            selectPredicate = (x) => new
            {
                Id = x.SurveyUserId,
                Name = x.User.FullName,
                Survey = x.SurveyUser.Survey.Name,
                SurveyDate = x.SurveyUser.Survey.PublicationDate.ToString("dd/MM/yyyy")
            };

            var query = _context.EmployeeSurveys
                    .Where(x => x.SurveyUser.UserId == userId)
                    .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.SurveyUser.Survey.Name.ToUpper().Contains(searchValue.ToUpper()));

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserEmployeeSurveyDatatable(DataTablesStructs.SentParameters sentParameters, bool graduated, Guid surveyId, string searchValue = null)
        {
            Expression<Func<EmployeeSurvey, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }


            var query = _context.EmployeeSurveys
                .Where(x => x.SurveyUser.SurveyId == surveyId && x.SurveyUser.IsGraduated == graduated)
                .AsQueryable();

                            
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    Student = x.User.FullName,
                    Date = x.SurveyUser.CreatedAt.Value.ToLocalDateFormat()
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

        public async Task<bool> StudentSurveyed(Guid surveyId, string companyUserId, string studentUserId)
        {
            var result = await _context.EmployeeSurveys.AnyAsync(x => x.SurveyUser.SurveyId == surveyId && x.SurveyUser.UserId == companyUserId && x.UserId == studentUserId);

            return result;
        }
    }
}
