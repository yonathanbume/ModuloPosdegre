using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates.Diploma;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates.RegistryPattern;
using ClosedXML.Excel;
using MessagePack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Implementations
{
    public class RegistryPatternRepository : Repository<RegistryPattern>, IRegistryPatternRepository
    {
        public RegistryPatternRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        protected static void CreateHeaderRow(IXLWorksheet worksheet)
        {
            const int position = 1;
            var column = 0;

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "CODUNIV", column);

            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "RAZ_SOC", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "MATRI_FEC", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "FAC_NOM", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "CARR_PROG", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "ESC_POST", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "EGRES_FEC", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "APEPAT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "APEMAT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "NOMBRE", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "SEXO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DOCU_TIP", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DOCU_NUM", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_BACH", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "GRAD_TITU", column);

            worksheet.Column(++column).Width = 60;
            SetHeaderRowStyle(worksheet, "DEN_GRAD", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "SEG_ESP", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "TRAB_INV", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "NUM_CRED", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "REG_METADATO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROG_ESTU", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_TITULO_PED", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "MOD_OBT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "PROG_ACREDIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_INICIO_ACREDIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_FIN_ACREDIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_INICIO_MOD_TIT_ACREDIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_FIN_MOD_TIT_ACREDIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_SOLICIT_GRAD_TIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_TRAB_GRAD_TIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "TRAB_INVEST_ORIGINAL", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "MOD_EST", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "ABRE_GYT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_REV_PAIS", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_REV_UNIV", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_REV_GRADO", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "CRIT_REV", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "RESO_NUM", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "RESO_FEC", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DIPL_FEC_ORG", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DIPL_FEC_DUP", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DIPL_NUM", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DIPL_TIP_EMI", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "REG_LIBRO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "REG_FOLIO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "REG_REGISTRO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "CARGO1", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "AUTORIDAD1", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "CARGO2", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "AUTORIDAD2", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "CARGO3", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "AUTORIDAD3", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_PAIS_EXT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_UNIV_EXT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_GRADO_EXT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "REG_OFICIO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "FEC_MAT_PROG", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "FEC_INICIO_PROG", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "FEC_FIN_PROG", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "MOD_SUSTENTACION", column);

            worksheet.SheetView.FreezeRows(position);
        }
        protected static void SetHeaderRowStyle(IXLWorksheet worksheet, string headerName, int column)
        {
            const int position = 1;
            var fillColor = XLColor.FromArgb(0x0c618c);
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);
            var fontColor = XLColor.White;
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            const XLAlignmentHorizontalValues alignmentHorizontal = XLAlignmentHorizontalValues.Left;

            worksheet.Column(column).Style.Alignment.Horizontal = alignmentHorizontal;
            worksheet.Cell(position, column).Value = headerName;
            worksheet.Cell(position, column).Style.Font.Bold = true;
            worksheet.Cell(position, column).Style.Font.FontColor = fontColor;
            worksheet.Cell(position, column).Style.Fill.BackgroundColor = fillColor;
            worksheet.Cell(position, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(position, column).Style.Border.OutsideBorderColor = outsideBorderColor;



        }
        protected static void SetInformationStyle(IXLWorksheet worksheet, int row, int column, string data, bool requireDateFormat = false)
        {
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);

            if (requireDateFormat)
            {
                worksheet.Cell(row, column).Style.DateFormat.Format = "dd/MM/yyyy";
            }

            worksheet.Cell(row, column).Value = data;
            worksheet.Cell(row, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(row, column).Style.Border.OutsideBorderColor = outsideBorderColor;
        }

        private async Task LoadRegistryPatternInformationSunedu(IXLWorksheet worksheet, Guid? facultyId, Guid? careerId, Guid? academicProgramId, string dni = null, string searchBookNumber = null, string dateStartFilter = null, string dateEndFilter = null, int? type = null, int? clasification = null, bool toSunedu = false)
        {
            var row = 2;

            const int CODUNIV = 1;    //CODUNIV
            const int RAZ_SOC = 2;    //RAZ_SOC 
            const int MATRI_FEC = 3; //MATRI_FEC 
            const int FAC_NOM = 4;  //FAC_NOM
            const int CARR_PROG = 5;    //CARR_PROG
            const int ESC_POST = 6;    //ESC_POST
            const int EGRES_FEC = 7; //EGRES_FEC
            const int APEPAT = 8;     //
            const int APEMAT = 9;     // 
            const int NOMBRE = 10;    //
            const int SEXO = 11;      //
            const int DOCUTIP = 12;   //
            const int DOCU_NUM = 13;  //  
            const int PROC_BACH = 14;  //PROC_BACH
            const int GRAD_TITU = 15;   //GRAD_TITU
            const int DEN_GRAD = 16;  //DEN_GRAD
            const int SEG_ESP = 17; //SEG_ESP
            const int TRAB_INV = 18; //TRAB_INV
            const int NUM_CRED = 19; //NUM_CRED
            const int REG_METADATO = 20; //REG_METADATO
            const int PROG_ESTU = 21; //PROG_ESTU
            const int PROC_TITULO_PED = 22; //PROC_TITULO_PED
            const int MOD_OBT = 23; //MOD_OBT
            const int PROG_ACREDIT = 24;
            const int FEC_INICIO_ACREDIT = 25;
            const int FEC_FIN_ACREDIT = 26;
            const int FEC_INICIO_MOD_TIT_ACREDIT = 27;
            const int FEC_FIN_MOD_TIT_ACREDIT = 28;
            const int FEC_SOLICIT_GRAD_TIT = 29;
            const int FEC_TRAB_GRAD_TIT = 30;
            const int TRAB_INVEST_ORIGINAL = 31;
            const int MOD_EST = 32; //MOD_EST
            const int ABRE_GYT = 33; //ABRE_GYT
            const int PROC_REV_PAIS = 34; //PROC_REV_PAIS
            const int PROC_REV_UNIV = 35; //PROC_REV_UNIV
            const int PROC_REV_GRADO = 36; //PROC_REV_GRADO
            const int CRIT_REVL = 37;
            const int RESO_NUM = 38; //RESO_NUM
            const int RESO_FEC = 39; //RESO_FEC
            const int DIPL_FEC_ORG = 40; //DIPL_FEC_ORG
            const int DIPL_FEC_DUP = 41; //DIPL_FEC_DUP
            const int DIPL_NUM = 42; //DIPL_NUM
            const int DIPL_TIP_EMI = 43; //DIPL_TIP_EMI
            const int REG_LIBRO = 44; //REG_LIBRO
            const int REG_FOLIO = 45; //REG_FOLIO
            const int REG_REGISTRO = 46; //REG_REGISTRO
            const int CARGO1 = 47; //CARGO1
            const int AUTORIDAD1 = 48; //AUTORIDAD1
            const int CARGO2 = 49; //CARGO2
            const int AUTORIDAD2 = 50; //AUTORIDAD2
            const int CARGO3 = 51; //CARGO3
            const int AUTORIDAD3 = 52; //AUTORIDAD3
            const int PROC_PAIS_EXT = 53; //PROC_PAIS_EXT
            const int PROC_UNI_EXT = 54; //PROC_UNI_EXT
            const int PROC_GRADO_EXT = 55; //PROC_GRADO_EXT
            const int REG_OFICIO = 56; //REG_OFICIO
            const int FEC_MAT_PROG = 57; //FEC_MAT_PROG
            const int FEC_INICIO_PROG = 58; //FEC_INICIO_PROG
            const int FEC_FIN_PROG = 59;
            const int MOD_SUSTENTACION = 60;

            var query = _context.RegistryPatterns
                .Where(x => x.UniversityCode != null && x.Status == ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED)
                .AsQueryable();


            if (toSunedu)
            {
                query = query.Where(x => x.ReadyToSunedu == false);
            }

            if (clasification.HasValue)
            {
                switch (clasification)
                {
                    case ConstantHelpers.REGISTRY_PATTERN.CLASIFICATION.GENERATED:
                        query = query.Where(x => x.FacultyCouncilDate != null);
                        break;
                    case ConstantHelpers.REGISTRY_PATTERN.CLASIFICATION.PENDING:
                        query = query.Where(x => x.FacultyCouncilDate == null);
                        break;
                    default:
                        break;
                }
            }

            if (type.HasValue)
            {
                if (type > 0)
                {
                    query = query.Where(x => x.GradeType == type);
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
            {
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);
            }

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
            {
                query = query.Where(x => x.Student.AcademicProgramId == academicProgramId);
            }

            if (!String.IsNullOrEmpty(dni))
            {
                query = query.Where(x => x.Student.User.Dni.ToLower().Contains(dni.ToLower()) || x.Student.User.FullName.ToLower().Contains(dni.ToLower()));
            }

            if (!String.IsNullOrEmpty(searchBookNumber))
            {
                query = query.Where(x => x.BookCode.ToLower().Contains(searchBookNumber.ToLower()));
            }

            if (!String.IsNullOrEmpty(dateStartFilter) && !String.IsNullOrEmpty(dateEndFilter))
            {
                var dateStartDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateStartFilter);
                var dateEndDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateEndFilter);
                query = query.Where(x => (dateStartDateTime.Date <= x.CreatedAt.Value.Date) && (x.CreatedAt.Value.Date <= dateEndDateTime.Date));

            }

            var queryList = await query
                .Select(x => new 
                {
                    x.UniversityCode,
                    x.BussinesSocialReason,
                    FacultyName = x.Student.Career.Faculty.Name,
                    CareerName = x.Student.Career.Name,
                    AcademicProgramName = x.Student.AcademicAgreementId == null ? "--" : x.Student.AcademicAgreement.Name,
                    x.PostGraduateSchool,
                    x.Relation,
                    x.Student.User.PaternalSurname,
                    x.Student.User.MaternalSurname,
                    x.Student.User.Name,
                    Sex = ConstantHelpers.SEX.ABREV.ContainsKey(x.Student.User.Sex) ? ConstantHelpers.SEX.ABREV[x.Student.User.Sex] : "--",
                    DocumentType = x.DocumentType.ToString(),
                    x.Student.User.Dni,
                    AdmissionTermDate = x.Student.AdmissionTerm.EnrollmentStartDate.ToDateFormat(),
                    GraduationTermDate = x.Student.GraduationTermId == null ? "--" : x.Student.GraduationTerm.EnrollmentStartDate.ToDateFormat(),
                    x.BachelorOrigin,
                    GradeAbbreviation = x.GradeAbbreviation == "B" ? "BACHILLER" : "TÍTULO PROFESIONAL",
                    x.AcademicDegreeDenomination,
                    x.SpecialtyMention,
                    x.Specialty,
                    x.ResearchWork,
                    Credits = x.Credits.ToString(),
                    x.ResearchWorkURL,
                    x.DegreeProgram,
                    x.PedagogicalTitleOrigin,
                    x.ObtainingDegreeModality,
                    x.StudyModality,
                    x.OriginDegreeCountry,
                    x.OriginDegreeUniversity,
                    x.GradDenomination,
                    x.ResolutionNumber,
                    ResolutionDateByUniversityCouncil = x.ResolutionDateByUniversityCouncil.HasValue ? x.ResolutionDateByUniversityCouncil.Value.ToDateFormat() : null,
                    OriginDiplomatDate = x.OriginDiplomatDate.HasValue ? x.OriginDiplomatDate.Value.ToDateFormat() : null,
                    DuplicateDiplomatDate = x.DuplicateDiplomatDate.HasValue ? x.DuplicateDiplomatDate.Value.ToDateFormat() : null,
                    x.DiplomatNumber,
                    x.EmissionTypeOfDiplomat,
                    x.BookCode,
                    x.FolioCode,
                    x.RegistryNumber,
                    x.ManagingDirector,
                    x.ManagingDirectorFullName,
                    x.GeneralSecretary,
                    x.GeneralSecretaryFullName,
                    x.AcademicResponsible,
                    x.AcademicResponsibleFullName,
                    x.OriginPreRequisiteDegreeCountry,
                    ForeignUniversityOriginName = x.ForeignUniversityOriginId.HasValue ? x.ForeignUniversityOrigin.Name : "--",
                    x.OriginPreRequisiteDegreeDenomination,
                    x.OfficeNumber,
                    DateEnrollmentProgram = x.DateEnrollmentProgram.HasValue ? x.DateEnrollmentProgram.Value.ToDateFormat() : null,
                    StartDateEnrollmentProgram = x.StartDateEnrollmentProgram.HasValue ? x.StartDateEnrollmentProgram.Value.ToDateFormat() : null,
                    EndDateEnrollmentProgram = x.EndDateEnrollmentProgram.HasValue ? x.EndDateEnrollmentProgram.Value.ToDateFormat() : null,
                    x.UniversityCouncilType,
                    FacultyCouncilDate = x.FacultyCouncilDate.HasValue ? x.FacultyCouncilDate.Value.ToDateFormat() : null,
                    UniversityCouncilDate = x.UniversityCouncilDate.HasValue ? x.UniversityCouncilDate.Value.ToDateFormat() : null,
                    SineaceAcreditation = ConstantHelpers.CONDITIONAL_ANSWER.VALUES.ContainsKey(x.SineaceAcreditation) ?
                        ConstantHelpers.CONDITIONAL_ANSWER.VALUES[x.SineaceAcreditation] : "--",
                    SineaceAcreditationStartDate = x.SineaceAcreditationStartDate.HasValue ? x.SineaceAcreditationStartDate.Value.ToDateFormat() : null,
                    SineaceAcreditationEndDate = x.SineaceAcreditationEndDate.HasValue ? x.SineaceAcreditationEndDate.Value.ToDateFormat() : null,
                    SineaceAcreditationDegreeModalityStartDate = x.SineaceAcreditationDegreeModalityStartDate.HasValue ? x.SineaceAcreditationDegreeModalityStartDate.Value.ToDateFormat() : null,
                    SineaceAcreditationDegreeModalityEndDate = x.SineaceAcreditationDegreeModalityEndDate.HasValue ? x.SineaceAcreditationDegreeModalityEndDate.Value.ToDateFormat() : null,
                    ProcessDegreeDate = x.ProcessDegreeDate.HasValue ? x.ProcessDegreeDate.Value.ToDateFormat() : null,
                    DegreeSustentationDate = x.DegreeSustentationDate.HasValue ? x.DegreeSustentationDate.Value.ToDateFormat() : null,
                    IsOriginal = ConstantHelpers.CONDITIONAL_ANSWER.VALUES.ContainsKey(x.IsOriginal) ? 
                        ConstantHelpers.CONDITIONAL_ANSWER.VALUES[x.IsOriginal] : "--",
                    ComplianceRevalidationCriteria = ConstantHelpers.CONDITIONAL_ANSWER.VALUES.ContainsKey(x.ComplianceRevalidationCriteria) ?
                        ConstantHelpers.CONDITIONAL_ANSWER.VALUES[x.ComplianceRevalidationCriteria]: "--",
                    SustainabilityMode = ConstantHelpers.THESIS_SUPPORT_MODALITY.VALUES.ContainsKey(x.SustainabilityMode) ?
                        ConstantHelpers.THESIS_SUPPORT_MODALITY.VALUES[x.SustainabilityMode] : "--",
                    x.Initial,
                    x.Correlative,
                    x.Year,
                    x.Student.User.UserName,
                    x.Student.User.Email,
                    x.Student.User.PhoneNumber

                })
                .ToListAsync();

            if (queryList != null)
            {
                if (queryList.Count == 0)
                {
                    throw new Exception("No existe registros");
                }

            }

            foreach (var registryPattern in queryList)
            {
                SetInformationStyle(worksheet, row, CODUNIV, registryPattern.UniversityCode);
                SetInformationStyle(worksheet, row, RAZ_SOC, registryPattern.BussinesSocialReason);
                SetInformationStyle(worksheet, row, MATRI_FEC, registryPattern.AdmissionTermDate, true);
                SetInformationStyle(worksheet, row, FAC_NOM, registryPattern.FacultyName);
                SetInformationStyle(worksheet, row, CARR_PROG, registryPattern.CareerName);
                SetInformationStyle(worksheet, row, ESC_POST, registryPattern.PostGraduateSchool);
                SetInformationStyle(worksheet, row, EGRES_FEC, registryPattern.GraduationTermDate, true);
                SetInformationStyle(worksheet, row, APEPAT, registryPattern.PaternalSurname);
                SetInformationStyle(worksheet, row, APEMAT, registryPattern.MaternalSurname);
                SetInformationStyle(worksheet, row, NOMBRE, registryPattern.Name);
                SetInformationStyle(worksheet, row, SEXO, registryPattern.Sex);
                SetInformationStyle(worksheet, row, DOCUTIP, registryPattern.DocumentType);
                SetInformationStyle(worksheet, row, DOCU_NUM, registryPattern.Dni);
                SetInformationStyle(worksheet, row, PROC_BACH, registryPattern.BachelorOrigin);
                SetInformationStyle(worksheet, row, GRAD_TITU, registryPattern.GradeAbbreviation);
                SetInformationStyle(worksheet, row, DEN_GRAD, registryPattern.AcademicDegreeDenomination);
                SetInformationStyle(worksheet, row, SEG_ESP, registryPattern.Specialty);
                SetInformationStyle(worksheet, row, TRAB_INV, registryPattern.ResearchWork);
                SetInformationStyle(worksheet, row, NUM_CRED, registryPattern.Credits.ToString());
                SetInformationStyle(worksheet, row, REG_METADATO, registryPattern.ResearchWorkURL);
                SetInformationStyle(worksheet, row, PROG_ESTU, registryPattern.DegreeProgram);
                SetInformationStyle(worksheet, row, PROC_TITULO_PED, registryPattern.PedagogicalTitleOrigin);
                SetInformationStyle(worksheet, row, MOD_OBT, registryPattern.ObtainingDegreeModality);
                SetInformationStyle(worksheet, row, PROG_ACREDIT, registryPattern.SineaceAcreditation);
                SetInformationStyle(worksheet, row, FEC_INICIO_ACREDIT, registryPattern.SineaceAcreditationStartDate, true);
                SetInformationStyle(worksheet, row, FEC_FIN_ACREDIT, registryPattern.SineaceAcreditationEndDate, true);
                SetInformationStyle(worksheet, row, FEC_INICIO_MOD_TIT_ACREDIT, registryPattern.SineaceAcreditationDegreeModalityStartDate, true);
                SetInformationStyle(worksheet, row, FEC_FIN_MOD_TIT_ACREDIT, registryPattern.SineaceAcreditationDegreeModalityEndDate, true);
                SetInformationStyle(worksheet, row, FEC_SOLICIT_GRAD_TIT, registryPattern.ProcessDegreeDate, true);
                SetInformationStyle(worksheet, row, FEC_TRAB_GRAD_TIT, registryPattern.DegreeSustentationDate, true);
                SetInformationStyle(worksheet, row, TRAB_INVEST_ORIGINAL, registryPattern.IsOriginal);
                SetInformationStyle(worksheet, row, MOD_EST, registryPattern.StudyModality);
                SetInformationStyle(worksheet, row, ABRE_GYT, registryPattern.GradeAbbreviation);
                SetInformationStyle(worksheet, row, PROC_REV_PAIS, registryPattern.OriginDegreeCountry);
                SetInformationStyle(worksheet, row, PROC_REV_UNIV, registryPattern.OriginDegreeUniversity);
                SetInformationStyle(worksheet, row, PROC_REV_GRADO, registryPattern.GradDenomination);
                SetInformationStyle(worksheet, row, CRIT_REVL, registryPattern.ComplianceRevalidationCriteria);
                SetInformationStyle(worksheet, row, RESO_NUM, registryPattern.ResolutionNumber);
                SetInformationStyle(worksheet, row, RESO_FEC, registryPattern.ResolutionDateByUniversityCouncil, true);
                SetInformationStyle(worksheet, row, DIPL_FEC_ORG, registryPattern.OriginDiplomatDate, true);
                SetInformationStyle(worksheet, row, DIPL_FEC_DUP, registryPattern.DuplicateDiplomatDate, true);
                SetInformationStyle(worksheet, row, DIPL_NUM, registryPattern.DiplomatNumber);
                SetInformationStyle(worksheet, row, DIPL_TIP_EMI, registryPattern.EmissionTypeOfDiplomat);
                SetInformationStyle(worksheet, row, REG_LIBRO, registryPattern.BookCode);
                SetInformationStyle(worksheet, row, REG_FOLIO, registryPattern.FolioCode);
                SetInformationStyle(worksheet, row, REG_REGISTRO, registryPattern.RegistryNumber);
                SetInformationStyle(worksheet, row, CARGO1, registryPattern.ManagingDirector);
                SetInformationStyle(worksheet, row, AUTORIDAD1, registryPattern.ManagingDirectorFullName);
                SetInformationStyle(worksheet, row, CARGO2, registryPattern.GeneralSecretary);
                SetInformationStyle(worksheet, row, AUTORIDAD2, registryPattern.GeneralSecretaryFullName);
                SetInformationStyle(worksheet, row, CARGO3, registryPattern.AcademicResponsible);
                SetInformationStyle(worksheet, row, AUTORIDAD3, registryPattern.AcademicResponsibleFullName);
                SetInformationStyle(worksheet, row, PROC_PAIS_EXT, registryPattern.OriginPreRequisiteDegreeCountry);
                SetInformationStyle(worksheet, row, PROC_UNI_EXT, registryPattern.ForeignUniversityOriginName);
                SetInformationStyle(worksheet, row, PROC_GRADO_EXT, registryPattern.OriginPreRequisiteDegreeDenomination);
                SetInformationStyle(worksheet, row, REG_OFICIO, registryPattern.OfficeNumber);
                SetInformationStyle(worksheet, row, FEC_MAT_PROG, registryPattern.DateEnrollmentProgram, true);
                SetInformationStyle(worksheet, row, FEC_INICIO_PROG, registryPattern.StartDateEnrollmentProgram, true);
                SetInformationStyle(worksheet, row, FEC_FIN_PROG, registryPattern.EndDateEnrollmentProgram, true);
                SetInformationStyle(worksheet, row, MOD_SUSTENTACION, registryPattern.SustainabilityMode);
                row++;
            }
        }
        private async Task LoadRegistryPatternInformation(IXLWorksheet worksheet, Guid? facultyId, Guid? careerId, Guid? academicProgramId, string dni = null, string searchBookNumber = null, string dateStartFilter = null, string dateEndFilter = null, int? type = null, int? clasification = null, bool toSunedu = false)
        {

            var row = 2;
            const int CODUNIV = 1;    //CODUNIV
            const int RAZ_SOC = 2;    //RAZ_SOC 
            const int FAC_NOM = 3;  //FAC_NOM
            const int CARR_PROG = 4;    //CARR_PROG
            const int PROGRAMA_ACADEMICO = 5;  //PROGRAMA_ACADEMICO --
            const int ESC_POST = 6;    //ESC_POST
            const int RELACION = 7;  //RELACION --
            const int APEPAT = 8;     //
            const int APEMAT = 9;     // 
            const int NOMBRE = 10;    //
            const int SEXO = 11;      //
            const int DOCUTIP = 12;   //
            const int DOCU_NUM = 13;  //  
            const int MATRI_FEC = 14; //MATRI_FEC 
            const int EGRES_FEC = 15; //EGRES_FEC
            const int PROC_BACH = 16;  //PROC_BACH
            const int GRAD_TITU = 17;   //GRAD_TITU
            const int DEN_GRAD = 18;  //DEN_GRAD
            const int ESPECIALIDAD_O_MENCION = 19;  //ESPECIALIDAD_O_MENCION --
            const int SEG_ESP = 20; //SEG_ESP
            const int TRAB_INV = 21; //TRAB_INV
            const int NUM_CRED = 22; //NUM_CRED
            const int REG_METADATO = 23; //REG_METADATO
            const int PROG_ESTU = 24; //PROG_ESTU
            const int PROC_TITULO_PED = 25; //PROC_TITULO_PED
            const int MOD_OBT = 26; //MOD_OBT
            const int MOD_EST = 27; //MOD_EST
            const int ABRE_GYT = 28; //ABRE_GYT
            const int PROC_REV_PAIS = 29; //PROC_REV_PAIS
            const int PROC_REV_UNIV = 30; //PROC_REV_UNIV
            const int PROC_REV_GRADO = 31; //PROC_REV_GRADO
            const int RESO_NUM = 32; //RESO_NUM
            const int RESO_FEC = 33; //RESO_FEC
            const int DIPL_FEC_ORG = 34; //DIPL_FEC_ORG
            const int DIPL_FEC_DUP = 35; //DIPL_FEC_DUP
            const int DIPL_NUM = 36; //DIPL_NUM
            const int DIPL_TIP_EMI = 37; //DIPL_TIP_EMI
            const int REG_LIBRO = 38; //REG_LIBRO
            const int REG_FOLIO = 39; //REG_FOLIO
            const int REG_REGISTRO = 40; //REG_REGISTRO
            const int CARGO1 = 41; //CARGO1
            const int AUTORIDAD1 = 42; //AUTORIDAD1
            const int CARGO2 = 43; //CARGO2
            const int AUTORIDAD2 = 44; //AUTORIDAD2
            const int CARGO3 = 45; //CARGO3
            const int AUTORIDAD3 = 46; //AUTORIDAD3
            const int PROC_PAIS_EXT = 47; //PROC_PAIS_EXT
            const int PROC_UNI_EXT = 48; //PROC_UNI_EXT
            const int PROC_GRADO_EXT = 49; //PROC_GRADO_EXT
            const int REG_OFICIO = 50; //REG_OFICIO
            const int FEC_MAT_PROG = 51; //FEC_MAT_PROG
            const int FEC_INICIO_PROG = 52; //FEC_INICIO_PROG
            const int FEC_FIN_PROG = 53;                  //FEC_FIN_PROG
            const int TIPO_DE_CONSEJO_UNIVERSITARIO = 54; //TIPP DE CONSEJO UNIVERSITARIO
            const int FEC_CONSEJO_DE_FACULTAD = 55;       //FEC_CONSEJO DE FACULTAD
            const int FEC_CONSEJO_UNIVERSITARIO = 56;     //FEC_CONSEJO UNIVERSITARIO
            const int PROG_ACREDIT = 57;
            const int FEC_INICIO_ACREDIT = 58;
            const int FEC_FIN_ACREDIT = 59;
            const int FEC_INICIO_MOD_TIT_ACREDIT = 60;
            const int FEC_FIN_MOD_TIT_ACREDIT = 61;
            const int FEC_SOLICIT_GRAD_TIT = 62;
            const int FEC_TRAB_GRAD_TIT = 63;
            const int TRAB_INVEST_ORIGINAL = 64;
            const int CRIT_REVL = 65;
            const int MOD_SUSTENTACION = 66;
            const int SIGLA = 67;                         //SIGLA
            const int CORRELATIVO = 68;                   //CORRELATIVO
            const int YEAR = 69;                          //AÑO
            const int CODIGO_MATRICULA = 70;                          //AÑO
            const int EMAIL = 71;                          //AÑO
            const int PHONE_NUMBER = 72;                          //AÑO

            var query = _context.RegistryPatterns.Include(x => x.Student.Career.Faculty).Include(x => x.Student.User)
                .Include(x => x.Student.AdmissionTerm)
                .Include(x => x.Student.GraduationTerm)
                .Include(x => x.Procedure)
                .Include(x => x.Concept)
                .Include(x => x.ForeignUniversityOrigin)
                .Include(x => x.Student.AcademicProgram).Where(x => x.UniversityCode != null && x.Status == ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED).AsQueryable();


            if (toSunedu)
            {
                query = query.Where(x => x.ReadyToSunedu == false);
            }

            if (clasification.HasValue)
            {
                switch (clasification)
                {
                    case ConstantHelpers.REGISTRY_PATTERN.CLASIFICATION.GENERATED:
                        query = query.Where(x => x.FacultyCouncilDate != null);
                        break;
                    case ConstantHelpers.REGISTRY_PATTERN.CLASIFICATION.PENDING:
                        query = query.Where(x => x.FacultyCouncilDate == null);
                        break;
                    default:
                        break;
                }
            }

            if (type.HasValue)
            {
                if (type > 0)
                {
                    query = query.Where(x => x.GradeType == type);
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
            {
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);
            }

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
            {
                query = query.Where(x => x.Student.AcademicProgramId == academicProgramId);
            }

            if (!String.IsNullOrEmpty(dni))
            {
                query = query.Where(x => x.Student.User.Dni.ToLower().Contains(dni.ToLower()) || x.Student.User.FullName.ToLower().Contains(dni.ToLower()));
            }

            if (!String.IsNullOrEmpty(searchBookNumber))
            {
                query = query.Where(x => x.BookCode.ToLower().Contains(searchBookNumber.ToLower()));
            }

            if (!String.IsNullOrEmpty(dateStartFilter) && !String.IsNullOrEmpty(dateEndFilter))
            {
                var dateStartDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateStartFilter);
                var dateEndDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateEndFilter);
                query = query.Where(x => (dateStartDateTime.Date <= x.CreatedAt.Value.Date) && (x.CreatedAt.Value.Date <= dateEndDateTime.Date));

            }

            var queryList = await query.ToListAsync();

            if (queryList != null)
            {
                if (queryList.Count == 0)
                {
                    throw new Exception("No existe registros");
                }

            }

            foreach (var registryPattern in queryList)
            {
                SetInformationStyle(worksheet, row, CODUNIV, registryPattern.UniversityCode);
                SetInformationStyle(worksheet, row, RAZ_SOC, registryPattern.BussinesSocialReason);
                SetInformationStyle(worksheet, row, FAC_NOM, registryPattern.Student.Career.Faculty.Name);
                SetInformationStyle(worksheet, row, CARR_PROG, registryPattern.Student.Career.Name);
                SetInformationStyle(worksheet, row, PROGRAMA_ACADEMICO, registryPattern.Student.AcademicProgramId.HasValue ? registryPattern.Student.AcademicProgram.Name : "--");
                SetInformationStyle(worksheet, row, ESC_POST, registryPattern.PostGraduateSchool);
                SetInformationStyle(worksheet, row, RELACION, registryPattern.Relation);
                SetInformationStyle(worksheet, row, APEPAT, registryPattern.Student.User.PaternalSurname);
                SetInformationStyle(worksheet, row, APEMAT, registryPattern.Student.User.MaternalSurname);
                SetInformationStyle(worksheet, row, NOMBRE, registryPattern.Student.User.Name);
                SetInformationStyle(worksheet, row, SEXO, registryPattern.Student.User.Sex == ConstantHelpers.SEX.MALE ? "M" : "F");
                SetInformationStyle(worksheet, row, DOCUTIP, registryPattern.DocumentType.ToString());
                SetInformationStyle(worksheet, row, DOCU_NUM, registryPattern.Student.User.Dni);
                SetInformationStyle(worksheet, row, MATRI_FEC, registryPattern.Student.AdmissionTerm.EnrollmentStartDate.ToDateFormat(), true);
                SetInformationStyle(worksheet, row, EGRES_FEC, registryPattern.Student.GraduationTermId.HasValue ? registryPattern.Student.GraduationTerm.EnrollmentStartDate.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, PROC_BACH, registryPattern.BachelorOrigin);
                SetInformationStyle(worksheet, row, GRAD_TITU, registryPattern.GradeAbbreviation == "B" ? "BACHILLER" : "TÍTULO PROFESIONAL");
                SetInformationStyle(worksheet, row, DEN_GRAD, registryPattern.AcademicDegreeDenomination);
                SetInformationStyle(worksheet, row, ESPECIALIDAD_O_MENCION, registryPattern.SpecialtyMention);
                SetInformationStyle(worksheet, row, SEG_ESP, registryPattern.Specialty);
                SetInformationStyle(worksheet, row, TRAB_INV, registryPattern.ResearchWork);
                SetInformationStyle(worksheet, row, NUM_CRED, registryPattern.Credits.ToString());
                SetInformationStyle(worksheet, row, REG_METADATO, registryPattern.ResearchWorkURL);
                SetInformationStyle(worksheet, row, PROG_ESTU, registryPattern.DegreeProgram);
                SetInformationStyle(worksheet, row, PROC_TITULO_PED, registryPattern.PedagogicalTitleOrigin);
                SetInformationStyle(worksheet, row, MOD_OBT, registryPattern.ObtainingDegreeModality);
                SetInformationStyle(worksheet, row, MOD_EST, registryPattern.StudyModality);
                SetInformationStyle(worksheet, row, ABRE_GYT, registryPattern.GradeAbbreviation);
                SetInformationStyle(worksheet, row, PROC_REV_PAIS, registryPattern.OriginDegreeCountry);
                SetInformationStyle(worksheet, row, PROC_REV_UNIV, registryPattern.OriginDegreeUniversity);
                SetInformationStyle(worksheet, row, PROC_REV_GRADO, registryPattern.GradDenomination);
                SetInformationStyle(worksheet, row, RESO_NUM, registryPattern.ResolutionNumber);
                SetInformationStyle(worksheet, row, RESO_FEC, registryPattern.ResolutionDateByUniversityCouncil.HasValue ? registryPattern.ResolutionDateByUniversityCouncil.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, DIPL_FEC_ORG, registryPattern.OriginDiplomatDate.HasValue ? registryPattern.OriginDiplomatDate.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, DIPL_FEC_DUP, registryPattern.DuplicateDiplomatDate.HasValue ? registryPattern.DuplicateDiplomatDate.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, DIPL_NUM, registryPattern.DiplomatNumber);
                SetInformationStyle(worksheet, row, DIPL_TIP_EMI, registryPattern.EmissionTypeOfDiplomat);
                SetInformationStyle(worksheet, row, REG_LIBRO, registryPattern.BookCode);
                SetInformationStyle(worksheet, row, REG_FOLIO, registryPattern.FolioCode);
                SetInformationStyle(worksheet, row, REG_REGISTRO, registryPattern.RegistryNumber);
                SetInformationStyle(worksheet, row, CARGO1, registryPattern.ManagingDirector);
                SetInformationStyle(worksheet, row, AUTORIDAD1, registryPattern.ManagingDirectorFullName);
                SetInformationStyle(worksheet, row, CARGO2, registryPattern.GeneralSecretary);
                SetInformationStyle(worksheet, row, AUTORIDAD2, registryPattern.GeneralSecretaryFullName);
                SetInformationStyle(worksheet, row, CARGO3, registryPattern.AcademicResponsible);
                SetInformationStyle(worksheet, row, AUTORIDAD3, registryPattern.AcademicResponsibleFullName);
                SetInformationStyle(worksheet, row, PROC_PAIS_EXT, registryPattern.OriginPreRequisiteDegreeCountry);
                SetInformationStyle(worksheet, row, PROC_UNI_EXT, registryPattern.ForeignUniversityOriginId.HasValue ? registryPattern.ForeignUniversityOrigin.Name : "--");
                SetInformationStyle(worksheet, row, PROC_GRADO_EXT, registryPattern.OriginPreRequisiteDegreeDenomination);
                SetInformationStyle(worksheet, row, REG_OFICIO, registryPattern.OfficeNumber);
                SetInformationStyle(worksheet, row, FEC_MAT_PROG, registryPattern.DateEnrollmentProgram.HasValue ? registryPattern.DateEnrollmentProgram.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, FEC_INICIO_PROG, registryPattern.StartDateEnrollmentProgram.HasValue ? registryPattern.StartDateEnrollmentProgram.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, FEC_FIN_PROG, registryPattern.EndDateEnrollmentProgram.HasValue ? registryPattern.EndDateEnrollmentProgram.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, TIPO_DE_CONSEJO_UNIVERSITARIO, registryPattern.UniversityCouncilType);
                SetInformationStyle(worksheet, row, FEC_CONSEJO_DE_FACULTAD, registryPattern.FacultyCouncilDate.HasValue ? registryPattern.FacultyCouncilDate.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, FEC_CONSEJO_UNIVERSITARIO, registryPattern.UniversityCouncilDate.HasValue ? registryPattern.UniversityCouncilDate.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, PROG_ACREDIT, ConstantHelpers.CONDITIONAL_ANSWER.VALUES[registryPattern.SineaceAcreditation]);
                SetInformationStyle(worksheet, row, FEC_INICIO_ACREDIT, registryPattern.SineaceAcreditationStartDate.HasValue ? registryPattern.SineaceAcreditationStartDate.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, FEC_FIN_ACREDIT, registryPattern.SineaceAcreditationEndDate.HasValue ? registryPattern.SineaceAcreditationEndDate.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, FEC_INICIO_MOD_TIT_ACREDIT, registryPattern.SineaceAcreditationDegreeModalityStartDate.HasValue ? registryPattern.SineaceAcreditationDegreeModalityStartDate.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, FEC_FIN_MOD_TIT_ACREDIT, registryPattern.SineaceAcreditationDegreeModalityEndDate.HasValue ? registryPattern.SineaceAcreditationDegreeModalityEndDate.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, FEC_SOLICIT_GRAD_TIT, registryPattern.ProcessDegreeDate.HasValue ? registryPattern.ProcessDegreeDate.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, FEC_TRAB_GRAD_TIT, registryPattern.DegreeSustentationDate.HasValue ? registryPattern.DegreeSustentationDate.Value.ToDateFormat() : null, true);
                SetInformationStyle(worksheet, row, TRAB_INVEST_ORIGINAL, ConstantHelpers.CONDITIONAL_ANSWER.VALUES[registryPattern.IsOriginal]);
                SetInformationStyle(worksheet, row, CRIT_REVL, ConstantHelpers.CONDITIONAL_ANSWER.VALUES[registryPattern.ComplianceRevalidationCriteria]);
                SetInformationStyle(worksheet, row, MOD_SUSTENTACION, ConstantHelpers.THESIS_SUPPORT_MODALITY.VALUES[registryPattern.SustainabilityMode]);
                SetInformationStyle(worksheet, row, SIGLA, registryPattern.Initial);
                SetInformationStyle(worksheet, row, CORRELATIVO, registryPattern.Correlative);
                SetInformationStyle(worksheet, row, YEAR, registryPattern.Year);
                SetInformationStyle(worksheet, row, CODIGO_MATRICULA, $"'{registryPattern.Student.User.UserName}");
                SetInformationStyle(worksheet, row, EMAIL, registryPattern.Student.User.Email);
                SetInformationStyle(worksheet, row, PHONE_NUMBER, registryPattern.Student.User.PhoneNumber);
                row++;
            }


        }
        
        #endregion


        //public bool AnyRegistryPatternByUserProcedure(Guid userProcedureId)
        //{
        //    return _context.RegistryPatterns.Where(x => x.UserProcedureId == userProcedureId).Any();
        //}

        public int GetCurrentAcademicYearAsync(string id)
        {
            return _context.Students.Where(s => s.UserId == id).FirstOrDefault().CurrentAcademicYear;
        }

        public Expression<Func<UserProcedure, dynamic>> SelectUserProcedures()
        {
            return (x) => new
            {
                Id = x.Id,
                User = x.UserId,
                DNI = x.DNI,
                ProcedureName = x.Procedure.Name,
                GenerateId = x.GeneratedId,
                CurrentAcademicYear = GetCurrentAcademicYearAsync(x.UserId),
                //StatusCreated = AnyRegistryPatternByUserProcedure(x.Id)

            };
        }

        public Expression<Func<RegistryPattern, dynamic>> SelectUserProceduresGenerates()
        {
            return (x) => new
            {
                FullName = x.Student.User.FullName,
                FacultyName = x.Student.Career.Faculty.Name,
                CareerName = x.Student.Career.Name
            };
        }

        public Func<UserProcedure, string[]> GetUserProcedureDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Procedure.Name + "",
                GetCurrentAcademicYearAsync(x.UserId) + "",
                x.DNI + "",
                x.GeneratedId + ""
            };
        }

        public Func<RegistryPattern, string[]> GetUserProcedureGenerateSearchValuePredicate()
        {
            return (x) => new[]
            {
               x.Student.User.Name + "",
               x.Student.User.MaternalSurname + "",
               x.Student.User.PaternalSurname + "",
               x.Student.Career.Name + "",
               x.Student.Career.Faculty.Name + ""
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProcedureByRegistryPatternAsync(DataTablesStructs.SentParameters sentParameters, Expression<Func<UserProcedure, dynamic>> selectPredicate = null, Func<UserProcedure, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.UserProcedures.Where(x => (x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED ||
                 x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.ARCHIVED || x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS)
                 && x.User.UserRoles.Any(s => s.Role.Name == "Alumnos")).AsQueryable();

            var lstGuid = new List<Guid>();
            var configurationBachelorType = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_TYPE_BACHELOR).FirstOrDefaultAsync();
            var configurationBachelor = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_BACHELOR).FirstOrDefaultAsync();
            var configurationTitleDegreeProffesionalExperience = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE).FirstOrDefaultAsync();
            var configurationTitleDegreeSufficiencyExam = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM).FirstOrDefaultAsync();
            var configurationTitleDegreeSupportTesis = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUPPORT_TESIS).FirstOrDefaultAsync();
            if (configurationBachelor != null && !String.IsNullOrEmpty(configurationBachelor.Value))
            {
                var guidBachelor = new Guid(configurationBachelor.Value);
                lstGuid.Add(guidBachelor);
            }
            if (configurationTitleDegreeProffesionalExperience != null && !String.IsNullOrEmpty(configurationTitleDegreeProffesionalExperience.Value))
            {
                var guidTitleDegreeProffesionalExperience = new Guid(configurationTitleDegreeProffesionalExperience.Value);
                lstGuid.Add(guidTitleDegreeProffesionalExperience);
            }
            if (configurationTitleDegreeSufficiencyExam != null && !String.IsNullOrEmpty(configurationTitleDegreeSufficiencyExam.Value))
            {
                var guidTitleDegreeSufficiencyExam = new Guid(configurationTitleDegreeSufficiencyExam.Value);
                lstGuid.Add(guidTitleDegreeSufficiencyExam);
            }
            if (configurationTitleDegreeSupportTesis != null && !String.IsNullOrEmpty(configurationTitleDegreeSupportTesis.Value))
            {
                var guidTitleDegreeSupportTesis = new Guid(configurationTitleDegreeSupportTesis.Value);
                lstGuid.Add(guidTitleDegreeSupportTesis);
            }
            query = query.Where(x => lstGuid.Contains(x.ProcedureId));

            query = query.AsNoTracking();

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    user = x.User.FullName,
                    dni = x.User.Dni,
                    procedure_name = x.Procedure.Name,
                    generateid = x.GeneratedId,
                    //status_created = _context.RegistryPatterns.Any(s => s.UserProcedureId == x.Id),
                    currentAcademicYear = _context.Students.Where(s => s.UserId == x.UserId).FirstOrDefault().CurrentAcademicYear
                }, searchValue)
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

        public async Task<DataTablesStructs.ReturnedData<object>> UserProceduresGenerateByRegistryPatternAsync(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, Expression<Func<RegistryPattern, dynamic>> selectPredicate = null, Func<RegistryPattern, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.RegistryPatterns
                                                 //.Include(x => x.UserProcedure)
                                                 .Include(x => x.Student.Career.Faculty)
                                                 .Include(x => x.Student.User).AsQueryable();

            var configurationBachelorType = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_TYPE_BACHELOR).FirstOrDefaultAsync();
            var configurationBachelor = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_BACHELOR).FirstOrDefaultAsync();
            var configurationTitleDegreeProffesionalExperience = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE).FirstOrDefaultAsync();
            var configurationTitleDegreeSufficiencyExam = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM).FirstOrDefaultAsync();
            var configurationTitleDegreeSupportTesis = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.TUPA_PROFESSIONAL_DEGREE_SUPPORT_TESIS).FirstOrDefaultAsync();


            var lstGuid = new List<Guid>();


            if (configurationBachelor != null && !String.IsNullOrEmpty(configurationBachelor.Value))
            {
                var guidBachelor = new Guid(configurationBachelor.Value);
                lstGuid.Add(guidBachelor);

            }
            if (configurationTitleDegreeProffesionalExperience != null && !String.IsNullOrEmpty(configurationTitleDegreeProffesionalExperience.Value))
            {
                var guidTitleDegreeProffesionalExperience = new Guid(configurationTitleDegreeProffesionalExperience.Value);
                lstGuid.Add(guidTitleDegreeProffesionalExperience);

            }
            if (configurationTitleDegreeSufficiencyExam != null && !String.IsNullOrEmpty(configurationTitleDegreeSufficiencyExam.Value))
            {
                var guidTitleDegreeSufficiencyExam = new Guid(configurationTitleDegreeSufficiencyExam.Value);
                lstGuid.Add(guidTitleDegreeSufficiencyExam);


            }
            if (configurationTitleDegreeSupportTesis != null && !String.IsNullOrEmpty(configurationTitleDegreeSupportTesis.Value))
            {
                var guidTitleDegreeSupportTesis = new Guid(configurationTitleDegreeSupportTesis.Value);
                lstGuid.Add(guidTitleDegreeSupportTesis);

            }
            //query = query.Where(x => lstGuid.Contains(x.UserProcedure.ProcedureId));
            if (facultyId.HasValue)
            {
                query = query.Where(x => x.Student.Career.FacultyId == facultyId.Value);
            }
            if (careerId.HasValue)
            {
                query = query.Where(x => x.Student.CareerId == careerId.Value);
            }
            query = query.WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }




        public async Task<RegistryPatternTemplate> GetRegistryPatternTemplate(Guid id)
        {
            var result = new RegistryPatternTemplate();
            var query = _context.RegistryPatterns.Include(x => x.Student.User)
                .Include(x => x.Student.Career.Faculty)
                .Include(x => x.Student.GraduationTerm)
                .Include(x => x.Student.AdmissionTerm)
                .Include(x => x.Student.AcademicProgram)
                .Include(x => x.ForeignUniversityOrigin).Where(x => x.Id == id).AsQueryable();


            result = await query.Select(x => new RegistryPatternTemplate
            {
                Id = x.Id,
                GraduationDate = x.GraduationDate.ToLocalDateFormat(),
                UniversityCode = x.UniversityCode,
                BussinesSocialReason = x.BussinesSocialReason,
                GradeAbbreviation = x.GradeAbbreviation,
                AcademicProgram = x.Student.AcademicProgram.Name,
                RegistrationDate = x.Student.AdmissionTerm.EnrollmentStartDate.ToLocalDateFormat(),
                RegistrationEnd = x.Student.GraduationTermId.HasValue ? x.Student.GraduationTerm.EnrollmentStartDate.ToLocalDateFormat() : "",
                FacultyName = x.Student.Career.Faculty.Name,
                CareerName = x.Student.Career.Name,
                PostGraduateSchool = x.PostGraduateSchool,
                PaternalSurname = x.Student.User.PaternalSurname,
                MaternalSurname = x.Student.User.MaternalSurname,
                StudentName = x.Student.User.Name,
                Sex = x.Student.User.Sex == ConstantHelpers.SEX.MALE ? "M" : "F",
                DocumentType = x.Student.User.DocumentType,
                DNI = x.Student.User.Dni,
                BachelorOrigin = x.BachelorOrigin,
                AcademicDegree = x.GradeAbbreviation == "B" ? "BACHILLER" : "TÍTULO PROFESIONAL",
                AcademicDegreeDenomination = x.AcademicDegreeDenomination,
                Specialty = x.Specialty,
                ResearchWork = x.ResearchWork,
                Credits = x.Credits,
                ResearchWorkURL = x.ResearchWorkURL,
                DegreeProgram = x.DegreeProgram,
                //String.IsNullOrEmpty() ? byte.MinValue : ConstantHelpers.PROGRAM_STUDIES.INDICES[x.DegreeProgram],
                //DegreeProgram = x.DegreeProgram,
                PedagogicalTitleOrigin = x.PedagogicalTitleOrigin,
                ObtainingDegreeModality = x.ObtainingDegreeModality,
                StudyModality = x.StudyModality,
                OriginDegreeCountry = x.OriginDegreeCountry,
                OriginDegreeUniversity = x.OriginDegreeUniversity,
                GradDenomination = x.GradDenomination,
                ResolutionNumber = x.ResolutionNumber,
                ResolutionDateByUniversityCouncil = x.ResolutionDateByUniversityCouncil.HasValue ? x.ResolutionDateByUniversityCouncil.Value.ToLocalDateFormat() : null,
                OriginDiplomatDate = x.OriginDiplomatDate.HasValue ? x.OriginDiplomatDate.Value.ToLocalDateFormat() : null,
                DuplicateDiplomatDate = x.DuplicateDiplomatDate.HasValue ? x.DuplicateDiplomatDate.Value.ToLocalDateFormat() : null,
                DiplomatNumber = x.DiplomatNumber,
                EmissionTypeOfDiplomat = x.EmissionTypeOfDiplomat,
                BookCode = x.BookCode,
                FolioCode = x.FolioCode,
                RegistryNumber = x.RegistryNumber,
                ManagingDirector = x.ManagingDirector,
                ManagingDirectorFullName = x.ManagingDirectorFullName,
                GeneralSecretary = x.GeneralSecretary,
                GeneralSecretaryFullName = x.GeneralSecretaryFullName,
                AcademicResponsible = x.AcademicResponsible,
                AcademicResponsibleFullName = x.AcademicResponsibleFullName,
                OriginPreRequisiteDegreeCountry = x.OriginPreRequisiteDegreeCountry,
                ForeignUniversityOriginId = x.ForeignUniversityOriginId,
                OriginPreRequisiteDegreeDenomination = x.OriginPreRequisiteDegreeDenomination,
                OfficeNumber = x.OfficeNumber,
                DateEnrollmentProgram = x.DateEnrollmentProgram.HasValue ? x.DateEnrollmentProgram.Value.ToLocalDateFormat() : null,
                StartDateEnrollmentProgram = x.StartDateEnrollmentProgram.HasValue ? x.StartDateEnrollmentProgram.Value.ToLocalDateFormat() : null,
                EndDateEnrollmentProgram = x.EndDateEnrollmentProgram.HasValue ? x.EndDateEnrollmentProgram.Value.ToLocalDateFormat() : null,
                Relation = x.Relation,
                SpecialtyMention = x.SpecialtyMention,
                UniversityCouncilType = x.UniversityCouncilType,
                FacultyCouncilDate = x.FacultyCouncilDate.HasValue ? x.FacultyCouncilDate.Value.ToLocalDateFormat() : null,
                UniversityCouncilDate = x.UniversityCouncilDate.HasValue ? x.UniversityCouncilDate.Value.ToLocalDateFormat() : null,
                Initial = x.Initial,
                Correlative = x.Correlative,
                Year = x.Year,
                BookTitle = x.BookTitle,
                GraduationTermId = x.Student.GraduationTermId,
                UserName = x.Student.User.UserName,
                Phone = x.Student.User.PhoneNumber,
                Email1 = x.Student.User.Email,
                Email2 = x.Student.User.PersonalEmail,
                AcademicProgramId = x.Student.AcademicProgramId,
                CareerId = x.Student.CareerId,
                StudentId = x.StudentId

            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task UpdateRegistryPatternTemplateAsync(RegistryPatternTemplate model)
        {
            var entity = await _context.RegistryPatterns.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            entity.UniversityCode = model.UniversityCode;
            entity.PostGraduateSchool = model.PostGraduateSchool;

            entity.BachelorOrigin = model.BachelorOrigin;


            entity.Specialty = model.Specialty;

            entity.DegreeProgram = ConstantHelpers.PROGRAM_STUDIES.VALUES[model.DegreeProgram]; ;
            entity.PedagogicalTitleOrigin = model.PedagogicalTitleOrigin;
            entity.ObtainingDegreeModality = model.ObtainingDegreeModality;
            entity.StudyModality = model.StudyModality;
            entity.OriginDegreeCountry = model.OriginDegreeCountry;
            entity.OriginDegreeUniversity = model.OriginDegreeUniversity;

            entity.ResolutionNumber = model.ResolutionNumber;
            entity.ResolutionDateByUniversityCouncil = !string.IsNullOrWhiteSpace(model.ResolutionDateByUniversityCouncil) ? ConvertHelpers.DatepickerToUtcDateTime(model.ResolutionDateByUniversityCouncil) : (DateTime?)null;
            entity.OriginDiplomatDate = !string.IsNullOrWhiteSpace(model.OriginDiplomatDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.OriginDiplomatDate) : (DateTime?)null;
            entity.DuplicateDiplomatDate = !string.IsNullOrWhiteSpace(model.DuplicateDiplomatDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.DuplicateDiplomatDate) : (DateTime?)null;
            entity.DiplomatNumber = model.DiplomatNumber;
            entity.EmissionTypeOfDiplomat = model.EmissionTypeOfDiplomat;
            entity.BookCode = model.BookCode;
            entity.FolioCode = model.FolioCode;
            entity.RegistryNumber = model.RegistryNumber;
            entity.ManagingDirector = model.ManagingDirector;
            entity.ManagingDirectorFullName = model.ManagingDirectorFullName;
            entity.GeneralSecretary = model.GeneralSecretary;
            entity.GeneralSecretaryFullName = model.GeneralSecretaryFullName;
            entity.AcademicResponsible = model.AcademicResponsible;
            entity.AcademicResponsibleFullName = model.AcademicResponsibleFullName;
            entity.OriginPreRequisiteDegreeCountry = model.OriginPreRequisiteDegreeCountry;
            entity.ForeignUniversityOriginId = model.ForeignUniversityOriginId;
            entity.OriginPreRequisiteDegreeDenomination = model.OriginPreRequisiteDegreeDenomination;
            entity.OfficeNumber = model.OfficeNumber;
            entity.DateEnrollmentProgram = !string.IsNullOrWhiteSpace(model.DateEnrollmentProgram) ? ConvertHelpers.DatepickerToUtcDateTime(model.DateEnrollmentProgram) : (DateTime?)null;
            entity.StartDateEnrollmentProgram = !string.IsNullOrWhiteSpace(model.StartDateEnrollmentProgram) ? ConvertHelpers.DatepickerToUtcDateTime(model.StartDateEnrollmentProgram) : (DateTime?)null;
            entity.EndDateEnrollmentProgram = !string.IsNullOrWhiteSpace(model.EndDateEnrollmentProgram) ? ConvertHelpers.DatepickerToUtcDateTime(model.EndDateEnrollmentProgram) : (DateTime?)null;
            entity.AcademicProgram = model.AcademicProgram;
            entity.Relation = model.Relation;
            entity.SpecialtyMention = model.SpecialtyMention;
            entity.UniversityCouncilType = model.UniversityCouncilType;
            entity.FacultyCouncilDate = !string.IsNullOrWhiteSpace(model.FacultyCouncilDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.FacultyCouncilDate) : (DateTime?)null;
            entity.UniversityCouncilDate = !string.IsNullOrWhiteSpace(model.UniversityCouncilDate) ? ConvertHelpers.DatepickerToUtcDateTime(model.UniversityCouncilDate) : (DateTime?)null;
            entity.Initial = model.Initial;
            entity.Correlative = model.Correlative;
            entity.Year = model.Year;
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProcedureByRegistryPattern(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            return await GetUserProcedureByRegistryPatternAsync(sentParameters, SelectUserProcedures(), GetUserProcedureDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> UserProceduresGenerateByRegistryPattern(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, Expression<Func<RegistryPattern, dynamic>> selectPredicate = null, Func<RegistryPattern, string[]> searchValuePredicate = null, string searchValue = null)
        {
            return await UserProceduresGenerateByRegistryPatternAsync(sentParameters, facultyId, careerId, SelectUserProceduresGenerates(), GetUserProcedureGenerateSearchValuePredicate(), searchValue);
        }

        public async Task UpdateRegistryPatternTemplate(RegistryPatternTemplate model)
        {
            await UpdateRegistryPatternTemplateAsync(model);
        }

        public async Task DownloadExcel(IXLWorksheet worksheet, Guid? facultyId, Guid? careerId, Guid? academicProgramId, string dni = null, string searchBookNumber = null, string dateStartFilter = null, string dateEndFilter = null, int? type = null, int? clasification = null, bool toSunedu = false)
        {
            CreateHeaderRow(worksheet);
            await LoadRegistryPatternInformationSunedu(worksheet, facultyId, careerId, academicProgramId, dni, searchBookNumber, dateStartFilter, dateEndFilter, type, clasification, toSunedu);
        }

        public async Task<List<DegreeRelationTemplate>> GetRelationDegreesByProcedure(DateTime startDateTime, DateTime endDateTime, List<Guid> resultsToSearch, string bookCode)
        {
            var query = _context.RegistryPatterns.AsQueryable();
            if (bookCode != null)
            {
                query = query.Where(x => x.BookCode == bookCode);
            }
            var result = await query.Where(x =>
                                                (x.UniversityCouncilDate.Value.Date <= endDateTime.Date &&
                                                x.UniversityCouncilDate.Value.Date >= startDateTime.Date) && x.Status != ConstantHelpers.REGISTRY_PATTERN.STATUS.GENERATED && x.Status != ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED)
                                              .Include(x => x.Student.User)
                                              .Include(x => x.Student.Career)
                                              .Select(x => new DegreeRelationTemplate
                                              {
                                                  Career = x.Student.Career.Name,
                                                  FullName = x.Student.User.FullName,
                                                  GradeType = x.GradeType
                                              }).ToListAsync();
            foreach (var item in query.Where(x =>
            (x.UniversityCouncilDate.Value.Date <= endDateTime.Date &&
            x.UniversityCouncilDate.Value.Date >= startDateTime.Date) && x.Status != ConstantHelpers.REGISTRY_PATTERN.STATUS.GENERATED && x.Status != ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED))
            {
                item.Status = ConstantHelpers.REGISTRY_PATTERN.STATUS.GENERATED;
            }
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<List<DegreeRelationTemplate>> GetRelationDegreesByConcept(DateTime startDateTime, DateTime endDateTime, List<Guid> resultsToSearch)
        {
            var result = await _context.RegistryPatterns
                                                .Where(x =>
                                                //resultsToSearch.Contains(x.Payment.ConceptId.Value) &&
                                                x.CreatedAt.Value.Date <= endDateTime.Date &&
                                                x.CreatedAt.Value.Date >= startDateTime.Date)
                                              .Include(x => x.Student.User)
                                              .Include(x => x.Student.Career).Select(x => new DegreeRelationTemplate
                                              {
                                                  Career = x.Student.Career.Name,
                                                  FullName = x.Student.User.FullName,
                                                  GradeType = x.GradeType
                                              }).ToListAsync();
            return result;
        }


        public async Task<int> Diploma3MonthAgoCount()
        {
            var Today = DateTime.UtcNow;
            var Today3MonthAgo = DateTime.UtcNow.Date.AddMonths(-3);
            var result = _context.RegistryPatterns.Where(x => x.Status == ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED).Where(x => Today3MonthAgo.Date <= x.CreatedAt.Value.Date && x.CreatedAt.Value.Date <= Today.Date);
            return await result.CountAsync();
        }

        public async Task<int> RegistryPattern3MonthAgoCount()
        {
            var Today = DateTime.UtcNow;
            var Today3MonthAgo = DateTime.UtcNow.Date.AddMonths(-3);
            var result = _context.RegistryPatterns.Where(x => Today3MonthAgo.Date <= x.CreatedAt.Value.Date && x.CreatedAt.Value.Date <= Today.Date);
            return await result.CountAsync();
        }

        public async Task<List<RegistryPattern>> GetAllByStudent(Guid studentId)
        {
            var query = _context.RegistryPatterns.Include(x => x.Student.Career)
                .Where(x => x.StudentId == studentId);

            return await query.ToListAsync();
        }

        public async Task<RegistryPatternConceptStudentTemplate> GetRegistryPatternConceptByStudentId(Guid studentId)
        {
            var registryPattern = await _context.RegistryPatterns
                .Where(x => x.StudentId == studentId)
                .Select(x => new RegistryPatternConceptStudentTemplate
                {
                    //ConceptId = x.Payment.Concept,
                    Date = $"{x.CreatedAt:dd-MM-yyyy}",
                    Number = x.RegistryNumber
                }).FirstOrDefaultAsync();
            return registryPattern;
        }

        public async Task<RegistryPatternProcedureStudentTemplate> GetRegistryPatternProcedureByStudentId(Guid studentId)
        {
            var registryPattern = await _context.RegistryPatterns
                .Where(X => X.StudentId == studentId).
                Select(x => new RegistryPatternProcedureStudentTemplate
                {
                    //ProcedureId = x.UserProcedure.ProcedureId,
                    Date = $"{x.CreatedAt:dd-MM-yyyy}",
                    Number = x.RegistryNumber
                }).FirstOrDefaultAsync();

            return registryPattern;
        }

        public async Task<List<DegreeDate>> GetAllByStudentTemplate(Guid studentId)
        {
            var result = await _context.RegistryPatterns
                .Where(x => x.StudentId == studentId)
                .Select(x => new DegreeDate
                {
                    Description = x.AcademicDegreeDenomination,
                    Institution = GeneralHelpers.GetInstitutionName(),
                })
                .ToListAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRegistryPatternDatatableByConfiguration(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, Guid? academicProgramId, string searchBookNumber = null, string dateStartFilter = null, string dateEndFilter = null, string searchValue = null, string searchFullName = null, string searchDNI = null, int? status = null, int? type = null, int? diplomaStatus = null, string searchDiplomaNumber = null, string officeNumber = null, int? clasification = null)
        {
            Expression<Func<RegistryPattern, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "2":
                    orderByPredicate = ((x) => x.Student.User.FullName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Student.User.Dni);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Student.Career.Name);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.Student.CurrentAcademicYear);
                    break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }
            var query = _context.RegistryPatterns.Include(x => x.Student.User).Include(x => x.Student.Career).Include(x => x.Procedure).Include(x => x.Concept).AsQueryable();
            if (clasification.HasValue)
            {
                switch (clasification)
                {
                    case ConstantHelpers.REGISTRY_PATTERN.CLASIFICATION.GENERATED:
                        query = query.Where(x => x.UniversityCouncilDate != null);
                        break;
                    case ConstantHelpers.REGISTRY_PATTERN.CLASIFICATION.PENDING:
                        query = query.Where(x => x.UniversityCouncilDate == null);
                        break;
                    default:
                        break;
                }
            }

            if (type.HasValue)
            {
                if (type > 0)
                {
                    query = query.Where(x => x.GradeType == type);
                }
            }

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status);
            }

            if (diplomaStatus.HasValue)
            {
                if (diplomaStatus >= 0)
                {
                    query = query.Where(x => x.DiplomaStatus == diplomaStatus);
                }

            }
            if (!String.IsNullOrEmpty(officeNumber))
            {
                query = query.Where(x => x.OfficeNumber == officeNumber);
            }
            if (!String.IsNullOrEmpty(searchDiplomaNumber))
            {
                query = query.Where(x => x.DiplomatNumber == searchDiplomaNumber);
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
            {
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);
            }

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
            {
                query = query.Where(x => x.Student.AcademicProgramId == academicProgramId);
            }

            if (!String.IsNullOrEmpty(searchFullName))
            {
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(searchFullName.ToLower()));
            }
            if (!String.IsNullOrEmpty(searchDNI))
            {
                query = query.Where(x => x.Student.User.Dni.ToLower().Contains(searchDNI.ToLower()));
            }

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(searchValue.ToLower())
                || x.Student.User.Dni.ToLower().Contains(searchValue.ToLower())
                || x.Student.User.UserName.ToLower().Contains(searchValue.ToLower()));
            }
            if (!String.IsNullOrEmpty(searchBookNumber))
            {
                query = query.Where(x => x.BookCode.ToLower().Contains(searchBookNumber.ToLower()));
            }

            if (!String.IsNullOrEmpty(dateStartFilter) && !String.IsNullOrEmpty(dateEndFilter))
            {
                var dateStartDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateStartFilter);
                var dateEndDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateEndFilter);
                query = query.Where(x => (dateStartDateTime.Date <= x.UniversityCouncilDate.Value.Date) && (x.UniversityCouncilDate.Value.Date <= dateEndDateTime.Date));

            }


            var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM).FirstOrDefaultAsync();

            var isIntegrated = bool.Parse(configuration.Value);

            var recordsFiltered = 0;
            recordsFiltered = await query.CountAsync();

            var data = await query
                            .Skip(sentParameters.PagingFirstRecord)
                            .Take(sentParameters.RecordsPerDraw)
                            .OrderByCondition("DESC", orderByPredicate)
                            .Select(x => new
                            {
                                id = x.Id,
                                userName = x.Student.User.UserName,
                                user = x.Student.User.FullName,
                                dni = x.Student.User.Dni,
                                request_name = (isIntegrated) ? x.Procedure.Name : x.Concept.Description,
                                careerName = x.Student.Career.Name,
                                type = (x.GradeType > 0) ? ConstantHelpers.GRADE_INFORM.DegreeType.VALUES[x.GradeType].ToUpper() : "--",
                                hasGeneratedCode = x.UniversityCouncilDate.HasValue,
                                currentAcademicYear = x.Student.CurrentAcademicYear,
                                diplomadelivery = ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.VALUES[x.DiplomaStatus].ToUpper(),
                                diplomadeliveryNumber = x.DiplomaStatus

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

        public async Task<RegistryPattern> GetWithIncludes(Guid id)
        {
            var query = _context.RegistryPatterns
                .Include(x => x.Student.User)
                .Include(x => x.Student.AcademicProgram)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<RegistryPattern> GetRegistryPatternBasedGradeReport(Guid studentId, int gradeType)
        {
            var query = _context.RegistryPatterns
                 .Include(x => x.Student)
                 .Where(x => x.StudentId == studentId && x.GradeType == gradeType);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRegistryPatternApprovedDatatableByConfiguration(DataTablesStructs.SentParameters sentParameters, Guid? careerId, string searchBookNumber, string dateStartFilter, string dateEndFilter, string searchValue, int? status)
        {
            var query = _context.RegistryPatterns.Include(x => x.Student.User).Include(x => x.Student.Career).Include(x => x.Procedure).Include(x => x.Concept).AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status);
            }

            if (careerId.HasValue)
            {
                query = query.Where(x => x.Student.CareerId == careerId);
            }


            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(searchValue.ToLower())
                || x.Student.User.Dni.ToLower().Contains(searchValue.ToLower()));
            }
            if (!String.IsNullOrEmpty(searchBookNumber))
            {
                query = query.Where(x => x.BookCode.ToLower().Contains(searchBookNumber.ToLower()));
            }

            if (!String.IsNullOrEmpty(dateStartFilter) && !String.IsNullOrEmpty(dateEndFilter))
            {
                var dateStartDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateStartFilter);
                var dateEndDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateEndFilter);
                query = query.Where(x => (dateStartDateTime.Date <= x.UniversityCouncilDate.Value.Date) && (x.UniversityCouncilDate.Value.Date <= dateEndDateTime.Date));

            }

            var recordsFiltered = 0;
            recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    user = x.Student.User.FullName,
                    dni = x.Student.User.Dni,
                    careerName = x.Student.Career.Name,
                    type = (x.GradeType > 0) ? ConstantHelpers.GRADE_INFORM.DegreeType.VALUES[x.GradeType] : "--",
                    hasGeneratedCode = String.IsNullOrEmpty(x.UniversityCode),
                    currentAcademicYear = x.Student.CurrentAcademicYear
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

        public async Task ApprovedAll(Guid? careerId, string searchBookNumber, string dateStartFilter, string dateEndFilter, string searchValue)
        {
            var query = _context.RegistryPatterns.Include(x => x.Student.User).Include(x => x.Student.Career).Include(x => x.Procedure).Include(x => x.Concept).Where(x => x.Status == ConstantHelpers.REGISTRY_PATTERN.STATUS.GENERATED).AsQueryable();


            if (careerId.HasValue)
            {
                query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(searchValue.ToLower())
                || x.Student.User.Dni.ToLower().Contains(searchValue.ToLower()));
            }
            if (!String.IsNullOrEmpty(searchBookNumber))
            {
                query = query.Where(x => x.BookCode.ToLower().Contains(searchBookNumber.ToLower()));
            }

            if (!String.IsNullOrEmpty(dateStartFilter) && !String.IsNullOrEmpty(dateEndFilter))
            {
                var dateStartDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateStartFilter);
                var dateEndDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateEndFilter);
                query = query.Where(x => (dateStartDateTime.Date <= x.UniversityCouncilDate.Value.Date) && (x.UniversityCouncilDate.Value.Date <= dateEndDateTime.Date));

            }

            var gradeReport = new GradeReport();

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                item.Status = ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED;
                switch (item.GradeType)
                {
                    case ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR:
                        item.Student.Status = ConstantHelpers.Student.States.BACHELOR;
                        break;
                    case ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE:
                        item.Student.Status = ConstantHelpers.Student.States.QUALIFIED;
                        break;
                }
                gradeReport = await _context.GradeReports.Where(x => x.StudentId == item.StudentId && x.GradeType == item.GradeType).FirstOrDefaultAsync();
                gradeReport.Status = ConstantHelpers.GRADE_INFORM.STATUS.FINALIZED;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRelationDegreeDatatable(DataTablesStructs.SentParameters sentParameters, DateTime startDate, DateTime endDate, string bookCode, string searchValue = null)
        {
            var query = _context.RegistryPatterns.Include(x => x.Student.User).Include(x => x.Student.User)
                                              .Include(x => x.Student.Career).AsQueryable();

            if (bookCode != null)
            {
                query = query.Where(x => x.BookCode.ToLower().Contains(bookCode));
            }
            if (searchValue != null)
            {
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(searchValue.ToLower()) ||
                x.Student.User.Dni.ToLower().Contains(searchValue.ToLower()));
            }

            var result = query.Where(x => x.UniversityCouncilDate.Value.Date <= endDate.Date &&
                                               x.UniversityCouncilDate.Value.Date >= startDate.Date && x.Status != ConstantHelpers.REGISTRY_PATTERN.STATUS.GENERATED && x.Status != ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED);


            var recordsFiltered = await result.CountAsync();
            var data = await result
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    Career = x.Student.Career.Name,
                    FullName = x.Student.User.FullName,
                    GradeType = ConstantHelpers.GRADE_INFORM.DegreeType.VALUES.ContainsKey(x.GradeType) ? ConstantHelpers.GRADE_INFORM.DegreeType.VALUES[x.GradeType] : "--"
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

        public async Task<DiplomaPdfReportTemplate> GetPdfReport(Guid id)
        {
            var Today = DateTime.UtcNow.ToDefaultTimeZone();
            var cultureInfo = new CultureInfo("es-PE");
            var myTI = cultureInfo.TextInfo;

            var registryPattern = await _context.RegistryPatterns.
                Include(x => x.Student.User).
                Include(x => x.Student.Career.Faculty).
                Include(x => x.Student.AcademicProgram)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var careerAcreditations = await _context.CareerAccreditations.Where(s => s.CareerId == registryPattern.Student.CareerId).ToListAsync();

            var result = new DiplomaPdfReportTemplate
            {
                Path = "",
                Type = registryPattern.DocumentType,
                Faculty = myTI.ToTitleCase(registryPattern.Student.Career.Faculty.Name.ToLower()) ?? "-",
                Career = myTI.ToTitleCase(registryPattern.Student.Career.Name.ToLower()),
                AcademicProgram = registryPattern.Student.AcademicProgramId.HasValue ? myTI.ToTitleCase(registryPattern.Student.AcademicProgram.Name.ToLower()) : "-",
                FormatedDate = $"a los {Today.Day} días del mes de {Today.ToString("MMMM", cultureInfo)} del {Today.Year}",
                CurrentDay = Today.ToString("dd", cultureInfo),
                CurrentMonth = Today.ToString("MMMM", cultureInfo),
                CurrentYear = Today.Year.ToString(),
                Dean = registryPattern.AcademicResponsibleFullName,
                GeneralSecretary = registryPattern.GeneralSecretaryFullName,
                Rector = registryPattern.ManagingDirectorFullName,
                StudentName = myTI.ToTitleCase(registryPattern.Student.User.Name.ToLower()),
                StudentMaternalSurName = myTI.ToTitleCase(registryPattern.Student.User.MaternalSurname.ToLower()),
                StudentPaternalSurName = myTI.ToTitleCase(registryPattern.Student.User.PaternalSurname.ToLower()),
                AcademicDegreeDenomination = registryPattern.AcademicDegreeDenomination,

                Book = registryPattern.BookCode ?? "-",
                Foil = registryPattern.FolioCode ?? "-",
                RegisterNumber = registryPattern.RegistryNumber ?? "-",
                DiplomaNumber = String.IsNullOrEmpty(registryPattern.DiplomatNumber) ? "-" : $"{registryPattern.DiplomatNumber:0000}",

                UniversityCode = registryPattern.UniversityCode,
                DocumentType = registryPattern.Student.User.DocumentType > 0 ? ConstantHelpers.DOCUMENT_TYPES.VALUES[registryPattern.Student.User.DocumentType] : "-",
                DocumentNumber = registryPattern.Student.User.Document ?? "-",
                StudentModality = registryPattern.StudyModality ?? "-",
                GradeAbrev = registryPattern.GradeAbbreviation ?? "-",
                GradeModalityGain = registryPattern.ObtainingDegreeModality,
                UniversitaryDate = registryPattern.UniversityCouncilDate.HasValue
                    ? $"{registryPattern.UniversityCouncilDate.ToDefaultTimeZone().Value.Day} de {registryPattern.UniversityCouncilDate.Value.ToString("MMMM", cultureInfo)} del {registryPattern.UniversityCouncilDate.ToDefaultTimeZone().Value.Year}" : "-",
                ResolutionNumber = registryPattern.ResolutionNumber ?? "-",
                EmittedDiplomaType = registryPattern.EmissionTypeOfDiplomat ?? "-",
                ResoultionRectorDate = registryPattern.ResolutionDateByUniversityCouncil.HasValue
                    ? $"{registryPattern.ResolutionDateByUniversityCouncil.ToDefaultTimeZone().Value.Day} de {registryPattern.ResolutionDateByUniversityCouncil.Value.ToString("MMMM", cultureInfo)} del {registryPattern.ResolutionDateByUniversityCouncil.ToDefaultTimeZone().Value.Year}" : "-",
                UniveristaryDateDay = registryPattern.UniversityCouncilDate.HasValue ? registryPattern.UniversityCouncilDate.ToDefaultTimeZone().Value.Day.ToString() : "",
                UniveristaryDateMonth = registryPattern.UniversityCouncilDate.HasValue ? registryPattern.UniversityCouncilDate.Value.ToString("MMMM", cultureInfo) : "",
                UniveristaryDateYear = registryPattern.UniversityCouncilDate.HasValue ? registryPattern.UniversityCouncilDate.ToDefaultTimeZone().Value.Year.ToString() : "",
                GradeTypeDescription = ConstantHelpers.GRADE_INFORM.DegreeType.VALUES.ContainsKey(registryPattern.GradeType) ? ConstantHelpers.GRADE_INFORM.DegreeType.VALUES[registryPattern.GradeType] : "--",
                Shield = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/themes/unjbg/logo-sm.png"),
                NationalEmblem = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/diploma/escudo.png"),
                OriginDiplomatDateBoolean = registryPattern.OriginDiplomatDate.HasValue,
                DuplicatedDiplomaDate = registryPattern.DuplicateDiplomatDate.HasValue ? registryPattern.DuplicateDiplomatDate.Value.ToLocalDateFormat() : "-",
                OriginalDiplomaDate = registryPattern.OriginDiplomatDate.HasValue ? registryPattern.OriginDiplomatDate.Value.ToLocalDateFormat() : "-",

            };

            var resultAcreditation = false;

            if (registryPattern.OriginDiplomatDate.HasValue)
            {
                foreach (var ca in careerAcreditations)
                {
                    if (ca.StartDate <= registryPattern.OriginDiplomatDate.Value && registryPattern.OriginDiplomatDate.Value <= ca.EndDate)
                    {
                        resultAcreditation = true;
                        break;
                    }
                }
            }
            result.IsAccredited = resultAcreditation;

            return result;
        }

        public async Task<List<FormatRegisterTemplate>> GetFormatRegisterTemplate(string searchValue, string searchBookNumber, string dateStartFilter, string dateEndFilter)
        {
            var pre_query = _context.RegistryPatterns.Include(x => x.Student.User).Include(x => x.Student.Career.Faculty).AsQueryable();

            if (!String.IsNullOrEmpty(dateStartFilter) && !String.IsNullOrEmpty(dateEndFilter))
            {
                var dateStartDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateStartFilter);
                var dateEndDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateEndFilter);
                pre_query = pre_query.Where(x => (dateStartDateTime.Date <= x.UniversityCouncilDate.Value.Date) && (x.UniversityCouncilDate.Value.Date <= dateEndDateTime.Date));
            }

            if (!String.IsNullOrEmpty(searchValue))
            {
                pre_query = pre_query.Where(x => x.Student.User.FullName.ToLower().Contains(searchValue.ToLower())
                || x.Student.User.Dni.ToLower().Contains(searchValue.ToLower()));
            }
            if (!String.IsNullOrEmpty(searchBookNumber))
            {
                pre_query = pre_query.Where(x => x.BookCode.ToLower().Contains(searchBookNumber.ToLower()));
            }

            var query = await pre_query.Where(x => x.Status == ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED).
                Select(x => new FormatRegisterTemplate
                {
                    Book = x.BookCode,
                    Career = x.Student.Career.Name,
                    DiplomaNum = x.DiplomatNumber,
                    Faculty = x.Student.Career.Faculty.Name,
                    Folio = x.FolioCode,
                    FullName = x.Student.User.FullName,
                    GradeType = (x.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE ? "Titulado" : "Bachiller"),
                    GradeTypeInt = x.GradeType,
                    RegistryNumber = x.RegistryNumber,
                    Resolution = x.ResolutionNumber,
                    University = GeneralHelpers.GetInstitutionName(),
                    AcademicDegreeDenomination = x.AcademicDegreeDenomination,
                    DNI = x.Student.User.Dni,
                    Name = x.Student.User.Name,
                    PaternalSurname = x.Student.User.PaternalSurname,
                    UserPhoneNumber = x.Student.User.PhoneNumber,
                    MaternalSurname = x.Student.User.MaternalSurname,
                    Image = (String.IsNullOrEmpty(x.Student.User.Picture)) ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/demo/user.png") : x.Student.User.Picture,
                    ManagingDirector = x.ManagingDirector,
                    ManagingDirectorFullName = x.ManagingDirectorFullName,
                    GeneralSecretary = x.GeneralSecretary,
                    GeneralSecretaryFullName = x.GeneralSecretaryFullName,
                    AcademicResponsible = x.AcademicResponsible,
                    AcademicResponsibleFullName = x.AcademicResponsibleFullName,
                    ResolutionDateByUniversityCouncil = x.ResolutionDateByUniversityCouncil.HasValue ? x.ResolutionDateByUniversityCouncil.Value.ToLocalDateFormat() : null,
                    OriginDiplomatDate = x.OriginDiplomatDate.HasValue ? x.OriginDiplomatDate.Value.ToLocalDateFormat() : null,

                }).ToListAsync();

            return query;
        }


        public async Task<object> GetReportProfessionalTitleByModality(string dateStartFilter, string dateEndFilter, List<Guid> Careers = null)
        {
            var query = _context.RegistryPatterns.Where(x => x.Status == ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED && x.Student.Status == ConstantHelpers.Student.States.QUALIFIED).AsQueryable();

            if (Careers != null)
            {
                query = query.Where(x => Careers.Contains(x.Student.CareerId));
            }
            if (!String.IsNullOrEmpty(dateStartFilter) && !String.IsNullOrEmpty(dateEndFilter))
            {
                var dateStartDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateStartFilter);
                var dateEndDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateEndFilter);
                query = query.Where(x => (dateStartDateTime.Date <= x.CreatedAt.Value.Date) && (x.CreatedAt.Value.Date <= dateEndDateTime.Date));

            }

            var result = await query.GroupBy(x => new { x.ObtainingDegreeModality }).Select(x => new
            {
                Year = x.Key.ObtainingDegreeModality,
                TitleCount = x.Count()
            }).ToListAsync();

            return result;

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDiplomasByDegreeModality(DataTablesStructs.SentParameters sentParameters, string degreeModality, ClaimsPrincipal user = null)
        {
            Expression<Func<RegistryPattern, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.Dni;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.User.Email;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
            }

            var query = _context.RegistryPatterns.Where(x => x.ObtainingDegreeModality == degreeModality)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    user = x.Student.User.FullName,
                    dni = x.Student.User.Dni,
                    email = x.Student.User.Email,
                    career = x.Student.Career.Name
                })
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetDiplomasByDegreeModalityChart(string degreeModality, ClaimsPrincipal user = null)
        {
            var query = _context.RegistryPatterns
                .Where(x => x.ObtainingDegreeModality == degreeModality)
                .AsNoTracking();

            var careersQuery = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            var data = await careersQuery
                .Select(x => new
                {
                    Career = x.Name,
                    Count = query.Where(y => y.Student.CareerId == x.Id).Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Career)
                .ToListAsync();


            var result = new
            {
                categories = data.Select(x => x.Career).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<List<RegistryPattern>> GetRegistryPatternsListByBookNumber(string bookNumer)
        {
            var result = await _context.RegistryPatterns.Include(x => x.Student.User).Where(x => x.BookCode.ToLower().Contains(bookNumer.ToLower()) && x.Status == ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED).ToListAsync();
            return result;
        }


        public async Task<int> CurrentCountByGradeType(int gradeType)
        {
            var result = await _context.RegistryPatterns.Where(x => x.GradeType == gradeType).CountAsync();
            return result;
        }

        public async Task<object> GetStudentBasicInformation(Guid registryPatternId)
        {
            var result = await _context.RegistryPatterns.Include(x => x.Student.User).Where(x => x.Id == registryPatternId)
                .Select(x => new
                {
                    fullname = x.Student.User.FullName,
                    code = x.Student.User.UserName,
                    phone = x.Student.User.PhoneNumber,
                    email1 = x.Student.User.Email,
                    email2 = x.Student.User.PersonalEmail
                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<RegistryPatternBook> GetRegistryPatternBook()
        {
            try
            {
                var result = new RegistryPatternBook();
                var totalRegistryNumber = await _context.RegistryPatterns.CountAsync();
                var currentBook = (int)Math.Round((totalRegistryNumber / 400M), 0, MidpointRounding.ToPositiveInfinity);
                var currentRegister = totalRegistryNumber % 200;
                var currentFolio = Math.Round((currentRegister / 2M), 0, MidpointRounding.AwayFromZero);

                var nextRegister = currentRegister + 1;
                if (nextRegister > 400)
                {
                    currentBook = +1;
                    nextRegister = 1;
                    currentFolio = 1;
                }

                result.BookCode = ArabicToRoman(currentBook);
                result.FolioCode = $"{currentFolio}";
                result.RegistryNumber = $"{currentRegister}";

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ArabicToRoman(int arabic)
        {
            string[] ThouLetters = { "", "M", "MM", "MMM" };
            string[] HundLetters = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
            string[] TensLetters = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
            string[] OnesLetters = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
            // See if it's >= 4000.
            if (arabic >= 4000)
            {
                // Use parentheses.
                int thou = arabic / 1000;
                arabic %= 1000;
                return "(" + ArabicToRoman(thou) + ")" +
                    ArabicToRoman(arabic);
            }

            // Otherwise process the letters.
            string result = "";

            // Pull out thousands.
            int num;
            num = arabic / 1000;
            result += ThouLetters[num];
            arabic %= 1000;

            // Handle hundreds.
            num = arabic / 100;
            result += HundLetters[num];
            arabic %= 100;

            // Handle tens.
            num = arabic / 10;
            result += TensLetters[num];
            arabic %= 10;

            // Handle ones.
            result += OnesLetters[arabic];

            return result;
        }

        private static int RomanToArabic(string roman)
        {
            Dictionary<char, int> RomanMap = new Dictionary<char, int>()
            {
                {'I', 1},
                {'V', 5},
                {'X', 10},
                {'L', 50},
                {'C', 100},
                {'D', 500},
                {'M', 1000}
            };

            int number = 0;
            for (int i = 0; i < roman.Length; i++)
            {
                if (i + 1 < roman.Length && RomanMap[roman[i]] < RomanMap[roman[i + 1]])
                {
                    number -= RomanMap[roman[i]];
                }
                else
                {
                    number += RomanMap[roman[i]];
                }
            }

            return number;
        }

        public async Task<RegistryPattern> GetByStudentIdAndGradeType(Guid studentId, int gradeType)
        {
            var result = await _context.RegistryPatterns
                .Where(x => x.StudentId == studentId && x.GradeType == gradeType)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<RegistryPatternReportTemplate>> GetRegistryPatternData()
        {
            var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM).FirstOrDefaultAsync();

            var isIntegrated = bool.Parse(configuration.Value);

            var registryPatterns = await _context.RegistryPatterns
                            .Select(x => new RegistryPatternReportTemplate
                            {
                                Id = x.Id,
                                UserName = x.Student.User.UserName,
                                PaternalSurname = x.Student.User.PaternalSurname,
                                MaternalSurname = x.Student.User.MaternalSurname,
                                Name = x.Student.User.Name,
                                Dni = x.Student.User.Dni,
                                CareerName = x.Student.Career.Name,
                                GradeType = ConstantHelpers.GRADE_INFORM.DegreeType.VALUES.ContainsKey(x.GradeType) ? ConstantHelpers.GRADE_INFORM.DegreeType.VALUES[x.GradeType].ToUpper() : "--",
                                RequestName = (isIntegrated) ? x.Procedure.Name : x.Concept.Description,
                                DiplomaDelivery = ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.VALUES.ContainsKey(x.DiplomaStatus) ? ConstantHelpers.REGISTRY_PATTERN.DIPLOMA_DELIVERY.VALUES[x.DiplomaStatus] : "--"
                            })
                            .ToListAsync();

            return registryPatterns;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
