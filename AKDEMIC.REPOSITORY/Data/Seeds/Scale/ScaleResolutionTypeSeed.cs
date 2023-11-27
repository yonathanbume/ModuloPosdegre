using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Scale
{
    public class ScaleResolutionTypeSeed
    {
        public static ScaleResolutionType[] Seed(AkdemicContext context)
        {
            var result = new List<ScaleResolutionType>()
            {
                //SECTION 1
                new ScaleResolutionType { Name = "Contratos", Description = "Contratos", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Nombramientos", Description = "Nombramientos", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Reingresos", Description = "Reingresos", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Reincoroporaciones", Description = "Reincoroporaciones", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Encargaturas", Description = "Encargaturas", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Designación de cargos", Description = "Designación de cargos", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Término de cargos", Description = "Término de cargos", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Cambio de categoría", Description = "Cambio de categoría", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Dedicación", Description = "Dedicación", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Grupo ocupacional", Description = "Grupo ocupacional", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Nivel", Description = "Nivel", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Promociones", Description = "Promociones", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Ascensos", Description = "Ascensos", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Rescisión de contrato", Description = "Rescisión de contrato", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Cese temporal", Description = "Cese temporal", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Cese definitivo", Description = "Cese definitivo", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Renuncia", Description = "Renuncia", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Destitución", Description = "Destitución", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Cese por fallecimiento", Description = "Cese por fallecimiento", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 2
                new ScaleResolutionType { Name = "Evaluación docente", Description = "Evaluación docente", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Evaluación administrativa", Description = "Evaluación administrativa", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Evaluación de trabajadores permanentes", Description = "Evaluación de trabajadores permanentes", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Evaluación de trabajadores contratados", Description = "Evaluación de trabajadores contratados", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Evaluación de funciones", Description = "Evaluación de funciones", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 3
                new ScaleResolutionType { Name = "Designación", Description = "Designación", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Reasignación", Description = "Reasignación", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Rotación", Description = "Rotación", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Destaque", Description = "Destaque", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Permuta", Description = "Permuta", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Desplazamiento de Encargaturas", Description = "Desplazamiento de Encargaturas", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Comisiones de servicios", Description = "Comisiones de servicios", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Transferencias", Description = "Transferencias", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 5
                new ScaleResolutionType { Name = "Felicitaciones y/o condecoraciones", Description = "Felicitaciones y/o condecoraciones", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Reconocimiento de buen comportamiento", Description = "Reconocimiento de buen comportamiento", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Reconocimiento de buen rendimiento laboral", Description = "Reconocimiento de buen rendimiento laboral", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Reconocimiento de ponencia internacional", Description = "Reconocimiento de ponencia internacional", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 10
                new ScaleResolutionType { Name = "Memorando de amonestaciones", Description = "Memorando de amonestaciones", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Instauraciones de proceso administrativo", Description = "Instauraciones de proceso administrativo", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Amonestaciones", Description = "Amonestaciones", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Suspensión laboral", Description = "Suspensión laboral", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Sanciones laborales", Description = "Sanciones laborales", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Declaratorias", Description = "Declaratorias", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Recursos impugnativos", Description = "Recursos impugnativos", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                
                //SECTION 6
                new ScaleResolutionType { Name = "Certificados y constancias de trabajo de otras instituciones", Description = "Certificados y constancias de trabajo de otras instituciones", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Informes y asignaciones de funciones", Description = "Informes y asignaciones de funciones", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Cargos de gobierno", Description = "Cargos de gobierno", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Documentos varios de agradecimiento a los servicios prestados", Description = "Documentos varios de agradecimiento a los servicios prestados", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 7
                new ScaleResolutionType { Name = "Beneficios", Description = "Beneficios", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Reconocimiento por tiempo de servicios", Description = "Reconocimiento por tiempo de servicios", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Acumulación de años de formación profesional", Description = "Acumulación de años de formación profesional", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Quinquenios", Description = "Quinquenios", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Goce de año sabático", Description = "Goce de año sabático", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Subsidio por luto", Description = "Subsidio por luto", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Sepelio", Description = "Sepelio", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Bonificación Familiar", Description = "Bonificación Familiar", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Pensiones", Description = "Pensiones", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Exoneración de pagos por estudios", Description = "Exoneración de pagos por estudios", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Pagos indebidos", Description = "Pagos indebidos", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Créditos devengados", Description = "Créditos devengados", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Retenciones judiciales", Description = "Retenciones judiciales", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 8
                new ScaleResolutionType { Name = "Declaraciones juradas de ingresos de bienes y rentas", Description = "Declaraciones juradas de ingresos de bienes y rentas", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Declaraciones juradas de incompatibilidad", Description = "Declaraciones juradas de incompatibilidad", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Certificados de ejercicios gravables anuales", Description = "Certificados de ejercicios gravables anuales", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Constancia de trabajo", Description = "Constancia de trabajo", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Entrega y recepción de cargo", Description = "Entrega y recepción de cargo", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 9
                new ScaleResolutionType { Name = "Publicación de Bibliografías", Description = "Publicación de Bibliografías", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Publicación de Revistas", Description = "Publicación de Revistas", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Publicación de Artículos de Investigación", Description = "Publicación de Artículos de Investigación", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Trabajos de Investigación", Description = "Trabajos de Investigación", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleResolutionType { Name = "Aportes Culturales y Deportivos", Description = "Aportes Culturales y Deportivos", IsSystemDefault = true, Status = ConstantHelpers.STATES.ACTIVE }
            };

            return result.ToArray();
        }
    }
}
