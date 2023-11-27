using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public sealed class SubstituteExamRepository : Repository<SubstituteExam>, ISubstituteExamRepository
    {
        public SubstituteExamRepository(AkdemicContext context) : base(context) { }

        public override async Task<SubstituteExam> Get(Guid id)
        {
            return await _context.SubstituteExams
                           .Include(x => x.Section.CourseTerm.Course)
                           .Include(x => x.Student.User)
                           .Where(x => x.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<bool> AnyByCourseIdTermIdAndStudentId(Guid courseId, Guid termId, Guid studentId, Guid? id = null)
        {
            if (id.HasValue)
                await _context.SubstituteExams.AnyAsync(x => x.Section.CourseTerm.CourseId == courseId && x.Section.CourseTerm.TermId == termId && x.StudentId == studentId && x.Id != id.Value);

            return await _context.SubstituteExams.AnyAsync(x => x.Section.CourseTerm.CourseId == courseId && x.Section.CourseTerm.TermId == termId && x.StudentId == studentId);
        }
        public async Task<bool> AnyByCourseTermIdAndStudentId(Guid courseTermId, Guid studentId)
        {
            return await _context.SubstituteExams.AnyAsync(x => x.Section.CourseTermId == courseTermId && x.StudentId == studentId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatableByFilters(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, Guid? courseId = null, Guid? sectionId = null, byte? status = null)
        {
            var examQuery = _context.SubstituteExams.Where(x => x.Status != ConstantHelpers.SUBSTITUTE_EXAM_STATUS.DELETED).AsNoTracking();

            if (!string.IsNullOrEmpty(teacherId))
                examQuery = examQuery.Where(x => x.Section.TeacherSections.Any(y => y.TeacherId == teacherId));

            if (termId.HasValue && termId.Value != Guid.Empty)
                examQuery = examQuery.Where(x => x.Section.CourseTerm.TermId == termId);

            if (courseId.HasValue && courseId.Value != Guid.Empty)
                examQuery = examQuery.Where(x => x.Section.CourseTerm.CourseId == courseId);

            if (sectionId.HasValue && sectionId.Value != Guid.Empty)
                examQuery = examQuery.Where(x => x.Id == sectionId);

            if (status.HasValue)
                examQuery = examQuery.Where(x => x.Status == status);

            var exams = examQuery.Select(
                    x => new
                    {
                        id = x.Id,
                        courseName = x.Section.CourseTerm.Course.Name,
                        sectionCode = x.Section.Code,
                        studentName = x.Student.User.FullName,
                        status = x.Status
                    }
                );

            return await exams.ToDataTables<object>(sentParameters);
        }

        async Task<DataTablesStructs.ReturnedData<object>> ISubstituteExamRepository.GetAcademicHistoryDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId)
        {
            var query = _context.SubstituteExams.AsQueryable();

            if (termId.HasValue)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            var exams = query
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Student.User.UserName,
                        x.Student.User.FullName,
                        CourseName = x.Section.CourseTerm.Course.Name,
                        Status = x.Status
                    }
                );

            return await exams.ToDataTables<object>(sentParameters);
        }

        async Task<object> ISubstituteExamRepository.GetAsModelA(Guid? id)
        {
            var query = _context.SubstituteExams.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id);

            var exam = await query
                .Select(
                    x => new
                    {
                        x.Id,
                        x.StudentId,
                        StudentName = $"{x.Student.User.UserName} - {x.Student.User.FullName}",
                        x.Section.CourseTerm.CourseId,
                        x.PaymentReceipt,
                        x.Underpin
                    }
                )
                .FirstOrDefaultAsync();

            return exam;
        }

        public async Task<SubstituteExam> GetSubstituteExamByStudentId(Guid studentId)
        {
            var SubstituteExamsStudient = await _context.SubstituteExams
                .Where(x => x.StudentId == studentId).FirstOrDefaultAsync();

            return SubstituteExamsStudient;
        }

        public async Task<SubstituteExam> GetSubstituteExamByStudentAndSectionId(Guid studentId, Guid sectionId)
            => await _context.SubstituteExams.Where(x => x.StudentId == studentId && x.SectionId == sectionId).FirstOrDefaultAsync();
        public async Task<SubstituteExam> GetSubstituteExamByStudentIdAndCourse(Guid studentId, Guid courseId)
            => await _context.SubstituteExams.Where(x => x.StudentId == studentId && x.CourseTerm.CourseId == courseId).FirstOrDefaultAsync();
        public async Task DeleteAllByCourseTerm(Guid courseTermId)
        {
            var exams = await _context.SubstituteExams.Where(x => x.Section.CourseTermId == courseTermId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED).ToListAsync();

            exams.ForEach(item =>
            {
                item.Status = ConstantHelpers.SUBSTITUTE_EXAM_STATUS.DELETED;
            });

            await _context.SaveChangesAsync();
        }

        public override async Task InsertRange(IEnumerable<SubstituteExam> substituteExams)
        {
            foreach (var item in substituteExams)
            {
                if (!await _context.SubstituteExams.AnyAsync(x => x.Section.CourseTermId == item.Section.CourseTermId && x.StudentId == item.StudentId))
                {
                    await _context.SubstituteExams.AddAsync(item);
                }
                else
                {
                    var exam = await _context.SubstituteExams.FirstOrDefaultAsync(x => x.Section.CourseTermId == item.Section.CourseTermId && x.StudentId == item.StudentId);
                    exam.Status = item.Status;
                }
            }

            await _context.SaveChangesAsync();
        }

        public override async Task Insert(SubstituteExam substituteExams)
        {
            var exam = await _context.SubstituteExams.FirstOrDefaultAsync(x => x.SectionId == substituteExams.SectionId && x.StudentId == substituteExams.StudentId);
            exam.Status = substituteExams.Status;

            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetExamScoreByCourseAndTermAndStudent(Guid courseId, Guid termId, Guid studentId)
        {
            return await _context.SubstituteExams.Where(x => x.Section.CourseTerm.CourseId == courseId && x.Section.CourseTerm.TermId == termId && x.StudentId == studentId).Select(x => x.ExamScore).FirstOrDefaultAsync();
        }

        public async Task SaveStudentsForSubstituteExam(Guid termid, Guid sectionId, bool isCheckAll, List<Guid> lstToAdd, List<Guid> lstToAvoid)
        {
            if (!isCheckAll)
            {
                if (lstToAdd.Count() == 0 && lstToAvoid.Count() == 0)
                {
                    await ChangeStatus(sectionId, ConstantHelpers.SUBSTITUTE_EXAM_STATUS.DELETED);
                }
                else
                {
                    foreach (var item in lstToAdd)
                    {
                        var exam = await _context.SubstituteExams.FirstOrDefaultAsync(x => x.SectionId == sectionId && x.StudentId == item && x.Status != ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED);
                        if (exam != null)
                            exam.Status = ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED;
                    }
                    foreach (var item in lstToAvoid)
                    {
                        var exam = await _context.SubstituteExams.FirstOrDefaultAsync(x => x.SectionId == sectionId && x.StudentId == item && x.Status != ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED);
                        if (exam != null)
                            exam.Status = ConstantHelpers.SUBSTITUTE_EXAM_STATUS.DELETED;
                    }
                }
            }
            else
            {
                await ChangeStatus(sectionId, ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED);
            }

            await _context.SaveChangesAsync();
        }

        private async Task ChangeStatus(Guid sectionId, byte status)
        {
            var courseterm = await _context.SubstituteExams.Where(x => x.SectionId == sectionId && x.Status != ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).ToListAsync();
            foreach (var item in courseterm)
            {
                item.Status = status;//ConstantHelpers.SUBSTITUTE_EXAM_STATUS.DELETED;
            }
        }

        public async Task<bool> ChangeCourseTermIdToSectionId()
        {
            var exams = await _context.SubstituteExams.Where(x => x.SectionId == null).ToListAsync();

            //secciones agregados ayer 
            //por cada seccion que cumpla la condicion de ayer los activo
            //


            foreach (var item in exams)
            {
                var section = await _context.StudentSections.Where(x => x.Section.CourseTermId == item.CourseTermId && x.StudentId == item.StudentId).FirstOrDefaultAsync();
                if (section != null)
                {
                    item.SectionId = section.Id;
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AnyBySectionId(Guid sectionId)
        {
            return await _context.SubstituteExams.AnyAsync(x => x.SectionId == sectionId);
        }

        public async Task<bool> AnySubstituteExamByStudent(Guid studentId, Guid sectionId, byte status)
            => await _context.SubstituteExams.AnyAsync(x => x.StudentId == studentId && x.SectionId == sectionId && x.Status == status);
    }
}