using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionExamClassroomTeacher;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class AdmissionExamClassroomTeacherRepository : Repository<AdmissionExamClassroomTeacher>, IAdmissionExamClassroomTeacherRepository
    {
        public AdmissionExamClassroomTeacherRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task DeleteByAdmissionExamClassroomId(Guid id)
        {
            var teachers = await GetByAdmissionExamClassroomId(id);
            _context.AdmissionExamClassroomTeachers.RemoveRange(teachers);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AdmissionExamClassroomTeacher>> GetByAdmissionExamClassroomId(Guid id)
        {
            return await _context.AdmissionExamClassroomTeachers.Where(x => x.AdmissionExamClassroomId == id).ToListAsync();
        }

        public async Task<List<AdmissionExamClassroomTeacher>> GetByAdmissionExamId(Guid examId)
        {
            var data = await _context.AdmissionExamClassroomTeachers
                .Where(x => x.AdmissionExamId == examId)
                .Include(x => x.User)
                .Include(x => x.AdmissionExamClassroom.Classroom)
                .ToListAsync();

            return data;
        }
        public async Task<List<ClassroomAssistanceTemplate>> GetClassroomsAssistanceData(Guid id)
        {
            var query = await _context.AdmissionExamClassroomTeachers
                .Where(x => x.AdmissionExamClassroom.AdmissionExamId == id)
               .Select(x => new ClassroomAssistanceTemplate
               {
                   id = x.AdmissionExamClassroomId,
                   classroomId = x.AdmissionExamClassroom.ClassroomId,
                   classroom = $"AULA: {x.AdmissionExamClassroom.Classroom.Description}",
                   building = x.AdmissionExamClassroom.Classroom.Building.Name,
                   campus = $"SEDE: {x.AdmissionExamClassroom.Classroom.Building.Campus.Name}",
                   teacherId = x.UserId,
                   teacherName = $"{x.User.UserName} - {x.User.FullName}",
                   PictureUrl = x.User.Picture,
                   @checked = x.Assisted,
                   isPrincipal = x.AdmissionExamClassroom.AdmissionExam.IsPrincipal,
                   isToday = x.AdmissionExamClassroom.AdmissionExam.DateEvaluation.ToLocalDateFormat() == DateTime.UtcNow.ToLocalDateFormat()
               }).ToListAsync();

            return query;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassroomsAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            var query = _context.AdmissionExamClassroomTeachers
                .Include(x => x.User)
                .Include(x => x.AdmissionExamClassroom.Classroom.Building.Campus)
                .Where(x => x.AdmissionExamClassroom.AdmissionExamId == id)
                .AsQueryable();

            var recordsFiltered = await query.CountAsync();

            if (sentParameters.RecordsPerDraw > 0)
            {
                query = query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);
            }

            var data = await query.Select(x => new ClassroomAssistanceTemplate
            {
                id = x.AdmissionExamClassroomId,
                classroomId = x.AdmissionExamClassroom.ClassroomId,
                classroom = $"AULA: {x.AdmissionExamClassroom.Classroom.Description}",
                building = x.AdmissionExamClassroom.Classroom.Building.Name,
                campus = $"SEDE: {x.AdmissionExamClassroom.Classroom.Building.Campus.Name}",
                teacherId = x.UserId,
                teacherName = $"{x.User.UserName} - {x.User.FullName}",
                PictureUrl = x.User.Picture,
                @checked = x.Assisted,
                isPrincipal = x.AdmissionExamClassroom.AdmissionExam.IsPrincipal,
                isToday = x.AdmissionExamClassroom.AdmissionExam.DateEvaluation.ToLocalDateFormat() == DateTime.UtcNow.ToLocalDateFormat(),
                ShowAssistance = x.AdmissionExamClassroom.AdmissionExam.DateEvaluation.Date < DateTime.UtcNow.Date,
            }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task SaveTeacherAssistance(Guid id, List<string> lstToAdd, List<string> lstToAvoid)
        {
            var AdmissionExamClassroomTeachers = await _context.AdmissionExamClassroomTeachers
                .Where(x => x.AdmissionExamClassroom.AdmissionExamId == id)
                .ToListAsync();

            foreach (var teacher in AdmissionExamClassroomTeachers)
            {
                if (lstToAdd.Contains(teacher.UserId))
                    teacher.Assisted = true;

                if (lstToAvoid.Contains(teacher.UserId))
                    teacher.Assisted = false;
            }

            if (lstToAdd.Count() == 0 && lstToAvoid.Count() == 0)
            {

                foreach (var item in AdmissionExamClassroomTeachers)
                {
                    item.Assisted = false;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
