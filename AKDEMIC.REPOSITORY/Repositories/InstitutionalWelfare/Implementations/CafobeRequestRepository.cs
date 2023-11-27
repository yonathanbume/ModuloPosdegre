using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template.CafobeRequest;
using AKDEMIC.WEBSERVICE.Services.Chat.Models.Request;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Experimental;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class CafobeRequestRepository : Repository<CafobeRequest>, ICafobeRequestRepository
    {
        public CafobeRequestRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, Guid? termId = null, Guid? careerId = null, string searchValue = null)
        {
            Expression<Func<CafobeRequest, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Student.User.UserName);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Term.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Student.Career.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Type);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.Status);
                    break;

            }

            var query = _context.CafobeRequests.AsNoTracking();

            if (type != null) query = query.Where(x => x.Type == type);

            if (status != null) query = query.Where(x => x.Status == status);

            if (termId != null) query = query.Where(x => x.TermId == termId);

            if (careerId != null) query = query.Where(x => x.Student.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchTrim = searchValue.Trim();
                query = query.Where(x => x.Student.User.UserName.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.PaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.MaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.Name.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.FullName.ToUpper().Contains(searchTrim.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    TermName = x.Term.Name,
                    CareerName = x.Student.Career.Name,
                    TypeText = ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES.ContainsKey(x.Type) ?
                        ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES[x.Type] : "",
                    x.Status,
                    StatusText = ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES[x.Status] : "",
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null, string searchValue = null)
        {
            Expression<Func<CafobeRequest, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Student.User.UserName);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Term.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Student.User.Sex);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Student.Career.Faculty.Name);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.Type);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.Status);
                    break;
                case "7":
                    orderByPredicate = ((x) => x.Observation);
                    break;
            }

            var query = _context.CafobeRequests.AsNoTracking();

            if (type != null) query = query.Where(x => x.Type == type);

            if (sex != null) query = query.Where(x => x.Student.User.Sex == sex);

            if (status != null) query = query.Where(x => x.Status == status);

            if (termId != null) query = query.Where(x => x.TermId == termId);

            if (facultyId != null) query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchTrim = searchValue.Trim();
                query = query.Where(x => x.Student.User.UserName.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.PaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.MaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.Name.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.FullName.ToUpper().Contains(searchTrim.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    TermName = x.Term.Name,
                    SexText = ConstantHelpers.SEX.VALUES.ContainsKey(x.Student.User.Sex) ?
                        ConstantHelpers.SEX.VALUES[x.Student.User.Sex] : "-",
                    FacultyName = x.Student.Career.Faculty.Name,
                    x.Type,
                    TypeText = ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES.ContainsKey(x.Type) ?
                        ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES[x.Type] : "",
                    x.Status,
                    StatusText = ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES[x.Status] : "",
                    x.Observation
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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


        public async Task<CafobeRequestTemplate> GetDataById(Guid id)
        {
            var data = await _context.CafobeRequests
                .Where(x => x.Id == id)
                .Select(x => new CafobeRequestTemplate
                {
                    Id = x.Id,
                    StudentId = x.StudentId,
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    TermId = x.TermId,
                    TermName = x.Term.Name,
                    FacultyName = x.Student.Career.Faculty.Name,
                    CareerName = x.Student.Career.Name,
                    Type = x.Type,
                    SupportAmount = x.SupportAmount,
                    TypeText = ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES.ContainsKey(x.Type) ?
                        ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES[x.Type] : "",
                    Status = x.Status,
                    StatusText = ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES[x.Status] : "",
                    Observation = x.Observation,
                    ApprovedResolutionUrl = x.ApprovedResolutionUrl,

                    DirectorRequestUrl = x.DirectorRequestUrl,
                    DocumentaryProcedureVoucherUrl = x.DocumentaryProcedureVoucherUrl,
                    EnrollmentFormUrl = x.EnrollmentFormUrl,
                    LastTermHistoriesUrl = x.LastTermHistoriesUrl,
                    DniUrl = x.DniUrl,

                    ConstancyHigherFifthUrl = x.ConstancyHigherFifthUrl,
                    MeritChartHigherFifthUrl = x.MeritChartHigherFifthUrl,

                    BabyBirhtCertificateUrl = x.BabyBirhtCertificateUrl,
                    BabyControlCardUrl = x.BabyControlCardUrl,

                    MedicalRecordUrl = x.MedicalRecordUrl,
                    TreatmentRecordUrl = x.TreatmentRecordUrl,

                    DeathCertificateUrl = x.DeathCertificateUrl,
                    StudentBirthCertificateUrl = x.StudentBirthCertificateUrl,

                    OpthicalMedicalDiagnosticUrl = x.OpthicalMedicalDiagnosticUrl,
                    OpthicalProformUrl = x.OpthicalProformUrl,

                    EventInvitationUrl = x.EventInvitationUrl,
                    StudentHealthInsuranceUrl = x.StudentHealthInsuranceUrl,
                    StudentSportParticipationUrl = x.StudentSportParticipationUrl
                })
                .FirstOrDefaultAsync();


            return data;
        }

        public async Task<List<CafobeRequestTemplate>> GetReportData(int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null)
        {
            var data = await _context.CafobeRequests
            .Where(x => x.Status == status)
            .Select(x => new CafobeRequestTemplate
            {
                UserName = x.Student.User.UserName,
                FullName = x.Student.User.FullName,
                TermId = x.TermId,
                FacultyId = x.Student.Career.FacultyId,
                TermName = x.Term.Name,
                FacultyName = x.Student.Career.Faculty.Name,
                TypeText = ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES.ContainsKey(x.Type) ?
                    ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES[x.Type] : "",
                Type = x.Type,
                Sex = x.Student.User.Sex,
                Observation = x.Observation
            })
            .ToListAsync();

            if (type != -1) data = data.Where(x => x.Type == type).ToList();

            if (sex != -1) data = data.Where(x => x.Sex == sex).ToList();

            if (termId != null) data = data.Where(x => x.TermId == termId).ToList();

            if (facultyId != null) data = data.Where(x => x.FacultyId == facultyId).ToList();

            return data;

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentRequestDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, int? type = null, int? status = null, string searchValue = null)
        {
            Expression<Func<CafobeRequest, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Term.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Type);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Status);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Observation);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.SupportAmount);
                    break;

            }

            var query = _context.CafobeRequests
                .Where(x => x.StudentId == studentId)
                .AsNoTracking();


            if (type != null) query = query.Where(x => x.Type == type);

            if (status != null) query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchTrim = searchValue.Trim();
                query = query.Where(x => x.Student.User.UserName.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.PaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.MaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.Name.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.FullName.ToUpper().Contains(searchTrim.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    TermName = x.Term.Name,
                    Type = x.Type,
                    TypeText = ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES.ContainsKey(x.Type) ?
                        ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES[x.Type] : "",
                    StatusText = ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES[x.Status] : "",
                    x.Observation,
                    SupportAmount = x.SupportAmount.ToString("0.00")
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<bool> GetLastByStudent(Guid studentId , Guid actualTermId, int type)
        {
            var validcafoberequest = await _context.CafobeRequests.AnyAsync(x => x.StudentId == studentId && x.TermId == actualTermId && x.Type == type);

            return validcafoberequest;
        }


    }
}
