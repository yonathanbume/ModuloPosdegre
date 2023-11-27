using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template.CafobeRequest;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class PostulationRepository : Repository<Postulation>, IPostulationRepository
    {
        public PostulationRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<Postulation>> GetPostulationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? scholarshipId = null, byte? state = null, string search = null)
        {
            Expression<Func<Postulation, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.Postulations
                .Where(x => x.IsCompleted)
                .AsQueryable();

            if (scholarshipId.HasValue)
                query = query.Where(x => x.Questionnaire.ScholarshipId == scholarshipId);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x => x.User.FullName.Trim().ToLower().Contains(search) || x.PostulationInformation.FullName.Trim().ToLower().Contains(search));
            }

            if (state.HasValue)
                query = query.Where(x => x.State == state);

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<Postulation, Postulation>> selectPredicate = (x) => new Postulation
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                User = new ApplicationUser
                {
                    Id = x.User.Id,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    UserName = x.User.UserName,
                    FullName= x.User.FullName,
                    Email = x.User.Email,
                    Document = x.User.Document,
                    PhoneNumber = x.User.PhoneNumber
                },
                PostulationInformation = new PostulationInformation
                {
                    DNI = x.PostulationInformation.DNI,
                    Email = x.PostulationInformation.Email,
                    FullName = x.PostulationInformation.FullName
                },
                Questionnaire = new Questionnaire
                {
                    Id = x.Questionnaire.Id,
                    Scholarship = new Scholarship
                    {
                        Id = x.Questionnaire.Scholarship.Id,
                        Name = x.Questionnaire.Scholarship.Name,
                        Program = x.Questionnaire.Scholarship.Program,
                        Target = x.Questionnaire.Scholarship.Target
                    }
                },
                State = x.State
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<Postulation>> GetAdmittedDatatable(DataTablesStructs.SentParameters sentParameters, Guid? scholarshipId = null, byte? state = null, string search = null)
        {
            Expression<Func<Postulation, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.Postulations
                .Where(x => x.IsCompleted && x.Admitted.HasValue && x.Admitted.Value)
                .AsQueryable();

            if (scholarshipId.HasValue)
                query = query.Where(x => x.Questionnaire.ScholarshipId == scholarshipId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => 
                x.PostulationInformation.FullName.Trim().ToLower().Contains(search.Trim().ToLower()) ||
                x.User.UserName.Trim().ToLower().Contains(search.Trim().ToLower()) ||
                x.PostulationInformation.DNI.Trim().ToLower().Contains(search.Trim().ToLower())
                );

            if (state.HasValue)
                query = query.Where(x => x.State == state);

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<Postulation, Postulation>> selectPredicate = (x) => new Postulation
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                User = new ApplicationUser
                {
                    Id = x.User.Id,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    UserName = x.User.UserName,
                    FullName = x.User.FullName,
                    Email = x.User.Email,
                    Document = x.User.Document,
                    PhoneNumber = x.User.PhoneNumber
                },
                PostulationInformation = new PostulationInformation
                {
                    DNI = x.PostulationInformation.DNI,
                    Email = x.PostulationInformation.Email,
                    FullName = x.PostulationInformation.FullName
                },
                Questionnaire = new Questionnaire
                {
                    Id = x.Questionnaire.Id,
                    Scholarship = new Scholarship
                    {
                        Id = x.Questionnaire.Scholarship.Id,
                        Name = x.Questionnaire.Scholarship.Name
                    }
                },
                State = x.State
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<Guid> InsertAndReturnId(Postulation questionnaireByUser)
        {
            await _context.Postulations.AddAsync(questionnaireByUser);
            await _context.SaveChangesAsync();
            return questionnaireByUser.Id;
        }

        public async Task<bool> WasAnsweredByuser(Guid questionnaireId, string userId, string externalUser = null)
        {
            return await _context.Postulations.AnyAsync(x => x.QuestionnaireId == questionnaireId && x.UserId == userId);
        }

        public async Task<Postulation> GetQuestionnaireByPostulationId(Guid postulationId)
        {
            var query = await _context.Postulations
                .Include(x => x.Questionnaire)
                    .ThenInclude(x => x.QuestionnaireSections)
                        .ThenInclude(x => x.QuestionnaireQuestions)
                            .ThenInclude(x => x.QuestionnaireAnswerByUsers)
                .Include(x => x.Questionnaire)
                    .ThenInclude(x => x.QuestionnaireSections)
                        .ThenInclude(x => x.QuestionnaireQuestions)
                            .ThenInclude(x => x.QuestionnaireAnswers)
                                .ThenInclude(x => x.QuestionnaireAnswerByUsers)
                .Include(x => x.QuestionnaireAnswerByUsers)
                .Where(x => x.Id == postulationId)
                .FirstOrDefaultAsync();

            return query;
        }

        public async Task<List<PostulationTemplate>> GetPostulationsReport(Guid? scholarshipId = null, byte? state = null)
        {
            var data = await _context
            .Postulations
            .Select(x => new PostulationTemplate
            {
                Id = x.Id,
                CreatedAtStr = x.CreatedAt.ToLocalDateFormat(),
                UserFullName = x.User.Id == null ? x.PostulationInformation.FullName : x.User.FullName,
                PostulationEmail = x.User.Id == null ? x.PostulationInformation.Email : x.User.Email,
                ScholarshipName = x.Questionnaire.Scholarship.Name,
                ScholarshipId = x.Questionnaire.ScholarshipId,
                State = x.State,
                StateText = ConstantHelpers.ACADEMIC_EXCHANGE.POSTULATION_STATE.VALUES.ContainsKey(x.State) ?
                    ConstantHelpers.ACADEMIC_EXCHANGE.POSTULATION_STATE.VALUES[x.State] : "",
            })
            .ToListAsync();

            if (state != 0) data = data.Where(x => x.State == state).ToList();

            if (scholarshipId.HasValue)
                data = data.Where(x => x.ScholarshipId == scholarshipId).ToList();

            return data;
        }

        public async Task<object> GetPostulantsByscholarship()
        {
            var result = await _context.Postulations
                //.Where(x => !string.IsNullOrEmpty(x.UserId))
                .Where(x => x.IsCompleted)
                .Select(x => new
                {
                    scholarship = x.Questionnaire.Scholarship.Name
                }).ToListAsync();

            var total = result.GroupBy(x => x.scholarship).OrderBy(x => x.Key).ToList();
            List<string> categories = total.Select(x => x.Key).ToList();
            List<int> data = total.Select(x => x.Count()).ToList();
            return new { categories, data };
        }

        public async Task<object> GetAdmittedByscholarship()
        {
            var result = await _context.Postulations
                .Where(x =>/* !string.IsNullOrEmpty(x.UserId) &&*/ x.IsCompleted && (x.Admitted.HasValue ? x.Admitted.Value : false))
                .Select(x => new
                {
                    scholarship = x.Questionnaire.Scholarship.Name
                }).ToListAsync();

            var total = result.GroupBy(x => x.scholarship).OrderBy(x => x.Key).ToList();
            List<string> categories = total.Select(x => x.Key).ToList();
            List<int> data = total.Select(x => x.Count()).ToList();
            return new { categories, data };
        }

        public async Task<object> GetAdmittedByProgram()
        {
            var result = await _context.Postulations
                .Where(x => /*!string.IsNullOrEmpty(x.UserId) &&*/ x.IsCompleted && (x.Admitted.HasValue ? x.Admitted.Value : false))
                .Select(x => new
                {
                    program = x.Questionnaire.Scholarship.Program
                }).ToListAsync();

            var total = result.GroupBy(x => x.program).OrderBy(x => x.Key).ToList();
            List<string> categories = total.Select(x => x.Key.ToString()).ToList();
            List<int> data = total.Select(x => x.Count()).ToList();
            return new { categories, data };
        }

        public async Task<object> GetPostulantsByProgram()
        {
            var result = await _context.Postulations
                .Where(x => x.IsCompleted)
               //.Where(x => !string.IsNullOrEmpty(x.UserId))
               .Select(x => new
               {
                   program = x.Questionnaire.Scholarship.Program
               }).ToListAsync();

            var total = result.GroupBy(x => x.program).OrderBy(x => x.Key).ToList();
            List<string> categories = total.Select(x => x.Key.ToString()).ToList();
            List<int> data = total.Select(x => x.Count()).ToList();
            return new { categories, data };
        }

        public async Task<object> GetReportByTerm(Guid termId)
        {
            var query = _context.Postulations.Include(x => x.Questionnaire).AsQueryable();

            //postulantes
            var postulants = await query
                 .Where(x => x.IsCompleted).ToListAsync();

            //admitidos
            var admitteds = await query
                .Where(x => x.IsCompleted && (x.Admitted.HasValue ? x.Admitted.Value : false)).ToListAsync();

            var resultScholarship = await _context.Scholarships
               .Where(x => x.TermId == termId)
               .Select(x => new
               {
                   x.Id,
                   program = x.Name,
               }).ToListAsync();

            var scholarships = resultScholarship.GroupBy(x => x.program).ToList();
            var scholarshipsIds = resultScholarship.GroupBy(x => x.Id).Select(x => new { x.Key, count = x.Count() }).ToList();

            var categories = scholarships.Select(x => x.Key).ToList();

            var groupedPostulants = postulants
                .GroupBy(x => x.Questionnaire.ScholarshipId)
                .Select(x => new { x.Key, count = x.Count() });

            var groupedAdmitteds = admitteds
                .GroupBy(x => x.Questionnaire.ScholarshipId)
                .Select(x => new { x.Key, count = x.Count() });


            var dataPostulants = new List<int>();
            var dataAdmitted = new List<int>();
            foreach (var item in scholarshipsIds)
            {
                var countp = groupedPostulants.FirstOrDefault(x => x.Key == item.Key);
                var counta = groupedAdmitteds.FirstOrDefault(x => x.Key == item.Key);
                dataPostulants.Add(countp?.count ?? 0);
                dataAdmitted.Add(counta?.count ?? 0);
            }

            var seriePostulants = new Tuple<string, List<int>>("Postulados", dataPostulants);
            var serieAdmitteds = new Tuple<string, List<int>>("Admitidos", dataAdmitted);

            return new { categories, seriePostulants, serieAdmitteds };
        }

        public async Task<Postulation> GetByUserIdAndScholarshipId(Guid scholarshipId, string userId)
        {
            return await _context.Postulations.Where(x => x.UserId == userId && x.Questionnaire.ScholarshipId == scholarshipId).FirstOrDefaultAsync();
        }

        public async Task<Postulation> GetByUserIdAndQuestionnaireId(string userId, Guid questionnaireId)
        {
            return await _context.Postulations.Where(x => x.UserId == userId && x.QuestionnaireId == questionnaireId).FirstOrDefaultAsync();
        }

        public async Task<object> GetReportByCareerChart()
        {
            var careers = await _context.Careers.OrderByDescending(x => x.Name).Select(x => x.Name).ToArrayAsync();
            var parentResult = await _context.PostulationInformations.Where(x=>x.CareerApplyId.HasValue).GroupBy(x => x.CareerApply.Name)
                .Select(x => new
                {
                    name = x.Key,
                    y = x.Count(),
                    drilldown = x.Key
                })
                .ToArrayAsync();

            var query = _context.Postulations.Where(x => x.IsCompleted && x.PostulationInformationId.HasValue)
                .AsQueryable();

            var details = await query
                .Where(x=>x.PostulationInformation.CareerApplyId.HasValue)
                .GroupBy(x => x.PostulationInformation.CareerApply.Name)
                .Select(x => new ReportByCareerTemplate
                {
                    Name = x.Key,
                    Id = x.Key
                })
                .ToArrayAsync();


            foreach (var item in details)
            {

                var objectDetails = new List<object[]>();

                var fd = await query.Where(x => x.PostulationInformationId.HasValue && x.PostulationInformation.CareerApply.Name == item.Name)
                    .GroupBy(x => new { x.Admitted, x.State })
                    .Select(x => new
                    {
                        state = x.Key.State,
                        admitted = x.Key.Admitted,
                        count = x.Count()
                    })
                    .ToArrayAsync();

                var total = fd.Sum(x => x.count);
                var rejected = fd.Where(x => x.state == ConstantHelpers.SCHOLARSHIP.POSTULATION.States.REJECTED || (x.admitted.HasValue && !x.admitted.Value)).Select(x => x.count).FirstOrDefault();
                var admitted = fd.Where(x => x.admitted.HasValue && x.admitted.Value).Select(x => x.count).FirstOrDefault();
                var pending = total - (rejected + admitted);

                objectDetails.Add(new object[] {"Admitidos y Ganadores de Beca", admitted});
                objectDetails.Add(new object[] { "Rechazados y/o No Ganaron Beca",rejected });
                objectDetails.Add(new object[] { "Pendientes", pending });

                item.Data = objectDetails;
            }

            return new { parentResult, details };
        }
    }
}
