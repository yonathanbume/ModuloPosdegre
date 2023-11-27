using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryPostulantRepository : Repository<PreuniversitaryPostulant>, IPreuniversitaryPostulantRepository
    {
        public PreuniversitaryPostulantRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue)
        {
            var query = _context.PreuniversitaryPostulants.Where(x => x.PreuniversitaryTermId == termId && x.IsPaid).AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Contains(searchValue) || x.MaternalSurname.Contains(searchValue) || x.PaternalSurname.Contains(searchValue) ||
                x.Document.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      id = x.Id,
                      name = x.Name,
                      paternalName = x.PaternalSurname,
                      maternalName = x.MaternalSurname,
                      dni = x.Document
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetGradesByStudent(Guid pid)
        {
            var postulant = await _context.PreuniversitaryPostulants.Where(x => x.Id == pid).FirstOrDefaultAsync();
            var user = await _context.Users.Where(x => x.Dni == postulant.Document).FirstOrDefaultAsync();
            var userGroups = await _context.PreuniversitaryUserGroups.Include(x => x.PreuniversitaryGroup.PreuniversitaryCourse)
                .Where(x => x.ApplicationUserId == user.Id).Select(x => new
                {
                    groupCode = x.PreuniversitaryGroup.Code,
                    courseCode = x.PreuniversitaryGroup.PreuniversitaryCourse.Code,
                    courseName = x.PreuniversitaryGroup.PreuniversitaryCourse.Name,
                    grade = x.Grade
                }).ToListAsync();

            return userGroups;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null)
        {
            var query = _context.PreuniversitaryPostulants
                .Where(x => x.PreuniversitaryTermId == termId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x =>
                    x.Name.Contains(searchValue) ||
                    x.MaternalSurname.Contains(searchValue) ||
                    x.PaternalSurname.Contains(searchValue) ||
                    x.Document.Contains(searchValue) ||
                    x.Career.Name.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      id = x.Id,
                      fullName = x.FullName,
                      dni = x.Document,
                      career = x.Career.Name,
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentDetailsDatatable(DataTablesStructs.SentParameters sentParameters, Guid guid)
        {
            var query = _context.PreuniversitaryPostulants.AsQueryable();
            int recordsFiltered = await query.CountAsync();

            var data = await query
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    paymenConcept = "",
                    price = "",
                    issueDate = "",
                    paymentDate = ""
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable_V2(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue)
        {
            var query = _context.PreuniversitaryPostulants
                .Where(x => x.PreuniversitaryTermId == termId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Contains(searchValue) ||
                                         x.MaternalSurname.Contains(searchValue) ||
                                         x.PaternalSurname.Contains(searchValue) ||
                                         x.Document.Contains(searchValue) ||
                                         x.Career.Name.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();

            var data = await query
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    fullName = x.FullName,
                    dni = x.Document,
                    career = x.Career.Name,
                    isPaid = x.IsPaid
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantFileDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            var query = _context.PreuniversitaryPostulants.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Contains(searchValue) ||
                         x.MaternalSurname.Contains(searchValue) ||
                         x.PaternalSurname.Contains(searchValue) ||
                         x.Document.Contains(searchValue) ||
                         x.Career.Name.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      PreuniversitaryTerm = x.PreuniversitaryTerm.Name,
                      id = x.Id,
                      fullName = x.FullName,
                      dni = x.Document,
                      career = x.Career.Name,
                      isPaid = x.IsPaid,
                      x.FinalScore,
                      x.AdmissionState
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantPreRegistersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            var query = _context.PreuniversitaryPostulants.Where(x => x.IsPaid == false).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Contains(searchValue) ||
                                         x.MaternalSurname.Contains(searchValue) ||
                                         x.PaternalSurname.Contains(searchValue) ||
                                         x.Document.Contains(searchValue) ||
                                         x.Career.Name.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      id = x.Id,
                      fullName = x.FullName,
                      dni = x.Document,
                      career = x.Career.Name,
                      isPaid = x.IsPaid
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<string> UsersWithCodeExist(string userCodePrefix)
            => await _context.PreuniversitaryPostulants.Where(x => x.Code.StartsWith(userCodePrefix)).Select(u => u.Code).OrderByDescending(u => u).FirstOrDefaultAsync();


        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, bool? onlyPaid = null)
        {
            Expression<Func<PreuniversitaryPostulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Document;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.WebCode;
                    break;
                case "3":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "4":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "7":
                    orderByPredicate = (x) => x.IsPaid;
                    break;
                default:
                    break;
            }

            var query = _context.PreuniversitaryPostulants
                .Where(x => x.PreuniversitaryTermId == termId)
                .AsNoTracking();

            if (onlyPaid.HasValue)
                query = query.Where(x => x.IsPaid == onlyPaid);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.FullName.ToUpper().Contains(searchValue.ToUpper())
                || x.Document.Contains(searchValue) || x.Code.Contains(searchValue));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    document = x.Document,
                    code = x.Code,
                    webCode = x.WebCode,
                    paternalSurname = x.PaternalSurname,
                    maternalSurname = x.MaternalSurname,
                    name = x.Name,
                    career = x.Career.Name,
                    isPaid = x.IsPaid ? "SI" : "NO",
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAdmittedPostulantsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, bool? onlyPaid = null)
        {
            Expression<Func<PreuniversitaryPostulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Document;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Email;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Phone;
                    break;
                case "5":
                    orderByPredicate = (x) => x.IsPaid;
                    break;
                default:
                    break;
            }

            var query = _context.PreuniversitaryPostulants
                .Where(x => x.PreuniversitaryTermId == termId)
                .AsNoTracking();

            if (onlyPaid.HasValue)
                query = query.Where(x => x.IsPaid == onlyPaid);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper())
                || x.MaternalSurname.ToUpper().Contains(searchValue.ToUpper())
                || x.PaternalSurname.ToUpper().Contains(searchValue.ToUpper())
                || x.Document.Contains(searchValue));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      id = x.Id,
                      name = x.FullName,
                      document = x.Document,
                      career = x.Career.Name,
                      email = x.Email,
                      phone = x.Phone,
                      isPaid = x.IsPaid ? "SI" : "NO",
                  })
                  .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<PostulantTemplate>> GetPostulantsReportData(Guid termId, string searchValue = null, bool? onlyPaid = null)
        {
            var query = _context.PreuniversitaryPostulants
                .Where(x => x.PreuniversitaryTermId == termId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper())
                || x.MaternalSurname.ToUpper().Contains(searchValue.ToUpper())
                || x.PaternalSurname.ToUpper().Contains(searchValue.ToUpper())
                || x.Document.Contains(searchValue));

            if (onlyPaid.HasValue)
                query = query.Where(x => x.IsPaid == onlyPaid);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                  .Select(x => new PostulantTemplate
                  {
                      Id = x.Id,
                      Document = x.Document,
                      Code = x.Code,
                      WebCode = x.WebCode,
                      FullName = x.FullName,
                      Name = x.Name,
                      PaternalSurname = x.PaternalSurname,
                      MaternalSurname = x.MaternalSurname,
                      Term = x.PreuniversitaryTerm.Name,
                      Career = x.Career.Name,
                      IsPaid = x.IsPaid ? "SI" : "NO",
                  })
                  .ToListAsync();

            return data;
        }
    }
}
