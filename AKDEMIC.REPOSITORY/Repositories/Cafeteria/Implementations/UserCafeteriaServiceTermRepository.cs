using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class UserCafeteriaServiceTermRepository : Repository<UserCafeteriaServiceTerm>, IUserCafeteriaServiceTermRepository
    {
        public UserCafeteriaServiceTermRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyTermActiveByStudent(Guid studentId)
        {
            var cafeteriaServiceTerm = await _context.CafeteriaServiceTerms
                                .Where(x => x.IsActive)
                                .FirstOrDefaultAsync();

            return await _context.UserCafeteriaServiceTerms.AnyAsync(x => x.CafeteriaServiceTermId == cafeteriaServiceTerm.Id && x.StudentId == studentId);
        }

        public async Task<UserCafeteriaServiceTerm> GetByTermActiveAndStudent(Guid studentId)
        {
            var cafeteriaServiceTerm = await _context.CafeteriaServiceTerms
                                .Where(x => x.IsActive)
                                .FirstOrDefaultAsync();

            var query = _context.UserCafeteriaServiceTerms
                .Where(x => x.CafeteriaServiceTermId == cafeteriaServiceTerm.Id && x.StudentId == studentId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaStudentsWithServiceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null)
        {
            Expression<Func<UserCafeteriaServiceTerm, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.StudentId;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.StudentId;
                    break;
            }
            //Usuarios que esten en la tabla de UserCafeteriaServiceTerms
            var cafeteriaServiceTermId = await _context.CafeteriaServiceTerms.Where(x => (x.DateBegin <= DateTime.UtcNow && x.DateEnd > DateTime.UtcNow)).Select(x => x.Id).FirstOrDefaultAsync();

            var query = _context.UserCafeteriaServiceTerms
                .Where(x => x.CafeteriaServiceTermId == cafeteriaServiceTermId)
                .AsNoTracking();

            if (facultyId != null)
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    x.Student.User.Email,
                    Career = x.Student.Career.Name,
                })
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaStudentsWithServiceForAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null)
        {
            //Usuarios que esten en la tabla de UserCafeteriaServiceTerms
            var cafeteriaServiceTerm = await _context.CafeteriaServiceTerms.Where(x => (x.DateBegin <= DateTime.UtcNow && x.DateEnd > DateTime.UtcNow)).FirstOrDefaultAsync();

            var today = ((int)DateTime.Now.DayOfWeek - 1 == 6 ? 0 : (int)DateTime.Now.DayOfWeek - 1);
            var timeOfDay = DateTime.UtcNow.TimeOfDay;

            var week = await _context.CafeteriaWeeklyScheduleTurnDetails.Where(x =>
                              x.CafeteriaWeeklySchedule.CafeteriaServiceTermSchedule.CafeteriaServiceTermId == cafeteriaServiceTerm.Id &&
                              (x.StartTime <= timeOfDay && x.EndTime > timeOfDay)).FirstOrDefaultAsync();


            //usuarios que asistieron al comedor en la fecha actual, tipo deseado /almuerzo por defecto
            //var query = _context.UserCafeteriaDailyAssistances
            //    .Where(x =>
            //                 x.UserCafeteriaServiceTerm.CafeteriaServiceTermId == cafeteriaServiceTerm.Id &&
            //                 x.CafeteriaWeeklyScheduleTurnDetail.CafeteriaWeeklySchedule.CafeteriaServiceTermSchedule.DateBegin <= DateTime.UtcNow &&
            //                 x.CafeteriaWeeklyScheduleTurnDetail.CafeteriaWeeklySchedule.CafeteriaServiceTermSchedule.DateEnd >= DateTime.UtcNow &&
            //                 x.CafeteriaWeeklyScheduleTurnDetail.CafeteriaWeeklySchedule.DayOfWeek == today &&
            //                 x.CafeteriaWeeklyScheduleTurnDetail.StartTime.ToLocalTimeSpanUtc() <= DateTime.UtcNow.TimeOfDay &&
            //                 x.CafeteriaWeeklyScheduleTurnDetail.EndTime.ToLocalTimeSpanUtc() >= DateTime.UtcNow.TimeOfDay ).AsQueryable();

            var query = _context.UserCafeteriaServiceTerms.Where(x => x.CafeteriaServiceTermId == cafeteriaServiceTerm.Id &&
                        x.CafeteriaServiceTerm.CafeteriaServiceTermSchedules.Any(y => y.DateBegin <= DateTime.UtcNow && y.DateEnd >= DateTime.UtcNow)
            ).AsQueryable();

            if (facultyId != null)
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));


            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();

            //CafeteriaWeeklySchedules actual /para el dia de hoy


            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    x.Student.User.Email,
                    x.Student.User.Document,
                    Career = x.Student.Career.Name,
                    WeekId = week.Id,
                    hasAssistance = _context.UserCafeteriaDailyAssistances.Any(y => y.UserCafeteriaServiceTermId == x.Id && y.CafeteriaWeeklyScheduleTurnDetailId == week.Id)
                })
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

        public async Task<object> GetCafeteriaStudentsWithServiceForAssistanceDatatableClientSide()
        {
            //Usuarios que esten en la tabla de UserCafeteriaServiceTerms
            var cafeteriaServiceTerm = await _context.CafeteriaServiceTerms.Where(x => (x.DateBegin <= DateTime.UtcNow && x.DateEnd > DateTime.UtcNow)).FirstOrDefaultAsync();

            var today = ((int)DateTime.Now.DayOfWeek - 1 == 6 ? 0 : (int)DateTime.Now.DayOfWeek - 1);
            var timeOfDay = DateTime.UtcNow.TimeOfDay;

            var week = await _context.CafeteriaWeeklyScheduleTurnDetails.Where(x =>
                              x.CafeteriaWeeklySchedule.CafeteriaServiceTermSchedule.CafeteriaServiceTermId == cafeteriaServiceTerm.Id &&
                              (x.StartTime <= timeOfDay && x.EndTime > timeOfDay)).FirstOrDefaultAsync();

            var query = _context.UserCafeteriaServiceTerms.Where(x => x.CafeteriaServiceTermId == cafeteriaServiceTerm.Id &&
                        x.CafeteriaServiceTerm.CafeteriaServiceTermSchedules.Any(y => y.DateBegin <= DateTime.UtcNow && y.DateEnd >= DateTime.UtcNow)
            ).AsQueryable();

            var data = await query
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    x.Student.User.Email,
                    x.Student.User.Document,
                    Career = x.Student.Career.Name,
                    WeekId = week.Id,
                    hasAssistance = _context.UserCafeteriaDailyAssistances.Any(y => y.UserCafeteriaServiceTermId == x.Id && y.CafeteriaWeeklyScheduleTurnDetailId == week.Id),
                    isAbsent = _context.UserCafeteriaDailyAssistances.Where(y => y.UserCafeteriaServiceTermId == x.Id && y.CafeteriaWeeklyScheduleTurnDetailId == week.Id).Select(y => y.IsAbsent).FirstOrDefault(),
                })
                .ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetHistoricAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? weekId, int dayId, int turn, string searchValue = null, Guid? facultyId = null, Guid? careerId = null)
        {
            Expression<Func<UserCafeteriaServiceTerm, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.StudentId;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.StudentId;
                    break;
            }


            var week = new CafeteriaServiceTermSchedule();
            var days = new List<CafeteriaWeeklySchedule>();
            var assistances = new List<UserCafeteriaDailyAssistance>();

            var turnDetail = new CafeteriaWeeklyScheduleTurnDetail();

            //periodo activo
            //var term = await _context.CafeteriaServiceTerms.Where(x => x.IsActive).FirstOrDefaultAsync();


            if (weekId.HasValue)
            {
                week = await _context.CafeteriaServiceTermSchedules.FirstOrDefaultAsync(x => x.Id == weekId.Value);

                var day = await _context.CafeteriaWeeklySchedules.FirstOrDefaultAsync(x => x.CafeteriaServiceTermScheduleId == week.Id && x.DayOfWeek == dayId);
                if (day != null)
                {
                    turnDetail = await _context.CafeteriaWeeklyScheduleTurnDetails.Where(x => x.Type == turn && x.CafeteriaWeeklySchedule.DayOfWeek == day.DayOfWeek).FirstOrDefaultAsync();

                    if (turnDetail != null)
                    {
                        assistances = await _context.UserCafeteriaDailyAssistances.Where(x => x.CafeteriaWeeklyScheduleTurnDetailId == turnDetail.Id).ToListAsync();
                    }
                }

            }

            var userCafeteriaServiceTermIds = assistances.Select(x => x.UserCafeteriaServiceTermId).ToArray();

            //usuarios con beneficio de comedor
            var query2 = _context.UserCafeteriaServiceTerms
                .Include(x => x.Student)
                .Include(x => x.CafeteriaServiceTerm)
                .Where(x => userCafeteriaServiceTermIds.Contains(x.Id))
                //.Where(x => assistances.Any(c => c.UserCafeteriaServiceTermId == x.Id))
                .AsNoTracking();

            if (facultyId != null)
                query2 = query2.Where(x => x.Student.Career.FacultyId == facultyId);

            if (careerId != null)
                query2 = query2.Where(x => x.Student.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
                query2 = query2.Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = 0;

            recordsFiltered = await query2.CountAsync();

            var data = await query2
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    x.Student.User.Email,
                    Career = x.Student.Career.Name,
                    WeekId = week.Id
                })
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetHistoricAssistanceByDateDatatable(DataTablesStructs.SentParameters sentParameters, DateTime date, int turn, string searchValue)
        {

            var schedules = await _context.CafeteriaServiceTermSchedules
                .Where(x => x.DateBegin.Date <= date.Date && x.DateEnd.Date >= date.Date).Select(x => x.Id)
                .ToListAsync();

            var week = await _context.CafeteriaWeeklySchedules.Where(x => schedules.Contains(x.CafeteriaServiceTermScheduleId) && x.DayOfWeek == ConstantHelpers.WEEKDAY.ENUM_TO_INT(date.DayOfWeek)).FirstOrDefaultAsync();

            if(week == null)
            {
                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = new List<object>(),
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                };
            }

            var turnDetail = await _context.CafeteriaWeeklyScheduleTurnDetails.Where(x => x.CafeteriaWeeklyScheduleId == week.Id && x.Type == turn).FirstOrDefaultAsync();

            if (turnDetail == null)
            {
                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = new List<object>(),
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                };
            }

            var query = _context.UserCafeteriaDailyAssistances.Where(x => x.CafeteriaWeeklyScheduleTurnDetailId == turnDetail.Id ).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.UserCafeteriaServiceTerm.Student.User.UserName.ToLower().Trim().Contains(searchValue.Trim().ToLower()) || x.UserCafeteriaServiceTerm.Student.User.FullName.ToLower().Trim().Contains(searchValue.Trim().ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.UserCafeteriaServiceTerm.Student.User.UserName,
                    x.UserCafeteriaServiceTerm.Student.User.FullName,
                    x.UserCafeteriaServiceTerm.Student.User.Email,
                    Career = x.UserCafeteriaServiceTerm.Student.Career.Name,
                    x.IsAbsent
                })
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

        public async Task SaveStudentList(Guid? careerId, bool isCheckAll, List<Guid> lstToAdd, List<Guid> lstToAvoid)
        {
            var QueryStudent = _context.Students.AsQueryable();
            var ActiveServiceTerm = await _context.CafeteriaServiceTerms.Where(x => (x.DateBegin <= DateTime.UtcNow && x.DateEnd > DateTime.UtcNow)).FirstOrDefaultAsync();
            var UserCafeteriaList = new List<UserCafeteriaServiceTerm>();
            var QueryStudentList = new List<Student>();
            if (careerId.HasValue)
            {
                QueryStudent = QueryStudent.Where(x => x.CareerId == careerId);
            }
            if (isCheckAll)
            {
                var array2 = await _context.UserCafeteriaServiceTerms.Where(x => x.CafeteriaServiceTermId == ActiveServiceTerm.Id).ToListAsync();
                QueryStudentList = await QueryStudent.Where(x => !lstToAvoid.Contains(x.Id)).ToListAsync();
                if (array2.Count > 0)
                {
                    for (int i = 0; i < QueryStudentList.Count; i++)
                    {
                        if (array2.Any(x => x.StudentId == QueryStudentList[i].Id))
                            continue;

                        var UserCafeteria = new UserCafeteriaServiceTerm();
                        UserCafeteria.CafeteriaServiceTermId = ActiveServiceTerm.Id;
                        UserCafeteria.StudentId = QueryStudentList[i].Id;
                        UserCafeteriaList.Add(UserCafeteria);

                    }
                }
                else
                {
                    for (int i = 0; i < QueryStudentList.Count; i++)
                    {
                        var UserCafeteria = new UserCafeteriaServiceTerm();
                        UserCafeteria.CafeteriaServiceTermId = ActiveServiceTerm.Id;
                        UserCafeteria.StudentId = QueryStudentList[i].Id;
                        UserCafeteriaList.Add(UserCafeteria);
                    }
                }

                await _context.UserCafeteriaServiceTerms.AddRangeAsync(UserCafeteriaList);
            }
            else
            {
                var array = await _context.UserCafeteriaServiceTerms.Where(x => x.CafeteriaServiceTermId == ActiveServiceTerm.Id).ToListAsync();
                if (lstToAdd.Count > 0)
                {
                    if (array.Count > 0)
                    {
                        for (int i = 0; i < lstToAdd.Count; i++)
                        {
                            if (array.Any(x => x.StudentId == lstToAdd[i]))
                                continue;
                            var UserCafeteria = new UserCafeteriaServiceTerm();
                            UserCafeteria.CafeteriaServiceTermId = ActiveServiceTerm.Id;
                            UserCafeteria.StudentId = lstToAdd[i];
                            UserCafeteriaList.Add(UserCafeteria);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < lstToAdd.Count; i++)
                        {

                            var UserCafeteria = new UserCafeteriaServiceTerm();
                            UserCafeteria.CafeteriaServiceTermId = ActiveServiceTerm.Id;
                            UserCafeteria.StudentId = lstToAdd[i];
                            UserCafeteriaList.Add(UserCafeteria);


                        }
                    }
                    await _context.UserCafeteriaServiceTerms.AddRangeAsync(UserCafeteriaList);
                }
                if (lstToAvoid.Count > 0)
                {
                    for (int i = 0; i < lstToAvoid.Count; i++)
                    {

                        var entity = array.Where(x => x.StudentId == lstToAvoid[i]).FirstOrDefault();
                        UserCafeteriaList.Add(entity);
                    }
                    _context.UserCafeteriaServiceTerms.RemoveRange(UserCafeteriaList);
                }

            }

            await _context.SaveChangesAsync();
        }
    }
}
