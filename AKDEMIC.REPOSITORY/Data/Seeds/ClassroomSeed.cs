using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ClassroomSeed
    {
        public static Classroom[] Seed(AkdemicContext context)
        {
            var types = context.ClassroomTypes.ToList();
            var buildings = context.Buildings.ToList();

            var result = new List<Classroom>()
            {
                new Classroom { Description = "Sin Asignar", Code = "Sin Asignar", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[1].Id },
                new Classroom { Description = "B10", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[1].Id },
                new Classroom { Description = "B23", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[1].Id },
                new Classroom { Description = "B22", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[1].Id },
                new Classroom { Description = "B25", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[1].Id },
                new Classroom { Description = "B26", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[1].Id },
                new Classroom { Description = "B32", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[1].Id },
                new Classroom { Description = "B33", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[1].Id },
                new Classroom { Description = "C23", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[2].Id },
                new Classroom { Description = "C28", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[2].Id },
                new Classroom { Description = "C29", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[2].Id },
                new Classroom { Description = "C35", Capacity = 30, Status = 1, ClassroomTypeId = types[1].Id, BuildingId = buildings[2].Id },
                new Classroom { Description = "C41", Capacity = 30, Status = 1, ClassroomTypeId = types[1].Id, BuildingId = buildings[2].Id },
                new Classroom { Description = "C42", Capacity = 30, Status = 1, ClassroomTypeId = types[1].Id, BuildingId = buildings[2].Id },
                new Classroom { Description = "C45", Capacity = 30, Status = 1, ClassroomTypeId = types[1].Id, BuildingId = buildings[2].Id },
                new Classroom { Description = "C48", Capacity = 30, Status = 1, ClassroomTypeId = types[1].Id, BuildingId = buildings[2].Id },
                new Classroom { Description = "C56", Capacity = 30, Status = 1, ClassroomTypeId = types[1].Id, BuildingId = buildings[2].Id },
                new Classroom { Description = "D11", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[3].Id },
                new Classroom { Description = "E33", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[4].Id },
                new Classroom { Description = "E35", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[4].Id },
                new Classroom { Description = "E44", Capacity = 30, Status = 1, ClassroomTypeId = types[1].Id, BuildingId = buildings[4].Id },
                new Classroom { Description = "E54", Capacity = 30, Status = 1, ClassroomTypeId = types[1].Id, BuildingId = buildings[4].Id },
                new Classroom { Description = "F23", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[5].Id },
                new Classroom { Description = "H21", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[6].Id },
                new Classroom { Description = "H22", Capacity = 30, Status = 1, ClassroomTypeId = types[0].Id, BuildingId = buildings[6].Id },
                new Classroom { Description = "H45", Capacity = 30, Status = 1, ClassroomTypeId = types[1].Id, BuildingId = buildings[6].Id }
            };

            return result.ToArray();
        }
    }
}
