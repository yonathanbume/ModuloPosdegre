using System.Collections.Generic;

namespace AKDEMIC.CORE.Helpers
{
    public partial class ConstantHelpers
    {

        #region Connections

        public static class Connections
        {
            public static string Get(bool isDev)
            {
                switch (GENERAL.Institution.Value)
                {
                    case Institution.Akdemic:
                    case Institution.PMESUT:
                    case Institution.UIGV:
                    case Institution.GRAY:
                        //return isDev ? "Server=localhost;Database=AKDEMIC;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=0;Default Command Timeout=0;" :
                        //               "Server=localhost;Database=AKDEMIC;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=0;Default Command Timeout=0;";
                        return isDev ? "Server=localhost;Database=AKDEMIC;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=AKDEMIC;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";

                    case Institution.UNAMAD:
                        return isDev ? "Server=localhost;Database=UNAMAD.AKDEMIC.DB;User Id=sqlserver;Password=root;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;" :
                                       "Server=localhost;Database=UNAMAD.AKDEMIC.DB;User Id=sqlserver;Password=root;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;";
                    case Institution.UNJBG:
                        return isDev ? "Server=localhost;Database=UNJBG.AKDEMIC.DB;User Id=sa;Password=root;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;" :
                                       "Server=172.16.1.3;Database=UNJBG.AKDEMIC.DB;User Id=sa;Password=root;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;";
                    case Institution.UNICA:
                        //return isDev ? "Server=localhost;Database=AKDEMIC;Trusted_Connection=True;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=0;" :
                        return isDev ? "Server=localhost;Initial Catalog=UNICA.AKDEMIC.DB;Persist Security Info=False;User ID=UNICA_DB_USER;Password=root;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;" :
                                       "Server=localhost;Initial Catalog=UNICA.AKDEMIC.DB;Persist Security Info=False;User ID=UNICA_DB_USER;Password=root;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;";
                    case Institution.UNICA_POST:
                        return isDev ? "" :
                                       "";
                    case Institution.UNICA_CEPU:
                        return isDev ? "Server=localhost;Initial Catalog=CEPRE.AKDEMIC.DB;Persist Security Info=False;User ID=UNICA_DB_USER;Password=root;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;" :
                                       "Server=localhost;Initial Catalog=CEPRE.AKDEMIC.DB;Persist Security Info=False;User ID=UNICA_DB_USER;Password=root;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;";
                    case Institution.UNAPI:
                        return isDev ? "Server=localhost;Database=UNAPIQUITOS.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNAPIQUITOS.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNFV:
                        return isDev ? "Server=localhost;Database=UNFV.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNFV.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNDC:
                        return isDev ? "Server=localhost;Database=UNDC.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNDC.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNAAA:
                        return isDev ? "Server=localhost;Database=UNAAA.SIGAU.DB;Uid=sa;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNAAA.SIGAU.DB;Uid=sa;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNAB:
                        return isDev ? "Server=localhost;Database=UNAB.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
									   "Server=localhost;Database=UNAB.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNTUMBES:
                        return isDev ? "Server=localhost;Database=UNT.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNT.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNIFSLB:
                        return isDev ? "Server=localhost;Database=UNIFSLB.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNIFSLB.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNISCJSA:
                        return isDev ? "Server=localhost;Database=UNISCJSA.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNISCJSA.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNAH:
                        return isDev ? "Server=localhost;Database=UNAH.SIGAU.DB;User Id=root;Password=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNAH.SIGAU.DB;User Id=root;Password=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNF:
                        return isDev ? "Server=localhost;Database=UNF.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNF.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNJ:
                        return isDev ? "Server=localhost;Database=UNJ.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNJ.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNAMBA:
                        return isDev ? "Server=172.16.10.108;Database=UNAMBA.SIGAU.DB;Uid=remoto;Pwd=Remoto45JK@#ssaa;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
									   "Server=172.16.10.108;Database=UNAMBA.SIGAU.DB;Uid=remoto;Pwd=Remoto45JK@#ssaa;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNAJMA:
                        return isDev ? "Server=localhost;Port=3306;Database=UNAJMA.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300; SslMode = none;" :
                                       "Server=localhost;Port=3306;Database=UNAJMA.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300; SslMode = none;";
                    case Institution.UNSCH:
                        return isDev ? "Server=localhost;Database=UNSCH.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNSCH.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    //case Institution.UNSM: // test
                    //    return isDev ? "Server=localhost;Database=UNSM.SIGAU.DB;Uid=root;Pwd=51G4U-UnSm2022$$;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                    //                   "Server=localhost;Database=UNSM.SIGAU.DB;Uid=root;Pwd=51G4U-UnSm2022$$;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNSM: // prod
                        return isDev ? "Server=localhost;Database=UNSM.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;" :
                                       "Server=localhost;Database=UNSM.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;";
                    //case Institution.UNCP: // test
                    //    return isDev ? "Server=152.44.37.241;Database=UNCP.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                    //                   "Server=152.44.37.241;Database=UNCP.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNCP: // prod
                        return isDev ? "Server=localhost;Database=UNCP.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=UNCP.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.ENSDF:
                        return isDev ? "Server=localhost;Database=folklore.sigau.db;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=folklore.sigau.db;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.EESPLI:
                        return isDev ? "Server=localhost;Database=ISEP.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;" :
                                       "Server=localhost;Database=ISEP.SIGAU.DB;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;";
                    case Institution.UNTRM:
                        return isDev ? "" :
                                       "";
                    case Institution.UNHEVAL:
                        return isDev ? "" :
                                       "";
                    case Institution.UNAM:
                        return isDev ? "" :
                                       "";
                    default:
                        return null;
                }
            }
        }

        #endregion

        #region CHAT

        public static class Chat
        {
            public static class System
            {
                public const byte Laurassia = 0;
                public const byte Enrollment = 1;
                public const byte JobExchange = 2;
                public const byte Admission = 3;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { Laurassia, "Aula Virtual" },
                    { Enrollment, "Matrícula" },
                    { JobExchange, "Bolsa de Trabajo" },
                    { Admission, "Admisión" },
                };
            }
            public static class Messages
            {
                public const string Success = "Tarea realizada satisfactoriamente";
                public const string Error = "Ocurrió un problema al procesar su consulta.";
            }
        }

        #endregion

        #region LAURASSIA

        public static class Laurassia
        {
            public static class Resource
            {

                public const byte File = 0;
                public const byte Link = 1;
            }

            public static class Reading
            {
                public const byte Required = 0;
                public const byte NonRequired = 1;
            }

            public static class VirtualClass
            {
                public static class Integration
                {
                    public const byte NONE = 0;
                    public const byte ZOOM = 1;
                    public const byte WEBEX = 2;
                    public const byte MEET = 3;
                    public const byte BBB = 4;
                    public const byte TEAMS = 5;

                    public static bool HasZoom()
                    {
                        return GENERAL.Institution.Value == Institution.PMESUT // ok
                            || GENERAL.Institution.Value == Institution.UNICA // ok
                            || GENERAL.Institution.Value == Institution.UNISCJSA // x
                            || GENERAL.Institution.Value == Institution.UNSM // ok
                            || GENERAL.Institution.Value == Institution.UNAAA // x
                            || GENERAL.Institution.Value == Institution.ENSDF; // ok
                    }
                    public static bool HasWebex()
                    {
                        return GENERAL.Institution.Value == Institution.UNAB;
                    }
                    public static bool HasMeet()
                    {
                        return GENERAL.Institution.Value == Institution.PMESUT
                            || GENERAL.Institution.Value == Institution.UNAH
                            || GENERAL.Institution.Value == Institution.UNF
                            || GENERAL.Institution.Value == Institution.UNSCH
                            || GENERAL.Institution.Value == Institution.UNTUMBES
                            || GENERAL.Institution.Value == Institution.UNAMAD;
                    }
                    public static bool HasBBB()
                    {
                        return GENERAL.Institution.Value == Institution.PMESUT
                            || GENERAL.Institution.Value == Institution.UNF;
                    }
                    public static bool HasTeams()
                    {
                        return GENERAL.Institution.Value == Institution.UNCP;
                    }
                    public static bool HasCredentials()
                    {
                        return HasZoom() || HasWebex() || HasMeet() || HasTeams();
                    }
                    public static bool HasIntegration()
                    {
                        return HasZoom() || HasWebex() || HasMeet() || HasBBB() || HasTeams();
                    }
                }
            }
        }

        #endregion
    }
}
