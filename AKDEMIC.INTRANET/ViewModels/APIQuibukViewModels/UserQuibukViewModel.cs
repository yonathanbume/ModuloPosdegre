using System;

namespace AKDEMIC.INTRANET.ViewModels.APIQuibukViewModels
{
    public class UserQuibukViewModel
    {
        public string username { get; set; }

        public string password { get; set; }

        public string email { get; set; }

        public string telefono { get; set; }
        public string direccion { get; set; }

        public byte estado_civil { get; set; }

        public string genero { get; set; }

        public string dni { get; set; }

        public string nombres { get; set; }

        public string paterno { get; set; }

        public string materno { get; set; }

        public DateTime fecha_nacimiento { get; set; }

        public Guid id_carrera { get; set; }

        public int AcademicYear { get; set; } = 10;
        public int tipo_usuario_id { get; set; }
        public string code_tipo_usuario { get; set; }
        public string Id { get; set; }
    }
}
