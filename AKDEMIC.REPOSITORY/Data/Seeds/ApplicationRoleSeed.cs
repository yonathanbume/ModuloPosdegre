using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ApplicationRoleSeed
    {
        public static ApplicationRole[] Seed(AkdemicContext context)
        {
            List<ApplicationRole> result = new List<ApplicationRole>()
            {
                // Administradores
                new ApplicationRole { Name = ConstantHelpers.ROLES.ACADEMIC_EXCHANGE_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.ADMISSION_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.CAFETERIA_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.COMPUTER_CENTER_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.COMPUTER_MANAGEMENT_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.DEGREE_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.DOCUMENTARY_PROCEDURE_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.ECONOMIC_MANAGEMENT_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.ENROLLMENT_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.ESCALAFON_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.EVALUATION_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.GEO_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.HELP_DESK_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.INDICATORS_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.INSTITUTIONAL_AGENDA_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.INTEREST_GROUP_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.INTRANET_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.INVESTIGATION_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.JOB_EXCHANGE_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.KARDEX_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.LANGUAGE_CENTER_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.LAURASSIA_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.PAYROLL_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.QUIBUK_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.PREUNIVERSITARY_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.PROCESSES_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.RESERVATIONS_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.RESOLUTIVE_ACTS_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.ROOM_RESERVATION_SYSTEM_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.SERVER_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.SERVICE_MANAGEMENT_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.SISCO_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.SURVEY_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.TEACHING_MANAGEMENT_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.TANSPARENCY_PORTAL_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.TUTORING_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.TUTORING_COORDINADOR_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.VIRTUAL_DIRECTORY_ADMIN, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.PROCUREMENTS_UNIT, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.SELECTION_PROCESS_UNIT, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.RESEARCH_COORDINATOR, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.REPORT_QUERIES, Priority = 1},
                new ApplicationRole { Name = ConstantHelpers.ROLES.TREASURY_CHIEF, Priority = 1},

                // Intranet
                new ApplicationRole { Name = "Registro Académico", Priority = 1},
                new ApplicationRole { Name = "Personal de Registro Académico", Priority = 1},
                new ApplicationRole { Name = "Dirección General Académica", Priority = 1},
                new ApplicationRole { Name = "Ventanilla Académica",Priority=1},

                // Help Desk
                new ApplicationRole { Name = "Admisión", Priority = 1 },
                new ApplicationRole { Name = "Alumnos", Priority = 1 },
                new ApplicationRole { Name = "Alumnos de cómputo", Priority = 1 },
                new ApplicationRole { Name = "Alumnos de idioma", Priority = 1 },
                // Pre-Universitario
                new ApplicationRole { Name = "Alumnos Pre Universitario", Priority = 1 },
                new ApplicationRole { Name = "Asistente Académico", Priority = 1 },
                new ApplicationRole { Name = "Autoridades", Priority = 1 },
                new ApplicationRole { Name = "Bibliotecario", Priority = 1 },
                new ApplicationRole { Name = "Bienestar", Priority = 1 },

                new ApplicationRole { Name = "Comedor", Priority = 1 },
                // Investigación
                new ApplicationRole { Name = "Comité Evaluador", Priority = 1 },
                new ApplicationRole { Name = "Consultas de Datos", Priority = 1 },
                new ApplicationRole { Name = "Consultas Matricula", Priority = 1 },
                new ApplicationRole { Name = "Consultas UTD", Priority = 1 },
                new ApplicationRole { Name = "Coordinador del curso", Priority = 1 },
                // Investigación
                new ApplicationRole { Name = "Coordinador de Proyecto", Priority = 1 },
                // Bolsa
                new ApplicationRole { Name = "Coordinador de Seguimiento", Priority = 1},
                new ApplicationRole { Name = "Coordinador de Tutorias", Priority = 1 },
                new ApplicationRole { Name = "Decano", Priority = 1 },
                new ApplicationRole { Name = "Departamento Académico", Priority = 1 },
                new ApplicationRole { Name = "Dependencia", Priority = 1 },
                new ApplicationRole { Name = "Deporte y Cultura", Priority = 1 },
                new ApplicationRole { Name = "Director de Carrera", Priority = 1 },
                new ApplicationRole { Name = "Director del Departamento Académico", Priority = 1 },
                new ApplicationRole { Name = "Director de Escuela", Priority = 1 },
                new ApplicationRole { Name = "Dirección", Priority = 1 },
                new ApplicationRole { Name = "Docentes", Priority = 1 },
                // Pre-Universitario
                new ApplicationRole { Name = "Docentes Pre Universitario", Priority = 1 },
                new ApplicationRole { Name = "Doctores", Priority = 1 },
                new ApplicationRole { Name = "Empresa", Priority = 1 },
                new ApplicationRole { Name = "Grados Académicos y Títulos Profesionales", Priority = 1 },
                new ApplicationRole { Name = "Jefe de Registros", Priority = 1 },
                new ApplicationRole { Name = "Mesa de Partes", Priority = 1 },
                new ApplicationRole { Name = "Nutrición", Priority = 1 },
                new ApplicationRole { Name = "Obstetricia", Priority = 1 },
                new ApplicationRole { Name = "Oficina", Priority = 1 },
                new ApplicationRole { Name = "Oficina de Adquisiciones", Priority = 1 },
                new ApplicationRole { Name = "Oficina de Presupuesto", Priority = 1 },
                new ApplicationRole { Name = "Oficina de Procesos de Selección", Priority = 1 },
                //Grupo de Interes
                new ApplicationRole {Name = "Participante de Programa", Priority = 1},
                new ApplicationRole {Name = "Programa de Estudio", Priority = 1},

                new ApplicationRole { Name = "Psicología", Priority = 1 },
                new ApplicationRole { Name = "Registrador", Priority = 1 },
                new ApplicationRole { Name = "Secretario Académico", Priority = 1 },
                new ApplicationRole { Name = "Coordinador Académico", Priority = 1 },
                new ApplicationRole { Name = "Secretaría de Servicios Generales", Priority = 1 },
                new ApplicationRole { Name = "Secretaría de las Direcciones de las Escuelas Profesionales", Priority = 1 },
                new ApplicationRole { Name = "Secretaría de las Vicepresidencias, Presidencias y Administración", Priority = 1 },
                new ApplicationRole { Name = "Soporte de Usuarios", Priority = 1 },
                new ApplicationRole { Name = "Soporte Docentes", Priority = 1 },
                new ApplicationRole { Name = "Superadmin", Priority = 1 },
                new ApplicationRole { Name = "Supervisor", Priority = 1 },
                new ApplicationRole { Name = "Supervisor VRAC", Priority = 1 },
                // Help Desk
                new ApplicationRole { Name = "Técnico de Soporte Técnico", Priority = 1 },
                new ApplicationRole { Name = "Tópico", Priority = 1 },
                new ApplicationRole { Name = "Unidad de Programación", Priority = 1 },
                new ApplicationRole { Name = "Usuario de Oficina de Soporte", Priority = 1 },
                new ApplicationRole { Name = "Usuario OPAC", Priority = 1 },
                // Evaluación
                new ApplicationRole { Name = "Validador / Evaluador", Priority = 1 },
                new ApplicationRole { Name = "Encargado de Proyección Social", Priority = 1 },
                new ApplicationRole { Name = "Encargado de Evaluación de Extensión Universitaria", Priority = 1 },
                new ApplicationRole { Name = "Cooperante de Equipos", Priority = 1 },

                new ApplicationRole { Name = "Proveedor", Priority = 1 },

                // Finanzas                
                new ApplicationRole { Name = "Tesoreria", Priority = 1 },
                new ApplicationRole { Name = "Cajero", Priority = 1 },
                new ApplicationRole { Name = "Centro de Costo", Priority = 1 },
                new ApplicationRole { Name = "Ingresos Gastos", Priority = 1 },
                new ApplicationRole { Name = "Recaudación", Priority = 1 },
                new ApplicationRole { Name = "Ventanilla", Priority = 1 },
                new ApplicationRole { Name = "Recepción de Ordenes", Priority = 1 },
                new ApplicationRole { Name = "Oficina de Abastecimiento", Priority = 1 },
                new ApplicationRole { Name = "Almacén", Priority = 1 },
                new ApplicationRole { Name = "Logística", Priority = 1 },
                new ApplicationRole { Name = ConstantHelpers.ROLES.BANK_COLLECTION, Priority = 1 },

                //cafeteria
                 new ApplicationRole { Name = "Proveedor Comedor", Priority = 1 },
                 new ApplicationRole { Name = "Almacén Comedor", Priority = 1 },

                //chat
                  new ApplicationRole { Name = "Administrador del chat", Priority = 1 }
    };

            return result.ToArray();
        }
    }
}