using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.INTRANET.Areas.Admin.Models.ConfigurationVIewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Services.EvaluationReportGenerator;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RecordConcept;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Route("admin/configuracion")]
    public class ConfigurationController : BaseController
    {
        private readonly IConfigurationService _configurationService;
        private readonly IRecordsConceptService _recordsConceptService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IEvaluationReportGeneratorService _evaluationReportGeneratorService;

        public ConfigurationController(
            IOptions<CloudStorageCredentials> storageCredentials,
            IEvaluationReportGeneratorService evaluationReportGeneratorService,
            IConfigurationService configurationService,
            IRecordsConceptService recordsConceptService
            ) : base()
        {
            _configurationService = configurationService;
            _recordsConceptService = recordsConceptService;
            _storageCredentials = storageCredentials;
            _evaluationReportGeneratorService = evaluationReportGeneratorService;
        }

        /// <summary>
        /// Vista principal donde se cargan las variables de configuración del sistema
        /// </summary>
        /// <returns>Retorna la vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var values = await _configurationService.GetDataDictionary();

            var min_substitute_examen = int.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.MIN_SUBSTITUTE_EXAMEN));
            var substitute_exam_evaluation_type = int.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAMEN_EVALUATION_TYPE));

            var integratedSystemConfi = await _configurationService.GetByKey(ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM);
            if (integratedSystemConfi is null)
            {
                integratedSystemConfi = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM,
                    Value = ConstantHelpers.Configuration.General.DEFAULT_VALUES[ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM]
                };

                await _configurationService.Insert(integratedSystemConfi);
            }

            var integratedSystem = bool.Parse(integratedSystemConfi.Value);
            //var max_substitute_examen = int.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.MAX_SUBSTITUTE_EXAMEN));
            //
            var cycle_withdrwal_concept = Guid.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT));
            var course_withdrwal_concept = Guid.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT));
            var substitute_exam_concept = Guid.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT));
            var evaluation_type_grade_recovery = Guid.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY));
            var min_grade_recovery = int.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MIN_EVALUATION_GRADE));
            var min_grade_deferred_exam = int.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.MIN_AVG_DEFERRED_EXAM));

            //
            var cycle_withdrwal_concept_auto = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT_AUTO));
            var course_withdrwal_concept_auto = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT_AUTO));
            var substitute_exam_concept_auto = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT_AUTO));
            var enable_survey_required_feature = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.SURVEY_ENFORCE_REQUIRED));
            var enable_student_payment_feature = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.ENABLE_STUDENT_PAYMENT));

            //DOCUMENTOS
            var documentMainCampus = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_CAMPUS);
            var documentMainOffice = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_OFFICE);
            var documentOffice = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_OFFICE);
            var documentTechnologyOffice = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_TECHNOLOGYOFFICE);
            var documentSender = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_SENDER);
            var documentSenderCoordinator = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_SENDER_COORDINATOR);
            var documentAcademicCharge = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_ACADEMIC_CHARGE);

            var enrollmentReportHeaderText = GetConfigurationValue(values, ConstantHelpers.Configuration.Enrollment.ENROLLMENT_REPORT_HEADER_TEXT);
            var enrollmentReportSubheaderText = GetConfigurationValue(values, ConstantHelpers.Configuration.Enrollment.ENROLLMENT_REPORT_SUBHEADER_TEXT);
            var enrollmentReportFooterText = GetConfigurationValue(values, ConstantHelpers.Configuration.Enrollment.ENROLLMENT_REPORT_FOOTER_TEXT);

            var enrollmentProformaTitleText = GetConfigurationValue(values, ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PROFORMA_TITLE_TEXT);
            var enrollmentProformaFooterText = GetConfigurationValue(values, ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PROFORMA_FOOTER_TEXT);

            var documentLogoPath = GetConfigurationValue(values, ConstantHelpers.Configuration.General.DOCUMENT_LOGO_PATH);
            var documentSuperiorText = GetConfigurationValue(values, ConstantHelpers.Configuration.General.DOCUMENT_SUPERIOR_TEXT);
            var documentHeaderText = GetConfigurationValue(values, ConstantHelpers.Configuration.General.DOCUMENT_HEADER_TEXT);
            var documentSubheaderText = GetConfigurationValue(values, ConstantHelpers.Configuration.General.DOCUMENT_SUBHEADER_TEXT);

            //var record_automatic_procedure = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.RECORD_AUTOMATIC_PROCEDURE));
            var record_rectification_charge = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.RECORD_RECTIFICATION_CHARGE));
            var record_rectification_charge_note = await _recordsConceptService.GetValueByRecordType(ConstantHelpers.RECORDS.RECTIFICATIONCHARGENOTE) ?? Guid.Empty;

            var meritOrderModality = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.MERIT_ORDER_BY_ACADEMIC_YEAR));
            var meritOrderGradeType = byte.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.MERIT_ORDER_GRADE_TYPE));

            var enrollmentPaymentMethod = byte.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PAYMENT_METHOD));
            var exemptFirstPlaces = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.Enrollment.EXEMPT_FIRST_PLACES_FROM_PAYMENTS));
            var exemptionType = byte.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.Enrollment.PAYMENT_EXEMPTION_TYPE));
            var firstPlacesQuantity = int.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.Enrollment.FIRST_PLACES_QUANTITY));

            var evaluationReportWithRegister = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_WITH_REGISTER));


            var medicalRecord = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.InstitutionalWelfareManagement.MEDICAL_RECORD_VISIBILITY));

            var portSmtp = Convert.ToInt32(GetConfigurationValue(values, ConstantHelpers.Configuration.Email.GENERAL_EMAIL_SMTP_PORT));
            var hostSmtp = GetConfigurationValue(values, ConstantHelpers.Configuration.Email.GENERAL_EMAIL_SMTP_HOST);
            var senderEmail = GetConfigurationValue(values, ConstantHelpers.Configuration.Email.GENERAL_EMAIL);
            var senderPassword = GetConfigurationValue(values, ConstantHelpers.Configuration.Email.GENERAL_EMAIL_PASSWORD);

            //EVALUTION REPORT
            var evaluationReportHeader = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER);
            var evaluationReportSubHeader = GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER);
            var evaluationReportActFormat = Convert.ToInt32(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_ACT_FORMAT));
            var evaluationReportRegisterFormat = Convert.ToInt32(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_REGISTER_FORMAT));
            var enabledPartialEvaluationReportRegister = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.ENABLED_PARTIAL_EVALUATION_REPORT_REGISTER));
            var enabledAuxiliaryEvaluationReport = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.IntranetManagement.ENABLED_AUXILIARY_EVALUATION_REPORT));

            var exoneratedCourseEnrollmentGrade = decimal.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.Enrollment.EXONERATED_COURSE_ENROLLMENT_AVERAGE_GRADE));
            var requireEnrollmentForReservation = bool.Parse(GetConfigurationValue(values, ConstantHelpers.Configuration.Enrollment.REQUIRE_ENROLLMENT_FOR_RESERVATION));

            var studentInformationVisibility = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_VISIBILITY));
            var institutionalWelfareSurveyVisibility = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.INSTITUTIONAL_WELFARE_SURVEY_VISIBILITY));

            var enableStudentGradeCorrectionRequest = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLE_STUDENT_GRADE_CORRECTION_REQUEST));
            var studentGradeCorrectionMaxDays = Convert.ToInt16(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.STUDENT_GRADE_CORRECTION_REQUEST_MAX_DAYS));

            var enabledExtraordinaryEvaluation = Convert.ToBoolean(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_EXTRAORDINARY_EVALUATION));
            var enabledGradeRecovery = Convert.ToBoolean(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_GRADE_RECOVERY));
            var enabledSubstituteExam = Convert.ToBoolean(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_SUBSTITUTE_EXAM));
            var enabledDeferredExam = Convert.ToBoolean(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_DEFERRED_EXAM));

            var loginBackgroundImagePath = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.INT_LOGIN_BACKGROUND_IMAGE);

            var gradesCanOnlyPublishedByPrincipalTeacher = Convert.ToBoolean(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.GRADES_CAN_ONLY_PUBLISHED_BY_PRINCIPAL_TEACHER));
            var gradeRecoveryEnabledToApproved = Convert.ToBoolean(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_ENABLED_TO_APPROVED));
            var gradeRecoveryModality = Convert.ToByte(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MODALITY));

            var careerDirectorGradeCorrection = Convert.ToBoolean(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.CAREER_DIRECTOR_GRADE_CORRECTION));
            var academicDepartmentGradeCorrection = Convert.ToBoolean(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ACADEMIC_DEPARTMENT_GRADE_CORRECTION));

            var academicRecordSigning = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ACADEMIC_RECORD_SIGNING);

            var bossPositionRecordSigning = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.BOSS_POSITION_RECORD_SIGNING);

            var formatCertificateOfStudies = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.RecordFormat.CERTIFICATEOFSTUDIES);
            var formatFirstEnrollment = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.RecordFormat.FIRSTENROLLMENT);

            var extraordinaryEvaluationTypesEnabled = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.EXTRAORDINARY_EVALUATION_TYPES_ENABLED);

            var imageWatermarkRecord = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD);

            var evaluationReportFormatDate = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE);

            var imageCertificateSignatureUrl = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.IMAGE_CERTIFICATE_SIGNATURE);

            var enabledSpecialAbsencePercentage = Convert.ToBoolean(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_SPECIAL_ABSENCE_PERCENTAGE));
            decimal.TryParse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE), out var specialAbsencePercentage);
            var specialAbsencePercentageDescription = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE_DESCRIPTION);

            int.TryParse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.General.PASSWORD_EXPIRATION_DAYS), out var passwordExpirationDays);


            var model = new ConfigurationViewModel()
            {
                MedicalRecord = medicalRecord,
                IntegratedSystem = integratedSystem,

                EnableSurveyRequiredFeature = enable_survey_required_feature,
                EnableStudentPaymentFeature = enable_student_payment_feature,
                //MaxAverageForSubstituteExam = max_substitute_examen,
                MinAverageForSubstituteExam = min_substitute_examen,
                SubstitutoryExamEvaluationType = substitute_exam_evaluation_type,

                EvaluationTypeGradeRecovery = evaluation_type_grade_recovery,
                MinGradeRecovery = min_grade_recovery,
                //
                CycleWithdrawalConcept = cycle_withdrwal_concept,
                CourseWithdrawalConcept = course_withdrwal_concept,
                SubstitutoryExamConcept = substitute_exam_concept,
                //
                CycleWithdrawalAuto = cycle_withdrwal_concept_auto,
                CourseWithdrawalAuto = course_withdrwal_concept_auto,
                SubstitutoryExamAuto = substitute_exam_concept_auto,
                //Documentos
                DocumentMainCampus = documentMainCampus,
                DocumentMainOffice = documentMainOffice,
                DocumentOffice = documentOffice,
                DocumentTechnologyOffice = documentTechnologyOffice,
                DocumentSender = documentSender,
                DocumentSenderCoordinator = documentSenderCoordinator,
                DocumentAcademicCharge = documentAcademicCharge,

                EnrollmentReportHeaderText = enrollmentReportHeaderText,
                EnrollmentReportSubheaderText = enrollmentReportSubheaderText,

                DocumentLogoPath = documentLogoPath,
                DocumentSuperiorText = documentSuperiorText,
                DocumentHeaderText = documentHeaderText,
                DocumentSubheaderText = documentSubheaderText,

                //Records
                //RecordAutomaticProcedure = record_automatic_procedure,

                RecordRectificationCharge = record_rectification_charge_note,
                RectificationCharge = record_rectification_charge,

                MeritOrderModality = meritOrderModality,
                MeritOrderGradeType = meritOrderGradeType,

                ShowExemptionConfiguration = enrollmentPaymentMethod == 1,
                ExemptFirstPlacesFromPayments = exemptFirstPlaces,
                PaymentExemptionType = exemptionType,
                FirstPlacesQuantity = firstPlacesQuantity,
                EvaluationReportWithRegister = evaluationReportWithRegister,

                //SMTP Configuration
                HostSMTP = hostSmtp,
                PortSMTP = portSmtp,
                SenderEmail = senderEmail,
                SenderEmailPassword = senderPassword,

                //EvaluationReport
                EvaluationReportHeader = evaluationReportHeader,
                EvaluationReportSubHeader = evaluationReportSubHeader,
                EvaluationReportActFormat = evaluationReportActFormat,
                EvaluationReportRegisterFormat = evaluationReportRegisterFormat,
                EnabledPartialEvaluationReportRegister = enabledPartialEvaluationReportRegister,
                EnabledAuxiliaryEvaluationReport = enabledAuxiliaryEvaluationReport,
                EnrollmentReportFooterText = enrollmentReportFooterText,
                ExoneratedCourseEnrollmentGrade = exoneratedCourseEnrollmentGrade,
                RequireEnrollmentForReservation = requireEnrollmentForReservation,

                InstitutionalWelfareSurveyVisibility = institutionalWelfareSurveyVisibility,
                StudentInformationVisibility = studentInformationVisibility,

                EnrollmentProformaFooterText = enrollmentProformaFooterText,
                EnrollmentProformaTitleText = enrollmentProformaTitleText,

                EnableStudentGradeCorrectionRequest = enableStudentGradeCorrectionRequest,
                StudentGradeCorrectionRequestMaxDays = studentGradeCorrectionMaxDays,

                EnabledExtraordinaryEvaluation = enabledExtraordinaryEvaluation,
                EnabledGradeRecovery = enabledGradeRecovery,
                EnabledSubstituteExam = enabledSubstituteExam,
                EnabledDeferredExam = enabledDeferredExam,

                IntranetLoginBackgroundImagePath = loginBackgroundImagePath,

                GradesCanOnlyPublishedByPrincipalTeacher = gradesCanOnlyPublishedByPrincipalTeacher,

                MinAvgDeferredExam = min_grade_deferred_exam,

                GradeRecoveryEnabledToApproved = gradeRecoveryEnabledToApproved,
                GradeRecoveryModality = gradeRecoveryModality,

                AcademicDepartmentGradeCorrection = academicDepartmentGradeCorrection,
                CareerDirectorGradeCorrection = careerDirectorGradeCorrection,

                AcademicRecordSigning = academicRecordSigning,
                BossPositionRecordSigning = bossPositionRecordSigning,

                FormatCertificateOfStudies = formatCertificateOfStudies,
                FormatFirstEnrollment = formatFirstEnrollment,

                ExtraordinaryEvaluationTypesEnabled = extraordinaryEvaluationTypesEnabled,

                EnabledImageWatermarkRecord = !string.IsNullOrEmpty(imageWatermarkRecord),
                ImageWatermarkRecordUrl = imageWatermarkRecord,

                EvaluationReportFormatDate = Convert.ToByte(evaluationReportFormatDate),

                CertificateSignatureImageUrl = imageCertificateSignatureUrl,

                EnabledSpecialAbsencePercentage = enabledSpecialAbsencePercentage,
                SpecialAbsencePercentage = specialAbsencePercentage,
                SpecialAbsencePercentageDescription = specialAbsencePercentageDescription,

                PasswordExpirationDays = passwordExpirationDays
            };

            model.EvaluationTypes = ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_TYPE.VALUES.Select(x => new SelectListItem
            {
                Value = x.Key.ToString(),
                Text = x.Value,
                Selected = substitute_exam_evaluation_type == x.Key
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// Actualiza las variables de configuración
        /// </summary>
        /// <param name="model">Objeto que contiene las variables de configuración actualizadas</param>
        /// <returns>Devuelve un Ok o BadRequest dependiendo si se logro o no actualizar las variables de configuración.</returns>
        [HttpPost("actualizar")]
        public async Task<IActionResult> UpdateConfigurations(ConfigurationViewModel model)
        {
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM, model.IntegratedSystem.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SURVEY_ENFORCE_REQUIRED, model.EnableSurveyRequiredFeature.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLE_STUDENT_PAYMENT, model.EnableStudentPaymentFeature.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.MIN_SUBSTITUTE_EXAMEN, model.MinAverageForSubstituteExam.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAMEN_EVALUATION_TYPE, model.SubstitutoryExamEvaluationType.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY, model.EvaluationTypeGradeRecovery.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MIN_EVALUATION_GRADE, model.MinGradeRecovery.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.MIN_AVG_DEFERRED_EXAM, model.MinAvgDeferredExam.ToString());

            //
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT, model.CourseWithdrawalConcept.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT, model.CycleWithdrawalConcept.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT, model.SubstitutoryExamConcept.ToString());
            //
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT_AUTO, model.CourseWithdrawalAuto.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT_AUTO, model.CycleWithdrawalAuto.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT_AUTO, model.SubstitutoryExamAuto.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_WITH_REGISTER, model.EvaluationReportWithRegister.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.InstitutionalWelfareManagement.MEDICAL_RECORD_VISIBILITY, model.MedicalRecord.ToString());


            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_VISIBILITY, model.StudentInformationVisibility.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.InstitutionalWelfareManagement.INSTITUTIONAL_WELFARE_SURVEY_VISIBILITY, model.InstitutionalWelfareSurveyVisibility.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLED_GRADE_RECOVERY, model.EnabledGradeRecovery.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLED_SUBSTITUTE_EXAM, model.EnabledSubstituteExam.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLED_EXTRAORDINARY_EVALUATION, model.EnabledExtraordinaryEvaluation.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLED_DEFERRED_EXAM, model.EnabledDeferredExam.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_ENABLED_TO_APPROVED, model.GradeRecoveryEnabledToApproved.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MODALITY, model.GradeRecoveryModality.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.CAREER_DIRECTOR_GRADE_CORRECTION, model.CareerDirectorGradeCorrection.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ACADEMIC_DEPARTMENT_GRADE_CORRECTION, model.AcademicDepartmentGradeCorrection.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ACADEMIC_RECORD_SIGNING, string.IsNullOrEmpty(model.AcademicRecordSigning) ? "" : model.AcademicRecordSigning.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.BOSS_POSITION_RECORD_SIGNING, string.IsNullOrEmpty(model.BossPositionRecordSigning) ? "" : model.BossPositionRecordSigning.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.RecordFormat.CERTIFICATEOFSTUDIES, model.FormatCertificateOfStudies);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.RecordFormat.FIRSTENROLLMENT, model.FormatFirstEnrollment);

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.EXTRAORDINARY_EVALUATION_TYPES_ENABLED, model.ExtraordinaryEvaluationTypesEnabled);

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE, model.EvaluationReportFormatDate.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLED_SPECIAL_ABSENCE_PERCENTAGE, model.EnabledSpecialAbsencePercentage.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE, model.SpecialAbsencePercentage.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.General.PASSWORD_EXPIRATION_DAYS, model.PasswordExpirationDays.ToString());

            if (model.EnabledSpecialAbsencePercentage)
            {
                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE_DESCRIPTION, model.SpecialAbsencePercentageDescription.ToString());
            }
            else
            {
                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE_DESCRIPTION, "");
            }


            var storage = new CloudStorageService(_storageCredentials);

            if (model.IntranetLoginBackgroundImage != null)
            {
                var imagePath = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.INT_LOGIN_BACKGROUND_IMAGE);

                if (!string.IsNullOrEmpty(imagePath))
                    await storage.TryDelete(imagePath, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION);

                var fileUrl = await storage.UploadFile(model.IntranetLoginBackgroundImage.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION,
                    Path.GetExtension(model.IntranetLoginBackgroundImage.FileName));

                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.INT_LOGIN_BACKGROUND_IMAGE, fileUrl);
            }

            var imagePathWatermark = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD);

            if (!model.EnabledImageWatermarkRecord)
            {
                if (!string.IsNullOrEmpty(imagePathWatermark))
                    await storage.TryDelete(imagePathWatermark, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION);

                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD, "");
            }

            if (model.ImageWatermarkRecord != null)
            {
                if (!string.IsNullOrEmpty(imagePathWatermark))
                    await storage.TryDelete(imagePathWatermark, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION);

                var fileUrl = await storage.UploadFile(model.ImageWatermarkRecord.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION,
                    Path.GetExtension(model.ImageWatermarkRecord.FileName));

                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.IMAGE_WATERMARK_RECORD, fileUrl);
            }

            var imageCertificateSignatureUrl = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.IMAGE_CERTIFICATE_SIGNATURE);

            if (model.CertificateSignatureImage != null)
            {
                if (!string.IsNullOrEmpty(imageCertificateSignatureUrl))
                    await storage.TryDelete(imageCertificateSignatureUrl, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION);

                var fileUrl = await storage.UploadFile(model.CertificateSignatureImage.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION,
                    Path.GetExtension(model.CertificateSignatureImage.FileName));

                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.IMAGE_CERTIFICATE_SIGNATURE, fileUrl);
            }

            //Records
            var conceptRecords = new List<RecordConceptSaveTemplate>
            {
                new RecordConceptSaveTemplate
                {
                    ConceptId = model.RecordRectificationCharge,
                    RecordType = ConstantHelpers.RECORDS.RECTIFICATIONCHARGENOTE
                }
            };
            //var result = await _recordsConceptService.SaveRecordsConcept(conceptRecords);

            //if (!string.IsNullOrEmpty(result))
            //    return BadRequest(result);

            //await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.RECORD_AUTOMATIC_PROCEDURE, model.RecordAutomaticProcedure.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.RECORD_RECTIFICATION_CHARGE, model.RectificationCharge.ToString());

            //Documentos
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_CAMPUS, model.DocumentMainCampus);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_MAIN_OFFICE, model.DocumentMainOffice);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_OFFICE, model.DocumentOffice);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_TECHNOLOGYOFFICE, model.DocumentTechnologyOffice);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_SENDER, model.DocumentSender);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_SENDER_COORDINATOR, model.DocumentSenderCoordinator);


            var enrollmentPaymentMethod = byte.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PAYMENT_METHOD));
            if (enrollmentPaymentMethod == 1)
            {
                //Se actualiza la info de exoneracion
                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Enrollment.EXEMPT_FIRST_PLACES_FROM_PAYMENTS, model.ExemptFirstPlacesFromPayments.ToString());
                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Enrollment.PAYMENT_EXEMPTION_TYPE, model.PaymentExemptionType.ToString());
                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Enrollment.FIRST_PLACES_QUANTITY, model.FirstPlacesQuantity.ToString());
            }

            //SMTP
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Email.GENERAL_EMAIL, model.SenderEmail);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Email.GENERAL_EMAIL_PASSWORD, model.SenderEmailPassword);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Email.GENERAL_EMAIL_SMTP_HOST, model.HostSMTP);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Email.GENERAL_EMAIL_SMTP_PORT, model.PortSMTP.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER, model.EvaluationReportHeader);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER, model.EvaluationReportSubHeader);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_ACT_FORMAT, model.EvaluationReportActFormat.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_REGISTER_FORMAT, model.EvaluationReportRegisterFormat.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLED_PARTIAL_EVALUATION_REPORT_REGISTER, model.EnabledPartialEvaluationReportRegister.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLED_AUXILIARY_EVALUATION_REPORT, model.EnabledAuxiliaryEvaluationReport.ToString());

            if (model.DocumentLogoFile != null)
            {
                var documentLogoPath = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.DOCUMENT_LOGO_PATH);
                if (!string.IsNullOrEmpty(documentLogoPath))
                    await storage.TryDelete(documentLogoPath, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION);

                var fileUrl = await storage.UploadFile(model.DocumentLogoFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION,
                    Path.GetExtension(model.DocumentLogoFile.FileName));
                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.General.DOCUMENT_LOGO_PATH, fileUrl);
            }

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.General.DOCUMENT_SUPERIOR_TEXT, model.DocumentSuperiorText);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.General.DOCUMENT_HEADER_TEXT, model.DocumentHeaderText);
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.General.DOCUMENT_SUBHEADER_TEXT, model.DocumentSubheaderText);

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.MERIT_ORDER_BY_ACADEMIC_YEAR, model.MeritOrderModality.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.MERIT_ORDER_GRADE_TYPE, model.MeritOrderGradeType.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_REPORT_HEADER_TEXT, model.EnrollmentReportHeaderText?.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_REPORT_SUBHEADER_TEXT, model.EnrollmentReportSubheaderText?.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_REPORT_FOOTER_TEXT, model.EnrollmentReportFooterText?.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PROFORMA_FOOTER_TEXT, model.EnrollmentProformaFooterText?.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PROFORMA_TITLE_TEXT, model.EnrollmentProformaTitleText?.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLE_STUDENT_GRADE_CORRECTION_REQUEST, model.EnableStudentGradeCorrectionRequest.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.STUDENT_GRADE_CORRECTION_REQUEST_MAX_DAYS, model.StudentGradeCorrectionRequestMaxDays.ToString());
            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.Enrollment.REQUIRE_ENROLLMENT_FOR_RESERVATION, model.RequireEnrollmentForReservation.ToString());

            await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.GRADES_CAN_ONLY_PUBLISHED_BY_PRINCIPAL_TEACHER, model.GradesCanOnlyPublishedByPrincipalTeacher.ToString());

            return Ok();
        }

        /// <summary>
        /// Obtiene la variable de configuración en base a los siguientes parámetros
        /// </summary>
        /// <param name="key">Identificador de la variable</param>
        /// <returns>Devuelve el valor de la configuración</returns>
        private string GetConfigurationValue(Dictionary<string, string> list, string key)
        {
            return list.ContainsKey(key) ? list[key] :
                CORE.Helpers.ConstantHelpers.Configuration.General.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.General.DEFAULT_VALUES[key] :

                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[key] :

                CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[key] :

                CORE.Helpers.ConstantHelpers.Configuration.Email.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.Email.DEFAULT_VALUES[key] :

                CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.InstitutionalWelfareManagement.DEFAULT_VALUES[key] : "";


        }

        [HttpGet("obtener-previsualizacion-acta/formato/{format}")]
        public async Task<IActionResult> GetEValuationReportPreview(byte format)
        {
            var result = await _evaluationReportGeneratorService.GetActEvaluationReportPreview(format);
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(result.Pdf, "application/pdf", $"{result.PdfName}.pdf");
        }
    }
}
