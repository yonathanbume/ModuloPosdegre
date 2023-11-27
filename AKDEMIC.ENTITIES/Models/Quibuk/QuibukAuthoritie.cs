using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Quibuk
{
    public class QuibukAuthoritie
    {
        public int id { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string descripcion { get; set; }
        public string unidad_subordinada { get; set; }
        public string json_data { get; set; }
        [Required]
        public int id_tipo { get; set; }
        public QuibukAuthorityType QuibukAuthorityType { get; set; }
        //[Column(TypeName = "DateTime")]
        public DateTime created_at { get; set; }
        //[Column(TypeName = "DateTime")]
        public DateTime updated_at { get; set; }

    }
}
