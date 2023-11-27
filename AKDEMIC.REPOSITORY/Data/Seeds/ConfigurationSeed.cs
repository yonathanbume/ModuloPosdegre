using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ConfigurationSeed
    {
        public static Configuration[] Seed(AkdemicContext context)
        {
            var result = new List<Configuration>()
            {
                new Configuration {
                    Key = "Nro. máximo de cursos desaprobados para ser repitente",
                    Value = "2",
                },
                new Configuration {
                    Key = "Fecha inicio de espera para reserva de matrícula",
                    Value = $"01/01/{DateTime.Now.Year}",
                },
                new Configuration {
                    Key = "Fecha fin de espera para reserva de matrícula",
                    Value = $"01/02/2018{DateTime.Now.Year}",
                },
                new Configuration {
                    Key = "Máximo de créditos para alumno regular",
                    Value = "24",
                },
                new Configuration {
                    Key = "Máximo de créditos para alumno de primeros puestos",
                    Value = "25",
                },
                new Configuration {
                    Key = "Máximo de créditos para repitente",
                    Value = "10",
                },
                new Configuration {
                    Key = "Mínimo de créditos por plan de estudios",
                    Value = "120",
                },
                new Configuration {
                    Key = "Cantidad mínima de asignaturas",
                    Value = "1",
                },
                new Configuration {
                    Key = "Tipo de Matrícula de Admisión",
                    Value = "1",
                },
            };
            return result.ToArray();
        }
    }
}
