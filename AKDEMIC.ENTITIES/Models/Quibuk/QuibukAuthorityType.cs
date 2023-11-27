using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Quibuk
{
    public class QuibukAuthorityType
    {
        public int id { get; set; }
        [Required]
        [StringLength(191)]
        //[Column(TypeName = "nvarchar(191)")]
        public string nombre { get; set; }
        [Required]
        [StringLength(3)]
        //[Column(TypeName = "nvarchar(3)")]
        public string bibliography_tag_number { get; set; }
        public string descripcion { get; set; }
        //[Column(TypeName = "DateTime")]
        public DateTime created_at { get; set; }
        //[Column(TypeName = "DateTime")]
        public DateTime updated_at { get; set; }
    }
}
