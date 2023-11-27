using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Scale
{
    public class ScaleSectionResolutionTypeSeed
    {
        public static ScaleSectionResolutionType[] Seed(AkdemicContext context)
        {
            var scaleResolutionTypes = context.ScaleResolutionTypes.ToList();
            var scaleSections = context.ScaleSections.ToList();

            var result = new List<ScaleSectionResolutionType>()
            {
                //SECTION 1
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[0].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[1].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[2].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[3].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[4].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[5].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[6].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[7].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[8].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[9].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[10].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[11].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[12].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[13].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[14].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[15].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[16].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[17].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[18].Id, ScaleSectionId = scaleSections[0].Id, Status = ConstantHelpers.STATES.ACTIVE },
                
                //SECTION 2
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[19].Id, ScaleSectionId = scaleSections[1].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[20].Id, ScaleSectionId = scaleSections[1].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[21].Id, ScaleSectionId = scaleSections[1].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[22].Id, ScaleSectionId = scaleSections[1].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[23].Id, ScaleSectionId = scaleSections[1].Id, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 3
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[24].Id, ScaleSectionId = scaleSections[2].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[25].Id, ScaleSectionId = scaleSections[2].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[26].Id, ScaleSectionId = scaleSections[2].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[27].Id, ScaleSectionId = scaleSections[2].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[28].Id, ScaleSectionId = scaleSections[2].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[29].Id, ScaleSectionId = scaleSections[2].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[30].Id, ScaleSectionId = scaleSections[2].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[31].Id, ScaleSectionId = scaleSections[2].Id, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 5
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[32].Id, ScaleSectionId = scaleSections[4].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[33].Id, ScaleSectionId = scaleSections[4].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[34].Id, ScaleSectionId = scaleSections[4].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[35].Id, ScaleSectionId = scaleSections[4].Id, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 10
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[36].Id, ScaleSectionId = scaleSections[9].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[37].Id, ScaleSectionId = scaleSections[9].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[38].Id, ScaleSectionId = scaleSections[9].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[39].Id, ScaleSectionId = scaleSections[9].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[40].Id, ScaleSectionId = scaleSections[9].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[41].Id, ScaleSectionId = scaleSections[9].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[42].Id, ScaleSectionId = scaleSections[9].Id, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 6
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[43].Id, ScaleSectionId = scaleSections[5].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[44].Id, ScaleSectionId = scaleSections[5].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[45].Id, ScaleSectionId = scaleSections[5].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[46].Id, ScaleSectionId = scaleSections[5].Id, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 7
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[47].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[48].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[49].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[50].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[51].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[52].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[53].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[54].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[55].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[56].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[57].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[58].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[59].Id, ScaleSectionId = scaleSections[6].Id, Status = ConstantHelpers.STATES.ACTIVE },
                
                //SECTION 8
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[60].Id, ScaleSectionId = scaleSections[7].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[61].Id, ScaleSectionId = scaleSections[7].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[62].Id, ScaleSectionId = scaleSections[7].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[63].Id, ScaleSectionId = scaleSections[7].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[64].Id, ScaleSectionId = scaleSections[7].Id, Status = ConstantHelpers.STATES.ACTIVE },

                //SECTION 9
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[65].Id, ScaleSectionId = scaleSections[8].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[66].Id, ScaleSectionId = scaleSections[8].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[67].Id, ScaleSectionId = scaleSections[8].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[68].Id, ScaleSectionId = scaleSections[8].Id, Status = ConstantHelpers.STATES.ACTIVE },
                new ScaleSectionResolutionType { ScaleResolutionTypeId = scaleResolutionTypes[69].Id, ScaleSectionId = scaleSections[8].Id, Status = ConstantHelpers.STATES.ACTIVE },
            };

            return result.ToArray();
        }
    }
}
