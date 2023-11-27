using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TermInform;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class TermInformRepository : Repository<TermInform>, ITermInformRepository
    {
        public TermInformRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyTermInformTeacher(Guid id)
            => await _context.TermInforms.Where(x => x.Id == id).AnyAsync(x => x.TeacherTermInform.Any());
        public async Task<DataTablesStructs.ReturnedData<object>> GetTermInformDatatable(DataTablesStructs.SentParameters parameters, Guid? termId)
        {
            Expression<Func<TermInform, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                case "1":
                    orderByPredicate = ((x) => x.Term.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Type); break;
                case "3":
                    orderByPredicate = ((x) => x.DateStart); break;
                case "4":
                    orderByPredicate = ((x) => x.DateEnd); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var query = _context.TermInforms.AsNoTracking();

            if (!termId.HasValue || termId == Guid.Empty)
                termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId);

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    term = x.Term.Name,
                    type = ConstantHelpers.TERM_INFORM.TYPE.VALUES[x.Type],
                    requestType = ConstantHelpers.TERM_INFORM.REQUEST_TYPE.VALUES[x.RequestType],
                    start = x.DateStart.ToLocalDateFormat(),
                    end = x.DateEnd.ToLocalDateFormat(),
                    anyInform = x.TeacherTermInform.Any()
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<TermInformTemplate> GetTermInformTemplate(string teacherId, byte type)
        {
            var user = await _context.Users.Where(x => x.Id == teacherId).FirstOrDefaultAsync();
            var termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();
            var termInform = await _context.TermInforms.Where(x => x.TermId == termId && x.Type == type).FirstOrDefaultAsync();
            var informsSent = await _context.TeacherTermInforms.Where(x => x.TermInform.TermId == termId && x.TermInform.Type == type && x.TeacherId == user.Id).ToListAsync();

            var sections = await _context.Sections
            .Where(x => x.CourseTerm.TermId == termId && x.TeacherSections.Any(y => y.TeacherId == teacherId))
            .Select(x => new
            {
                course = x.CourseTerm.Course.Name,
                section = x.Code,
                sectionId = x.Id
            })
            .ToListAsync();

            var any = await _context.TermInforms.AnyAsync(x => x.TermId == termId);

            if (termInform is null)
            {
                return new TermInformTemplate
                {
                    AnyByTerm = any,
                    HasError = true,
                    Details = new List<TeacherTermInfoTemplate>()
                };
            }

            var model = new TermInformTemplate
            {
                Id = termInform.Id,
                DateStart = termInform.DateStart.ToLocalDateFormat(),
                DateEnd = termInform.DateEnd.ToLocalDateFormat(),
                Type = ConstantHelpers.TERM_INFORM.TYPE.VALUES[termInform.Type],
                RequestType = ConstantHelpers.TERM_INFORM.REQUEST_TYPE.VALUES[termInform.RequestType],
                IsBySection = termInform.RequestType == ConstantHelpers.TERM_INFORM.REQUEST_TYPE.BYSECTION ? true : false,
                AnyByTerm = any
            };

            if (termInform.RequestType == ConstantHelpers.TERM_INFORM.REQUEST_TYPE.BYSECTION)
            {
                model.Details = sections
                    .Where(x => !informsSent.Any(y => y.SectionId == x.sectionId))
                .Select(x => new TeacherTermInfoTemplate
                {
                    Course = x.course,
                    Section = x.section,
                    SectionId = x.sectionId,
                }).ToList();
            }
            else
            {
                model.Details = new List<TeacherTermInfoTemplate>();

                if (!informsSent.Any())
                {
                    model.Details.Add(
                        new TeacherTermInfoTemplate
                        {
                            TeacherId = user.Id,
                            Teacher = user.FullName
                        }
                    );
                }
            }

            if (!model.Details.Any())
                model.Completed = true;

            return model;
        }
        public async Task<bool> AnyByType(Guid termId, byte type)
            => await _context.TermInforms.AnyAsync(x => x.TermId == termId && x.Type == type);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTermInformReportDatatable(DataTablesStructs.SentParameters parameters, Guid termId, byte type, string searchValue)
        {

            var termInform = await _context.TermInforms.Where(x => x.TermId == termId && x.Type == type).FirstOrDefaultAsync();

            if (termInform != null)
            {
                if (termInform.RequestType == ConstantHelpers.TERM_INFORM.REQUEST_TYPE.BYSECTION)
                {
                    var queryTeacherSections = _context.TeacherSections.Where(x => x.Section.CourseTerm.TermId == termId);

                    if (!string.IsNullOrEmpty(searchValue))
                        queryTeacherSections = queryTeacherSections.Where(x => x.Teacher.User.FullName.ToLower().Contains(searchValue.ToLower()));

                    int recordsFiltered = await queryTeacherSections.CountAsync();
                    var data = await queryTeacherSections
                        .Skip(parameters.PagingFirstRecord)
                        .Take(parameters.RecordsPerDraw)
                        .Select(x => new
                        {
                            section = x.Section.Code,
                            code = x.Section.CourseTerm.Course.Code,
                            course = x.Section.CourseTerm.Course.Name,
                            teacher = x.Teacher.User.FullName,
                            teacherId = x.TeacherId,
                            sectionId = x.SectionId,
                            termInformId = termInform.Id,
                            term = _context.TeacherTermInforms.Where(y=>y.TermInform.TermId == termId && y.TermInform.Type == type && y.TeacherId == x.TeacherId && y.SectionId == x.SectionId)
                            .Select(y=> new
                            {
                                y.Id,
                                createdAt = y.CreatedAt.ToLocalDateTimeFormat(),
                                y.Url
                            })
                            .FirstOrDefault()
                        })
                        .ToListAsync();

                    int recordsTotal = data.Count;

                    return new DataTablesStructs.ReturnedData<object>
                    {
                        Data = data,
                        DrawCounter = parameters.DrawCounter,
                        RecordsFiltered = recordsFiltered,
                        RecordsTotal = recordsTotal
                    };
                }
                else if (termInform.RequestType == ConstantHelpers.TERM_INFORM.REQUEST_TYPE.BYTEACHER)
                {
                    var queryTeachers = _context.Teachers.Where(x => x.TeacherSections.Any(y=>y.Section.CourseTerm.TermId == termId));

                    if (!string.IsNullOrEmpty(searchValue))
                        queryTeachers = queryTeachers.Where(x => x.User.FullName.ToLower().Contains(searchValue.ToLower()));

                    int recordsFiltered = await queryTeachers.CountAsync();
                    var data = await queryTeachers
                        .Skip(parameters.PagingFirstRecord)
                        .Take(parameters.RecordsPerDraw)
                        .Select(x => new
                        {
                            section = "-",
                            teacherId = x.UserId,
                            code = "-",
                            course = "-",
                            teacher = x.User.FullName,
                            termInformId = termInform.Id,
                            inform = _context.TeacherTermInforms.Where(y => y.TermInform.TermId == termId && y.TermInform.Type == type && y.TeacherId == x.UserId)
                            .Select(y => new
                            {
                                y.Id,
                                createdAt = y.CreatedAt.ToLocalDateTimeFormat(),
                                y.Url
                            })
                            .FirstOrDefault()
                        })
                        .ToListAsync();

                    int recordsTotal = data.Count;

                    return new DataTablesStructs.ReturnedData<object>
                    {
                        Data = data,
                        DrawCounter = parameters.DrawCounter,
                        RecordsFiltered = recordsFiltered,
                        RecordsTotal = recordsTotal
                    };
                }
            }
            else
            {
                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = new List<object>(),
                    DrawCounter = parameters.DrawCounter,
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                };
            }

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = new List<object>(),
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };
        }

        public async Task<object> GetByFilters(Guid termId, byte type)
        {
            var result = await _context.TermInforms.Where(x => x.TermId == termId && x.Type == type)
                .Select(x => new
                {
                    x.Id,
                    createdAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    start = x.DateStart.ToLocalDateFormat(),
                    end = x.DateEnd.ToLocalDateFormat(),
                    type = ConstantHelpers.TERM_INFORM.TYPE.VALUES[x.Type],
                    requestType = ConstantHelpers.TERM_INFORM.REQUEST_TYPE.VALUES[x.RequestType]
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}