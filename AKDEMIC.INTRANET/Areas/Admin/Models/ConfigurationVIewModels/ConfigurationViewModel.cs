using AKDEMIC.CORE.Structs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ConfigurationVIewModels
{
    public class ConfigurationViewModel
    {

        //[Required]
        //[Display(Name = "Porcentaje mínimo de asistencias")]
        //public double MininumAssistance { get; set; }

        //[Required]
        //[Display(Name = "Nota final mínima aprobatoria")]
        //public double MininumAprooving { get; set; }

        //[Required]
        //[Display(Name = "Promedio mínimo aprobatorio para rendir examen aplazado")]
        //public double MininumDeferred{ get; set; }

        [Required]
        [Display(Name = "¿Sistema Integrado?")]
        public bool IntegratedSystem { get; set; }

        [Required]
        [Display(Name = "Habilitar parametro requerido en gestion de encuesta")]
        public bool EnableSurveyRequiredFeature { get; set; }

        [Required]
        [Display(Name = "Habilitar pagos en linea para el estudiante")]
        public bool EnableStudentPaymentFeature { get; set; }

        [Required]
        [Display(Name = "Prom. mín. aprobatorio para examen sustitutorio")]
        public int MinAverageForSubstituteExam { get; set; }
        //[Required]
        //[Display(Name = "Promedio máximo aprobatorio para rendir examen sustitutorio")]
        //public int MaxAverageForSubstituteExam { get; set; }
        //
        [Required]
        [Display(Name = "Concepto para retiro de curso")]
        public Guid CourseWithdrawalConcept { get; set; }
        [Required]
        [Display(Name = "Concepto para retiro de ciclo")]
        public Guid CycleWithdrawalConcept { get; set; }
        [Required]
        [Display(Name = "Concepto para exámen sustitutorio")]
        public Guid SubstitutoryExamConcept { get; set; }
        [Required]
        [Display(Name = "Modalidad de evaluación de sustitutorio")]
        public int SubstitutoryExamEvaluationType { get; set; }

        [Required]
        [Display(Name = "Tipo de eva. que aplica a la recuperación de nota")]
        public Guid? EvaluationTypeGradeRecovery { get; set; }
        [Required]
        [Display(Name = "Nota mínima para la recuperación de nota")]
        public int MinGradeRecovery { get; set; }

        //
        [Required]
        [Display(Name = "¿Se Aplica Automáticamente?")]
        public bool CourseWithdrawalAuto { get; set; }
        [Required]
        [Display(Name = "¿Se Aplica Automáticamente?")]
        public bool CycleWithdrawalAuto { get; set; }
        [Required]
        [Display(Name = "¿Se Aplica Automáticamente?")]
        public bool SubstitutoryExamAuto { get; set; }

        [Required]
        //[Display(Name = "¿Generación de Trámites Automáticos?")]
        //public bool RecordAutomaticProcedure { get; set; }
        [Display(Name = "¿Recuperación de nota habilitado para aprobados?")]
        public bool GradeRecoveryEnabledToApproved { get; set; }

        [Required]
        [Display(Name = "¿Cobrar al docente por Rectificación?")]
        public bool RectificationCharge { get; set; }
        [Display(Name = "Concepto de Rectificación de Nota")]
        public Guid RecordRectificationCharge { get; set; }

        [Display(Name = "Sede Principal")]
        public string DocumentMainCampus { get; set; }

        [Display(Name = "Encabezado de Oficina")]
        public string DocumentMainOffice { get; set; }

        [Display(Name = "Oficina")]
        public string DocumentOffice { get; set; }

        [Display(Name = "Oficina de Tecnología")]
        public string DocumentTechnologyOffice { get; set; }

        [Display(Name = "Subscribe Oficina")]
        public string DocumentSender { get; set; }

        [Display(Name = "Encabezado ficha matrícula")]
        public string EnrollmentReportHeaderText { get; set; }
        [Display(Name = "Subencabezado ficha matrícula")]
        public string EnrollmentReportSubheaderText { get; set; }
        [Display(Name = "Texto inferior en ficha de matrícula")]
        public string EnrollmentReportFooterText { get; set; }

        [Display(Name = "Titulo de la proforma de matrícula")]
        public string EnrollmentProformaTitleText { get; set; }
        [Display(Name = "Texto Inferior de la proforma de matrícula")]
        public string EnrollmentProformaFooterText { get; set; }

        [Display(Name = "Firma de Carga Académica")]
        public string DocumentAcademicCharge { get; set; }

        [Display(Name = "Subscribe Coordinación")]
        public string DocumentSenderCoordinator { get; set; }
        public List<SelectListItem> EvaluationTypes { get; set; }

        [Display(Name = "Calcular orden de mérito por:")]
        public bool MeritOrderModality { get; set; }

        [Display(Name = "Promedio ponderado a usar:")]
        public byte MeritOrderGradeType { get; set; }

        public bool ShowExemptionConfiguration { get; set; }
        [Display(Name = "Habilitar la exoneración de pagos primeros puestos")]
        public bool ExemptFirstPlacesFromPayments { get; set; }

        [Display(Name = "Nro. de puestos considerados dentro de los primeros")]
        public int FirstPlacesQuantity { get; set; }
        [Display(Name = "Tipo de exoneración de pago a aplicar")]
        public byte PaymentExemptionType { get; set; }
        [Display(Name = "¿Acta de Notas con Registro?")]
        public bool EvaluationReportWithRegister { get; set; }

        [Display(Name = "Visualización de la ficha médica")]
        public bool MedicalRecord { get; set; }

        [Display(Name = "Correo enviador")]
        public string SenderEmail { get; set; }

        [Display(Name = "Contraseña del correo enviador")]
        public string SenderEmailPassword { get; set; }

        [Display(Name = "Host SMTP")]
        public string HostSMTP { get; set; }

        [Display(Name = "Puerto SMTP")]
        public int PortSMTP { get; set; }


        //EvaluationReport
        [Display(Name = "Encabezado")]
        public string EvaluationReportHeader { get; set; }

        [Display(Name = "Subencabezado")]
        public string EvaluationReportSubHeader { get; set; }

        [Display(Name = "Formato del Acta")]
        public int EvaluationReportActFormat { get; set; }

        [Display(Name = "Formato del Registro de Acta")]
        public int EvaluationReportRegisterFormat { get; set; }

        [Display(Name = "Logo superior")]
        public IFormFile DocumentLogoFile { get; set; }

        public string DocumentLogoPath { get; set; }

        [Display(Name = "Texto superior Matrícula")]
        public string DocumentSuperiorText { get; set; }

        [Display(Name = "Encabezado del documento")]
        public string DocumentHeaderText { get; set; }

        [Display(Name = "Subencabezado del documento")]
        public string DocumentSubheaderText { get; set; }
        [Display(Name = "Habilitar impresión parcial del registro de evaluación")]
        public bool EnabledPartialEvaluationReportRegister { get; set; }
        [Display(Name = "Habilitar impresión del registro auxiliar de evaluación")]
        public bool EnabledAuxiliaryEvaluationReport { get; set; }

        [Display(Name = "Requerir matrícula del estudiante para realizar reserva")]
        public bool RequireEnrollmentForReservation { get; set; }

        [Display(Name = "Prom. semestral mín. para llevar cursos exonerados")]
        public decimal ExoneratedCourseEnrollmentGrade { get; set; }

        [Display(Name = "Solicitar ficha socioeconómica al estudiante")]
        public bool StudentInformationVisibility { get; set; }

        [Display(Name = "Solicitar ficha de evaluación al estudiante")]
        public bool InstitutionalWelfareSurveyVisibility { get; set; }

        [Display(Name = "Habilitar Solicitud de Corrección de Nota por Estudiante")]
        public bool EnableStudentGradeCorrectionRequest { get; set; }

        [Display(Name = "Cantidad de días posteriores para presentar solicitud de corrección de nota")]
        public int StudentGradeCorrectionRequestMaxDays { get; set; }

        [Display(Name = "Habilitar Evaluación Extraordinaria")]
        public bool EnabledExtraordinaryEvaluation { get; set; }

        [Display(Name = "Habilitar Examen Aplazado")]
        public bool EnabledDeferredExam { get; set; }

        [Display(Name = "Habilitar Examen de Recuperación de Nota")]
        public bool EnabledGradeRecovery { get; set; }

        [Display(Name = "Habilitar Solicitudes de Sustitutorio")]
        public bool EnabledSubstituteExam { get; set; }

        public string IntranetLoginBackgroundImagePath { get; set; }

        [Display(Name = "Imagen de fondo del Login")]
        public IFormFile IntranetLoginBackgroundImage { get; set; }

        [Display(Name = "¿Las notas solo podrán ser publicadas por los docentes principales?")]
        public bool GradesCanOnlyPublishedByPrincipalTeacher { get; set; }

        [Display(Name = "Promedio mínimo para examen aplazado")]
        public int MinAvgDeferredExam { get; set; }

        [Display(Name = "Modalidad de Recuperación de Nota")]
        public byte GradeRecoveryModality { get; set; }

        [Display(Name = "¿Director de Departamento Académico puede aceptar solicitudes de corrección de notas?")]
        public bool AcademicDepartmentGradeCorrection { get; set; }
        [Display(Name = "¿Director de escuela puede aceptar solicitudes de corrección de notas?")]
        public bool CareerDirectorGradeCorrection { get; set; }


        [Display(Name = "Firma Registro Académico")]
        public string AcademicRecordSigning { get; set; }
        [Display(Name = "Cargo Jefe Firma")]
        public string BossPositionRecordSigning { get; set; }

        [Display(Name = "Formato Certificado de Estudios")]
        public string FormatCertificateOfStudies { get; set; }
        [Display(Name = "Formato Constancia Primera Matrícula")]
        public string FormatFirstEnrollment { get; set; }

        [Display(Name = "Tipos de evaluación extraordinaria")]
        public string ExtraordinaryEvaluationTypesEnabled { get; set; }
        [Display(Name = "Habilitar imagen de fondo para constancias")]
        public bool EnabledImageWatermarkRecord { get; set; }

        [Display(Name = "Imagen de fondo para constancias")]
        public IFormFile ImageWatermarkRecord { get; set; }
        public string ImageWatermarkRecordUrl { get; set; }

        [Display(Name = "Fecha del Acta")]
        public byte EvaluationReportFormatDate { get; set; }
        [Display(Name = "Imagen de la firma para constancias")]
        public IFormFile CertificateSignatureImage { get; set; }
        public string CertificateSignatureImageUrl { get; set; }

        [Display(Name = "Habilitar % Inasistencia Especial")]
        public bool EnabledSpecialAbsencePercentage { get; set; }
        [Display(Name = "% Inasistencia Especial")]
        public decimal SpecialAbsencePercentage { get; set; }
        [Display(Name = "Descripción % Inasistencia Especial")]
        public string SpecialAbsencePercentageDescription { get; set; }
        [Display(Name = "Número de días que expira la contraseña")]
        public int PasswordExpirationDays { get; set; }
    }
}
