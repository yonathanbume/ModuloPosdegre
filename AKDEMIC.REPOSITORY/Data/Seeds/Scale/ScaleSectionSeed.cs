using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Scale
{
    public class ScaleSectionSeed
    {
        public static ScaleSection[] Seed(AkdemicContext context)
        {
            var result = new List<ScaleSection>()
            {
                new ScaleSection { SectionNumber = 1, Name = "Contratos, Nombramientos, Ascensos y Ceses" },
                new ScaleSection { SectionNumber = 2, Name = "Evaluación del Desempeño Laboral" },
                new ScaleSection { SectionNumber = 3, Name = "Desplazamiento de Personal" },
                new ScaleSection { SectionNumber = 4, Name = "Licencias y Vacaciones" },
                new ScaleSection { SectionNumber = 5, Name = "Méritos" },
                new ScaleSection { SectionNumber = 6, Name = "Trayectoria Laboral en otras Instituciones" },
                new ScaleSection { SectionNumber = 7, Name = "Beneficios Laborales, Sociales y Otros" },
                new ScaleSection { SectionNumber = 8, Name = "Otra Documentación de índole Institucional" },
                new ScaleSection { SectionNumber = 9, Name = "Investigaciones y Publicaciones" },
                new ScaleSection { SectionNumber = 10, Name = "Deméritos" }
            };

            return result.ToArray();
        }
    }
}
