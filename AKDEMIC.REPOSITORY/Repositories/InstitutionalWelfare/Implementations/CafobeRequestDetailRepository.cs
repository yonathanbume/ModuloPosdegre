using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template.CafobeRequest;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class CafobeRequestDetailRepository : Repository<CafobeRequestDetail>, ICafobeRequestDetailRepository
    {
        public CafobeRequestDetailRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, Guid? termId = null, Guid? careerId = null, string searchValue = null)
        {
            Expression<Func<CafobeRequestDetail, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CafobeRequest.Student.User.UserName);
                    break;
                case "1":
                    orderByPredicate = (x) => x.CafobeRequest.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.CafobeRequest.Term.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.CafobeRequest.Student.Career.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.CafobeRequest.Type);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.Status);
                    break;

            }

            var query = _context.CafobeRequestDetails.AsNoTracking();

            if (type != null) query = query.Where(x => x.CafobeRequest.Type == type);

            if (status != null) query = query.Where(x => x.Status == status);

            if (termId != null) query = query.Where(x => x.CafobeRequest.TermId == termId);

            if (careerId != null) query = query.Where(x => x.CafobeRequest.Student.CareerId == careerId);


            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchTrim = searchValue.Trim();
                query = query.Where(x => x.CafobeRequest.Student.User.UserName.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.CafobeRequest.Student.User.PaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.CafobeRequest.Student.User.MaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.CafobeRequest.Student.User.Name.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.CafobeRequest.Student.User.FullName.ToUpper().Contains(searchTrim.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.CafobeRequestId,
                    x.CafobeRequest.Student.User.UserName,
                    x.CafobeRequest.Student.User.FullName,
                    TermName = x.CafobeRequest.Term.Name,
                    CareerName = x.CafobeRequest.Student.Career.Name,
                    TypeText = ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES.ContainsKey(x.CafobeRequest.Type) ?
                        ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES[x.CafobeRequest.Type] : "",
                    x.Status,
                    StatusText = ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.VALUES[x.Status] : "",
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
            Expression<Func<CafobeRequestDetail, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CafobeRequest.Student.User.UserName);
                    break;
                case "1":
                    orderByPredicate = (x) => x.CafobeRequest.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.CafobeRequest.Term.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.CafobeRequest.Student.User.Sex);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.CafobeRequest.Student.Career.Faculty.Name);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.CafobeRequest.Type);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.Status);
                    break;
            }

            var query = _context.CafobeRequestDetails.AsNoTracking();

            if (type != null) query = query.Where(x => x.CafobeRequest.Type == type);

            if (sex != null) query = query.Where(x => x.CafobeRequest.Student.User.Sex == sex);

            if (status != null) query = query.Where(x => x.Status == status);

            if (termId != null) query = query.Where(x => x.CafobeRequest.TermId == termId);

            if (facultyId != null) query = query.Where(x => x.CafobeRequest.Student.Career.FacultyId == facultyId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchTrim = searchValue.Trim();
                query = query.Where(x => x.CafobeRequest.Student.User.UserName.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.CafobeRequest.Student.User.PaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.CafobeRequest.Student.User.MaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.CafobeRequest.Student.User.Name.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.CafobeRequest.Student.User.FullName.ToUpper().Contains(searchTrim.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.CafobeRequest.Student.User.UserName,
                    x.CafobeRequest.Student.User.FullName,
                    TermName = x.CafobeRequest.Term.Name,
                    SexText = ConstantHelpers.SEX.VALUES.ContainsKey(x.CafobeRequest.Student.User.Sex) ?
                        ConstantHelpers.SEX.VALUES[x.CafobeRequest.Student.User.Sex] : "-",
                    FacultyName = x.CafobeRequest.Student.Career.Faculty.Name,
                    TypeText = ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES.ContainsKey(x.CafobeRequest.Type) ?
                        ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES[x.CafobeRequest.Type] : "",
                    x.Status,
                    StatusText = ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES[x.Status] : "",
                    FileUrl = x.FileDetailUrl
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
        public async Task<List<CafobeRequestDetailTemplate>> GetReportData(int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null)
        {
            var data = await _context
            .CafobeRequestDetails
            .Select(x => new CafobeRequestDetailTemplate
            {
                UserName = x.CafobeRequest.Student.User.UserName,
                FullName = x.CafobeRequest.Student.User.FullName,
                TermId = x.CafobeRequest.TermId,
                TermName = x.CafobeRequest.Term.Name,
                Type = x.CafobeRequest.Type,
                FacultyId = x.CafobeRequest.Student.Career.FacultyId,
                FacultyName = x.CafobeRequest.Student.Career.Faculty.Name,
                TypeText = ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES.ContainsKey(x.CafobeRequest.Type) ?
                    ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES[x.CafobeRequest.Type] : "",
                Sex = x.CafobeRequest.Student.User.Sex,
                SexText = ConstantHelpers.SEX.VALUES.ContainsKey(x.CafobeRequest.Student.User.Sex) ?
                        ConstantHelpers.SEX.VALUES[x.CafobeRequest.Student.User.Sex] : "-",
                Status = x.Status,
                StatusText  = ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.VALUES.ContainsKey(x.Status) ?
                    ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.VALUES[x.Status] : "",
         
                Observation = x.CafobeRequest.Observation
            })
            .ToListAsync();

            if (status != -1) data = data.Where(x => x.Status == status).ToList();

            if (type != -1) data = data.Where(x => x.Type == type).ToList();

            if (sex != -1) data = data.Where(x => x.Sex == sex).ToList();

            if (termId != null) data = data.Where(x => x.TermId == termId).ToList();

            if (facultyId != null) data = data.Where(x => x.FacultyId == facultyId).ToList();

            return data;

        }

        public async Task<CafobeRequestDetailTemplate> GetDataById(Guid cafobeRequestId)
        {
            var data = await _context.CafobeRequestDetails
                            .Where(x => x.CafobeRequestId == cafobeRequestId)
                            .Select(x => new CafobeRequestDetailTemplate
                            {
                                CafobeRequestId = x.CafobeRequestId,
                                Status = x.Status,
                                StatusText = ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.VALUES.ContainsKey(x.Status) ?
                                    ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.VALUES[x.Status] : "",
                                Comentary = x.Comentary,
                                FileDetailUrl = x.FileDetailUrl,
                                RegisterDate = x.RegisterDate,

                                StudentId = x.CafobeRequest.StudentId,
                                UserName = x.CafobeRequest.Student.User.UserName,
                                FullName = x.CafobeRequest.Student.User.FullName,
                                TermId = x.CafobeRequest.TermId,
                                TermName = x.CafobeRequest.Term.Name,
                                CareerName = x.CafobeRequest.Student.Career.Name,
                                CafobeRequestType = x.CafobeRequest.Type,
                                CafobeRequestTypeText = ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES.ContainsKey(x.CafobeRequest.Type) ?
                                    ConstantHelpers.CAFOBE_REQUEST.TYPE.VALUES[x.CafobeRequest.Type] : "",
                                CafobeRequestStatus = x.CafobeRequest.Status,
                                CafobeRequestStatusText = ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES.ContainsKey(x.CafobeRequest.Status) ?
                                    ConstantHelpers.CAFOBE_REQUEST.STATUS.VALUES[x.CafobeRequest.Status] : "",
                                Observation = x.CafobeRequest.Observation,
                                ApprovedResolutionUrl = x.CafobeRequest.ApprovedResolutionUrl,

                                DirectorRequestUrl = x.CafobeRequest.DirectorRequestUrl,
                                DocumentaryProcedureVoucherUrl = x.CafobeRequest.DocumentaryProcedureVoucherUrl,
                                EnrollmentFormUrl = x.CafobeRequest.EnrollmentFormUrl,
                                LastTermHistoriesUrl = x.CafobeRequest.LastTermHistoriesUrl,
                                DniUrl = x.CafobeRequest.DniUrl,

                                ConstancyHigherFifthUrl = x.CafobeRequest.ConstancyHigherFifthUrl,
                                MeritChartHigherFifthUrl = x.CafobeRequest.MeritChartHigherFifthUrl,

                                BabyBirhtCertificateUrl = x.CafobeRequest.BabyBirhtCertificateUrl,
                                BabyControlCardUrl = x.CafobeRequest.BabyControlCardUrl,

                                MedicalRecordUrl = x.CafobeRequest.MedicalRecordUrl,
                                TreatmentRecordUrl = x.CafobeRequest.TreatmentRecordUrl,

                                DeathCertificateUrl = x.CafobeRequest.DeathCertificateUrl,
                                StudentBirthCertificateUrl = x.CafobeRequest.StudentBirthCertificateUrl,

                                OpthicalMedicalDiagnosticUrl = x.CafobeRequest.OpthicalMedicalDiagnosticUrl,
                                OpthicalProformUrl = x.CafobeRequest.OpthicalProformUrl,

                                EventInvitationUrl = x.CafobeRequest.EventInvitationUrl,
                                StudentHealthInsuranceUrl = x.CafobeRequest.StudentHealthInsuranceUrl,
                                StudentSportParticipationUrl = x.CafobeRequest.StudentSportParticipationUrl
                            })
                            .FirstOrDefaultAsync();


            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentRequestDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, int? type = null, int? status = null, string searchValue = null)
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

            }

            var query = _context.CafobeRequests
                .Where(x => x.StudentId == studentId && x.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.APPROVED)
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
                    Status = _context.CafobeRequestDetails.Where(y=>y.CafobeRequestId == x.Id).Select(y=>y.Status).FirstOrDefault(),
                    StatusText = ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.VALUES.ContainsKey(_context.CafobeRequestDetails.Where(y => y.CafobeRequestId == x.Id).Select(y => y.Status).FirstOrDefault()) ?
                        ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.VALUES[_context.CafobeRequestDetails.Where(y => y.CafobeRequestId == x.Id).Select(y => y.Status).FirstOrDefault()] : "",
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
    }
}
