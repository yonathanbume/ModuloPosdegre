using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Moodle;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AkdemicContext _context;
        private readonly DbSet<T> _entities;

        public Repository(AkdemicContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<bool> Any(Guid id)
        {
            var entity = await _entities.FindAsync(id);
            return entity != null;
        }

        public async Task<bool> Any(string id)
        {
            var entity = await _entities.FindAsync(id);
            return entity != null;
        }

        public async Task<int> Count() => await _entities.CountAsync();

        public async Task<T> First() => await _entities.FirstOrDefaultAsync();

        public async Task<T> Last() => await _entities.LastOrDefaultAsync();

        public virtual async Task<T> Get(Guid id) => await _entities.FindAsync(id);

        public virtual async Task<T> Get(string id) => await _entities.FindAsync(id);

        public virtual async Task<T> Get(params object[] keyValues) => await _entities.FindAsync(keyValues);

        public virtual async Task<IEnumerable<T>> GetAll() => await _entities.AsQueryable().ToListAsync();

        public async Task<T> Add(T entity)
        {
            var result = await _entities.AddAsync(entity);
            return result.Entity;
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public virtual async Task Delete(T entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteById(Guid id)
        {
            var entity = await this.Get(id);
            await Delete(entity);
        }

        public virtual async Task DeleteById(string id)
        {
            var entity = await this.Get(id);
            await Delete(entity);
        }

        public virtual async Task DeleteById(params object[] keyValues)
        {
            var entity = await this.Get(keyValues);
            await Delete(entity);
        }

        public async Task DeleteRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Insert(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task InsertRange(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Update(T entity)
        {
            if (entity == null)
            {
                // Should call Dispose() to remove the elements from the failed context?
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRange(IEnumerable<T> entities)
        {
            if (entities != null)
            {
                await _context.SaveChangesAsync();
            }
        }

        protected virtual async Task<string> GetConfigurationValue(string key)
        {
            var configuration = await _context.Configurations.FirstOrDefaultAsync(x => x.Key == key);

            if (configuration == null)
            {
                var value = CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.EconomicManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.EconomicManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.DocumentaryProcedureManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.DocumentaryProcedureManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.AdmissionManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.AdmissionManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.JobExchangeManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.JobExchangeManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.TransparencyPortalManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.TransparencyPortalManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.ScaleConfiguration.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.ScaleConfiguration.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.Tutoring.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.Tutoring.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.Server.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.Server.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.PreProfessionalPracticeConfiguraton.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.PreProfessionalPracticeConfiguraton.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.RecordFormat.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.RecordFormat.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.Email.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.Email.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.General.DEFAULT_VALUES[key];

                configuration = new Configuration
                {
                    Key = key,
                    Value = value
                };

                await _context.Configurations.AddAsync(configuration);
                await _context.SaveChangesAsync();
            }

            return configuration.Value;
        }

        protected async Task<int> GetIntConfigurationValue(string key)
        {
            var configuration = await _context.Configurations.FirstOrDefaultAsync(x => x.Key == key);

            if (configuration == null)
            {
                var value = CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.EconomicManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.EconomicManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.AdmissionManagement.DEFAULT_VALUES.ContainsKey(key) ? CORE.Helpers.ConstantHelpers.Configuration.AdmissionManagement.DEFAULT_VALUES[key]
                    : CORE.Helpers.ConstantHelpers.Configuration.General.DEFAULT_VALUES[key];

                configuration = new Configuration
                {
                    Key = key,
                    Value = value
                };

                await _context.Configurations.AddAsync(configuration);
                await _context.SaveChangesAsync();
            }

            return int.Parse(configuration.Value);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }

        protected List<Guid> GetCoordinatorCareers(string coordinatorId)
        {
            var careers = _context.Careers.Where(x => x.AcademicCoordinatorId == coordinatorId || x.AcademicSecretaryId == coordinatorId || x.CareerDirectorId == coordinatorId || x.AcademicDepartmentDirectorId == coordinatorId).Select(x => x.Id).ToList();
            return careers;
        }

        public async Task<decimal> CalculateStudentCredits(Guid studentId)
        {
            var credits = 0.0M;
            var student = await _context.Students.FindAsync(studentId);
            var enableLowGradeCredits = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENABLE_CREDITS_FOR_LOW_GRADE_STUDENTS));
            var lowGradeMaximumGrade = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.LOW_GRADE_STUDENTS_MAXIMUM_GRADE));
            var lowGradeCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.LOW_GRADE_STUDENTS_CREDITS));

            var extraCreditsModality = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.EXTRA_CREDITS_MODALITY));
            var unbeatenStudentCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENT_CREDITS));
            var unbeatenStudentCreditsByRange = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENTS_CREDITS_BY_RANGE));
            var extraCredisConfigurations = await _context.ExtraCreditConfigurations.ToListAsync();

            var academicSummary = await _context.AcademicSummaries
                  .Where(x => x.StudentId == student.Id && (x.Term.Number == "1" || x.Term.Number == "2"))
                  .OrderByDescending(x => x.Term.StartDate)
                  .FirstOrDefaultAsync();

            var creditsModality = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.TERM_CREDITS_MODALITY);
            var regularCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REGULAR_MAXIMUM_CREDITS));

            var creditsQry = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear && !x.IsElective)
                .AsNoTracking();

            if (student.AcademicProgramId.HasValue)
            {
                creditsQry = creditsQry.Where(x => !x.Course.AcademicProgramId.HasValue || (x.Course.AcademicProgramId.HasValue && (x.Course.AcademicProgramId == student.AcademicProgramId || x.Course.AcademicProgram.Code == "00")));
            }

            var yearCredits = await creditsQry.SumAsync(x => x.Course.Credits);

            credits = creditsModality == ConstantHelpers.Term.CreditsModality.MAXIMUM_CREDITS_BASED_ON_CURRICULUM ? yearCredits : regularCredits;

            if (creditsModality == ConstantHelpers.Term.CreditsModality.ACADEMIC_YEAR_CREDITS_CONFIGURATION)
            {
                var academicYearCredit = await _context.AcademicYearCredits
                                  .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear)
                                  .FirstOrDefaultAsync();
                if (academicYearCredit != null)
                    credits = academicYearCredit.Credits;
            }

            if (extraCreditsModality == ConstantHelpers.Configuration.Enrollment.ExtraCreditModality.UNBEATEN_STUDENTS)
            {
                if (student.Status == ConstantHelpers.Student.States.UNBEATEN || student.Status == ConstantHelpers.Student.States.HIGH_PERFORMANCE)
                {
                    if (unbeatenStudentCreditsByRange && academicSummary != null)
                    {
                        var extraCreditsConfiguration = extraCredisConfigurations.FirstOrDefault(x => x.AverageGradeStart <= academicSummary.WeightedAverageGrade && academicSummary.WeightedAverageGrade <= x.AverageGradeEnd);
                        if (extraCreditsConfiguration != null)
                            credits += extraCreditsConfiguration.Credits;
                    }
                    else
                        credits += unbeatenStudentCredits;
                }
            }
            else
            {
                var extraCreditsConfiguration = extraCredisConfigurations.FirstOrDefault(x => x.MeritType == student.CurrentMeritType);
                if (extraCreditsConfiguration != null)
                    credits += extraCreditsConfiguration.Credits;
            }

            if (enableLowGradeCredits && academicSummary != null)
            {
                if (academicSummary.WeightedAverageGrade <= lowGradeMaximumGrade)
                    credits = lowGradeCredits;
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAB)
            {
                var studentCurriculumCourses = await _context.AcademicYearCourses
                    .Where(x => x.CurriculumId == student.CurriculumId)
                    .Select(x => x.CourseId).ToListAsync();

                var academicHistories = await _context.AcademicHistories
                    .Where(x => x.StudentId == student.Id && !x.Withdraw)
                    .Select(x => new
                    {
                        x.CourseId,
                        x.Approved,
                        x.Try,
                        x.TermId
                    }).ToListAsync();

                var studentHistories = academicHistories
                    .Where(x => studentCurriculumCourses.Contains(x.CourseId))
                    .GroupBy(x => x.CourseId)
                    .Select(x => new
                    {
                        CourseId = x.Key,
                        Try = x.Max(y => y.Try),
                        Approved = x.Any(y => y.Approved)
                    }).ToList();

                if (studentHistories.Any(x => x.Try >= 2 && !x.Approved))
                    credits = 12;
            }

            return credits;
        }

        protected async Task CreateStudentEnrollmentPayments(Guid studentId, bool isRectification = false)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null) return;

            var enrollmentPaymentMethod = byte.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PAYMENT_METHOD));

            var student = await _context.Students
               .Where(x => x.Id == studentId)
               .Select(x => new StudentEnrollmentInfoTemplate
               {
                   Id = x.Id,
                   CareerId = x.CareerId,
                   FacultyId = x.Career.FacultyId,
                   UserId = x.UserId,
                   CurriculumId = x.CurriculumId,
                   Status = x.Status,
                   AdmissionTypeId = x.AdmissionTypeId,
                   IsExoneratedEnrollment = x.AdmissionType.IsExoneratedEnrollment,
                   AdmissionTypeAbbrev = x.AdmissionType.Abbreviation,
                   AdmissionTermStartDate = x.AdmissionTerm.StartDate,
                   Condition = x.Condition,
                   Benefit = x.Benefit,
                   AcademicYear = x.CurrentAcademicYear,
                   EnrollmentFeeId = x.EnrollmentFeeId,
               }).FirstOrDefaultAsync();

            if (student.Benefit != ConstantHelpers.Student.Benefit.NONE)
            {
                if (student.Condition == ConstantHelpers.Student.Condition.WORKERS_SON
                    && student.Benefit == ConstantHelpers.Student.Benefit.WORKERS_SON)
                    student.DiscountPercentage = ConstantHelpers.Student.Benefit.DISCOUNTS[student.Benefit];
            }

            var currentPayments = await _context.Payments
                   .Where(x => x.UserId == student.UserId
                   && x.TermId == term.Id
                   && (x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                   || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)
                   && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING)
                   .ToListAsync();
            //var currentEnrollmentFeeStudents = await _context.StudentEnrollmentFees
            //    .Where(x => x.StudentId == student.Id && x.TermId == term.Id)
            //    .ToListAsync();

            if (enrollmentPaymentMethod == 2)
            {
                var isEnrolled = await _context.StudentSections
                    .AnyAsync(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == term.Id);
                if (!isEnrolled)
                    return;
            }

            var academicYearCourses = await _context.AcademicYearCourses
            .Where(x => x.CurriculumId == student.CurriculumId)
            .Select(x => new
            {
                x.CurriculumId,
                x.CourseId,
                x.AcademicYear
            }).ToListAsync();

            #region Alumnos exonerados
            var exemptedStudents = new List<Guid>();

            var exemptFirstPlaces = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.EXEMPT_FIRST_PLACES_FROM_PAYMENTS));
            if (exemptFirstPlaces)
            {
                var regularTerm = await _context.Terms
                      .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2"))
                      .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number)
                      .FirstOrDefaultAsync();

                var firstPlaceQuantity = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.FIRST_PLACES_QUANTITY));
                var exemptType = byte.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.PAYMENT_EXEMPTION_TYPE));

                if (exemptType == 1)
                {
                    var academicSummaries = await _context.AcademicSummaries
                        .Where(x => x.TermId == regularTerm.Id && x.CareerId == student.CareerId)
                        .Select(x => new
                        {
                            x.StudentId,
                            x.WeightedAverageGrade,
                            x.CareerId,
                            x.Career.FacultyId,
                            x.StudentAcademicYear
                        }).ToListAsync();

                    var students = academicSummaries
                        .OrderByDescending(x => x.WeightedAverageGrade)
                        .ToArray();

                    var cont = 0;
                    var lastGrade = -1.0M;
                    var i = 0;

                    while (cont < firstPlaceQuantity && i < students.Count())
                    {
                        exemptedStudents.Add(students[i].StudentId);

                        if (lastGrade != students[i].WeightedAverageGrade)
                        {
                            lastGrade = students[i].WeightedAverageGrade;
                            cont++;
                        }

                        i++;
                    }
                }
                else if (exemptType == 2)
                {
                    var academicSummaries = await _context.AcademicSummaries
                 .Where(x => x.TermId == regularTerm.Id && x.CareerId == student.CareerId)
                 .Select(x => new
                 {
                     x.StudentId,
                     x.WeightedAverageGrade,
                     x.CareerId,
                     x.Career.FacultyId,
                     x.StudentAcademicYear
                 }).ToListAsync();

                    var academicYears = academicYearCourses
                        .Select(x => x.AcademicYear)
                        .Distinct()
                        .ToList();

                    foreach (var academicYear in academicYears)
                    {
                        var students = academicSummaries
                            .Where(x => x.StudentAcademicYear == academicYear)
                            .OrderByDescending(x => x.WeightedAverageGrade)
                            .ToArray();

                        var cont = 0;
                        var lastGrade = -1.0M;
                        var i = 0;

                        while (cont < firstPlaceQuantity && i < students.Count())
                        {
                            exemptedStudents.Add(students[i].StudentId);

                            if (lastGrade != students[i].WeightedAverageGrade)
                            {
                                lastGrade = students[i].WeightedAverageGrade;
                                cont++;
                            }

                            i++;
                        }
                    }
                }
                else if (exemptType == 3)
                {
                    var academicSummaries = await _context.AcademicSummaries
                        .Where(x => x.TermId == regularTerm.Id && x.Career.FacultyId == student.FacultyId)
                        .Select(x => new
                        {
                            x.StudentId,
                            x.WeightedAverageGrade,
                            x.CareerId,
                            x.Career.FacultyId,
                            x.StudentAcademicYear
                        }).ToListAsync();

                    var students = academicSummaries
                        .OrderByDescending(x => x.WeightedAverageGrade)
                        .ToArray();

                    var cont = 0;
                    var lastGrade = -1.0M;
                    var i = 0;

                    while (cont < firstPlaceQuantity && i < students.Count())
                    {
                        exemptedStudents.Add(students[i].StudentId);

                        if (lastGrade != students[i].WeightedAverageGrade)
                        {
                            lastGrade = students[i].WeightedAverageGrade;
                            cont++;
                        }

                        i++;
                    }
                }
            }

            var isExempted = exemptedStudents.Contains(student.Id);
            #endregion

            //var enrollmentFeeStudents = new List<StudentEnrollmentFee>();
            var payments = new List<Payment>();

            var createRegularConcept = true;

            if (student.Status != ConstantHelpers.Student.States.TRANSFER && student.Status != ConstantHelpers.Student.States.ENTRANT)
            {
                var disapprovedCoursesResult = await CreateDissaprovedCoursesConcepts(student, term.Id, isExempted, enrollmentPaymentMethod);
                if (disapprovedCoursesResult.Item2)
                    createRegularConcept = true;

                payments.AddRange(disapprovedCoursesResult.Item1);
            }

            var additionalPayments = await CreateAdditionalEnrollmentConcepts(student, term.Id, isExempted);
            payments.AddRange(additionalPayments.Item1);
            if (additionalPayments.Item2) createRegularConcept = true;

            var isExtemporaneous = term.ComplementaryEnrollmentStartDate <= DateTime.UtcNow && DateTime.UtcNow <= term.ComplementaryEnrollmentEndDate;
            if (enrollmentPaymentMethod == 3) //pago posterior
            {
                var studentSection = await _context.StudentSections
                    .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == term.Id && x.CreatedAt.HasValue)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
                if (studentSection != null)
                    isExtemporaneous = term.ComplementaryEnrollmentStartDate.Date <= studentSection.CreatedAt.Value.Date && studentSection.CreatedAt.Value.Date <= term.ComplementaryEnrollmentEndDate.Date;
            }

            if (isExtemporaneous)
            {
                var conceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXTEMPORANEOUS_ENROLLMENT_SURCHARGE_PROCEDURE));
                var extemporaneousEnrollmentModality = byte.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXTEMPORANEOUS_ENROLLMENT_MODALITY));

                if (isRectification && !currentPayments.Any(x => x.ConceptId == conceptId))
                {

                }
                else
                {
                    //Se reemplaza al pago regular por lo que ya no se creará
                    if (extemporaneousEnrollmentModality == 2) createRegularConcept = false;

                    var extemporaneousConcept = await _context.EnrollmentConcepts
                    .OrderByDescending(x => x.CareerId)
                    .ThenByDescending(x => x.AdmissionTypeId)
                    .FirstOrDefaultAsync(x => (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                    && (!x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId)
                    && x.Type == ConstantHelpers.EnrollmentConcept.Type.EXTEMPORANEOUS_ENROLLMENT_CONCEPT);
                    if (extemporaneousConcept != null) conceptId = extemporaneousConcept.ConceptId;

                    var concept = await _context.Concepts.FindAsync(conceptId);

                    var total = isExempted ? 0.00M : concept.Amount;
                    var discount = total * student.DiscountPercentage / 100.0M;

                    total -= discount;
                    var subtotal = total;
                    var igv = 0.00M;

                    if (concept.IsTaxed)
                    {
                        subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                        igv = total + discount - subtotal;
                    }

                    payments.Add(new Payment
                    {
                        Description = concept.Description,
                        SubTotal = subtotal,
                        IgvAmount = igv,
                        Discount = discount,
                        Total = total,
                        EntityId = null,
                        Type = ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT,
                        UserId = student.UserId,
                        ConceptId = concept.Id,
                        TermId = term.Id,
                        CurrentAccountId = concept.CurrentAccountId
                    });
                }
            }

            if (createRegularConcept)
            {
                var regularResult = await CreateEnrollmentConcept(student, term.Id, isExempted, enrollmentPaymentMethod);

                payments.AddRange(regularResult.Item1);
                //enrollmentFeeStudents.AddRange(regularResult.Item2);
            }

            //_context.StudentEnrollmentFees.RemoveRange(currentEnrollmentFeeStudents);
            _context.Payments.RemoveRange(currentPayments);
            await _context.SaveChangesAsync();

            var paidPayments = await _context.Payments
                  .Where(x => x.UserId == student.UserId && x.TermId == term.Id
                  && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                  .ToListAsync();

            var newPayments = new List<Payment>();
            foreach (var item in payments)
            {
                var paidPayment = paidPayments.FirstOrDefault(x => x.ConceptId == item.ConceptId && x.Total == item.Total);

                if (paidPayment != null)
                {
                    if (paidPayment.IsBankPayment) paidPayment.WasBankPaymentUsed = true;
                    paidPayment.Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT;
                }
                else
                    newPayments.Add(item);
            }

            //var newEnrollmentFeeStudents = new List<StudentEnrollmentFee>();
            //foreach (var item in enrollmentFeeStudents)
            //{
            //    var paidPayment = paidPayments.FirstOrDefault(x => x.ConceptId == item.ConceptId && x.Total == item.Amount);

            //    if (paidPayment != null)
            //    {
            //        if (paidPayment.IsBankPayment) paidPayment.WasBankPaymentUsed = true;
            //        paidPayment.Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT;
            //    }
            //    else
            //        newEnrollmentFeeStudents.Add(item);
            //}

            //await _context.StudentEnrollmentFees.AddRangeAsync(newEnrollmentFeeStudents);
            await _context.Payments.AddRangeAsync(newPayments);
            await _context.SaveChangesAsync();
        }

        private async Task<Tuple<List<Payment>, List<Payment>>> CreateEnrollmentConcept(StudentEnrollmentInfoTemplate student, Guid termId, bool isExempted, byte enrollmentPaymentMethod)
        {
            var enrollmentConcepts = await _context.EnrollmentConcepts
                .IgnoreQueryFilters()
                .Select(x => new
                {
                    x.ConceptId,
                    x.Concept.Description,
                    x.Concept.Amount,
                    x.Concept.IsTaxed,
                    x.Type,
                    x.CareerId,
                    x.AdmissionTypeId,
                    x.Condition
                }).ToListAsync();

            var payments = new List<Payment>();
            var enrollmentFeeStudents = new List<Payment>();

            //var enrollmentFees = await _context.EnrollmentFees.Where(x => x.TermId == termId).ToListAsync();

            var totalCredits = await _context.StudentSections
                .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termId)
                .SumAsync(x => x.Section.CourseTerm.Course.Credits);

            if (student.Status == ConstantHelpers.Student.States.TRANSFER || student.Status == ConstantHelpers.Student.States.ENTRANT)
            {
                var admissionConceptId = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ADMISSION_ENROLLMENT_PROCEDURE));

                var enrollmentSpecialConcepts = await _context.EnrollmentConcepts
                    .Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ENROLLMENT_CONCEPT)
                    .ToListAsync();
                var specialConcept = enrollmentSpecialConcepts
                    .OrderByDescending(x => x.CareerId)
                    .ThenByDescending(x => x.AdmissionTypeId)
                    .ThenByDescending(x => x.Condition)
                    .FirstOrDefault(x => (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                    && (!x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId)
                    && (!x.Condition.HasValue || x.Condition == student.Condition));
                if (specialConcept != null) admissionConceptId = specialConcept.ConceptId;

                var admissionConcept = await _context.Concepts.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == admissionConceptId);
                var admissionEnrollmentCost = admissionConcept.Amount;
                var discount = admissionEnrollmentCost * student.DiscountPercentage / 100.0M;

                admissionEnrollmentCost -= discount;
                var admissionEnrollmentSubTotal = admissionEnrollmentCost;
                var admissionEnrollmentIgvAmount = 0.00M;
                if (admissionConcept.IsTaxed)
                {
                    admissionEnrollmentSubTotal = discount + admissionEnrollmentCost / (1.00M + ConstantHelpers.Treasury.IGV);
                    admissionEnrollmentIgvAmount = admissionEnrollmentCost + discount - admissionEnrollmentSubTotal;
                }

                //if (enrollmentFees.Any(x => x.AdmissionTypeId == student.AdmissionTypeId) && !student.IsExoneratedEnrollment)
                //    enrollmentFeeStudents.Add(new StudentEnrollmentFee
                //    {
                //        Amount = admissionEnrollmentCost,
                //        ConceptId = admissionConceptId,
                //        StudentId = student.Id,
                //        TermId = termId
                //    });
                //else
                    payments.Add(new Payment
                    {
                        Id = Guid.NewGuid(),
                        Description = admissionConcept.Description,
                        SubTotal = student.IsExoneratedEnrollment ? 0.00M : admissionEnrollmentSubTotal,
                        IgvAmount = student.IsExoneratedEnrollment ? 0.00M : admissionEnrollmentIgvAmount,
                        Discount = student.IsExoneratedEnrollment ? 0.00M : discount,
                        Total = student.IsExoneratedEnrollment ? 0.00M : admissionEnrollmentCost,
                        EntityId = Guid.Empty,
                        Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                        UserId = student.UserId,
                        ConceptId = admissionConceptId,
                        TermId = termId,
                        CurrentAccountId = admissionConcept.CurrentAccountId
                    });
            }
            else if (enrollmentPaymentMethod == 2 && totalCredits < 12
                && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD)
            {
                var twelveEnrollmentConcepts = enrollmentConcepts.Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.LESS_THAN_TWELVE_CREDITS_ENROLLMENT).ToList();
                var twelveConcept = twelveEnrollmentConcepts
                    .OrderByDescending(x => x.CareerId)
                    .ThenByDescending(x => x.AdmissionTypeId)
                    .ThenByDescending(x => x.Condition)
                    .FirstOrDefault(x => (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                    && (!x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId)
                    && (!x.Condition.HasValue || x.Condition == student.Condition));

                if (twelveConcept != null)
                {
                    var irregularConcept = await _context.Concepts.FindAsync(twelveConcept.ConceptId);

                    if (irregularConcept != null)
                    {
                        var irregularEnrollmentCost = irregularConcept.Amount;
                        var discount = irregularEnrollmentCost * student.DiscountPercentage / 100.0M;

                        irregularEnrollmentCost -= discount;
                        var irregularEnrollmentSubTotal = irregularEnrollmentCost;
                        var irregularEnrollmentIgvAmount = 0.00M;

                        if (irregularConcept.IsTaxed)
                        {
                            irregularEnrollmentSubTotal = discount + irregularEnrollmentCost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                            irregularEnrollmentIgvAmount = irregularEnrollmentCost + discount - irregularEnrollmentSubTotal;
                        }


                        //if (enrollmentFees.Any(x => x.AdmissionTypeId == student.AdmissionTypeId))
                        //    enrollmentFeeStudents.Add(new StudentEnrollmentFee
                        //    {
                        //        Amount = irregularEnrollmentCost,
                        //        ConceptId = irregularConcept.Id,
                        //        StudentId = student.Id,
                        //        TermId = termId,
                        //    });
                        //else
                            payments.Add(new Payment
                            {
                                Description = irregularConcept.Description,
                                SubTotal = irregularEnrollmentSubTotal,
                                IgvAmount = irregularEnrollmentIgvAmount,
                                Discount = discount,
                                Total = irregularEnrollmentCost,
                                EntityId = null,
                                Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                UserId = student.UserId,
                                ConceptId = irregularConcept.Id,
                                TermId = termId,
                                CurrentAccountId = irregularConcept.CurrentAccountId
                            });
                    }
                }
                else
                {
                    var irregularModality = int.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.SPECIAL_ENROLLMENT_MODALITY));

                    switch (irregularModality)
                    {
                        case 1:
                        default:
                            var irregularConceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.SPECIAL_ENROLLMENT_PROCEDURE));
                            var irregularConcept = await _context.Concepts.FindAsync(irregularConceptId);

                            if (irregularConcept != null)
                            {
                                var irregularEnrollmentCost = irregularConcept.Amount;
                                var irregularDiscount = irregularEnrollmentCost * student.DiscountPercentage / 100.0M;

                                irregularEnrollmentCost -= irregularDiscount;
                                var irregularEnrollmentSubTotal = irregularEnrollmentCost;
                                var irregularEnrollmentIgvAmount = 0.00M;

                                if (irregularConcept.IsTaxed)
                                {
                                    irregularEnrollmentSubTotal = irregularDiscount + irregularEnrollmentCost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                                    irregularEnrollmentIgvAmount = irregularEnrollmentCost + irregularDiscount - irregularEnrollmentSubTotal;// * CORE.Helpers.ConstantHelpers.Treasury.IGV;
                                }


                                //if (enrollmentFees.Any(x => x.AdmissionTypeId == student.AdmissionTypeId))
                                //    enrollmentFeeStudents.Add(new StudentEnrollmentFee
                                //    {
                                //        Amount = irregularEnrollmentCost,
                                //        ConceptId = irregularConceptId,
                                //        StudentId = student.Id,
                                //        TermId = termId,
                                //    });
                                //else
                                    payments.Add(new Payment
                                    {
                                        Description = irregularConcept.Description,
                                        SubTotal = irregularEnrollmentSubTotal,
                                        IgvAmount = irregularEnrollmentIgvAmount,
                                        Discount = irregularDiscount,
                                        Total = irregularEnrollmentCost,
                                        EntityId = null,
                                        Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                        UserId = student.UserId,
                                        ConceptId = irregularConceptId,
                                        TermId = termId,
                                        CurrentAccountId = irregularConcept.CurrentAccountId
                                    });
                            }

                            break;

                        case 2:
                            var irregularCreditCost = decimal.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.SPECIAL_ENROLLMENT_CREDIT_COST));

                            var irregularEnrollmentCreditCost = irregularCreditCost * totalCredits;
                            var discount = irregularEnrollmentCreditCost * student.DiscountPercentage / 100.0M;

                            irregularEnrollmentCreditCost -= discount;
                            var irregularEnrollmentCreditSubtotal = irregularEnrollmentCreditCost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                            var irregularEnrollmentCreditIgv = irregularEnrollmentCreditCost - irregularEnrollmentCreditSubtotal;

                            payments.Add(new Payment
                            {
                                Description = "Pago por matrícula especial por ciclo menor a 12 créditos",
                                SubTotal = irregularEnrollmentCreditSubtotal,
                                IgvAmount = irregularEnrollmentCreditIgv,
                                Discount = discount,
                                Total = irregularEnrollmentCreditCost,
                                EntityId = null,
                                Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                UserId = student.UserId,
                                ConceptId = null,
                                TermId = termId,
                                CurrentAccountId = null
                            });

                            break;
                    }
                }
            }
            else if (student.Status == ConstantHelpers.Student.States.UNBEATEN || student.Status == ConstantHelpers.Student.States.HIGH_PERFORMANCE || (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD && student.Status == ConstantHelpers.Student.States.REGULAR))
            {
                var exonerateUnbeatenStudents = bool.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXONERATE_REGULAR_STUDENTS_FROM_PAYMENT));

                if (!exonerateUnbeatenStudents || student.Condition == ConstantHelpers.Student.Condition.PERMANENT_PAYER || student.Condition == ConstantHelpers.Student.Condition.SECOND_CAREER)
                {
                    var unbeatenConceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENT_ENROLLMENT_PROCEDURE));

                    var unbeatenEnrollmentSpecialConcepts = enrollmentConcepts
                        .Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.UNBEATEN_ENROLLMENT_CONCEPT)
                        .ToList();
                    var unbeatenSpecialConcept = unbeatenEnrollmentSpecialConcepts
                        .OrderByDescending(x => x.CareerId)
                        .ThenByDescending(x => x.AdmissionTypeId)
                        .ThenByDescending(x => x.Condition)
                        .FirstOrDefault(x => (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                        && (!x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId)
                        && (!x.Condition.HasValue || x.Condition == student.Condition));
                    if (unbeatenSpecialConcept != null) unbeatenConceptId = unbeatenSpecialConcept.ConceptId;

                    var unbeatenConcept = await _context.Concepts.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == unbeatenConceptId);
                    var unbeatenEnrollmentCost = unbeatenConcept.Amount;
                    //if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNCP
                    //&& (student.Status == ConstantHelpers.Student.States.REPEATER || student.Condition == ConstantHelpers.Student.Condition.TRANSITORY_PAYER || student.Condition == ConstantHelpers.Student.Condition.PERMANENT_PAYER))
                    //{
                    //    unbeatenEnrollmentCost += 30.0M;
                    //}

                    var discount = unbeatenEnrollmentCost * student.DiscountPercentage / 100.0M;

                    unbeatenEnrollmentCost -= discount;
                    var unbeatenEnrollmentSubTotal = unbeatenEnrollmentCost;
                    var unbeatenEnrollmentIgvAmount = 0.00M;

                    if (unbeatenConcept.IsTaxed)
                    {
                        unbeatenEnrollmentSubTotal = discount + unbeatenEnrollmentCost / (1.00M + ConstantHelpers.Treasury.IGV);
                        unbeatenEnrollmentIgvAmount = unbeatenEnrollmentCost + discount - unbeatenEnrollmentSubTotal;
                    }

                    //if (enrollmentFees.Any(x => x.AdmissionTypeId == student.AdmissionTypeId) && !isExempted && !student.IsExoneratedEnrollment)
                    //    enrollmentFeeStudents.Add(new StudentEnrollmentFee
                    //    {
                    //        Amount = unbeatenEnrollmentCost,
                    //        ConceptId = unbeatenConceptId,
                    //        StudentId = student.Id,
                    //        TermId = termId
                    //    });
                    //else
                    {
                        var payment = new Payment
                        {
                            Id = Guid.NewGuid(),
                            Description = unbeatenConcept.Description,
                            SubTotal = isExempted || student.IsExoneratedEnrollment ? 0.00M : unbeatenEnrollmentSubTotal,
                            IgvAmount = isExempted || student.IsExoneratedEnrollment ? 0.00M : unbeatenEnrollmentIgvAmount,
                            Discount = isExempted || student.IsExoneratedEnrollment ? 0.00M : discount,
                            Total = isExempted || student.IsExoneratedEnrollment ? 0.00M : unbeatenEnrollmentCost,
                            EntityId = Guid.Empty,
                            Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId,
                            ConceptId = unbeatenConceptId,
                            TermId = termId,
                            CurrentAccountId = unbeatenConcept.CurrentAccountId
                        };
                        payments.Add(payment);
                    }
                }
            }
            else
            {
                var regularConceptId = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REGULAR_ENROLLMENT_PROCEDURE));

                var enrollmentSpecialConcepts = enrollmentConcepts
                    .Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ENROLLMENT_CONCEPT)
                    .ToList();
                var specialConcept = enrollmentSpecialConcepts
                    .OrderByDescending(x => x.CareerId)
                    .ThenByDescending(x => x.AdmissionTypeId)
                    .ThenByDescending(x => x.Condition)
                    .FirstOrDefault(x => (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                    && (!x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId)
                    && (!x.Condition.HasValue || x.Condition == student.Condition));
                if (specialConcept != null) regularConceptId = specialConcept.ConceptId;

                var regularConcept = await _context.Concepts.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == regularConceptId);
                var regularEnrollmentCost = regularConcept.Amount;
                //if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNCP
                //    && (student.Status == ConstantHelpers.Student.States.REPEATER || student.Condition == ConstantHelpers.Student.Condition.TRANSITORY_PAYER || student.Condition == ConstantHelpers.Student.Condition.PERMANENT_PAYER))
                //{
                //    regularEnrollmentCost += 30.0M;
                //}

                var discount = regularEnrollmentCost * student.DiscountPercentage / 100.0M;

                regularEnrollmentCost -= discount;
                var regularEnrollmentSubTotal = regularEnrollmentCost;
                var regularEnrollmentIgvAmount = 0.00M;
                if (regularConcept.IsTaxed)
                {
                    regularEnrollmentSubTotal = discount + regularEnrollmentCost / (1.00M + ConstantHelpers.Treasury.IGV);
                    regularEnrollmentIgvAmount = regularEnrollmentCost + discount - regularEnrollmentSubTotal;
                }

                //if (enrollmentFees.Any(x => x.AdmissionTypeId == student.AdmissionTypeId) && !isExempted && !student.IsExoneratedEnrollment)
                //    enrollmentFeeStudents.Add(new StudentEnrollmentFee
                //    {
                //        Amount = regularEnrollmentCost,
                //        ConceptId = regularConceptId,
                //        StudentId = student.Id,
                //        TermId = termId
                //    });
                //else
                {
                    var payment = new Payment
                    {
                        Id = Guid.NewGuid(),
                        Description = regularConcept.Description,
                        SubTotal = isExempted || student.IsExoneratedEnrollment ? 0.00M : regularEnrollmentSubTotal,
                        IgvAmount = isExempted || student.IsExoneratedEnrollment ? 0.00M : regularEnrollmentIgvAmount,
                        Discount = isExempted || student.IsExoneratedEnrollment ? 0.00M : discount,
                        Total = isExempted || student.IsExoneratedEnrollment ? 0.00M : regularEnrollmentCost,
                        EntityId = Guid.Empty,
                        Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                        UserId = student.UserId,
                        ConceptId = regularConceptId,
                        TermId = termId,
                        CurrentAccountId = regularConcept.CurrentAccountId
                    };
                    payments.Add(payment);
                }
            }

            //return new Tuple<List<Payment>, List<Payment>>(payments, enrollmentFeeStudents);
            return new Tuple<List<Payment>, List<Payment>>(payments, enrollmentFeeStudents);
        }

        private async Task<Tuple<List<Payment>, bool>> CreateDissaprovedCoursesConcepts(StudentEnrollmentInfoTemplate student, Guid termId, bool isExempted, byte enrollmentPaymentMethod)
        {
            var createRegularConcept = false;
            var payments = new List<Payment>();

            //CONCEPTOS UNAMAD
            var oneDisapprovedCourseConceptId = new Guid("666892be-c64e-4851-6b67-08d8672e2958");
            var twoDisapprovedCourseConceptId = new Guid("b543dbd4-3461-4d8e-6b68-08d8672e2958");
            var threeDisapprovedCourseConceptId = new Guid("4513db1d-6ec2-47d5-6b69-08d8672e2958");

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA)
            {
                oneDisapprovedCourseConceptId = new Guid("79F67FA8-F3A2-1DE7-9181-A67B7542122C");
                twoDisapprovedCourseConceptId = new Guid("7F3BDAE4-CEBB-4523-D777-2B0674A318D5");
                threeDisapprovedCourseConceptId = new Guid("1C097916-885A-AF0F-6FB5-E6087EB1B2DC");
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMBA)
            {
                oneDisapprovedCourseConceptId = new Guid("08db525e-6632-4fa5-83bb-2894609f966d");
                twoDisapprovedCourseConceptId = new Guid("08db525e-7661-48f7-8251-1831cde8aea8");
                threeDisapprovedCourseConceptId = new Guid("08db525e-89ea-430b-85c8-2423cb117313");
            }

            var disapprovedCourseConcepts = await _context.DisapprovedCourseConcepts.Include(x => x.Concept).ToListAsync();

            if (enrollmentPaymentMethod == 2)
            {
                createRegularConcept = true;

                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA
                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMBA)
                {
                    var lastTerm = await _context.AcademicHistories
                        .Where(x => x.Term.Number == "1" || x.Term.Number == "2")
                        .OrderByDescending(x => x.Term.Year)
                        .ThenByDescending(x => x.Term.Number)
                        .Select(x => x.Term)
                        .FirstOrDefaultAsync();


                    if (lastTerm != null)
                    {
                        var disaprovedCourses = await _context.AcademicHistories
                            .Where(x => x.StudentId == student.Id && x.TermId == lastTerm.Id && !x.Approved && !x.Withdraw)
                            .Select(x => new
                            {
                                x.Id,
                                x.Course.Credits,
                                x.Try,
                                x.CourseId
                            })
                            .ToListAsync();

                        if (disaprovedCourses.Any())
                        {
                            var concept = new Concept();
                            concept = disaprovedCourses.Count switch
                            {
                                1 => await _context.Concepts.FindAsync(oneDisapprovedCourseConceptId),
                                2 => await _context.Concepts.FindAsync(twoDisapprovedCourseConceptId),
                                _ => await _context.Concepts.FindAsync(threeDisapprovedCourseConceptId),
                            };

                            var total = isExempted ? 0.00M : disaprovedCourses.Sum(x => x.Credits) * concept.Amount;
                            var discount = total * student.DiscountPercentage / 100.0M;

                            total -= discount;
                            var subtotal = total;
                            var igv = 0.00M;

                            if (concept.IsTaxed)
                            {
                                subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                igv = total + discount - subtotal;
                            }

                            payments.Add(new Payment
                            {
                                Id = Guid.NewGuid(),
                                Description = $"{concept.Description} {disaprovedCourses.Sum(x => x.Credits)} cred.",
                                SubTotal = subtotal,
                                IgvAmount = igv,
                                Discount = discount,
                                Total = total,
                                EntityId = null,
                                Type = (byte)ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                UserId = student.UserId,
                                ConceptId = concept.Id,
                                TermId = termId,
                                CurrentAccountId = concept.CurrentAccountId
                            });
                        }
                    }
                }
                else
                {
                    var studentSections = await _context.StudentSections
                       .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termId)
                       .Select(x => new
                       {
                           x.Id,
                           x.Section.CourseTerm.Course.Credits,
                           x.Try,
                           x.Section.CourseTerm.CourseId
                       })
                       .ToListAsync();

                    var disaprovedCourses = studentSections.Where(x => x.Try > 1).ToList();

                    if (disaprovedCourses.Count > 0)
                    {
                        var disapprovedPayments = new List<Payment>();

                        foreach (var item in disaprovedCourses)
                        {
                            var concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == item.Try && x.AdmissionTypeId == student.AdmissionTypeId);
                            if (concept == null)
                                concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == item.Try && !x.AdmissionTypeId.HasValue);
                            if (concept == null) continue;

                            var total = isExempted ? 0.00M : concept.IsCostPerCredit ? item.Credits * concept.Concept.Amount : concept.Concept.Amount;
                            var discount = total * student.DiscountPercentage / 100.0M;

                            total -= discount;
                            var subtotal = total;
                            var igv = 0.00M;
                            if (concept.Concept.IsTaxed)
                            {
                                subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                igv = total + discount - subtotal;
                            }

                            disapprovedPayments.Add(new Payment
                            {
                                Id = Guid.NewGuid(),
                                Description = concept.Concept.Description,
                                SubTotal = subtotal,
                                IgvAmount = igv,
                                Discount = discount,
                                Total = total,
                                EntityId = item.CourseId,
                                Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                UserId = student.UserId,
                                ConceptId = concept.Concept.Id,
                                TermId = termId,
                                CurrentAccountId = concept.Concept.CurrentAccountId
                            });
                        }

                        disapprovedPayments = disapprovedPayments
                               .GroupBy(x => new { x.Description, x.UserId, x.ConceptId, x.TermId, x.CurrentAccountId })
                               .Select(x => new Payment
                               {
                                   Id = Guid.NewGuid(),
                                   Description = $"{x.Key.Description} X{x.Count()}",
                                   SubTotal = x.Sum(y => y.SubTotal),
                                   IgvAmount = x.Sum(y => y.IgvAmount),
                                   Discount = x.Sum(y => y.Discount),
                                   Total = x.Sum(y => y.Total),
                                   EntityId = null,
                                   Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                   UserId = student.UserId,
                                   ConceptId = x.Key.ConceptId,
                                   TermId = termId,
                                   CurrentAccountId = x.Key.CurrentAccountId,
                                   Quantity = x.Count()
                               }).ToList();

                        payments.AddRange(disapprovedPayments);
                    }
                }
            }
            else
            {
                var regularTerm = await _context.Terms
                      .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2"))
                      .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number)
                      .FirstOrDefaultAsync();

                var summerTerm = await _context.Terms
                    .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && x.IsSummer)
                    .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number).FirstOrDefaultAsync();

                var validateSummer = false;

                if (regularTerm == null && summerTerm != null) validateSummer = true;
                else if (summerTerm == null) validateSummer = false;
                else validateSummer = summerTerm.StartDate > regularTerm.StartDate;

                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
                    validateSummer = false;

                var academicHistories = await _context.AcademicHistories
                    .Where(x => x.StudentId == student.Id && !x.Withdraw)
                    .Select(x => new
                    {
                        x.StudentId,
                        x.CourseId,
                        x.Approved,
                        x.Try,
                        x.TermId,
                        x.Course.Credits,
                        x.Course.Code,
                        x.Validated,
                        TermNumber = x.Term.Number,
                        TermName = x.Term.Name,
                        TermYear = x.Term.Year,
                        x.Term.StartDate
                    }).ToListAsync();

                var allSummaries = academicHistories
                         .Where(x => !x.Validated && (x.TermNumber == "1" || x.TermNumber == "2"))
                         .GroupBy(x => new { x.TermYear, x.TermNumber, x.StartDate, x.TermName })
                         .Select(x => new
                         {
                             Year = x.Key.TermYear,
                             Number = x.Key.TermNumber,
                             x.Key.TermName,
                             x.Key.StartDate,
                             Courses = x.ToList()
                         }).ToList();

                var lastSummary = allSummaries.OrderBy(x => x.Year).ThenBy(x => x.Number).LastOrDefault();

                if (lastSummary != null && lastSummary.TermName != regularTerm.Name)
                {
                    var disapprovedCourses = lastSummary.Courses.Where(x => !academicHistories.Any(y => y.CourseId == x.CourseId && y.Approved)).ToList();

                    if (disapprovedCourses.Any())
                    {
                        createRegularConcept = true;

                        if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD
                            || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA
                            || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMBA)
                        {
                            var concept = new Concept();
                            switch (disapprovedCourses.Count)
                            {
                                case 1:
                                    concept = await _context.Concepts.FindAsync(oneDisapprovedCourseConceptId);
                                    break;
                                case 2:
                                    concept = await _context.Concepts.FindAsync(twoDisapprovedCourseConceptId);
                                    break;
                                default:
                                    concept = await _context.Concepts.FindAsync(threeDisapprovedCourseConceptId);
                                    break;
                            }

                            var totalCredits = disapprovedCourses.Sum(x => x.Credits);

                            var total = isExempted ? 0.00M : totalCredits * concept.Amount;
                            var discount = total * student.DiscountPercentage / 100.0M;

                            total -= discount;
                            var subtotal = total;
                            var igv = 0.00M;
                            if (concept.IsTaxed)
                            {
                                subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                igv = total + discount - subtotal;
                            }

                            var coursePayment = new Payment
                            {
                                Id = Guid.NewGuid(),
                                Description = $"{concept.Description} {totalCredits} cred.",
                                SubTotal = subtotal,
                                IgvAmount = igv,
                                Discount = discount,
                                Total = total,
                                EntityId = null,
                                Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                UserId = student.UserId,
                                ConceptId = concept.Id,
                                TermId = termId,
                                CurrentAccountId = concept.CurrentAccountId
                            };

                            payments.Add(coursePayment);
                        }
                        else
                        {
                            foreach (var item in disapprovedCourses)
                            {
                                var lastTry = academicHistories
                                  .Where(x => x.CourseId == item.CourseId)
                                  .OrderBy(x => x.Try)
                                  .Select(x => x.Try)
                                  .Last();

                                var concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == lastTry + 1 && x.AdmissionTypeId == student.AdmissionTypeId);
                                if (concept == null)
                                    concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == lastTry + 1);
                                if (concept == null) continue;

                                var total = concept.IsCostPerCredit ? item.Credits * concept.Concept.Amount : concept.Concept.Amount;
                                var discount = total * student.DiscountPercentage / 100.0M;

                                total -= discount;
                                var subtotal = total;
                                var igv = 0.00M;
                                if (concept.Concept.IsTaxed)
                                {
                                    subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                    igv = total + discount - subtotal;
                                }

                                payments.Add(new Payment
                                {
                                    Id = Guid.NewGuid(),
                                    Description = $"{concept.Concept.Description} - {item.Code}",
                                    SubTotal = subtotal,
                                    IgvAmount = igv,
                                    Discount = discount,
                                    Total = total,
                                    EntityId = item.CourseId,
                                    Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = concept.Concept.Id,
                                    TermId = termId,
                                    CurrentAccountId = concept.Concept.CurrentAccountId
                                });
                            }
                        }
                    }
                }
                else
                {
                    var curriculumCourses = _context.AcademicYearCourses
                        .Where(x => x.CurriculumId == student.CurriculumId)
                        .Select(x => x.CourseId)
                        .ToHashSet();

                    var regularCourses = academicHistories
                        .Where(x => curriculumCourses.Contains(x.CourseId) && regularTerm != null && x.TermId == regularTerm.Id)
                        .ToList();

                    var regularApprovedCourses = regularCourses
                        .Where(x => x.Approved)
                        .Select(x => x.CourseId)
                        .ToList();

                    var regularDisapprovedCourses = regularCourses
                        .Where(x => !regularApprovedCourses.Contains(x.CourseId) && !x.Approved)
                        .ToList();

                    if (validateSummer)
                    {
                        var summerCourses = academicHistories
                            .Where(x => curriculumCourses.Contains(x.CourseId) && x.TermId == summerTerm.Id)
                            .ToList();

                        var summerApprovedCourses = summerCourses
                            .Where(x => x.Approved)
                            .Select(x => x.CourseId)
                            .ToList();

                        var summerDisapprovedCourses = summerCourses
                            .Where(x => !summerApprovedCourses.Contains(x.CourseId) && !x.Approved)
                            .ToList();

                        if (summerDisapprovedCourses.Any())
                        {
                            createRegularConcept = true;

                            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD
                                || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA
                                || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMBA)
                            {
                                var concept = new Concept();
                                switch (summerDisapprovedCourses.Count)
                                {
                                    case 1:
                                        concept = await _context.Concepts.FindAsync(oneDisapprovedCourseConceptId);
                                        break;
                                    case 2:
                                        concept = await _context.Concepts.FindAsync(twoDisapprovedCourseConceptId);
                                        break;
                                    default:
                                        concept = await _context.Concepts.FindAsync(threeDisapprovedCourseConceptId);
                                        break;
                                }

                                var totalCredits = summerDisapprovedCourses.Sum(x => x.Credits);

                                var total = isExempted ? 0.00M : totalCredits * concept.Amount;
                                var discount = total * student.DiscountPercentage / 100.0M;

                                total -= discount;
                                var subtotal = total;
                                var igv = 0.00M;
                                if (concept.IsTaxed)
                                {
                                    subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                    igv = total + discount - subtotal;
                                }

                                var coursePayment = new Payment
                                {
                                    Id = Guid.NewGuid(),
                                    Description = $"{concept.Description} {totalCredits} cred.",
                                    SubTotal = subtotal,
                                    IgvAmount = igv,
                                    Discount = discount,
                                    Total = total,
                                    EntityId = null,
                                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = concept.Id,
                                    TermId = termId,
                                    CurrentAccountId = concept.CurrentAccountId
                                };

                                payments.Add(coursePayment);
                            }
                            else
                            {
                                foreach (var item in summerDisapprovedCourses.Where(x => x.StudentId == student.Id).ToList())
                                {
                                    var concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == item.Try + 1 && x.AdmissionTypeId == student.AdmissionTypeId);
                                    if (concept == null)
                                        concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == item.Try + 1 && !x.AdmissionTypeId.HasValue);
                                    if (concept == null) continue;

                                    var total = isExempted ? 0.00M : concept.IsCostPerCredit ? item.Credits * concept.Concept.Amount : concept.Concept.Amount;
                                    var discount = total * student.DiscountPercentage / 100.0M;

                                    total -= discount;
                                    var subtotal = total;
                                    var igv = 0.00M;
                                    if (concept.Concept.IsTaxed)
                                    {
                                        subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                        igv = total + discount - subtotal;
                                    }

                                    var coursePayment = new Payment
                                    {
                                        Id = Guid.NewGuid(),
                                        Description = concept.Concept.Description,
                                        SubTotal = subtotal,
                                        IgvAmount = igv,
                                        Discount = discount,
                                        Total = total,
                                        EntityId = item.CourseId,
                                        Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                        UserId = student.UserId,
                                        ConceptId = concept.Concept.Id,
                                        TermId = termId,
                                        CurrentAccountId = concept.Concept.CurrentAccountId
                                    };

                                    payments.Add(coursePayment);
                                }
                            }
                        }

                        regularDisapprovedCourses = regularDisapprovedCourses
                            .Where(x => !summerCourses.Any(y => y.CourseId == x.CourseId))
                            .ToList();
                    }

                    if (regularDisapprovedCourses.Any())
                    {
                        createRegularConcept = true;

                        if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD
                            || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA
                            || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMBA)
                        {
                            var concept = new Concept();
                            switch (regularDisapprovedCourses.Count)
                            {
                                case 1:
                                    concept = await _context.Concepts.FindAsync(oneDisapprovedCourseConceptId);
                                    break;
                                case 2:
                                    concept = await _context.Concepts.FindAsync(twoDisapprovedCourseConceptId);
                                    break;
                                default:
                                    concept = await _context.Concepts.FindAsync(threeDisapprovedCourseConceptId);
                                    break;
                            }

                            var totalCredits = regularDisapprovedCourses.Sum(x => x.Credits);

                            var total = isExempted ? 0.00M : totalCredits * concept.Amount;
                            var discount = total * student.DiscountPercentage / 100.0M;

                            total -= discount;
                            var subtotal = total;
                            var igv = 0.00M;
                            if (concept.IsTaxed)
                            {
                                subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                igv = total + discount - subtotal;
                            }

                            var coursePayment = new Payment
                            {
                                Id = Guid.NewGuid(),
                                Description = $"{concept.Description} {totalCredits} cred.",
                                SubTotal = subtotal,
                                IgvAmount = igv,
                                Discount = discount,
                                Total = total,
                                EntityId = null,
                                Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                UserId = student.UserId,
                                ConceptId = concept.Id,
                                TermId = termId,
                                CurrentAccountId = concept.CurrentAccountId
                            };

                            payments.Add(coursePayment);
                        }
                        else
                        {
                            foreach (var item in regularDisapprovedCourses.Where(x => x.StudentId == student.Id).ToList())
                            {
                                var concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == item.Try + 1 && x.AdmissionTypeId == student.AdmissionTypeId);
                                if (concept == null)
                                    concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == item.Try + 1 && !x.AdmissionTypeId.HasValue);
                                if (concept == null) continue;

                                var total = isExempted ? 0.00M : concept.IsCostPerCredit ? item.Credits * concept.Concept.Amount : concept.Concept.Amount;
                                var discount = total * student.DiscountPercentage / 100.0M;

                                total -= discount;
                                var subtotal = total;
                                var igv = 0.00M;
                                if (concept.Concept.IsTaxed)
                                {
                                    subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                    igv = total + discount - subtotal;
                                }

                                var coursePayment = new Payment
                                {
                                    Id = Guid.NewGuid(),
                                    Description = concept.Concept.Description,
                                    SubTotal = subtotal,
                                    IgvAmount = igv,
                                    Discount = discount,
                                    Total = total,
                                    EntityId = item.CourseId,
                                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = concept.Concept.Id,
                                    TermId = termId,
                                    CurrentAccountId = concept.Concept.CurrentAccountId
                                };

                                payments.Add(coursePayment);
                            }
                        }
                    }
                }
            }

            return new Tuple<List<Payment>, bool>(payments, createRegularConcept);
        }

        private async Task<Tuple<List<Payment>, bool>> CreateAdditionalEnrollmentConcepts(StudentEnrollmentInfoTemplate student, Guid termId, bool isExempted)
        {
            var payments = new List<Payment>();
            var createRegularConcept = true;

            var enrollmentConcepts = await _context.EnrollmentConcepts
                .IgnoreQueryFilters()
                .Select(x => new
                {
                    x.ConceptId,
                    x.Concept.Description,
                    x.Concept.Amount,
                    x.Concept.IsTaxed,
                    x.Type,
                    x.CareerId,
                    x.AdmissionTypeId,
                    x.Concept.CurrentAccountId,
                    x.Condition,
                    x.ConceptToReplaceId
                }).ToListAsync();

            var additionalConcepts = enrollmentConcepts.Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ADDITIONAL_CONCEPT
            && (!x.Condition.HasValue || x.Condition == student.Condition)).ToList();
            var replacementConcepts = enrollmentConcepts.Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ADDITIONAL_CONCEPT_REPLACEMENT).ToList();

            if (student.Status == ConstantHelpers.Student.States.ENTRANT)
            {
                additionalConcepts = enrollmentConcepts.Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ADDITIONAL_CONCEPT
                && (!x.Condition.HasValue || x.Condition == student.Condition)).ToList();
                replacementConcepts = enrollmentConcepts.Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ADDITIONAL_CONCEPT_REPLACEMENT).ToList();
            }

            foreach (var item in additionalConcepts)
            {
                var total = isExempted ? 0.00M : item.Amount;
                var discount = total * student.DiscountPercentage / 100.0M;

                total -= discount;
                var subtotal = total;
                var igv = 0.00M;

                var isTaxed = item.IsTaxed;
                var description = item.Description;
                var conceptId = item.ConceptId;
                var currentAccountId = item.CurrentAccountId;

                if (replacementConcepts.Any(x => x.ConceptToReplaceId == item.ConceptId && x.CareerId == student.CareerId))
                {
                    var conceptReplace = replacementConcepts.FirstOrDefault(x => x.ConceptToReplaceId == item.ConceptId && x.CareerId == student.CareerId);

                    total = isExempted ? 0.00M : conceptReplace.Amount;
                    discount = total * student.DiscountPercentage / 100.0M;
                    total -= discount;
                    subtotal = total;
                    igv = 0.00M;

                    isTaxed = conceptReplace.IsTaxed;
                    description = conceptReplace.Description;
                    conceptId = conceptReplace.ConceptId;
                    currentAccountId = conceptReplace.CurrentAccountId;
                }

                if (isTaxed)
                {
                    subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                    igv = total + discount - subtotal;
                }

                payments.Add(new Payment
                {
                    Id = Guid.NewGuid(),
                    Description = description,
                    SubTotal = subtotal,
                    IgvAmount = igv,
                    Discount = discount,
                    Total = total,
                    EntityId = Guid.Empty,
                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                    UserId = student.UserId,
                    ConceptId = conceptId,
                    TermId = termId,
                    CurrentAccountId = currentAccountId
                });
            }

            var enableExtraPayment = bool.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.ENABLE_ADDITIONAL_PAYMENT_FOR_EXTRA_ACADEMIC_YEARS));
            if (enableExtraPayment)
            {
                var academicYearsEnrolled = 0;
                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
                {
                    academicYearsEnrolled = await _context.Terms
                        .CountAsync(x => student.AdmissionTermStartDate <= x.StartDate && x.Status == ConstantHelpers.TERM_STATES.FINISHED && !x.IsSummer);
                }
                else
                {
                    academicYearsEnrolled = await _context.AcademicSummaries
                        .Where(x => x.StudentId == student.Id && x.CurriculumId == student.CurriculumId
                        && !x.Term.IsSummer && !x.WasWithdrawn)
                        .CountAsync();
                }

                var curriculumAcademicYears = await _context.AcademicYearCourses.Where(x => x.CurriculumId == student.CurriculumId).OrderByDescending(x => x.AcademicYear).Select(x => x.AcademicYear).FirstOrDefaultAsync();
                var gracePeriod = int.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXTRA_ACADEMIC_YEARS_GRACE_PERIOD));

                if (academicYearsEnrolled + 1 > curriculumAcademicYears + gracePeriod)
                {
                    var conceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.ADDITIONAL_PAYMENT_FOR_EXTRA_ACADEMIC_YEARS_CONCEPT));

                    var extraEnrollmentConcepts = enrollmentConcepts.Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.EXTRA_ENROLLMENT_CONCEPT).ToList();
                    var extraConcept = extraEnrollmentConcepts
                        .OrderByDescending(x => x.CareerId)
                        .ThenByDescending(x => x.AdmissionTypeId)
                        .FirstOrDefault(x => (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                        && (!x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId));
                    if (extraConcept != null) conceptId = extraConcept.ConceptId;

                    var concept = await _context.Concepts.FindAsync(conceptId);

                    var cost = concept.Amount;

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
                    {
                        var enrolledCredits = await _context.StudentSections
                            .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termId)
                            .SumAsync(x => x.Section.CourseTerm.Course.Credits);
                        cost = concept.Amount * enrolledCredits;
                    }
                    var discount = cost * student.DiscountPercentage / 100.0M;

                    cost -= discount;
                    var subTotal = cost;
                    var igvAmount = 0.00M;

                    if (concept.IsTaxed)
                    {
                        subTotal = discount + cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                        igvAmount = cost + discount - subTotal;
                    }

                    payments.Add(new Payment
                    {
                        Description = concept.Description,
                        SubTotal = subTotal,
                        IgvAmount = igvAmount,
                        Discount = discount,
                        Total = cost,
                        EntityId = null,
                        Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                        UserId = student.UserId,
                        ConceptId = conceptId,
                        TermId = termId,
                        CurrentAccountId = concept.CurrentAccountId
                    });
                }
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH)
            {
                var lastSummary = await _context.AcademicSummaries
                    .Where(x => x.StudentId == student.Id && !x.Term.IsSummer && !x.WasWithdrawn && x.CurriculumId == student.CurriculumId)
                    .OrderByDescending(x => x.Term.Year)
                    .ThenByDescending(x => x.Term.Number)
                    .Select(x => new
                    {
                        x.WeightedAverageGrade,
                        x.Term.MinGrade
                    })
                    .FirstOrDefaultAsync();

                if (lastSummary != null && lastSummary.WeightedAverageGrade < lastSummary.MinGrade)
                {
                    //cobro concepto adicional por promedio desaprobado
                    var conceptId = new Guid("08d9ea5c-c9a7-498c-8fcd-9d00cd085913");
                    if (student.AdmissionTypeAbbrev == "GYTDNU") conceptId = new Guid("08d9ea5c-d3ce-46ac-8492-2d25baece45c");

                    var concept = await _context.Concepts.FindAsync(conceptId);
                    if (concept != null)
                    {
                        var cost = concept.Amount;
                        var discount = cost * student.DiscountPercentage / 100.0M;

                        cost -= discount;
                        var subTotal = cost;
                        var igvAmount = 0.00M;

                        if (concept.IsTaxed)
                        {
                            subTotal = discount + cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                            igvAmount = cost + discount - subTotal;
                        }

                        payments.Add(new Payment
                        {
                            Description = concept.Description,
                            SubTotal = subTotal,
                            IgvAmount = igvAmount,
                            Discount = discount,
                            Total = cost,
                            EntityId = null,
                            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId,
                            ConceptId = conceptId,
                            TermId = termId,
                            CurrentAccountId = concept.CurrentAccountId
                        });
                    }
                }
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH)
            {
                var enrolledCredits = await _context.StudentSections
                   .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termId)
                   .SumAsync(x => x.Section.CourseTerm.Course.Credits);

                if (student.AdmissionTypeAbbrev == "GYTDNU" && enrolledCredits > 0)
                {
                    var conceptId = new Guid("08d9f6ed-e520-46e5-89dd-9212aa711e25");
                    var concept = await _context.Concepts.FindAsync(conceptId);
                    if (concept != null)
                    {
                        var cost = concept.Amount * enrolledCredits;
                        var discount = cost * student.DiscountPercentage / 100.0M;

                        cost -= discount;
                        var subTotal = cost;
                        var igvAmount = 0.00M;

                        if (concept.IsTaxed)
                        {
                            subTotal = discount + cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                            igvAmount = cost + discount - subTotal;
                        }

                        payments.Add(new Payment
                        {
                            Description = $"{concept.Description} {enrolledCredits} cred.",
                            SubTotal = subTotal,
                            IgvAmount = igvAmount,
                            Discount = discount,
                            Total = cost,
                            EntityId = null,
                            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId,
                            ConceptId = conceptId,
                            TermId = termId,
                            CurrentAccountId = concept.CurrentAccountId
                        });
                    }
                }
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
            {
                var enrolledCredits = await _context.StudentSections
                   .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termId)
                   .SumAsync(x => x.Section.CourseTerm.Course.Credits);

                if (student.AdmissionTypeAbbrev == "SEGUNDA PROFESIONALIZACION" && enrolledCredits > 0)
                {
                    var conceptId = new Guid("08da01e4-216d-4ed4-8764-4756b4f2e819");
                    var concept = await _context.Concepts.FindAsync(conceptId);
                    if (concept != null)
                    {
                        var cost = concept.Amount * enrolledCredits;
                        var discount = cost * student.DiscountPercentage / 100.0M;

                        cost -= discount;
                        var subTotal = cost;
                        var igvAmount = 0.00M;

                        if (concept.IsTaxed)
                        {
                            subTotal = discount + cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                            igvAmount = cost + discount - subTotal;
                        }

                        payments.Add(new Payment
                        {
                            Description = $"{concept.Description} {enrolledCredits} cred.",
                            SubTotal = subTotal,
                            IgvAmount = igvAmount,
                            Discount = discount,
                            Total = cost,
                            EntityId = null,
                            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId,
                            ConceptId = conceptId,
                            TermId = termId,
                            CurrentAccountId = concept.CurrentAccountId
                        });
                    }
                }
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.ENSDF)
            {
                var term = await _context.Terms.FindAsync(termId);
                var isExtemporaneous = term.ComplementaryEnrollmentStartDate <= DateTime.UtcNow && DateTime.UtcNow <= term.ComplementaryEnrollmentEndDate;

                var studentCourses = await _context.StudentSections
                   .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termId)
                   .Select(x => x.Section.CourseTerm.CourseId)
                   .ToListAsync();
                var curriculumCourses = await _context.AcademicYearCourses
                    .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear != student.AcademicYear)
                    .Select(x => x.CourseId).ToListAsync();

                var additionalCourses = studentCourses.Where(x => curriculumCourses.Contains(x)).Count();

                if (additionalCourses > 0)
                {
                    var conceptId = new Guid("08db1677-f3e4-4dee-8cbf-a5176957cf86");
                    if (isExtemporaneous)
                        conceptId = new Guid("08db1677-f3e4-4e0a-8810-7c940c0fb58d");

                    var concept = await _context.Concepts.FindAsync(conceptId);
                    if (concept != null)
                    {
                        var cost = concept.Amount * additionalCourses;
                        var discount = cost * student.DiscountPercentage / 100.0M;

                        cost -= discount;
                        var subTotal = cost;
                        var igvAmount = 0.00M;

                        if (concept.IsTaxed)
                        {
                            subTotal = discount + cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                            igvAmount = cost + discount - subTotal;
                        }

                        payments.Add(new Payment
                        {
                            Description = $"{concept.Description} {additionalCourses} Cursos",
                            SubTotal = subTotal,
                            IgvAmount = igvAmount,
                            Discount = discount,
                            Total = cost,
                            EntityId = null,
                            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId,
                            ConceptId = conceptId,
                            TermId = termId,
                            CurrentAccountId = concept.CurrentAccountId
                        });
                    }
                }
            }

            var regularTerm = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2"))
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number)
                .Select(x => new
                {
                    x.Name
                })
                .FirstOrDefaultAsync();

            var lastRegularSummary = await _context.AcademicSummaries
                .Where(x => x.StudentId == student.Id && (x.Term.Number == "1" || x.Term.Number == "2") && !x.WasWithdrawn)
                .OrderByDescending(x => x.Term.StartDate)
                .Select(x => new
                {
                    TermName = x.Term.Name,
                    x.Term.StartDate,
                    WasSactioned = x.Student.AcademicHistories.Where(y => y.TermId == x.TermId).Any(y => y.Try >= 3 && !y.Approved)
                })
                .FirstOrDefaultAsync();

            if (regularTerm != null && lastRegularSummary != null && lastRegularSummary.TermName != regularTerm.Name)
            {
                var terms = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2")).ToListAsync();
                var term = await _context.Terms.FindAsync(termId);
                var calculateAcademicYearsTerms = terms.Where(x => lastRegularSummary.StartDate < x.StartDate && x.StartDate < term.StartDate).ToList();
                var calculateAcademicYears = terms.Where(x => lastRegularSummary.StartDate < x.StartDate && x.StartDate < term.StartDate).Count();

                var reservationIsRenewable = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_RENEWABLE_PER_SEMESTER));
                if (reservationIsRenewable)
                {
                    var enrollmentReservations = await _context.EnrollmentReservations
                        .Where(x => x.StudentId == student.Id)
                        .Select(x => x.TermId)
                        .ToListAsync();

                    calculateAcademicYears = terms.Where(x => lastRegularSummary.StartDate < x.StartDate && x.StartDate < term.StartDate && !enrollmentReservations.Contains(x.Id)).Count();
                }

                if (lastRegularSummary.WasSactioned) calculateAcademicYears -= 1;

                if (calculateAcademicYears > 0)
                {
                    var reentryConceptIdString = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REENTRY_COST_CONCEPT);
                    if (!string.IsNullOrEmpty(reentryConceptIdString))
                    {
                        var reentryConceptId = Guid.Parse(reentryConceptIdString);
                        var reentryConcept = await _context.Concepts.FindAsync(reentryConceptId);

                        var total = calculateAcademicYears * reentryConcept.Amount;
                        var discount = total * student.DiscountPercentage / 100.0M;

                        total -= discount;
                        var subtotal = total;
                        var igv = 0.00M;
                        if (reentryConcept.IsTaxed)
                        {
                            subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                            igv = total + discount - subtotal;
                        }

                        payments.Add(new Payment
                        {
                            Id = Guid.NewGuid(),
                            Description = reentryConcept.Description,
                            SubTotal = subtotal,
                            IgvAmount = igv,
                            Discount = discount,
                            Total = total,
                            EntityId = Guid.Empty,
                            Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId,
                            ConceptId = reentryConcept.Id,
                            TermId = term.Id,
                            CurrentAccountId = reentryConcept.CurrentAccountId
                        });
                        createRegularConcept = true;
                    }
                }
            }

            return new Tuple<List<Payment>, bool>(payments, createRegularConcept);
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public async Task<Tuple<bool, string>> MoodleCreateSection(Guid sectionId)
        {
            var isEnabled = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.General.ENABLE_MOODLE_INTEGRATION));
            var serviceUrl = await GetConfigurationValue(ConstantHelpers.Configuration.General.MOODLE_WEBSERVICE_URL);

            if (!isEnabled)
                return new Tuple<bool, string>(true, "");

            var section = await _context.Sections
                .Where(x => x.Id == sectionId)
                .Include(x => x.CourseTerm.Course)
                .Include(x => x.CourseTerm.Term)
                .FirstOrDefaultAsync();

            if (section.CourseTerm.Term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                return new Tuple<bool, string>(false, "Solo puede crearse un registro de secciones del periodo actual");

            if (section.MoodleId.HasValue && section.MoodleId != 0)
                return new Tuple<bool, string>(true, $"{section.MoodleId}");

            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wstoken", "3f3ebc39d7b565c5912be2505495811e"),
                new KeyValuePair<string, string>("wsfunction", "core_course_create_courses"),
                new KeyValuePair<string, string>("courses[0][fullname]", $"{section.CourseTerm.Course.Code}-{section.CourseTerm.Course.Name}-{section.Code}"),
                new KeyValuePair<string, string>("courses[0][shortname]", $"{section.CourseTerm.Course.ShortCode}-{section.Code}"),
                new KeyValuePair<string, string>("courses[0][categoryid]", "7"),
            };

            var requestContent = new FormUrlEncodedContent(values);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync($"{serviceUrl}", requestContent))
                //using (var response = await httpClient.PostAsync($"https://moodle-qa.lideradigital.com/webservice/rest/server.php?moodlewsrestformat=json", requestContent))
                {
                    if (!response.IsSuccessStatusCode)
                        return new Tuple<bool, string>(false, "Ocurrió un error al consultar el servicio");

                    var apiResponse = await response.Content.ReadAsStringAsync();

                    if (apiResponse.Contains("exception") && apiResponse.Contains("errorcode"))
                    {
                        var errorResponse = JsonConvert.DeserializeObject<MoodleErrorResponse>(apiResponse);
                        return new Tuple<bool, string>(false, $"{errorResponse.Exception} - {errorResponse.Message}");
                    }

                    var result = JsonConvert.DeserializeObject<List<MoodleCreateCourseResponse>>(apiResponse);

                    section.MoodleId = result.FirstOrDefault().id;
                    await _context.SaveChangesAsync();

                    return new Tuple<bool, string>(true, $"{section.MoodleId}");
                }
            }
        }

        public async Task<Tuple<bool, string>> MoodleEditSection(Guid sectionId)
        {
            var isEnabled = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.General.ENABLE_MOODLE_INTEGRATION));
            var serviceUrl = await GetConfigurationValue(ConstantHelpers.Configuration.General.MOODLE_WEBSERVICE_URL);

            if (!isEnabled) return new Tuple<bool, string>(true, "");

            var section = await _context.Sections
                .Where(x => x.Id == sectionId)
                .Select(x => new
                {
                    Section = x.Code,
                    x.CourseTerm.Course.Code,
                    x.CourseTerm.Course.Name,
                    x.CourseTerm.Course.ShortCode,
                    x.MoodleId,
                    TermStatus = x.CourseTerm.Term.Status
                }).FirstOrDefaultAsync();

            if (section.TermStatus != ConstantHelpers.TERM_STATES.ACTIVE)
                return new Tuple<bool, string>(true, "");

            if (!section.MoodleId.HasValue || section.MoodleId == 0)
                return await MoodleCreateSection(sectionId);

            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wstoken", "3f3ebc39d7b565c5912be2505495811e"),
                new KeyValuePair<string, string>("wsfunction", "core_course_update_courses"),
                new KeyValuePair<string, string>("courses[0][id]", section.MoodleId.ToString()),
                new KeyValuePair<string, string>("courses[0][fullname]",  $"{section.Code}-{section.Name}-{section.Section}"),
                new KeyValuePair<string, string>("courses[0][shortname]", $"{section.ShortCode}-{section.Section}"),
            };

            var requestContent = new FormUrlEncodedContent(values);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync($"{serviceUrl}", requestContent))
                //using (var response = await httpClient.PostAsync($"https://moodle-qa.lideradigital.com/webservice/rest/server.php?moodlewsrestformat=json", requestContent))
                {
                    if (!response.IsSuccessStatusCode)
                        return new Tuple<bool, string>(false, "Ocurrió un error al consultar el servicio");

                    var apiResponse = await response.Content.ReadAsStringAsync();

                    if (apiResponse.Contains("exception") && apiResponse.Contains("errorcode"))
                    {
                        var errorResponse = JsonConvert.DeserializeObject<MoodleErrorResponse>(apiResponse);
                        return new Tuple<bool, string>(false, $"{errorResponse.Exception} - {errorResponse.Message}");
                    }

                    var result = JsonConvert.DeserializeObject<MoodleSuccessResponse>(apiResponse);
                    //revisar warnings
                    return new Tuple<bool, string>(true, "");

                }
            }
        }

        public async Task<Tuple<bool, string>> MoodleDeleteSection(Guid sectionId)
        {
            return new Tuple<bool, string>(true, "");
        }

        public async Task<Tuple<bool, string>> MoodleCreateStudent(Guid studentId)
        {
            var isEnabled = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.General.ENABLE_MOODLE_INTEGRATION));
            var serviceUrl = await GetConfigurationValue(ConstantHelpers.Configuration.General.MOODLE_WEBSERVICE_URL);

            if (!isEnabled)
                return new Tuple<bool, string>(true, "");

            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            if (student.MoodleId.HasValue && student.MoodleId != 0)
                return new Tuple<bool, string>(true, $"{student.MoodleId}");

            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wstoken", "3f3ebc39d7b565c5912be2505495811e"),
                new KeyValuePair<string, string>("wsfunction", "core_user_create_users"),
                new KeyValuePair<string, string>("users[0][username]", student.User.UserName),
                new KeyValuePair<string, string>("users[0][password]", student.User.Document),
                new KeyValuePair<string, string>("users[0][firstname]", student.User.Name),
                new KeyValuePair<string, string>("users[0][lastname]", $"{student.User.PaternalSurname} {student.User.MaternalSurname}"),
                new KeyValuePair<string, string>("users[0][email]", student.User.Email),
            };

            var requestContent = new FormUrlEncodedContent(values);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync($"{serviceUrl}", requestContent))
                //using (var response = await httpClient.PostAsync($"https://moodle-qa.lideradigital.com/webservice/rest/server.php?moodlewsrestformat=json", requestContent))
                {
                    if (!response.IsSuccessStatusCode)
                        return new Tuple<bool, string>(false, "Ocurrió un error al consultar el servicio");

                    var apiResponse = await response.Content.ReadAsStringAsync();

                    if (apiResponse.Contains("exception") && apiResponse.Contains("errorcode"))
                    {
                        var errorResponse = JsonConvert.DeserializeObject<MoodleErrorResponse>(apiResponse);
                        return new Tuple<bool, string>(false, $"{errorResponse.Exception} - {errorResponse.Message}");
                    }

                    var result = JsonConvert.DeserializeObject<List<MoodleCreateStudentResponse>>(apiResponse);

                    student.MoodleId = result.FirstOrDefault().id;
                    await _context.SaveChangesAsync();

                    return new Tuple<bool, string>(true, $"{student.MoodleId}");
                }
            }
        }

        public async Task<Tuple<bool, string>> MoodleCreateEnrollment(Guid studentSectionId)
        {
            var isEnabled = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.General.ENABLE_MOODLE_INTEGRATION));
            if (!isEnabled) return new Tuple<bool, string>(true, "");

            var serviceUrl = await GetConfigurationValue(ConstantHelpers.Configuration.General.MOODLE_WEBSERVICE_URL);

            var moodleData = await _context.StudentSections
                .Where(x => x.Id == studentSectionId)
                .Select(x => new
                {
                    SectionId = x.SectionId,
                    StudentId = x.StudentId,
                    SectionMoodleId = x.Section.MoodleId,
                    StudentMoodleId = x.Student.MoodleId
                })
                .FirstOrDefaultAsync();

            var sectionMoodleId = moodleData.SectionMoodleId;
            var studentMoodleId = moodleData.StudentMoodleId;

            if (!moodleData.SectionMoodleId.HasValue || moodleData.SectionMoodleId == 0)
            {
                var result = await MoodleCreateSection(moodleData.SectionId);
                if (!result.Item1)
                    return result;

                sectionMoodleId = int.Parse(result.Item2);
            }

            if (!moodleData.StudentMoodleId.HasValue || moodleData.StudentMoodleId == 0)
            {
                var result = await MoodleCreateStudent(moodleData.StudentId);
                if (!result.Item1)
                    return result;

                studentMoodleId = int.Parse(result.Item2);
            }

            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wstoken", "3f3ebc39d7b565c5912be2505495811e"),
                new KeyValuePair<string, string>("wsfunction", "enrol_manual_enrol_users"),
                new KeyValuePair<string, string>("enrolments[0][roleid]", "5"),
                new KeyValuePair<string, string>("enrolments[0][courseid]", sectionMoodleId.ToString()),
                new KeyValuePair<string, string>("enrolments[0][userid]", studentMoodleId.ToString()),
            };

            var requestContent = new FormUrlEncodedContent(values);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync($"{serviceUrl}", requestContent))
                //using (var response = await httpClient.PostAsync($"https://moodle-qa.lideradigital.com/webservice/rest/server.php?moodlewsrestformat=json", requestContent))
                {
                    if (!response.IsSuccessStatusCode)
                        return new Tuple<bool, string>(false, "Ocurrió un error al consultar el servicio");

                    var apiResponse = await response.Content.ReadAsStringAsync();

                    if (apiResponse.Contains("exception") && apiResponse.Contains("errorcode"))
                    {
                        var errorResponse = JsonConvert.DeserializeObject<MoodleErrorResponse>(apiResponse);
                        return new Tuple<bool, string>(false, $"{errorResponse.Exception} - {errorResponse.Message}");
                    }

                    var result = JsonConvert.DeserializeObject<MoodleSuccessResponse>(apiResponse);
                    //revisar warnings
                    return new Tuple<bool, string>(true, "");

                }
            }
        }
    }
}