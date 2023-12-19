using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class PosdegreeTeacher
    {
       public Guid id { get; set; }
       public string name { get; set; }
       public string PaternalSurName { get; set;}
       public string Maternalsurname { get; set; }
       public string Email { get;set; }
       public string PhoneNumber { get; set; }
       public string Departament { get; set; }
       public string Especiality { get; set; }
       public ICollection<RegistroNotas> RegistroNotas { get; set; }
    }
}
