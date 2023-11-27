using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.REPOSITORY.Data.Temp
{
    public class ApplicationUserSeed2
    {
        public static ApplicationUser[] Seed(AkdemicContext context)
        {
            var result = new List<ApplicationUser>()
            {
                new ApplicationUser { Email = "superadmin@enchufate.pe", EmailConfirmed = true, Name = "Superadmin", UserName = "superadmin", Dni = "71252283" },
                new ApplicationUser { Email = "admin.procesos@enchufate.pe", EmailConfirmed = true, Name = "Administrador de Procesos", UserName = "admin.procesos", Dni = "71252284" },
                new ApplicationUser { Email = "admin.encuesta@enchufate.pe", EmailConfirmed = true, Name = "Administrador Encuesta", UserName = "admin.encuesta", Dni = "71252285" },
                new ApplicationUser { Email = "admision@enchufate.pe", EmailConfirmed = true, Name = "Admisión", UserName = "admision", Dni = "71252286" },


                new ApplicationUser { Email = "alumno@enchufate.pe", EmailConfirmed = true, Name = "Juan", PaternalSurname = "Flores", MaternalSurname = "Soto", UserName = "alumno", Dni = "74804725" },

                
                new ApplicationUser { Email = "fcalumno@enchufate.pe", EmailConfirmed = true, Name = "Roberto", PaternalSurname = "Flores", MaternalSurname = "Carrasco", UserName = "fcalumno", Dni = "74804726" },
                new ApplicationUser { Email = "dcalumno@enchufate.pe", EmailConfirmed = true, Name = "Luis", PaternalSurname = "Dávila", MaternalSurname = "Coronado", UserName = "dcalumno", Dni = "74804727" },

                new ApplicationUser { Email = "ztalumno@enchufate.pe", EmailConfirmed = true, Name = "Rocío", PaternalSurname = "Zavaleta", MaternalSurname = "Torres", UserName = "ztalumno", Dni = "74804728" },
                new ApplicationUser { Email = "mcalumno@enchufate.pe", EmailConfirmed = true, Name = "Fabiola", PaternalSurname = "Martinez", MaternalSurname = "Corante", UserName = "mcalumno", Dni = "74804729"},
                new ApplicationUser { Email = "vhalumno@enchufate.pe", EmailConfirmed = true, Name = "Sandra", PaternalSurname = "Vargas", MaternalSurname = "Huamán", UserName = "vhalumno", Dni = "74804730" },
                new ApplicationUser { Email = "smalumno@enchufate.pe", EmailConfirmed = true, Name = "Daniel", PaternalSurname = "Sanchez", MaternalSurname = "Miranda", UserName = "smalumno", Dni = "74804731" },
                new ApplicationUser { Email = "pyalumno@enchufate.pe", EmailConfirmed = true, Name = "Carlos", PaternalSurname = "Perez", MaternalSurname = "Yupanqui", UserName = "pyalumno", Dni = "74804732" },
                new ApplicationUser { Email = "vpalumno@enchufate.pe", EmailConfirmed = true, Name = "Alberto", PaternalSurname = "Valle", MaternalSurname = "Prado", UserName = "vpalumno", Dni = "74804732" },
                new ApplicationUser { Email = "qsalumno@enchufate.pe", EmailConfirmed = true, Name = "Luis", PaternalSurname = "Quispe", MaternalSurname = "Salaverry", UserName = "qsalumno", Dni = "74804732" },
                new ApplicationUser { Email = "pwalumno@enchufate.pe", EmailConfirmed = true, Name = "Eduardo", PaternalSurname = "Perez", MaternalSurname = "Wang", UserName = "pwalumno", Dni = "74804732" },
                new ApplicationUser { Email = "gsalumno@enchufate.pe", EmailConfirmed = true, Name = "Cesar", PaternalSurname = "García", MaternalSurname = "Smith", UserName = "gsalumno", Dni = "74804732" },
                new ApplicationUser { Email = "zgalumno@enchufate.pe", EmailConfirmed = true, Name = "Pablo", PaternalSurname = "Zúñiga", MaternalSurname = "Guerrero", UserName = "zgalumno", Dni = "74804732" },
                new ApplicationUser { Email = "htalumno@enchufate.pe", EmailConfirmed = true, Name = "Victor", PaternalSurname = "Hernandez", MaternalSurname = "Talavera", UserName = "htalumno", Dni = "74804732" },
                new ApplicationUser { Email = "olalumno@enchufate.pe", EmailConfirmed = true, Name = "Valeria", PaternalSurname = "Orozco", MaternalSurname = "Loo", UserName = "olalumno", Dni = "74804732" },
                new ApplicationUser { Email = "cmalumno@enchufate.pe", EmailConfirmed = true, Name = "Nicole", PaternalSurname = "Contreras", MaternalSurname = "Maldonado", UserName = "cmalumno", Dni = "74804732" },
                new ApplicationUser { Email = "gtalumno@enchufate.pe", EmailConfirmed = true, Name = "Jorge", PaternalSurname = "Gonzales", MaternalSurname = "Toledo", UserName = "gtalumno", Dni = "74804732" },

                new ApplicationUser { Email = "doctor@enchufate.pe", EmailConfirmed = true, Name = "Francisco", PaternalSurname = "Bejar", MaternalSurname = "Cuesta", UserName = "doctor", Dni = "75804790" },

                new ApplicationUser { Email = "autoridades@enchufate.pe", EmailConfirmed = true, Name = "Autoridad", UserName = "autoridad", Dni = "74804732" },
                new ApplicationUser { Email = "consultas.datos@enchufate.pe", EmailConfirmed = true, Name = "Consultas de Datos", UserName = "consultas.datos", Dni = "74804732" },
                new ApplicationUser { Email = "consultas.matricula@enchufate.pe", EmailConfirmed = true, Name = "Consultas Matricula", UserName = "consultas.matricula", Dni = "74804732" },
                new ApplicationUser { Email = "consultas.utd@enchufate.pe", EmailConfirmed = true, Name = "Consultas UTD", UserName = "consultas.utd", Dni = "74804732" },
                new ApplicationUser { Email = "decano@enchufate.pe", EmailConfirmed = true, Name = "Decano", UserName = "decano", Dni = "74804732" },
                new ApplicationUser { Email = "departamento.academico@enchufate.pe", EmailConfirmed = true, Name = "Departamento Académico", UserName = "departamento.academico", Dni = "74804732" },
                new ApplicationUser { Email = "director.escuela@enchufate.pe", EmailConfirmed = true, Name = "Director de Escuela", UserName = "director.escuela", Dni = "74804732" },
                new ApplicationUser { Email = "docentes@enchufate.pe", EmailConfirmed = true, Name = "Rafael Alonso", PaternalSurname = "Ramirez", MaternalSurname = "Soto", UserName = "docente", Dni = "70241682"},
                new ApplicationUser { Email = "frdocentes@enchufate.pe", EmailConfirmed = true, Name = "Francisco Rubén", PaternalSurname = "Peralta", MaternalSurname = "Sotelo", UserName = "frdocente", Dni = "74804732" },
                new ApplicationUser { Email = "radocentes@enchufate.pe", EmailConfirmed = true, Name = "Raúl Adrián", PaternalSurname = "Torres", MaternalSurname = "Villanueva", UserName = "radocente", Dni = "74804732" },
                new ApplicationUser { Email = "jedocentes@enchufate.pe", EmailConfirmed = true, Name = "Jaime Emilio", PaternalSurname = "Fernandes", MaternalSurname = "Maldonado", UserName = "jedocente", Dni = "74804732" },
                new ApplicationUser { Email = "agdocentes@enchufate.pe", EmailConfirmed = true, Name = "Antonio Gonzalo", PaternalSurname = "Soriano", MaternalSurname = "Huertas", UserName = "agdocente", Dni = "74804732" },
                new ApplicationUser { Email = "jmdocentes@enchufate.pe", EmailConfirmed = true, Name = "José Mariano", PaternalSurname = "Vargas", MaternalSurname = "Perea", UserName = "jmdocente", Dni = "74804732" },
                new ApplicationUser { Email = "apdocentes@enchufate.pe", EmailConfirmed = true, Name = "Ana Pilar", PaternalSurname = "Sanchez", MaternalSurname = "Sabater", UserName = "apdocente", Dni = "74804732" },
                new ApplicationUser { Email = "mcdocentes@enchufate.pe", EmailConfirmed = true, Name = "María Carmen", PaternalSurname = "Beltrán", MaternalSurname = "Olivares", UserName = "mcdocente", Dni = "74804732" },
                new ApplicationUser { Email = "cidocentes@enchufate.pe", EmailConfirmed = true, Name = "Cristina Isabel", PaternalSurname = "Quispe", MaternalSurname = "Céspedes", UserName = "cidocente", Dni = "74804732" },
                new ApplicationUser { Email = "jefe.registros@enchufate.pe", EmailConfirmed = true, Name = "Jefe de Registros", UserName = "jefe.registros", Dni = "74804732" },
                new ApplicationUser { Email = "registrador@enchufate.pe", EmailConfirmed = true, Name = "Registrador", UserName = "registrador", Dni = "74804732" },
                new ApplicationUser { Email = "soporte.usuarios@enchufate.pe", EmailConfirmed = true, Name = "Soporte de Usuarios", UserName = "soporte.usuarios", Dni = "74804732" },
                new ApplicationUser { Email = "soporte.docentes@enchufate.pe", EmailConfirmed = true, Name = "Soporte Docentes", UserName = "soporte.docentes", Dni = "74804732" },
                new ApplicationUser { Email = "supervisor@enchufate.pe", EmailConfirmed = true, Name = "Supervisor", UserName = "supervisor", Dni = "74804732" },
                new ApplicationUser { Email = "supervisor.vrac@enchufate.pe", EmailConfirmed = true, Name = "Supervisor VRAC", UserName = "supervisor.vrac", Dni = "74804732" },
                new ApplicationUser { Email = "tesorero@enchufate.pe", EmailConfirmed = true, Name = "Richard Gomez", UserName = "tesorero", Dni = "74804732" },

                new ApplicationUser { Email = "asistenteacademico@enchufate.pe", EmailConfirmed = true, Name = "Asistente Academico", UserName = "asistente.academico",Dni = "74804732"},

                new ApplicationUser { Email = "enchufate@enchufate.pe", EmailConfirmed = true, Name = "Enchufate", UserName = "empresa",Dni = "12345678"},
                new ApplicationUser { Email = "alumno.idioma@enchufate.pe", EmailConfirmed = true, Name = "alumno idioma", UserName = "alumno.idioma",Dni = "74804732"},
                
            };

            return result.ToArray();
        }
    }
}
