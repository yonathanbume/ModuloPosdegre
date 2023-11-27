using AKDEMIC.ENTITIES.Models.Laurassia;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class FrequentQuestionSeed
    {
        public static FrequentQuestion[] Seed(AkdemicContext context)
        {
            var result = new List<FrequentQuestion>()
            {
                new FrequentQuestion { Description = "Ingresa presionando el boton de Office 365 e ingresa tus datos", State = true, Title = "¿Cómo puedo ingresar a mi cuenta de Office 365?" },
                new FrequentQuestion { Description = "Busca en la página oficial de Oracle y descárgalo", State = true, Title = "¿Cómo puedo obtener la ultima actualizacion de Java para mis tareas?" },
                new FrequentQuestion { Description = "Busca en nuestra página de Bolsa de trabajo", State = true, Title = "¿Dónde puedo buscar para realizar mis prácticas profesionales?" },
                new FrequentQuestion { Description = "Un descuento en la boleta mensual", State = true, Title = "¿Cuáles son los beneficios de tener beca?" }
            };
            return result.ToArray();
        }
    }
}