using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class HistoryReferredTutoringStudentRepository : Repository<HistoryReferredTutoringStudent>, IHistoryReferredTutoringStudentRepository
    {
        public HistoryReferredTutoringStudentRepository(AkdemicContext context) : base(context) { }
        public async Task<DataTablesStructs.ReturnedData<HistoryReferredTutoringStudent>> GetAllHistoryReferredTutoringStudentDatatable(DataTablesStructs.SentParameters sentParameters,  Guid termId, string userId, Guid tutoringSessionId, Guid tutoringId)
        {
            Expression<Func<HistoryReferredTutoringStudent, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.NameAttend);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.TutoringSessionStudent.TutoringSession.StartTime);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Observation);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Problems);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.SendTime);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.SupportOfficeName);
                    break;
                default:
                    orderByPredicate = ((x) => x.SendTime);

                    break;
            }
            var query = _context.HistoryReferredTutoringStudents
                .Where(x => x.TutoringSessionStudent.TutoringSessionId == tutoringSessionId)
                   .AsNoTracking();


            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new HistoryReferredTutoringStudent
                {
                    Id = x.Id,
                    TutoringSessionStudent = new TutoringSessionStudent
                    {
                        TutoringStudentStudentId = x.TutoringSessionStudent.TutoringStudentStudentId,
                        TutoringStudentTermId = x.TutoringSessionStudent.TutoringStudentTermId,
                        TutoringSessionId = x.TutoringSessionStudent.TutoringSessionId,
                        SendTime = x.TutoringSessionStudent.SendTime,
                        SupportOffice = x.TutoringSessionStudent.SupportOfficeId.HasValue
                        ? new SupportOffice
                        {
                            Id = x.TutoringSessionStudent.SupportOffice.Id,
                            Name = x.TutoringSessionStudent.SupportOffice.Name
                        } : null,
                        TutoringStudent = new TutoringStudent
                        {
                            StudentId = x.TutoringSessionStudent.TutoringStudent.StudentId,
                            Student = new ENTITIES.Models.Generals.Student
                            {
                                Status = x.TutoringSessionStudent.TutoringStudent.Student.Status,
                                CurrentAcademicYear = x.TutoringSessionStudent.TutoringStudent.Student.CurrentAcademicYear,
                                Campus = new ENTITIES.Models.Enrollment.Campus
                                {
                                    Name = x.TutoringSessionStudent.TutoringStudent.Student.Campus.Name
                                },
                                Career = new Career
                                {
                                    Name = x.TutoringSessionStudent.TutoringStudent.Student.Career.Name
                                },
                                AdmissionTerm = new ENTITIES.Models.Enrollment.Term
                                {
                                    Name = x.TutoringSessionStudent.TutoringStudent.Student.AdmissionTerm.Name
                                },
                                User = new ApplicationUser
                                {
                                    Name = x.TutoringSessionStudent.TutoringStudent.Student.User.Name,
                                    PaternalSurname = x.TutoringSessionStudent.TutoringStudent.Student.User.PaternalSurname,
                                    MaternalSurname = x.TutoringSessionStudent.TutoringStudent.Student.User.MaternalSurname,
                                    FullName = x.TutoringSessionStudent.TutoringStudent.Student.User.FullName,
                                    Dni = x.TutoringSessionStudent.TutoringStudent.Student.User.Dni,
                                    UserName = x.TutoringSessionStudent.TutoringStudent.Student.User.UserName,
                                    Picture = x.TutoringSessionStudent.TutoringStudent.Student.User.Picture,
                                    PhoneNumber = x.TutoringSessionStudent.TutoringStudent.Student.User.PhoneNumber
                                }
                            }
                        },

                        TutoringSession = new TutoringSession
                        {
                            StartTime = x.TutoringSessionStudent.TutoringSession.StartTime,
                            EndTime = x.TutoringSessionStudent.TutoringSession.EndTime,
                            IsDictated = x.TutoringSessionStudent.TutoringSession.IsDictated,
                            Classroom = new ENTITIES.Models.Enrollment.Classroom
                            {
                                Description = x.TutoringSessionStudent.TutoringSession.Classroom.Description,
                                Building = new ENTITIES.Models.Enrollment.Building
                                {
                                    Name = x.TutoringSessionStudent.TutoringSession.Classroom.Building.Name,
                                    Campus = new ENTITIES.Models.Enrollment.Campus
                                    {
                                        Name = x.TutoringSessionStudent.TutoringSession.Classroom.Building.Campus.Name
                                    }
                                }
                            },
                            Tutor = new Tutor
                            {
                                    User = new ApplicationUser
                                    {
                                        Name = x.TutoringSessionStudent.TutoringSession.Tutor.User.Name,
                                        MaternalSurname = x.TutoringSessionStudent.TutoringSession.Tutor.User.PaternalSurname,
                                        PaternalSurname = x.TutoringSessionStudent.TutoringSession.Tutor.User.MaternalSurname,
                                        FullName = x.TutoringSessionStudent.TutoringSession.Tutor.User.FullName
                                    }
                            }
                        }
                    },
                    NameAttend = x.NameAttend,
                    SendTime = x.SendTime,
                    Rol = x.Rol,
                    SupportOfficeName = x.SupportOfficeName,
                    Observation = x.Observation,
                    Problems = x.Problems
                    
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<HistoryReferredTutoringStudent>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<HistoryReferredTutoringStudent> GetWithData(Guid id)
                => await _context.HistoryReferredTutoringStudents.Include(x => x.TutoringSessionStudent).ThenInclude(x => x.TutoringSession).Where(x => x.Id == id).FirstOrDefaultAsync();
    }
}
