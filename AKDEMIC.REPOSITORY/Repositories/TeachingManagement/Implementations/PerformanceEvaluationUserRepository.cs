using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluationUser;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class PerformanceEvaluationUserRepository : Repository<PerformanceEvaluationUser>, IPerformanceEvaluationUserRepository
    {
        public PerformanceEvaluationUserRepository(AkdemicContext context) : base(context) { }
        public async Task<bool> ValidatePerformanceEvaluationUser(string fromRoleId, Guid templateId, string fromId, string toId, Guid? sectionId = null)
        {
            var today = DateTime.UtcNow.ToDefaultTimeZone().Date;
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE && x.StartDate.Date <= today && x.EndDate.Date >= today).FirstOrDefaultAsync();
            var query = _context.PerformanceEvaluationUsers.Where(x => x.PerformanceEvaluationId == evaluation.Id && x.PerformanceEvaluationTemplateId == templateId && x.FromUserId == fromId && x.ToTeacherId == toId && x.FromRoleId == fromRoleId).AsQueryable();

            if (sectionId.HasValue && sectionId != null)
                query = query.Where(x => x.SectionId == sectionId);

            PerformanceEvaluationUser performanceEvaluationUser = await query.FirstOrDefaultAsync();
            return performanceEvaluationUser != null;
        }

        public async Task<ReturnedData<object>> GetEvaluatedUsers(SentParameters sentParameters, Guid evaluationId, string userId = null)
        {
            PerformanceEvaluation evaluation = await _context.PerformanceEvaluations.AsNoTracking().Where(x => x.Id == evaluationId).FirstAsync();

            var query = _context.PerformanceEvaluationUsers.Where(x => x.PerformanceEvaluationId == evaluation.Id).AsNoTracking();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.ToTeacherId == userId);

            var evaluationUsers = await query.Select(x => new
            {
                toUserId = x.ToTeacherId,
                toUserName = x.ToTeacher.User.UserName,
                toRoleId = x.PerformanceEvaluationTemplate.RoleId,
                toRoleName = x.PerformanceEvaluationTemplate.Role.Name
            }).Distinct().OrderBy(x => x.toRoleName).ThenBy(x => x.toUserName).ToListAsync();

            int total = evaluationUsers.Count();

            evaluationUsers = evaluationUsers.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = evaluationUsers,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = total,
                RecordsTotal = evaluationUsers.Count
            };
        }

        public async Task<PerformanceEvaluationUserReportTemplate> GetEvalutedTeachersDatatableClientSide(Guid evaluationId, Guid academicDepartmentId)
        {
            var evaluation = await _context.PerformanceEvaluations.AsNoTracking().Where(x => x.Id == evaluationId).FirstAsync();
            var term = await _context.Terms.Where(x => x.Id == evaluation.TermId).FirstOrDefaultAsync();

            var academicDepartment = await _context.AcademicDepartments.Where(x => x.Id == academicDepartmentId).FirstOrDefaultAsync();

            var queryUserGroup = _context.PerformanceEvaluationUsers.Where(x => x.PerformanceEvaluationId == evaluation.Id).AsNoTracking();

            if (academicDepartment is null)
            {
                queryUserGroup = queryUserGroup.Where(x => !x.ToTeacher.AcademicDepartmentId.HasValue);
            }
            else
            {
                queryUserGroup = queryUserGroup.Where(x => x.ToTeacher.AcademicDepartmentId == academicDepartment.Id);
            }

            var userGroup = await queryUserGroup
                .GroupBy(x => x.ToTeacherId)
                .Select(x => new
                {
                    x.Key,
                    total = x.Count()
                })
            .ToListAsync();


            //var career = await _context.Careers.Where(x => x.Id == careerId).Select(x => new { x.Name, x.Id }).FirstOrDefaultAsync();

            var result = new PerformanceEvaluationUserReportTemplate
            {
                AcademicDepartment = academicDepartment?.Name ?? "Sin Asignar",
                Term = term.Name,
                Users = new List<PerformanceEvaluationUserReportDetailTemplate>()
            };

            var index = 1;

            var questionsByTemplateDB = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluationId)
                .Select(x => new
                {
                    questions = x.PerformanceEvaluationTemplate.Questions.Count(),
                    role = x.PerformanceEvaluationTemplate.Role.Name,
                    x.PerformanceEvaluationTemplate.Max
                })
                .ToListAsync();

            //var questionsByTemplateDB = await _context.PerformanceEvaluationTemplates.Where(x => x.IsActive)
            //    .Select(x => new
            //    {
            //        questions = x.Questions.Count(),
            //        role = x.Role.Name,
            //        x.Max
            //    })
            //    .ToListAsync();

            var questionsByTemplate = questionsByTemplateDB
                .Select(x => new
                {
                    x.questions,
                    x.role,
                    x.Max,
                    maximumRaiting = x.questions * (int)x.Max
                })
                .ToList();

            var questionsToStudent = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.STUDENTS).Select(x => x.questions).FirstOrDefault();
            var questionsToAcademicDepartmentDirector = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Select(x => x.questions).FirstOrDefault();
            var questionsToCareerDirector = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.CAREER_DIRECTOR).Select(x => x.questions).FirstOrDefault();
            var questionsToDean = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.DEAN).Select(x => x.questions).FirstOrDefault();
            var questionsToResearchCoordinator = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Select(x => x.questions).FirstOrDefault();
            var questionsToSocialResponsbilityCoordinator = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Select(x => x.questions).FirstOrDefault();
            var questionsToTutoringCoordinator = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Select(x => x.questions).FirstOrDefault();

            var MaximumRaitingStudent = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.STUDENTS).Select(x => x.maximumRaiting).FirstOrDefault() == 0 ? 1 : questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.STUDENTS).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingAcademicDepartmentDirector = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Select(x => x.maximumRaiting).FirstOrDefault() == 0 ? 1 : questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingCareerDirector = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.CAREER_DIRECTOR).Select(x => x.maximumRaiting).FirstOrDefault() == 0 ? 1 : questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.CAREER_DIRECTOR).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingDean = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.DEAN).Select(x => x.maximumRaiting).FirstOrDefault() == 0 ? 1 : questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.DEAN).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingResearchCoordinator = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Select(x => x.maximumRaiting).FirstOrDefault() == 0 ? 1 : questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingToSocialResponsbilityCoordinator = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Select(x => x.maximumRaiting).FirstOrDefault() == 0 ? 1 : questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingToTutoringCoordinator = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Select(x => x.maximumRaiting).FirstOrDefault() == 0 ? 1 : questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Select(x => x.maximumRaiting).FirstOrDefault());

            foreach (var user in userGroup)
            {
                var teacher = await _context.Teachers.Where(x => x.UserId == user.Key).Include(X => X.User).FirstOrDefaultAsync();
                if (teacher is null/* || teacher.AcademicDepartmentId != academicDepartment.Id*/)
                    continue;

                var performanceEvaluationUser = await _context.PerformanceEvaluationUsers
                    .Where(x => x.PerformanceEvaluationId == evaluation.Id && x.ToTeacherId == user.Key)
                    .Select(x => new
                    {
                        x.FromUserId,
                        FromRoleName = x.FromRole.Name,
                        SumResponses = x.Responses.Sum(y => y.Value)
                    })
                    .ToListAsync();

                var performanceEvaluationStudents = performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.STUDENTS).ToList();
                var userWithRoleStudentUserIds = performanceEvaluationStudents.Select(x => x.FromUserId).ToList();
                var studentsWhoResponded = await _context.Students.Where(x => userWithRoleStudentUserIds.Contains(x.UserId)).Select(x => new { x.CareerId, x.UserId }).ToListAsync();

                var totalStudentsWhoResponded = new List<EvaluationStudentTemplate>();
                userWithRoleStudentUserIds.ForEach(x =>
                {
                    totalStudentsWhoResponded.Add(new EvaluationStudentTemplate
                    {
                        UserId = x,
                        CareerId = studentsWhoResponded.Where(y => y.UserId == x).Select(x => x.CareerId).FirstOrDefault()
                    });
                });

                var studentsWhoRespondedSameCareer = totalStudentsWhoResponded.Where(x => x.CareerId == academicDepartment?.CareerId).Count();
                var studentsWhoRespondedOtherCareer = totalStudentsWhoResponded.Where(x => x.CareerId != academicDepartment?.CareerId).Count();

                var vigesimal_finalGradeSameCareer = performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId == academicDepartment?.CareerId)).Any() ? GetFinalGradePerformanceEvaluation(performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId == academicDepartment?.CareerId)).Sum(x => x.SumResponses), MaximumRaitingStudent, performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId == academicDepartment?.CareerId)).Count(), true) : 0;
                var vigesimal_finalGradeOtherCareer = performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId != academicDepartment?.CareerId)).Any() ? GetFinalGradePerformanceEvaluation(performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId != academicDepartment?.CareerId)).Sum(x => x.SumResponses), MaximumRaitingStudent, performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId != academicDepartment?.CareerId)).Count(), true) : 0;

                var total_finalGradeSameCareer = performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId == academicDepartment?.CareerId)).Any() ? GetFinalGradePerformanceEvaluation(performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId == academicDepartment?.CareerId)).Sum(x => x.SumResponses), MaximumRaitingStudent, performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId == academicDepartment?.CareerId)).Count(), false) : 0;
                var total_finalGradeOtherCareer = performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId != academicDepartment?.CareerId)).Any() ? GetFinalGradePerformanceEvaluation(performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId != academicDepartment?.CareerId)).Sum(x => x.SumResponses), MaximumRaitingStudent, performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId && y.CareerId != academicDepartment?.CareerId)).Count(), false) : 0;

                //Final Grade Students
                var vigesimal_finalGradeStudents = performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId)).Any() ? GetFinalGradePerformanceEvaluation(performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId)).Sum(x => x.SumResponses), MaximumRaitingStudent, performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId)).Count(), true) : 0;
                var total_finalGradeStudents = performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId)).Any() ? GetFinalGradePerformanceEvaluation(performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId)).Sum(x => x.SumResponses), MaximumRaitingStudent, performanceEvaluationStudents.Where(x => totalStudentsWhoResponded.Any(y => y.UserId == x.FromUserId)).Count(), false) : 0;

                var sections = await _context.TeacherSections.Where(x => x.TeacherId == teacher.UserId && x.Section.CourseTerm.TermId == evaluation.TermId).Select(x => x.SectionId).ToListAsync();
                var totalStudentCareer = await _context.StudentSections.Where(x => sections.Contains(x.SectionId)).Select(x => x.Student.CareerId).ToListAsync();
                var totalStudentSameCareer = totalStudentCareer.Where(x => x == academicDepartment?.CareerId).Count();
                var totalStudentOtherCareer = totalStudentCareer.Where(x => x != academicDepartment?.CareerId).Count();


                //FinalGrade Authorities
                var vigesimal_academicDepartmentDirectorFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Sum(x => x.SumResponses), MaximumRaitingAcademicDepartmentDirector, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Count(), true) : 0;
                var vigesimal_careerDirectorFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.CAREER_DIRECTOR) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.CAREER_DIRECTOR).Sum(x => x.SumResponses), MaximumRaitingCareerDirector, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.CAREER_DIRECTOR).Count(), true) : 0;
                var vigesimal_deanFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.DEAN) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.DEAN).Sum(x => x.SumResponses), MaximumRaitingDean, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.DEAN).Count(), true) : 0;
                var vigesimal_researchCoordinatorFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.RESEARCH_COORDINATOR) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Sum(x => x.SumResponses), MaximumRaitingResearchCoordinator, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Count(), true) : 0;
                var vigesimal_socialResponsabilityCoordinatorFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Sum(x => x.SumResponses), MaximumRaitingToSocialResponsbilityCoordinator, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Count(), true) : 0;
                var vigesimal_tutoringCoordinatorFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.TUTORING_COORDINATOR) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Sum(x => x.SumResponses), MaximumRaitingToTutoringCoordinator, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Count(), true) : 0;

                var total_academicDepartmentDirectorFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Sum(x => x.SumResponses), MaximumRaitingAcademicDepartmentDirector, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Count(), false) : 0;
                var total_careerDirectorFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.CAREER_DIRECTOR) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.CAREER_DIRECTOR).Sum(x => x.SumResponses), MaximumRaitingCareerDirector, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.CAREER_DIRECTOR).Count(), false) : 0;
                var total_deanFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.DEAN) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.DEAN).Sum(x => x.SumResponses), MaximumRaitingDean, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.DEAN).Count(), false) : 0;
                var total_researchCoordinatorFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.RESEARCH_COORDINATOR) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Sum(x => x.SumResponses), MaximumRaitingResearchCoordinator, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Count(), false) : 0;
                var total_socialResponsabilityCoordinatorFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Sum(x => x.SumResponses), MaximumRaitingToSocialResponsbilityCoordinator, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Count(), false) : 0;
                var total_tutoringCoordinatorFinalGrade = performanceEvaluationUser.Any(x => x.FromRoleName == ConstantHelpers.ROLES.TUTORING_COORDINATOR) ? GetFinalGradePerformanceEvaluation(performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Sum(x => x.SumResponses), MaximumRaitingToTutoringCoordinator, performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Count(), false) : 0;

                decimal dividedBy = 0;

                if (total_academicDepartmentDirectorFinalGrade != 0)
                    dividedBy += MaximumRaitingAcademicDepartmentDirector;
                if (total_careerDirectorFinalGrade != 0)
                    dividedBy += MaximumRaitingCareerDirector;
                if (total_deanFinalGrade != 0)
                    dividedBy += MaximumRaitingDean;
                if (total_researchCoordinatorFinalGrade != 0)
                    dividedBy += MaximumRaitingResearchCoordinator;
                if (total_socialResponsabilityCoordinatorFinalGrade != 0)
                    dividedBy += MaximumRaitingToSocialResponsbilityCoordinator;
                if (total_tutoringCoordinatorFinalGrade != 0)
                    dividedBy += MaximumRaitingToTutoringCoordinator;
                if (dividedBy == 0)
                    dividedBy = 1;

                var vigesimal_finalGradeAuthorities = ((vigesimal_academicDepartmentDirectorFinalGrade * MaximumRaitingAcademicDepartmentDirector) +
                                            (vigesimal_careerDirectorFinalGrade * MaximumRaitingCareerDirector) +
                                            (vigesimal_deanFinalGrade * MaximumRaitingDean) +
                                            (vigesimal_researchCoordinatorFinalGrade * MaximumRaitingResearchCoordinator) +
                                            (vigesimal_socialResponsabilityCoordinatorFinalGrade * MaximumRaitingToSocialResponsbilityCoordinator) +
                                            (vigesimal_tutoringCoordinatorFinalGrade * MaximumRaitingToTutoringCoordinator)) / dividedBy;

                var total_finalGradeAuthorities = ((total_academicDepartmentDirectorFinalGrade * MaximumRaitingAcademicDepartmentDirector) +
                                            (total_careerDirectorFinalGrade * MaximumRaitingCareerDirector) +
                                            (total_deanFinalGrade * MaximumRaitingDean) +
                                            (total_researchCoordinatorFinalGrade * MaximumRaitingResearchCoordinator) +
                                            (total_socialResponsabilityCoordinatorFinalGrade * MaximumRaitingToSocialResponsbilityCoordinator) +
                                            (total_tutoringCoordinatorFinalGrade * MaximumRaitingToTutoringCoordinator)) / dividedBy;


                var vigesimal_finalGrade = 0M;
                var total_finalGrade = 0M;

                if (evaluation.Target == ConstantHelpers.PERFORMANCE_EVALUATION.TARGET.ALL)
                {
                    if (total_finalGradeAuthorities > 0M)
                    {
                        vigesimal_finalGrade += vigesimal_finalGradeAuthorities;
                        total_finalGrade += total_finalGradeAuthorities;
                    }

                    if (total_finalGradeStudents > 0M)
                    {
                        vigesimal_finalGrade += vigesimal_finalGradeStudents;
                        total_finalGrade += total_finalGradeStudents;
                    }

                    if (total_finalGradeAuthorities > 0M && total_finalGradeStudents > 0M)
                    {
                        vigesimal_finalGrade /= 2;
                        total_finalGrade /= 2;
                    }
                }
                else
                {
                    vigesimal_finalGrade = vigesimal_finalGradeStudents;
                    total_finalGrade = total_finalGradeStudents;
                }

                var template = new PerformanceEvaluationUserReportDetailTemplate
                {
                    UserId = teacher.UserId,
                    Number = index,
                    UserCode = teacher.User.UserName,
                    UserFullName = teacher.User.FullName,
                    StudentSameCareer = new PerformanceEvaluationStudentTemplate
                    {
                        Total = totalStudentSameCareer,
                        Remaining = totalStudentSameCareer - studentsWhoRespondedSameCareer,
                        Answered = studentsWhoRespondedSameCareer,
                        VigesimalFinalGrade = Math.Round(vigesimal_finalGradeSameCareer, 2, MidpointRounding.AwayFromZero),
                        TotalFinalGrade = Math.Round(total_finalGradeSameCareer, 2, MidpointRounding.AwayFromZero),
                    },
                    StudentOtherCareer = new PerformanceEvaluationStudentTemplate
                    {
                        Total = totalStudentOtherCareer,
                        Remaining = totalStudentOtherCareer - studentsWhoRespondedOtherCareer,
                        Answered = studentsWhoRespondedOtherCareer,
                        VigesimalFinalGrade = Math.Round(vigesimal_finalGradeOtherCareer, 2, MidpointRounding.AwayFromZero),
                        TotalFinalGrade = Math.Round(total_finalGradeOtherCareer, 2, MidpointRounding.AwayFromZero)
                    },

                    VigesimalStudentFinalGrade = Math.Round(vigesimal_finalGradeStudents, 2, MidpointRounding.AwayFromZero),
                    VigesimalAcademicDepartmentDirectorFinalGrade = Math.Round(vigesimal_academicDepartmentDirectorFinalGrade, 2, MidpointRounding.AwayFromZero),
                    VigesimalCareerDirectorFinalGrade = Math.Round(vigesimal_careerDirectorFinalGrade, 2, MidpointRounding.AwayFromZero),
                    VigesimalDeanFinalGrade = Math.Round(vigesimal_deanFinalGrade, 2, MidpointRounding.AwayFromZero),
                    VigesimalResearchCoordinatorFinalGrade = Math.Round(vigesimal_researchCoordinatorFinalGrade, 2, MidpointRounding.AwayFromZero),
                    VigesimalSocialResponsabilityCoordinatorFinalGrade = Math.Round(vigesimal_socialResponsabilityCoordinatorFinalGrade, 2, MidpointRounding.AwayFromZero),
                    VigesimalTutoringCoordinatorFinalGrade = Math.Round(vigesimal_tutoringCoordinatorFinalGrade, 2, MidpointRounding.AwayFromZero),
                    VigesimalFinalGradeAuthorities = Math.Round(vigesimal_finalGradeAuthorities, 2, MidpointRounding.AwayFromZero),
                    VigesimalFinalGrade = Math.Round(vigesimal_finalGrade, 2, MidpointRounding.AwayFromZero),

                    TotalStudentFinalGrade = Math.Round(total_finalGradeStudents, 2, MidpointRounding.AwayFromZero),
                    TotalAcademicDepartmentDirectorFinalGrade = Math.Round(total_academicDepartmentDirectorFinalGrade, 2, MidpointRounding.AwayFromZero),
                    TotalCareerDirectorFinalGrade = Math.Round(total_careerDirectorFinalGrade, 2, MidpointRounding.AwayFromZero),
                    TotalDeanFinalGrade = Math.Round(total_deanFinalGrade, 2, MidpointRounding.AwayFromZero),
                    TotalResearchCoordinatorFinalGrade = Math.Round(total_researchCoordinatorFinalGrade, 2, MidpointRounding.AwayFromZero),
                    TotalSocialResponsabilityCoordinatorFinalGrade = Math.Round(total_socialResponsabilityCoordinatorFinalGrade, 2, MidpointRounding.AwayFromZero),
                    TotalTutoringCoordinatorFinalGrade = Math.Round(total_tutoringCoordinatorFinalGrade, 2, MidpointRounding.AwayFromZero),
                    TotalFinalGradeAuthorities = Math.Round(total_finalGradeAuthorities, 2, MidpointRounding.AwayFromZero),
                    TotalFinalGrade = Math.Round(total_finalGrade, 2, MidpointRounding.AwayFromZero),
                };

                index++;
                result.Users.Add(template);
            }

            result.Users = result.Users.OrderBy(x => x.UserFullName).ToList();

            return result;
        }


        public async Task<PerformanceEvaluationUserDetail> GetResult(Guid evaluationId, string toUserId, string fromRoleId, Guid? sectionId = null)
        {
            PerformanceEvaluationUserDetail result = new PerformanceEvaluationUserDetail()
            {
                Roles = new List<Role>(),
                Total = 0,
            };

            var fromRole = await _context.Roles.Where(x => x.Id == fromRoleId).FirstOrDefaultAsync();

            PerformanceEvaluation evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId).FirstAsync();
            var template = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluation.Id && x.PerformanceEvaluationTemplate.RoleId == fromRoleId).Select(x => x.PerformanceEvaluationTemplate).FirstOrDefaultAsync();

            if (template is null)
                return null;

            var templateInformation = await _context.RelatedPerformanceEvaluationTemplates
                .Where(x => x.PerformanceEvaluationId == evaluationId && x.PerformanceEvaluationTemplate.RoleId == fromRoleId)
                .Select(x => new
                {
                    count = x.PerformanceEvaluationTemplate.Questions.Count(),
                    maximumRaiting = x.PerformanceEvaluationTemplate.Questions.Count() * x.PerformanceEvaluationTemplate.Max
                })
                .FirstOrDefaultAsync();

            var usersQuery = _context.PerformanceEvaluationUsers.Where(x => x.PerformanceEvaluationId == evaluation.Id && x.PerformanceEvaluationTemplateId == template.Id && x.FromRole.Id == fromRoleId && x.ToTeacherId == toUserId);
            var queryResponses = _context.PerformanceEvaluationResponses.AsQueryable();

            if (fromRole.Name == ConstantHelpers.ROLES.STUDENTS && sectionId.HasValue && sectionId != Guid.Empty)
            {
                usersQuery = usersQuery.Where(x => x.SectionId == sectionId);
                queryResponses = queryResponses.Where(x => x.PerformanceEvaluationUser.SectionId == sectionId);
                result.SectionId = sectionId;
            }

            var users = await usersQuery.ToListAsync();
            var evaluationsUsersId = users.Select(x => x.Id).ToList();

            queryResponses = queryResponses.Where(x => evaluationsUsersId.Contains(x.PerformanceEvaluationUserId));
            var responses = await queryResponses.ToListAsync();

            result.Questions = await _context.PerformanceEvaluationQuestions.Where(x => x.PerformanceEvaluationTemplateId == template.Id)
                .Select(x => new Question
                {
                    Id = x.Id,
                    Description = x.Description,
                    Max = x.PerformanceEvaluationTemplate.Max,
                    CriterionId = x.PerformanceEvaluationCriterionId
                })
                .ToListAsync();

            if (template.EnabledCriterions)
            {
                result.Criterions = await _context.PerformanceEvaluationCriterions.Where(x => x.PerformanceEvaluationTemplateId == template.Id)
                    .Select(x => new PECriterion
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToListAsync();
            }

            var usersCount = users.Count() == 0 ? 1 : users.Count();

            for (int i = 0; i < result.Questions.Count; i++)
            {
                result.Questions[i].Responses = new List<Response>();

                for (int j = 1; j <= result.Questions[i].Max; j++)
                {
                    result.Questions[i].Responses.Add(new Response
                    {
                        Value = j,
                        Count = responses.Where(x => x.PerformanceEvaluationQuestionId == result.Questions[i].Id && x.Value == j).Count()
                    });
                }

                result.Questions[i].Average = Math.Round(((decimal)result.Questions[i].Responses.Sum(x => x.Value * x.Count) / (decimal)usersCount), 2, MidpointRounding.AwayFromZero);
            }

            var rubrics = await _context.PerformanceEvaluationRubrics.Where(x => x.PerformanceEvaluationId == evaluationId).OrderByDescending(x => x.Max).Select(x => new
            {
                max = x.Max,
                description = x.Description
            }).ToListAsync();

            result.EvaluatedStudents = users.Count();
            result.Total = Math.Round((users.Sum(x => x.Value) / usersCount), 2, MidpointRounding.AwayFromZero);

            var scales = await _context.PerformanceEvaluationRatingScales.Where(x => x.MaxScore == result.Questions[0].Max)
                .OrderByDescending(x => x.Value)
                .ToListAsync();

            if (result.Questions != null && result.Questions.Any())
            {
                result.ChartData = new object[result.Questions[0].Max];
                var respones = result.Questions.SelectMany(x => x.Responses).ToList();
                var index = 0;

                if (scales.Any() && scales.Count() == result.Questions[0].Max)
                {
                    for (int i = scales.Count; i > 0; i--)
                    {
                        result.ChartData[index] = new object[]
                        {
                        scales.Where(y=>y.Value == i).Select(y=>y.Description).FirstOrDefault(),
                        respones.Where(x=>x.Value == i).Sum(x=>x.Count)
                        };
                        index++;
                    }
                }
                else
                {
                    for (int i = result.Questions[0].Max; i > 0; i--)
                    {
                        result.ChartData[index] = new object[]
                        {
                        ConstantHelpers.PERFORMANCE_EVALUATION.RATING_SCALE.GET_NAME(result.Questions[0].Max ,(byte)i),
                        respones.Where(x=>x.Value == i).Sum(x=>x.Count)
                        };
                        index++;
                    }
                }
            }

            ///
            var sumResponses = await queryResponses.SumAsync(x => x.Value);
            var totalResponded = users.Count();
            result.Qualification = GetFinalGradePerformanceEvaluation(sumResponses, templateInformation.maximumRaiting, totalResponded, true);

            for (var i = 0; i < rubrics.Count; i++)
            {
                if (result.Qualification <= rubrics[i].max)
                {
                    result.Description = rubrics[i].description;
                }
            }

            if (scales.Any() && scales.Count() == template.Max)
            {
                result.RaitingScales = scales
                    .Select(x => new RaitingScale
                    {
                        Value = x.Value,
                        Description = x.Description
                    })
                    .OrderByDescending(x => x.Value)
                    .ToList();
            }


            return result;
        }

        public async Task<List<PerformationEvaluationStudentTemplate>> GetStudentComplianceReportDatatableClientSide(Guid evaluationId)
        {
            var evaluation = await _context.PerformanceEvaluations.AsNoTracking().Where(x => x.Id == evaluationId).FirstAsync();

            var resultTpm = await _context.Careers
                .Select(x => new PerformationEvaluationStudentTemplate
                {
                    CareerId = x.Id,
                    Career = x.Name,
                    Code = x.Code
                })
                .ToListAsync();

            var result = new List<PerformationEvaluationStudentTemplate>();

            var template = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluation.Id && x.PerformanceEvaluationTemplate.Role.Name == ConstantHelpers.ROLES.STUDENTS).Select(x => x.PerformanceEvaluationTemplate).FirstOrDefaultAsync();

            var evaluationUsers = await _context.PerformanceEvaluationUsers
                .Where(x =>
                x.PerformanceEvaluationId == evaluation.Id &&
                x.PerformanceEvaluationTemplateId == template.Id &&
                x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS)
                .Select(x => new { x.FromUserId, x.ToTeacherId, careerId = x.Section.CourseTerm.Course.CareerId }).ToArrayAsync();

            foreach (var item in resultTpm)
            {
                var studentSections = await _context.StudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == item.CareerId && x.Section.CourseTerm.TermId == evaluation.TermId).Select(x => x.Section.TeacherSections.Count()).ToListAsync();
                var evaluated = evaluationUsers.Where(x => x.careerId == item.CareerId).Count();
                item.Programmed = studentSections.Sum();
                item.Evaluated = evaluated;

                if (item.Programmed != 0)
                    result.Add(item);
            }

            return result.OrderBy(x => x.Code).ToList();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentCompliancecDetailedReportDatatable(DataTablesStructs.SentParameters parameters, Guid evaluationId, Guid? careerId, string searchValue)
        {
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId).FirstOrDefaultAsync();

            var query = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == evaluation.TermId))
                .OrderBy(x => x.Career.Name).ThenBy(x => x.User.FullName)
                .AsNoTracking();

            query = query
                .Where(x => x.StudentSections.Where(y => y.Section.CourseTerm.TermId == evaluation.TermId).SelectMany(y => y.Section.TeacherSections).Count() > x.User.PerformanceEvaluationUsers.Where(y => y.PerformanceEvaluationId == evaluationId).Count());

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                query = query.Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();

            var result = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    username = x.User.UserName,
                    fullName = x.User.FullName,
                    career = x.Career.Name,
                    total = x.StudentSections.Where(y => y.Section.CourseTerm.TermId == evaluation.TermId).SelectMany(y => y.Section.TeacherSections).Count(),
                    answered = x.User.PerformanceEvaluationUsers.Where(y => y.PerformanceEvaluationId == evaluationId).Count(),
                    pendings = x.StudentSections.Where(y => y.Section.CourseTerm.TermId == evaluation.TermId).SelectMany(y => y.Section.TeacherSections).Count() - x.User.PerformanceEvaluationUsers.Where(y => y.PerformanceEvaluationId == evaluationId).Count()
                }).ToListAsync();

            int recordsTotal = result.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };

        }

        public async Task<StudentDebtResultTemplate> GenerateStudentDebts(Guid evaluationId)
        {
            var result = new StudentDebtResultTemplate();

            var confiConcept = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.CONCEPT_PERFORMANCE_EVALUATION).FirstOrDefaultAsync();

            if (confiConcept is null)
            {
                result.Message = "No se configuró el concepto de mora para la evaluación de desempeño docente.";
                return result;
            }

            if (Guid.Parse(confiConcept.Value) == Guid.Empty)
            {
                result.Message = "No se configuró el concepto de mora para la evaluación de desempeño docente.";
                return result;
            }

            var concept = await _context.Concepts.Where(x => x.Id == Guid.Parse(confiConcept.Value)).FirstOrDefaultAsync();

            if (concept is null)
            {
                result.Message = "No se se encontró el concepto asignado para la deuda.";
                return result;
            }

            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId).FirstOrDefaultAsync();

            var paymentsToDelete = await _context.Payments.Where(x => x.Type == ConstantHelpers.PAYMENT.TYPES.TEACHER_PERFORMANCE_EVALUATION && x.EntityId == evaluation.Id && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING).ToListAsync();

            if (paymentsToDelete.Any())
            {
                if (ConstantHelpers.GENERAL.DATABASES.DATABASE == ConstantHelpers.DATABASES.SQL)
                {
                    await _context.Payments.Where(x => paymentsToDelete.Select(y => y.Id).ToList().Contains(x.Id)).BatchDeleteAsync();
                }
                else
                {
                    _context.Payments.RemoveRange(paymentsToDelete);
                    await _context.SaveChangesAsync();
                }
            }

            var paymentsMade = await _context.Payments
                .Where(x => x.Type == ConstantHelpers.PAYMENT.TYPES.TEACHER_PERFORMANCE_EVALUATION && x.EntityId == evaluation.Id && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .Select(x => x.UserId).ToListAsync();


            var query = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == evaluation.TermId))
                .OrderBy(x => x.Career.Name).ThenBy(x => x.User.FullName)
                .AsNoTracking();

            query = query
                .Where(x => x.StudentSections.Where(y => y.Section.CourseTerm.TermId == evaluation.TermId).SelectMany(y => y.Section.TeacherSections).Count() > x.User.PerformanceEvaluationUsers.Where(y => y.PerformanceEvaluationId == evaluationId).Count());

            var students = await query
                .Select(x => new
                {
                    x.Id,
                    x.UserId
                })
                .ToListAsync();


            students = students.Where(x => !paymentsMade.Contains(x.UserId)).ToList();

            var payments = new List<Payment>();

            var total = concept.Amount;
            var subtotal = total;
            var igv = 0.00M;

            if (concept.IsTaxed)
            {
                subtotal = total / (1.00M + ConstantHelpers.Treasury.IGV);
                igv = total - subtotal;
            }

            foreach (var student in students)
            {
                payments.Add(new Payment
                {
                    Description = concept.Description,
                    SubTotal = subtotal,
                    IgvAmount = igv,
                    Discount = 0.00M,
                    Total = total,
                    EntityId = evaluation.Id,
                    Type = ConstantHelpers.PAYMENT.TYPES.TEACHER_PERFORMANCE_EVALUATION,
                    UserId = student.UserId,
                    ConceptId = concept.Id,
                    TermId = evaluation.TermId,
                    CurrentAccountId = concept.CurrentAccountId
                });
            }

            await _context.Payments.AddRangeAsync(payments);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Deudas asignadas con éxito.";
            return result;
        }

        public async Task<List<ComplianceDetailedReport>> GetStudentCompliancecDetailedReport(Guid evaluationId, Guid? careerId, string searchValue)
        {
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId).FirstOrDefaultAsync();

            var query = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == evaluation.TermId))
                .OrderBy(x => x.Career.Name).ThenBy(x => x.User.FullName)
                .AsNoTracking();

            query = query
                .Where(x => x.StudentSections.Where(y => y.Section.CourseTerm.TermId == evaluation.TermId).SelectMany(y => y.Section.TeacherSections).Count() > x.User.PerformanceEvaluationUsers.Where(y => y.PerformanceEvaluationId == evaluationId).Count());

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                query = query.Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue));
            }

            var result = await query
                .Select(x => new ComplianceDetailedReport
                {
                    Id = x.Id,
                    Username = x.User.UserName,
                    Fullname = x.User.FullName,
                    PhoneNumber = x.User.PhoneNumber,
                    Email = x.User.Email,
                    Curriculum = x.Curriculum.Code,
                    Career = x.Career.Name,
                    Total = x.StudentSections.Where(y => y.Section.CourseTerm.TermId == evaluation.TermId).SelectMany(y => y.Section.TeacherSections).Count(),
                    Answered = x.User.PerformanceEvaluationUsers.Where(y => y.PerformanceEvaluationId == evaluationId).Count(),
                    Pending = x.StudentSections.Where(y => y.Section.CourseTerm.TermId == evaluation.TermId).SelectMany(y => y.Section.TeacherSections).Count() - x.User.PerformanceEvaluationUsers.Where(y => y.PerformanceEvaluationId == evaluationId).Count()
                }).ToListAsync();

            return result;
        }

        public async Task<List<PerformanceEvaluationAuthoritiesTemplate>> GetAuthoritiesComplianceReportDatatableClientSide(Guid evaluationId)
        {
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId).FirstOrDefaultAsync();
            var result = new List<PerformanceEvaluationAuthoritiesTemplate>();

            var faculties = await _context.Faculties
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();

            var templates = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluationId && x.PerformanceEvaluationTemplate.Role.Name != ConstantHelpers.ROLES.STUDENTS).Select(x => x.PerformanceEvaluationTemplateId).ToListAsync();

            var evaluationUsers = await _context.PerformanceEvaluationUsers
                .Where(x =>
                x.PerformanceEvaluationId == evaluation.Id &&
                templates.Contains(x.PerformanceEvaluationTemplateId) &&
                x.FromRole.Name != ConstantHelpers.ROLES.STUDENTS)
                .Select(x => new { x.FromUserId, x.ToTeacherId, fromRoleName = x.FromRole.Name }).ToArrayAsync();

            foreach (var faculty in faculties)
            {
                var careers = await _context.Careers.Where(x => x.FacultyId == faculty.Id).ToArrayAsync();
                var deanFaculty = await _context.Faculties.Where(x => x.Id == faculty.Id)
                    .Select(x => new
                    {
                        x.Dean.FullName,
                        Id = x.DeanId,
                    }).FirstOrDefaultAsync();

                var teachers = await _context.Teachers.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == evaluation.TermId && x.AcademicDepartment.Career.FacultyId == faculty.Id))
                    .Select(x => new
                    {
                        x.UserId,
                        x.AcademicDepartment.CareerId,
                        x.AcademicDepartmentId
                    })
                    .ToListAsync();

                if (deanFaculty != null)
                {
                    var template = new PerformanceEvaluationAuthoritiesTemplate
                    {
                        Faculty = faculty.Name,
                        Position = "Decano de Facultad",
                        Programmed = teachers.Where(x => x.UserId != deanFaculty.Id).Count(),
                        FullName = deanFaculty?.FullName ?? "",
                        Evaluated = evaluationUsers.Where(x => x.FromUserId == deanFaculty.Id && x.fromRoleName == ConstantHelpers.ROLES.DEAN).Count()
                    };

                    result.Add(template);
                }

                var academicDepartmentDirectors = await _context.AcademicDepartments.Where(x => x.FacultyId == faculty.Id && !string.IsNullOrEmpty(x.AcademicDepartmentDirectorId))
                    .Select(x => new
                    {
                        director = x.AcademicDepartmentDirector,
                        academicDepartmentId = x.Id
                    })
                    .ToListAsync();

                if (academicDepartmentDirectors != null && academicDepartmentDirectors.Any())
                {
                    foreach (var academicDepartmentDirector in academicDepartmentDirectors)
                    {
                        var template = new PerformanceEvaluationAuthoritiesTemplate
                        {
                            Faculty = faculty.Name,
                            Position = $"Director de Departamento Académico ({academicDepartmentDirector.director.FullName})",
                            Programmed = teachers.Where(x => x.UserId != academicDepartmentDirector.director.Id && x.AcademicDepartmentId == academicDepartmentDirector.academicDepartmentId).Count(),
                            FullName = academicDepartmentDirector.director.FullName,
                            Evaluated = evaluationUsers.Where(x => x.FromUserId == academicDepartmentDirector.director.Id && x.fromRoleName == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Count()
                        };

                        result.Add(template);
                    }
                }

                foreach (var career in careers)
                {
                    var teachersByCareer = teachers.Where(x => x.CareerId == career.Id).ToList();
                    var evalutionByCareers = evaluationUsers.Where(x => teachersByCareer.Any(y => y.UserId == x.ToTeacherId)).ToList();

                    var tutoringCoordinatorCareer = await _context.TutoringCoordinators.Where(x => x.CareerId == career.Id).Select(x => x.UserId).FirstOrDefaultAsync();
                    var careerDirector = await _context.Users.Where(x => x.Id == career.CareerDirectorId).FirstOrDefaultAsync();
                    var tutorings = await _context.Users.Where(x => tutoringCoordinatorCareer.Contains(x.Id)).ToListAsync();

                    if (careerDirector != null)
                    {
                        var template = new PerformanceEvaluationAuthoritiesTemplate
                        {
                            Faculty = faculty.Name,
                            Position = $"Director de Escuela ({career.Name})",
                            Programmed = teachers.Where(x => x.CareerId == career.Id && x.UserId != careerDirector.Id).Count(),
                            FullName = careerDirector.FullName,
                            Evaluated = evalutionByCareers.Where(x => x.FromUserId == careerDirector.Id && x.fromRoleName == ConstantHelpers.ROLES.CAREER_DIRECTOR).Count()
                        };

                        result.Add(template);
                    }

                    foreach (var tutoring in tutorings)
                    {
                        var template = new PerformanceEvaluationAuthoritiesTemplate
                        {
                            Faculty = faculty.Name,
                            Position = $"Coordinador de Tutoria ({career.Name})",
                            Programmed = teachers.Where(x => x.CareerId == career.Id && x.UserId != tutoring.Id).Count(),
                            FullName = tutoring.FullName,
                            Evaluated = evalutionByCareers.Where(x => x.FromUserId == tutoring.Id && x.fromRoleName == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Count()
                        };

                        result.Add(template);
                    }

                    var socialResponsabiliy = await _context.Users.Where(x => x.Id == career.SocialResponsabilityCoordinatorId).FirstOrDefaultAsync();

                    if (socialResponsabiliy != null)
                    {
                        var template = new PerformanceEvaluationAuthoritiesTemplate
                        {
                            Faculty = faculty.Name,
                            Position = $"Coordinador de responsabilidad social ({career.Name})",
                            Programmed = teachers.Where(x => x.CareerId == career.Id && x.UserId != socialResponsabiliy.Id).Count(),
                            FullName = socialResponsabiliy.FullName,
                            Evaluated = evalutionByCareers.Where(x => x.FromUserId == socialResponsabiliy.Id && x.fromRoleName == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Count()
                        };

                        result.Add(template);
                    }

                    var researchCoordinator = await _context.Users.Where(x => x.Id == career.ResearchCoordinatorId).FirstOrDefaultAsync();

                    if (researchCoordinator != null)
                    {
                        var template = new PerformanceEvaluationAuthoritiesTemplate
                        {
                            Faculty = faculty.Name,
                            Position = $"Coordinador de investigación ({career.Name})",
                            Programmed = teachers.Where(x => x.CareerId == career.Id && x.UserId != researchCoordinator.Id).Count(),
                            FullName = researchCoordinator.FullName,
                            Evaluated = evalutionByCareers.Where(x => x.FromUserId == researchCoordinator.Id && x.fromRoleName == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Count()
                        };

                        result.Add(template);
                    }
                }
            }

            return result.OrderBy(x => x.Faculty).ToList();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid evaluationId, Guid academicDepartmentId, Guid? curriculumId, string searchValue)
        {
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId).FirstOrDefaultAsync();

            var evaluationUsers = await _context.PerformanceEvaluationUsers
                .Where(x =>
                x.PerformanceEvaluationId == evaluation.Id &&
                x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS)
                .Select(x => x.SectionId).ToListAsync();

            var academicDepartment = await _context.AcademicDepartments.Where(x => x.Id == academicDepartmentId).FirstOrDefaultAsync();

            var query = _context.Sections.Where(x => x.CourseTerm.TermId == evaluation.TermId).AsNoTracking();

            if (academicDepartment is null)
            {
                query = query.Where(x => x.TeacherSections.Any(y => !y.Teacher.AcademicDepartmentId.HasValue));
            }
            else
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId == academicDepartment.Id));
            }

            if (curriculumId.HasValue)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.CourseTerm.Course.Name.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            var recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .OrderBy(x => x.CourseTerm.CourseId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.CourseTerm.Course.Name,
                    x.Code,
                    teachersCount = x.TeacherSections.Count(),
                    studentsCount = x.StudentSections.Count()
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    surveys = evaluationUsers.Where(y => y == x.Id).Count(),
                    total = x.teachersCount * x.studentsCount,
                    remaining = (x.teachersCount * x.studentsCount) - evaluationUsers.Where(y => y == x.Id).Count()
                })
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<StatisticalReportCareerTeacherPerformanceTemplate> GetStatisticalReportCareerTeacherPerformance(Guid evaluationId, ClaimsPrincipal user)
        {
            var evaluation = await _context.PerformanceEvaluations.Include(x => x.Term).Where(x => x.Id == evaluationId).FirstOrDefaultAsync();

            var template = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluation.Id && x.PerformanceEvaluationTemplate.Role.Name == ConstantHelpers.ROLES.STUDENTS)
                .Select(x => new
                {
                    Id = x.PerformanceEvaluationTemplateId,
                    x.PerformanceEvaluationTemplate.Max,
                    MaximumRaiting = x.PerformanceEvaluationTemplate.Questions.Count() * x.PerformanceEvaluationTemplate.Max
                })
                .FirstOrDefaultAsync();

            //var template = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluation.Id && x.PerformanceEvaluationTemplate.Role.Name == ConstantHelpers.ROLES.STUDENTS).Select(x => x.PerformanceEvaluationTemplate).FirstOrDefaultAsync();

            var questions = await _context.PerformanceEvaluationQuestions.Where(x => x.PerformanceEvaluationTemplateId == template.Id).Include(X => X.PerformanceEvaluationCriterion).ToListAsync();

            var model = new StatisticalReportCareerTeacherPerformanceTemplate
            {
                Code = evaluation.Code,
                EndDate = evaluation.EndDate.ToString(ConstantHelpers.FORMATS.DATE),
                StartDate = evaluation.StartDate.ToString(ConstantHelpers.FORMATS.DATE),
                Max = template.Max,
                Term = evaluation.Term.Name,
                Questions = questions.Select(x => new StatisticalReportCareerTeacherPerformanceQuestionTemplate
                {
                    CriterionId = x.PerformanceEvaluationCriterionId,
                    Criterion = x.PerformanceEvaluationCriterion?.Name,
                    Description = x.Description,
                    Id = x.Id
                }).OrderBy(x => x.CriterionId).ToList(),
                Details = new List<StatisticalReportCareerTeacherPerformanceDetailTemplate>()
            };

            var query = _context.AcademicDepartments.AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                query = query.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId);
            }

            var academicDepartments = await query
                .Select(x => new AcademicDepartmentTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Faculty = x.Faculty.Name
                })
                .OrderByDescending(x => x.Name).ToListAsync();

            var performanceEvaluationUsers = await _context.PerformanceEvaluationUsers
                .Where(x => x.PerformanceEvaluationId == evaluation.Id && x.PerformanceEvaluationTemplateId == template.Id)
                .Select(x => new
                {
                    x.ToTeacherId,
                    x.ToTeacher.AcademicDepartmentId,
                    x.FromUserId,
                    responses = x.Responses.Select(y => new
                    {
                        y.PerformanceEvaluationQuestionId,
                        y.PerformanceEvaluationQuestion.PerformanceEvaluationCriterionId,
                        criterion = y.PerformanceEvaluationQuestion.PerformanceEvaluationCriterion.Name,
                        y.Value
                    })
                    .ToList(),
                    SumResponses = x.Responses.Sum(y => y.Value)
                })
                .ToListAsync();

            var criterions = await _context.PerformanceEvaluationCriterions.Where(x => x.PerformanceEvaluationTemplateId == template.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    questions = x.PerformanceEvaluationQuestions.Count()
                })
                .ToListAsync();

            model.Criterions = criterions.Select(x => new StatisticalReportCareerTeacherPerformanceCriterionTemplate
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            foreach (var item in academicDepartments)
            {

                var performanceEvaluationUsersByAcademicDeparment = performanceEvaluationUsers.Where(x => x.AcademicDepartmentId == item.Id).ToList();
                var sumResponses = performanceEvaluationUsersByAcademicDeparment.Sum(x => x.SumResponses);
                var realFinalGrade = Math.Round(GetFinalGradePerformanceEvaluation(sumResponses, template.MaximumRaiting, performanceEvaluationUsersByAcademicDeparment.Count(), false), 2, MidpointRounding.AwayFromZero);
                var finalGrade = Math.Round(GetFinalGradePerformanceEvaluation(sumResponses, template.MaximumRaiting, performanceEvaluationUsersByAcademicDeparment.Count(), true), 2, MidpointRounding.AwayFromZero);
                var finalGradePercentage = ConvertToPercentage(finalGrade, 20);

                var responsesByAcademicDepartment = performanceEvaluationUsersByAcademicDeparment.SelectMany(x => x.responses).ToList();
                var responses = responsesByAcademicDepartment
                    .GroupBy(x => new { x.PerformanceEvaluationQuestionId, x.Value })
                    .Select(x => new StatisticalReportCareerTeacherPerformanceResponseTemplate
                    {
                        QuestionId = x.Key.PerformanceEvaluationQuestionId,
                        Value = x.Key.Value,
                        Quantity = x.Count()
                    })
                    .ToList();

                var detail = new StatisticalReportCareerTeacherPerformanceDetailTemplate
                {
                    AcademicDepartment = item.Name,
                    Faculty = item.Faculty,
                    Responses = responses,
                    Qualification = finalGrade,
                    QualificationPercentage = finalGradePercentage,
                    RealQualification = realFinalGrade
                };

                if (criterions != null && criterions.Any())
                {
                    detail.CriterionDetails = responsesByAcademicDepartment.GroupBy(x => x.PerformanceEvaluationCriterionId)
                    .Select(x => new StatisticalReportCareerTeacherPerformanceCriterionTemplate
                    {
                        Id = x.Key.Value,
                        Name = x.Select(y => y.criterion).FirstOrDefault(),
                        Qualification = Math.Round(GetFinalGradePerformanceEvaluation(x.Sum(y => y.Value), template.Max * criterions.Where(y => y.Id == x.Key.Value).Select(y => y.questions).FirstOrDefault(), performanceEvaluationUsersByAcademicDeparment.Count(), true), 2, MidpointRounding.AwayFromZero),
                        QualificationPercentage = ConvertToPercentage(Math.Round(GetFinalGradePerformanceEvaluation(x.Sum(y => y.Value), template.Max * criterions.Where(y => y.Id == x.Key.Value).Select(y => y.questions).FirstOrDefault(), performanceEvaluationUsersByAcademicDeparment.Count(), true), 2, MidpointRounding.AwayFromZero), 20)
                    })
                    .ToList();
                }

                model.Details.Add(detail);
            }

            return model;
        }

        public async Task<PerformanceEvaluationUserDetail> GetResultBySection(Guid evaluationId, Guid sectionId, string toUserId)
        {
            PerformanceEvaluationUserDetail result = new PerformanceEvaluationUserDetail()
            {
                Roles = new List<Role>(),
                Total = 0,

            };

            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId).FirstOrDefaultAsync();
            var template = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluation.Id && x.PerformanceEvaluationTemplate.Role.Name == ConstantHelpers.ROLES.STUDENTS).Select(x => x.PerformanceEvaluationTemplate).FirstOrDefaultAsync();

            var users = await _context.PerformanceEvaluationUsers.Where(x => x.PerformanceEvaluationId == evaluation.Id && x.PerformanceEvaluationTemplateId == template.Id && x.SectionId == sectionId && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS && x.ToTeacherId == toUserId).ToListAsync();
            var responses = await _context.PerformanceEvaluationResponses.Where(x => x.PerformanceEvaluationUser.PerformanceEvaluationId == evaluationId && x.PerformanceEvaluationQuestion.PerformanceEvaluationTemplateId == template.Id && x.PerformanceEvaluationUser.SectionId == sectionId && x.PerformanceEvaluationUser.FromRole.Name == ConstantHelpers.ROLES.STUDENTS && x.PerformanceEvaluationUser.ToTeacherId == toUserId).ToListAsync();

            result.Questions = await _context.PerformanceEvaluationQuestions.Where(x => x.PerformanceEvaluationTemplateId == template.Id)
                .Select(x => new Question
                {
                    Id = x.Id,
                    Description = x.Description,
                    Max = x.PerformanceEvaluationTemplate.Max
                })
                .ToListAsync();

            for (int i = 0; i < result.Questions.Count; i++)
            {
                result.Questions[i].Responses = new List<Response>();

                for (int j = 1; j <= result.Questions[i].Max; j++)
                {
                    result.Questions[i].Responses.Add(new Response
                    {
                        Value = j,
                        Count = responses.Where(x => x.PerformanceEvaluationQuestionId == result.Questions[i].Id && x.Value == j).Count()
                    });
                }
            }

            var maxPoints = result.Questions.Count() * template.Max;
            result.Qualification = users.Any() ? Math.Round((responses.Sum(x => x.Value) * 20M) / ((decimal)maxPoints * users.Count()), 2, MidpointRounding.AwayFromZero) : 0M;

            var rubrics = await _context.PerformanceEvaluationRubrics.Where(x => x.PerformanceEvaluationId == evaluation.Id).OrderByDescending(x => x.Max).Select(x => new
            {
                max = x.Max,
                description = x.Description
            }).ToListAsync();

            var usersCount = users.Count() == 0 ? 1 : users.Count();
            result.Total = Math.Round((users.Sum(x => x.Value) / usersCount), 2, MidpointRounding.AwayFromZero);
            result.EvaluatedStudents = users.Count();

            for (var i = 0; i < rubrics.Count; i++)
            {
                if (result.Total <= rubrics[i].max)
                {
                    result.Description = rubrics[i].description;
                }
            }
            return result;
        }

        public async Task<List<PerformanceEvaluationCriteronTemplate>> GetTeacherReportPerformanceEvaluationCriterion(Guid? academicDepartmentId, Guid evaluationId)
        {
            if (academicDepartmentId == Guid.Empty)
                academicDepartmentId = null;

            var template = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluationId & x.PerformanceEvaluationTemplate.Role.Name == ConstantHelpers.ROLES.STUDENTS)
                .Select(x => new
                {
                    evaluation = x.PerformanceEvaluation.Name,
                    x.PerformanceEvaluationId,
                    id = x.PerformanceEvaluationTemplateId,
                    max = x.PerformanceEvaluationTemplate.Max,
                    x.PerformanceEvaluationTemplate.RoleId,
                    x.PerformanceEvaluation.PercentageRaitingScale
                })
                .FirstOrDefaultAsync();

            var rubrics = await _context.PerformanceEvaluationRubrics.Where(x => x.PerformanceEvaluationId == template.PerformanceEvaluationId).ToListAsync();

            var criterions = await _context.PerformanceEvaluationCriterions.Where(x => x.PerformanceEvaluationTemplateId == template.id)
               .Select(x => new
               {
                   x.Id,
                   x.Name,
                   x.Description,
                   questions = x.PerformanceEvaluationQuestions.Count()
               })
               .ToListAsync();

            var totalQuestions = criterions.Sum(x => x.questions);

            var responses = await _context.PerformanceEvaluationResponses
                .Where(x =>
                x.PerformanceEvaluationUser.PerformanceEvaluationId == template.PerformanceEvaluationId &&
                x.PerformanceEvaluationUser.ToTeacher.AcademicDepartmentId == academicDepartmentId &&
                x.PerformanceEvaluationUser.FromRole.Name == ConstantHelpers.ROLES.STUDENTS
                )
                .Select(x => new
                {
                    x.Value,
                    x.PerformanceEvaluationQuestion.PerformanceEvaluationCriterionId,
                    x.PerformanceEvaluationUser.FromUserId,
                    x.PerformanceEvaluationUser.ToTeacherId,
                    x.PerformanceEvaluationUserId
                })
                .ToListAsync();

            var teachers = await _context.Teachers.Where(x => x.AcademicDepartmentId == academicDepartmentId && x.PerformanceEvaluationUsers.Any(y => y.PerformanceEvaluationId == evaluationId))
                .Select(x => new
                {
                    x.UserId,
                    x.User.FullName
                })
                .ToListAsync();

            var result = new List<PerformanceEvaluationCriteronTemplate>();

            foreach (var teacher in teachers)
            {
                var users = responses.Where(x => x.ToTeacherId == teacher.UserId).GroupBy(x => x.PerformanceEvaluationUserId).Count();

                var criterionsObj = new object[criterions.Count()];

                var answerByTeacher = responses.Where(x => x.ToTeacherId == teacher.UserId).ToList();

                var groupByCriterion = answerByTeacher.GroupBy(x => x.PerformanceEvaluationCriterionId)
                    .Select(x => new
                    {
                        x.Key,
                        average = Math.Round(GetFinalGradePerformanceEvaluation(x.Sum(y => y.Value), criterions.Where(y => y.Id == x.Key).Select(y => y.questions).FirstOrDefault() * template.max, users), 2, MidpointRounding.AwayFromZero)
                    })
                    .ToList();

                if (template.PercentageRaitingScale)
                {
                    for (int i = 0; i < criterions.Count(); i++)
                    {
                        criterionsObj[i] = new object[]
                        {
                            criterions[i].Name,
                            ConvertToPercentage(groupByCriterion.Where(x=>x.Key == criterions[i].Id).Select(x=>x.average).FirstOrDefault(),20)
                        };
                    }
                }
                else
                {
                    for (int i = 0; i < criterions.Count(); i++)
                    {
                        criterionsObj[i] = new object[]
                        {
                            criterions[i].Name,
                            groupByCriterion.Where(y=>y.Key == criterions[i].Id).Select(x=>x.average).FirstOrDefault()
                        };
                    }
                }

                var maxRubricValue = !rubrics.Any() ? 0 : rubrics.Max(x => x.Max);

                var model = new PerformanceEvaluationCriteronTemplate
                {
                    RaitingPercentage = Math.Round((users != 0 ? GetFinalGradePerformanceEvaluation(answerByTeacher.Sum(y => y.Value), totalQuestions * template.max, users) : 0m) * 100 / 20M, 2, MidpointRounding.AwayFromZero),
                    Raiting = Math.Round((users != 0 ? GetFinalGradePerformanceEvaluation(answerByTeacher.Sum(y => y.Value), totalQuestions * template.max, users) : 0m)),
                    RaitingStr = GetRaitingStrByRubrics(rubrics, GetFinalGradePerformanceEvaluation(answerByTeacher.Sum(y => y.Value), totalQuestions * template.max, users), template.PercentageRaitingScale),
                    EvaluationId = template.PerformanceEvaluationId,
                    Evaluation = template.evaluation,
                    TeacherId = teacher.UserId,
                    FullName = teacher.FullName,
                    PercentageRaitingScale = template.PercentageRaitingScale,
                    TeacherRaitingDescription = GetTeacherRaitingDescriptionByRubrics(rubrics, GetFinalGradePerformanceEvaluation(answerByTeacher.Sum(y => y.Value), totalQuestions * template.max, users), template.PercentageRaitingScale),
                    ChartDataJson = criterionsObj,
                    Rubrics = rubrics.Select(x => new PeformanceEvaluationRubricTemplate
                    {
                        Description = x.Description,
                        Max = template.PercentageRaitingScale ? ConvertToPercentage(x.Max, maxRubricValue) : x.Max,
                        Min = template.PercentageRaitingScale ? ConvertToPercentage(x.Min, maxRubricValue) : x.Min
                    }).ToList(),
                    Criterions = criterions.Select(x => new CriterionTemplate
                    {
                        Name = x.Name,
                        Description = x.Description,
                        Raiting = groupByCriterion.Where(y => y.Key == x.Id).Select(x => x.average).FirstOrDefault(),
                        RaitingStr = GetRaitingStrByRubrics(rubrics, groupByCriterion.Where(y => y.Key == x.Id).Select(x => x.average).FirstOrDefault(), template.PercentageRaitingScale)
                    }).ToList()
                };

                result.Add(model);
            }

            return result;
        }

        public async Task<List<PerformanceEvaluationUserReportTemplate>> GetReportConsolidatedByAcademicDepartment(Guid evaluationId)
        {
            var result = new List<PerformanceEvaluationUserReportTemplate>();

            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId)
                .Select(x => new
                {
                    x.Name,
                    x.Id,
                    x.Code,
                    Term = x.Term.Name
                })
                .FirstOrDefaultAsync();

            var academicDeparments = await _context.AcademicDepartments
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .ToListAsync();

            var performanceEvaluationUsers = await _context.PerformanceEvaluationUsers.Where(x => x.PerformanceEvaluationId == evaluationId && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS)
                .Select(x => new
                {
                    x.ToTeacherId,
                    x.Id,
                    x.ToTeacher.AcademicDepartmentId,
                    x.Value,
                    responses = x.Responses.ToList()
                })
                .ToListAsync();

            foreach (var item in academicDeparments)
            {
                var surveysByAcademicDepartment = performanceEvaluationUsers.Where(x => x.AcademicDepartmentId == item.Id).ToList();

                if (surveysByAcademicDepartment.Any())
                {
                    var toAdd = new PerformanceEvaluationUserReportTemplate
                    {
                        AcademicDepartment = item.Name,
                        Term = evaluation.Term,
                        TeachersSurveyed = surveysByAcademicDepartment.GroupBy(x => x.ToTeacherId).Count(),
                    };

                    toAdd.Average = Math.Round(surveysByAcademicDepartment.Average(x => x.Value), 2, MidpointRounding.AwayFromZero);
                    result.Add(toAdd);
                }
            }

            return result;
        }

        public async Task<List<Question>> GetReportConsolidatedByQuestion(Guid evaluationId)
        {
            var result = new List<Question>();

            var template = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluationId)
                .Select(x => new
                {
                    Id = x.PerformanceEvaluationTemplateId,
                    Questions = x.PerformanceEvaluationTemplate
                    .Questions.OrderBy(y => y.CreatedAt).Select(y => new
                    {
                        y.Id,
                        y.Description
                    }).ToList()
                })
                .FirstOrDefaultAsync();


            var performanceEvaluationResponses = await _context.PerformanceEvaluationResponses.
                Where(x => x.PerformanceEvaluationUser.PerformanceEvaluationId == evaluationId && x.PerformanceEvaluationUser.FromRole.Name == ConstantHelpers.ROLES.STUDENTS && x.PerformanceEvaluationUser.PerformanceEvaluationTemplateId == template.Id)
                .Select(x => new
                {
                    x.Value,
                    x.PerformanceEvaluationQuestionId
                })
                .ToListAsync();

            foreach (var item in template.Questions)
            {
                var respones = performanceEvaluationResponses.Where(x => x.PerformanceEvaluationQuestionId == item.Id).ToList();

                result.Add(new Question
                {
                    Description = item.Description,
                    Average = respones.Any() ? (decimal)Math.Round(respones.Average(x => x.Value), 2, MidpointRounding.AwayFromZero) : 0M
                });
            }

            return result;
        }

        private decimal ConvertToPercentage(decimal value, decimal max)
        {
            if (max == 0)
                return 0;

            return Math.Round((value * 100M / max), 2, MidpointRounding.AwayFromZero);
        }


        private decimal GetFinalGradePerformanceEvaluation(int sumResponses, decimal maximumRaiting, int count, bool vigesimalScale = true)
        {
            if (count == 0)
                return 0M;

            if (vigesimalScale)
            {
                return ((sumResponses * 20M) / (maximumRaiting * count));
            }
            else
            {
                return sumResponses / (count * 1M);
            }
        }

        private string GetRaitingStrByRubrics(List<PerformanceEvaluationRubric> rubrics, decimal result, bool percentageRaitingScale)
        {

            if (percentageRaitingScale)
                result = ConvertToPercentage(result, 20);

            for (var i = 0; i < rubrics.Count; i++)
            {
                if (result <= rubrics[i].Max)
                {
                    return rubrics[i].Description;
                }
            }


            return string.Empty;
        }

        private string GetTeacherRaitingDescriptionByRubrics(List<PerformanceEvaluationRubric> rubrics, decimal result, bool percentageRaitingScale)
        {

            if (percentageRaitingScale)
                result = ConvertToPercentage(result, 20);

            for (var i = 0; i < rubrics.Count; i++)
            {
                if (result <= rubrics[i].Max)
                {
                    return rubrics[i].TeacherRaitingDescription;
                }
            }

            return string.Empty;
        }
    }
}
