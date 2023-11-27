using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Scale
{
    public class WorkerManagementPositionSeed
    {
        public static WorkerManagementPosition[] Seed(AkdemicContext context)
        {
            var dependencies = context.Dependencies.ToList();

            var result = new List<WorkerManagementPosition>()
            {
                new WorkerManagementPosition { Code = "ASISADMIN", Name = "Asistente Administrativo", Status = ConstantHelpers.STATES.ACTIVE, DependencyId = dependencies[1].Id },
                new WorkerManagementPosition { Code = "TECADMIN", Name = "Técnico Administrativo", Status = ConstantHelpers.STATES.ACTIVE, DependencyId = dependencies[1].Id },
                new WorkerManagementPosition { Code = "SECRE", Name = "Secretaria", Status = ConstantHelpers.STATES.ACTIVE, DependencyId = dependencies[1].Id },
                new WorkerManagementPosition { Code = "DIREADMIN", Name = "Director de Administración", Status = ConstantHelpers.STATES.ACTIVE, DependencyId = dependencies[1].Id },
                new WorkerManagementPosition { Code = "AUXMESA", Name = "Auxiliar de Mesa de Partes", Status = ConstantHelpers.STATES.ACTIVE, DependencyId = dependencies[0].Id },
                new WorkerManagementPosition { Code = "DIRMESA", Name = "Director de Mesa de Partes", Status = ConstantHelpers.STATES.ACTIVE, DependencyId = dependencies[0].Id },
                new WorkerManagementPosition { Code = "ASISTESO", Name = "Asistente de Tesorería", Status = ConstantHelpers.STATES.ACTIVE, DependencyId = dependencies[2].Id },
                new WorkerManagementPosition { Code = "DIRTESO", Name = "Director de Tesorería", Status = ConstantHelpers.STATES.ACTIVE, DependencyId = dependencies[2].Id },
                new WorkerManagementPosition { Code = "BIB", Name = "Bibliotecólogo", Status = ConstantHelpers.STATES.ACTIVE, DependencyId = dependencies[3].Id },
                new WorkerManagementPosition { Code = "ASIBIB", Name = "Asistente de Biblioteca", Status = ConstantHelpers.STATES.ACTIVE, DependencyId = dependencies[3].Id }
            };

            return result.ToArray();
        }
    }
}
