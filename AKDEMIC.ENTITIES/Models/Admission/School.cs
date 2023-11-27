using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class School
    {
        public Guid Id { get; set; }

        public string ModularCode { get; set; }

        public string LocalCode { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public Guid ProvinceId { get; set; }
        public Province Province { get; set; }

        public Guid DistrictId { get; set; }
        public District District { get; set; }

        public string UbigeoCode { get; set; }

        public byte Type { get; set; } = ConstantHelpers.Admission.School.Type.DIRECT_MANAGEMENT_PUBLIC_SCHOOL;

        public bool IsSchooled { get; set; } // Escolarizado - No escolarizado
    }
}
