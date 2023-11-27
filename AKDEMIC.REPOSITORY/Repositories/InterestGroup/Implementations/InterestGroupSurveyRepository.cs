using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupSurveyTemplate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class InterestGroupSurveyRepository : Repository<InterestGroupSurvey>, IInterestGroupSurveyRepository
    {
        public InterestGroupSurveyRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE
        private async Task<Select2Structs.ResponseParameters> GetInterestGroupSurveysByInterestGroupIdSelect2(Guid? interestGroupId, Select2Structs.RequestParameters requestParameters, Expression<Func<InterestGroupSurvey, Select2Structs.Result>> selectPredicate, Func<InterestGroupSurvey, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.InterestGroupSurveys
                            .Where(x => x.InterestGroupId == interestGroupId)
                            .WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }
        #endregion

        #region PUBLIC 
        public async Task<DataTablesStructs.ReturnedData<SurveyTemplate>> GetInterestGroupSurveys(DataTablesStructs.SentParameters sentParameters,Guid?academicProgramId, string search, int year, int month,string userDegreeProgramId = null, Guid? interestGroupId = null, byte? status = null)
        {
            Expression<Func<InterestGroupSurvey, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {

                case "0":
                    orderByPredicate = ((x) => x.Survey.Code); break;
                case "1":
                    orderByPredicate = ((x) => x.Survey.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.InterestGroup.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Survey.CreatedDate); break;
                case "4":
                    orderByPredicate = ((x) => x.Survey.PublicationDate); break;
                case "5":
                    orderByPredicate = ((x) => x.Survey.FinishDate); break;
                default:
                    orderByPredicate = ((x) => x.Survey.CreatedDate); break;
            }

            var query = _context.InterestGroupSurveys.Include(x => x.Survey.Career.AcademicPrograms).Include(x => x.InterestGroup)
              .AsQueryable();

            if(!string.IsNullOrEmpty(userDegreeProgramId))
                query = query.Where(x=>x.InterestGroup.UserAdminId == userDegreeProgramId);

            if (interestGroupId.HasValue)
                query = query.Where(x => x.InterestGroupId == interestGroupId);

            if(academicProgramId.HasValue)
                query = query.Where(x =>
                        x.Survey.Career.AcademicPrograms
                            .Any(z => z.Id == academicProgramId.Value)
                    );


            if (year != 0 && month != 0)
                query = query.Where(x => x.Survey.PublicationDate.Year == year && x.Survey.PublicationDate.Month == month);

            if (status.HasValue)
                query = query.Where(x => x.Survey.State == status);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x => x.Survey.Name.Trim().ToLower().Contains(search) ||
                                        x.Survey.Description.Trim().ToLower().Contains(search) ||
                                        x.InterestGroup.Name.Trim().ToLower().Contains(search) ||
                                        x.Survey.Code.Trim().ToLower().Contains(search));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                    .Where(x => x.Survey.Type == ConstantHelpers.TYPE_SURVEY.GENERAL)
                    .Include(x => x.Survey.Career)
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                    .Select(
                    x => new SurveyTemplate()
                    {
                        Id = x.Survey.Id,
                        Name = x.Survey.Name,
                        Description = x.Survey.Description,
                        Code = x.Survey.Code,
                        PublicationDate = x.Survey.PublicationDate.ToString("dd/MM/yyyy"),
                        FinishDate = x.Survey.FinishDate.ToString("dd/MM/yyyy"),
                        InterestGroupName = x.InterestGroup.Name,
                        Status = ConstantHelpers.SURVEY_STATES.VALUES[x.Survey.State],
                        CreatedDate = x.Survey.CreatedDate.ToString("dd/MM/yyyy")
                    })
                   .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<SurveyTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<InterestGroupSurvey>> GetInterestGroupSurveysByInterestGroupId(Guid interestGroupId)
        {
            return await _context.InterestGroupSurveys.Where(x => x.InterestGroupId == interestGroupId).ToArrayAsync();
        }

        public async Task<Select2Structs.ResponseParameters> GetInterestGroupSurveysByInterestGroupIdSelect2(Select2Structs.RequestParameters requestParameters, Guid? interestGroupId, string searchValue = null)
        {
            return await GetInterestGroupSurveysByInterestGroupIdSelect2(interestGroupId, requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Survey.Id,
                Text = x.Survey.Name,
            }, (x) => new[] { x.Survey.Name }, searchValue);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetInterestGroupSurveysByInterestGroupIdSelect2ClientSide(Guid interestGroupId)
        {
            var query = await _context.InterestGroupSurveys
                .Where(x => x.InterestGroupId == interestGroupId)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Survey.Id,
                    Text = x.Survey.Name
                })
                .ToArrayAsync();

            return query;
        }

        public async Task<IEnumerable<SurveyUser>> GetInterestGroupSurveyUsersByUserIdAndInterestGroupId(string userId, Guid interestGroupId)
        {
            var interestGroupSurveys = await _context.InterestGroupSurveys
                .Where(x => x.InterestGroupId == interestGroupId).Select(x=>x.SurveyId).ToArrayAsync();

            var result = await _context.SurveyUsers
                .Include(x => x.Survey)
                .Where(x => x.UserId == userId &&
                !x.AnswerByUsers.Any(y => y.SurveyUser.UserId == x.UserId &&
                y.SurveyUser.SurveyId == x.SurveyId) && interestGroupSurveys.Contains(x.SurveyId)
                ).OrderByDescending(x => x.CreatedAt).ToArrayAsync();

            return result;
        }

        public async Task DeleteBySurveyId(Guid surveyId)
        {
            var interestGroupSurvey = await _context.InterestGroupSurveys.Where(x => x.SurveyId == surveyId).FirstOrDefaultAsync();
            _context.Remove(interestGroupSurvey);
            await _context.SaveChangesAsync();
        }
        //=> await _context.InterestGroupSurveys.Where(x => x.InterestGroupId == interestGroupId && x.Survey.SurveyUsers.Any(y => y.UserId == userId &&
        //!x.Survey.SurveyUsers.Any(z => z.AnswerByUsers.Any(g => g.SurveyUser.UserId == userId && g.SurveyUser.SurveyId == z.SurveyId)))).ToListAsync();

        public async Task<InterestGroupSurvey> GetInterestGroupSurveyBySurveyUser(Guid id)
        {
            var survey = await _context.SurveyUsers.FirstOrDefaultAsync(x => x.Id == id);

            return await _context.InterestGroupSurveys.Include(x => x.InterestGroup).FirstOrDefaultAsync(x => x.SurveyId == survey.SurveyId);
        }

        public async Task<int> GetCountSlopesSurveysByUserId(string userId)
            => await _context.InterestGroupSurveys.Where(x => x.Survey.SurveyUsers.Any(y => y.UserId == userId && y.DateTime == null)).CountAsync();
        
        #endregion
    }
}
