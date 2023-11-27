using AKDEMIC.ENTITIES.Models.Intranet;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class PsychologicalDiagnosticSeed
    {
        public static PsychologicalDiagnostic[] Seed(AkdemicContext context)
        {
            var result = new List<PsychologicalDiagnostic>
            {
            
            new PsychologicalDiagnostic { Description = "Episodio depresivo leve" , Code = "F32.0" },
            new PsychologicalDiagnostic { Description = "Episodio depresivo moderado" , Code = "F32.1" },
            new PsychologicalDiagnostic { Description = "Episodio depresivo grave sin síntomas psicóticos" , Code = "F32.2" },
            new PsychologicalDiagnostic { Description = "Episodio depresivo grave con síntomas psicóticos" , Code = "F32.3" },
            new PsychologicalDiagnostic { Description = "Otros episodios depresivos" , Code = "F32.8" },
            new PsychologicalDiagnostic { Description = "Episodio depresivo sin especificación" , Code = "F32.9" },
            new PsychologicalDiagnostic { Description = "Trastorno depresivo recurrente, episodio actual leve" , Code = "F33.0" },
            new PsychologicalDiagnostic { Description = "Trastorno depresivo recurrente, episodio actual moderado" , Code = "F33.1" },
            new PsychologicalDiagnostic { Description = "Trastorno depresivo recurrente, episodio actual grave sin síntomas psicóticos" , Code = "F33.2" },
            new PsychologicalDiagnostic { Description = "Trastorno depresivo recurrente, episodio actual grave con síntomas psicóticos" , Code = "F33.3" },
            new PsychologicalDiagnostic { Description = "Trastorno depresivo recurrente actualmente en remisión" , Code = "F33.4" },
            new PsychologicalDiagnostic { Description = "Otros trastornos depresivos recurrentes" , Code = "F33.8" },
            new PsychologicalDiagnostic { Description = "Trastorno depresivo recurrente sin especificación" , Code = "F33.9" },
            new PsychologicalDiagnostic { Description = "Trastorno de pánico (ansiedad paroxística episódica)" , Code = "F41.0" },
            new PsychologicalDiagnostic { Description = "Trastorno de ansiedad generalizada" , Code = "F41.1" },
            new PsychologicalDiagnostic { Description = "Trastorno mixto ansioso-depresivo" , Code = "F41.2" },
            new PsychologicalDiagnostic { Description = "Otro trastorno mixto de ansiedad" , Code = "F41.3" },
            new PsychologicalDiagnostic { Description = "Otros trastornos de ansiedad especificados" , Code = "F41.8" },
            new PsychologicalDiagnostic { Description = "Trastorno de ansiedad sin especificación" , Code = "F41.9" },
            new PsychologicalDiagnostic { Description = "Anorexia nerviosa" , Code = "F50.0" },
            new PsychologicalDiagnostic { Description = "Anorexia nerviosa atípica" , Code = "F50.1" },
            new PsychologicalDiagnostic { Description = "Bulimia nerviosa" , Code = "F50.2" },
            new PsychologicalDiagnostic { Description = "Bulimia nerviosa atípica" , Code = "F50.3" },
            new PsychologicalDiagnostic { Description = "Hiperfagia en otras alteraciones psicológicas" , Code = "F50.4" },
            new PsychologicalDiagnostic { Description = "Vómitos en otras alteraciones psicológicas" , Code = "F50.5" },
            new PsychologicalDiagnostic { Description = "Otros trastornos de la conducta alimentaria" , Code = "F50.8" },
            new PsychologicalDiagnostic { Description = "Trastorno de la conducta alimentaria sin especificación" , Code = "F50.9" },
            new PsychologicalDiagnostic { Description = "Trastornos mentales y del comportamiento debidos al consumo de alcohol" , Code = "F10" },
            new PsychologicalDiagnostic { Description = "Trastornos mentales y del comportamiento debidos al consumo de opioides" , Code = "F11" },
            new PsychologicalDiagnostic { Description = "Trastornos mentales y del comportamiento debidos al consumo de cannabinoides" , Code = "F12" },
            new PsychologicalDiagnostic { Description = "Trastornos mentales y del comportamiento debidos al consumo de sedantes o hipnóticos" , Code = "F13" },
            new PsychologicalDiagnostic { Description = "Trastornos mentales y del comportamiento debidos al consumo de cocaína" , Code = "F14" },
            new PsychologicalDiagnostic { Description = "Trastornos mentales y del comportamiento debidos al consumo de otros estimulantes (incluyendo la cafeína)" , Code = "F15" },
            new PsychologicalDiagnostic { Description = "Trastornos mentales y del comportamiento debidos al consumo de alucinógenos" , Code = "F16" },
            new PsychologicalDiagnostic { Description = "Trastornos mentales y del comportamiento debidos al consumo de tabaco" , Code = "F17" },
            new PsychologicalDiagnostic { Description = "Trastornos mentales y del comportamiento debidos al consumo de disolventes volátiles" , Code = "F18" },
            new PsychologicalDiagnostic { Description = "Trastornos mentales y del comportamiento debidos al consumo de múltiples drogas o de otras sustancias psicotropas" , Code = "F19" },
            new PsychologicalDiagnostic { Description = "Esquizofrenia paranoide" , Code = "F20.0" },
            new PsychologicalDiagnostic { Description = "Esquizofrenia hebefrénica" , Code = "F20.1" },
            new PsychologicalDiagnostic { Description = "Esquizofrenia catatónica" , Code = "F20.2" },
            new PsychologicalDiagnostic { Description = "Esquizofrenia indiferenciada" , Code = "F20.3" },
            new PsychologicalDiagnostic { Description = "Depresión post-esquizofrénica" , Code = "F20.4" },
            new PsychologicalDiagnostic { Description = "Esquizofrenia residual" , Code = "F20.5" },
            new PsychologicalDiagnostic { Description = "Esquizofrenia simple" , Code = "F20.6" },
            new PsychologicalDiagnostic { Description = "Otras esquizofrenias" , Code = "F20.8" },
            new PsychologicalDiagnostic { Description = "Esquizofrenia sin especificación" , Code = "F20.9" },
            new PsychologicalDiagnostic { Description = "Trastorno de ideas delirantes" , Code = "F22.0" },
            new PsychologicalDiagnostic { Description = "Otros trastornos de ideas delirantes persistentes" , Code = "F22.8" },
            new PsychologicalDiagnostic { Description = "Trastorno de ideas delirantes persistentes sin especificación" , Code = "F22.9" },
            new PsychologicalDiagnostic { Description = "Trastorno psicótico agudo polimorfo sin síntomas de esquizofrenia" , Code = "F23.0" },
            new PsychologicalDiagnostic { Description = "Trastorno psicótico agudo polimorfo con síntomas de esquizofrenia" , Code = "F23.1" },
            new PsychologicalDiagnostic { Description = "Trastorno psicótico agudo de tipo esquizofrénico" , Code = "F23.2" },
            new PsychologicalDiagnostic { Description = "Otro trastorno psicótico agudo con predominio de ideas delirantes" , Code = "F23.3" },
            new PsychologicalDiagnostic { Description = "Otros trastornos psicóticos agudos transitorios" , Code = "F23.8" },
            new PsychologicalDiagnostic { Description = "Trastorno psicótico agudo transitorio sin especificación" , Code = "F23.9" },
            new PsychologicalDiagnostic { Description = "Trastorno esquizoafectivo de tipo maníaco" , Code = "F25.0" },
            new PsychologicalDiagnostic { Description = "Trastorno esquizoafectivo de tipo depresivo" , Code = "F25.1" },
            new PsychologicalDiagnostic { Description = "Trastorno esquizoafectivo de tipo mixto" , Code = "F25.2" },
            new PsychologicalDiagnostic { Description = "Otros trastornos esquizoafectivos" , Code = "F25.8" },
            new PsychologicalDiagnostic { Description = "Trastorno esquizoafectivo sin especificación" , Code = "F25.9" },
            new PsychologicalDiagnostic { Description = "Con predominio de pensamientos o rumiaciones obsesivas" , Code = "F42.0" },
            new PsychologicalDiagnostic { Description = "Con predominio de actos compulsivos (rituales obsesivos)" , Code = "F42.1" },
            new PsychologicalDiagnostic { Description = "Con mezcla de pensamientos y actos obsesivos" , Code = "F42.2" },
            new PsychologicalDiagnostic { Description = "Otros trastornos obsesivo-compulsivos" , Code = "F42.8" },
            new PsychologicalDiagnostic { Description = "Trastorno obsesivo-compulsivo sin especificación" , Code = "F42.9" },
            new PsychologicalDiagnostic { Description = "Reacción a estrés agudo" , Code = "F43.0" },
            new PsychologicalDiagnostic { Description = "Trastorno de estrés post-traumático" , Code = "F43.1" },
            new PsychologicalDiagnostic { Description = "Trastornos de adaptación" , Code = "F43.2" },
            new PsychologicalDiagnostic { Description = "Otras reacciones a estrés grave" , Code = "F43.8" },
            new PsychologicalDiagnostic { Description = "Reacción a estrés grave sin especificación" , Code = "F43.9" },
            new PsychologicalDiagnostic { Description = "Insomnio no orgánico" , Code = "F51.0" },
            new PsychologicalDiagnostic { Description = "Hipersomnio no orgánico" , Code = "F51.1" },
            new PsychologicalDiagnostic { Description = "Trastorno no orgánico del ciclo sueño-vigilia" , Code = "F51.2" },
            new PsychologicalDiagnostic { Description = "Sonambulismo" , Code = "F51.3" },
            new PsychologicalDiagnostic { Description = "Terrores nocturnos" , Code = "F51.4" },
            new PsychologicalDiagnostic { Description = "Pesadillas" , Code = "F51.5" },
            new PsychologicalDiagnostic { Description = "Otros trastornos no orgánicos del sueño" , Code = "F51.8" },
            new PsychologicalDiagnostic { Description = "Trastorno no orgánico del sueño de origen sin especificación" , Code = "F51.9" }

            };

            return result.ToArray();
        }

    }
}
