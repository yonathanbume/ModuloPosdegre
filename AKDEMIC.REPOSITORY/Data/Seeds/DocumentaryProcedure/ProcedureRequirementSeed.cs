using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class ProcedureRequirementSeed
    {
        public static ProcedureRequirement[] Seed(AkdemicContext context)
        {
            var procedures = context.Procedures.ToList();

            var result = new List<ProcedureRequirement>()
            {
                new ProcedureRequirement { Name = "Solicitud o Formato N° 01 dirigido al Vicepresidente Académico, exponiendo las razones que motivan la solicitud", Code = "0001", Cost = 0, ProcedureId = procedures[0].Id },
                new ProcedureRequirement { Name = "Copia fedateada de los documentos que sustentan el pedido.", Code = "0002", Cost = 0, ProcedureId = procedures[0].Id },
                new ProcedureRequirement { Name = "Pago por derecho de trámite.", Code = "0003", Cost = 65.53M, ProcedureId = procedures[0].Id },

                new ProcedureRequirement { Name = "No Aplica", Code = "0001", Cost = 50.00M, ProcedureId = procedures[1].Id },

                new ProcedureRequirement { Name = "Proforma de matrícula", Code = "0001", Cost = 0, ProcedureId = procedures[4].Id },
                new ProcedureRequirement { Name = "Constancia Tutorial", Code = "0002", Cost = 0, ProcedureId = procedures[4].Id },
                new ProcedureRequirement { Name = "Completar la Encuesta de desempeño docente", Code = "0003", Cost = 0, ProcedureId = procedures[4].Id },
                new ProcedureRequirement { Name = "Haber tramitado la emisión de Carné Universitario en la Dirección de Actividades y Servicios Académicos por medio del Procedimiento PR-020-DASA-01 (Opcional)", Code = "0004", Cost = 0, ProcedureId = procedures[4].Id },
                new ProcedureRequirement { Name = "Constancia de Seguro de Salud visada por la Dirección de Bienestar Universitario", Code = "0005", Cost = 0, ProcedureId = procedures[4].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para estudiantes regulares", Code = "0006", Cost = 50.90M, ProcedureId = procedures[4].Id },

                new ProcedureRequirement { Name = "Proforma de matrícula", Code = "0001", Cost = 0, ProcedureId = procedures[5].Id },
                new ProcedureRequirement { Name = "Constancia Tutorial", Code = "0002", Cost = 0, ProcedureId = procedures[5].Id },
                new ProcedureRequirement { Name = "Completar la Encuesta de desempeño docente", Code = "0003", Cost = 0, ProcedureId = procedures[5].Id },
                new ProcedureRequirement { Name = "Haber tramitado la emisión de Carné Universitario en la Dirección de Actividades y Servicios Académicos por medio del Procedimiento PR-020-DASA-01 (Opcional)", Code = "0004", Cost = 0, ProcedureId = procedures[5].Id },
                new ProcedureRequirement { Name = "Constancia de Seguro de Salud visada por la Dirección de Bienestar Universitario", Code = "0005", Cost = 0, ProcedureId = procedures[5].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para estudiantes regulares", Code = "0006", Cost = 50.90M, ProcedureId = procedures[5].Id },
                new ProcedureRequirement { Name = "Cancelación del recargo por matrícula extemporánea", Code = "0007", Cost = 19.41M, ProcedureId = procedures[5].Id },

                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para casos de matrícula especial por ciclo menor a 12 créditos, de ser el caso (costo considera todas las asignaturas).", Code = "0001", Cost = 18.01M, ProcedureId = procedures[6].Id },

                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para casos de Segunda, Tercera o Cuarta Matrícula sin cambio de currículo, de corresponder (Costo por cada asignatura desaprobada).", Code = "0001", Cost = 9.08M, ProcedureId = procedures[8].Id },

                new ProcedureRequirement { Name = "Solicitud o Formato N° 01 dirigido al Director de la Escuela Profesional.", Code = "0001", Cost = 0, ProcedureId = procedures[9].Id },
                new ProcedureRequirement { Name = "Tener como máximo dos (02) asignaturas desaprobadas con nota igual o superior a siete (07) y que estas le impidan culminar con el Plan de Estudios para la graduación.", Code = "0002", Cost = 0, ProcedureId = procedures[9].Id },
                new ProcedureRequirement { Name = "Pago por derecho de inscripción.", Code = "0003", Cost = 31.98M, ProcedureId = procedures[9].Id },
                new ProcedureRequirement { Name = "Cancelación del Costo por el Curso Dirigido aprobado según su propio Reglamento.", Code = "0004", Cost = 0, ProcedureId = procedures[9].Id },

                new ProcedureRequirement { Name = "Solicitud o Formato N° 01 dirigido al Director de la Escuela Profesional.", Code = "0001", Cost = 0, ProcedureId = procedures[11].Id },
                new ProcedureRequirement { Name = "Tener como máximo dos (02) asignaturas desaprobadas con nota igual o superior a siete (07) y que estas le impidan culminar con el Plan de Estudios para la graduación.", Code = "0002", Cost = 0, ProcedureId = procedures[11].Id },
                new ProcedureRequirement { Name = "Pago por derecho de trámite.", Code = "0003", Cost = 63.86M, ProcedureId = procedures[11].Id },

                new ProcedureRequirement { Name = "Recibo de Pago por derecho de matrícula.", Code = "0001", Cost = 23.00M, ProcedureId = procedures[12].Id },
                new ProcedureRequirement { Name = "2 Fotografías tamaño carné.", Code = "0002", Cost = 0, ProcedureId = procedures[12].Id },
                new ProcedureRequirement { Name = "Recibo de pago por derecho de Carné.", Code = "0003", Cost = 0, ProcedureId = procedures[12].Id },

                new ProcedureRequirement { Name = "Proforma de matrícula", Code = "0001", Cost = 0, ProcedureId = procedures[17].Id },
                new ProcedureRequirement { Name = "Constancia Tutorial", Code = "0002", Cost = 0, ProcedureId = procedures[17].Id },
                new ProcedureRequirement { Name = "Completar la Encuesta de desempeño docente", Code = "0003", Cost = 0, ProcedureId = procedures[17].Id },
                new ProcedureRequirement { Name = "Haber tramitado la emisión de Carné Universitario en la Dirección de Actividades y Servicios Académicos por medio del Procedimiento PR-020-DASA-01 (Opcional)", Code = "0004", Cost = 0, ProcedureId = procedures[17].Id },
                new ProcedureRequirement { Name = "Constancia de Seguro de Salud visada por la Dirección de Bienestar Universitario", Code = "0005", Cost = 0, ProcedureId = procedures[17].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para estudiantes no regulares", Code = "0006", Cost = 9.08M, ProcedureId = procedures[17].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para casos de matrícula especial por ciclo menor a 12 créditos, de ser el caso", Code = "0007", Cost = 18.01M, ProcedureId = procedures[18].Id },

                new ProcedureRequirement { Name = "Proforma de matrícula", Code = "0001", Cost = 0, ProcedureId = procedures[18].Id },
                new ProcedureRequirement { Name = "Constancia Tutorial", Code = "0002", Cost = 0, ProcedureId = procedures[18].Id },
                new ProcedureRequirement { Name = "Completar la Encuesta de desempeño docente", Code = "0003", Cost = 0, ProcedureId = procedures[18].Id },
                new ProcedureRequirement { Name = "Haber tramitado la emisión de Carné Universitario en la Dirección de Actividades y Servicios Académicos por medio del Procedimiento PR-020-DASA-01 (Opcional)", Code = "0004", Cost = 0, ProcedureId = procedures[18].Id },
                new ProcedureRequirement { Name = "Constancia de Seguro de Salud visada por la Dirección de Bienestar Universitario", Code = "0005", Cost = 0, ProcedureId = procedures[18].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para estudiantes no regulares", Code = "0006", Cost = 9.08M, ProcedureId = procedures[18].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para casos de Segunda, Tercera o Cuarta Matrícula sin cambio de currículo, de corresponder", Code = "0007", Cost = 9.08M, ProcedureId = procedures[18].Id },

                new ProcedureRequirement { Name = "Proforma de matrícula", Code = "0001", Cost = 0, ProcedureId = procedures[19].Id },
                new ProcedureRequirement { Name = "Constancia Tutorial", Code = "0002", Cost = 0, ProcedureId = procedures[19].Id },
                new ProcedureRequirement { Name = "Completar la Encuesta de desempeño docente", Code = "0003", Cost = 0, ProcedureId = procedures[19].Id },
                new ProcedureRequirement { Name = "Haber tramitado la emisión de Carné Universitario en la Dirección de Actividades y Servicios Académicos por medio del Procedimiento PR-020-DASA-01 (Opcional)", Code = "0004", Cost = 0, ProcedureId = procedures[19].Id },
                new ProcedureRequirement { Name = "Constancia de Seguro de Salud visada por la Dirección de Bienestar Universitario", Code = "0005", Cost = 0, ProcedureId = procedures[19].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para estudiantes no regulares", Code = "0006", Cost = 9.08M, ProcedureId = procedures[19].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para casos de Segunda, Tercera o Cuarta Matrícula con cambio de currículo, de corresponder ", Code = "0007", Cost = 11.41M, ProcedureId = procedures[19].Id },

                new ProcedureRequirement { Name = "Proforma de matrícula", Code = "0001", Cost = 0, ProcedureId = procedures[20].Id },
                new ProcedureRequirement { Name = "Constancia Tutorial", Code = "0002", Cost = 0, ProcedureId = procedures[20].Id },
                new ProcedureRequirement { Name = "Completar la Encuesta de desempeño docente", Code = "0003", Cost = 0, ProcedureId = procedures[20].Id },
                new ProcedureRequirement { Name = "Haber tramitado la emisión de Carné Universitario en la Dirección de Actividades y Servicios Académicos por medio del Procedimiento PR-020-DASA-01 (Opcional)", Code = "0004", Cost = 0, ProcedureId = procedures[20].Id },
                new ProcedureRequirement { Name = "Constancia de Seguro de Salud visada por la Dirección de Bienestar Universitario", Code = "0005", Cost = 0, ProcedureId = procedures[20].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para estudiantes no regulares", Code = "0006", Cost = 9.08M, ProcedureId = procedures[20].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para casos de matrícula especial por ciclo menor a 12 créditos, de ser el caso", Code = "0007", Cost = 18.01M, ProcedureId = procedures[20].Id },
                new ProcedureRequirement { Name = "Cancelación del recargo por matrícula extemporánea.", Code = "0008", Cost = 19.41M, ProcedureId = procedures[20].Id },

                new ProcedureRequirement { Name = "Proforma de matrícula", Code = "0001", Cost = 0, ProcedureId = procedures[21].Id },
                new ProcedureRequirement { Name = "Constancia Tutorial", Code = "0002", Cost = 0, ProcedureId = procedures[21].Id },
                new ProcedureRequirement { Name = "Completar la Encuesta de desempeño docente", Code = "0003", Cost = 0, ProcedureId = procedures[21].Id },
                new ProcedureRequirement { Name = "Haber tramitado la emisión de Carné Universitario en la Dirección de Actividades y Servicios Académicos por medio del Procedimiento PR-020-DASA-01 (Opcional)", Code = "0004", Cost = 0, ProcedureId = procedures[21].Id },
                new ProcedureRequirement { Name = "Constancia de Seguro de Salud visada por la Dirección de Bienestar Universitario", Code = "0005", Cost = 0, ProcedureId = procedures[21].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para estudiantes no regulares", Code = "0006", Cost = 9.08M, ProcedureId = procedures[21].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para casos de Segunda, Tercera o Cuarta Matrícula sin cambio de currículo, de corresponder", Code = "0007", Cost = 9.08M, ProcedureId = procedures[21].Id },
                new ProcedureRequirement { Name = "Cancelación del recargo por matrícula extemporánea.", Code = "0008", Cost = 19.41M, ProcedureId = procedures[21].Id },

                new ProcedureRequirement { Name = "Proforma de matrícula", Code = "0001", Cost = 0, ProcedureId = procedures[22].Id },
                new ProcedureRequirement { Name = "Constancia Tutorial", Code = "0002", Cost = 0, ProcedureId = procedures[22].Id },
                new ProcedureRequirement { Name = "Completar la Encuesta de desempeño docente", Code = "0003", Cost = 0, ProcedureId = procedures[22].Id },
                new ProcedureRequirement { Name = "Haber tramitado la emisión de Carné Universitario en la Dirección de Actividades y Servicios Académicos por medio del Procedimiento PR-020-DASA-01 (Opcional)", Code = "0004", Cost = 0, ProcedureId = procedures[22].Id },
                new ProcedureRequirement { Name = "Constancia de Seguro de Salud visada por la Dirección de Bienestar Universitario", Code = "0005", Cost = 0, ProcedureId = procedures[22].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para estudiantes no regulares", Code = "0006", Cost = 9.08M, ProcedureId = procedures[22].Id },
                new ProcedureRequirement { Name = "Pago por derecho de Matrícula para casos de Segunda, Tercera o Cuarta Matrícula con cambio de currículo, de corresponder ", Code = "0007", Cost = 11.41M, ProcedureId = procedures[22].Id },
                new ProcedureRequirement { Name = "Cancelación del recargo por matrícula extemporánea.", Code = "0008", Cost = 19.41M, ProcedureId = procedures[22].Id },

                //Centro de idiomas
                new ProcedureRequirement { Name = "Recibo de Pago por derecho de matrícula centro de idiomas.", Code = "0001", Cost = 150.00M, ProcedureId = procedures[14].Id },
                new ProcedureRequirement { Name = "2 Fotografías tamaño carné.", Code = "0002", Cost = 0, ProcedureId = procedures[14].Id },
                new ProcedureRequirement { Name = "Recibo de pago por derecho de Carné.", Code = "0003", Cost = 0, ProcedureId = procedures[14].Id },

                new ProcedureRequirement { Name = "Pagos por derecho de matrícula Computación e Informática.", Code = "0001", Cost = 0, ProcedureId = procedures[13].Id },
                new ProcedureRequirement { Name = "Estudiantes UNAMAD", Code = "0001-1", Cost = 67.00M, ProcedureId = procedures[13].Id },
                new ProcedureRequirement { Name = "Personas en general", Code = "0001-2", Cost = 86.00M, ProcedureId = procedures[13].Id },
                new ProcedureRequirement { Name = "Docentes y administrativos", Code = "0001-3", Cost = 33.00M, ProcedureId = procedures[13].Id }
            };

            return result.ToArray();
        }
    }
}
