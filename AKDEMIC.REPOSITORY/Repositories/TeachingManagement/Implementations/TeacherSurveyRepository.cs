using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class TeacherSurveyRepository:Repository<TeacherSurvey> , ITeacherSurveyRepository
    {
        public TeacherSurveyRepository(AkdemicContext context) : base(context){ }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSatisfactionPendingUsersBySurveyDatatable(DataTablesStructs.SentParameters sentParameters,Guid surveyId, string searchValue = null)
        {
            Expression<Func<TeacherSurvey, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.SurveyUserId);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.TeacherSurveys
                        .Where(x => x.SurveyUser.SurveyId == surveyId && x.SurveyUser.AnswerByUsers.Count == 0)
                        .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                        .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.SurveyUser.User.FullName.ToUpper().Contains(searchValue.ToUpper()));
            }

            Expression<Func<TeacherSurvey, dynamic>> selectPredicate = null;

            selectPredicate = (x) => new
            {
                student = x.SurveyUser.User.FullName,
                teacher = x.User.FullName,
                studentEmail = x.SurveyUser.User.Email,
                survey = x.SurveyUser.Survey.Name,
                finishDate = x.SurveyUser.Survey.FinishDate.ToString("dd-MM-yyyy")
            };
            return await query.ToDataTables2(sentParameters , selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherOnSurveySectionDatatable(DataTablesStructs.SentParameters sentParameters, Guid seccionId, Guid surveyId)
        {
            Expression<Func<TeacherSurvey, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.SurveyUserId);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }


            var query = _context.TeacherSurveys
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsQueryable();

            var surveyusers = _context.SurveyUsers.Where(x => x.SectionId == seccionId).AsQueryable();
            if (surveyId != Guid.Empty)
                surveyusers = surveyusers.Where(x => x.SurveyId == surveyId);
            var surveyusersids = surveyusers.Select(x => x.Id);
            var recordsFiltered = 0;
            recordsFiltered = await query.CountAsync();
            var data = await query
                .Where(x => surveyusersids.Contains(x.SurveyUserId))
                .Select(x => new
                {
                    id = x.SurveyUser.Section.Id,
                    teacher = x.User.FullName,
                    code = x.SurveyUser.Section.Code,
                    survey = x.SurveyUser.Survey.Name
                })
                .Distinct()
                .OrderBy(x=>x.id)
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
    }
}
