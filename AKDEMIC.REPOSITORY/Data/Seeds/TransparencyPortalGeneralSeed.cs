using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class TransparencyPortalGeneralSeed
    {
        public static TransparencyPortalGeneral[] Seed(AkdemicContext context)
        {
            var result = new List<TransparencyPortalGeneral>()
            {
                new TransparencyPortalGeneral { Type = 1, Title = "Misión", Content = "La Universidad, es una Institución formadora de profesionales de alto nivel académico, competitivos con contenido humanístico y social, solidarios. Comprometidos con el desarrollo regional y nacional, mediante la investigación científica y tecnológica, dentro de los proncipios de integridad, transparencia, pertinencia y la preservacion del medio ambiente.", Url = "https://akdemic.blob.core.windows.net/missionvision/0ff1f2e5-16fe-436d-bbc0-1a5166aa2539.jpg" },
                new TransparencyPortalGeneral { Type = 2, Title = "Visión", Content = "Ser una Universidad acreditada con liderazgo regional, nacional e internacional, por su excelencia académica e investigación científica y tecnológica, orienda a resolver prioritariamente los problemas regionales y nacionales, creando una cultura de calidad que logre una solida integración con sus graduados y sustentada en el pluralismo y responsabilidad social, promoviendo el manejo sostenible de los recursos naturales y conservación del medio ambiente.", Url = "https://akdemic.blob.core.windows.net/missionvision/7ad18495-cb07-4dfd-89ea-aa46bc1d81db.jpg" },
                new TransparencyPortalGeneral { Type = 3, Title = "Calendario", Content = "Calendario academico" },
                new TransparencyPortalGeneral { Type = 4, Title = "Reglamento", Content = "Es un hecho establecido hace demasiado tiempo que un lector se distraerá con el contenido del texto de un sitio mientras que mira su diseño. El punto de usar Lorem Ipsum es que tiene una distribución más o menos normal de las letras, al contrario de usar textos como por ejemplo 'Contenido aquí, contenido aquí'. Estos textos hacen parecerlo un español que se puede leer. Muchos paquetes de autoedición y editores de páginas web usan el Lorem Ipsum como su texto por defecto, y al hacer una búsqueda de 'Lorem Ipsum' va a dar por resultado muchos sitios web que usan este texto si se encuentran en estado de desarrollo. Muchas versiones han evolucionado a través de los años, algunas veces por accidente, otras veces a propósito (por ejemplo insertándole humor y cosas por el estilo).", Url = "https://akdemic.blob.core.windows.net/admission/593ec439-f475-4ed7-b091-2325e32569c8.png" },
                new TransparencyPortalGeneral { Type = 5, Title = "Temario", Content = "" }
            };

            return result.ToArray();
        }
    }
}
