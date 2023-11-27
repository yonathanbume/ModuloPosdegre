using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template.CafobeRequest
{
    public class CafobeRequestDetailTemplate
    {
        public Guid CafobeRequestId { get; set; }
        public int Status { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS;
        public string StatusText { get; set; }
        public string Comentary { get; set; }
        public string FileDetailUrl { get; set; }
        public DateTime RegisterDate { get; set; }

        public Guid StudentId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Guid TermId { get; set; }
        public string TermName { get; set; }
        public string CareerName { get; set; }
        public int CafobeRequestType { get; set; }
        public string CafobeRequestTypeText { get; set; }
        public int CafobeRequestStatus { get; set; }
        public string CafobeRequestStatusText { get; set; }
        public Guid FacultyId { get; set; }

        public string FacultyName { get; set; }
        public int Type { get; set; }
        public string TypeText { get; set; }
        public int Sex { get; set; }
        public string SexText { get; set; }
        public string Observation { get; set; } //Observaciones
        public string ApprovedResolutionUrl { get; set; } //Documento de Resolucion de aprobación de la solicitud
        public string SystemUrl { get; set; } //Enlace publico del sistema
        //Generic Requirements
        public string DirectorRequestUrl { get; set; } //Solicitud Dirigida al Director de la DBU (Dirección de Bienestar Universitario)
        public string DocumentaryProcedureVoucherUrl { get; set; } //Recibo de pago por tramite Documentario
        public string EnrollmentFormUrl { get; set; } //Ficha de Matrícula Actualizada
        public string LastTermHistoriesUrl { get; set; } //Boleta de notas del semestre anterior
        public string DniUrl { get; set; } //Copia legible del DNI del estudiante

        #region Alto Rendimiento
        public string ConstancyHigherFifthUrl { get; set; } //Constancia de Pertenecer al Quinto Superior de Facultad (del semestre anterior al que se encuentra)
        public string MeritChartHigherFifthUrl { get; set; } //Cuadro de mérito o listado de alumnos del Quinto Superior otorgado por su facultad (2 semestres consecutivos anteriores al que se encuentra)
        #endregion

        #region Maternidad
        public string BabyBirhtCertificateUrl { get; set; } //Partida o acta de nacimiento original o copia legalizada notarialmente del DNI del bebe hasta los 4 meses de nacido
        public string BabyControlCardUrl { get; set; } //Tarjeta de control del recién nacido
        #endregion

        #region Salud
        public string MedicalRecordUrl { get; set; } //Certificado, constancia o informe médico con diagnósticos en original o copia legalizada notarialmente
        public string TreatmentRecordUrl { get; set; } //Hoja de seguimiento del tratamiento del alumno
        #endregion

        #region Defunción
        public string DeathCertificateUrl { get; set; } //Certificado o acta de defunción original o copia legalizada notarialmente con fecha no mayor a 2 meses (en caso de fallecimiento del alumno) y 4 meses (en caso de fallecimiento de un familiar directo)
        public string StudentBirthCertificateUrl { get; set; } //Partida de nacimiento del estudiante en original o copia legalizada notarialmente
        #endregion

        #region Oftalmológico
        public string OpthicalMedicalDiagnosticUrl { get; set; } //Diagnóstico médico y medida de vista; otorgado por un hospital o centro de salud (MINSA)
        public string OpthicalProformUrl { get; set; } //Proforma Original
        #endregion

        #region Deportista destacado | Beca de estimulo
        public string EventInvitationUrl { get; set; } //Copia legalizada notarialmente o vista por el Decano de su Facultad de la invitación como ponente al evento (congresos nacionales o internacionales)
        public string StudentHealthInsuranceUrl { get; set; } //Copia de seguro de salud con el que cuenta el estudiante
        public string StudentSportParticipationUrl { get; set; } //Para el caso de deportista presentar la hoja de resumen de trayectoria de participación en competencias visado por el área de deportes
        #endregion
    }
}
