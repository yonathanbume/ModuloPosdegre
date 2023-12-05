using Amazon;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AKDEMIC.CORE.Helpers
{
    public partial class ConstantHelpers
    {
        public static class GENERAL
        {
            public static class DATABASES
            {
                public static int DATABASE = ConstantHelpers.Institution.Databases[Institution.Value];

                public const bool FULLTEXT_ENABLED = false;
                public static class CONNECTION_STRINGS
                {
                    public readonly static int CONNECTION_STRING = ConstantHelpers.DATABASES.CONNECTION_STRINGS.MYSQL.DEFAULT;
                }
                public static class VERSIONS
                {
                    public readonly static int VERSION = ConstantHelpers.DATABASES.VERSIONS.MYSQL.V8021;
                }
            }

            public static class Institution
            {
                public static int Value = ConstantHelpers.Institution.UNAMBA;
            }

            public static class Themes
            {
                public static int Value = Institution.Value;
            }
            public static class FileStorage
            {
                public static int STORAGE_MODE = ConstantHelpers.Institution.StorageMode[Institution.Value];
                public static string PATH = ConstantHelpers.Institution.Path[Institution.Value];
            }
            public static class ExternalAuthentication
            {
                public static bool GOOGLE = ConstantHelpers.Institution.GoogleAuth[Institution.Value];
                public static bool MICROSOFT = ConstantHelpers.Institution.MicrosoftAuth[Institution.Value];
                public static bool LINKEDIN = false;
            }
            public static class Authentication
            {
                public static bool SSO_ENABLED = false;
            }
        }

        public static class DATABASES
        {
            public const int MYSQL = 1;
            public const int SQL = 2;
            public const int PSQL = 3;
            public static class CONNECTION_STRINGS
            {
                public static Dictionary<Tuple<int, int>, string> VALUES = new Dictionary<Tuple<int, int>, string>()
                {
                    { new Tuple<int, int>(DATABASES.MYSQL, MYSQL.DEFAULT), MYSQL.VALUES[MYSQL.DEFAULT] },
                    { new Tuple<int, int>(DATABASES.SQL, SQL.DEFAULT), SQL.VALUES[SQL.DEFAULT] },
                    { new Tuple<int, int>(DATABASES.PSQL, PSQL.DEFAULT), PSQL.VALUES[PSQL.DEFAULT] },
                };
                public static class MYSQL
                {
                    public const int DEFAULT = 0;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { DEFAULT, "MySqlDefaultConnection" },
                    };
                }
                public static class SQL
                {
                    public const int DEFAULT = 0;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { DEFAULT, "SqlDefaultConnection" },
                    };
                }
                public static class PSQL
                {
                    public const int DEFAULT = 0;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { DEFAULT, "PostgreSqlDefaultConnection" },
                    };
                }
            }
            public static class VERSIONS
            {
                public static class MYSQL
                {
                    public const int VNULL = 0;
                    public const int V5717 = 1;
                    public const int V5723 = 2;
                    public const int V5726 = 3;
                    public const int V8021 = 4;
                    public static Dictionary<int, Version> VALUES = new Dictionary<int, Version>()
                    {
                        { VNULL, null },
                        { V5717, new Version(5, 7, 17) },
                        { V5723, new Version(5, 7, 23) },
                        { V5726, new Version(5, 7, 26) },
                        { V8021, new Version(8, 0, 21) },
                    };
                }
            }
        }
        public static class VISA
        {
            public const int DEVELOPMENT = 1;
            public const int PRODUCTION = 2;
            public static Dictionary<int, string> API = new Dictionary<int, string>()
            {
                { DEVELOPMENT, "https://apitestenv.vnforapps.com/" },
                { PRODUCTION, "https://apiprod.vnforapps.com/" },
            };
            public static Dictionary<int, string> SCRIPT = new Dictionary<int, string>()
            {
                { DEVELOPMENT, "https://static-content-qas.vnforapps.com/v2/js/checkout.js?qa=true" },
                { PRODUCTION, "https://static-content.vnforapps.com/v2/js/checkout.js" },
            };
            public static class States
            {
                public const int NotConfirmed = 0;
                public const int Confirmed = 1;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NotConfirmed, "No Confirmado" },
                    { Confirmed, "Confirmado" }
                };
            }
        }
        public static class SYSTEMS
        {
            public const byte INTRANET = 1;
            public const byte ENROLLMENT = 2;
            public const byte SCALE = 3;
            public const byte DOCUMENTARY_PROCEDURE = 4;
            public const byte INDICATORS = 5;
            public const byte INVESTIGATION = 6;
            public const byte ADMISSION = 7;
            public const byte EVALUATION = 8;
            public const byte ACADEMIC_EXCHANGE = 9;
            public const byte INTEREST_GROUP = 10;
            public const byte TEACHING_MANAGEMENT = 11;
            public const byte LAURASSIA = 12;
            public const byte TRANSPARENCY_PORTAL = 13;
            public const byte LANGUAGE_CENTER = 14;
            public const byte KARDEX = 15;
            public const byte RESERVATIONS = 16;
            public const byte JOBEXCHANGE = 17;
            public const byte TUTORING = 18;
            public const byte PREUNIVERSITARY = 19;
            public const byte RESOLUTIVE_ACTS = 20;
            public const byte SISCO = 21;
            public const byte DEGREE = 22;
            public const byte ECONOMICMANAGEMENT = 23;
            public const byte COMPUTERMANAGEMENT = 24;
            public const byte CHAT = 25;
            public const byte TEACHER_HIRING = 26;
            public const byte ADMINISTRATION = 27;
            public const byte CONTINUINGEDUCATION = 28;
            public const byte INSTITUTIONAL_WELFARE = 29;
            public const byte PAYROLL = 30;
            public const byte POSDEGREE = 31;
        }
        public static class Lockout
        {
            public const int Time = 15;
            public const int MaxFailedAccessAttempts = 6;
        }
        public static class AmazonS3
        {
            public static string AccessKey = Institution.AmazonS3Config[GENERAL.Institution.Value].Item1;
            public static string SecretKey = Institution.AmazonS3Config[GENERAL.Institution.Value].Item2;
            public static string BucketName = Institution.AmazonS3Config[GENERAL.Institution.Value].Item3;
            public static RegionEndpoint RegionEndpoint = Institution.AmazonS3Config[GENERAL.Institution.Value].Item4;
        }

        public static class Institution
        {
            public const int Akdemic = 1;
            public const int UNAM = 2;
            public const int UNAMAD = 3;
            public const int UNICA = 4;
            public const int UNJBG = 5;
            public const int UNICA_POST = 7;
            public const int UNFV = 8;
            public const int PMESUT = 9;
            public const int UNDC = 10;
            public const int UNAAA = 11;
            public const int UNTRM = 12;
            public const int UNAB = 13;
            public const int UNSM = 14;
            public const int UNAPI = 15;
            public const int UNTUMBES = 16;
            public const int UNIFSLB = 17;
            public const int UNISCJSA = 18;
            public const int UNAH = 19;
            public const int UNF = 20;
            public const int UNJ = 21;
            public const int UNAMBA = 22;
            public const int UNAJMA = 23;
            public const int GRAY = 24;
            public const int UNSCH = 25;
            public const int UNHEVAL = 26;
            public const int UNCP = 27;
            public const int EESPLI = 28;
            public const int ENSDF = 29;
            public const int UNICA_CEPU = 30;
            public const int UIGV = 31;

            public static Dictionary<int, string> Abbreviations = new Dictionary<int, string>()
            {
                { Akdemic, "AKDEMIC" },
                { UNAM, "UNAM" },
                { UNAMAD, "UNAMAD" },
                { UNICA, "UNICA" },
                { UNJBG, "UNJBG" },
                { UNICA_POST, "UNICA" },
                { UNICA_CEPU, "UNICA" },
                { UNFV, "UNFV" },
                { PMESUT, "PMESUT" },
                { UNDC, "UNDC" },
                { UNAAA, "UNAAA" },
                { UNTRM, "UNTRM" },
                { UNAB, "UNAB" },
                { UNSM, "UNSM" },
                { UNAPI, "UNAPI" },
                { UNTUMBES, "UNTUMBES" },
                { UNIFSLB, "UNIFSLB" },
                { UNISCJSA, "UNISCJSA" },
                { UNAH, "UNAH" },
                { UNF, "UNF" },
                { UNJ, "UNJ" },
                { UNAMBA, "UNAMBA" },
                { UNAJMA, "UNAJMA" },
                { GRAY, "SIGAU" },
                { UNSCH, "UNSCH" },
                { UNHEVAL, "UNHEVAL" },
                { UNCP, "UNCP" },
                { EESPLI, "EESPLI" },
                { ENSDF , "ENSDF"},
                { UIGV , "UIGV"},
            };
            public static Dictionary<int, string> Locations = new Dictionary<int, string>()
            {
                { Akdemic, "Lima" },
                { UNAM, "Moquegua" },
                { UNAMAD, "Madre de Dios" },
                { UNICA, "Ica" },
                { UNICA_CEPU, "Ica" },
                { UNJBG, "Tacna" },
                { UNICA_POST, "Ica" },
                { UNFV, "Lima" },
                { PMESUT, "Lima" },
                { UNDC, "Lima" },
                { UNAAA, "Yurimaguas" },
                { UNTRM, "Amazonas" },
                { UNAB, "Barranca" },
                { UNSM, "Tarapoto" },
                { UNAPI, "Iquitos" },
                { UNTUMBES, "Tumbes" },
                { UNIFSLB, "Bagua" },
                { UNISCJSA, "Junín" },
                { UNAH, "Huanta" },
                { UNF, "Piura" },
                { UNJ, "Cajamarca" },
                { UNAMBA, "Apurímac" },
                { UNAJMA, "Andahuaylas" },
                { GRAY, "SIGAU" },
                { UNSCH, "Ayacucho" },
                { UNHEVAL, "Huánuco" },
                { UNCP, "Huancayo" },
                { EESPLI, "Arequipa" },
                { ENSDF , "Lima"},
                { UIGV , "UIGV"},
            };
            public static Dictionary<int, string> Codes = new Dictionary<int, string>()
            {
                { Akdemic, "" },
                { UNAM, "035" },
                { UNAMAD, "029" },
                { UNICA, "009" },
                { UNICA_CEPU, "009" },
                { UNJBG, "032" },
                { UNICA_POST, "009" },
                { UNFV, "" },
                { PMESUT, "" },
                { UNDC, "" },
                { UNAAA, "" },
                { UNTRM, "" },
                { UNAB, "" },
                { UNSM, "" },
                { UNAPI, "" },
                { UNTUMBES, "" },
                { UNIFSLB, "" },
                { UNISCJSA, "" },
                { UNAH, "" },
                { UNF, "" },
                { UNJ, "" },
                { UNAMBA, "" },
                { UNAJMA, "" },
                { GRAY, "" },
                { UNSCH, "" },
                { UNHEVAL, "" },
                { UNCP, "" },
                { EESPLI, "" },
                { ENSDF , "" },
                { UIGV , ""},
            };
            public static Dictionary<int, string> Repositories = new Dictionary<int, string>()
            {
                { Akdemic, "" },
                { UNAM, "http://repositorio.unam.edu.pe/handle/UNAM/" },
                { UNAMAD, "http://repositorio.unamad.edu.pe/handle/UNAMAD/" },
                { UNICA, "http://repositorio.unica.edu.pe/handle/UNICA/" },
                { UNJBG, "http://repositorio.unjbg.edu.pe/handle/UNJBG/" },
                { UNICA_POST, "http://repositorio.unica.edu.pe/handle/UNICA/" },
                { UNICA_CEPU, "http://repositorio.unica.edu.pe/handle/UNICA/" },
                { UNFV, "" },
                { PMESUT, "" },
                { UNDC, "" },
                { UNAAA, "" },
                { UNTRM, "" },
                { UNAB, "http://repositorio.unab.edu.pe/handle/UNAB/" },
                { UNSM, "" },
                { UNAPI, "" },
                { UNTUMBES, "" },
                { UNIFSLB, "" },
                { UNISCJSA, "" },
                { UNAH, "" },
                { UNF, "" },
                { UNJ, "" },
                { UNAMBA, "" },
                { UNAJMA, "" },
                { GRAY, "" },
                { UNSCH, "" },
                { UNHEVAL, "" },
                { UNCP, "" },
                { EESPLI, "" },
                { ENSDF , "" },
                { UIGV , ""},
            };
            public static Dictionary<int, string> Names = new Dictionary<int, string>()
            {
                { Akdemic, "AKDEMIC" },
                { UNAM, "Universidad Nacional de Moquegua" },
                { UNAMAD, "Universidad Nacional Amazónica de Madre de Dios" },
                { UNICA, "Universidad Nacional \"San Luis Gonzaga\"" },
                { UNJBG, "Universidad Nacional Jorge Basadre Grohmann" },
                { UNICA_POST, "Universidad Nacional \"San Luis Gonzaga\" de Ica" },
                { UNICA_CEPU, "Centro de Estudios Pre universitario CEPU - UNICA" },
                { UNFV, "Universidad Nacional Federico Villarreal" },
                { PMESUT, "PMESUT" },
                { UNDC, "Universidad Nacional de Cañete" },
                { UNAAA, "Universidad Nacional Autónoma de Alto Amazonas" },
                { UNTRM, "Universidad Nacional Toribio Rodríguez de Mendoza de Amazonas" },
                { UNAB, "Universidad Nacional de Barranca" },
                { UNSM, "Universidad Nacional de San Martín" },
                { UNAPI, "Universidad Nacional de la Amazonía Peruana" },
                { UNTUMBES, "Universidad Nacional de Tumbes" },
                { UNIFSLB, "Universidad Nacional Intercultural Fabiola Salazar Leguía de Bagua" },
                { UNISCJSA, "Universidad Nacional Intercultural de la Selva Central Juan Santos Atahualpa" },
                { UNAH, "Universidad Nacional Autónoma de Huanta" },
                { UNF, "Universidad Nacional de Frontera" },
                { UNJ, "Universidad Nacional de Jaén" },
                { UNAMBA, "Universidad Nacional Micaela Bastidas de Apurímac" },
                { UNAJMA, "Universidad Nacional José María Arguedas" },
                { GRAY, "SIGAU" },
                { UNSCH, "Universidad Nacional de San Cristobal de Huamanga" },
                { UNHEVAL, "Universidad Nacional Hermilio Valdizán" },
                { UNCP, "Universidad Nacional del Centro del Perú" },
                { EESPLI, "EESP La Inmaculada" },
                { ENSDF, "Escuela Nacional Superior de Folklore José María Arguedas" },
                { UIGV , "Universidad Inca Garcilaso de la Vega"}
            };
            public static Dictionary<int, string> SupportEmail = new Dictionary<int, string>()
            {
                { Akdemic, "acarbajal@enchufate.pe" },
                { UNAM, "enchufatest@gmail.com" },
                { UNAMAD, "registros.academicos@unamad.edu.pe" },
                { UNICA, "soporte2@unica.edu.pe" },
                { UNJBG, "soporte.informatica@unjbg.edu.pe" },
                { UNICA_POST, "enchufatest@gmail.com" },
                { UNICA_CEPU, "soporte2@unica.edu.pe" },
                { UNFV, "enchufatest@gmail.com" },
                { PMESUT, "acarbajal@enchufate.pe" },
                { UNDC, "soporte2@enchufate.pe" },
                { UNAAA, "aula-virtual@unaaa.edu.pe" },
                { UNTRM, "enchufatest@gmail.com" },
                { UNAB, "acarbajal@enchufate.pe" },
                { UNSM, "aulavirtual@unsm.edu.pe" },
                { UNAPI, "egresadosunap15@gmail.com" },
                { UNTUMBES, "sigaucampus@untumbes.edu.pe" },
                { UNIFSLB, "acarbajal@enchufate.pe" },
                { UNISCJSA, "acarbajal@enchufate.pe" },
                { UNAH, "aulavirtual@unah.edu.pe" },
                { UNF, "aulavirtual@unf.edu.pe" },
                { UNJ, "acarbajal@enchufate.pe" },
                { UNAMBA, "infobolsa@unamba.edu.pe" },
                { UNAJMA, "acarbajal@enchufate.pe" },
                { GRAY, "acarbajal@enchufate.pe" },
                { UNSCH, "aula.virtual@unsch.edu.pe" },
                { UNHEVAL, "acarbajal@enchufate.pe" },
                { UNCP, "acarbajal@enchufate.pe" },
                { EESPLI, "acarbajal@enchufate.pe" },
                { ENSDF , "aulavirtualensfjma@gmail.com" },
                { UIGV , "acarbajal@enchufate.pe" },
            };
            public static Dictionary<int, string> SupportEmailName = new Dictionary<int, string>()
            {
                { Akdemic, "acarbajal" },
                { UNAM, "enchufatest" },
                { UNAMAD, "registros.academicos" },
                { UNICA, "soporte2" },
                { UNJBG, "soporte.informatica" },
                { UNICA_POST, "enchufatest" },
                { UNICA_CEPU, "soporte2" },
                { UNFV, "enchufatest" },
                { PMESUT, "acarbajal" },
                { UNDC, "soporte2" },
                { UNAAA, "Aula Virtual UNAAA" },
                { UNTRM, "enchufatest" },
                { UNAB, "acarbajal" },
                { UNSM, "Aula Virtual UNSM" },
                { UNAPI, "CorreoSoporte" },
                { UNTUMBES, "Aula Virtual UNTUMBES" },
                { UNIFSLB, "acarbajal" },
                { UNISCJSA, "acarbajal" },
                { UNAH, "Aula virtual UNAH" },
                { UNF, "Aula Virtual UNF" },
                { UNJ, "acarbajal" },
                { UNAMBA, "enchufatest" },
                { UNAJMA, "acarbajal" },
                { GRAY, "acarbajal" },
                { UNSCH, "Aula Virtual UNSCH" },
                { UNHEVAL, "acarbajal" },
                { UNCP, "acarbajal" },
                { EESPLI, "acarbajal" },
                { ENSDF, "Aula Virtual UNSDF" },
                { UIGV , "acarbajal" },
            };
            public static Dictionary<int, string> SupportEmailPassword = new Dictionary<int, string>()
            {
                { Akdemic, "Enchufate.2020" },
                { UNAM, "Enchufate.2020" },
                { UNAMAD, "0r@//2021**" },
                { UNICA, "Enchufate.2022" },
                { UNJBG, "Lpc23$Us1" },
                { UNICA_POST, "Enchufate.2020" },
                { UNICA_CEPU, "Enchufate.2022" },
                { UNFV, "Enchufate.2020" },
                { PMESUT, "Enchufate.2020" },
                { UNDC, "Enchufate.2018" },
                { UNAAA, "unaaa123" },
                { UNTRM, "Enchufate.2020" },
                { UNAB, "Enchufate.2020" },
                { UNSM, "aulavirtual@2021." },
                { UNAPI, "Jorgedise15" },
                { UNTUMBES, "ffkonudafgaqsxxo" },
                { UNIFSLB, "Enchufate.2020" },
                { UNISCJSA, "Enchufate.2020" },
                { UNAH, "Unah2021" },
                { UNF, "C9yM*MBJ" },
                { UNJ, "Enchufate.2020" },
                { UNAMBA, "nsphcocnewlnfrqo" },
                { UNAJMA, "Enchufate.2020" },
                { GRAY, "Enchufate.2020" },
                { UNSCH, "123456oti" },
                { UNHEVAL, "Enchufate.2020" },
                { UNCP, "Enchufate.2020" },
                { EESPLI, "Enchufate.2020" },
                { ENSDF , "bjbeetmztxshuqlb" },
                { UIGV , "Enchufate.2020" },
            };
            public static Dictionary<int, string> Values = new Dictionary<int, string>()
            {
                { Akdemic, "akdemic" },
                { UNAM, "unam" },
                { UNAMAD, "unamad" },
                { UNICA, "unica" },
                { UNJBG, "unjbg" },
                { UNICA_POST, "unica_post"},
                { UNICA_CEPU, "unica_cepu" },
                { UNFV, "unfv"},
                { PMESUT, "pmesut"},
                { UNDC, "undc"},
                { UNAAA, "unaaa"},
                { UNTRM, "untrm"},
                { UNAB, "unab"},
                { UNSM, "unsm"},
                { UNAPI, "unapi"},
                { UNTUMBES, "untumbes"},
                { UNIFSLB, "unifslb"},
                { UNISCJSA, "uniscjsa"},
                { UNAH, "unah"},
                { UNF, "unf"},
                { UNJ, "unj"},
                { UNAMBA, "unamba"},
                { UNAJMA, "unajma"},
                { GRAY, "gray"},
                { UNSCH, "unsch"},
                { UNHEVAL, "unheval"},
                { UNCP, "uncp"},
                { EESPLI, "eespli"},
                { ENSDF , "ensdf" },
                { UIGV , "uigv" },

            };
            public static Dictionary<int, string> VisaMerchant = new Dictionary<int, string>()
            {
                { Akdemic, "342062522" },
                { UNAM, "" },
                { UNAMAD, "342062522" },
                { UNICA, "" },
                { UNJBG, "" },
                { UNICA_POST, ""},
                { UNICA_CEPU, "" },
                { UNFV, ""},
                { PMESUT, "342062522"},
                { UNDC, ""},
                { UNAAA, ""},
                { UNTRM, ""},
                { UNAB, ""},
                { UNSM, ""},
                { UNAPI, ""},
                { UNTUMBES, ""},
                { UNIFSLB, ""},
                { UNISCJSA, ""},
                { UNAH, ""},
                { UNF, ""},
                { UNJ, ""},
                { UNAMBA, ""},
                { UNAJMA, ""},
                { GRAY, ""},
                { UNSCH, ""},
                { UNHEVAL, ""},
                { UNCP, ""},
                { EESPLI, ""},
                { ENSDF , "" },
                { UIGV , "" }
            };
            public static Dictionary<int, string> VisaCredentials = new Dictionary<int, string>()
            {
                //User:Password
                { Akdemic, "" },
                { UNAM, "" },
                { UNAMAD, "" },
                { UNICA, "" },
                { UNJBG, "" },
                { UNICA_POST, ""},
                { UNICA_CEPU, "" },
                { UNFV, ""},
                { PMESUT, ""},
                { UNDC, ""},
                { UNAAA, ""},
                { UNTRM, ""},
                { UNAB, ""},
                { UNSM, ""},
                { UNAPI, ""},
                { UNTUMBES, ""},
                { UNIFSLB, ""},
                { UNISCJSA, ""},
                { UNAH, ""},
                { UNF, ""},
                { UNJ, ""},
                { UNAMBA, ""},
                { UNAJMA, ""},
                { GRAY, ""},
                { UNSCH, ""},
                { UNHEVAL, ""},
                { UNCP, ""},
                { EESPLI, ""},
                { ENSDF, "" },
                { UIGV, "" },
            };
            public static Dictionary<int, string> VisaTestCredentials = new Dictionary<int, string>()
            {
                //User:Password
                { Akdemic, "integraciones.visanet@necomplus.com:d5e7nk$M" },
                { UNAM, "" },
                { UNAMAD, "integraciones.visanet@necomplus.com:d5e7nk$M" },
                { UNICA, "" },
                { UNJBG, "" },
                { UNICA_POST, ""},
                { UNICA_CEPU, "" },
                { UNFV, ""},
                { PMESUT, "integraciones.visanet@necomplus.com:d5e7nk$M"},
                { UNDC, ""},
                { UNAAA, ""},
                { UNTRM, ""},
                { UNAB, ""},
                { UNSM, ""},
                { UNAPI, ""},
                { UNTUMBES, ""},
                { UNIFSLB, ""},
                { UNISCJSA, ""},
                { UNAH, ""},
                { UNF, ""},
                { UNJ, ""},
                { UNAMBA, ""},
                { UNAJMA, ""},
                { GRAY, ""},
                { UNSCH, ""},
                { UNHEVAL, ""},
                { UNCP, ""},
                { EESPLI, ""},
                { ENSDF , ""},
                { UIGV, "" },
            };
            public static Dictionary<int, int> Databases = new Dictionary<int, int>()
            {
                { Akdemic, DATABASES.MYSQL },
                { PMESUT, DATABASES.MYSQL },
                { UNAM, DATABASES.MYSQL },
                { UNAMAD, DATABASES.SQL },
                { UNICA, DATABASES.SQL },
                { UNICA_POST, DATABASES.SQL },
                { UNICA_CEPU, DATABASES.SQL },
                { UNJBG, DATABASES.SQL },
                { UNAAA, DATABASES.MYSQL },
                { UNAB, DATABASES.MYSQL },
                { UNAH, DATABASES.MYSQL },
                { UNDC, DATABASES.MYSQL },
                { UNIFSLB, DATABASES.MYSQL },
                { UNAJMA, DATABASES.MYSQL },
                { UNAMBA, DATABASES.MYSQL },
                { UNF, DATABASES.MYSQL },
                { UNISCJSA, DATABASES.MYSQL },
                { UNTUMBES, DATABASES.MYSQL },
                { UNAPI, DATABASES.MYSQL },
                { UNFV, DATABASES.MYSQL },
                { UNJ, DATABASES.MYSQL },
                { UNSM, DATABASES.MYSQL },
                { UNTRM, DATABASES.MYSQL },
                { GRAY, DATABASES.MYSQL },
                { UNSCH, DATABASES.MYSQL },
                { UNHEVAL, DATABASES.MYSQL },
                { UNCP, DATABASES.MYSQL },
                { EESPLI, DATABASES.MYSQL },
                { ENSDF, DATABASES.MYSQL },
                { UIGV, DATABASES.MYSQL },
            };
            public static Dictionary<int, int> StorageMode = new Dictionary<int, int>()
            {
                { Akdemic, FileStorage.Mode.SERVER_STORAGE_MODE },
                { PMESUT, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNAM, FileStorage.Mode.BLOB_STORAGE_MODE },
                { UNAMAD, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNICA, FileStorage.Mode.HUAWEI_STORAGE_MODE },
                { UNICA_POST, FileStorage.Mode.BLOB_STORAGE_MODE },
                { UNICA_CEPU, FileStorage.Mode.HUAWEI_STORAGE_MODE },
                { UNJBG, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNAAA, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNAB, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNAH, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNDC, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNIFSLB, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNAJMA, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNAMBA, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNF, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNISCJSA, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNTUMBES, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNAPI, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNFV, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNJ, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNSM, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNTRM, FileStorage.Mode.SERVER_STORAGE_MODE },
                { GRAY, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNSCH, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNHEVAL, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UNCP, FileStorage.Mode.SERVER_STORAGE_MODE },
                { EESPLI, FileStorage.Mode.SERVER_STORAGE_MODE },
                { ENSDF, FileStorage.Mode.SERVER_STORAGE_MODE },
                { UIGV, FileStorage.Mode.SERVER_STORAGE_MODE },
            };

            public static Dictionary<int, Tuple<string, string, string, Amazon.RegionEndpoint>> AmazonS3Config = new Dictionary<int, Tuple<string, string, string, Amazon.RegionEndpoint>>()
            {
                { Akdemic, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { PMESUT, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNAM, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNAMAD, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNICA, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNICA_POST, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNICA_CEPU, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNJBG, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNAAA, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNAB, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNAH, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNDC, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNIFSLB, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNAJMA, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNAMBA, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNF, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNISCJSA, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNTUMBES, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNAPI, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNFV, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNJ, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNSM, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNTRM, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { GRAY, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNSCH, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNHEVAL, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
                { UNCP, new Tuple<string, string , string, Amazon.RegionEndpoint>("AKIAUSYCDPCGHJZJX5BH", "0mymAqfwiOiEyp/JeIySwUwj0aWG5e/jWj/Y9/rp", "erpakdemicbucket" , Amazon.RegionEndpoint.USEast1) },
                { EESPLI, new Tuple<string, string , string, Amazon.RegionEndpoint>(null,null,null, null) },
                { ENSDF ,  new Tuple<string, string , string, Amazon.RegionEndpoint>(null,null,null, null) },
                { UIGV, new Tuple<string, string , string , Amazon.RegionEndpoint>(null,null,null, null) },
            };

            public static Dictionary<int, string> Path = new Dictionary<int, string>()
            {
                { Akdemic, "/usr/share/nginx/html/common/sigau" },
                { PMESUT, "/usr/share/nginx/html/common/sigau" },
                { UNAM, null },
                { UNAMAD, "C:/inetpub/files" },
                { UNICA, null },
                { UNICA_POST, null },
                { UNICA_CEPU, null },
                { UNJBG, "D:/akdemic" },
                { UNAAA, "C:/inetpub/wwwroot/common/sigau" },
                { UNAB, "/usr/share/nginx/html/common/sigau" },
                { UNAH, "/usr/share/nginx/html/common/sigau" },
                { UNDC, "/usr/share/nginx/html/common/sigau" },
                { UNIFSLB, "/home/usr/share/nginx/html/common/sigau" },
                { UNAJMA, "/home/usr/share/nginx/html/common/sigau" },
                { UNAMBA, "/usr/share/nginx/html/common/sigau" },
                { UNF, "C:/inetpub/wwwroot/common/sigau" },
                { UNISCJSA, "/usr/share/nginx/html/common/sigau" },
                { UNTUMBES, "/usr/share/nginx/html/common/sigau" },
                { UNAPI, "/home/usr/share/nginx/html/common/sigau" },
                { UNFV, "/mnt/usr/share/nginx/html/common/sigau" },
                { UNJ, "/usr/share/nginx/html/common/sigau" },
                { UNSM, "E:/Vhosts/common/sigau" },
                //{ UNSM, "/home/usr/share/nginx/html/common/sigau" },
                { UNTRM, "D:/inetpub/wwwroot/common/sigau" },
                { GRAY, "/usr/share/nginx/html/common/sigau" },
                { UNSCH, "/usr/share/nginx/html/common/sigau" },
                //{ UNSCH, "C:/inetpub/wwwroot/common/akdemic" },
                { UNHEVAL, "/usr/share/nginx/html/common/sigau" },
                { UNCP, "/usr/share/nginx/html/common/sigau" },
                { EESPLI, "/usr/share/nginx/html/common/sigau" },
                { ENSDF, "/usr/share/nginx/html/common/sigau" },
                { UIGV, "/usr/share/nginx/html/common/sigau" },
                //{ UNCP, null },
            };
            public static Dictionary<int, bool> GoogleAuth = new Dictionary<int, bool>()
            {
                { Akdemic, false },
                { UNAM, false },
                { UNAMAD, false },
                { UNICA, false },
                { UNJBG, false },
                { UNICA_POST, false},
                { UNICA_CEPU, false },
                { UNFV, false},
                { PMESUT, true},
                { UNDC, false},
                { UNAAA, false},
                { UNTRM, false},
                { UNAB, false},
                { UNSM, false},
                { UNAPI, false},
                { UNTUMBES, false},
                { UNIFSLB, false},
                { UNISCJSA, false},
                { UNAH, false},
                { UNF, true},
                { UNJ, false},
                { UNAMBA, false},
                { UNAJMA, false},
                { GRAY, false},
                { UNSCH, false},
                { UNHEVAL, false},
                { UNCP, false},
                { EESPLI, false},
                { ENSDF, false },
                { UIGV, false },
            };
            public static Dictionary<int, bool> MicrosoftAuth = new Dictionary<int, bool>()
            {
                { Akdemic, false },
                { UNAM, false },
                { UNAMAD, false },
                { UNICA, false },
                { UNJBG, false },
                { UNICA_POST, false},
                { UNICA_CEPU, false },
                { UNFV, false},
                { PMESUT, true},
                { UNDC, false},
                { UNAAA, false},
                { UNTRM, false},
                { UNAB, false},
                { UNSM, false},
                { UNAPI, false},
                { UNTUMBES, false},
                { UNIFSLB, false},
                { UNISCJSA, false},
                { UNAH, false},
                { UNF, false},
                { UNJ, false},
                { UNAMBA, false},
                { UNAJMA, false},
                { GRAY, false},
                { UNSCH, false},
                { UNHEVAL, false},
                { UNCP, false},
                { EESPLI, false},
                { ENSDF, false },
                { UIGV, false },
            };
        }
        public static class CountryCode
        {
            public const string Current = "PE";
        }
        public static class Language
        {
            public const string English = "en";
            public const string Spanish = "es";
        }
        public static class TimeZoneInfo
        {
            public const bool DisableDaylightSavingTime = true;
            public const int Gmt = -5;
        }
        public const string LINUX_TIMEZONE_ID = "America/Bogota";
        public const string OSX_TIMEZONE_ID = "America/Cayman";
        public const string WINDOWS_TIMEZONE_ID = "SA Pacific Standard Time";
        public const string PASSWORDQUIBUK = "_Api-Quibuk_";
        public const string PASSWORDINVESTIGATION = "_Api-Investigation_";
        public static class AzureEnvironment
        {
            public const string AZURE_ENVIRONMENT_VARIABLE = "WEBSITE_SITE_NAME";
            public const string AZURE_SHELL_SCRIPT_PATH = "azureDependencies.sh";
        }
        public static class Seeder
        {
            public const bool Enabled = false;
            public const string IdentityRole = "ApplicationRoleSeed";
            public const string UserRole = "UserRoleSeed";
            public static readonly List<string> Priorities = new List<string>()
            {
                "ApplicationRoleSeed",              "UserRoleSeed",                     "DependencySeed",               "UserDependencySeed",                   "CountrySeed",                      "DepartmentSeed",                   "ProvinceSeed",
                "DistrictSeed",                     "ClassifierSeed",                   "SimulationSubjectSeed",        "SimulationSubjectAreaSeed",            "SimulationQuestionSeed",           "SimulationAnswerSeed",             "PsychologyCategorySeed",
                "PsychologyTestQuestionSeed",       "AnnouncementSeed",                 "RolAnnouncementSeed",          "EventTypeSeed",                        "EventSeed",                        /*"SurveySeed",                       "SurveyUserSeed",
                "QuestionSeed",                     "AnswerSeed",                       "AnswerByUserSeed",*/             "AdmissionSubjectSeed",                 "AdmissionSubjectAreaSeed",       "AdmissionQuestionSeed",            "AdmissionAnswerSeed",
                "FacultySeed",                      "CurriculumAreaSeed",               "FacultyCurriculumAreaSeed",    "AcademicDepartmentSeed",               "DeanSeed",                         "CareerSeed",
                "SimulationExamRegistrationSeed",   "CourseComponentSeed",              "AccountingPlanSeed",           "CurrentAccountSeed",
                "TermSeed",                         "AcademicCalendarDateSeed",         "CurriculumSeed",               "NewSeed",                              "ManualSeed",                       "FrequentQuestionSeed",             "PsychologicalDiagnosticSeed",
                "ConsultTypeSeed",                  "ForumSeed",                        /*"BankSeed",*/                 "UITSeed",                              "RecordSubjectTypeSeed",            /*"PaymentMethodSeed",*/            "ConfigurationSeed",
                "DocumentaryRecordTypeSeed",        "SettlementSeed",                   "IndicatorsSeed",               "TransparencyPortalGeneralSeed",        "DocumentTypeSeed",                 "ConceptSeed",                      "ExternalUserSeed",
                "WorkerCapPositionSeed",            "WorkerLaborRegimeSeed",            "WorkerLaborCategorySeed",      "WorkerLaborConditionSeed",             "WorkerPositionClassificationSeed", "AdmissionTypeSeed",                "AdmissionTypeDescountSeed",
                "ApplicationTermSeed",              "CourseTypeSeed",                   "CreditCategorySeed",           "AreaSeed",                             "AcademicYearSeed",                 "AdmissionExamSeed",                "AdmissionExamAdmissionTypeSeed",
                "AdmissionExamSubjectAreaSeed",    /* "DoctorSpecialtySeed",*/
                "StudentSeed",                      "EnrollmentTurnSeed",               "StudentFamilySeed",            "SpecialtySeed",                        /*"DoctorSeed",  */                 /*"MedicalAppointmentSeed",*/       "TeacherDedicationSeed",
                "TeacherSeed",                      "CourseSeed",                       "AcademicYearCourseSeed",       "AcademicYearCoursePreRequisiteSeed",   "ElectiveCourseSeed",               "CourseSyllabusSeed",               "CourseUnitSeed",
                "UnitActivitySeed",                 "UnitResourceSeed",                 "CourseTermSeed",               "GroupSeed",                            "SectionSeed",                      "StudentSectionSeed",               "AcademicHistorySeed",
                "TeacherSectionSeed",               "AcademicSummarySeed",              "CampusSeed",                   "BuildingSeed",                         "ClassroomTypeSeed",                "ClassroomSeed",                    "ClassScheduleSeed",
                "ClassSeed",                        "EvaluationSeed",                   "GradeSeed",                    "GradeCorrectionSeed",                  "PostulantSeed",                    "AdmissionResultSeed",              "SimulationExamSeed",
                "SimulationExamSubjectAreaSeed",    "ClassStudentSeed",                 "ClassRescheduleSeed",          "TutorialSeed",                         "TeacherScheduleSeed",              "TutorialStudentSeed",              "ScaleSectionSeed",
                "ScaleResolutionTypeSeed",          "ScaleSectionResolutionTypeSeed",   "InternalProcedureSeed",        "InternalProcedureFileSeed",            "InternalProcedureReferenceSeed",   "ExternalProcedureSeed",            "UserExternalProcedureSeed",
                "UserInternalProcedureSeed",        "ProcedureCategorySeed",            "ProcedureSubcategorySeed",     "ProcedureSeed",                        "ProcedureResolutionSeed",          "ProcedureRoleSeed",                "ProcedureRequirementSeed",
                "ProcedureDependencySeed",          "UserProcedureSeed",                "WorkerManagementPositionSeed", /*"EnrollmentReservationSeed",*/            "LanguageCourseSeed",               "LanguageLevelSeed",                "LanguageSectionSeed",
                "LanguageQualificationSeed",        "LanguageSectionScheduleSeed",
                // ECONOMIC MANAGEMENT
                "SupplierCategorySeed",
                "SupplierSeed",                     "OrderSeed",                        /*"RequirementSeed",              "RequirementSupplierSeed",*/
                // PAYROLL
                "BankSeed",                         "PaymentMethodSeed",                "PayrollTypeSeed",              "PayrollClassSeed",                     "WorkAreaSeed",                     "WorkerOcupationSeed",              "WorkerHistorySeed",                
                // TUTORING
                "TutorSeed",                        "TutoringStudentSeed",              "TutoringCoordinatorSeed",      "TutorTutoringStudentSeed",             "TutoringAnnouncementSeed",         "SupportOfficeSeed",                "SupportOfficeUserSeed",
                // JOB EXCHANGE            
                "SectorSeed",
                 //"CompanySeed",
                "AbilitySeed",                      "LanguageSeed",                         /*"JobOfferSeed",*/
                // PREUNIVERSITARY
                "PreuniversitaryTermSeed",          "PreuniversitaryCourseSeed",        "PreuniversitaryGroupSeed",     "PreuniversitaryScheduleSeed",          "PreuniversitaryUserGroupSeed",
                // ?
                "SupplierSeed",                     /*"RequirementSeed"*/
                //RESOLUTIVE ACTS
                "SorterSeed",                       "ResolutionCategorySeed"
            };
        }
        public static class ACADEMIC_ORDER
        {
            public const int NONE = 1;
            public const int UPPER_THIRD = 2;
            public const int UPPER_FIFTH = 3;
            public const int UPPER_TENTH = 4;
            public const int UPPER_HALF = 5;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { NONE, "-" },
                { UPPER_THIRD, "Tercio Superior" },
                { UPPER_FIFTH, "Quinto Superior" },
                { UPPER_TENTH, "Décimo Superior" },
                { UPPER_HALF, "Medio Superior" }
            };
            public static Dictionary<int, string> SHORT_VALUES = new Dictionary<int, string>()
            {
                { NONE, "-" },
                { UPPER_THIRD, "Tercio" },
                { UPPER_FIFTH, "Quinto" },
                { UPPER_TENTH, "Décimo" },
                { UPPER_HALF, "M. Superior" }
            };
        }
        public static class ASSISTANCE_STATES
        {
            public const int ABSENCE = 0;
            public const int ASSISTED = 1;
            //public const int LATE = 2;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { ABSENCE, "Inasistencias" },
                { ASSISTED, "Asistencias" }
            };
            public static Dictionary<int, bool> INVERSE_VALUES = new Dictionary<int, bool>()
            {
                { ABSENCE, true },
                { ASSISTED, false }
            };
        }

        public static class DEFERRED_EXAM_STATUS
        {
            public const byte PENDING = 1;
            public const byte QUALIFIED = 2;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
            {
                { PENDING, "Pendiente" },
                { QUALIFIED, "Calificado" },
            };
        }

        public static class CORRECTION_EXAM_STUDENT_STATUS
        {
            public const byte PENDING = 1;
            public const byte QUALIFIED = 2;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
            {
                { PENDING, "Pendiente" },
                { QUALIFIED, "Calificado" },
            };
        }

        public static class SUBSTITUTE_EXAM_STATUS
        {
            public const byte REGISTERED = 0;
            public const byte EVALUATED = 1;
            public const byte DELETED = 2;
            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
            {
                { REGISTERED, "Registrado" },
                { EVALUATED, "Evaluado" },
                { DELETED, "Eliminado" }
            };
        }

        public static class GRADE_RECOVERY_EXAM_MODALITY
        {
            public const byte HIGHEST_GRADE = 1;
            public const byte DIRECT_REPLACEMENT = 2;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
            {
                { HIGHEST_GRADE, "La nota obtenida debe ser mayor a la de la evaluación" },
                { DIRECT_REPLACEMENT, "Reemplaza directamente la nota" },
            };
        }

        public static class SUBSTITUTE_EXAM_EVALUATION_TYPE
        {
            public const byte DIRECTED_FINAL_GRADE = 1;
            public const byte AVERAGE_WITH_PREVIOUS_GRADE = 2;
            public const byte GRADE_BY_FACTOR = 3;
            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
            {
                { DIRECTED_FINAL_GRADE, "Nota final directa" },
                { AVERAGE_WITH_PREVIOUS_GRADE, "Promedio con nota final anterior" },
                { GRADE_BY_FACTOR, "Nota por factor" }
            };
        }
        public static class SUBSTITUTE_EXAM_EVALUATION_FACTOR
        {
            public static Dictionary<int, double> FACTORS = new Dictionary<int, double>
            {
                { 1, 1.000 },
                { 2, 1.000 },
                { 3, 1.000 },
                { 4, 1.000 },
                { 5, 1.000 },
                { 6, 1.000 },
                { 7, 1.000 },
                { 8, 1.000 },
                { 9, 1.000 },
                { 10, 1.000 },
                { 11, 1.000 },
                { 12, 1.000 },
                { 13, 1.000 },
                { 14, 0.9285 },
                { 15, 0.8666 },
                { 16, 0.8750 },
                { 17, 0.8235 },
                { 18, 0.7777 },
                { 19, 0.7368 },
                { 20, 0.7000 },
            };
        }
        public static class INVESTIGATION_PROJECT_MODALITY
        {
            public const byte SOCIAL_PROYECTION = 0;
            public const byte UNIVERSITY_EXTENSION = 1;
            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { SOCIAL_PROYECTION, "Proyección Social" },
                { UNIVERSITY_EXTENSION, "Extensión Universitaria" }
            };
        }
        public static class INVESTIGATION_PROJECT_STATUS
        {
            public const byte APPROVED = 0;
            public const byte DISAPPROVED = 1;
            public const byte IN_PROGRESS = 2;
            public const byte COMPLETED = 3;
            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { APPROVED, "Aprobado" },
                { DISAPPROVED, "Desaprobado" },
                { IN_PROGRESS, "En proceso" },
                { COMPLETED, "Finalizado" }
            };
        }
        public static class CIVIL_STATUS
        {
            public const byte SINGLE = 0;
            public const byte MARRIED = 1;
            public const byte WIDOWER = 2;
            public const byte COHABITED = 3;
            public const byte DIVORCED = 4;
            public const byte SEPARATED = 5;
            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { SINGLE, "Soltero(a)" },
                { MARRIED, "Casado(a)" },
                { WIDOWER, "Viudo(a)" },
                { COHABITED, "Conviviente(a)" },
                { DIVORCED, "Divorciado(a)" },
                { SEPARATED, "Separado(a)" }
            };
        }
        public static class COMMUNICATION
        {
            public static class NOTIFICATION
            {
                public static class SERVER_SIDE
                {
                    public static class REQUEST_PARAMETERS
                    {
                        public const string PAGE = "page";
                        public const string RECORDS = "records";
                    }
                }
            }
        }
        public static class FileStorage
        {
            public static class Mode
            {
                public const int BLOB_STORAGE_MODE = 1;
                public const int SERVER_STORAGE_MODE = 2;
                public const int HUAWEI_STORAGE_MODE = 3;
                public const int AMAZONS3_STORAGE_MODE = 4;
            }
            public static class SystemFolder
            {
                public const string INTRANET = "intranet";
                public const string ENROLLMENT = "enrollment";
                public const string SCALE = "scale";
                public const string DOCUMENTARY_PROCEDURE = "documentaryprocedure";
                public const string INDICATORS = "indicators";
                public const string INVESTIGATION = "investigation";
                public const string ADMISSION = "admission";
                public const string EVALUATION = "evaluation";
                public const string CONTINUINGEDUCATION = "continuingeducation";
                public const string ACADEMIC_EXCHANGE = "academicexchange";
                public const string INTEREST_GROUP = "interestgroup";
                public const string TEACHING_MANAGEMENT = "teachingmanagement";
                public const string LAURASSIA = "laurassia";
                public const string TRANSPARENCY_PORTAL = "transparencyportal";
                public const string LANGUAGE_CENTER = "languagecenter";
                public const string KARDEX = "kardex";
                public const string RESERVATIONS = "reservations";
                public const string JOBEXCHANGE = "jobexchange";
                public const string TUTORING = "tutoring";
                public const string PREUNIVERSITARY = "preuniversitary";
                public const string RESOLUTIVE_ACTS = "resolutiveacts";
                public const string SISCO = "sisco";
                public const string DEGREE = "degree";
                public const string POSDEGREE = "posdegree";
                public const string ECONOMICMANAGEMENT = "economicmanagement";
                public const string COMPUTERMANAGEMENT = "computermanagement";
                public const string CHAT = "chat";
                public const string INSTITUTIONALWELFARE = "institutionalwelfare";
                public const string TEACHER_HIRING = "teacherhiring";
                public const string TEACHING_RESEARCH = "teachingresearch";
                public const string PAYROLL = "payroll";
            }
        }
        public static class CONTAINER_NAMES
        {
            public const string GENERAL_INFORMATION = "generalinformation";
            public const string ACADEMICHISTORYDOCUMENT = "academichistorydocument";
            public const string ADMISSIONTYPE = "admissiontypepdf";
            public const string ADMISSIONTERMFILES = "admissiontermfiles";
            public const string ADM_REQUIREMENT_DOCUMENT = "admissionrequirementdocument";
            public const string ACADEMICRECORDS = "academicrecords";
            public const string ANNOUNCEMENT_FILES = "announcementpicture";
            public const string DEPENDENCY_SIGNATURE = "dependencysignature";
            public const string EVENT_PICTURE = "eventpicture";
            public const string EVENT_FILES = "eventfile";
            public const string EXTERNALLINK_PICTURE = "externallinkpicture";
            public const string AGREEMENTFORMAT_FILES = "agreementformatfiles";
            public const string AGREEMENTTEMPLATE_FILES = "agreementtemplatefiles";
            public const string FORUM_FILES = "forumpicture";
            public const string INTERNAL_PROCEDURE_DOCUMENT = "internalproceduredocument";
            public const string NONACTIVITIES_FILES = "nonactivitiesfile";
            public const string POSITIONS_FILES = "positionsfile";
            public const string RESOLUTIONS = "resolutions";
            public const string APROBERESOLUTIONS = "aproberesolutions";
            public const string STUDENT_ABSENCE_JUSTIFICATION_FILE = "studentabsencejustificationfile";
            public const string STUDENTCERTIFICATE_FILE = "studentcertificatefile";
            public const string STUDENTABILITYLANGUAGE_FILE = "studentabilitylanguagefile";
            public const string TEMARY = "temarypdf";
            public const string NON_TEACHING_LOAD_DELIVERABLE_FILES = "nonteachingloaddeliverablefiles";
            public const string TEACHER_NONACTIVITY_HISTORIAL = "teachernonactivityhistorial";
            public const string USER_ABSENCE_JUSTIFICATION_FILE = "userabsencejustificationfile";
            public const string USER_PROFILE_PICTURE = "userprofilepicture";
            public const string USER_PROCEDURE_DERIVATION_DOCUMENT = "userprocedurederivationdocument";
            public const string USER_INTERNAL_PROCEDURE_DERIVATION_DOCUMENT = "userinternalprocedurederivationdocument";
            public const string USER_PROCEDURE_FILE = "userprocedurefile";
            public const string USER_PERSONAL_DOCUMENTS = "scaleworkerpersonaldocuments";
            public const string USER_PROCEDURE_RECORD_DOCUMENTS = "userprocedurerecorddocuments";
            public const string USER_REQUIREMENT_DEGREE = "userrequirementdegree";
            public const string WORKER_RESOLUTIONS = "scaleworkerresolutions";
            public const string WORKER_RESOLUTIONS_ANNEXES = "scaleworkerresolutionsannexes";
            public const string WORKER_VACATIONS_LICENSES = "scaleworkervacationslicenses";
            public const string POSTULANT_REQUIREMENTS = "postulantrequirements";
            public const string POSTULANT_PHOTOS = "postulantphotos";
            public const string POSTULANT_FINGERPRINTS = "postulantfingerprints";
            public const string CONTINUING_EDUCATION = "continuingeducation";
            public const string RESERVATION_ENVIRONMENT = "reservationenvironment";
            public const string INVESTIGATION = "investigation";
            public const string EVALUATION = "evaluation";
            public const string RESEARCH = "research";
            public const string INSTITUTIONAL_AGENDA = "agendainstitucional";
            public const string SUBACTIVITY = "subactivity";
            public const string CURRICULUM = "curriculumvitaes";
            public const string DISABILITYCERTIFICATE = "disabilitycertificate";
            public const string COMPANY = "companiesn";
            public const string COMPANY_IMAGES = "companiesimages";
            public const string AGREEMENT_JOB_EXCHANGE = "agreementdocuments";
            public const string FINANCIAL_EXECUTIONS = "financialexecutions";
            public const string INSTITUTIONAL_ACTIVITIES = "institutionalactivities";
            public const string PUBLIC_INFORMATION = "publicinformation";
            public const string RESEARCH_PROJECTS = "researchprojects";
            public const string SESSION_RECORD = "sessionrecord";
            public const string GRADE_REPORT = "gradereportfiles";
            public const string REGISTRY_PATTERN = "registrypatternfiles";
            public const string FINANCIAL_STATEMENT = "financialstatement";
            public const string RESOLUTIVE_ACTS = "resolutiveacts";
            public const string INSTITUTIONAL_INFORMATION = "institutionalinformation";
            public const string HELP_DESK_FILES = "helpdeskfiles";
            public const string ADMIN_ENROLLMENT = "adminenrollment";
            public const string TRANSPARENCY_PORTAL_REGULATIONS = "transparencyportalregulations";
            public const string TRANSPARENCY_PORTAL_MANAGEMENT_DOCUMENTS = "transparencyportalmanagementdocuments";
            public const string TRANSPARENCY_PORTAL_GENERAL_FILES = "transparencyportalgeneralfiles";
            public const string TRANSPARENCY_PORTAL_SCHOLARSHIP = "transparencyportalscholarship";
            public const string ORDER_CHANGED_FILES = "orderchangedfiles";
            public const string ENROLLMENT_RESERVATION = "enrollmentreservation";
            public const string ENROLLMENT_TURN = "enrollmentturn";
            public const string LIVING_COST = "livingcostpdf";
            public const string DIGITIZED_SIGNATURES = "digitizedsignatures";
            public const string DIDACTICAL_MATERIAL = "didacticalmaterial";
            public const string JOB_OFFERS = "jobofferspdf";
            public const string GRADE_CORRECTION = "gradecorrection";
            public const string SUBSTITUTE_EXAMEN_CORRECTION = "substituteexamencorrection";
            public const string GRADE_RECTIFICATION = "graderectification";
            public const string INSTITUTIONAL_WELFARE_SCHOLARSHIP_FORMAT = "institutionalwelfarescholarshipformat";
            public const string INSTITUTIONAL_WELFARE_SCHOLARSHIP_STUDENT = "institutionalwelfarescholarshipstudent";
            public const string INSTITUTIONAL_STRATEGIC_PLAN = "institutionalstrategicplan";

            //cafobe
            public const string CAFOBE_HIGHPERFORMANCE = "cafobehighperformance";
            public const string CAFOBE_MATERNITY = "cafobematernity";
            public const string CAFOBE_OPHTHALMOLOGICAL = "cafobeophthalmological";
            public const string CAFOBE_FAMILYDEATH = "cafobefamilydeath";
            public const string CAFOBE_HEALTH = "cafobehealth";
            public const string CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN = "cafobestimulusscholarshiporoutstandingsportsman";

            public const string CAFOBE_REQUESTDETAIL = "cafoberequestdetail";

            public const string INSTITUTIONAL_WELFARE_CONVOCATION_FORMATS = "institutionalwelfaresconvocationformats";
            public const string INSTITUTIONAL_WELFARE_CONVOCATION_POSTULANT_FILES = "institutionalwelfaresconvocationpostulantfiles";
            public const string GENERAL_PORTAL_NEWS = "generalportalnews";
            public const string DIGITAL_SIGNATURE = "digitalsignature";
            //Scale
            public const string SCALE_REPORT_HISTORIES = "scalereporthistories";
            //intranet 
            public const string INTRANET_FILES = "intranetfiles";
            public const string STUDENT_OBSERVATION = "studentobservations";
            public const string STUDENT_PORTFOLIO = "studentportfolio";
            public const string CORRECTION_EXAM = "correctionexam";
            public const string RECORD_HISTORIES_FILES = "recordhistoriesfiles";
            public const string EXTRACURRICULAR_ACTIVITIES_STUDENTS = "extracurricularactivities";

            //Portal
            public const string COMPETITION_FILES = "competitionfiles";
            //sisco
            public const string NORM = "norm";
            public const string AGREEMENT_PUBLICATION_FILES = "agreementpublicationfiles";
            public const string INCUBATION_CALL_FILES = "incubationcallfiles";
            public const string TUTOR_WORKING_PLANS = "tutorworkingplans";
            public const string TUTORING_PLANS = "tutoringplans";
            public const string TUTORING_PROBLEMS = "tutoringproblems";
            public const string INTEREST_GROUPS_FILES = "interestgroups";
            public const string SISCO = "sisco";
            //EnrollmentManagement
            public const string PURCHASE_ORDER_DOCUMENTS = "purchaseorderdocuments";
            //EconomicManagement
            public const string TREASURY_DOCUMENTS = "treasurydocuments";
            public const string USER_REQUIREMENT_DOCUMENTS = "userrequirementdocuments";
            public const string EXONERATED_PAYMENTS = "exoneratedpayments";
            //AcademicExchange
            public const string ACADEMIC_EXCHANGE_FILES = "academicexchangefiles";
            public const string SCHOLARSHIPS_FILES = "scholarshipsfiles";
            public const string SCHOLARSHIP_GALLERY = "scholarshipgalleryfiles";
            //internshipRequests
            public const string INTERNSHIPREQUESTS_FILES = "internshiprequests";
            public const string PICTURE_GROUP = "picturegroup";
            public const string FILE_INDIVIDUAL = "fileindividual";
            //computermanagement
            public const string COMPUTER_CONDITION_FILES = "computerconditionfiles";
            //degree
            public const string GRADE_REPORT_REQUIREMENTS = "gradereportrequirements";
            //posdegre
            //CHAT
            public const string FILE_CHAT = "filechat";
            //TeachingManagement
            public const string INFORMS = "informs";
            public const string TEACHER_PORTFOLIO = "teacherportfolio";

            //tutoring
            public const string TUTORING_ANNOUNCEMENTS = "tutoringannouncements";
            //institutional_welfare
            public const string SISFOH_DOCUMENTS = "sisfohdocuments";
            public const string CAFOBEREQUEST_RESOLUTIONS = "cafoberequestresolutions";
            //Teacher Hiring
            public const string CONVOCATION_DOCUMENTS = "convocationdocuments";
            //Teacher Research
            public const string TEACHING_RESEARCH_CONVOCATION_FILES = "teachingresearchconvocationfiles";

            public const string STUDENT_PRESENTATION_LETTER_FILES = "studentpresentationletterfiles";

            //Server
            public const string CAMPUS_IMAGES = "campusimages";

            //ComplaintBook
            public const string COMPLAINT_FILES = "complaintfiles";

            //payroll

            public const string PAYROLL_USER_PICTURE = "payrolluserpicture";
        }
        public static class COLORS
        {
            public static class BADGE
            {
                public static class STATE
                {
                    public const int NONE = 0;
                    public const int SUCCESS = 1;
                    public const int WARNING = 2;
                    public const int DANGER = 3;
                    public const int INFO = 4;
                    public const int PRIMARY = 5;
                    public const int SECONDARY = 6;
                    public const int BRAND = 7;
                    public const int ACCENT = 8;
                    public const int FOCUS = 9;
                    public const int METAL = 10;
                    public const int LIGHT = 11;
                    public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "NONE", NONE },
                    { "SUCCESS", SUCCESS },
                    { "WARNING", WARNING },
                    { "DANGER", DANGER },
                    { "INFO", INFO },
                    { "PRIMARY", PRIMARY },
                    { "SECONDARY", SECONDARY },
                    { "BRAND", BRAND },
                    { "ACCENT", ACCENT },
                    { "FOCUS", FOCUS },
                    { "METAL", METAL },
                    { "LIGHT", LIGHT }
                };
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NONE, "" },
                    { SUCCESS, "m-badge--success" },
                    { WARNING, "m-badge--warning" },
                    { DANGER, "m-badge--danger" },
                    { INFO, "m-badge--info" },
                    { PRIMARY, "m-badge--primary" },
                    { SECONDARY, "m-badge--secondary" },
                    { BRAND, "m-badge--brand" },
                    { ACCENT, "m-badge--accent" },
                    { FOCUS, "m-badge--focus" },
                    { METAL, "m-badge--metal" },
                    { LIGHT, "m-badge--light" }
                };
                }
            }
        }
        public static class DATATABLE
        {
            public static class SERVER_SIDE
            {
                public static class DEFAULT
                {
                    public const string ORDER_DIRECTION = "DESC";
                }
                public static class SENT_PARAMETERS
                {
                    public const string DRAW_COUNTER = "draw";
                    public const string PAGING_FIRST_RECORD = "start";
                    public const string RECORDS_PER_DRAW = "length";
                    public const string SEARCH_VALUE = "search[value]";
                    public const string SEARCH_REGEX = "search[regex]";
                    public const string ORDER_COLUMN = "order[0][column]";
                    public const string ORDER_DIRECTION = "order[0][dir]";
                }
            }
        }
        public static class DOCUMENTS
        {
            public static class FILE_EXTENSION_GROUP
            {
                public const string DOCUMENTS = "doc,docx,pdf";
                public const string IMAGES = "bmp,gif,jpeg,jpg,png,svg,jfif";
                public const string PDF = "pdf";
                public const string PDF_IMAGES = "bmp,gif,jpeg,jpg,pdf,png,svg";
                public const string WORD = "doc,docx";
                public const string PDF_IMG_DOCUMENTS = "doc,docx,pdf,jpeg,jpg,png,xlsx";
            }
            public static class FILE_SIZE_BYTES
            {
                public static class APPLICATION
                {
                    public const double GENERIC = 20 * 1024 * 1024;
                }
                public static class AUDIO
                {
                    public const double GENERIC = 20 * 1024 * 1024;
                }
                public static class IMAGE
                {
                    public const double GENERIC = 20 * 1024 * 1024;
                }
                public static class TEXT
                {
                    public const double GENERIC = 20 * 1024 * 1024;
                }
                public static class VIDEO
                {
                    public const double GENERIC = 20 * 1024 * 1024;
                }
            }
            public static class MIME_TYPE
            {
                public static class APPLICATION
                {
                    public const string GENERIC = "application/*";
                    public const string PDF = "application/pdf";
                    public const string DOC = "application/msword";
                    public const string DOCX = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    public const string XLS = "application/vnd.ms-excel";
                    public const string XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                }
                public static class AUDIO
                {
                    public const string GENERIC = "audio/*";
                }
                public static class IMAGE
                {
                    public const string GENERIC = "image/*";
                }
                public static class TEXT
                {
                    public const string GENERIC = "text/*";
                }
                public static class VIDEO
                {
                    public const string GENERIC = "video/*";
                }
            }
        }
        public static class CLASS_STATES
        {
            public const int WAITING = 0;
            public const int TAKEN = 1;
            public const int NOT_TAKEN = 2;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { WAITING, "En espera" },
                { TAKEN, "Tomado" },
                { NOT_TAKEN, "No tomado" }
            };
        }

        public static class USER_TYPES
        {
            public const int NOT_ASIGNED = 0;
            public const int STUDENT = 1;
            public const int TEACHER = 2;
            public const int ADMINISTRATIVE = 3;
            public const int COMPANY = 4;
            public const int EXTERNAL = 5;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { NOT_ASIGNED, "No Asignado" },
                { STUDENT, "Estudiante" },
                { TEACHER, "Docente" },
                { ADMINISTRATIVE, "Administrativo" },
                { COMPANY, "Empresa" },
                { EXTERNAL, "Usuario Externo" },
            };
        }
        public static class USER_STATES //Activo - activoTemporal - Inactivo para ApplicationUser
        {
            public const int ACTIVE = 1;
            public const int TEMPORARY = 2;
            public const int INACTIVE = 3;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { ACTIVE, "Activo" },
                { TEMPORARY, "Activo-Temporal" },
                { INACTIVE, "Inactivo" },
            };
        }
        public static class DOCUMENT_TYPES
        {
            public const byte BIRTH_CERTIFICATE = 1;
            public const byte DNI = 2;
            public const byte FOREIGN_RESIDENT_IDENTIFICATION_CARD = 3;
            public const byte IDENTITY_CARD = 4;
            public const byte MILITARY_CARD = 5;
            public const byte MILITARY_BALLOT = 6;
            public const byte PASSPORT = 7;
            public const byte PROVISIONAL_DOCUMENT = 8;
            public const byte VOTER_REGISTRATION_BOOK = 9;
            public const byte RUC = 10;
            public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
            {
                { "BIRTH_CERTIFICATE", BIRTH_CERTIFICATE },
                { "DNI", DNI },
                { "FOREIGN_RESIDENT_IDENTIFICATION_CARD", FOREIGN_RESIDENT_IDENTIFICATION_CARD },
                { "IDENTITY_CARD", IDENTITY_CARD },
                { "MILITARY_CARD", MILITARY_CARD },
                { "MILITARY_BALLOT", MILITARY_BALLOT },
                { "PASSPORT", PASSPORT },
                { "PROVISIONAL_DOCUMENT", PROVISIONAL_DOCUMENT },
                { "VOTER_REGISTRATION_BOOK", VOTER_REGISTRATION_BOOK },
                { "RUC", RUC },
            };
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { BIRTH_CERTIFICATE, "Partida de Nacimiento" },
                { DNI, "DNI" },
                { FOREIGN_RESIDENT_IDENTIFICATION_CARD, "CE-Carné de Extranjería" },
                { IDENTITY_CARD, "Carné de Identidad" },
                { MILITARY_CARD, "Libreta Militar" },
                { MILITARY_BALLOT, "Boleta Militar" },
                { PASSPORT, "Pasaporte" },
                { PROVISIONAL_DOCUMENT, "Documento Provisional" },
                { VOTER_REGISTRATION_BOOK, "Libreta Electoral" },
                { RUC, "RUC" },
            };
        }
        public static class ENTITY_ENTRIES
        {
            public static class PROPERTY_NAME
            {
                public const string CODE_NUMBER = "CodeNumber";
                public const string CODE_TEXT = "CodeText";
                public const string CREATED_AT = "CreatedAt";
                public const string DELETED_AT = "DeletedAt";
                public const string UPDATED_AT = "UpdatedAt";
                public const string CREATED_BY = "CreatedBy";
                public const string DELETED_BY = "DeletedBy";
                public const string UPDATED_BY = "UpdatedBy";
            }
        }
        public static class ENTITY_FRAMEWORK
        {
            public const int CHANGES_LIMIT = 100;
            public const int RECORD_LIMIT = 1000;
        }
        public static class ENTITY_MODELS
        {
            public static class DBO
            {
                public const string ASPNETUSERS = "dbo.AspNetUsers";
            }
            public static class GENERALS
            {
                public const string STUDENTS = "Generals.Students";
            }
            public static class INTRANET
            {
                public const string ACADEMIC_HISTORY = "Intranet.AcademicHistories";
                public const string ACADEMIC_SUMMARY = "Intranet.AcademicSummaries";
                public const string SURVEY_USER = "Intranet.SurveyUsers";
                public static class ACADEMIC_SUMMARIES
                {
                    public const string MERIT_ORDER = "MeritOrder";
                }
            }
            public static class TeachingManagement
            {
                public const string TEACHER_SURVEY = "TeachingManagement.TeacherSurveys";
            }
        }
        public static class FORMATS
        {
            public const string DATE = "dd/MM/yyyy";
            public const string DURATION = "{0}h {1}m";
            public const string TIME = "h:mm tt";
            public const string DATETIME = "dd/MM/yyyy h:mm tt";
            public const string DATETIME_CUSTOM = "dd/MM/yyyy HH:mm";
        }

        public static class GRADERECTIFICATION
        {
            public static class TYPE
            {
                public const int GRADECORRECTION = 1;
                public const int SUBSTITUTORY = 2;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "GRADECORRECTION", GRADECORRECTION },
                    { "SUBSTITUTORY", SUBSTITUTORY }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { GRADECORRECTION, "Corrección de Notas" },
                    { SUBSTITUTORY, "Sustitutorio" },
                };
            }
            public static class STATE
            {
                public const int CREATED = 1;
                public const int UPDATEDBYTEACHER = 2;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "CREATED", CREATED },
                    { "UPDATEDBYTEACHER", UPDATEDBYTEACHER }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { CREATED, "Creado" },
                    { UPDATEDBYTEACHER, "Modificado por el Docente" },
                };
            }
        }


        public static class INTERNAL_PROCEDURES
        {
            public static class PRIORITY
            {
                public const int LOW = 1;
                public const int MEDIUM = 2;
                public const int HIGH = 3;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "LOW", LOW },
                    { "MEDIUM", MEDIUM },
                    { "HIGH", HIGH }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { LOW, "Normal" },
                    { MEDIUM, "Medio" },
                    { HIGH, "Alto" }
                };
            }
            public static class STATIC_TYPE
            {
                public const int COMPLAINTS_BOOK = 1;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "COMPLAINTS_BOOK", COMPLAINTS_BOOK }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { COMPLAINTS_BOOK, "Libro de Reclamaciones" },
                };
            }
        }

        public static class COMPLAINT_BOOK
        {
            public static class HIRED_TYPE
            {
                public const byte PRODUCT = 1;
                public const byte SERVICE = 2;

                public static Dictionary<byte, string> INDICES = new Dictionary<byte, string>()
                {
                    { PRODUCT, "Producto" },
                    { SERVICE, "Servicio"}
                };
            }

            public static class STATUS
            {
                public const byte PENDING = 1;
                public const byte ACCEPTED = 2;
                public const byte DENIED = 3;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { PENDING, "Pendiente" },
                    { ACCEPTED, "Aceptado"},
                    { DENIED, "Denegado"}
                };
            }
        }

        public static class HUBS
        {
            public static class AKDEMIC
            {
                public static class CLIENT_PROXY
                {
                    public static class METHODS
                    {
                        public static class CHAT
                        {
                            public const string NEWMESSAGE = "akdemic-chat-new-message";
                            public const string SUCCESSENDMESSAGE = "akdemic-chat-success-send-message";
                        }
                        public static class NOTIFICATION
                        {
                            public const string RECIEVE = "RecieveNotification";
                        }
                    }
                    public const string METHOD = "akdemic-notification";
                    public const string URL = "/hubs/akdemic";
                }
                public static class DATABASE
                {
                    public static class TABLE
                    {
                        public const string NOTIFICATION = "Generals.Notifications";
                        public const string USER_NOTIFICATION = "Generals.UserNotifications";
                    }
                }
            }
        }
        public static class MESSAGES
        {
            public static class ERROR
            {
                public const string MESSAGE = "Ocurrió un problema al procesar su consulta";
                public const string TITLE = "Error";
            }
            public static class INFO
            {
                public const string MESSAGE = "Mensaje informativo";
                public const string TITLE = "Info";
            }
            public static class SUCCESS
            {
                public const string MESSAGE = "Tarea realizada satisfactoriamente";
                public const string TITLE = "Éxito";
            }
            public static class VALIDATION
            {
                public const string COMPARE = "El campo '{0}' no coincide con '{1}'";
                public const string EMAIL_ADDRESS = "El campo '{0}' no es un correo electrónico válido";
                public const string RANGE = "El campo '{0}' debe tener un valor entre {1}-{2}";
                public const string MINIMUM_VALUE = "El campo '{0}' debe tener un valor mínimo de {1}";
                public const string REGULAR_EXPRESSION = "El campo '{0}' no es válido";
                public const string REQUIRED = "El campo '{0}' es obligatorio";
                public const string STRING_LENGTH = "El campo '{0}' debe tener {1}-{2} caracteres";
                public const string NOT_VALID = "El campo '{0}' no es válido'";
                public const string FILE_EXTENSIONS = "El campo '{0}' solo acepta archivos con los formatos: {1}";
            }
            public static class VALIDATION_LANGUAGES
            {
                public const string COMPARE = "ValidationMessageCompare";
                public const string EMAIL_ADDRESS = "ValidationMessageEmail";
                public const string RANGE = "ValidationMessageRange";
                public const string REGULAR_EXPRESSION = "ValidationMessageRegularExpression";
                public const string REQUIRED = "ValidationMessageRequired";
                public const string STRING_LENGTH = "ValidationMessageStringLength";
                public const string URL = "ValidationMessageStringUrl";
                public const string STRING_UNIQUE_LENGTH = "ValidationMessageStringUniqueLength";
                public const string NOT_VALID = "ValidationMessageNotValid";
                public const string FILE_EXTENSIONS = "ValidationMessageFileExtensions";
                public const string POSITIVE_INTEGER = "ValidationMessagePositiveInteger";
                public const string POSITIVE_INTEGER_OR_ZERO = "ValidationMessagePositiveIntegerOrZero";
            }
            public static class WARNING
            {
                public const string MESSAGE = "Mensaje de advertencia";
                public const string TITLE = "Advertencia";
            }
        }
        public static class MONTHS
        {
            public const int JANUARY = 1;
            public const int FEBRAURY = 2;
            public const int MARCH = 3;
            public const int APRIL = 4;
            public const int MAY = 5;
            public const int JUNE = 6;
            public const int JULY = 7;
            public const int AUGUST = 8;
            public const int SEPTEMBER = 9;
            public const int OCTOBER = 10;
            public const int NOVEMBER = 11;
            public const int DECEMBER = 12;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { JANUARY, "Enero" },
                { FEBRAURY, "Febrero" },
                { MARCH, "Marzo" },
                { APRIL, "Abril" },
                { MAY, "Mayo" },
                { JUNE, "Junio" },
                { JULY, "Julio" },
                { AUGUST, "Agosto" },
                { SEPTEMBER, "Setiembre" },
                { OCTOBER, "Octubre" },
                { NOVEMBER, "Noviembre" },
                { DECEMBER, "Diciembre" }
            };
        }
        public static class NOTIFICATIONS
        {
            public const int DEFAULT_PAGE_SIZE = 10;
            public static class COLORS
            {
                public const int NONE = 0;
                public const int SUCCESS = 1;
                public const int WARNING = 2;
                public const int DANGER = 3;
                public const int INFO = 4;
                public const int PRIMARY = 5;
                public const int SECONDARY = 6;
                public const int BRAND = 7;
                public const int ACCENT = 8;
                public const int FOCUS = 9;
                public const int METAL = 10;
                public static Dictionary<int, string> LABELS = new Dictionary<int, string>()
                {
                    { NONE, "" },
                    { SUCCESS, "m-badge--success" },
                    { WARNING, "m-badge--warning" },
                    { DANGER, "m-badge--danger" },
                    { INFO, "m-badge--info" },
                    { PRIMARY, "m-badge--primary" },
                    { SECONDARY, "m-badge--secondary" },
                    { BRAND, "m-badge--brand" },
                    { ACCENT, "m-badge--accent" },
                    { FOCUS, "m-badge--focus" },
                    { METAL, "m-badge--metal" },
                };
            }
        }
        public static class ORDERS
        {
            public static class STATUS
            {
                public const int NOTHING = 0;
                public const int RUNNING = 1;
                public const int FINALIZED = 2;
                public const int PENDING = 3;
                public const int INTERNSHIP = 4;
                public const int ATTENDED = 5;
                public const int REJECTED = 6;
                public const int CUSTODY = 7;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "NOTHING", NOTHING },
                    { "RUNNING", RUNNING },
                    { "FINALIZED", FINALIZED },
                    { "PENDING", PENDING },
                    { "INTERNSHIP", INTERNSHIP },
                    { "ATTENDED", ATTENDED },
                    { "REJECTED", REJECTED },
                    { "CUSTODY", CUSTODY }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NOTHING, "--" },
                    { RUNNING, "En Tránsito" },
                    { FINALIZED, "Finalizado" },
                    { PENDING, "Pendiente" },
                    { INTERNSHIP, "Internado" },
                    { ATTENDED, "Atendido" },
                    { REJECTED, "Rechazado" },
                    { CUSTODY, "Custodia" }
                };
            }
            public static class TYPE
            {
                public const bool PURCHASE = false;
                public const bool SERVICE = true;
                public static Dictionary<bool, string> VALUES = new Dictionary<bool, string>()
                {
                    { PURCHASE, "Orden de compra" },
                    { SERVICE, "Orden de servicio" }
                };
            }
            public static class DELIVERY
            {
                public const bool YES = true;
                public const bool NO = false;
                public static Dictionary<bool, string> VALUES = new Dictionary<bool, string>()
                {
                    { YES, "Entregado" },
                    { NO, "Por entregar" }
                };
            }
            public static class FUNDING_SOURCE
            {
                public const int ORDINARY_SOURCES = 1;
                public const int DIRECTLY_COLLECTED_SOURCES = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { ORDINARY_SOURCES, "Recursos ordinarios" },
                    { DIRECTLY_COLLECTED_SOURCES, "Recursos directamente recaudado" }
                };
            }
        }

        public static class PROCEDURE_TASK
        {
            public static class TYPE
            {
                public const byte DESCRIPTION = 1;
                public const byte RECORD_HISTORY = 2;
                public const byte ACTIVITY = 3;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { DESCRIPTION, "Descripción" },
                    { RECORD_HISTORY, "Documento Académico" },
                    { ACTIVITY, "Actividad" }
                };

                public static Dictionary<byte, string> VALUES_V2 = new Dictionary<byte, string>()
                {
                    { RECORD_HISTORY, "Documento Académico" },
                    { ACTIVITY, "Actividad" }
                };
            }

            public static class ACTIVITY_TYPE
            {
                public const byte CAREER_TRANSFER = 1;
                public const byte ACADEMIC_YEAR_WITHDRAWAL = 2;
                public const byte COURSE_WITHDRAWAL = 3;
                public const byte RESIGNATION = 4;
                public const byte REENTRY = 5;
                public const byte REGISTRATION_RESERVATION = 6;
                public const byte CHANGE_ACADEMIC_PROGRAM = 7;
                public const byte EXONERATED_COURSE = 8;
                public const byte EXTRAORDINARY_EVALUATION = 9;
                public const byte SUBSTITUTE_EXAM = 10;
                public const byte GRADE_RECOVERY = 11;
                //public const byte DEFERRED_EXAM = 12;
                public const byte BACHELOR_REQUEST = 13;
                public const byte JOB_TITLE_REQUEST = 14;
                public const byte COURSE_WITHDRAWAL_MASSIVE = 15;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { CAREER_TRANSFER, "Traslado de Escuela" },
                    { ACADEMIC_YEAR_WITHDRAWAL, "Retiro de Ciclo" },
                    { COURSE_WITHDRAWAL, "Retiro de asignatura" },
                    { RESIGNATION, "Renuncia como estudiante" },
                    { REENTRY, "Reingreso como estudiante" },
                    { REGISTRATION_RESERVATION, "Reserva de matrícula" },
                    { CHANGE_ACADEMIC_PROGRAM, "Cambio de especialidad" },
                    { EXONERATED_COURSE, "Curso Exonerado" },
                    { EXTRAORDINARY_EVALUATION, "Evaluación Extraordinaria" },
                    { SUBSTITUTE_EXAM, "Examen Sustitutorio" },
                    { GRADE_RECOVERY, "Recuperación de Nota" },
                    //{ DEFERRED_EXAM, "Examen Aplazado" },
                    { BACHELOR_REQUEST, "Solicitud de bachiller" },
                    { JOB_TITLE_REQUEST, "Solicitud de Titulo Profesional" },
                    { COURSE_WITHDRAWAL_MASSIVE, "Retiro de Asignatura en bloque" },
                };
            }
        }

        public static class PROCEDURE_CATEGORIES
        {
            public static class STATIC_TYPE
            {
                public const int DEGREES_AND_TITLES = 1;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "DEGREES_AND_TITLES", DEGREES_AND_TITLES }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { DEGREES_AND_TITLES, "GRADOS ACADÉMICOS Y TÍTULOS PROFESIONALES" }
                };
            }
        }
        public static class PROCEDURE_SUBCATEGORIES
        {
            public static class STATIC_TYPE
            {
                public const int BACHELOR = 1;
                public const int PROFESIONAL_TITLE = 2;
                public const int MASTER = 3;
                public const int DOCTOR = 4;
                public const int SECOND_SPECIALTY_DEGREE = 5;
                public static Dictionary<int, string> INDICES = new Dictionary<int, string>()
                {
                    { BACHELOR, "B" },
                    { PROFESIONAL_TITLE, "T" },
                    { MASTER, "M" },
                    { DOCTOR, "D" },
                    { SECOND_SPECIALTY_DEGREE, "S" }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { BACHELOR, "BACHILLER" },
                    { PROFESIONAL_TITLE, "TITULO PROFESIONAL" },
                    { MASTER, "MAESTRO" },
                    { DOCTOR, "DOCTOR" },
                    { SECOND_SPECIALTY_DEGREE, "TITULO DE SEGUNDA ESPECIALIDAD PROFESIONAL" }
                };
            }
        }
        public static class PROCEDURE_RECORDS
        {
            public const int STUDY_RECORD = 1;
            public const int NON_DEBIT_OF_ASSETS_UNIVERSITY_RECORD = 2;
            public const int CERTIFICATE_OF_SUPERIOR_THIRD_TABLE_RECORD = 3;
            public const int CERTIFICATE_OF_SUPERIOR_THIRD_PROMOTION_ENTRANTS_RECORD = 4;
            public const int NON_DEBIT_OF_ASSETS_UNIVERSITY_RECORD_REQUEST = 5;
            public const int NON_DEBIT_OF_GOODS_LABORATORIES_RECORD = 6;
            public const int PROFESSIONAL_TITLE_REQUEST = 7;
            public const int NON_DEBIT_OF_BIBLIOGRAPHIC_MATERIAL_RECORD = 8;
            public const int NON_DEBIT_OF_BIBLIOGRAPHIC_MATERIAL_REQUEST = 9;
            public const int EGRESS_RECORD = 10;
            public const int CERTIFICATE_OF_SUPERIOR_FIFTH_PROMOTION_ENTRANTS_RECORD = 11;
            public const int CERTIFICATE_OF_SUPERIOR_FIFTH_TABLE_RECORD = 12;
            public const int DOCUMENT_COMPLIANCE_RECORD = 13;
            public const int ENROLLMENT_START_RECORD = 14;
            public const int CURRICULAR_ACTIVITIES_RECORD = 15;
            public const int BACHELLOR_REQUEST = 16;
            public const int CURRICULAR_ACTIVITIES_REQUEST = 17;
            public static Dictionary<int, Tuple<string, string, bool, int[]>> VALUES = new Dictionary<int, Tuple<string, string, bool, int[]>>()
            {
                {
                    STUDY_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de Estudios", "/admin/generacion-constancias/constancia-de-estudios/", false, new int[] { }) // name, url, isEgressed, academicOrder
                },
                {
                    NON_DEBIT_OF_ASSETS_UNIVERSITY_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de No Adeudo de Bienes a la universidad", "/admin/generacion-constancias/constancia-no-adeudo-bienes-universidad/", false, new int[] { })
                },
                {
                    CERTIFICATE_OF_SUPERIOR_THIRD_TABLE_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de tercio superior", "/admin/generacion-constancias/constancia-tercio-superior/", false, new int[]
                    {
                        ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD,
                        ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                    })
                },
                {
                    CERTIFICATE_OF_SUPERIOR_THIRD_PROMOTION_ENTRANTS_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de tercio superior promoción de ingresantes", "/admin/generacion-constancias/constancia-tercio-superior-promocion-ingresantes/", false, new int[]
                    {
                        ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD,
                        ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                    })
                },
                {
                    NON_DEBIT_OF_ASSETS_UNIVERSITY_RECORD_REQUEST,
                    new Tuple<string, string, bool, int[]>("Solicitud de la Constancia de No Adeudo de Bienes a la universidad", "/admin/generacion-constancias/solicitud-constancia-no-adeudo-bienes-universidad/", false, new int[] { })
                },
                {
                    NON_DEBIT_OF_GOODS_LABORATORIES_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de no adeudo de bienes a laboratorios", "/admin/generacion-constancias/constancia-de-no-adeudo-bienes-laboratorio/", false, new int[] { })
                },
                {
                    PROFESSIONAL_TITLE_REQUEST,
                    new Tuple<string, string, bool, int[]>("Solicitud Título profesional", "/admin/generacion-constancias/solicitud-titulo-profesional/", true, new int[]{})
                },
                {
                    NON_DEBIT_OF_BIBLIOGRAPHIC_MATERIAL_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de no adeudo de material bibliográfico", "/admin/generacion-constancias/constancia-de-no-adeudo-material-bibliografico/", false, new int[] { })
                },
                {
                    NON_DEBIT_OF_BIBLIOGRAPHIC_MATERIAL_REQUEST,
                    new Tuple<string, string, bool, int[]>("Solicitud de Constancia de no adeudo de material bibliográfico", "/admin/generacion-constancias/solicitud-de-constancia-de-no-adeudo-material-bibliografico/", false, new int[] { })
                },
                {
                    EGRESS_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de egresado", "/admin/generacion-constancias/constancia-de-egreso/", true, new  int[] { })
                },
                {
                    CERTIFICATE_OF_SUPERIOR_FIFTH_PROMOTION_ENTRANTS_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de quinto superior promoción de ingresantes", "/admin/generacion-constancias/constancia-quinto-superior-promocion-ingresantes/", false, new int[]
                    {
                        ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                    })
                },
                {
                    CERTIFICATE_OF_SUPERIOR_FIFTH_TABLE_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de quinto superior", "/admin/generacion-constancias/constancia-quinto-superior/", false, new int[]
                    {
                        ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                    })
                },
                {
                    DOCUMENT_COMPLIANCE_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de conformidad de documentos", "/admin/generacion-constancias/constancia-de-conformidad-de-documentos/", false, new int[] { })
                },
                {
                    ENROLLMENT_START_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de Inicio de Matrícula", "/admin/generacion-constancias/constancia-de-inicio-de-matricula/", false, new int[] { })
                },
                {
                    CURRICULAR_ACTIVITIES_RECORD,
                    new Tuple<string, string, bool, int[]>("Constancia de actividades cocurriculares", "/admin/generacion-constancias/constancia-de-actividades-curriculares/", false, new int[] { })
                },
                {
                    BACHELLOR_REQUEST,
                    new Tuple<string, string, bool, int[]>("Solicitud de grado de bachiller", "/admin/generacion-constancias/solicitud-grado-bachiller/", true, new int[] { })
                },
                {
                    CURRICULAR_ACTIVITIES_REQUEST,
                    new Tuple<string, string, bool, int[]>("Solicitud de constancia de actividades cocurriculares", "/admin/generacion-constancias/solicitud-constancia-actividad-cocurricular/", false, new int[] { })
                }
            };
        }

        public static class PROCEDURE_REQUIREMENTS
        {
            public static class TYPE
            {
                public const byte INFORMATIVE = 1;
                public const byte COST = 2;
                public const byte DIGITAL = 3;
                public const byte SYSTEM_VALIDATION = 4;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { INFORMATIVE, "Informativo" },
                    { COST, "Costo" },
                    { DIGITAL, "Digital" },
                    { SYSTEM_VALIDATION, "Validación del Sistema" }
                };
            }

            public static class STATIC_TYPE
            {
                public const int REQUIREMENT_1 = 1;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "REQUIREMENT_1", REQUIREMENT_1 }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { REQUIREMENT_1, "Requisito 1" }
                };
            }

            public static class SYSTEM_VALIDATION_TYPE
            {
                public const byte NO_DEBTS = 1;
                public const byte ENROLLED_IN_THE_ACTIVE_TERM = 2;
                public const byte VALIDATE_GRADUATE = 3;
                public const byte VALIDATE_BACHELOR = 4;
                public const byte VALIDATE_QUALIFIED = 5;
                public const byte VALIDATE_ACTIVE_STUDENT = 6;
                public const byte VALIDATE_NOT_SANCTIONED = 7;
                public const byte VALIDATE_UNBEATEN = 8;
                public const byte VALIDATE_LAST_TERM_APPROVED = 9;
                public const byte VALIDATE_NO_PENDING_GRADES = 10;

                public const byte VALIDATE_UPPER_THIRD = 11;
                public const byte VALIDATE_HIGHER_FIFTH = 12;
                public const byte VALIDATE_MERIT_ORDER = 13;
                public const byte VALIDATE_NOT_SANCTIONED_HISTORY = 14;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NO_DEBTS, "VALIDAR QUE NO TENGA DEUDAS" },
                    { ENROLLED_IN_THE_ACTIVE_TERM, "VALIDAR QUE SE ENCUENTRE MATRICULADO EN EL PERIODO ACTIVO" },
                    { VALIDATE_GRADUATE, "VALIDAR QUE SEA EGRESADO" },
                    { VALIDATE_BACHELOR, "VALIDAR QUE SEA BACHILLER" },
                    { VALIDATE_QUALIFIED, "VALIDAR QUE SEA TITULADO" },
                    { VALIDATE_ACTIVE_STUDENT, "VALIDAR QUE SEA EL ALUMNO SE ENCUENTRE ACTIVO" },
                    { VALIDATE_NOT_SANCTIONED, "VALIDAR QUE EL ALUMNO NO SE ENCUENTRE SANCIONADO" },
                    { VALIDATE_UNBEATEN, "VALIDAR QUE EL ALUMNO SEA INVICTO" },
                    { VALIDATE_LAST_TERM_APPROVED, "VALIDAR QUE EL ÚLTIMO PROMEDIO SEMESTRAL SEA APROBADO" },
                    { VALIDATE_NO_PENDING_GRADES, "VALIDAR QUE NO TENGA NOTAS PENDIENTES" },

                    { VALIDATE_UPPER_THIRD, "VALIDAR QUE PERTENEZCA AL TERCIO SUPERIOR" },
                    { VALIDATE_HIGHER_FIFTH, "VALIDAR QUE PERTENEZCA AL QUINTO SUPERIOR" },
                    { VALIDATE_MERIT_ORDER, "VALIDAR QUE TENGA ORDEN DE MÉRITO" },
                    { VALIDATE_NOT_SANCTIONED_HISTORY, "VALIDAR QUE EL ALUMNO NO HAYA ESTADO SANCIONADO" },
                };
            }
        }
        public static class CURRICULUM
        {
            public static class REGIME_TYPE
            {
                public const byte FLEXIBLE = 1;
                public const byte RIGID = 2;
                public const byte UNSPECIFIED = 3;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { FLEXIBLE, "Flexible" },
                    { RIGID, "Rígido" },
                    { UNSPECIFIED, "No Especificado" }
                };
            }
            public static class STUDY_REGIME
            {
                public const byte BIANNUAL = 1;
                public const byte ANNUAL = 2;
                public const byte MIXED = 3;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { BIANNUAL, "Semestral" },
                    { ANNUAL, "Anual" },
                    { MIXED, "Mixto" }
                };
            }
        }

        public static class DEPENDENCY
        {
            public static class TYPE
            {
                public const byte DEPENDENCY = 1;
                public const byte FACULTY = 2;
                public const byte CAREER = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { DEPENDENCY, "Dependencia" },
                    { FACULTY, "Facultad" },
                    { CAREER, "Escuela Profesional" },
                };
            }
        }

        public static class PROCEDURES
        {

            public static class CONFIGURATION
            {
                public static class PAYMENT_TYPE
                {
                    public const byte PREVIOUS_PAYMENT = 1;
                    public const byte LATER_PAYMENT = 2;
                }

                public static class PREVIOUS_PAYMENT_TYPE
                {
                    public const byte PAYMENT_RECEIPT = 1;
                    public const byte CASHIER = 2;
                    public const byte MIXED = 3;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { PAYMENT_RECEIPT, "Validación por recibo de pago" },
                        { CASHIER, "Validación por caja" },
                        { MIXED, "Validación mixta" },
                    };
                }
            }

            public static class TYPE
            {
                public const byte DEPENDENCY = 1;
                public const byte FACULTY = 2;
                public const byte CAREER = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { DEPENDENCY, "Dependencia" },
                    { FACULTY, "Facultad" },
                    { CAREER, "Escuela Profesional" },
                };
            }

            public static class STATIC_TYPE
            {
                public const int ENROLLMENT_RESERVATION = 1;
                public const int ENROLLMENT_FEE = 2;
                public const int ENROLLMENT_RETIREMENT = 3;
                public const int ENROLLMENT_RECTIFICATION = 4;
                public const int REGULAR_ENROLLMENT = 5;
                public const int REGULAR_EXTEMPORANEOUS_ENROLLMENT = 6;
                public const int SPECIAL_ENROLLMENT = 7;
                public const int SUMMER_ENROLLMENT = 8;
                public const int FAILED_COURSE_ENROLLMENT = 9;
                public const int DIRECTED_COURSE_INSCRIPTION = 10;
                public const int RECOVERY_COURSE_INSCRIPTION = 11;
                public const int EXTRAORDINARY_EVALUATION = 12;
                public const int COMPUTER_SCIENCE_INSTITUTE_ENROLLMENT = 13;
                public const int COMPUTER_SCIENCE_TECHNICAL_EDUCATION_MONTHLY_PAYMENT = 14;
                public const int RESUMPTION_OF_STUDIES_APPLICATION = 15;
                public const int LANGUAGE_CENTER_INSTITUTE_ENROLLMENT = 16;
                public const int LANGUAGE_CENTER_INSTITUTE_MONTHLY_PAYMENT = 17;
                public const int IRREGULAR_ENROLLMENT_CREDITS = 18;
                public const int IRREGULAR_ENROLLMENT_WITH_CURRUCULUM_CHANGE = 19;
                public const int IRREGULAR_ENROLLMENT_WITHOUT_CURRUCULUM_CHANGE = 20;
                public const int IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_CREDITS = 21;
                public const int IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITH_CURRUCULUM_CHANGE = 22;
                public const int IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITHOUT_CURRUCULUM_CHANGE = 23;
                public const int PUBLIC_INFORMATION_ACCESS = 24;
                public const int EXTERNAL_TRANSFER = 25;
                public const int INTERNAL_TRANSFER = 26;
                public const int BACHELOR_REQUESTED = 27;
                public const int TITLE_BY_TESIS = 28;
                public const int TITLE_BY_EXAM = 29;
                public const int TITLE_BY_EXPERIENCE = 30;
                public const int BACHELOR_DEGREE_APPLICATION = 31;
                public const int TITLE_DEGREE_APPLICATION = 32;
                public const int STUDYRECORD = 33;
                public const int PROOFONINCOME = 34;
                public const int ENROLLMENT = 35;
                public const int REGULARSTUDIES = 36;
                public const int EGRESS = 37;
                public const int MERITCHART = 38;
                public const int UPPERFIFTH = 39;
                public const int UPPERTHIRD = 40;
                public const int ACADEMICRECORD = 41;
                public const int ACADEMICPERFORMANCESUMMARY = 42;
                public const int BACHELOR = 43;
                public const int JOBTITLE = 44;
                public const int CERTIFICATEOFSTUDIES = 45;
                public const int CERTIFICATEOFSTUDIESPARTIAL = 45;
                public const int PROCEDURE_INTERNAL = 50;
                public const int PROCEDURE_EXTERNAL = 51;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "ENROLLMENT_RESERVATION", ENROLLMENT_RESERVATION },
                    { "ENROLLMENT_FEE", ENROLLMENT_FEE },
                    { "ENROLLMENT_RETIREMENT", ENROLLMENT_RETIREMENT },
                    { "ENROLLMENT_RECTIFICATION", ENROLLMENT_RECTIFICATION },
                    { "REGULAR_ENROLLMENT", REGULAR_ENROLLMENT },
                    { "REGULAR_EXTEMPORANEOUS_ENROLLMENT", REGULAR_EXTEMPORANEOUS_ENROLLMENT },
                    { "SPECIAL_ENROLLMENT", SPECIAL_ENROLLMENT },
                    { "SUMMER_ENROLLMENT", SUMMER_ENROLLMENT },
                    { "FAILED_COURSE_ENROLLMENT", FAILED_COURSE_ENROLLMENT },
                    { "DIRECTED_COURSE_INSCRIPTION", DIRECTED_COURSE_INSCRIPTION },
                    { "RECOVERY_COURSE_INSCRIPTION", RECOVERY_COURSE_INSCRIPTION },
                    { "EXTRAORDINARY_EVALUATION", EXTRAORDINARY_EVALUATION },
                    { "COMPUTER_SCIENCE_INSTITUTE_ENROLLMENT", COMPUTER_SCIENCE_INSTITUTE_ENROLLMENT },
                    { "COMPUTER_SCIENCE_TECHNICAL_EDUCATION_MONTHLY_PAYMENT", COMPUTER_SCIENCE_TECHNICAL_EDUCATION_MONTHLY_PAYMENT },
                    { "RESUMPTION_OF_STUDIES_APPLICATION", RESUMPTION_OF_STUDIES_APPLICATION },
                    { "LANGUAGE_CENTER_INSTITUTE_ENROLLMENT", LANGUAGE_CENTER_INSTITUTE_ENROLLMENT },
                    { "LANGUAGE_CENTER_INSTITUTE_MONTHLY_PAYMENT", LANGUAGE_CENTER_INSTITUTE_MONTHLY_PAYMENT },
                    { "IRREGULAR_ENROLLMENT_CREDITS", IRREGULAR_ENROLLMENT_CREDITS },
                    { "IRREGULAR_ENROLLMENT_WITH_CURRUCULUM_CHANGE", IRREGULAR_ENROLLMENT_WITH_CURRUCULUM_CHANGE },
                    { "IRREGULAR_ENROLLMENT_WITHOUT_CURRUCULUM_CHANGE", IRREGULAR_ENROLLMENT_WITH_CURRUCULUM_CHANGE },
                    { "IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_CREDITS", IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_CREDITS },
                    { "IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITH_CURRUCULUM_CHANGE", IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITH_CURRUCULUM_CHANGE },
                    { "IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITHOUT_CURRUCULUM_CHANGE", IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITH_CURRUCULUM_CHANGE },
                    { "PUBLIC_INFORMATION_ACCESS", PUBLIC_INFORMATION_ACCESS },
                    { "EXTERNAL_TRANSFER", EXTERNAL_TRANSFER },
                    { "INTERNAL_TRANSFER", INTERNAL_TRANSFER },
                    { "BACHELOR_REQUESTED",BACHELOR_REQUESTED },
                    { "TITLE_BY_TESIS",TITLE_BY_TESIS },
                    { "TITLE_BY_EXAM",TITLE_BY_EXAM },
                    { "TITLE_BY_EXPERIENCE",TITLE_BY_EXPERIENCE },
                    { "BACHELOR_DEGREE_APPLICATION",BACHELOR_DEGREE_APPLICATION },
                    { "TITLE_DEGREE_APPLICATION",TITLE_DEGREE_APPLICATION },
                    { "STUDYRECORD",STUDYRECORD },
                    { "PROOFONINCOME",PROOFONINCOME },
                    { "ENROLLMENT",ENROLLMENT },
                    { "REGULARSTUDIES",REGULARSTUDIES },
                    { "EGRESS",EGRESS },
                    { "MERITCHART",MERITCHART },
                    { "UPPERFIFTH",UPPERFIFTH },
                    { "UPPERTHIRD",UPPERTHIRD },
                    { "ACADEMICRECORD",ACADEMICRECORD },
                    { "ACADEMICPERFORMANCESUMMARY",ACADEMICPERFORMANCESUMMARY },
                    { "BACHELOR",BACHELOR },
                    { "JOBTITLE",JOBTITLE },
                    { "CERTIFICATEOFSTUDIES",CERTIFICATEOFSTUDIES },
                    { "CERTIFICATEOFSTUDIESPARTIAL",CERTIFICATEOFSTUDIESPARTIAL },
                    { "PROCEDURE_INTERNAL",PROCEDURE_INTERNAL },
                    { "PROCEDURE_EXTERNAL",PROCEDURE_EXTERNAL },
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { ENROLLMENT_RESERVATION, "Reserva de Matrícula" },
                    { ENROLLMENT_FEE, "Derecho de Matrícula" },
                    { ENROLLMENT_RETIREMENT, "Retiro de Matrícula" },
                    { ENROLLMENT_RECTIFICATION, "Rectificación de Matrícula" },
                    { REGULAR_ENROLLMENT, "Matrícula Regular" },
                    { REGULAR_EXTEMPORANEOUS_ENROLLMENT, "Matrícula Regular Extemporánea" },
                    { SPECIAL_ENROLLMENT, "Matrícula Especial" },
                    { SUMMER_ENROLLMENT, "Matrícula de Verano" },
                    { FAILED_COURSE_ENROLLMENT, "Matrícula de Curso Reprobado" },
                    { RECOVERY_COURSE_INSCRIPTION, "Inscripción en Curso Dirigido" },
                    { DIRECTED_COURSE_INSCRIPTION, "Inscripción en Curso de recuperación y/o nivelación académica (Ciclo de verano u otros)" },
                    { EXTRAORDINARY_EVALUATION, "Evaluación Extraordinaria" },
                    { COMPUTER_SCIENCE_INSTITUTE_ENROLLMENT, "Matrícula Instituto de Informática" },
                    { COMPUTER_SCIENCE_TECHNICAL_EDUCATION_MONTHLY_PAYMENT, "Mensualidad Carrera Técnica en Informática" },
                    { RESUMPTION_OF_STUDIES_APPLICATION, "Autorización para reanudación de estudios" },
                    { LANGUAGE_CENTER_INSTITUTE_ENROLLMENT, "Matrícula Centro de Idiomas" },
                    { LANGUAGE_CENTER_INSTITUTE_MONTHLY_PAYMENT, "Mensualidad Curso Centro de Idiomas" },
                    { IRREGULAR_ENROLLMENT_CREDITS, "Matrícula Irregular por ciclo menos a 12 créditos" },
                    { IRREGULAR_ENROLLMENT_WITH_CURRUCULUM_CHANGE, "Matrícula Irregular con cambio de currículo" },
                    { IRREGULAR_ENROLLMENT_WITHOUT_CURRUCULUM_CHANGE, "Matrícula Irregular sin cambio de currículo" },
                    { IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_CREDITS, "Matrícula Irregular Extemporánea por ciclo menos a 12 créditos" },
                    { IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITH_CURRUCULUM_CHANGE, "Matrícula Irregular Extemporánea con cambio de currículo" },
                    { IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITHOUT_CURRUCULUM_CHANGE, "Matrícula Irregular Extemporánea sin cambio de currículo" },
                    { PUBLIC_INFORMATION_ACCESS, "Acceso a la Información Pública" },
                    { EXTERNAL_TRANSFER, "Traslado Externo" },
                    { INTERNAL_TRANSFER, "Traslado Interno" },
                    { BACHELOR_REQUESTED, "Obtención por solicitud del grado académico de bachiller" },
                    { TITLE_BY_TESIS, "Obtención del título profesionqal sustentación de tesis" },
                    { TITLE_BY_EXAM, "Obtención del título profesionqal U.N.A por examen de suficiencia" },
                    { TITLE_BY_EXPERIENCE, "Obtención del título profesional por experiencia profesional" },
                    { BACHELOR_DEGREE_APPLICATION, "Solicitud de Grado Bachiller" },
                    { TITLE_DEGREE_APPLICATION, "Solicitud de Título Académico" },
                    { STUDYRECORD ,"Constancia de Estudio"},
                    { PROOFONINCOME, "Constancia de Ingreso" },
                    { ENROLLMENT ,"Constancia de Matricula" },
                    { REGULARSTUDIES ,"Constancia de Estudios Regulares"},
                    { EGRESS ,"Constancia de Egresados" },
                    { MERITCHART , "Cuadro de Méritos"},
                    { UPPERFIFTH , "Constancia de Quinto Superior"},
                    { UPPERTHIRD , "Constancia de Tercio Superior"},
                    { ACADEMICRECORD , "Record Academico Titulacion"},
                    { ACADEMICPERFORMANCESUMMARY , "Resumen de Rendimiento Académico"},
                    { BACHELOR , "Informe de Grado"},
                    { JOBTITLE , "Informe de Titulo profesional"},
                    { CERTIFICATEOFSTUDIES , "Certificado de Estudios"},
                };
            }
            public static class RESOLUTION
            {
                public static class RESOLUTION_TYPE
                {
                    public const int APPEAL = 1;
                    public const int RECONSIDERATION = 2;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { APPEAL, "Apelación" },
                        { RECONSIDERATION, "Reconsideración" }
                    };
                }
            }
            public static class SCORE
            {
                public const int AUTOMATIC = 1;
                public const int MANUAL = 2;
                public const int SEMIAUTOMATIC = 3;
                //public const int POSITIVE = 2;
                //public const int NEGATIVE = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { AUTOMATIC, "Automático" },
                    { MANUAL, "Manual" },
                    { SEMIAUTOMATIC, "Semiautomático" }
                    //{ POSITIVE, "Positivo" },
                    //{ NEGATIVE, "Negativo" }
                };

                public static Dictionary<int, string> VALUES_V2 = new Dictionary<int, string>()
                {
                    //{ AUTOMATIC, "Automático" },
                    { MANUAL, "Manual" },
                    //{ SEMIAUTOMATIC, "Semiautomático" }
                    //{ POSITIVE, "Positivo" },
                    //{ NEGATIVE, "Negativo" }
                };
            }
        }
        public static class ROLES
        {
            #region  ADMINISTRADORES
            public const string ACADEMIC_EXCHANGE_ADMIN = "Administrador de Cooperacion Internacional";
            public const string ADMISSION_ADMIN = "Administrador de Admision";
            public const string CAFETERIA_ADMIN = "Administrador de Comedor";
            public const string COMPUTER_CENTER_ADMIN = "Administrador de Centro de Computo";
            public const string COMPUTER_MANAGEMENT_ADMIN = "Administrador de Gestion Informatica";
            public const string DEGREE_ADMIN = "Administrador de Grados";
            public const string DOCUMENTARY_PROCEDURE_ADMIN = "Administrador de Procedimiento Documental";
            public const string ECONOMIC_MANAGEMENT_ADMIN = "Administrador de Finanzas";
            public const string ENROLLMENT_ADMIN = "Administrador de Matricula";
            public const string EVALUATION_ADMIN = "Administrador de Evaluacion";
            public const string GEO_ADMIN = "Administrador de Geo";
            public const string HELP_DESK_ADMIN = "Administrador de Soporte Técnico";
            public const string INDICATORS_ADMIN = "Administrador de Indicadores";
            public const string ESCALAFON_ADMIN = "Administrador de Escalafón";
            public const string INSTITUTIONAL_AGENDA_ADMIN = "Administrador de Agenda Institucional";
            public const string INTEREST_GROUP_ADMIN = "Administrador de Grupos de Interes";
            public const string INTRANET_ADMIN = "Administrador de Intranet";
            public const string INVESTIGATION_ADMIN = "Administrador de Investigacion";
            public const string JOB_EXCHANGE_ADMIN = "Administrador de Bolsa";
            public const string KARDEX_ADMIN = "Administrador de KARDEX";
            public const string LANGUAGE_CENTER_ADMIN = "Administrador de Centro de Idiomas";
            public const string LAURASSIA_ADMIN = "Administrador de Laurassia";
            public const string PAYROLL_ADMIN = "Administrador de Nomina";
            public const string QUIBUK_ADMIN = "Administrador Quibuk";
            public const string PREUNIVERSITARY_ADMIN = "Administrador de Preuniversitarios";
            public const string RESERVATIONS_ADMIN = "Administrador de Reservaciones";
            public const string RESOLUTIVE_ACTS_ADMIN = "Administrador de Actos Resolutivos";
            public const string ROOM_RESERVATION_SYSTEM_ADMIN = "Administrador de Sistema de Reserva de Habitaciones";
            public const string SERVER_ADMIN = "Administrador de Servidores";
            public const string SERVICE_MANAGEMENT_ADMIN = "Administrador de Gestion de Servicios";
            public const string SISCO_ADMIN = "Administrador de SISCO";
            public const string TEACHING_MANAGEMENT_ADMIN = "Administrador de Gestion Docente";
            public const string TANSPARENCY_PORTAL_ADMIN = "Administrador de Portal de Transparencia";
            public const string TUTORING_ADMIN = "Administrador de Tutorias";
            public const string VIRTUAL_DIRECTORY_ADMIN = "Administrador de Directorio Virtual";
            public const string UNIVERSITARY_EXTENSION_ADMIN = "Administrador de Extensión Universitaria";
            public const string CONTINUINGEDUCATION_ADMIN = "Administrador de Formación Continua";
            //chat 
            public const string CHAT_MANAGER = "Administrador del chat";
            public const string TUTORING_COORDINADOR_ADMIN = "Coordinador Administrador de Tutorias";
            #endregion
            public const string REPORT_QUERIES = "Consulta Reportes";
            public const string OTI_SUPPORT = "Apoyo de Tecnología";
            public const string CERTIFICATION = "Certificación";
            //Intranet
            public const string DATA_QUERIES = "Consultas de Datos";
            public const string GENERAL_ACADEMIC_DIRECTORATE = "Dirección General Académica";
            public const string SOCIAL_SERVICE = "Servicio Social";
            public const string ACADEMIC_COUNTER = "Ventanilla Académica";
            public const string ACADEMIC_ASSISTANT = "Asistente Académico";
            public const string ADMINISTRATIVE_ASSISTANT = "Asistente Administrativo";
            public const string ACADEMIC_COORDINATOR = "Coordinador Académico";
            public const string ACADEMIC_DEPARTMENT_GENERAL = "Director del Departamento General"; // similar superadmin
            public const string ACADEMIC_DEPARTMENT = "Departamento Académico";
            public const string ACADEMIC_DEPARTMENT_DIRECTOR = "Director del Departamento Académico";
            public const string ACADEMIC_DEPARTMENT_COORDINATOR = "Coordinador del Departamento Académico";
            public const string ACADEMIC_SECRETARY = "Secretario Académico";
            public const string ACADEMIC_RECORD = "Registro Académico";
            public const string ACADEMIC_RECORD_STAFF = "Personal de Registro Académico";
            public const string PASSWORD_EDITOR = "Restablecer Clave";
            public const string ADMINISTRATION_OFFICE = "Dirección";
            public const string ADMISSION = "Admisión";
            public const string ALERT_MANAGER = "Administrador de alertas";
            public const string AUTORITY = "Autoridades";
            public const string BUDGET_OFFICE = "Oficina de Presupuesto";
            public const string CAREER_DIRECTOR = "Director de Escuela";
            public const string CAREER_DIRECTOR_GENERAL = "Director de Escuela General";
            public const string COMMITEE = "Comité Evaluador";
            public const string COMPUTER_STUDENTS = "Alumnos de cómputo";
            public const string COMPUTERS_MANAGER = "Administrador de Equipos Informáticos";
            public const string COORDINATOR = "Coordinador de Proyecto";
            public const string COST_CENTER = "Centro de Costo";
            public const string COURSE_COORDINATOR = "Coordinador del curso";
            public const string DEAN = "Decano";
            public const string DEAN_SECRETARY = "Secretario de decano";
            public const string DEGREES_AND_TITLES = "Grados Académicos y Títulos Profesionales";
            public const string DEPENDENCY = "Dependencia";
            public const string DINING_ROOM = "Comedor";
            public const string DOCUMENT_RECEPTION = "Mesa de Partes";
            public const string ENROLLMENT_SUPPORT = "Soporte Matricula";
            public const string ENTERPRISE = "Empresa";
            public const string GENERAL_COUNTER = "Contador General";
            public const string HELP_DESK_TECH = "Técnico de Soporte Técnico";
            public const string INFIRMARY = "Tópico";
            public const string INSTITUTIONAL_WELFARE = "Bienestar";
            public const string JOBEXCHANGE_COORDINATOR = "Coordinador de Seguimiento";
            public const string LANGUAGE_STUDENTS = "Alumnos de idioma";
            public const string LIBRARIAN = "Bibliotecario";
            public const string NUTRITION = "Nutrición";
            public const string OBSTETRICS = "Obstetricia";
            public const string OFFICE = "Oficina";
            public const string OPAC_USER = "Usuario OPAC";
            public const string PRE_UNIVERSITARY_TEACHERS = "Docentes Pre Universitario";
            //public const string PRINCIPAL = "Director de Escuela";
            public const string PROCESSES_ADMIN = "Administrador de Procesos";
            public const string RESEARCH_COORDINATOR = "Coordinador de investigación";
            public const string SOCIAL_RESPONSABILITY_COORDINATOR = "Coordinador de responsabilidad social";
            public const string RECTOR = "Rector";
            public const string VICERRECTOR = "Vicerrector";
            public const string GENERAL_SECRETARY = "Secretario General";
            public const string ACADEMIC_DEPARTMENT_SECRETARY = "Secretario de Departamento Académico";
            public const string TEACHING_QUALITY = "Calidad Docente";
            public const string PROVIDER = "Proveedor";
            public const string PSYCHOLOGY = "Psicología";
            public const string RECORD_MANAGER = "Jefe de Registros";
            public const string RECORDER = "Registrador";
            public const string SECRETARY_OF_GENERAL_SERVICES = "Secretaría de Servicios Generales";
            public const string SECRETARY_OF_PROFESSIONAL_SCHOOLS_ADMINISTRATIONS = "Secretaría de las Direcciones de las Escuelas Profesionales";
            public const string SECRETARY_OF_VICE_PRESIDENCY_PRESIDENCY_AND_ADMINISTRATION = "Secretaría de las Vicepresidencias, Presidencias y  Administración";
            public const string SECTION_MANAGER = "Gestor de secciones";
            public const string CLASSROOM_MANAGER = "Gestor de aulas";
            public const string SELECTION_PROCESS_UNIT = "Unidad de Procesos de Selección";
            public const string SOCIAL_WORKER = "Asistente Social";
            public const string SPORT_AND_CULTURE = "Deporte y Cultura";
            public const string STUDENTS = "Alumnos";
            public const string PRE_UNIVERSITARY_STUDENTS = "Alumnos Pre Universitario";
            public const string SUPERADMIN = "Superadmin";
            public const string SUPERVISOR = "Supervisor";
            public const string SUPPORT_OFFICE_USER = "Usuario de Oficina de Soporte";
            public const string SURVEY_ADMIN = "Administrador Encuesta";
            public const string TEACHER_SUPPORT = "Soporte Docentes";
            public const string TEACHERS = "Docentes";
            public const string TUTOR = "Tutor";
            public const string TUTORING_STUDENT = "Tutorado";
            public const string TUTORING_COORDINATOR = "Coordinador de Tutorias";
            public const string USERS_SUPPORT = "Soporte de Usuarios";
            public const string UTD_QUERIES = "Consultas UTD";
            public const string VRAC_SUPERVISOR = "Supervisor VRAC";
            public const string VALIDATOR = "Validador / Evaluador";
            public const string EVALUATION_MANAGER = "Encargado de Evaluación de Extensión Universitaria";
            public const string PROYECTION_MANAGER = "Encargado de Proyección Social";
            public const string EVALUTION_TEAM_COLLABORATOR = "Cooperante de Equipos";
            //Bienestar
            public const string ADMIN_SOCIAL_WORKER = "Administrador Trabajo Social"; //No tanto como un administrador del sistema , pero si jefe de un grupo de rol, al momento no tiene vinculo con el rol de Asistente Social?
            public const string CAFOBE = "Cafobe";
            //POSDEGREE
            public const string ADMIN_POSDEGREE = "Administrador Posgrado";
            //Bolsa
            public const string CENTRAL_ARCHIVE = "Archivo Central";
            //ECONOMIC MANAGEMENT
            //public const string BASIC_SERVICE = "Servicio Básico";
            public const string CASHIER = "Cajero";
            public const string COLLECTION = "Recaudación";
            public const string COMPARISON_CHART_QUOTES = "Cuadro Comparativo de Cotizaciones";
            public const string INCOMES = "Ingresos Gastos";
            public const string LOGISTIC = "Logística";
            public const string MARKET_RESEARCH_PROGRAMMING_A = "Estudio de Mercado A - Programación";
            public const string MARKET_RESEARCH_PROGRAMMING_B = "Estudio de Mercado B - Programación";
            public const string MARKET_RESEARCH_PROGRAMMING_C = "Estudio de Mercado C - Programación";
            public const string MARKET_RESEARCH_PROGRAMMING_D = "Estudio de Mercado D - Programación";
            public const string PREVIOUS_CONTROL = "Control Previo";
            public const string PROCUREMENTS_UNIT = "Unidad de Adquisiciones";
            public const string PROGRAMMING_UNIT = "Unidad de Programación";
            public const string PURCHASE_ORDERS_A = "Ordenes de Compra A";
            public const string PURCHASE_ORDERS_B = "Ordenes de Compra B";
            public const string PURCHASE_ORDERS_C = "Ordenes de Compra C";
            public const string PURCHASE_ORDERS_D = "Ordenes de Compra D";
            public const string QUOTATIONS = "Cotizaciones";
            public const string RECEIPT_ORDERS = "Recepción de Ordenes";
            public const string REQUIREMENT_RECORD = "Registro de Requerimiento";
            public const string SUPPLY_OFFICE = "Oficina de Abastecimiento";
            public const string STORE = "Almacén";
            public const string TREASURY = "Tesoreria";
            public const string TREASURY_CHIEF = "Jefe de Tesoreria";
            public const string WINDOW = "Ventanilla";
            public const string BANK_COLLECTION = "Recaudación Bancaria";
            public const string EXONERATE_PAYMENT = "Exonerador Deudas";
            public const string TREASURY_OPERATOR = "Operador de Tesorería";
            //INTEREST GROUP
            public const string DEGREE_PROGRAM = "Programa de Estudio";
            public const string PROGRAM_PARTICIPANT = "Participante de Programa";
            //CAFETERIA
            public const string CAFETERIA_PROVIDER = "Proveedor Comedor";
            public const string CAFETERIA_WAREHOUSE = "Almacén Comedor";
            //SCORECARDS
            public const string QUALITY_COORDINATOR = "Coordinador de Calidad";
            //Escalafon
            public const string ESCALAFON_ASSISTANT = "Auxiliar de Escalafon";
            //EVALUATION
            //
            public const string INTERNSHIP_SUPERVISOR = "Supervisor de Prácticas Pre Profesionales";

            //RESOLUTIVE ACTS
            public const string RESOLUTIVE_ACTS_VALIDATOR = "Validador de Actas";
            public const string LEGAL_SECREATARY = "Secretario Jurídico";
            public const string RESOLUTIVE_ACTS_DIRECTOR = "Director de Actas";

            public const string EXTERNAL_USER = "Usuario Externo";

            public const string VALIDATE_ADMISSION = "Validar Admisión";
            public const string CHECKER_ADMISSION = "Verificador Admisión";
            public const string ADMISSION_PRINCIPAL_OFFICE = "Dirección de Admisión";

            public const string DEBT_MANAGER = "Gestor de Deudas";

            public const string SYLLABUS_MANAGER = "Gestor de Silabos";

            public const string INTERNSHIP_COORDINATOR = "Coordinador de Prácticas";

            //Transparencia
            public const string TRANSPARENCY_RECORD_MANAGER = "Transparencia Gestor de Actos";
            public const string TRANSPARENCY_FINANCIAL_MANAGER = "Transparencia Gestor Financiero";
            public const string TRANSPARENCY_INVESTMENT_MANAGER = "Transparencia Gestor de Inversiones";
            public const string TRANSPARENCY_INVESTIGATION_MANAGER = "Transparencia Gestor de Investigaciones";
        }
        public static class USER_REQUIREMENTES_ROLES
        {
            public const int COST_CENTER = 1;
            public const int PROGRAMMING_UNIT = 2;
            public const int BUDGET_OFFICE = 3;
            public const int PROCUREMENTS_UNIT = 4;
            public const int SELECTION_PROCESS_UNIT = 5;
            public const int LOGISTIC = 6;
            public const int MARKET_RESEARCH_PROGRAMMING_A = 7;
            public const int QUOTATIONS = 8;
            public const int COMPARATIVE_CHART_QUOTATIONS = 9;
            public const int PURCHASE_ORDERS_A = 10;
            public const int PREVIOUS_CONTROL = 11;
            public const int REQUIREMENT_RECORD = 12;
            public const int MARKET_RESEARCH_PROGRAMMING_B = 13;
            public const int MARKET_RESEARCH_PROGRAMMING_C = 14;
            public const int MARKET_RESEARCH_PROGRAMMING_D = 15;
            public const int PURCHASE_ORDERS_B = 16;
            public const int PURCHASE_ORDERS_C = 17;
            public const int PURCHASE_ORDERS_D = 18;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { COST_CENTER, "Centro de Costo" },
                    { PROGRAMMING_UNIT, "Unidad de Programación" },
                    { BUDGET_OFFICE, "Oficina de Presupuesto" },
                    { PROCUREMENTS_UNIT, "Unidad de Adquisiciones" },
                    { SELECTION_PROCESS_UNIT, "Unidad de Procesos de Selección" },
                    { LOGISTIC, "Logística" },
                    { MARKET_RESEARCH_PROGRAMMING_A, "Estudio de Mercado A - Programación"},
                    { MARKET_RESEARCH_PROGRAMMING_B, "Estudio de Mercado B - Programación"},
                    { MARKET_RESEARCH_PROGRAMMING_C, "Estudio de Mercado C - Programación"},
                    { MARKET_RESEARCH_PROGRAMMING_D, "Estudio de Mercado D - Programación"},
                    { QUOTATIONS, "Cotizaciones"},
                    { COMPARATIVE_CHART_QUOTATIONS, "Cuadro Comparativo de Cotizaciones"},
                    { PURCHASE_ORDERS_A, "Ordenes de Compra A"},
                    { PURCHASE_ORDERS_B, "Ordenes de Compra B"},
                    { PURCHASE_ORDERS_C, "Ordenes de Compra C"},
                    { PURCHASE_ORDERS_D, "Ordenes de Compra D"},
                    { PREVIOUS_CONTROL, "Control Previo"},
                    { REQUIREMENT_RECORD, "Registro de Requerimiento"}
                };
            //public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
            //    {
            //        { "Centro de Costo",COST_CENTER },
            //        { "Unidad de Programación",PROGRAMMING_UNIT},
            //        { "Oficina de Presupuesto",BUDGET_OFFICE },
            //        { "Oficina de Adquisiciones",PROCUREMENTS_OFFICE },
            //        { "Oficina de Procesos de Selección",SELECTION_PROCESS_OFFICE }
            //    };
        }
        public static class SEARCH_USER
        {
            public const int EXTERNAL = 1;
            public const int STUDENT = 2;
        }
        public static class CLAIMS_USER
        {
            public const string DEPENDENCY_ID = "CLAIM_DEPENDECY_ID";
            public const string DEPENDENCY_NAME = "CLAIM_DEPENDECY_NAME";
        }
        public static class SESSION_TYPE
        {
            public const int THEORY = 1;
            public const int PRACTICE = 2;
            public const int VIRTUAL = 3;
            public const int SEMINAR = 4;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { THEORY, "Teoría" },
                { PRACTICE, "Práctica" },
                { VIRTUAL, "Virtual" },
                { SEMINAR, "Seminario" }
            };
        }
        public static class SELECT2
        {
            public static class DEFAULT
            {
                public const int PAGE_SIZE = 10;
            }
            public static class SERVER_SIDE
            {
                public static class REQUEST_PARAMETERS
                {
                    public const string CURRENT_PAGE = "page";
                    public const string QUERY = "q";
                    public const string REQUEST_TYPE = "_type";
                    public const string SEARCH_TERM = "term";
                }
                public static class REQUEST_TYPE
                {
                    public const string QUERY = "query";
                    public const string QUERY_APPEND = "query_append";
                }
            }
        }
        public static class FORUM_TOPIC
        {
            public const byte ACADEMIC = 1;
            public const byte JOB_OFERT = 2;
            public const byte OTHER = 3;
            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { ACADEMIC, "Académico" },
                { JOB_OFERT, "Ofertas laborales" },
                { OTHER, "Otros" }
            };
            //public static Dictionary<byte, string> TYPES = new Dictionary<string, bool>()
            //{
            //    { "typeFile",  },
            //    { "typeFile", "Académico" },
            //};
        }
        public static class SEX
        {
            public const int UNDEFINED = 0;
            public const int MALE = 1;
            public const int FEMALE = 2;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { UNDEFINED, "No especifica" },
                { MALE, "Masculino" },
                { FEMALE, "Femenino" }
            };
            public static Dictionary<int, string> ABREV = new Dictionary<int, string>()
            {
                { UNDEFINED, "N" },
                { MALE, "M" },
                { FEMALE, "F" }
            };
        }
        public static class AGE_RANGES
        {
            public const int YOUTH = 1;
            public const int EARLY_ADULTHOOD = 2;
            public const int INTERMEDIATE_ADULTHOOD = 3;
            public const int ADVANCED_ADULTHOOD = 4;
            public const int OLD_AGE = 5;
            public const int CHILDHOOD = 6;
            public const int PUBERTY = 7;
            public const int TEEN = 8;
            public class AGE_RANGE
            {
                public string Name { get; set; }
                public int MinAge { get; set; }
                public int MaxAge { get; set; }
            }
            public static Dictionary<int, AGE_RANGE> VALUES => new Dictionary<int, AGE_RANGE>
            {
                {CHILDHOOD, new AGE_RANGE {Name = "Niñez", MinAge = 0, MaxAge = 10}},
                {PUBERTY, new AGE_RANGE {Name = "Pubertad", MinAge = 10, MaxAge = 13}},
                {TEEN, new AGE_RANGE {Name = "Adolescencia", MinAge = 13, MaxAge = 18}},
                {YOUTH, new AGE_RANGE {Name = "Juventud", MinAge = 18, MaxAge = 25}},
                {EARLY_ADULTHOOD,new AGE_RANGE() {Name = "Adultez Temprana", MinAge = 25, MaxAge = 40}},
                {INTERMEDIATE_ADULTHOOD,new AGE_RANGE {Name = "Adultez Intermedia", MinAge = 40, MaxAge = 55}},
                {ADVANCED_ADULTHOOD,new AGE_RANGE {Name = "Adultez Avanzada", MinAge = 55, MaxAge = 65}},
                {OLD_AGE, new AGE_RANGE {Name = "Vejez", MinAge = 65, MaxAge = 130}}
            };
            public static Dictionary<int, AGE_RANGE> VALUES_TEACHER => new Dictionary<int, AGE_RANGE>
            {
                {YOUTH, new AGE_RANGE {Name = "Juventud", MinAge = 18, MaxAge = 25}},
                {EARLY_ADULTHOOD,new AGE_RANGE() {Name = "Adultez Temprana", MinAge = 25, MaxAge = 40}},
                {INTERMEDIATE_ADULTHOOD,new AGE_RANGE {Name = "Adultez Intermedia", MinAge = 40, MaxAge = 55}},
                {ADVANCED_ADULTHOOD,new AGE_RANGE {Name = "Adultez Avanzada", MinAge = 55, MaxAge = 65}},
                {OLD_AGE, new AGE_RANGE {Name = "Vejez", MinAge = 65, MaxAge = 130}}
            };
        }
        public static class STATES
        {
            public const int INACTIVE = 0;
            public const int ACTIVE = 1;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { INACTIVE, "Inactivo" },
                { ACTIVE, "Activo" }
            };
        }
        public static class STUDENT_SECTION_STATES
        {
            public const int IN_PROCESS = 0;
            public const int APPROVED = 1;
            public const int DISAPPROVED = 2;
            public const int WITHDRAWN = 3;
            public const int DPI = 4;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { IN_PROCESS, "En Curso" },
                { APPROVED, "Aprobado" },
                { DISAPPROVED, "Desaprobado" },
                { WITHDRAWN, "Retirado" },
                { DPI, "Desaprobado por Inasistencia" }
            };
        }
        public static class TEACHER_LEVEL_STUDIE
        {
            public const int PREGRADE = 1;
            public const int POSTGRADEMASTER = 2;
            public const int POSTGRADEDOCTORATE = 3;
            public const int POSTGRADESECONDSPECIALITY = 4;
            public const int POSTGRADEDIPLOMAT = 5;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { PREGRADE, "Pregrado" },
                { POSTGRADEMASTER, "Posgrado-Maestría" },
                { POSTGRADEDOCTORATE, "Posgrado-Doctorado" },
                { POSTGRADESECONDSPECIALITY, "Posgrado-Segunda Especialidad" },
                { POSTGRADEDIPLOMAT, "Posgrado-Diplomado" },
            };
        };
        public static class LEVEL_EXPERIENCE
        {
            public const int BASIC = 1;
            public const int INTERMEDIATE = 2;
            public const int ADVANCED = 3;
            public const int EXPERT = 4;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { BASIC, "Básico" },
                { INTERMEDIATE, "Intermedio" },
                { ADVANCED, "Avanzado" },
                { EXPERT, "Experto" }
            };
        };
        public static class CONTACT_CHANNEL_TYPE
        {
            public const int FACEBOOK = 1;
            public const int WEB = 2;
            public const int TWITTER = 3;
            public const int EMAIL = 4;
            public const int LINKEDIN = 5;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { FACEBOOK, "Facebook" },
                { WEB, "Web" },
                { TWITTER, "Twitter" },
                { EMAIL, "Email" },
                { LINKEDIN, "LinkedIn" },
            };
        };
        public static class SURVEY
        {
            public const int TEXT_QUESTION = 1;
            public const int MULTIPLE_SELECTION_QUESTION = 2;
            public const int UNIQUE_SELECTION_QUESTION = 3;
            public const int LIKERT_QUESTION = 4;

            public static Dictionary<int, string> TYPE_QUESTION = new Dictionary<int, string>()
            {
                { TEXT_QUESTION , "Pregunta de texto" },
                { MULTIPLE_SELECTION_QUESTION , "Pregunta de selección múltiple" },
                { UNIQUE_SELECTION_QUESTION , "Pregunta de selección única" },
            };
        }

        public static class SURVEY_LIKERT
        {
            public static class RATING_SCALE
            {
                public static string GET_NAME(int max, byte value)
                {
                    switch (max)
                    {
                        case 4:
                            return SURVEY_LIKERT.RATING_SCALE.OTHER[value];
                        case 5:
                            return SURVEY_LIKERT.RATING_SCALE.LIKERT[value];
                        default:
                            return "-";
                    }
                }

                public static Dictionary<byte, string> LIKERT = new Dictionary<byte, string>()
                {
                    { 1, "Muy en desacuerdo" },
                    { 2, "En desacuerdo" },
                    { 3, "Ni acuerdo, ni desacuerdo"},
                    { 4, "De acuerdo" },
                    { 5, "Muy de acuerdo" },
                };

                public static Dictionary<byte, string> OTHER = new Dictionary<byte, string>()
                {
                    { 1, "Muy insatisfactorio" },
                    { 2, "Insatisfactorio" },
                    { 3, "Satisfactorio" },
                    { 4, "Muy satisfactorio" },
                };
            }
        }

        public static class SURVEY_STATES
        {
            public const int NOTSENT = 1;
            public const int SENT = 2;
            public const int INPROCESS = 3;
            public const int FINISHED = 4;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { NOTSENT, "Creada"},
                { SENT , "Enviada" },
                { INPROCESS , "Enviando"},
                { FINISHED , "Finalizada" }
            };
        }
        public static class TERM_STATES
        {
            public const int INACTIVE = 0;
            public const int ACTIVE = 1;
            public const int FINISHED = 2;
        }
        public static class TYPE_SURVEY
        {
            //General
            public const byte GENERAL = 1;
            //Docente
            public const byte TEACHER_SATISFACTION = 2;
            //Bolsa
            public const byte BOSS_REPORT = 3;
            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { GENERAL, "General" },
                    { TEACHER_SATISFACTION, "Satisfacción al docente" },
                    { BOSS_REPORT, "Jefe Directo" }
                };
        }
        public static class USER_EXTERNAL_PROCEDURES
        {
            public static class STATUS
            {
                public const int REQUESTED = 1;
                public const int IN_PROCESS = 2;
                public const int UNPAID = 3;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "REQUESTED", REQUESTED },
                    { "IN_PROCESS", IN_PROCESS },
                    { "UNPAID", UNPAID }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { REQUESTED, "Solicitado" },
                    { IN_PROCESS, "En Proceso" },
                    { UNPAID, "No pagado" }
                };
            }
            public static class PaymentType
            {
                public const int VISA = 1;
                public const int CAJA = 2;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "VISA", VISA},
                    { "CAJA", CAJA}
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { VISA, "VISA" },
                    { CAJA, "CAJA" }
                };
            }
        }
        public static class USER_INTERNAL_PROCEDURES
        {
            public static class STATUS
            {
                public const int DISPATCHED = 1;
                public const int ARCHIVED = 2;
                public const int ACCEPTED = 3;
                public const int NOT_APPLICABLE = 4;
                //
                public const int DOCUMENT_RECEPTION = 5;
                public const int PREPARE_REPORT = 6;
                public const int OBSERVATION = 7;
                public const int VERIFY_OBSERVATION = 8;
                public const int SIGNATURE = 9;
                public const int SENT_TO_FACULTY = 10;
                public const int GENERATED = 11;
                public const int FINALIZED = 12;

                //public const int DERIVED = 13;

                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "DISPATCHED", DISPATCHED },
                    { "ARCHIVED", ARCHIVED },
                    { "ACCEPTED", ACCEPTED },
                    { "NOT_APPLICABLE", NOT_APPLICABLE },
                    { "FINALIZED", FINALIZED },
                    //{ "DERIVED", DERIVED },
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { DISPATCHED, "Pendiente" },
                    { ARCHIVED, "Archivado" },
                    { ACCEPTED, "Aceptado" },
                    { NOT_APPLICABLE, "No Procede" },
                    { FINALIZED, "Finalizado" },
                    //{ DERIVED, "Derivado" },
                };
                public static Dictionary<int, string> VALUES_V2 = new Dictionary<int, string>()
                {
                    { DOCUMENT_RECEPTION, "Recepción de Documento" },
                    { PREPARE_REPORT, "Elaborar Informe" },
                    { OBSERVATION, "Observación" },
                    { VERIFY_OBSERVATION, "Verificar Observación" },
                    { SIGNATURE, "Firmas" },
                    { SENT_TO_FACULTY, "Enviado a Facultad" },
                    { GENERATED, "Generado" }
                };
                public static Dictionary<int, string> ALLVALUES = new Dictionary<int, string>()
                {
                    { DISPATCHED, "Pendiente" },
                    { ARCHIVED, "Archivado" },
                    { ACCEPTED, "Aceptado/Finalizado" },
                    { NOT_APPLICABLE, "No Procede" },
                    { DOCUMENT_RECEPTION, "Recepción de Documento" },
                    { PREPARE_REPORT, "Elaborar Informe" },
                    { OBSERVATION, "Observación" },
                    { VERIFY_OBSERVATION, "Verificar Observación" },
                    { SIGNATURE, "Firmas" },
                    { SENT_TO_FACULTY, "Enviado a Facultad" },
                    { GENERATED, "Generado" }
                };
            }
            public static class TYPE
            {
                public const int ANSWER = 1;
                public const int RESEND = 2;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "ANSWER", ANSWER },
                    { "RESEND", RESEND }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { ANSWER, "Responder" },
                    { RESEND, "Reenviar" },
                };
            }
        }
        public static class USER_PROCEDURES
        {
            public static class FILE
            {
                public static class STATUS
                {
                    public const int SENT = 1;
                    public const int RESOLVED = 2;
                    public const int OBSERVED = 3;

                    public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                    {
                        { "SENT", SENT },
                        { "RESOLVED", RESOLVED },
                        { "OBSERVED", OBSERVED }
                    };
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { SENT, "Enviado" },
                        { RESOLVED, "Resuelto" },
                        { OBSERVED, "Observado" }
                    };
                }
                public static class TYPE
                {
                    public const int EXTERNAL_USER = 1;
                    public const int DEPENDENCY = 2;
                    public const int FINALY = 3;
                    public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                    {
                        { "EXTERNAL_USER", EXTERNAL_USER },
                        { "DEPENDENCY", DEPENDENCY },
                        { "FINALY", FINALY }
                    };
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { EXTERNAL_USER, "Usuario Externo" },
                        { DEPENDENCY, "Dependencia" },
                        { FINALY, "Finalizado" }
                    };
                }
                public static class TITLE
                {
                    public const string REQUIREMENT = "Req";
                    public const string ANEXANNEXED = "Anx";
             
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    public const string FINALY = "FNL";
                }
            }
            public static class OBSERVATION
            {
                public static class STATUS
                {
                    public const int SENT = 1;
                    public const int RESOLVED = 2;
                    public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                    {
                        { "SENT", SENT },
                        { "RESOLVED", RESOLVED }
                    };
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { SENT, "Enviado" },
                        { RESOLVED, "Resuelto" }
                    };
                }
            }
            public static class STATUS
            {
                public const int REQUESTED = 1;
                public const int ARCHIVED = 2;
                public const int IN_PROCESS = 3;
                public const int ACCEPTED = 4;
                public const int NOT_APPLICABLE = 5;
                public const int PENDING = 6;
                public const int OBSERVED = 7;
                public const int GENERATED = 8;
                public const int FINALIZED = 9;
                public const int TO_COMPLETE = 10;
                public const int PENDING_PAYMENT = 11;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "REQUESTED", REQUESTED },
                    { "ARCHIVED", ARCHIVED },
                    { "IN_PROCESS", IN_PROCESS },
                    { "ACCEPTED", ACCEPTED },
                    { "NOT_APPLICABLE", NOT_APPLICABLE },
                    { "PENDING", PENDING },
                    { "OBSERVED", OBSERVED },
                    { "GENERATED", GENERATED },
                    { "FINALIZED", FINALIZED },
                    { "TO_COMPLETE", TO_COMPLETE },
                    { "PENDING_PAYMENT", PENDING_PAYMENT },
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { REQUESTED, "Solicitado" }, // Solicitado > En Proceso > Aceptado / Denegado // Va a saltar de Observado / En Proceso mientras hayan observaciones // Una vez que las observaciones se hayan resuelto va a cambiar de vuelta a En Proceso
                    { ARCHIVED, "Archivado" },
                    { IN_PROCESS, "En Proceso" },
                    { ACCEPTED, "Aceptado" },
                    { NOT_APPLICABLE, "No Procede" },
                    { PENDING, "Pendiente"},
                    { OBSERVED, "Observado"},
                    { GENERATED, "Generado"},
                    { FINALIZED, "Finalizado"},
                    { TO_COMPLETE, "Por Completar"},
                    { PENDING_PAYMENT, "Pendiente de Pago"},
                };
            }
        }
        public static class USER_REQUIREMENTS
        {
            public static class STATUS
            {
                public const int REQUESTED = 1;
                public const int ACCEPTED = 2;
                public const int NOT_APPLICABLE = 3;
                public const int PENDING_SIGNATURE = 4;
                public const int PENDING = 5;
                public const int PENDING_REGISTER = 6;
                public const int EXECUTION = 7;
                public const int TO_DERIVE = 8;
                public const int FINALIZED = 9;
                public const int REGISTER = 10;
                public const int PENDING_CERTIFICATION = 11;
                public const int CERTIFICATION = 12;
                public const int PENDING_APPROVAL = 13;
                public const int FILE_DEVELOPMEN = 14;
                public const int APPROVE_CASEFILE = 15;
                public const int APPROVED_RECORD = 16;
                public const int TO_QUOTE = 17;
                public const int DERIVED_TO_RESPONSABLE_SELECCION = 18;
                public const int DERIVED_TO_RESPONSABLE_OEC = 19;
                public const int PENDING_DEVELOPMENT = 20;
                public const int TO_DERIVE_RESPONSABLE = 21;
                public const int EXECUTION_ORDER = 22;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "REQUESTED", REQUESTED },
                    { "ACCEPTED", ACCEPTED },
                    { "NOT_APPLICABLE", NOT_APPLICABLE },
                    { "PENDING_SIGNATURE", PENDING_SIGNATURE },
                    { "PENDING", PENDING },
                    { "PENDING_REGISTER", PENDING_REGISTER },
                    { "EXECUTION", EXECUTION },
                    { "TO_DERIVE", TO_DERIVE },
                    { "FINALIZED", FINALIZED },
                    { "REGISTER", REGISTER },
                    { "PENDING_CERTIFICATION", PENDING_CERTIFICATION },
                    { "PENDING_DEVELOPMENT", PENDING_DEVELOPMENT },
                    { "CERTIFICATION", CERTIFICATION },
                    { "PENDING_APPROVAL", PENDING_APPROVAL },
                    { "FILE_DEVELOPMEN", FILE_DEVELOPMEN },
                    { "APPROVE_CASEFILE", APPROVE_CASEFILE },
                    { "APPROVED_RECORD", APPROVED_RECORD },
                    { "TO_QUOTE", TO_QUOTE },
                    { "DERIVED_TO_RESPONSABLE_SELECCION", DERIVED_TO_RESPONSABLE_SELECCION },
                    { "DERIVED_TO_RESPONSABLE_OEC", DERIVED_TO_RESPONSABLE_OEC },
                    { "TO_DERIVE_RESPONSABLE", TO_DERIVE_RESPONSABLE },
                    { "EXECUTION_ORDER", EXECUTION_ORDER }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { REQUESTED, "Solicitado" },
                    { ACCEPTED, "Aceptado" },
                    { NOT_APPLICABLE, "No Procede" },
                    { PENDING_SIGNATURE, "Pendiente de Firma" },
                    { PENDING, "Pendiente" },
                    { PENDING_REGISTER, "Pendiente Detalle" },
                    { EXECUTION, "En ejecución" },
                    { TO_DERIVE, "Por derivar" },
                    { FINALIZED, "Finalizado" },
                    { REGISTER, "Registrado" },
                    { PENDING_CERTIFICATION, "Pendiente de Certificación" },
                    { PENDING_DEVELOPMENT, "Pendiente de Contratación" },
                    { CERTIFICATION, "Certificación" },
                    { PENDING_APPROVAL, "Pendiente de Aprobación" },
                    { FILE_DEVELOPMEN, "Solicitud de Aprobación de Expediente de Contratación" },
                    { APPROVE_CASEFILE, "Aprobar Expediente" },
                    { APPROVED_RECORD, "Expediente Aprobado" },
                    { TO_QUOTE, "Por Cotizar" },
                    { DERIVED_TO_RESPONSABLE_SELECCION, "Derivado a Comité de Selección" },
                    { DERIVED_TO_RESPONSABLE_OEC, "Derivado a Comité OEC" },
                    { TO_DERIVE_RESPONSABLE, "Por derivar a Comité" },
                    { EXECUTION_ORDER, "En ejecución O." }
                };
            }
            public static class SUBSTATUS
            {
                public const int CONSOLIDATE_NECESSITIES_TABLES = 3;
                public const int ANNUAL_PLAN = 4;
                public const int PURCHASE_ORDER_RECORD = 5;
                public const int MARKET_ANALYSIS = 6;
                public const int DETERMINATE_ESTIMATED_VALUE = 7;
                public const int CERTIFICATION = 8;
                public const int FILE_APPROVAL = 9;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "CONSOLIDATE_NECESSITIES_TABLES", CONSOLIDATE_NECESSITIES_TABLES },
                    { "ANNUAL_PLAN", ANNUAL_PLAN },
                    { "PURCHASE_ORDER_RECORD", PURCHASE_ORDER_RECORD },
                    { "MARKET_ANALYSIS", MARKET_ANALYSIS },
                    { "DETERMINATE_ESTIMATED_VALUE", DETERMINATE_ESTIMATED_VALUE },
                    { "CERTIFICATION", CERTIFICATION },
                    { "FILE_APPROVAL", FILE_APPROVAL }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { CONSOLIDATE_NECESSITIES_TABLES, "Consolida cuadros de necesidades" },
                    { ANNUAL_PLAN, "Plan Anual" },
                    { PURCHASE_ORDER_RECORD, "Registro de Pedido de Compra" },
                    { MARKET_ANALYSIS, "Estudio de Mercado" },
                    { DETERMINATE_ESTIMATED_VALUE, "Determina Valor Estimado" },
                    { CERTIFICATION, "Certificación" },
                    { FILE_APPROVAL, "Aprobación de Expediente" }
                };
            }
            public static class PROCESSTYPE
            {
                public const int BIDDING = 1;
                public const int PUBLIC_COMPETITION = 2;
                public const int PUBLIC = 3;
                public const int SIMPLIFIED_AWARD = 4;
                public const int SELECTION_OF_INDIVIDUAL_CONSULTANTS = 5;
                public const int PRICE_COMPARISON = 6;
                public const int ELECTRONIC_REVERSE_AUCTION = 7;
                public const int DIRECT_HIRE = 8;
                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "BIDDING", BIDDING },
                    { "PUBLIC_COMPETITION", PUBLIC_COMPETITION },
                    { "PUBLIC", PUBLIC },
                    { "SIMPLIFIED_AWARD", SIMPLIFIED_AWARD },
                    { "SELECTION_OF_INDIVIDUAL_CONSULTANTS", SELECTION_OF_INDIVIDUAL_CONSULTANTS },
                    { "PRICE_COMPARISON", PRICE_COMPARISON },
                    { "ELECTRONIC_REVERSE_AUCTION", ELECTRONIC_REVERSE_AUCTION },
                    { "DIRECT_HIRE", DIRECT_HIRE }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { BIDDING, "Licitación" },
                    { PUBLIC_COMPETITION, "Pública Concurso" },
                    { PUBLIC, "Público" },
                    { SIMPLIFIED_AWARD, "Adjudicación Simplificada" },
                    { SELECTION_OF_INDIVIDUAL_CONSULTANTS, "Selección de Consultores Individuales" },
                    { PRICE_COMPARISON, "Comparación de Precios" },
                    { ELECTRONIC_REVERSE_AUCTION, "Subasta Inversa Electrónica" },
                    { DIRECT_HIRE, "Contratación Directa" }
                };
            }
            public static class ISFORECAST
            {
                public const bool PREVENTION = false;
                public const bool CERTIFICATION = true;
                public static Dictionary<bool, string> VALUES = new Dictionary<bool, string>()
                {
                    { PREVENTION, "Previsión" },
                    { CERTIFICATION, "Certificación" }
                };
            }
        }
        public static class WEEKDAY
        {
            public const int MONDAY = 0;
            public const int TUESDAY = 1;
            public const int WEDNESDAY = 2;
            public const int THURSDAY = 3;
            public const int FRIDAY = 4;
            public const int SATURDAY = 5;
            public const int SUNDAY = 6;
            public static string ENUM_TO_STRING(DayOfWeek day)
            {
                return VALUES[ENUM_TO_INT(day)];
            }
            public static int ENUM_TO_INT(DayOfWeek day)
            {
                return (int)day - 1 == -1 ? 6 : (int)day - 1;
            }
            public static DayOfWeek TO_ENUM(int day)
            {
                return (DayOfWeek)(day + 1 == 7 ? 0 : day + 1);
            }
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { MONDAY, "Lunes" },
                { TUESDAY, "Martes" },
                { WEDNESDAY, "Miércoles" },
                { THURSDAY, "Jueves" },
                { FRIDAY, "Viernes" },
                { SATURDAY, "Sábado" },
                { SUNDAY, "Domingo" }
            };
        }
        public static class SCHOLARSHIP
        {
            public static class POSTULATION
            {
                public static class States
                {
                    public const byte PENDING = 0;
                    public const byte OBSERVED = 1;
                    public const byte APPROVED = 2;
                    public const byte REJECTED = 3;
                    public static string GETVALUE(byte? status)
                    {
                        if (status == PENDING) return "Pendiente";
                        if (status == OBSERVED) return "Observado";
                        if (status == APPROVED) return "Aprobado";
                        if (status == REJECTED) return "Rechazado";
                        return "-";
                    }
                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        {PENDING,"Pendiente" },
                        {OBSERVED,"Observación" },
                        {APPROVED,"Aprobado" },
                        {REJECTED,"Rechazado" }
                    };
                }
            }
        }
        public static class RECORDS
        {
            public const string ROUTEBASE = "/Areas/Admin/Views/RecordGeneration/";
            public const string CSSROUTEBASE = "css/areas/admin/record/";
            public const int CREDIT = 1;
            //public const int STUDY = 2;
            public const int EGRESS = 3;
            public const int CONDUCT = 4;
            public const int ENROLLMENT = 5;
            public const int PROCEDURE = 6;
            public const int GRADE = 7;
            public const int REGULARSTUDIES = 8;
            public const int NODEBT = 9;
            public const int REPORT_CARD = 10;
            public const int UPPERTHIRD = 11;
            public const int NODEBTLABORATORY = 12;
            public const int UPPERFIFTH = 13;
            public const int NODEBTBIBLIOGRAPHIC = 14;
            public const int BACHELOR = 15;
            public const int CONFORMITY = 16;
            public const int PROOFONINCOME = 17;
            public const int STUDYRECORD = 18;
            public const int MERITCHART = 19;
            public const int JOBTITLE = 20;
            public const int ACADEMICRECORD = 21;
            public const int ACADEMICPERFORMANCESUMMARY = 22;
            public const int CERTIFICATEOFSTUDIES = 23;
            public const int RECTIFICATIONCHARGENOTE = 24;
            public const int CERTIFICATEOFSTUDIESPARTIAL = 25;
            public const int COMPLETE_CURRICULUM = 26;
            public const int FIRST_ENROLLMENT = 27;
            public const int CERTIFICATE_MERIT_ORDER = 28;
            public const int CURRICULUM_REVIEW = 29;
            public const int TENTH_HIGHER = 30;
            public const int UPPER_MIDDLE = 31;
            public const int NOT_BE_PENALIZED = 32;

            public const int ENROLLMENT_REPORT = 33;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { CREDIT, "Constancia de Creditaje" },
                //{ STUDY, "Constancia de Estudio" },
                { EGRESS, "Constancia de Egresados" },
                { CONDUCT, "Constancia de Conducta" },
                { ENROLLMENT, "Constancia de Matricula" },
                { PROCEDURE, "Constancia de Trámite" },
                { STUDYRECORD, "Constancia de Estudio" },
                { NODEBT, "Constancia de no adeudo" },
                { REPORT_CARD, "Boleta de Notas" },
                { UPPERTHIRD, "Constancia de Tercio Superior" },
                { NODEBTLABORATORY, "Constancia de adeudo de bienes a laboratorios" },
                { GRADE, "Constancia de Grados" },
                { CONFORMITY, "Constancia de Conformidad" },
                { UPPERFIFTH, "Constancia de Quinto Superior" },
                { NODEBTBIBLIOGRAPHIC, "Constancia de no adeudo de material bibliográfico" },
                { BACHELOR, "Informe de Grado" },
                { PROOFONINCOME,"Constancia de Ingreso" },
                { REGULARSTUDIES,"Constancia de Estudios Regulares" },
                { MERITCHART,"Cuadro de Méritos" },
                { JOBTITLE,"Informe de Titulo profesional" },
                { ACADEMICRECORD,"Record Academico" },
                { ACADEMICPERFORMANCESUMMARY,"Resumen de Rendimiento Académico" },
                { CERTIFICATEOFSTUDIES,"Certificado de Estudios" },
                { RECTIFICATIONCHARGENOTE,"Rectificacion de Notas" },
                { CERTIFICATEOFSTUDIESPARTIAL,"Certificado de Estudios Parcial" },
                { COMPLETE_CURRICULUM,"Constancia de Plan de Estudios Completo" },
                { FIRST_ENROLLMENT,"Constancia de Primera Matrícula" },
                { CURRICULUM_REVIEW,"Revisión Curricular" },
                { TENTH_HIGHER ,"Constancia de Décimo Superior" },
                { UPPER_MIDDLE ,"Constancia de Medio Superior" },
                { NOT_BE_PENALIZED ,"Constancia de no ser sancionado" },
                { ENROLLMENT_REPORT ,"Ficha de Matrícula" },

            };

            public static Dictionary<int, string> CONSTANCIES_VALUES = new Dictionary<int, string>()
            {
                { STUDYRECORD, "Constancia de Estudio" },
                { EGRESS, "Constancia de Egresados" },
                //{ CONDUCT, "Constancia de Conducta" },
                { ENROLLMENT, "Constancia de Matricula" },
                //{ STUDYRECORD, "Constancia de Estudio" },
                { NODEBT, "Constancia de no adeudo" },
                { UPPERTHIRD, "Constancia de Tercio Superior" },
                //{ NODEBTLABORATORY, "Constancia de adeudo de bienes a laboratorios" },
                //{ GRADE, "Constancia de Grados" },
                //{ CONFORMITY, "Constancia de Conformidad" },
                { UPPERFIFTH, "Constancia de Quinto Superior" },
                { TENTH_HIGHER, "Constancia de Décimo Superior" },
                { UPPER_MIDDLE, "Constancia de Medio Superior" },

                //{ NODEBTBIBLIOGRAPHIC, "Constancia de no adeudo de material bibliográfico" },
                { PROOFONINCOME,"Constancia de Ingreso" },
                { REGULARSTUDIES,"Constancia de Estudios Regulares" },
                { FIRST_ENROLLMENT,"Constancia de Primera Matrícula" },
                { NOT_BE_PENALIZED ,"Constancia de no ser sancionado" },
            };

            public static Dictionary<int, string> ENABLED_PROCEDURES = new Dictionary<int, string>()
            {
                { CREDIT, "Constancia de Creditaje" },
                { NODEBT, "Constancia de no adeudo" },
                //{ STUDY, "Constancia de Estudio" },
                { EGRESS, "Constancia de Egresados" },
                { ENROLLMENT, "Constancia de Matricula" },
                { STUDYRECORD, "Constancia de Estudio" },
                { REPORT_CARD, "Boleta de Notas" },
                { UPPERTHIRD, "Constancia de Tercio Superior" },
                { UPPERFIFTH, "Constancia de Quinto Superior" },
                { TENTH_HIGHER, "Constancia de Décimo Superior" },
                { UPPER_MIDDLE, "Constancia de Medio Superior" },
                { CERTIFICATE_MERIT_ORDER, "Constancia de Orden de Mérito" },
                { PROOFONINCOME,"Constancia de Ingreso" },
                { REGULARSTUDIES,"Constancia de Estudios Regulares" },
                { MERITCHART,"Cuadro de Méritos" },
                { ACADEMICRECORD,"Record Academico" },
                { ACADEMICPERFORMANCESUMMARY,"Resumen de Rendimiento Académico" },
                { CERTIFICATEOFSTUDIES,"Certificado de Estudios" },
                { CERTIFICATEOFSTUDIESPARTIAL,"Certificado de Estudios Parcial" },
                { COMPLETE_CURRICULUM,"Constancia de Plan de Estudios Completo" },
                { FIRST_ENROLLMENT,"Constancia de Primera Matrícula" },
                { NOT_BE_PENALIZED ,"Constancia de no ser sancionado" },
                { ENROLLMENT_REPORT ,"Ficha de Matrícula" },
            };

            public static Dictionary<int, int> PROCEDURE_STATIC_TYPE = new Dictionary<int, int>()
            {
                {STUDYRECORD,ConstantHelpers.PROCEDURES.STATIC_TYPE.STUDYRECORD },
                {PROOFONINCOME,ConstantHelpers.PROCEDURES.STATIC_TYPE.PROOFONINCOME },
                {ENROLLMENT,ConstantHelpers.PROCEDURES.STATIC_TYPE.ENROLLMENT },
                {REGULARSTUDIES,ConstantHelpers.PROCEDURES.STATIC_TYPE.REGULARSTUDIES },
                {EGRESS,ConstantHelpers.PROCEDURES.STATIC_TYPE.EGRESS },
                {MERITCHART,ConstantHelpers.PROCEDURES.STATIC_TYPE.MERITCHART },
                {UPPERFIFTH,ConstantHelpers.PROCEDURES.STATIC_TYPE.UPPERFIFTH },
                {UPPERTHIRD,ConstantHelpers.PROCEDURES.STATIC_TYPE.UPPERTHIRD },
                {ACADEMICRECORD,ConstantHelpers.PROCEDURES.STATIC_TYPE.ACADEMICRECORD },
                {ACADEMICPERFORMANCESUMMARY,ConstantHelpers.PROCEDURES.STATIC_TYPE.ACADEMICPERFORMANCESUMMARY },
                {BACHELOR,ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR },
                {JOBTITLE,ConstantHelpers.PROCEDURES.STATIC_TYPE.JOBTITLE },
                {CERTIFICATEOFSTUDIES,ConstantHelpers.PROCEDURES.STATIC_TYPE.CERTIFICATEOFSTUDIES },
                {CERTIFICATEOFSTUDIESPARTIAL,ConstantHelpers.PROCEDURES.STATIC_TYPE.CERTIFICATEOFSTUDIES },
            };
            public static Dictionary<int, string> ROUTES = new Dictionary<int, string>()
            {
                { CREDIT, ROUTEBASE+"Credit.cshtml" },
                //{ STUDY, ROUTEBASE+"Study.cshtml" },
                { EGRESS, ROUTEBASE+"Egress.cshtml" },
                { CONDUCT, ROUTEBASE+"Conduct.cshtml" },
                { ENROLLMENT, ROUTEBASE+"Enrollment.cshtml" },
                { STUDYRECORD, ROUTEBASE+"GetStudyRecord.cshtml" },
                { NODEBT, ROUTEBASE+"nodebt.cshtml" },
                { REPORT_CARD, ROUTEBASE+"reportgrades.cshtml"  },
                { UPPERTHIRD, ROUTEBASE+"upperthird.cshtml"  },
                { NODEBTLABORATORY, ROUTEBASE+"nodebtlaboratory.cshtml"  },
                { UPPERFIFTH, ROUTEBASE+"upperfifth.cshtml"  },
                { NODEBTBIBLIOGRAPHIC, ROUTEBASE+"nodebtbibliographic.cshtml"  },
                { BACHELOR, ROUTEBASE+"bachelor.cshtml"  },
            };
            public static Dictionary<int, string> CSSROUTES = new Dictionary<int, string>()
            {
                { CREDIT, CSSROUTEBASE+"credit.css" },
                //{ STUDY, CSSROUTEBASE+"study.css" },
                { EGRESS, CSSROUTEBASE+"egress.css" },
                { CONDUCT,CSSROUTEBASE+ "conduct.css" },
                { ENROLLMENT,CSSROUTEBASE+ "enrollment.css" },
                { STUDYRECORD, CSSROUTEBASE+"getstudyrecord.css" },
                { NODEBT, CSSROUTEBASE+"nodebt.css" },
                { REPORT_CARD, CSSROUTEBASE+"reportgrades.css"  },
                { UPPERTHIRD, CSSROUTEBASE+"upperthird.css"  },
                { NODEBTLABORATORY, CSSROUTEBASE+"nodebtlaboratory.css"  },
                { UPPERFIFTH, CSSROUTEBASE+"upperfifth.css"  },
                { NODEBTBIBLIOGRAPHIC, CSSROUTEBASE+"nodebtbibliographic.css"  },
                { BACHELOR, CSSROUTEBASE+"bachelor.css"  },
            };
        }
        public static class RECORD_HISTORY
        {
            public const short GENERAL = 1;
            public static Dictionary<short, string> VALUES = new Dictionary<short, string>()
            {
                { GENERAL, "Constancias Generales" }
            };
        }
        public static class RECORD_HISTORY_STATUS
        {
            public const byte GENERATED = 1;
            public const byte WITH_OBSERVATIONS = 2;
            public const byte FINALIZED = 3;
            public const byte WITH_PROCEDURE = 4;

            public static Dictionary<short, string> VALUES = new Dictionary<short, string>()
            {
                { GENERATED, "Solicitado" },
                { WITH_OBSERVATIONS, "Con Observaciones" },
                { FINALIZED, "Finalizado" },
                { WITH_PROCEDURE, "Asociado a un Trámite" },
            };
            public static Dictionary<short, string> VALUES_NOT_INTEGRATED = new Dictionary<short, string>()
            {
                { GENERATED, "Solicitado" },
                { WITH_OBSERVATIONS, "Con Observaciones" },
                { FINALIZED, "Finalizado" },
            };
        }
        public static class Student
        {
            public static class States
            {
                public const int ENTRANT = 1;
                public const int REGULAR = 2;
                public const int TRANSFER = 3;
                public const int IRREGULAR = 4;
                public const int REPEATER = 5;
                public const int GRADUATED = 6;
                public const int RESERVED = 7;
                public const int UNBEATEN = 8;
                public const int SANCTIONED = 9;
                public const int DESERTION = 10;
                public const int RETIRED = 11;
                public const int OBSERVED = 12;
                public const int NOENROLLMENT = 13;
                public const int BACHELOR = 14;
                public const int QUALIFIED = 15;
                public const int EXPELLED = 16;
                public const int NONPRESENTED = 17;
                public const int RESIGNATION = 18;
                public const int CANCELLATION = 19;
                public const int HIGH_PERFORMANCE = 20;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { ENTRANT, "Ingresante" },
                    { REGULAR, "Regular" },
                    { TRANSFER, "Traslado" },
                    { IRREGULAR, "Irregular" },
                    { REPEATER, "Repitente" },
                    { GRADUATED, "Egresado" },
                    { RESERVED, "Reservado" },
                    { UNBEATEN, "Invicto" },
                    { SANCTIONED, "Sancionado" },
                    { DESERTION, "Abandono" },
                    { RETIRED, "Retirado" },
                    { OBSERVED, "Observado" },
                    { NOENROLLMENT, "Sin matrícula" },
                    { BACHELOR, "Bachiller" },
                    { QUALIFIED, "Titulado" },
                    { EXPELLED, "Expulsado" },
                    { NONPRESENTED, "No se Presentó" },
                    { RESIGNATION, "Renuncia" },
                    { CANCELLATION, "Ingreso anulado" },
                    { HIGH_PERFORMANCE, "Alto Rendimiento" },
                };
            }
            public static class Procedures
            {
                public static class DirectedCourse
                {
                    public static class States
                    {
                        public const byte PENDING = 1;
                        public const byte APPROVED = 2;
                        public const byte DISAPPROVED = 3;
                    }
                }
            }
            public static class COURSE_ATTEMPTS
            {
                public const int REGULAR = 1;
                public const int SECOND = 2;
                public const int THIRD = 3;
                public const int FOURTH = 4;
                public const int FIFTH = 5;
                public const int SIXTH = 6;
                public const int SEVENTH = 7;
                public const int EIGHTH = 8;
                public const int NINTH = 9;
                public const int TENTH = 10;
                public const int ELEVENTH = 11;
                public const int TWELFTH = 12;
                public const int THIRTEENTH = 13;
                public const int FOURTEENTH = 14;
                public const int FIFTEENTH = 15;
                public const int SIXTEENTH = 16;
                public const int SEVENTEENTH = 17;
                public const int EIGHTEENTH = 18;
                public const int NINETEENTH = 19;
                public const int TWENTIETH = 20;
                public static Dictionary<int, string> NAMES = new Dictionary<int, string>()
                {
                    { REGULAR,"REGULAR" },
                    { SECOND,"REGULAR" },
                    { THIRD,"TERCERA MAT." },
                    { FOURTH, "CUARTA MAT." },
                    { FIFTH,"QUINTA MAT." },
                    { SIXTH,"SEXTA MAT." },
                    { SEVENTH,"SEPTIMA MAT." },
                    { EIGHTH,"OCTAVA MAT." },
                    { NINTH, "NOVENA MAT." },
                    { TENTH,"DECIMA MAT." },
                    { ELEVENTH,"UNDECIMA MAT." },
                    { TWELFTH,"DUODECIMA MAT." },
                    { THIRTEENTH,"DECIMOTERCERO MAT." },
                    { FOURTEENTH, "DECIMOCUARTO MAT." },
                    { FIFTEENTH,"DECIMOQUINTO MAT." },
                    { SIXTEENTH,"DECIMOSEXTA MAT." },
                    { SEVENTEENTH,"DECIMOSEPTIMA MAT." },
                    { EIGHTEENTH,"DECIMOOCTAVA MAT." },
                    { NINETEENTH,"DECIMONOVENA MAT." },
                    { TWENTIETH,"VIGESIMA MAT." },
                };
            }
            public static class RacialIdentity
            {
                public const byte NATIVE = 1;
                public const byte HALF_BLOOD = 2;
                public const byte OTHER = 99;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { NATIVE, "Originario" },
                    { HALF_BLOOD, "Mestizo" },
                    { OTHER, "No especifica" },
                };
            }

            public static class Portfolio
            {
                public static class Type
                {
                    public const int DOCUMENT = 1;
                    public const int BIRTH_CERTIFICATE = 2;
                    public const int HIGH_SCHOOL_CERTIFICATE = 3;
                    public const int ADMISSION_CERTIFICATE = 4;
                    public const int REGISTRATION_FORM = 5;
                    public const int CARD = 6;
                    public const int MEDICAL_EXAM = 7;
                    public const int MEDICAL_INSURANCE = 8;
                    public const int TC_PHOTO = 9;
                    public const int COMMITMENT_LETTER = 10;
                    public const int UNIVERSITARY_CARD_PAYMENT = 11;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { DOCUMENT, "Copia D.N.I." },
                        { BIRTH_CERTIFICATE, "Part. nacimiento original" },
                        { HIGH_SCHOOL_CERTIFICATE, "Cert. estudios secundarios originales" },
                        { ADMISSION_CERTIFICATE, "Cons. Ingreso" },
                        { REGISTRATION_FORM, "Formulario de inscripción llenado" },
                        { CARD, "Entrega de carnet" },
                        { MEDICAL_EXAM, "Examen médico vigente" },
                        { MEDICAL_INSURANCE, "Seguro médico vigente" },
                        { TC_PHOTO, "Fotos T/C" },
                        { COMMITMENT_LETTER, "Carta compromiso" },
                        { UNIVERSITARY_CARD_PAYMENT, "Pago carnet universitario" }
                    };
                }
            }

            public static class Condition
            {
                public const byte REGULAR = 1;
                public const byte TRANSITORY_PAYER = 2;
                public const byte PERMANENT_PAYER = 3;
                public const byte RETIRED = 4;
                public const byte SECOND_CAREER = 5;
                public const byte WORKERS_SON = 6;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { REGULAR, "Normal" },
                    { TRANSITORY_PAYER, "Pagante Transitorio" },
                    { PERMANENT_PAYER, "Pagante Permanente" },
                    { RETIRED, "Retirado" },
                    { SECOND_CAREER, "Segunda Carrera" },
                    { WORKERS_SON, "H. Trabaj" },
                };
            }

            public static class Benefit
            {
                public const byte NONE = 0;
                public const byte WORKERS_SON = 1;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { NONE, "Ninguno" },
                    { WORKERS_SON, "Hijo de Trabajador" },
                };

                public static Dictionary<int, int> DISCOUNTS = new Dictionary<int, int>
                {
                    { NONE, 0 },
                    { WORKERS_SON, 50 },
                };
            }
        }
        public static class AcademicHistory
        {
            public static class Types
            {
                public const byte REGULAR = 1;
                public const byte DEFERRED = 2;
                public const byte DIRECTED = 3;
                public const byte EXTRAORDINARY_EVALUATION = 4;
                //public const byte SUBSTITUTE_EXAM = 5;
                public const byte CHARGE = 5;
                public const byte SPECIAL = 6;
                public const byte LEVELING = 7;
                public const byte REEVALUATION = 8;
                public const byte HOLIDAY = 9;
                public const byte TEMPORARY_GRADE = 10;
                public const byte GRADE_BY_SENTENCE = 11;
                public const byte SUMMER = 12;
                public const byte CONVALIDATION = 14;
                public const byte CORRECTION = 15;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { REGULAR, "Regular" },
                    { DEFERRED, "Aplazado" },
                    { DIRECTED, "Curso Dirigido" },
                    { EXTRAORDINARY_EVALUATION, "Evaluación Extraordinaria" },
                    //{ SUBSTITUTE_EXAM, "Examen Sustitutorio" },
                    { CHARGE, "Cargo" },
                    { SPECIAL, "Especial" },
                    { LEVELING, "Nivelación" },
                    { REEVALUATION, "Reevaluación" },
                    { HOLIDAY, "Vacaciones" },
                    { TEMPORARY_GRADE, "Nota Temporal" },
                    { GRADE_BY_SENTENCE, "Nota por Sentencia" },
                    { SUMMER, "Verano" },
                    { CONVALIDATION, "Convalidación" },
                    { CORRECTION, "Subsanacion" },

                };
                public static Dictionary<int, string> ABREVIATIONS = new Dictionary<int, string>
                {
                    { REGULAR, "REG" },
                    { DEFERRED, "APL" },
                    { DIRECTED, "DIR" },
                    { EXTRAORDINARY_EVALUATION, "EXT" },
                    //{ SUBSTITUTE_EXAM, "SUS" },
                    { CHARGE, "CAR" },
                    { SPECIAL, "ESP" },
                    { LEVELING, "NIV" },
                    { REEVALUATION, "REE" },
                    { HOLIDAY, "VAC" },
                    { TEMPORARY_GRADE, "TMP" },
                    { GRADE_BY_SENTENCE, "SEN" },
                    { SUMMER, "VER" },
                };

                public static Dictionary<int, string> ABREVIATIONS_V2 = new Dictionary<int, string>
                {
                    { REGULAR, "R" },
                    { DEFERRED, "A" },
                    { DIRECTED, "CD" },
                    { EXTRAORDINARY_EVALUATION, "EE" },
                    //{ SUBSTITUTE_EXAM, "SUS" },
                    { CHARGE, "CH" },
                    { SPECIAL, "E" },
                    { LEVELING, "N" },
                    { REEVALUATION, "RR" },
                    { HOLIDAY, "V" },
                    { TEMPORARY_GRADE, "NT" },
                    { GRADE_BY_SENTENCE, "NS" },
                    { SUMMER, "VR" },
                    { CONVALIDATION, "C" },
                };
            }
        }
        public static class PAYMENT
        {
            public const decimal IGV = 0.18M;
            public static class TYPES
            {
                public const byte PAYMENT = 0;
                public const byte PROCEDURE = 1;
                public const byte POSTULANT_PAYMENT = 2;
                public const byte CONCEPT = 3;
                public const byte EXTERNAL_PROCEDURE = 4;
                public const byte RESERVATION = 5;
                public const byte COMPUTER_STUDENT_PAYMENT = 6;
                public const byte LANGUAGE_CENTER_STUDENT_PAYMENT = 7;
                public const byte EXTRACURRICULAR_COURSE_PAYMENT = 8;
                public const byte ENROLLMENT = 9;
                public const byte DEGREE = 10;
                public const byte EXTEMPORANEOUS_ENROLLMENT = 11;
                public const byte EXTEMPORANEOUS_ENROLLMENT_PROCESSED = 12;
                public const byte EXTEMPORANEOUS_ADMISSION = 13;
                public const byte EXONERATED_COURSE_ENROLLMENT = 14;
                public const byte TEACHER_PERFORMANCE_EVALUATION = 15;
                public const byte POSTGRADE_DATABASE = 16;

                public static Dictionary<int, string> DESCRIPTION = new Dictionary<int, string>()
                {
                    { PAYMENT, "Pago" },
                    { PROCEDURE, "Trámites" },
                    { POSTULANT_PAYMENT, "P. postulante" },
                    { CONCEPT, "Concepto" },
                    { EXTERNAL_PROCEDURE, "T. Externos" },
                    { RESERVATION, "Res. ambientes" },
                    { COMPUTER_STUDENT_PAYMENT, "Cómputo"},
                    { LANGUAGE_CENTER_STUDENT_PAYMENT, "Idiomas"},
                    { EXTRACURRICULAR_COURSE_PAYMENT, "Extracurricular" },
                    { ENROLLMENT, "Matrícula" },
                    { EXTEMPORANEOUS_ENROLLMENT, "Matrícula" },
                    { EXTEMPORANEOUS_ENROLLMENT_PROCESSED, "Matrícula" },
                    { DEGREE, "Grados y títulos" },
                    { EXTEMPORANEOUS_ADMISSION, "Admision" },
                    { EXONERATED_COURSE_ENROLLMENT, "Curso Exonerado" },
                    { TEACHER_PERFORMANCE_EVALUATION, "Evaluación desempeño docente" },
                    { POSTGRADE_DATABASE, "Posgrado" },
                };
            }
            public static class STATUS
            {
                public const byte PENDING = 1;
                public const byte PAID = 2;
                public const byte CANCELLED = 3;
                public static Dictionary<int, string> Status = new Dictionary<int, string>
                {
                    { PENDING, "Pendiente" },
                    { PAID, "Pagado" },
                    { CANCELLED, "Anulado" }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { PENDING, "Pendiente" },
                    { PAID, "Pagado" },
                    { CANCELLED, "Anulado" }
                };
            }
            public static class CONCEPTSTATUS
            {
                public const byte PENDING = 1;
                public const byte APPROVED = 2;
                public const byte DONE = 3;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { PENDING, "Pendiente" },
                    { APPROVED, "Aprovado" },
                    { DONE, "Realizado" },
                };
            }
        }
        public static class DIGITAL_DOCUMENTS
        {
            public static class TYPES
            {
                public const int DIRECTIVE = 1;
                public const int REGULATION = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { DIRECTIVE, "Directiva" },
                    { REGULATION, "Reglamento" },
                };
            }
            public static class CLASS
            {
                public const int INSTITUTIONAL = 1;
                public const int INTERINE = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { INSTITUTIONAL, "Institucional" },
                    { INTERINE, "Interno" },
                };
            }
        }
        public static class Treasury
        {
            public const decimal IGV = 0.18M;
            public static class Invoice
            {
                public const int SERIAL_PADLEFT = 3;
                public const int CORRELATIVE_PADLEFT = 8;
                public static class PdfTemplate
                {
                    public const byte MULTIPLE_CONCEPTS = 1;
                    public const byte UNIQUE_CONCEPT = 2;
                    public const byte TICKET = 3;
                }
                public static class PaymentType
                {
                    public const byte CASH = 1;
                    public const byte VOUCHER = 2;
                    public const byte BANK = 3;

                    public static Dictionary<byte, string> NAMES = new Dictionary<byte, string>()
                {
                    { CASH, "Efectivo" },
                    { VOUCHER, "Voucher" },
                    { BANK, "Banco" }
                };
                }
            }
            public static class DocumentType
            {
                public const int INTERNAL_TICKET = 1;
                public const int TICKET = 2;
                public const int BILL = 3;
                public static Dictionary<int, string> NAMES = new Dictionary<int, string>()
                {
                    { INTERNAL_TICKET, "Boleta Interna" },
                    { TICKET, "Boleta" },
                    { BILL, "Factura" }
                };
            }

            public static class ElectronicDocumentType
            {
                public const byte TICKET = 1;
                public const byte BILL = 2;
                public static Dictionary<byte, string> NAMES = new Dictionary<byte, string>()
                {
                    { TICKET, "Boleta Elec." },
                    { BILL, "Factura Elec." }
                };
            }

            public static class Income
            {
                public static class Type
                {
                    public const byte INCOME = 1;
                    public const byte OUTCOME = 2;
                }
            }
            public static class ExpenseTypes
            {
                public const byte PreviousBalance = 0;
                public const byte Devolution = 1;
                public const byte Expense = 2;
                public const byte Annulment = 3;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
            {
                { PreviousBalance, "Saldo anterior" },
                { Devolution, "Devolución" },
                { Expense, "Gasto" },
                { Annulment, "Anulación" },
            };
            }
            public static class CutType
            {
                public const byte Outcome = 1;
                public const byte Income = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
            {
                { Outcome, "Salida" },
                { Income, "Ingreso" }
            };
            }
            public static class EntityLoadFormat
            {
                public static class UserIdentifier
                {
                    public const byte USERNAME = 1;
                    public const byte DOCUMENT = 2;
                    public const byte BOTH = 3;
                }

                public static class DocumentType
                {
                    public const byte TXT = 1;
                    public const byte EXCEL = 2;
                }
            }
        }
        public static class Intranet
        {

            public static class STUDENT_PORTFOLIO_TYPE
            {
                public const byte GENERAL = 1;
                public const byte BACHELOR = 2;
                public const byte JOBTITLE = 3;
                public const byte ENROLLMENT = 4;

                public static Dictionary<byte, string> NAMES = new Dictionary<byte, string>()
                {
                    { GENERAL, "General" },
                    { BACHELOR, "Bachiller" },
                    { JOBTITLE, "Titulo Profesional" },
                    { ENROLLMENT, "Matrícula" },
                };
            }

            public static class EvaluationReport
            {
                public const int GENERATED = 1;
                public const int RECEIVED = 2;
                public const int PENDING = 3;
                public static Dictionary<int, string> NAMES = new Dictionary<int, string>()
                {
                    { GENERATED, "Generado" },
                    { RECEIVED, "Recibido" },
                    { PENDING, "Pendiente" }
                };
                public static Dictionary<int, string> NAMESV2 = new Dictionary<int, string>()
                {
                    { GENERATED, "Generado o Pendiente" },
                    { RECEIVED, "Recibido" },
                };
            }

            public static class EvaluationReportConfigurationFormatDate
            {
                public const int CreatedAt = 1;
                public const int ReceptionDate = 2;
                public const int LastGradePublished = 3;

                public static Dictionary<int, string> NAMES = new Dictionary<int, string>()
                {
                    { CreatedAt, "Fecha de Emisión" },
                    { ReceptionDate, "Fecha de Recepción" },
                    { LastGradePublished, "Última nota publicada" }
                };
            }

            public static class EvaluationReportType
            {
                public const byte REGULAR = 1;
                public const byte DEFERRED = 2;
                public const byte DIRECTED_COURSE = 3;
                public const byte EXTRAORDINARY_EVALUATION = 4;
                public const byte CHARGE = 5;
                public const byte SPECIAL = 6;
                public const byte LEVELING = 7;
                public const byte REEVALUATION = 8;
                public const byte HOLIDAY = 9;
                public const byte TEMPORARY_GRADE = 10;
                public const byte GRADE_BY_SENTENCE = 11;
                public const byte SUMMER = 12;
                public const byte CONVALIDATION = 14;
                public const byte CORRECTION_EXAM = 15;

                public static Dictionary<byte, string> NAMES = new Dictionary<byte, string>
                {
                    { REGULAR, "Regular" },
                    { DEFERRED, "Aplazado" },
                    { DIRECTED_COURSE, "Dirigido" },
                    { EXTRAORDINARY_EVALUATION, "Evaluación Extraordinaria" },
                    { CHARGE, "Cargo" },
                    { SPECIAL, "Especial" },
                    { LEVELING, "Nivelación" },
                    { REEVALUATION, "Reevaluación" },
                    { HOLIDAY, "Vacaciones" },
                    { TEMPORARY_GRADE, "Nota Temporal" },
                    { GRADE_BY_SENTENCE, "Nota por Sentencia" },
                    { SUMMER, "Verano" },
                    { CONVALIDATION, "Convalidación" },
                    { CORRECTION_EXAM, "Subsanación Especial" },
                };
            }
            public static class EVALUATION_REPORT_FORMAT
            {
                public static class ACT_FORMAT
                {
                    public const byte FORMAT_1 = 1;
                    public const byte FORMAT_2 = 2;
                    public const byte FORMAT_3 = 3;
                    public const byte FORMAT_4 = 4; //UNAH
                    public const byte FORMAT_5 = 5; //UNAB
                    public const byte FORMAT_6 = 6; //UNISCJSA
                    public const byte FORMAT_7 = 7; //UNSCH
                    public const byte FORMAT_8 = 8; //UFrontera
                    public const byte FORMAT_9 = 9; //UNAAA
                    public const byte FORMAT_11 = 11; //UNIFSLB (Bagua)
                    public const byte FORMAT_12 = 12; //UNSM
                    public const byte FORMAT_13 = 13; //UNISCJSA MEMBRETADO
                    public const byte FORMAT_14 = 14; //UNJBG
                    public const byte FORMAT_16 = 16; //Folklore

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                    {
                        {FORMAT_1,"Formato 1" },
                        {FORMAT_2,"Formato 2" },
                        {FORMAT_3,"Formato 3" },
                        {FORMAT_4,"Formato 4" },
                        {FORMAT_5,"Formato 5" },
                        {FORMAT_6,"Formato 6" },
                        {FORMAT_7,"Formato 7" },
                        {FORMAT_8,"Formato 8" },
                        {FORMAT_9,"Formato 9" },
                        {FORMAT_11,"Formato 11" },
                        {FORMAT_12,"Formato 12" },
                        {FORMAT_13,"Formato 13" },
                        {FORMAT_14,"Formato 14" },
                        {FORMAT_16,"Formato 16" },
                    };
                }
                public static class REGISTER_FORMAT
                {
                    public const byte FORMAT_1 = 1; //UNAB
                    public const byte FORMAT_4 = 4; //UNAH
                    public const byte FORMAT_5 = 5; //UNAB
                    public const byte FORMAT_6 = 6; //UNISCJSA
                    public const byte FORMAT_7 = 7; //UNSCH
                    public const byte FORMAT_8 = 8; //UFrontera
                    public const byte FORMAT_10 = 10; //UNAJMA
                    public const byte FORMAT_11 = 11; //UNIFSLB (Bagua)
                    public const byte FORMAT_12 = 12; //UNSM
                    public const byte FORMAT_14 = 14; //UNJBG
                    public const byte FORMAT_15 = 15; //UNCP

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                    {
                        { FORMAT_1,"Formato 1" },
                        { FORMAT_4,"Formato 4" },
                        { FORMAT_5,"Formato 5" },
                        { FORMAT_6,"Formato 6" },
                        { FORMAT_7,"Formato 7" },
                        { FORMAT_8,"Formato 8" },
                        { FORMAT_10,"Formato 10" },
                        { FORMAT_11,"Formato 11" },
                        { FORMAT_12,"Formato 12" },
                        { FORMAT_14,"Formato 14" },
                        { FORMAT_15,"Formato 15" },
                    };
                }
            }
            public static class ExtraordinaryEvaluation
            {
                public const byte EXTRAORDINARY = 1;
                public const byte RE_EVALAUTION = 2;
                public const byte EXONERATED = 3;
                public const byte UNIQUE_COURSE = 4;
                public const byte RECOGNITION = 5;
                public const byte PRE_PROFESSIONAL_PRACTICES = 6;

                public static Dictionary<int, string> TYPES = new Dictionary<int, string>
                {
                    { EXTRAORDINARY, "Evaluación Extraordinaria" },
                    { RE_EVALAUTION, "Reevaluación" },
                    { EXONERATED, "Exonerado" },
                    { UNIQUE_COURSE, "Curso Único" },
                    { RECOGNITION, "Convalidación" },
                    { PRE_PROFESSIONAL_PRACTICES, "Prácticas Pre Profesionales" },
                };
            }

            public static class ExtraordinaryEvaluationStudent
            {
                public const byte PENDING = 1;
                public const byte APPROVED = 2;
                public const byte DISAPPROVED = 3;
                public static Dictionary<int, string> States = new Dictionary<int, string>
                {
                    { PENDING, "En proceso" },
                    { APPROVED, "Aprobado" },
                    { DISAPPROVED, "Desaprobado" }
                };
            }
            public static class StudentAbsenceJustification
            {
                public static class Status
                {
                    public const int REQUESTED = 1;
                    public const int TEACHER_APPROVED = 2;
                    public const int OBSERVED = 3;
                    public const int ACCEPTED = 4;
                    public const int DENIED = 5;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                    {
                        { REQUESTED, "Solicitado" },
                        { TEACHER_APPROVED, "Doc. Aprob." },
                        { OBSERVED, "Observado"},
                        { ACCEPTED, "Aceptada" },
                        { DENIED, "Denegada" }
                    };
                }
            }
        }
        // TODO: Remover esto
        public static class DEGREES
        {
            public static class STATUS
            {
                public const byte PRESENTED = 1;
                public const byte OBSERVED = 2;
                public const byte ACEPTED = 3;
                public static Dictionary<int, string> Status = new Dictionary<int, string>
                {
                    { PRESENTED, "Presentado" },
                    { OBSERVED, "Observado" },
                    { ACEPTED, "Aceptado" }
                };
            }
        }
        public static class RESERVATION
        {
            public static class STATUS
            {
                public const byte PENDING = 1;
                public const byte APPROVED = 2;
                public const byte REJECTED = 3;
                public const byte CANCELLED = 4;
                public static Dictionary<int, string> Status = new Dictionary<int, string>
                {
                    { PENDING, "Pendiente" },
                    { APPROVED, "Aprobado" },
                    { REJECTED, "Rechazado" },
                    { CANCELLED, "Cancelado" }
                };
            }
        }
        public static class KARDEX
        {
            public static class STATUS
            {
                public const byte INACTIVE = 1;
                public const byte ACTIVE = 2;
                public const byte UNAVAILABLE = 3;
                public static Dictionary<int, string> Status = new Dictionary<int, string>
                {
                    { INACTIVE, "Inactivo" },
                    { ACTIVE, "Activo" },
                    { UNAVAILABLE, "Agotado" }
                };
            }
        }
        public static class LANGUAGECENTERSYSTEM
        {
            public static class SECTION_STATES
            {
                public const byte OPEN = 1;
                public const byte IN_PROGRESS = 2;
                public const byte COMPLETED = 3;
                public static Dictionary<int, string> States = new Dictionary<int, string>
                {
                    { OPEN, "Abierta" },
                    { IN_PROGRESS, "En curso" },
                    { COMPLETED, "Finalizado" }
                };
            }
            public static class SECTION_STUDENT_STATES
            {
                public const byte APPROVED = 1;
                public const byte DISAPPROVED = 2;
                public const byte WITHDRAWN = 2;
                public static Dictionary<int, string> States = new Dictionary<int, string>
                {
                    { APPROVED, "Aprobado" },
                    { DISAPPROVED, "Desaprobado" },
                    { WITHDRAWN, "Retirado" }
                };
            }
        }
        public enum StudentSectionStates
        {
            [Description("En Curso")]
            InProcess = STUDENT_SECTION_STATES.IN_PROCESS,
            [Description("Aprobado")]
            Approved = STUDENT_SECTION_STATES.APPROVED,
            [Description("Desaprobado")]
            Disapproved = STUDENT_SECTION_STATES.DISAPPROVED,
            [Description("Retirado")]
            Withdrawn = STUDENT_SECTION_STATES.WITHDRAWN
        }
        public enum State
        {
            [Description("Inactivo")]
            Inactive = STATES.INACTIVE,
            [Description("Activo")]
            Active = STATES.ACTIVE
        }
        public enum StateFilter
        {
            [Description("Inactivo")]
            Inactive = STATES.INACTIVE,
            [Description("Activo")]
            Active = STATES.ACTIVE,
            [Description("Todas")]
            All = -1
        }
        public enum TermState
        {
            [Description("Inactivo")]
            Inactive = TERM_STATES.INACTIVE,
            [Description("Activo")]
            Active = TERM_STATES.ACTIVE,
            [Description("Finalizado")]
            Finished = TERM_STATES.FINISHED
        }
        public static class PermissionHelpers
        {
            public enum Permission
            {
                [Description("Eliminar usuarios")]
                DeleteUser,
                [Description("Agregar usuarios")]
                AddUser,
                [Description("Administrar roles")]
                ManageRoles
            }
        }
        public static class DEGREE_MODALITY
        {
            public const string BACHELOR = "B";
            public const string JOBTITLE = "T";
            public const string MASTER = "M";
            public const string DOCTOR = "D";
            public const string SECOND_SPECIALITY = "S";
            public static Dictionary<string, string> DEGREES = new Dictionary<string, string>()
            {
                { BACHELOR, "Bachiller" },
                { JOBTITLE, "Titulo Profesional" },
                { MASTER, "Maestro" },
                { DOCTOR, "Doctor" },
                { SECOND_SPECIALITY, "Título de Segunda Especialidad Profesional" }
            };
        }
        public static class DEGREE_OBTENTION_MOD
        {
            public const string NO_SPECIFY = "No especifica";
            public const string PROFFESIONAL_EXPERIENCE = "Experiencia Profesional";
            public const string SUPPORTING_TESIS = "Sustentación de Tesis";
            public const string SUFFIENCY_EXAM = "Examen de Suficiencia";
            public const string AUTOMATIC_BACHELOR = "Bachillerato Automático";
            public const string ACADEMIC_WORK = "Trabajo Académico";
            public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
            {
                { NO_SPECIFY, NO_SPECIFY },
                { PROFFESIONAL_EXPERIENCE,  PROFFESIONAL_EXPERIENCE},
                { SUPPORTING_TESIS, SUPPORTING_TESIS },
                { SUFFIENCY_EXAM, SUFFIENCY_EXAM },
                { AUTOMATIC_BACHELOR, AUTOMATIC_BACHELOR },
                { ACADEMIC_WORK,ACADEMIC_WORK  },
            };
        }
        public static class PROGRAM_STUDIES
        {
            public const string REGULAR_CYCLE = "CICLO REGULAR";
            public const string RECOGNITION = "CONVALIDACION";
            public const string ACADEMIC_COMPLEMENTATION = "COMPLEMENTACION ACADEMICA";
            public const string PEDAGOGICAL_COMPLEMENTATION = "COMPLEMENTACION PEDAGOGICA";
            public const string PROGRAM_FOR_ADULTS = "PROGRAMA PARA ADULTOS";
            public const string OTHER = "OTROS";
            public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
                {
                    { REGULAR_CYCLE, REGULAR_CYCLE },
                    { RECOGNITION, RECOGNITION },
                    { ACADEMIC_COMPLEMENTATION, ACADEMIC_COMPLEMENTATION },
                    { PEDAGOGICAL_COMPLEMENTATION, PEDAGOGICAL_COMPLEMENTATION },
                    { PROGRAM_FOR_ADULTS, PROGRAM_FOR_ADULTS },
                    { OTHER, OTHER },
                };
        }
        public static class STUDY_MODALITY
        {
            public const string PRESENTIAL = "P";
            public const string SEMI_PRESENTIAL = "S";
            public const string DISTANCE = "D";
            public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
            {
                { PRESENTIAL, "Presencial" },
                { SEMI_PRESENTIAL, "Semi Presencial" },
                { DISTANCE, "Distancia" }
            };
        }
        public static class THESIS_SUPPORT_MODALITY
        {
            public const int NOTHING = 0;
            public const int PRESENTIAL = 1;
            public const int VIRTUAL = 2;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { NOTHING, "-" },
                { PRESENTIAL, "Presencial" },
                { VIRTUAL, "Virtual" }
            };
        }
        public static class EMISION_TYPE_OF_DIPLOMAT
        {
            public const string ORIGINAL = "O";
            public const string DUPLICATE = "D";
            public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
            {
                { ORIGINAL, "Original" },
                { DUPLICATE, "Duplicado" }
            };
        }
        public static class Configuration
        {
            public static class General
            {
                public const string INSTITUTION_NAME = "GEN_Institution_Name";
                public const string INSTITUTION_ACRONYM = "GEN_Institution_Acronym";
                public const string INSTITUTION_PHONENUMBER = "GEN_Institution_PhoneNumber";
                public const string INSTITUTION_ADDRESS = "GEN_Institution_Address";
                public const string INSTITUTION_RUC = "GEN_Institution_Ruc";
                public const string INSTITUTION_AUTHORIZATION_TYPE = "GEN_Institution_Authorization_Type";
                public const string INTEGRATED_SYSTEM = "GEN_Integrated_System";
                public const string INSTITUTION_EMAIL = "GEN_Institution_Email";
                public const string INSTITUTION_CODE = "GEN_Institution_Code";
                public const string INSTITUTION_GRADE_CODE = "GEN_Institution_Grade_Code";
                public const string INSTITUTION_INEI_CODE = "GEN_Institution_INEI_Code";
                public const string INSTITUTION_WEBSITE = "GEN_Institution_Website";
                public const string JOBEXCHANGE_WEBSITE = "GEN_JobExchange_Website";
                public const string INSTITUTION_VALIDITY = "GEN_Institution_Validity";
                public const string INSTITUTION_MANAGEMENT_TYPE = "GEN_Institution_Management_Type";
                public const string INSTITUTION_CONSTITUIION_TYPE = "GEN_Institution_Constitution_Type";
                public const string DOCUMENT_LOGO_PATH = "GEN_Document_Logo_Path";
                public const string DOCUMENT_SUPERIOR_TEXT = "GEN_Document_Superior_Text";
                public const string DOCUMENT_HEADER_TEXT = "GEN_Document_Header_Text";
                public const string DOCUMENT_SUBHEADER_TEXT = "GEN_Document_Subheader_Text";
                public const string PIDE_INTEGRATED = "GEN_Pide_Integrated";
                public const string ADMINISTRATOR_ACCESS_IP = "GEN_Administrator_Access_IP";
                public const string ENABLE_MOODLE_INTEGRATION = "GEN_Enable_Moodle_Integration";
                public const string MOODLE_WEBSERVICE_URL = "GEN_Moodle_Webservice_Url";
                public const string PASSWORD_EXPIRATION_DAYS = "GEN_Password_Expiration_Days";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { INSTITUTION_NAME, ConstantHelpers.Institution.Names[ConstantHelpers.GENERAL.Institution.Value]},
                    { INSTITUTION_ACRONYM, ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value] },
                    { INSTITUTION_ADDRESS, "" },
                    { INSTITUTION_PHONENUMBER , ""},
                    { INSTITUTION_RUC, "" },
                    { INSTITUTION_AUTHORIZATION_TYPE, ConstantHelpers.Configuration.Authorization.VALUES[ConstantHelpers.Configuration.Authorization.AUTHORIZATED]},
                    { INTEGRATED_SYSTEM, "true" },
                    { INSTITUTION_WEBSITE, "" },
                    { INSTITUTION_EMAIL, "" },
                    { JOBEXCHANGE_WEBSITE, "" },
                    { INSTITUTION_CODE, "000" },
                    { INSTITUTION_GRADE_CODE, "000" },
                    { INSTITUTION_INEI_CODE, "000000000" },
                    { INSTITUTION_VALIDITY, "0" },
                    { INSTITUTION_MANAGEMENT_TYPE, "0" },
                    { INSTITUTION_CONSTITUIION_TYPE, "0" },
                    { DOCUMENT_LOGO_PATH, "" },
                    { DOCUMENT_SUPERIOR_TEXT, "" },
                    { DOCUMENT_HEADER_TEXT, "" },
                    { DOCUMENT_SUBHEADER_TEXT, "" },
                    { PIDE_INTEGRATED, "false" },
                    { ADMINISTRATOR_ACCESS_IP, "0.0.0.0" },
                    { ENABLE_MOODLE_INTEGRATION, "false" },
                    { MOODLE_WEBSERVICE_URL, "" },
                    { PASSWORD_EXPIRATION_DAYS, "0" }
                };
            }
            public static class Email
            {
                public const string EMAIL_MULTIPLE_ENABLED = "EMAIL_Multiple_Enabled";
                public const string GENERAL_EMAIL = "EMAIL_General_Email";
                public const string GENERAL_EMAIL_PASSWORD = "EMAIL_General_Email_Password";
                public const string GENERAL_EMAIL_SMTP_HOST = "EMAIL_General_Email_Smtp_Host";
                public const string GENERAL_EMAIL_SMTP_PORT = "EMAIL_General_Email_Smtp_Port";
                public const string INTRANET_EMAIL = "EMAIL_Intranet_Email";
                public const string INTRANET_EMAIL_PASSWORD = "EMAIL_Intranet_Email_Password";
                public const string INTRANET_EMAIL_SMTP_HOST = "EMAIL_Intranet_Email_Smtp_Host";
                public const string INTRANET_EMAIL_SMTP_PORT = "EMAIL_Intranet_Email_Smtp_Port";
                public const string JOBEXCHANGE_EMAIL = "EMAIL_Jobexchange_Email";
                public const string JOBEXCHANGE_EMAIL_PASSWORD = "EMAIL_Jobexchange_Email_Password";
                public const string JOBEXCHANGE_EMAIL_SMTP_HOST = "EMAIL_Jobexchange_Email_Smtp_Host";
                public const string JOBEXCHANGE_EMAIL_SMTP_PORT = "EMAIL_Jobexchange_Email_Smtp_Port";
                public const string LAURASSIA_EMAIL = "EMAIL_Laurassia_Email";
                public const string LAURASSIA_EMAIL_PASSWORD = "EMAIL_Laurassia_Email_Password";
                public const string LAURASSIA_EMAIL_SMTP_HOST = "EMAIL_Laurassia_Email_Smtp_Host";
                public const string LAURASSIA_EMAIL_SMTP_PORT = "EMAIL_Laurassia_Email_Smtp_Port";
                public const string ADMISSION_EMAIL = "EMAIL_Admission_Email";
                public const string ADMISSION_EMAIL_PASSWORD = "EMAIL_Admission_Email_Password";
                public const string ADMISSION_EMAIL_SMTP_HOST = "EMAIL_Admission_Email_Smtp_Host";
                public const string ADMISSION_EMAIL_SMTP_PORT = "EMAIL_Admission_Email_Smtp_Port";
                public const string DOCUMENTARY_PROCEDURE_EMAIL = "EMAIL_Documentary_Procedure_Email";
                public const string DOCUMENTARY_PROCEDURE_EMAIL_PASSWORD = "EMAIL_Documentary_Procedure_Email_Password";
                public const string DOCUMENTARY_PROCEDURE_EMAIL_SMTP_HOST = "EMAIL_Documentary_Procedure_Email_Smtp_Host";
                public const string DOCUMENTARY_PROCEDURE_EMAIL_SMTP_PORT = "EMAIL_Documentary_Procedure_Email_Smtp_Port";
                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { EMAIL_MULTIPLE_ENABLED , "false"},
                    { GENERAL_EMAIL , ConstantHelpers.Institution.SupportEmail.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmail[ConstantHelpers.GENERAL.Institution.Value] : "" },
                    { GENERAL_EMAIL_PASSWORD , ConstantHelpers.Institution.SupportEmailPassword.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmailPassword[ConstantHelpers.GENERAL.Institution.Value] : ""  },
                    { GENERAL_EMAIL_SMTP_HOST , "smtp.gmail.com"},
                    { GENERAL_EMAIL_SMTP_PORT , "587"},
                    { INTRANET_EMAIL , ConstantHelpers.Institution.SupportEmail.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmail[ConstantHelpers.GENERAL.Institution.Value] : "" },
                    { INTRANET_EMAIL_PASSWORD , ConstantHelpers.Institution.SupportEmailPassword.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmailPassword[ConstantHelpers.GENERAL.Institution.Value] : ""  },
                    { INTRANET_EMAIL_SMTP_HOST , "smtp.gmail.com"},
                    { INTRANET_EMAIL_SMTP_PORT , "587"},
                    { JOBEXCHANGE_EMAIL , ConstantHelpers.Institution.SupportEmail.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmail[ConstantHelpers.GENERAL.Institution.Value] : "" },
                    { JOBEXCHANGE_EMAIL_PASSWORD , ConstantHelpers.Institution.SupportEmailPassword.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmailPassword[ConstantHelpers.GENERAL.Institution.Value] : ""  },
                    { JOBEXCHANGE_EMAIL_SMTP_HOST , "smtp.gmail.com"},
                    { JOBEXCHANGE_EMAIL_SMTP_PORT , "587"},
                    { LAURASSIA_EMAIL , ConstantHelpers.Institution.SupportEmail.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmail[ConstantHelpers.GENERAL.Institution.Value] : "" },
                    { LAURASSIA_EMAIL_PASSWORD , ConstantHelpers.Institution.SupportEmailPassword.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmailPassword[ConstantHelpers.GENERAL.Institution.Value] : ""  },
                    { LAURASSIA_EMAIL_SMTP_HOST , "smtp.gmail.com"},
                    { LAURASSIA_EMAIL_SMTP_PORT , "587"},
                    { ADMISSION_EMAIL , ConstantHelpers.Institution.SupportEmail.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmail[ConstantHelpers.GENERAL.Institution.Value] : "" },
                    { ADMISSION_EMAIL_PASSWORD , ConstantHelpers.Institution.SupportEmailPassword.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmailPassword[ConstantHelpers.GENERAL.Institution.Value] : ""  },
                    { ADMISSION_EMAIL_SMTP_HOST , "smtp.gmail.com"},
                    { ADMISSION_EMAIL_SMTP_PORT , "587"},
                    { DOCUMENTARY_PROCEDURE_EMAIL , ConstantHelpers.Institution.SupportEmail.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmail[ConstantHelpers.GENERAL.Institution.Value] : "" },
                    { DOCUMENTARY_PROCEDURE_EMAIL_PASSWORD , ConstantHelpers.Institution.SupportEmailPassword.ContainsKey(ConstantHelpers.GENERAL.Institution.Value) ? ConstantHelpers.Institution.SupportEmailPassword[ConstantHelpers.GENERAL.Institution.Value] : ""  },
                    { DOCUMENTARY_PROCEDURE_EMAIL_SMTP_HOST , "smtp.gmail.com"},
                    { DOCUMENTARY_PROCEDURE_EMAIL_SMTP_PORT , "587"}
                };
            }
            public static class Authorization
            {
                public const string AUTHORIZATED = "0";
                public const string NO_AUTHORIZATED = "1";
                public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
                {
                    { AUTHORIZATED, "Autorizado"},
                    { NO_AUTHORIZATED, "No autorizado" }
                };
            }
            public static class Validity
            {
                public const string VIGENTE = "0";
                public const string NO_VIGENTE = "1";
                public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
                {
                    { VIGENTE, "Vigente"},
                    { NO_VIGENTE, "No vigente" }
                };
            }
            public static class ManagementType
            {
                public const string PUBLIC = "0";
                public const string PRIVATE = "1";
                public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
                {
                    { PUBLIC, "Público"},
                    { PRIVATE, "Privado" }
                };
            }
            public static class ConstitutionType
            {
                public const string AUTHORIZATED = "0";
                public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
                {
                    { AUTHORIZATED, "Autorizado"},
                };
            }

            public static class RecordFormat
            {
                public const string CERTIFICATEOFSTUDIES = "RF_Certificate_Of_Studies";
                public const string FIRSTENROLLMENT = "RF_First_Enrollment";
                public const string REPORTCARD = "RF_Report_Card";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { CERTIFICATEOFSTUDIES , "0"},
                    { FIRSTENROLLMENT , "0"},
                    { REPORTCARD , "0"},
                };
            }

            public static class IntranetManagement
            {
                public const string SURVEY_ENFORCE_REQUIRED = "INT_Survey_Enforce_Required";
                public const string MIN_SUBSTITUTE_EXAMEN = "INT_Min_Avg_Substitute_Exam";
                public const string MIN_AVG_DEFERRED_EXAM = "INT_Min_AVG_Deferred_Exam";

                public const string SUBSTITUTE_EXAMEN_EVALUATION_TYPE = "INT_Substitute_Exam_Evaluation_Type";
                public const string EVALUATION_TYPE_GRADE_RECOVERY = "INT_Evaluation_Type_Grade_Recovery";
                public const string GRADE_RECOVERY_MIN_EVALUATION_GRADE = "INT_Grade_Recovery_Min_Evaluation_Grade";
                public const string GRADE_RECOVERY_ENABLED_TO_APPROVED = "INT_Grade_Recovery_Enabled_To_Approved";
                public const string GRADE_RECOVERY_MODALITY = "INT_Grade_Recovery_Modality";

                //public const string MAX_SUBSTITUTE_EXAMEN = "INT_Max_Avg_Substitute_Exam";
                //
                public const string COURSE_WITHDRWAL_CONCEPT = "INT_Course_Withdrawal_Concept";
                public const string CYCLE_WITHDRWAL_CONCEPT = "INT_Cycle_Withdrawal_Concept";
                public const string SUBSTITUTE_EXAM_CONCEPT = "INT_Substitute_Exam_Concept";
                //
                public const string COURSE_WITHDRWAL_CONCEPT_AUTO = "INT_Course_Withdrawal_Concept_Auto";
                public const string CYCLE_WITHDRWAL_CONCEPT_AUTO = "INT_Cycle_Withdrawal_Concept_Auto";
                public const string SUBSTITUTE_EXAM_CONCEPT_AUTO = "INT_Substitute_Exam_Concept_Auto";
                //PAYMENT
                public const string ENABLE_STUDENT_PAYMENT = "INT_Student_Payment_Enabled";
                //RECORDS
                //public const string RECORD_AUTOMATIC_PROCEDURE = "INT_RECORD_AUTOMATIC_PROCEDURE";
                public const string RECORD_RECTIFICATION_CHARGE = "INT_RECORD_RECTIFICATION_CHARGE";
                public const string EVALUATION_REPORT_WITH_REGISTER = "INT_EVALUATION_REPORT_WITH_REGISTER";
                //DOCUMENTS
                public const string DOCUMENT_MAIN_CAMPUS = "INT_DOCUMENT_MAIN_CAMPUS";
                public const string DOCUMENT_MAIN_OFFICE = "INT_DOCUMENT_MAIN_OFFICE";
                public const string DOCUMENT_OFFICE = "INT_DOCUMENT_OFFICE";
                public const string DOCUMENT_TECHNOLOGYOFFICE = "INT_DOCUMENT_TECHNOLOGYOFFICE";
                public const string DOCUMENT_SENDER = "INT_DOCUMENT_SENDER";
                public const string DOCUMENT_SENDER_COORDINATOR = "INT_DOCUMENT_SENDER_COORDINATOR";
                public const string DOCUMENT_ACADEMIC_CHARGE = "INT_DOCUMENT_ACADEMIC_CHARGE";
                public const string MERIT_ORDER_BY_ACADEMIC_YEAR = "INT_Merit_Order_By_Academic_Year";
                public const string MERIT_ORDER_GRADE_TYPE = "INT_Merit_Order_Grade_Type";
                //public const string MERIT_ORDER_PER_CURRICULUM = "INT_Merit_Order_Per_Curriculum";
                //EvaluationReport
                public const string EVALUATION_REPORT_HEADER = "INT_Evaluation_Report_Header";
                public const string EVALUATION_REPORT_SUBHEADER = "INT_Evaluation_Report_Subheader";
                public const string EVALUATION_REPORT_ACT_FORMAT = "INT_Evaluation_Report_Act_Format";
                public const string EVALUATION_REPORT_REGISTER_FORMAT = "INT_Evaluation_Report_Register_Format";
                public const string ENABLED_PARTIAL_EVALUATION_REPORT_REGISTER = "INT_Enabled_Partial_Evaluation_Report_Register";
                public const string ENABLED_AUXILIARY_EVALUATION_REPORT = "INT_Auxiliary_Evaluation_Report";

                public const string ENABLE_STUDENT_GRADE_CORRECTION_REQUEST = "INT_Enable_Student_Grade_Correction_Request";
                public const string STUDENT_GRADE_CORRECTION_REQUEST_MAX_DAYS = "INT_Student_Grade_Correction_Request_Max_Days";

                public const string ENABLED_EXTRAORDINARY_EVALUATION = "INT_Enabled_Extraordinary_Evaluation";
                public const string ENABLED_GRADE_RECOVERY = "INT_Enabled_Grade_Recovery";
                public const string ENABLED_SUBSTITUTE_EXAM = "INT_Enabled_Substitute_Exam";
                public const string ENABLED_DEFERRED_EXAM = "INT_Enabled_Deferred_Exam";

                public const string INT_LOGIN_BACKGROUND_IMAGE = "INT_Login_Background_Image";
                public const string GRADES_CAN_ONLY_PUBLISHED_BY_PRINCIPAL_TEACHER = "INT_GRADES_CAN_ONLY_PUBLISHED_BY_PRINCIPAL_TEACHER";

                public const string CAREER_DIRECTOR_GRADE_CORRECTION = "INT_Career_Director_Grade_Correction";
                public const string ACADEMIC_DEPARTMENT_GRADE_CORRECTION = "INT_Academic_Department_Grade_Correction";

                public const string ACADEMIC_RECORD_SIGNING = "INT_Academic_Record_Signing";

                public const string BOSS_POSITION_RECORD_SIGNING = "INT_Boss_Position_Record_Signing";


                public const string IP_ADDRESS_UPDATE_STUDENT_GRADE = "INT_Ip_Address_Update_Student_Grade";
                public const string PIN_HASH_UPDATE_STUDENT_GRADE = "INT_Pin_Hash_Update_Student_Grade";

                public const string EXTRAORDINARY_EVALUATION_TYPES_ENABLED = "INT_Extraordinary_Evaluation_Types_Enabled";

                public const string IMAGE_WATERMARK_RECORD = "INT_IMAGE_WATERMARK_RECORD";
                public const string EVALUATION_REPORT_FORMAT_DATE = "INT_Evaluation_Report_Format_Date";

                public const string MAX_COURSE_WITHDRAWAL = "INT_Max_Course_Withdrawal";

                public const string IMAGE_CERTIFICATE_SIGNATURE = "INT_Image_Certificate_Signature";

                //
                public const string ENABLED_SPECIAL_ABSENCE_PERCENTAGE = "INT_Enabled_Special_Absence_Percentage";
                public const string SPECIAL_ABSENCE_PERCENTAGE = "INT_Special_Absence_Percentage";
                public const string SPECIAL_ABSENCE_PERCENTAGE_DESCRIPTION = "INT_Special_Absence_Percentage_Description";

                public const string CONSTANCY_RECORD_HEADER_TYPE = "INT_Constancy_Record_Header_Type";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { SURVEY_ENFORCE_REQUIRED , "false" },
                    { ENABLE_STUDENT_PAYMENT , "false"},
                    { MIN_SUBSTITUTE_EXAMEN, "7"},
                    { MIN_AVG_DEFERRED_EXAM, "7"},
                    { SUBSTITUTE_EXAMEN_EVALUATION_TYPE, "1"},
                    { EVALUATION_REPORT_WITH_REGISTER,"true"},
                    { GRADE_RECOVERY_ENABLED_TO_APPROVED, "true" },
                    //{ MAX_SUBSTITUTE_EXAMEN, "11"},
                    //
                    { COURSE_WITHDRWAL_CONCEPT, Guid.Empty.ToString()},
                    { CYCLE_WITHDRWAL_CONCEPT, Guid.Empty.ToString()},
                    { SUBSTITUTE_EXAM_CONCEPT, Guid.Empty.ToString()},
                    { EVALUATION_TYPE_GRADE_RECOVERY, Guid.Empty.ToString() },
                    { GRADE_RECOVERY_MIN_EVALUATION_GRADE, "0"},
                    //
                    { COURSE_WITHDRWAL_CONCEPT_AUTO, "false"},
                    { CYCLE_WITHDRWAL_CONCEPT_AUTO, "false"},
                    { SUBSTITUTE_EXAM_CONCEPT_AUTO, "false"},
                    ////Records
                    //{ RECORD_AUTOMATIC_PROCEDURE, "false"},
                    { RECORD_RECTIFICATION_CHARGE, "false"},
                    //Documentos
                    { DOCUMENT_MAIN_CAMPUS, ""},
                    { DOCUMENT_MAIN_OFFICE, ""},
                    { DOCUMENT_OFFICE, ""},
                    { DOCUMENT_TECHNOLOGYOFFICE, "OFICINA DE TECNOLOGÍA INFORMÁTICA"},
                    { DOCUMENT_SENDER, ""},
                    { DOCUMENT_SENDER_COORDINATOR, ""},
                    { DOCUMENT_ACADEMIC_CHARGE, "Departamento Académico"},
                    { MERIT_ORDER_BY_ACADEMIC_YEAR, "false" },
                    { MERIT_ORDER_GRADE_TYPE, "1" },
                    //{ MERIT_ORDER_PER_CURRICULUM, "false" }
                    //EvaluationReport
                    { EVALUATION_REPORT_HEADER, "VICE RECTORADO ACADÉMICO" },
                    { EVALUATION_REPORT_SUBHEADER, "OFICINA DE REGISTRO Y ARCHIVO ACADÉMICO" },
                    { EVALUATION_REPORT_ACT_FORMAT, ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_1.ToString() },
                    { EVALUATION_REPORT_REGISTER_FORMAT, ConstantHelpers.Intranet.EVALUATION_REPORT_FORMAT.ACT_FORMAT.FORMAT_1.ToString() },
                    { ENABLED_PARTIAL_EVALUATION_REPORT_REGISTER, "false" },
                    { ENABLED_AUXILIARY_EVALUATION_REPORT, "false" },

                    { ENABLE_STUDENT_GRADE_CORRECTION_REQUEST, "false" },
                    { STUDENT_GRADE_CORRECTION_REQUEST_MAX_DAYS, "3" },

                    { ENABLED_EXTRAORDINARY_EVALUATION, "true" },
                    { ENABLED_GRADE_RECOVERY, "true" },
                    { ENABLED_SUBSTITUTE_EXAM, "true" },
                    { ENABLED_DEFERRED_EXAM, "true" },

                    { INT_LOGIN_BACKGROUND_IMAGE, "" },
                    { GRADES_CAN_ONLY_PUBLISHED_BY_PRINCIPAL_TEACHER, "false" },
                    { GRADE_RECOVERY_MODALITY, ConstantHelpers.GRADE_RECOVERY_EXAM_MODALITY.HIGHEST_GRADE.ToString() },

                    { CAREER_DIRECTOR_GRADE_CORRECTION, "false" },
                    { ACADEMIC_DEPARTMENT_GRADE_CORRECTION, "false" },

                    { ACADEMIC_RECORD_SIGNING, ""},

                    { IP_ADDRESS_UPDATE_STUDENT_GRADE, ""},
                    { PIN_HASH_UPDATE_STUDENT_GRADE, ""},

                    { IMAGE_WATERMARK_RECORD, ""},

                    { EXTRAORDINARY_EVALUATION_TYPES_ENABLED, "["+  $"{Intranet.ExtraordinaryEvaluation.EXTRAORDINARY},{Intranet.ExtraordinaryEvaluation.RE_EVALAUTION}" +"]"},

                    { EVALUATION_REPORT_FORMAT_DATE, "3"}, //1 CreatedAt, 2 ReceptionDate, 3 LastGradeDate

                    { MAX_COURSE_WITHDRAWAL , "" },
                    { IMAGE_CERTIFICATE_SIGNATURE, "" },

                    { ENABLED_SPECIAL_ABSENCE_PERCENTAGE, "false" },
                    { SPECIAL_ABSENCE_PERCENTAGE, "" },
                    { SPECIAL_ABSENCE_PERCENTAGE_DESCRIPTION, "" },

                    { CONSTANCY_RECORD_HEADER_TYPE, "1" },

                    { BOSS_POSITION_RECORD_SIGNING, "" },

                };
            }

            public static class Enrollment
            {
                public const string IRREGULAR_DISAPPROVED_COURSES_LIMIT = "ENR_Irregular_disapproved_courses_limit";
                public const string ADMISSION_TYPE = "ENR_Admission_type";
                public const string MINUTES_BETWEEN_SHIFTS = "ENR_Minutes_Between_Shifts";
                public const string REQUIRE_STUDENT_INFORMATION = "ENR_Require_Student_Information";
                public const string REQUIRE_STUDENT_INFORMATION_UPDATE = "ENR_Require_Student_Information_Update";
                public const string REQUIRE_ENROLLMENT_RECEPTION = "ENR_Require_Enrollment_Reception";
                public const string RESERVATION_TIME_LIMIT = "ENR_Reservation_Time_Limit";
                public const string RESERVATION_RENEWABLE_PER_SEMESTER = "ENR_Reservation_Renewable_Per_Semester";
                public const string EXTRA_CREDITS_MODALITY = "ENR_Extra_Credits_Modality";
                public const string UNBEATEN_STUDENTS_CREDITS_BY_RANGE = "ENR_Unbeaten_Students_Credits_By_Range";
                public const string UNBEATEN_STUDENT_CREDITS = "ENR_Unbeaten_Student_Credits";
                public const string ADMISSION_ENROLLMENT_PROCEDURE = "ENR_Admission_Enrollment_Procedure";
                public const string REGULAR_ENROLLMENT_PROCEDURE = "ENR_Regular_Enrollment_Procedure";
                public const string EXONERATE_REGULAR_STUDENTS_FROM_PAYMENT = "ENR_Exonerate_Regular_Students_From_Payment";
                public const string UNBEATEN_STUDENT_ENROLLMENT_PROCEDURE = "ENR_Unbeaten_Student_Enrollment_Procedure";
                public const string ENABLE_MULTIPLE_GRADES_EQUIVALENCE = "ENR_Enable_Multiple_Grades_Equivalence";
                public const string ENABLE_DISAPPROVED_COURSE_EQUIVALENCE = "ENR_Disapproved_Course_Equivalence";
                public const string DISAPROVED_COURSE_MODALITY = "ENR_Disaproved_Course_Modality";
                public const string DISAPROVED_COURSE_PROCEDURE = "ENR_Disaproved_Course_Procedure";
                public const string DISAPROVED_COURSE_CREDIT_COST = "ENR_Disaproved_Course_Credit_Cost";
                public const string DISAPROVED_COURSE_CURRICULUM_CHANGE_MODALITY = "ENR_Disaproved_Course_Curriculum_Change_Modality";
                public const string DISAPROVED_COURSE_CURRICULUM_CHANGE_PROCEDURE = "ENR_Disaproved_Course_Curriculum_Change_Procedure";
                public const string DISAPROVED_COURSE_CURRICULUM_CHANGE_CREDIT_COST = "ENR_Disaproved_Course_Curriculum_Change_Credit_Cost";
                public const string SPECIAL_ENROLLMENT_MODALITY = "ENR_Special_Enrollment_Modality";
                public const string SPECIAL_ENROLLMENT_PROCEDURE = "ENR_Special_Enrollment_Procedure";
                public const string SPECIAL_ENROLLMENT_CREDIT_COST = "ENR_Special_Enrollment_Credit_Cost";
                //public const string EXTEMPORANEOUS_ENROLLMENT_SURCHARGE = "ENR_Extemporaneous_Enrollment_Surcharge";
                public const string EXTEMPORANEOUS_ENROLLMENT_MODALITY = "ENR_Extemporaneous_Enrollment_Modality";
                public const string EXTEMPORANEOUS_ENROLLMENT_SURCHARGE_PROCEDURE = "ENR_Extemporaneous_Enrollment_Surcharge_Procedure";
                public const string PEDAGOGICAL_HOUR_TIME = "ENR_Pedagogical_Hour_Time";
                public const string UNBEATEN_STUDENT_WEIGHTED_AVERAGE = "ENR_Unbeaten_Student_Weighted_Average";
                public const string IRREGULAR_STUDENT_TERM_LIMIT = "ENR_Irregular_Student_Irregular_Limit";
                public const string ACADEMIC_YEAR_DISPERSION = "ENR_Academic_Year_Dispersion";
                public const string ACADEMIC_YEAR_DISPERSION_ADMIN = "ENR_Academic_Year_Dispersion_Admin";
                public const string TERM_CREDITS_MODALITY = "ENR_Term_Credits_Modality";
                public const string REGULAR_MAXIMUM_CREDITS = "ENR_Regular_Maximum_Credits";
                public const string ENROLLMENT_PAYMENT_METHOD = "ENR_Enrollment_Payment_Method";
                public const string RESERVATION_PROCEDURE = "ENR_Reservation_Procedure";
                public const string ENROLLMENT_TURN_ONLY_FOR_REGULAR = "ENR_Enrollment_Turn_Only_For_Regular";
                //public const string SECOND_TIME_COURSE_MODALITY = "ENR_Second_Time_Course_Modality";
                //public const string SECOND_TIME_COURSE_TOTAL_COST_CONCEPT = "ENR_Second_Time_Course_Total_Cost_Concept";
                //public const string SECOND_TIME_COURSE_CREDIT_COST_CONCEPT = "ENR_Second_Time_Course_Credit_Cost_Concept";
                //public const string THIRD_TIME_COURSE_MODALITY = "ENR_Third_Time_Course_Modality";
                //public const string THIRD_TIME_COURSE_TOTAL_COST_CONCEPT = "ENR_Third_Time_Course_Total_Cost_Concept";
                //public const string THIRD_TIME_COURSE_CREDIT_COST_CONCEPT = "ENR_Third_Time_Course_Credit_Cost_Concept";
                //public const string FOURTH_TIME_COURSE_MODALITY = "ENR_Fourth_Time_Course_Modality";
                //public const string FOURTH_TIME_COURSE_TOTAL_COST_CONCEPT = "ENR_Fourth_Time_Course_Total_Cost_Concept";
                //public const string FOURTH_TIME_COURSE_CREDIT_COST_CONCEPT = "ENR_Fourth_Time_Course_Credit_Cost_Concept";
                //public const string ENABLE_PARALLEL_COURSES = "ENR_Enable_Parallel_Courses";
                public const string FIRST_PARALLEL_COURSE_ACADEMIC_YEAR = "ENR_First_Parallel_Course_Academic_Year";
                public const string FIRST_PARALLEL_COURSE_QUANTITY = "ENR_First_Parallel_Course_Quantity";
                public const string SECOND_PARALLEL_COURSE_ACADEMIC_YEAR = "ENR_Second_Parallel_Course_Academic_Year";
                public const string SECOND_PARALLEL_COURSE_QUANTITY = "ENR_Second_Parallel_Course_Quantity";
                /**MANAGEABLE_CODE**/
                public const string NEW_STUDENT_CODE_FORMAT = "ENR_New_Student_Code_Format";
                public const string NEW_STUDENT_CODE_USE_DOCUMENT = "ENR_New_Student_Code_Use_Document";

                public const string ENABLE_CHANNEL_POSTULANT_INSCRIPTION = "ENR_Enable_Channel_Postulant_Inscription";
                public const string DIRECTED_COURSE_COST_CONCEPT = "ENR_Directed_Course_Cost_Concept";
                public const string REENTRY_COST_CONCEPT = "ENR_Reentry_Cost_Concept";
                public const string ENABLE_STUDENT_GROUP_SELECTION = "ENR_Enable_Student_Group_Selection";
                public const string EXEMPT_FIRST_PLACES_FROM_PAYMENTS = "ENR_Exempt_First_Places_From_Payments";
                public const string PAYMENT_EXEMPTION_TYPE = "ENR_Payment_Exemption_Type";
                public const string FIRST_PLACES_QUANTITY = "ENR_First_Places_Quantity";
                public const string ENABLE_CREDITS_FOR_LOW_GRADE_STUDENTS = "ENR_Enable_Credits_For_Low_Grade_Students";
                public const string LOW_GRADE_STUDENTS_MAXIMUM_GRADE = "ENR_Low_Grade_Students_Maximum_Grade";
                public const string LOW_GRADE_STUDENTS_CREDITS = "ENR_Low_Grade_Students_Credits";
                public const string PRE_ENROLLMENT_SURVEY = "ENR_Pre_Enrollment_Survey";
                public const string REQUIRE_ALL_COURSES_FOR_ENTRANTS = "ENR_Require_All_Courses_For_Entrants";
                public const string VALIDATE_ONLY_CURRENT_PAYMENTS_ENROLLMENT = "ENR_Validate_Only_Current_Payments_Enrollment";
                public const string ENROLLMENT_REPORT_HEADER_TEXT = "ENR_Enrollment_Report_Header_Text";
                public const string ENROLLMENT_REPORT_SUBHEADER_TEXT = "ENR_Enrollment_Report_Subheader_Text";
                public const string ENROLLMENT_REPORT_FOOTER_TEXT = "ENR_Enrollment_Report_Footer_Text";

                public const string ENROLLMENT_PROFORMA_TITLE_TEXT = "ENR_Enrollment_Proforma_Title_Text";
                public const string ENROLLMENT_PROFORMA_FOOTER_TEXT = "ENR_Enrollment_Proforma_Footer_Text";

                public const string EXONERATED_COURSE_ENROLLMENT_START_DATE = "ENR_Exonerated_Course_Enrollment_Start_Date";
                public const string EXONERATED_COURSE_ENROLLMENT_END_DATE = "ENR_Exonerated_Course_Enrollment_End_Date";
                public const string EXONERATED_COURSE_ENROLLMENT_AVERAGE_GRADE = "ENR_Exonerated_Course_Enrollment_Average_Grade";
                public const string EXONERATED_COURSE_CONCEPT = "ENR_Exonerated_Course_Concept";
                public const string REQUIRE_ENROLLMENT_FOR_RESERVATION = "ENR_Require_Enrollment_For_Reservation";
                public const string LIMIT_ENROLLMENT_TO_STUDENT_CAMPUS = "ENR_Limit_Enrollment_To_Student_Campus";

                public const string ENABLE_ADDITIONAL_PAYMENT_FOR_EXTRA_ACADEMIC_YEARS = "ENR_Enable_Additional_Payment_For_Extra_Academic_Years";
                public const string ADDITIONAL_PAYMENT_FOR_EXTRA_ACADEMIC_YEARS_CONCEPT = "ENR_Additional_Payment_For_Extra_Academic_Years_Concept";
                public const string EXTRA_ACADEMIC_YEARS_GRACE_PERIOD = "ENR_Extra_Academic_Years_Grace_Period";

                public const string ENABLE_ENROLLMENT_FEES = "ENR_Enable_Enrollment_Fees";
                public const string ENABLE_ENROLLMENT_FEES_BATCH_GENERATION = "ENR_Enable_Enrollment_Fees_Batch_Generation";

                public const string ALLOW_STUDENT_ENROLLMENT_RESET = "ENR_Allow_Student_Enrollment_Reset";
                public const string INCLUDE_CREDITS_TO_ENROLL_IN_ACADEMIC_YEAR_CALCULATION = "ENR_Include_Credits_To_Enroll_In_Academic_Year_Calculation";

                public const string LOGIN_BACKGROUND_IMAGE = "ENR_Login_Background_Image";
                public const string SUMMER_COURSE_COUNTS_TRY = "ENR_Summer_Course_Counts_Try";

                public const string ENABLE_STUDENT_STATUS_BY_GRADE = "ENR_Enable_Student_Status_By_Grade";
                public const string UNBEATEN_STUDENT_LESS_THAN_GRADE = "ENR_Unbeaten_Student_Less_Than_Grade";
                public const string HIGH_PERFORMANCE_STUDENT_MIN_GRADE = "ENR_High_Performance_Student_Min_Grade";
                public const string REGULAR_STUDENT_MIN_GRADE = "ENR_Regular_Student_Min_Grade";
                public const string REPEATER_STUDENT_LESS_THAN_GRADE = "ENR_Repeater_Student_Less_Than_Grade";
                public const string SANCTIONED_STUDENT_TERMS_TO_STUDY = "ENR_Sanctioned_Student_Terms_To_Study";

                public const string REQUIRED_STUDENT_PORTFOLIO = "ENR_Required_Student_Portfolio";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { IRREGULAR_DISAPPROVED_COURSES_LIMIT, "2" },
                    { ADMISSION_TYPE, "1" },
                    { MINUTES_BETWEEN_SHIFTS, "30" },
                    { REQUIRE_STUDENT_INFORMATION, "false" },
                    { REQUIRE_STUDENT_INFORMATION_UPDATE, "false" },
                    { REQUIRE_ENROLLMENT_RECEPTION, "false" },
                    { RESERVATION_TIME_LIMIT, "3" },
                    { RESERVATION_RENEWABLE_PER_SEMESTER, "false" },
                    { EXTRA_CREDITS_MODALITY, "1" },
                    { UNBEATEN_STUDENTS_CREDITS_BY_RANGE, "false" },
                    { UNBEATEN_STUDENT_CREDITS, "25" },
                    { ADMISSION_ENROLLMENT_PROCEDURE, "" },
                    { REGULAR_ENROLLMENT_PROCEDURE, "" },
                    { EXONERATE_REGULAR_STUDENTS_FROM_PAYMENT, "true" },
                    { UNBEATEN_STUDENT_ENROLLMENT_PROCEDURE, "" },
                    { ENABLE_MULTIPLE_GRADES_EQUIVALENCE, "false" },
                    { ENABLE_DISAPPROVED_COURSE_EQUIVALENCE, "false" },
                    { DISAPROVED_COURSE_MODALITY, "1" },
                    { DISAPROVED_COURSE_PROCEDURE, "" },
                    { DISAPROVED_COURSE_CREDIT_COST, "0" },
                    { DISAPROVED_COURSE_CURRICULUM_CHANGE_MODALITY, "1" },
                    { DISAPROVED_COURSE_CURRICULUM_CHANGE_PROCEDURE, "" },
                    { DISAPROVED_COURSE_CURRICULUM_CHANGE_CREDIT_COST, "0" },
                    { SPECIAL_ENROLLMENT_MODALITY, "1" },
                    { SPECIAL_ENROLLMENT_PROCEDURE, "" },
                    { SPECIAL_ENROLLMENT_CREDIT_COST, "0" },
                    { EXTEMPORANEOUS_ENROLLMENT_MODALITY, "1" },
                    { EXTEMPORANEOUS_ENROLLMENT_SURCHARGE_PROCEDURE, "" },
                    { PEDAGOGICAL_HOUR_TIME, "50" },
                    { UNBEATEN_STUDENT_WEIGHTED_AVERAGE, "13" },
                    { IRREGULAR_STUDENT_TERM_LIMIT, "3" },
                    { ACADEMIC_YEAR_DISPERSION, "3" },
                    { ACADEMIC_YEAR_DISPERSION_ADMIN, "3" },
                    { TERM_CREDITS_MODALITY, "1" },
                    { REGULAR_MAXIMUM_CREDITS, "18" },
                    { ENROLLMENT_PAYMENT_METHOD, "1" },
                    { RESERVATION_PROCEDURE, ""},
                    { ENROLLMENT_TURN_ONLY_FOR_REGULAR, "false"},
                    //{ SECOND_TIME_COURSE_MODALITY, "1" },
                    //{ SECOND_TIME_COURSE_TOTAL_COST_CONCEPT, "" },
                    //{ SECOND_TIME_COURSE_CREDIT_COST_CONCEPT, "0" },
                    //{ THIRD_TIME_COURSE_MODALITY, "1" },
                    //{ THIRD_TIME_COURSE_TOTAL_COST_CONCEPT, "" },
                    //{ THIRD_TIME_COURSE_CREDIT_COST_CONCEPT, "0" },
                    //{ FOURTH_TIME_COURSE_MODALITY, "1" },
                    //{ FOURTH_TIME_COURSE_TOTAL_COST_CONCEPT, "" },
                    //{ FOURTH_TIME_COURSE_CREDIT_COST_CONCEPT, "0" },
                    //{ ENABLE_PARALLEL_COURSES, "true" },
                    { FIRST_PARALLEL_COURSE_ACADEMIC_YEAR, "0" },
                    { FIRST_PARALLEL_COURSE_QUANTITY, "0" },
                    { SECOND_PARALLEL_COURSE_ACADEMIC_YEAR, "0" },
                    { SECOND_PARALLEL_COURSE_QUANTITY, "0" },
                    /**MANAGEABLE_CODE**/
                    { NEW_STUDENT_CODE_FORMAT, "yyyypprrrrr" },
                    { ENABLE_CHANNEL_POSTULANT_INSCRIPTION, "false" },
                    { DIRECTED_COURSE_COST_CONCEPT, "" },
                    { REENTRY_COST_CONCEPT, "" },
                    { ENABLE_STUDENT_GROUP_SELECTION, "true" },
                    { EXEMPT_FIRST_PLACES_FROM_PAYMENTS, "false" },
                    { PAYMENT_EXEMPTION_TYPE, "1" },
                    { FIRST_PLACES_QUANTITY, "2" },
                    { ENABLE_CREDITS_FOR_LOW_GRADE_STUDENTS, "false" },
                    { LOW_GRADE_STUDENTS_MAXIMUM_GRADE, "10.4" },
                    { LOW_GRADE_STUDENTS_CREDITS, "12" },
                    { PRE_ENROLLMENT_SURVEY, "" },
                    { REQUIRE_ALL_COURSES_FOR_ENTRANTS, "false" },
                    { VALIDATE_ONLY_CURRENT_PAYMENTS_ENROLLMENT, "true" },
                    { ENROLLMENT_REPORT_HEADER_TEXT, "DIRECCIÓN DE SERVICIOS ACADÉMICOS" },
                    { ENROLLMENT_REPORT_SUBHEADER_TEXT, "UNIDAD DE REGISTROS ACADÉMICOS" },
                    { ENROLLMENT_REPORT_FOOTER_TEXT, "" },
                    { EXONERATED_COURSE_ENROLLMENT_START_DATE, "" },
                    { EXONERATED_COURSE_ENROLLMENT_END_DATE, "" },
                    { EXONERATED_COURSE_ENROLLMENT_AVERAGE_GRADE, "10.5" },
                    { EXONERATED_COURSE_CONCEPT, "" },
                    { REQUIRE_ENROLLMENT_FOR_RESERVATION, "true" },
                    { LIMIT_ENROLLMENT_TO_STUDENT_CAMPUS, "false" },

                    { ENABLE_ADDITIONAL_PAYMENT_FOR_EXTRA_ACADEMIC_YEARS, "false" },
                    { ADDITIONAL_PAYMENT_FOR_EXTRA_ACADEMIC_YEARS_CONCEPT, "" },
                    { EXTRA_ACADEMIC_YEARS_GRACE_PERIOD, "0" },

                    { ENABLE_ENROLLMENT_FEES, "false" },
                    { ENABLE_ENROLLMENT_FEES_BATCH_GENERATION, "false" },

                    { ENROLLMENT_PROFORMA_TITLE_TEXT, "PREFICHA DE MATRÍCULA" },
                    { ENROLLMENT_PROFORMA_FOOTER_TEXT, "" },

                    { ALLOW_STUDENT_ENROLLMENT_RESET, "false" },
                    { INCLUDE_CREDITS_TO_ENROLL_IN_ACADEMIC_YEAR_CALCULATION, "false" },

                    { LOGIN_BACKGROUND_IMAGE, "" },
                    { SUMMER_COURSE_COUNTS_TRY, "true" },

                    { ENABLE_STUDENT_STATUS_BY_GRADE, "false" },
                    { UNBEATEN_STUDENT_LESS_THAN_GRADE, "0" },
                    { HIGH_PERFORMANCE_STUDENT_MIN_GRADE, "0" },
                    { REGULAR_STUDENT_MIN_GRADE, "0" },
                    { REPEATER_STUDENT_LESS_THAN_GRADE, "0" },
                    { SANCTIONED_STUDENT_TERMS_TO_STUDY, "2" },
                    { REQUIRED_STUDENT_PORTFOLIO, "false" },
                    { NEW_STUDENT_CODE_USE_DOCUMENT , "false"}
                };
                public static class ExtraCreditModality
                {
                    public const int UNBEATEN_STUDENTS = 1;
                    public const int MERIT_ORDER = 2;
                }
            }

            public static class Payroll
            {
                //renta de quinta
                public const string UIT_IMPORT = "PAYROLL_uit_import";
                public const string UIT_7_PERCENT = "PAYROLL_uit_7_percent";
                public const string UIT_12_PERCENT = "PAYROLL_uit_12_percent";
                public const string UIT_20_PERCENT = "PAYROLL_uit_20_percent";
                public const string UIT_35_PERCENT = "PAYROLL_uit_35_percent";
                public const string UIT_45_PERCENT = "PAYROLL_uit_45_percent";
                public const string MINIMUM_SALARY = "PAYROLL_minimum_salary";


                //retencion CAS
                public const string CAS_RETENTION = "PAYROLL_cas_retention";
                public const string CAS_MINIMUM_SALARY = "PAYROLL_cas_minimum_salary";
                public const string CAS_ESSALUD_SALARY = "PAYROLL_cas_essalud_salary";

                //Estructura PDT 
                public const string PDT_STRUCTURE = "Payroll_pdt_structure";
                public const string AFFECTED_REMUNERATION = "PAYROLL_affected_remuneration";
                public const string NON_AFFECTED_REMUNERATION = "PAYROLL_non_affected_remuneration";
                public const string VACATIONAL_REMUNERATION = "PAYROLL_vacational_remuneraton";
                public const string FIFTH_RENT_TAX = "PAYROLL_fifth_rent_tax";
                public const string JUDICIAL_REMUNERATION = "PAYROLL_judicial_remuneration";
                public const string INVESTIGATION_REMUNERATION = "PAYROLL_investigation_remuneration";
                public const string SCHOLARSHIP_REMUNERATION = "PAYROLL_scholarship_remuneration";
                public const string AFP_COMISSION = "PAYROLL_afp_comission";
                public const string AFP_CONTRIBUTION = "PAYROLL_afp_contribution";
                public const string AFP_INSURANCE = "PAYROLL_afp_insurance";
                public const string OTHER_INCOMES = "PAYROLL_other_incomes";
                public const string BONUS_CHRISTMAS = "PAYROLL_bonus_christmas";
                public const string BONUS = "PAYROLL_bonus";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { UIT_IMPORT, "0" },
                    { UIT_7_PERCENT, "0" },
                    { UIT_12_PERCENT, "0" },
                    { UIT_20_PERCENT, "0" },
                    { UIT_35_PERCENT, "0" },
                    { UIT_45_PERCENT, "0" },
                    { MINIMUM_SALARY, "0" } ,
                    { CAS_RETENTION, "0" },
                    { CAS_MINIMUM_SALARY, "0" },
                    { CAS_ESSALUD_SALARY, "0" },

                    { PDT_STRUCTURE, Guid.Empty.ToString() },
                    { AFFECTED_REMUNERATION,"0" },
                    { NON_AFFECTED_REMUNERATION,"0" },
                    { VACATIONAL_REMUNERATION ,"0" },
                    { FIFTH_RENT_TAX ,"0" },
                    { JUDICIAL_REMUNERATION,"0" },
                    { INVESTIGATION_REMUNERATION,"0" },
                    { SCHOLARSHIP_REMUNERATION,"0" },
                    { AFP_COMISSION ,"0" },
                    { AFP_CONTRIBUTION ,"0" },
                    { AFP_INSURANCE ,"0" },
                    { OTHER_INCOMES ,"0" },
                    { BONUS_CHRISTMAS ,"0" },
                    { BONUS ,"0" },
            };
            }
            public static class Tutoring
            {
                public const string QUANTITY_TUTORING_SESSION = "Quantity_Tutoring_Session";
                public const string GREEN_PASS_PERCENTAGE = "Green_Pass_Percentage";
                public const string YELLOW_PASS_PERCENTAGE = "Yellow_Pass_Percentage";
                public const string QUANTITY_CURRENT_ACADEMIC_YEAR = "QUANTITY_CURRENT_ACADEMIC_YEAR";
                public const string TUTORING_PLAN_ADMIN = "TUTORING_PLAN_ADMIN";
                public const string TUTORING_WELFARE_INTEGRATED = "TUTORING_WELFARE_INTEGRATED";
                public const string PSYCHOLOGY_SUPPORT_OFFICE = "PSYCHOLOGY_SUPPORT_OFFICE";
                public const string TUTOR_TEACHER_WITHOUT_ACADEMICCHARGE = "TUTOR_TEACHER_WITHOUT_ACADEMICCHARGE";
                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { QUANTITY_TUTORING_SESSION, "3" },
                    { GREEN_PASS_PERCENTAGE, "70" },
                    { YELLOW_PASS_PERCENTAGE, "50" },
                    { QUANTITY_CURRENT_ACADEMIC_YEAR, "5" },
                    { TUTORING_PLAN_ADMIN, "false" },
                    { PSYCHOLOGY_SUPPORT_OFFICE, "" },
                    {TUTORING_WELFARE_INTEGRATED,"true" },
                    { TUTOR_TEACHER_WITHOUT_ACADEMICCHARGE , "false"}
                };
            }
            public static class TeacherManagement
            {
                public const string EVALUATIONS_BY_COMPETENCE = "TM_Evaluations_by_competence";
                public const string DISPLAY_AVERAGE_GRADE_BY_EVALUATION_TYPE = "TM_Display_Average_Grade_By_Evaluation_Type";
                public const string MINIMUM_GRADING_SCALE = "TM_MINIMUM_GRADING_SCALE";
                public const string MAXIMUM_GRADING_SCALE = "TM_MAXIMUM_GRADING_SCALE";
                public const string ALLOW_TEACHER_TIME_CROSSING = "TM_ALLOW_TEACHER_TIME_CROSSING";
                public const string EARLY_SHIFT_END = "TM_EARLY_SHIFT";
                public const string LATE_SHIFT_END = "TM_LATE_SHIFT";
                public const string NIGHT_SHIFT_END = "TM_NIGHT_SHIFT";
                public const string EVALUATIONS_BY_UNIT = "TM_EVALUATIONS_BY_UNIT";
                public const string ENABLE_BULK_SAVE_GRADES = "INT_Enable_Bulk_Save_Grades";
                public const string CONCEPT_PERFORMANCE_EVALUATION = "TM_CONCEPT_PERFORMANCE_EVALUATION";
                public const string PERFORMANCE_EVALUATION_REQUIRED = "TM_PERFORMANCE_EVALUATION_REQUIRED";
                public const string ENABLED_CURRICULUM_COMPETENCIE = "TM_ENABLED_CURRICULUM_COMPETENCIE";
                public const string CONFERENCE_ATTENDANCE_MANAGEMENT = "TM_CONFERENCE_ATTENDANCE_MANAGEMENT";
                public const string CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR = "TM_CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR";
                public const string ENABLED_SYLLABUS_VALIDATION = "TM_ENABLED_SYLLABUS_VALIDATION";
                public const string ENABLE_TEACHER_UPLOAD_NON_TEACHING_LOAD = "TM_ENABLE_TEACHER_UPLOAD_NON_TEACHING_LOAD";
                public const string TM_LOGIN_BACKGROUND_IMAGE = "TM_Login_Background_Image";
                public const string ENABLE_UPDATE_CLASS = "TM_Enable_Update_Class";
                public const string EVALUATIONS_BY_SECTION = "TM_Evaluations_By_Section";


                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { EVALUATIONS_BY_COMPETENCE, "true" },
                    { DISPLAY_AVERAGE_GRADE_BY_EVALUATION_TYPE, "false" },
                    { MINIMUM_GRADING_SCALE, "0" },
                    { MAXIMUM_GRADING_SCALE, "20" },
                    { ALLOW_TEACHER_TIME_CROSSING, "false" },
                    { EARLY_SHIFT_END, "12:00 PM"},
                    { LATE_SHIFT_END, "6:00 PM"},
                    { NIGHT_SHIFT_END, "12:00 AM"},
                    { EVALUATIONS_BY_UNIT, "true"},
                    { ENABLE_BULK_SAVE_GRADES,"false"},
                    { CONCEPT_PERFORMANCE_EVALUATION, Guid.Empty.ToString()},
                    { PERFORMANCE_EVALUATION_REQUIRED, "false"},
                    { ENABLED_CURRICULUM_COMPETENCIE , "true"},
                    { CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR , "0"},
                    { CONFERENCE_ATTENDANCE_MANAGEMENT , "false"},
                    { ENABLED_SYLLABUS_VALIDATION , "false"},
                    { ENABLE_TEACHER_UPLOAD_NON_TEACHING_LOAD , "true"},
                    { TM_LOGIN_BACKGROUND_IMAGE , ""},
                    { ENABLE_UPDATE_CLASS , "false"},
                    { EVALUATIONS_BY_SECTION , "false"},
                };
            }
            public static class DegreeManagement
            {
                public const string TUPA_BACHELOR = "GRAD_Tupa_Bachelor";
                public const string TUPA_TYPE_BACHELOR = "GRAD_Tupa_Type_Bachelor";
                public const string TUPA_PROFESSIONAL_DEGREE_SUPPORT_TESIS = "GRAD_Tupa_Professional_Degree_Support_Tesis";
                public const string TUPA_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM = "GRAD_Tupa_Professional_Degree_Suffiency_Exam";
                public const string TUPA_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE = "GRAD_Tupa_Professional_Degree_Professional_Experience";
                public const string CONCEPT_BACHELOR = "GRAD_Concept_Bachelor";
                public const string CONCEPT_TYPE_BACHELOR = "GRAD_Concept_Type_Bachelor";
                public const string CONCEPT_PROFESSIONAL_DEGREE_SUPPORT_TESIS = "GRAD_Concept_Professional_Degree_Support_Tesis";
                public const string CONCEPT_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM = "GRAD_Concept_Professional_Degree_Suffiency_Exam";
                public const string CONCEPT_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE = "GRAD_Concept_Professional_Degree_Professional_Experience";
                public const string METHOD_TYPE_REGISTRY_PATTERN_CREATION = "GRAD_Method_Type_Registry_Pattern_Creation";
                public const string GRAD_LOGIN_BACKGROUND_IMAGE = "GRAD_Login_Background_Image";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { TUPA_BACHELOR, "" },
                    { TUPA_TYPE_BACHELOR, "1" },
                    { TUPA_PROFESSIONAL_DEGREE_SUPPORT_TESIS, "" },
                    { TUPA_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM, "" },
                    { TUPA_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE, "" },
                    { CONCEPT_BACHELOR ,""},
                    { CONCEPT_TYPE_BACHELOR,"1"},
                    { CONCEPT_PROFESSIONAL_DEGREE_SUPPORT_TESIS, "" },
                    { CONCEPT_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM, "" },
                    { CONCEPT_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE, "" },
                     { METHOD_TYPE_REGISTRY_PATTERN_CREATION, "false" },
                     { GRAD_LOGIN_BACKGROUND_IMAGE, "" }
                };
            }
            public static class EconomicManagement
            {
                public const string CUT_DEPENDENCY = "ECO_CUT_Dependency";
                public const string PARTIAL_PAYMENT = "ECO_Partial_Payment";
                public const string ONLINE_PAYMENT = "ECO_Online_Payment";
                public const string MULTIPLE_PAYMENTS = "ECO_Multiple_Payments";
                public const string ENTITY_CODE = "ECO_Entity_Code";
                public const string ASSOCIATE_PROCEDURE_CONCEPT = "ECO_Associate_Procedure_Concept";
                public const string CAN_EDIT_TICKET = "ECO_Can_Edit_Ticket";
                public const string INTERNAL_SALE_DOCUMENT_TYPE = "ECO_Internal_Sale_Document_Type";
                public const string INCLUDE_SIGNATURE_IN_DOCOUMENTS = "ECO_Include_Signature_In_Documents";
                public const string SIGNATURE_FILE = "ECO_Signature_File";
                public const string VALIDATE_ONLY_ACTIVE_TERM_PAYMENTS = "ECO_Validate_Only_Active_Term_Payments";
                public const string BANK_FILE_DUMMIE_CONCEPT = "ECO_Bank_File_Dummie_Concept";

                public const string ECO_LOGIN_BACKGROUND_IMAGE = "ECO_Login_Background_Image";

                public const string INTERNAL_TICKET_TITLE = "ECO_Internal_Ticket_Title";

                public const string TICKET_HEADER = "ECO_Ticket_Header";
                public const string TICKET_FONT_SIZE = "ECO_Ticket_Font_Size";

                public const string ALLOW_DEPENDENCY_SELECTION_ON_SALE = "ECO_Allow_Dependency_Selection_On_Sale";

                public const string ENABLE_CONTASOFT_INTEGRATION = "ECO_Enable_Contasoft_Integration";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { CUT_DEPENDENCY, "" },
                    { PARTIAL_PAYMENT , "false"},
                    { ONLINE_PAYMENT, "false"},
                    { MULTIPLE_PAYMENTS, "false"},
                    { ENTITY_CODE, ""},
                    { ASSOCIATE_PROCEDURE_CONCEPT, "false"},
                    { CAN_EDIT_TICKET, "false"},
                    { INTERNAL_SALE_DOCUMENT_TYPE, "1"},
                    { INCLUDE_SIGNATURE_IN_DOCOUMENTS, "false"},
                    { SIGNATURE_FILE, ""},
                    { VALIDATE_ONLY_ACTIVE_TERM_PAYMENTS, "true"},
                    { BANK_FILE_DUMMIE_CONCEPT, ""},

                    { ECO_LOGIN_BACKGROUND_IMAGE, ""},
                    { INTERNAL_TICKET_TITLE, "RECIBO DE PAGO"},
                    { TICKET_HEADER, "TESORERÍA-CAJA"},
                    { TICKET_FONT_SIZE, "18"},

                    { ALLOW_DEPENDENCY_SELECTION_ON_SALE, "false"},
                    { ENABLE_CONTASOFT_INTEGRATION, "false"},
                };
            }
            public static class ScaleConfiguration
            {
                public const string LICENSE_MAX_DAYS = "SCALE_License_Max_Days";
                public const string LAW_30220 = "SCALE_Law_30220";
                public const string LICENSE_INSTITUTION_DATE = "SCALE_License_Institution_Date";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { LICENSE_MAX_DAYS, "20" },
                    { LAW_30220, "02/05/2021" },
                    { LICENSE_INSTITUTION_DATE, "01/01/2021" },
                };
            }

            public static class PreProfessionalPracticeConfiguraton
            {
                public const string ANNEX_URL = "PPP_Annex_File";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { ANNEX_URL, "" },
                };
            }

            public static class Server
            {
                public const string ENABLE_CUSTOM_SYSTEMS = "SERVER_Enable_Custom_Systems";
                public const string ENABLE_SLIDER = "SERVER_Enable_Slider";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { ENABLE_CUSTOM_SYSTEMS, "false" },
                    { ENABLE_SLIDER, "false" },
                };

            }

            public static class BachelorTypeConfiguration
            {
                public static class TUPA
                {
                    public const byte AUTOMATIC = 1;
                    public const byte BY_REQUEST = 2;
                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        { AUTOMATIC, "TUPA para bachiller automático" },
                        { BY_REQUEST, "TUPA para bachiller por solicitud" }
                    };
                };
                public static class CONCEPT
                {
                    public const byte AUTOMATIC = 1;
                    public const byte BY_REQUEST = 2;
                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        { AUTOMATIC, "Concepto para bachiller automático" },
                        { BY_REQUEST, "Concepto para bachiller por solicitud" }
                    };
                };
            }
            public static class DocumentaryProcedureManagement
            {
                public const string DERIVATION_CONFIGURATION = "DOCUMENTARY_PROC_Derivation_Configuration";
                public const string PAYMENT_CONFIGURATION = "DOCUMENTARY_PROC_Payment_Configuration";
                public const string DURATION_INTERNAL_PROCEDURE = "DURATION_INTERNAL_PROCEDURE";
                public const string DOCUMENT_RECEPTION_VIRTUAL = "DOCUMENT_RECEPTION_VIRTUAL";
                public const string ENABLE_TUPA = "ENABLE_TUPA";
                public const string ENABLE_SEARCH_STUDENT_EXTERNAL = "ENABLE_SEARCH_STUDENT_EXTERNAL";
                public const string DOCUMENT_RECEPTION_START_TIME = "DP_Document_Reception_Start_Time";
                public const string DOCUMENT_RECEPTION_END_TIME = "DP_Document_Reception_End_Time";
                public const string EMAIL_USERNAME = "DP_Email_Username";
                public const string EMAIL_PASSWORD = "DP_Email_Password";
                public const string EMAIL_SMTP_DOMAIN = "DP_Email_Smtp_Domain";
                public const string EMAIL_SMTP_PORT = "DP_Email_Smtp_Port";
                public const string EXTERNAL_USER_TERMS_AND_CONDITIONS = "DP_EXTERNAL_USER_TERMS_AND_CONDITIONS";
                public const string EXTERNAL_USER_REGISTER_MESSAGE = "DP_EXTERNAL_USER_REGISTER_MESSAGE";
                public const string TUPA_READONLY = "DP_TUPA_READONLY";

                public const string PROCEDURE_PAYMENT_TYPE = "DP_PROCEDURE_PAYMENT_TYPE";
                public const string PROCEDURE_PREVIOUS_PAYMENT_TYPE = "DP_PROCEDURE_PREVIOUS_PAYMENT_TYPE";

                public const string LOGIN_BACKGROUND_IMAGE = "DOCUMENTARY_PROC_Login_Background_Image";

                public const string PROCEDURE_MAX_FILE_SIZE_MB = "DOCUMENTARY_PROC_Procedure_Max_File_Size_Mb";

                public const string ONLY_MANUAL_TUPA_PROCEDURE = "DP_Only_Manual_Tupa_Procedure";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { DERIVATION_CONFIGURATION, "false" },
                    { PAYMENT_CONFIGURATION, "false" },
                    { DURATION_INTERNAL_PROCEDURE, "3" },
                    { DOCUMENT_RECEPTION_VIRTUAL, "false" },
                    { ENABLE_TUPA, "false" },
                    { ENABLE_SEARCH_STUDENT_EXTERNAL, "false" },
                    { DOCUMENT_RECEPTION_START_TIME, "8:00 AM" },
                    { DOCUMENT_RECEPTION_END_TIME, "7:00 PM" },
                    { EMAIL_USERNAME, ConstantHelpers.Institution.SupportEmail[ConstantHelpers.GENERAL.Institution.Value] },
                    { EMAIL_PASSWORD, ConstantHelpers.Institution.SupportEmailPassword[ConstantHelpers.GENERAL.Institution.Value] },
                    { EMAIL_SMTP_DOMAIN, "smtp.gmail.com" },
                    { EMAIL_SMTP_PORT, "587" },
                    { EXTERNAL_USER_TERMS_AND_CONDITIONS, "" },
                    { EXTERNAL_USER_REGISTER_MESSAGE, "" },
                    { TUPA_READONLY, "true" },

                    { PROCEDURE_PAYMENT_TYPE, ConstantHelpers.PROCEDURES.CONFIGURATION.PAYMENT_TYPE.LATER_PAYMENT.ToString() },
                    { PROCEDURE_PREVIOUS_PAYMENT_TYPE, "" },

                    { LOGIN_BACKGROUND_IMAGE, "" },
                    { PROCEDURE_MAX_FILE_SIZE_MB, "10" },

                    { ONLY_MANUAL_TUPA_PROCEDURE , "false" }
                };
            }

            public static class AdmissionManagement
            {
                public const string PAYMENT_BANK = "ADM_Payment_Bank";
                public const string BANK_CODE = "ADM_Bank_Code";
                public const string UNIVERSITY_CODE = "ADM_University_Code";
                public const string TYPE_OF_SCHOOL_PAYMENT = "ADM_Type_of_School_Payment";
                public const string VOCATIONAL_TEST_MODALITY_NONE_FLAG = "ADM_Vocational_Test_Modality_None_Flag";
                public const string VOCATIONAL_TEST_MODALITY_ORDINARY_FLAG = "ADM_Vocational_Test_Modality_Ordinary_Flag";
                public const string VOCATIONAL_TEST_MODALITY_EXTRAORDINARY_FLAG = "ADM_Vocational_Test_Modality_Extraordinary_Flag";
                public const string VOCATIONAL_TEST_VISIBILITY_FLAG = "ADM_Vocational_Test_Modality_Visibility_Flag";
                public const string VOCATIONAL_TEST_REQUIRED_FLAG = "ADM_Vocational_Test_Modality_Required_Flag";
                public const string VOCATIONAL_TEST_MODALITY_NONE_FLAG_REQUIRED = "ADM_Vocational_Test_Modality_None_Flag_Required";
                public const string VOCATIONAL_TEST_MODALITY_ORDINARY_FLAG_REQUIRED = "ADM_Vocational_Test_Modality_Ordinary_Flag_Required";
                public const string VOCATIONAL_TEST_MODALITY_EXTRAORDINARY_FLAG_REQUIRED = "ADM_Vocational_Test_Modality_Extraordinary_Flag_Required";
                //public const string POSTULANT_CODE_DOCUMENT_FLAG = "ADM_Postulant_Code_Document_Flag";
                //public const string POSTULANT_CODE_CAREER_CODE_FLAG = "ADM_Postulant_Code_Career_Code_Flag";
                //public const string POSTULANT_CODE_CORRELATIVE_DIGITS = "ADM_Postulant_Code_Correlative_Digits";
                public const string ADMISSION_TIEBREAKER_MODALITY = "ADM_Admission_Tiebreaker_Modality";
                public const string PROOF_OF_IMCOME_PICK_UP_TIME = "ADM_Proof_Of_Income_Pick_up_Time";
                public const string FOLDER_FOR_USERS_IMAGES = "ADM_Folder_For_Users_Images";
                public const string FOLDER_FOR_USERS_IMAGES_PATH = "ADM_Folder_For_Users_Images_Path";
                public const string FOLDER_FOR_FINGERPRINT_IMAGES_PATH = "ADM_Folder_For_Fingerprint_Images_Path";
                public const string FOLDER_FOR_USERS_IMAGES_EXTENSION = "ADM_Folder_For_Users_Images_Extension";
                public const string FOLDER_FOR_FINGERPRINT_IMAGES_EXTENSION = "ADM_Folder_For_Fingerprint_Images_Extension";
                public const string VALIDATE_ADMISSION_FOLDER = "ADM_Validate_Admission_Folder";
                public const string ENABLE_RANDOM_TEACHER_ASSIGNMENT = "ADM_Enable_Random_Teacher_Assignment";
                public const string ENABLE_POSTULANT_UPLOAD_DOCUMENT = "ADM_Enable_Postulant_Upload_Document";
                public const string POSTULANT_PICTURE = "ADM_Postulant_Picture_Flag";
                public const string POSTULANT_PICTURE_REQUIRED = "ADM_Postulant_Picture_Required";
                //public const string UNIVERSITY_PREPARATION_REQUIRED_FLAG = "ADM_University_Preparation_Required_Flag";
                public const string PREVIOUS_PAYMENT_TYPE = "ADM_Previous_Payment_Type";
                //constancia de admision
                public const string HEADER_ADMISSION_RECORD = "ADM_Header_Admission_Record";
                public const string SUBHEADER_ADMISSION_RECORD = "ADM_SubHeader_Admission_Record";
                public const string POSTULANT_CODE_FORMAT = "ADM_Postulant_Code_Format";
                public const string EXCONTEMPORARY_CONCEPT = "ADM_ExContemporary_Concept";
                public const string REINSCRIPTION_CONCEPT = "ADM_Reinscription_Concept";
                public const string PROSPECT_CONCEPT = "ADM_Prospect_Concept";
                public const string TERMS_AND_CONDITIONS = "ADM_TERMS_AND_CONDITIONS";
                public const string POSTULATION_CODE_USING_DNI = "ADM_POSTULATION_CODE_USING_DNI";
                public const string ORDINARY_ADMISSION_RESULTS_TYPE = "ADM_Ordinary_Admission_Results_Type";
                public const string ENABLE_STUDENT_INSCRIPTION = "ADM_Enable_Student_Inscription";
                public const string EXAMN_POSTULANT_ASSIGNMENT_TYPE = "ADM_Examn_Postulant_Assignment_Type";
                public const string ADMISSION_EXAM_ALLOW_CHANNEL_SELECTION = "ADM_Admission_Exam_Allow_Channel_Selection";

                public const string POSTULANT_PREVIOUS_DAYS_TO_SHOW_CLASSROOM = "ADM_Postulant_Previous_Days_To_Show_Classroom";
                public const string ADMIN_PREVIOUS_DAYS_TO_SHOW_CLASSROOM = "ADM_Admin_Previous_Days_To_Show_Classroom";

                public const string VALIDATE_CEPRE_LOAD = "ADM_Validate_Cepre_Load";
                public const string VALIDATE_GRADUATE_ADMISSION_TYPE = "ADM_Validate_Graduate_Admission_Type";

                public const string POSTULANT_INSCRIPTION_IMAGE_HEIGHT = "ADM_Postulant_Inscription_Image_Height";
                public const string POSTULANT_INSCRIPTION_IMAGE_WIDTH = "ADM_Postulant_Inscription_Image_Width";

                public const string SWORN_DECLARATION_NO_CRIMINAL_RECORD_DESCRIPTION = "ADM_Sworn_Declaration_No_Criminal_Record_Description";

                public const string ADM_LOGIN_BACKGROUND_IMAGE = "Admission_Login_Background_Image";
                public const string ADM_POSTULANT_HANDBOOK = "Admission_Postulant_Handbook";

                public const string INSCRIPTION_REQUIRE_ACADEMIC_PROGRAM = "ADM_Inscription_Require_Academic_Program";

                public const string SWORN_DECLARATION_FINGERPRINT_1 = "ADM_Sworn_Declaration_Fingerprint_1";
                public const string SWORN_DECLARATION_FINGERPRINT_2 = "ADM_Sworn_Declaration_Fingerprint_2";
                public const string SWORN_DECLARATION_OFFICE_TEXT = "ADM_Sworn_Declaration_Office_Text";
                public const string SWORN_DECLARATION_TITLE = "ADM_Sworn_Declaration_Title";

                public const string SWORN_DECLARATION_FORMAT = "ADM_Sworn_Declaration_Format";

                public const string MESSAGE_FOR_YOUNGER = "ADM_MESSAGE_FOR_YOUNGER";
                public const string PAYMENT_ORDER_TEXT = "ADM_Payment_Order_Text";
                public const string POSTULANTE_REQUIREMENTS_APPROVED_EMAIL_TEXT = "ADM_Postulant_Requirements_Approved_Email_Text";
                public const string POSTULANTE_REQUIREMENTS_OBSERVED_EMAIL_TEXT = "ADM_Postulant_Requirements_Observed_Email_Text";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { PAYMENT_BANK, "false" },
                    { BANK_CODE, "" },
                    { UNIVERSITY_CODE, "" },
                    { TYPE_OF_SCHOOL_PAYMENT, "false" },
                    { VOCATIONAL_TEST_MODALITY_NONE_FLAG, "false" },
                    { VOCATIONAL_TEST_MODALITY_ORDINARY_FLAG, "false" },
                    { VOCATIONAL_TEST_MODALITY_EXTRAORDINARY_FLAG, "false" },
                    //{ POSTULANT_CODE_DOCUMENT_FLAG, "false" },
                    //{ POSTULANT_CODE_CAREER_CODE_FLAG, "true" },
                    //{ POSTULANT_CODE_CORRELATIVE_DIGITS, "4" },
                    { VOCATIONAL_TEST_MODALITY_NONE_FLAG_REQUIRED, "false" },
                    { VOCATIONAL_TEST_MODALITY_ORDINARY_FLAG_REQUIRED, "false" },
                    { VOCATIONAL_TEST_MODALITY_EXTRAORDINARY_FLAG_REQUIRED, "false" },
                    { ADMISSION_TIEBREAKER_MODALITY, "1" },
                    { PROOF_OF_IMCOME_PICK_UP_TIME, "30" },
                    { FOLDER_FOR_USERS_IMAGES, "false" },
                    { FOLDER_FOR_USERS_IMAGES_PATH, "" },
                    { FOLDER_FOR_FINGERPRINT_IMAGES_PATH, "" },
                    { FOLDER_FOR_USERS_IMAGES_EXTENSION, ".jpg" },
                    { FOLDER_FOR_FINGERPRINT_IMAGES_EXTENSION, ".jpg" },
                    { VOCATIONAL_TEST_VISIBILITY_FLAG, "false" },
                    { VOCATIONAL_TEST_REQUIRED_FLAG, "false" },
                    { VALIDATE_ADMISSION_FOLDER, "false" },
                    { ENABLE_RANDOM_TEACHER_ASSIGNMENT, "true" },
                    { ENABLE_POSTULANT_UPLOAD_DOCUMENT, "false" },
                    { TERMS_AND_CONDITIONS, ""},
                    { POSTULANT_PICTURE, "false" },
                    { POSTULANT_PICTURE_REQUIRED, "false"},
                    { PREVIOUS_PAYMENT_TYPE, "0" },
                    //constancia
                    { HEADER_ADMISSION_RECORD, "" },
                    { SUBHEADER_ADMISSION_RECORD, "" },
                    { POSTULANT_CODE_FORMAT, "yyyypprrrrr" },
                    { EXCONTEMPORARY_CONCEPT, ""},
                    { REINSCRIPTION_CONCEPT, ""},
                    { POSTULATION_CODE_USING_DNI, "false"},
                    { ORDINARY_ADMISSION_RESULTS_TYPE, "1"},
                    { ENABLE_STUDENT_INSCRIPTION, "true"},
                    { ADMISSION_EXAM_ALLOW_CHANNEL_SELECTION, "false"},
                    { EXAMN_POSTULANT_ASSIGNMENT_TYPE, "1"},
                    { POSTULANT_PREVIOUS_DAYS_TO_SHOW_CLASSROOM, "1"},
                    { ADMIN_PREVIOUS_DAYS_TO_SHOW_CLASSROOM, "1"},
                    { VALIDATE_CEPRE_LOAD, "false"},
                    { VALIDATE_GRADUATE_ADMISSION_TYPE, "false"},

                    { POSTULANT_INSCRIPTION_IMAGE_HEIGHT, "300"},
                    { POSTULANT_INSCRIPTION_IMAGE_WIDTH, "300"},
                    { SWORN_DECLARATION_NO_CRIMINAL_RECORD_DESCRIPTION, "Declaro Bajo Juramento De Ley, NO HABER SIDO CONDENADO POR EL DELITO DE TERRORISMO O APOLOGÍA AL TERRORISMO EN CUALQUIERA DE SUS MODALIDADES (Ley N° 30220, Art 98°), para el Proceso de Admisión Primeros Puestos Secundaria."},

                    { ADM_LOGIN_BACKGROUND_IMAGE, ""},
                    { ADM_POSTULANT_HANDBOOK, ""},

                    { INSCRIPTION_REQUIRE_ACADEMIC_PROGRAM, "true"},

                    { SWORN_DECLARATION_FINGERPRINT_1, "Huella Día del Examen"},
                    { SWORN_DECLARATION_FINGERPRINT_2, "Huella Al Recoger Certificado de Ingreso"},
                    { SWORN_DECLARATION_OFFICE_TEXT, "OFICINA CENTRAL DE ADMISIÓN"},
                    { SWORN_DECLARATION_TITLE, "DIRECCIÓN DE ADMISIÓN"},

                    { SWORN_DECLARATION_FORMAT, "1"},

                    { MESSAGE_FOR_YOUNGER, ""},
                    { PAYMENT_ORDER_TEXT, ""},
                    { POSTULANTE_REQUIREMENTS_APPROVED_EMAIL_TEXT, ""},
                    { POSTULANTE_REQUIREMENTS_OBSERVED_EMAIL_TEXT, ""},
                };
            }
            public static class JobExchangeManagement
            {
                public const string MINIMUM_CYCLE = "JOB_EXCHANGE_Minimum_Cycle";
                public const string GRADUATEDSURVEYENABLED = "JOB_EXCHANGE_GraduatedSurvey";
                public const string UNIT_LOGO = "JOB_EXCHANGE_Unit_Logo";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { MINIMUM_CYCLE, "6" },
                    { GRADUATEDSURVEYENABLED, "false"},
                    { UNIT_LOGO, "" },
                };
            }
            public static class InstitutionalWelfareManagement
            {
                public const string STUDENT_INFORMATION_VISIBILITY = "INSTITUTIONAL_WELFARE_Student_Information_Visibility";
                public const string STUDENT_INFORMATION_REITERATIVE = "INSTITUTIONAL_WELFARE_Student_Information_Reiterative";
                public const string STUDENT_INFORMATION_ALLSTUDENT = "INSTITUTIONAL_WELFARE_Student_Information_AllStudent";
                public const string INSTITUTIONAL_WELFARE_SURVEY_VISIBILITY = "INSTITUTIONAL_WELFARE_Survey_Visibility";
                public const string MEDICAL_RECORD_VISIBILITY = "INSTITUTIONAL_WELFARE_Medical_Record_Visibility";
                public const bool INTEGRATION_TUTORING = true;
                public const string SUPPORT_OFFICE = "SUPPORT_OFFICE";
                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { STUDENT_INFORMATION_VISIBILITY, "false" },
                    { STUDENT_INFORMATION_REITERATIVE, "false" },
                    { STUDENT_INFORMATION_ALLSTUDENT, "false" },
                    { INSTITUTIONAL_WELFARE_SURVEY_VISIBILITY, "false" },
                    { MEDICAL_RECORD_VISIBILITY , "false"},
                    { SUPPORT_OFFICE , ""}
                };
            }
            public static class TransparencyPortalManagement
            {
                public const string ACADEMIC_REGULATION_TITLE = "TP_ACADEMIC_REGULATION_TITLE";
                public const string DIRECTIVE_TITLE = "TP_DIRECTIVE_TITLE";
                public const string SCHOLARSHIP_FOOTER = "TP_SCHOLARSHIP_FOOTER";
                public const string RESOLUTIVE_ACTS_INTEGRATED = "TP_RESOLUTIVE_ACTS_INTEGRATED";

                public const string CONTACT_INFORMATION = "TP_CONTACT_INFORMATION";

                public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
                {
                    { ACADEMIC_REGULATION_TITLE, "REGLAMENTO ACADÉMICO" },
                    { DIRECTIVE_TITLE, "DIRECTIVAS" },
                    { SCHOLARSHIP_FOOTER, "" },
                    { RESOLUTIVE_ACTS_INTEGRATED, "false" },
                    { CONTACT_INFORMATION, "" }
                };
            }
        }
        public static class Term
        {
            public static class CreditsModality
            {
                public const int MAXIMUM_CREDITS_BASED_ON_CURRICULUM = 1;
                public const int REGULAR_MAXIMUM_CREDITS = 2;
                public const int ACADEMIC_YEAR_CREDITS_CONFIGURATION = 3;
                public const int LAST_AVERAGE_GRADE_CREDITS = 4;
            }
            public static class RomanNumbers
            {
                public const int ZERO = 0;
                public const int FIRST = 1;
                public const int SECOND = 2;
                public const int THIRD = 3;
                public const int QUARTER = 4;
                public const int FIFTH = 5;
                public const int SIXTH = 6;
                public const int SEVENTH = 7;
                public const int EIGHTH = 8;
                public const int NINETH = 9;
                public const string ZERO_ROMAN = "00";
                public const string FIRST_ROMAN = "I";
                public const string SECOND_ROMAN = "II";
                public const string THIRD_ROMAN = "III";
                public const string QUARTER_ROMAN = "IV";
                public const string FIFTH_ROMAN = "V";
                public const string SIXTH_ROMAN = "VI";
                public const string SEVENTH_ROMAN = "VII";
                public const string EIGHTH_ROMAN = "VIII";
                public const string NINETH_ROMAN = "NIV";
                public static Dictionary<int, string> ROMAN_NUMERALS = new Dictionary<int, string>()
                {
                    {ZERO, ZERO_ROMAN },
                    {FIRST, FIRST_ROMAN },
                    {SECOND, SECOND_ROMAN },
                    {THIRD, THIRD_ROMAN },
                    {QUARTER, QUARTER_ROMAN },
                    {FIFTH, FIFTH_ROMAN },
                    {SIXTH, SIXTH_ROMAN },
                    {SEVENTH, SEVENTH_ROMAN },
                    {EIGHTH, EIGHTH_ROMAN },
                    {NINETH, NINETH_ROMAN }
                };
                public static Dictionary<string, int> ROMAN_NUMERALS_INVERT = new Dictionary<string, int>()
                {
                    {ZERO_ROMAN,ZERO },
                    {FIRST_ROMAN,FIRST },
                    {SECOND_ROMAN,SECOND},
                    {THIRD_ROMAN,THIRD },
                    {QUARTER_ROMAN,QUARTER},
                    {FIFTH_ROMAN,FIFTH},
                    {SIXTH_ROMAN,SIXTH},
                    {SEVENTH_ROMAN,SEVENTH},
                    {EIGHTH_ROMAN,EIGHTH},
                    {NINETH_ROMAN,NINETH}
                };
            }
        }
        /* ESCALAFON */
        public static class USER_LABOR_INFORMATION
        {
            public static class LABOR_TYPES
            {
                public const int TEACHER = 1;
                public const int ADMINISTRATIVE = 2;
                public const int OTHER = 3;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { TEACHER, "Docente" },
                    { ADMINISTRATIVE, "Administrativo" },
                    { OTHER, "Otro" }
                };
            };
            public static class TEACHER_CONDITIONS
            {
                public const int DOPPREDOUT = 1;
                public const int PENDING = 2;
                public const int NOMINATED = 3;
                public const int HIRED = 4;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { DOPPREDOUT, "De baja" },
                    { PENDING, "Pendiente" },
                    { NOMINATED, "Nombrado" },
                    { HIRED, "Contratado" },
                };
                public static Dictionary<int, string> PUBLIC_VALUES = new Dictionary<int, string>()
                {
                    { NOMINATED, "Nombrado" },
                    { HIRED, "Contratado" },
                };
            };
            public static class TEACHER_STATES
            {
                public const bool STATE = true;
                public const bool NOTSTATE = false;
                public static Dictionary<bool, string> VALUES = new Dictionary<bool, string>()
                {
                    { STATE, "Estatal" },
                    { NOTSTATE, "No Estatal" },
                };
            };
            public static class TEACHER_LEVEL_STUDIE
            {
                public const int PREGRADE = 1;
                public const int POSTGRADEMASTER = 2;
                public const int POSTGRADEDOCTORATE = 3;
                public const int POSTGRADESECONDSPECIALITY = 4;
                public const int POSTGRADEDIPLOMAT = 5;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PREGRADE, "Pregrado" },
                    { POSTGRADEMASTER, "Posgrado-Maestría" },
                    { POSTGRADEDOCTORATE, "Posgrado-Doctorado" },
                    { POSTGRADESECONDSPECIALITY, "Posgrado-Segunda Especialidad" },
                    { POSTGRADEDIPLOMAT, "Posgrado-Diplomado" },
                };
                public static Dictionary<int, int> HIERARCHICAL_ORDER = new Dictionary<int, int>
                {
                    { PREGRADE, 0 },
                    { POSTGRADEMASTER, 3 },
                    { POSTGRADEDOCTORATE, 4 },
                    { POSTGRADESECONDSPECIALITY, 2 },
                    { POSTGRADEDIPLOMAT, 1 },
                };
            };
        }
        public static class DRIVERS_LICENSE_TYPE
        {
            public const int AI = 1;
            public const int AIIA = 2;
            public const int AIIB = 3;
            public const int AIIIA = 4;
            public const int AIIIB = 5;
            public const int AIIIC = 6;
            public const int BI = 7;
            public const int BIIA = 8;
            public const int BIIB = 9;
            public const int BIIC = 10;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { AI , "A-I" },
                { AIIA , "A-IIa" },
                { AIIB , "A-IIb" },
                { AIIIA , "A-IIIA" },
                { AIIIB , "A-IIIb" },
                { AIIIC , "A-IIIc" },
                { BI , "B-I" },
                { BIIA , "B-IIa" },
                { BIIB , "B-IIb" },
                { BIIC , "B-IIc" }
            };
        }
        public static class RETIREMENT_SYSTEM
        {
            public const byte AFP = 1;
            public const byte ONP = 2;
            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { AFP, "AFP" },
                { ONP, "ONP" }
            };
        };
        public static class STUDENT_FAMILY
        {
            public static class RELATIONSHIPS
            {
                public const int FATHER = 1;
                public const int MOTHER = 2;
                public const int SON = 3;
                public const int DAUGHTER = 4;
                public const int OTHER = 5;
                public static Dictionary<int, string> TYPE = new Dictionary<int, string>()
                    {
                        { FATHER, "Padre" },
                        { MOTHER, "Madre" },
                        { SON, "Hijo" },
                        { DAUGHTER, "Hija" },
                        { OTHER, "Otros" }
                    };
            }
            public static class CERTIFICATES
            {
                public const int COMPLETE_PRIMARY = 1;
                public const int INCOMPLETE_PRIMARY = 2;
                public const int COMPLETE_SECONDARY = 3;
                public const int INCOMPLETE_SECONDARY = 4;
                public const int SUPERIOR_COMPLETE_TECHNIQUE = 5;
                public const int SUPERIOR_INCOMPLETE_TECHNIQUE = 6;
                public const int COMPLETE_UNIVERSITY = 7;
                public const int INCOMPLETE_UNIVERSITY = 8;
                public const int POSTGRADUATE = 9;
                public const int NO_STUDIES = 10;
                public static Dictionary<int, string> TYPE = new Dictionary<int, string>()
                {
                    { COMPLETE_PRIMARY , "Primaria completa" },
                    { INCOMPLETE_PRIMARY , "Primaria incompleta"},
                    { COMPLETE_SECONDARY , "Secundaria completa"},
                    { INCOMPLETE_SECONDARY , "Secundaria incompleta"},
                    { SUPERIOR_COMPLETE_TECHNIQUE, "Superior técnica completa"},
                    { SUPERIOR_INCOMPLETE_TECHNIQUE , "Superior técnica incompleta"},
                    { COMPLETE_UNIVERSITY , "Universitario completo"},
                    { INCOMPLETE_UNIVERSITY , "Universitario incompleta"},
                    { POSTGRADUATE , "Posgrado"},
                    { NO_STUDIES , "Sin nivel"}
                };
            }
        }
        /* PAYROLL */
        public static class PAYROLL
        {
            public static class WAGE_ITEM_VARIABLE_TYPES
            {
                public const byte STATIC = 0;
                public const byte DYNAMIC = 1;
                public const byte TEMPORARY = 2;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                {
                    { STATIC, "Fijo" },
                    { DYNAMIC, "Variable" },
                    { TEMPORARY, "Temporal" }
                };
            }
            public static class WAGE_ITEM_TYPES
            {
                public const byte INCOME = 0;
                public const byte DISCOUNT = 1;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                {
                    { INCOME, "Ingreso" },
                    { DISCOUNT, "Descuento" }
                };
                public static Dictionary<byte, short> SIGNS = new Dictionary<byte, short>
                {
                    { INCOME, 1 },
                    { DISCOUNT, -1 }
                };
            }
            public static class PAYROLL_INTERVALS
            {
                public const byte WEEKLY = 1;
                public const byte BIWEEKLY = 2;
                public const byte MONTHLY = 3;
                public const byte BIMONTHLY = 4;
                public const byte QUARTERLY = 5;
                public const byte BIANNUAL = 6;
                public const byte ANNUAL = 7;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { WEEKLY, "Semanal" },
                    { BIWEEKLY, "Quincenal" },
                    { MONTHLY, "Mensual" },
                    { BIMONTHLY, "Bimestral" },
                    { QUARTERLY, "Trimestral" },
                    { BIANNUAL, "Semestral" },
                    { ANNUAL, "Anual" }
                };
            }
            public static class FORMULA_FIELDS
            {
                public static class Type
                {
                    public const byte CONSTANT = 0;
                    public const byte VARIABLE = 1;
                }
                public class Field
                {
                    public byte Type;
                    public short Code;
                    public string Text;
                    public int? Value;
                }
                public static class APPLY_CONDITION
                {
                    public const byte ALWAYS = 0;
                    public const byte CONDITION = 1;
                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                    {
                        { ALWAYS, "Todos los casos" },
                        { CONDITION, "Trabajadores que cumplen la condición" }
                    };
                }
                public const short SALARY = 0;
                public const short WORK_DAYS = 1;
                public static Dictionary<short, Field> VALUES = new Dictionary<short, Field>
                {
                    { SALARY, new Field { Type = Type.VARIABLE, Code = SALARY, Text = "Salario" } },
                    { WORK_DAYS, new Field { Type = Type.VARIABLE, Code = WORK_DAYS, Text = "Días Trabajados" } }
                };
            }
        }
        /* TUTORING */
        public static class TUTORING
        {
            public static class TYPECOORDINATOR
            {
                public const byte FULL_TIME = 1;
                public const byte PARTIAL_TIME = 2;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                {
                    { FULL_TIME,  "Completo" },
                    { PARTIAL_TIME, "Parcial" }
                };
            }
            public static class TYPETUTOR
            {
                public const byte TEACHER = 1;
                public const byte STUDENT = 2;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                {
                    { TEACHER,  "Docente" },
                    { STUDENT, "Alumno" }
                };
            }
            public static class PROBLEM_CATEGORIES
            {
                public const byte FAMILY = 0;
                public const byte PERSONAL = 1;
                public const byte ACADEMIC = 2;
                public const byte PROFESSIONAL = 3;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                {
                    { FAMILY, "Familiar" },
                    { PERSONAL, "Personal" },
                    { ACADEMIC, "Académico" },
                    { PROFESSIONAL, "Profesional" }
                };
            }
            public static class STATUSSUGGESTION
            {
                public const byte UNREAD = 1;
                public const byte READ = 2;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                {
                    { UNREAD,  "No Leído" },
                    { READ, "Leído" }
                };
            }
        }
        /*ECONOMICMANAGMENT*/
        public static class ECONOMICMANAGEMENT
        {
            public static class TYPECATALOG
            {
                public const byte CAT_BN = 1;
                public const byte CAT_SER = 2;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                {
                    { CAT_BN,  "Bienes" },
                    { CAT_SER, "Servicios" }
                };
            }
            public static class UNITMEASUREMENT
            {
                public const int M2 = 69;
                public const int M3 = 70;
                public const int METER = 73;
                public const int SERVICE = 107;
                public const int UNIT = 112;
                public const int KILOGRAM = 128;
                public const int GRAM = 327;
                public const int HUNDRED = 339;
                public const int DOZEN = 343;
                public const int GALLON = 346;
                public const int LITER = 349;
                public const int MILLAR = 350;
                public const int PAR = 353;
                public const int SQUARE_FOOT = 412;
                public const int MILLILITER = 419;
                public const int POUND = 423;
                public const int CUBIC_FOOT = 424;
                public const int TEN = 445;
                public const int PACKING_X_25 = 447;
                public const int PACKING_X_48 = 448;
                public const int PACKING_X_50 = 449;
                public const int PACKING_X_24 = 450;
                public const int PACKING_X_500 = 452;
                public const int PACKING_X_250 = 453;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>
                {
                    { M2,  "M2" },
                    { M3, "M3" },
                    { METER,  "METRO" },
                    { SERVICE,  "SERVICIO" },
                    { UNIT,  "UNIDAD" },
                    { KILOGRAM,  "KILOGRAMO" },
                    { GRAM,  "GRAMO" },
                    { HUNDRED,  "CIENTO" },
                    { DOZEN,  "DOCENA" },
                    { GALLON,  "GALON" },
                    { LITER,  "LITRO" },
                    { MILLAR,  "MILLAR" },
                    { PAR,  "PAR" },
                    { SQUARE_FOOT,  "PIE_CUADRADO" },
                    { MILLILITER,  "MILILITRO" },
                    { POUND,  "LIBRA" },
                    { CUBIC_FOOT,  "PIE_CUBICO" },
                    { TEN,  "DECENA" },
                    { PACKING_X_25,  "EMPAQUE_X_25" },
                    { PACKING_X_48,  "EMPAQUE_X_48" },
                    { PACKING_X_50,  "EMPAQUE_X_50" },
                    { PACKING_X_24,  "EMPAQUE_X_24" },
                    { PACKING_X_500,  "EMPAQUE_X_500" },
                    { PACKING_X_250,  "EMPAQUE_X_250" }
                };
            }
            public static class SUPPLIERCATEGORYSTATES
            {
                public const int INACTIVE = 0;
                public const int ACTIVE = 1;
                public static Dictionary<int, string> STATES = new Dictionary<int, string>()
                {
                    { INACTIVE, "Inactivo" },
                    { ACTIVE, "Activo" },
                };
            }
        }
        /* GEO */
        public static class GEO
        {
            public static class REQUEST
            {
                public const byte PENDING = 0;
                public const byte ACCEPTED = 1;
                public const byte DENIED = 2;
                public static Dictionary<byte, string> STATES = new Dictionary<byte, string>()
                {
                    { PENDING, "Pendiente" },
                    { ACCEPTED, "Aceptado" },
                    { DENIED, "Rechazado" }
                };
            }
            public static class ASSISTANCE
            {
                public const byte ABSENT = 1;
                public const byte PRESENTED = 2;
                public static Dictionary<byte, string> STATES = new Dictionary<byte, string>()
                {
                    { ABSENT, "Ausente" },
                    { PRESENTED, "Asistió" },
                };
            }
        }
        /* GEO */
        public static class ACADEMIC_YEAR
        {
            public const int ZERO = 0;
            public const int FIRST = 1;
            public const int SECOND = 2;
            public const int THIRD = 3;
            public const int QUARTER = 4;
            public const int FIFTH = 5;
            public const int SIXTH = 6;
            public const int SEVENTH = 7;
            public const int EIGHTH = 8;
            public const int NINETH = 9;
            public const int TENTH = 10;
            public const int ELEVEN = 11;
            public const int TWELVE = 12;
            public const int THIRTEEN = 13;
            public const int FOURTEEN = 14;
            public const int FIFTEEN = 15;
            public const int SIXTEEN = 16;
            public const int SEVENTEEN = 17;
            public const int EIGTHTEEN = 18;
            public const int NINETEEN = 19;
            public const int TWENTY = 20;
            public static Dictionary<int, string> TEXT = new Dictionary<int, string>()
            {
                {ZERO, "-" },
                {FIRST, "Primero" },
                {SECOND, "Segundo" },
                {THIRD, "Tercero" },
                {QUARTER, "Cuarto" },
                {FIFTH, "Quinto" },
                {SIXTH, "Sexto" },
                {SEVENTH, "Septimo" },
                {EIGHTH, "Octavo" },
                {NINETH, "Noveno" },
                {TENTH, "Décimo" },
                {ELEVEN, "Décimo Primero" },
                {TWELVE, "Décimo Segundo" },
                {THIRTEEN, "Décimo Tercero" },
                {FOURTEEN, "Décimo Cuarto" },
                {FIFTEEN, "Décimo Quinto" },
                {SIXTEEN, "Décimo Sexto" },
                {SEVENTEEN, "Décimo Septimo" },
                {EIGTHTEEN, "Décimo Octavo" },
                {NINETEEN, "Décimo Noveno" },
                {TWENTY, "Vigésimo" },
            };
            public static Dictionary<int, string> ROMAN_NUMERALS = new Dictionary<int, string>()
            {
                {ZERO, "-" },
                {FIRST, "I" },
                {SECOND, "II" },
                {THIRD, "III" },
                {QUARTER, "IV" },
                {FIFTH, "V" },
                {SIXTH, "VI" },
                {SEVENTH, "VII" },
                {EIGHTH, "VIII" },
                {NINETH, "IX" },
                {TENTH, "X" },
                {ELEVEN, "XI" },
                {TWELVE, "XII" },
                {THIRTEEN, "XIII" },
                {FOURTEEN, "XIV" },
                {FIFTEEN, "XV" },
                {SIXTEEN, "XVI" },
                {SEVENTEEN, "XVII" },
                {EIGTHTEEN, "XVIII" },
                {NINETEEN, "XIX" },
                {TWENTY, "XX" },
            };
        }
        /*JOBEXCHANGE*/
        public static class EXTERNAL_REQUEST_STATES
        {
            public const int PENDING = 0;
            public const int OBSERVED = 1;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                {PENDING, "Pendiente" },
                {OBSERVED, "Observado" }
            };
        }
        public static class ABILITY_REQUESTS
        {
            public const int ACTIVE = 0;
            public const int REQUESTED = 1;
            public const int DENIED = 2;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                {ACTIVE, "Activo" },
                {REQUESTED, "Solicitado" },
                {DENIED, "Denegado" }
            };
        }
        public static class AGREEMENT_STATES
        {
            public const int ALL = 0;
            public const int ACTIVE = 1;
            public const int CADUCED = 2;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                {ALL, "Todas" },
                {ACTIVE, "Activos" },
                {CADUCED, "Vencidos" }
            };
        }
        public static class GRADES
        {
            public const int ZERO = 0;
            public const int ONE = 1;
            public const int TWO = 2;
            public const int THREE = 3;
            public const int FOUR = 4;
            public const int FIVE = 5;
            public const int SIX = 6;
            public const int SEVEN = 7;
            public const int EIGH = 8;
            public const int NINE = 9;
            public const int TEN = 10;
            public const int ELEVEN = 11;
            public const int TWELVE = 12;
            public const int THIRTEEN = 13;
            public const int FOURTEEN = 14;
            public const int FIFTEEN = 15;
            public const int SIXTEEN = 16;
            public const int SEVENTEEN = 17;
            public const int EIGTHTEEN = 18;
            public const int NINETEEN = 19;
            public const int TWENTY = 20;
            public static Dictionary<int, string> TEXT = new Dictionary<int, string>()
            {
                {ZERO, "Cero" },
                {ONE, "Uno" },
                {TWO, "Dos" },
                {THREE, "Tres" },
                {FOUR, "Cuatro" },
                {FIVE, "Cinco" },
                {SIX, "Seis" },
                {SEVEN, "Siete" },
                {EIGH, "Ocho" },
                {NINE, "Nueve" },
                {TEN, "Diez" },
                {ELEVEN, "Once" },
                {TWELVE, "Doce" },
                {THIRTEEN, "Trece" },
                {FOURTEEN, "Catorce" },
                {FIFTEEN, "Quince" },
                {SIXTEEN, "Dieciseis" },
                {SEVENTEEN, "Diecisiete" },
                {EIGTHTEEN, "Dieciocho" },
                {NINETEEN, "Diecinueve" },
                {TWENTY, "Veinte" },
            };
        }
        public static class ACADEMIC_LEVEL
        {
            public const byte PRIMARY = 1;
            public const byte SECONDARY = 2;
            public const byte SUPERIOR = 3;
            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    {PRIMARY, "Nivel Primario" },
                    {SECONDARY, "Nivel Secundario" },
                    {SUPERIOR, "Nivel Superior" }
                };
        }
        public static class USER
        {
            public static class PICTURE
            {
                public const string USER_DEFAULT = "/images/demo/user.png";
                public const string COMPANY_DEFAULT = "/images/demo/company.jpg";
            }
        }
        public static class ExpenditureProvision
        {
            public static class Status
            {
                public const byte PENDING = 1;
                public const byte ACCEPTED = 2;
                public const byte DENIED = 3;
            }
        }
        public static class STUDENT_INFORMATION
        {
            public static class PERSONAL_INFORMATION
            {
                public static class PERSONAL_INFORMATION_TYPE_SCHOOL
                {
                    public const int STATE_SCHOOL = 0;
                    public const int PRIVATE_SCHOOL = 1;
                    public static Dictionary<int, string> TYPE_SCHOOL = new Dictionary<int, string>()
                    {
                        { STATE_SCHOOL,"Estatal"},
                        { PRIVATE_SCHOOL,"Particular"}
                    };
                }
                public static class PERSONAL_INFORMATION_LEVEL_EDUCATION
                {
                    public const int PRIMARY = 0;
                    public const int SECONDARY = 1;
                    public static Dictionary<int, string> LEVEL_EDUCATION = new Dictionary<int, string>()
                    {
                        { PRIMARY,"Primaria"},
                        { SECONDARY,"Secundaria"}
                    };
                }
                public static class PERSONAL_INFORMATION_CIVIL_STATUS
                {
                    public const int SINGLE = 0;
                    public const int DIVORCED = 1;
                    public const int MARRIED = 2;
                    public const int WIDOWED = 3;
                    public static Dictionary<int, string> CIVIL_STATUS = new Dictionary<int, string>()
                    {
                        { SINGLE, "Soltero(a)" },
                        { DIVORCED, "Divorciado(a)" },
                        { MARRIED, "Casado(a)" },
                        { WIDOWED, "Viudo(a)" },
                    };
                }
                public static class PERSONAL_INFORMATION_INSURANCE
                {
                    public const bool NO = false;
                    public const bool YES = true;
                    public static Dictionary<bool, string> INSURANCE = new Dictionary<bool, string>()
                    {
                        { NO, "No" },
                        { YES, "Si"}
                    };
                }
                public static class PERSONAL_INFORMATION_UNIVERSITY_PREPARATION
                {
                    public const int CEPRE_TYPE_1 = 0;
                    public const int CEPRE_TYPE_2 = 1;
                    public const int CEPRE_TYPE_3 = 2;
                    public const int CEPRE_TYPE_4 = 3;
                    public const int CEPRE_TYPE_5 = 4;
                    public const int CEPRE_TYPE_6 = 5;
                    public const int CEPRE_TYPE_7 = 6;
                    public const int CEPRE_TYPE_8 = 7;
                    public const int PARTICULAR_ACADEMY_1 = 8;
                    public const int PARTICULAR_ACADEMY_2 = 9;
                    public const int PARTICULAR_ACADEMY_3 = 10;
                    public const int PARTICULAR_ACADEMY_4 = 11;
                    public const int MYSELF_1 = 12;
                    public const int MYSELF_2 = 13;
                    public const int PARTICULAR_TEACHER = 14;
                    public const int NONE = 15;
                    public static Dictionary<int, string> UNIVERSITY_PREPARATION = new Dictionary<int, string>()
                    {
                        { CEPRE_TYPE_1, $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]}, Academica particular, Autopreparación y profesor" },
                        { CEPRE_TYPE_2, $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]}, Academica particular, Autopreparación" },
                        { CEPRE_TYPE_3,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]}, Academica particular y profesor particular, círculo de estudios" },
                        { CEPRE_TYPE_4,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]} y Academica particular" },
                        { CEPRE_TYPE_5,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]}, Autopreparación y profesor particular , círculo de estudios" },
                        { CEPRE_TYPE_6,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]} y Autopreparación " },
                        { CEPRE_TYPE_7,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]} y profesor particular, círculo de estudios" },
                        { CEPRE_TYPE_8,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]} solamente" },
                        { PARTICULAR_ACADEMY_1,  "Academia particular, Autopreparación y profesor particular" },
                        { PARTICULAR_ACADEMY_2,  "Academia particular y Autopreparación " },
                        { PARTICULAR_ACADEMY_3,  "Academia particular y profesor particular, círculo de estudios " },
                        { PARTICULAR_ACADEMY_4,  "Academia particular" },
                        { MYSELF_1,  "Autopreparación y profesor particular, círculo de estudios" },
                        { MYSELF_2,  "Autopreparación" },
                        { PARTICULAR_TEACHER,  "Profesor particular, círculo de estudios" },
                        { NONE,  "No tuvo preparación alguna" },
                    };
                }
                public static class PERSONAL_INFORMATION_UNIVERISITY_CHOOSE
                {
                    public const byte CHOOSE_1 = 0;
                    public const byte CHOOSE_2 = 1;
                    public const byte CHOOSE_3 = 2;
                    public const byte CHOOSE_4 = 3;
                    public const byte CHOOSE_5 = 4;
                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        { CHOOSE_1, "Por ser estatal" },
                        { CHOOSE_2, "Por tener prestigio"},
                        { CHOOSE_3, "Por la cercanía"},
                        { CHOOSE_4, "Por ser la única en el departamento"},
                        { CHOOSE_5, "Por ser la única que ofrece dicha carrera"},
                    };
                }
                public static class PERSONAL_INFORMATION_PROFESSIONAL_SCHOOL_CHOOSE
                {
                    public const byte PROFESSIONAL_SCHOOL_CHOOSE_1 = 0;
                    public const byte PROFESSIONAL_SCHOOL_CHOOSE_2 = 1;
                    public const byte PROFESSIONAL_SCHOOL_CHOOSE_3 = 2;
                    public const byte PROFESSIONAL_SCHOOL_CHOOSE_4 = 3;
                    public const byte PROFESSIONAL_SCHOOL_CHOOSE_5 = 4;
                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        { PROFESSIONAL_SCHOOL_CHOOSE_1, "Por vocación (Me Gusta)" },
                        { PROFESSIONAL_SCHOOL_CHOOSE_2, "Me parece buena profesión"},
                        { PROFESSIONAL_SCHOOL_CHOOSE_3, "Me obligaron"},
                        { PROFESSIONAL_SCHOOL_CHOOSE_4, "Escogi al azar"},
                        { PROFESSIONAL_SCHOOL_CHOOSE_5, "Para asegurar el ingreso"},
                    };
                }
            }
            public static class HEALTH
            {
                public static class HEALTH_HABITS
                {
                    public const int NO = 0;
                    public const int YES = 1;
                    public const int SOMETIMES = 2;
                    public static Dictionary<int, string> HABITS = new Dictionary<int, string>()
                    {
                        { NO, "No" },
                        { YES, "Si" },
                        { SOMETIMES, "A Veces" }
                    };
                }
                public static class HEALTH_INSURANCE
                {
                    public const int NO = 0;
                    public const int YES = 1;

                    public static Dictionary<int, string> INSURANCE = new Dictionary<int, string>()
                    {
                        { NO, "No" },
                        { YES, "Si" }
                    };
                }
                public static class TYPE_INSURANCE
                {
                    public const int PRIVATE = 1;
                    public const int SIS = 2;
                    public const int ESSALUD = 3;
                    public const int OTHER = 4;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { PRIVATE, "Privado" },
                        { SIS, "SIS"},
                        { ESSALUD, "ESSALUD" },
                        { OTHER ,"Otro"}
                    };
                }
                public static class HEALTH_PARENT_SICK
                {
                    public const int NO = 0;
                    public const int YES = 1;
                    public static Dictionary<int, string> PARENT_SICK = new Dictionary<int, string>()
                    {
                        { NO, "No" },
                        { YES, "Si" }
                    };
                }

                public static class HEALTH_PARENT_PARENT_SICK
                {
                    public const int NONE = 0;
                    public const int FATHER = 1;
                    public const int MOM = 2;
                    public const int BROTHER = 3;
                    public const int ANOTHERPARENT = 4;
                    public static Dictionary<int, string> PARENT_SICK_WHO = new Dictionary<int, string>()
                    {
                        { NONE, "---" },
                        { FATHER, "Padre" },
                        { MOM, "Madre" },
                        { BROTHER, "Hermano" },
                        { ANOTHERPARENT, "Otro familiar" }
                    };
                }
                public static class HEALTH_BLOODTYPE
                {
                    public const int AB_POSITIVE = 0;
                    public const int AB_NEGATIVE = 1;
                    public const int A_POSITIVE = 2;
                    public const int A_NEGATIVE = 3;
                    public const int B_POSITIVE = 4;
                    public const int B_NEGATIVE = 5;
                    public const int O_POSITIVE = 6;
                    public const int O_NEGATIVE = 7;
                    public static Dictionary<int, string> BLOODTYPE = new Dictionary<int, string>()
                    {
                        { AB_POSITIVE, "AB+" },
                        { AB_NEGATIVE, "AB-" },
                        { A_POSITIVE, "A+" },
                        { A_NEGATIVE, "A-" },
                        { B_POSITIVE, "B+" },
                        { B_NEGATIVE, "B-" },
                        { O_POSITIVE, "O+" },
                        { O_NEGATIVE, "O-" }
                    };
                }
            }
            public static class ECONOMY
            {
                public static class ECONOMY_WORK_CONDITION
                {
                    public const int CONTRACT_FULL_TIME = 1;
                    public const int CONTRACT_PARTIAL_TIME = 2;
                    public const int CONTRACT_TEMPORAL = 3;
                    public const int CONTRACT_INDEF = 4;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { CONTRACT_FULL_TIME, "Contrato a tiempo total" },
                        { CONTRACT_PARTIAL_TIME, "Contrato a tiempo parcial"},
                        { CONTRACT_TEMPORAL, "Contrato temporal" },
                        { CONTRACT_INDEF, "Contrato indefinido" }
                    };
                }
                public static class ECONOMY_WORK_DEDICATION
                {
                    public const int COMPLETE = 1;
                    public const int PARTIAL = 2;
                    public const int FOR_HOURS = 3;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { COMPLETE, "Tiempo completo" },
                        { PARTIAL, "Tiempo parcial"},
                        { FOR_HOURS, "Por horas" }
                    };
                }
                public static class ECONOMY_PRINCIPAL_PERSON
                {
                    public const int FATHER_MOTHER = 0;
                    public const int FATHER = 1;
                    public const int MOTHER = 2;
                    public const int TUTOR = 3;
                    public const int MYSELF = 4;
                    public static Dictionary<int, string> PRINCIPAL_PERSON = new Dictionary<int, string>()
                    {
                        { FATHER_MOTHER, "Padre y madre" },
                        { FATHER, "Padre"},
                        { MOTHER, "Madre" },
                        { TUTOR, "Familiar tutor" },
                        { MYSELF, "El mismo alumno "}
                    };
                }
                public static class ECONOMY_ECONOMIC_METHOD
                {
                    public const int MONTHLY = 0;
                    public const int BIWEEKLY = 1;
                    public const int WEEKLY = 2;
                    public const int DAILY = 3;
                    public static Dictionary<int, string> ECONOMIC_METHOD = new Dictionary<int, string>()
                    {
                        { MONTHLY, "Mensual" },
                        { BIWEEKLY, "Quincenal"},
                        { WEEKLY, "Semanal" },
                        { DAILY, "Diario" }
                    };
                }
                public static class ECONOMY_IS_WORK
                {
                    public const int YES = 0;
                    public const int NO = 1;
                    public static Dictionary<int, string> IS_WORK = new Dictionary<int, string>()
                    {
                        { YES, "Si" },
                        { NO, "No"}
                    };
                }
                public static class ECONOMY_D_SECTOR
                {
                    public const int NONE = 0;
                    public const int PUBLIC = 1;
                    public const int PRIVATE = 2;
                    public static Dictionary<int, string> D_SECTOR = new Dictionary<int, string>()
                    {
                        { NONE ,"---"},
                        { PUBLIC, "Público" },
                        { PRIVATE, "Privado"}
                    };
                }
                public static class ECONOMY_D_WORK_CONDITION
                {
                    public const int NONE = 0;
                    public const int APPOINTED = 1;
                    public const int HIRED = 2;
                    public static Dictionary<int, string> D_WORK_CONDITION = new Dictionary<int, string>()
                    {
                        { NONE ,"---"},
                        { APPOINTED, "Nombrado" },
                        { HIRED, "Contratado"}
                    };
                }
                public static class ECONOMY_BUSY
                {
                    public const int NONE = 0;
                    public const int YES = 1;
                    public const int NO = 2;
                    public static Dictionary<int, string> BUSY = new Dictionary<int, string>()
                    {
                        { NONE, "---" },
                        { YES, "Si" },
                        { NO, "No"}
                    };
                }
                public static class ECONOMY_I_SECTOR
                {
                    public const int NONE = 0;
                    public const int FORMAL_TRADE = 1;
                    public const int AMBULATORY_TRADE = 2;
                    public static Dictionary<int, string> I_SECTOR = new Dictionary<int, string>()
                    {
                        { NONE, "---" },
                        { FORMAL_TRADE, "Comercio formal" },
                        { AMBULATORY_TRADE, "Comercio ambulatorio"}
                    };
                }
                public static class ECONOMY_I_WORK_CONDITION
                {
                    public const int NONE = 0;
                    public const int BY_SERVICES = 1;
                    public const int OTHERS = 2;
                    public static Dictionary<int, string> I_WORK_CONDITION = new Dictionary<int, string>()
                    {
                        { NONE, "---" },
                        { BY_SERVICES, "Por servicios" },
                        { OTHERS, "Otros"}
                    };
                }
                public static class ECONOMY_STUDENT_DEPENDENCY
                {
                    public const int BY_PARENTS = 0;
                    public const int BY_MOTHER = 1;
                    public const int BY_FATHER = 2;
                    public const int UNIQUEPARENT = 3;
                    public const int MYSELF = 4;
                    public static Dictionary<int, string> STUDENT_DEPENDENCY = new Dictionary<int, string>()
                    {
                        { BY_PARENTS, "Depende de ambos padres" },
                        { BY_MOTHER, "Sólo de madre"},
                        { BY_FATHER, "Sólo de padre" },
                        { UNIQUEPARENT, "De un familiar"},
                        { MYSELF, "De si mismo"}
                    };
                }
                public static class ECONOMY_STUDENT_COEXISTENCE
                {
                    public const int WITH_PARENTS = 0;
                    public const int ONE_PARENT = 1;
                    public const int HOSTED = 2;
                    public const int CAUTION = 3;
                    public const int RENTED_ROOM = 4;
                    public static Dictionary<int, string> STUDENT_COEXISTENCE = new Dictionary<int, string>()
                    {
                        { WITH_PARENTS, "Con sus padres" },
                        { ONE_PARENT, "Con uno de sus padres"},
                        { HOSTED, "Como alojado" },
                        { CAUTION, "Como cuidante"},
                        { RENTED_ROOM, "Cuarto alquilado"}
                    };
                }
                public static class ECONOMY_STUDENT_RISK
                {
                    public const int LIVING_PARENTS = 0;
                    public const int DEAD_FATHER = 1;
                    public const int DEAD_MOTHER = 2;
                    public const int DEAD_FATHER_MOTHER = 3;
                    public const int ALONE = 4;
                    public static Dictionary<int, string> STUDENT_RISK = new Dictionary<int, string>()
                    {
                        { LIVING_PARENTS, "Hijo de padres vivos" },
                        { DEAD_FATHER, "Huérfano de padre"},
                        { DEAD_MOTHER, "Huérfano de madre" },
                        { DEAD_FATHER_MOTHER, "Huérfano de padre y madre"},
                        { ALONE, "Vive solo"}
                    };
                }
            }
            public static class LIVING_PLACE
            {
                public static class LIVING_PLACE_ZONE
                {
                    public const int RESIDENTIAL = 0;
                    public const int URBAN = 1;
                    public const int HUMAN_SETTLEMENTS = 2;
                    public const int INVASION = 3;
                    public static Dictionary<int, string> ZONE = new Dictionary<int, string>()
                    {
                        { RESIDENTIAL, "Residencial" },
                        { URBAN, "Urbana" },
                        { HUMAN_SETTLEMENTS, "AA.HH" },
                        { INVASION, "Invasión" }
                    };
                }
                public static class LIVING_PLACE_TYPE
                {
                    public const int HOME = 0;
                    public const int HOME_CONDOMINIUM = 1;
                    public const int APARTMENT_IN_BUILDING = 2;
                    public const int FIFTH_APARTMENT = 3;
                    public const int ROOM = 4;
                    public static Dictionary<int, string> TYPE = new Dictionary<int, string>()
                    {
                        { HOME, "Casa" },
                        { HOME_CONDOMINIUM, "Casa en condominio" },
                        { APARTMENT_IN_BUILDING, "Dpto. en edificio" },
                        { FIFTH_APARTMENT, "Dpto. en quinta" },
                        { ROOM, "Cuarto" }
                    };
                }
                public static class LIVING_PLACE_TENURE
                {
                    public const int OWN = 0;
                    public const int RENTAL_SALE = 1;
                    public const int RENTED = 2;
                    public const int BORROWED = 3;
                    public const int SHARED = 4;
                    public static Dictionary<int, string> TENURE = new Dictionary<int, string>()
                    {
                        { OWN, "Propia" },
                        { RENTAL_SALE, "Alquiler-Venta" },
                        { RENTED, "Alquilada" },
                        { BORROWED, "Prestada" },
                        { SHARED, "Compartida" }
                    };
                }
                public static class LIVINGPLACE_TYPEBUILD
                {
                    public const int BRICK_CONCRETE = 0;
                    public const int ADOBE = 1;
                    public const int QUINCHA = 2;
                    public const int WOOD = 3;
                    public const int MAT = 4;
                    public static Dictionary<int, string> TYPEBUILD = new Dictionary<int, string>()
                    {
                        { BRICK_CONCRETE, "Ladrillo y Concreto" },
                        { ADOBE, "Adobe" },
                        { QUINCHA, "Quincha" },
                        { WOOD, "Madera" },
                        { MAT, "Estera" }
                    };
                }
                public static class LIVINGPLACE_CONSTRUCTION_CONDITION
                {
                    public const int IN_CONSTRUCTION = 0;
                    public const int FINISHED = 1;
                    public static Dictionary<int, string> CONSTRUCTION_CONDITION = new Dictionary<int, string>()
                    {
                        { IN_CONSTRUCTION, "En Construcción" },
                        { FINISHED, "Acabada" }
                    };
                }
                public static class LIVINGPLACE_CONSTRUCTION_TYPE
                {
                    public const int NOBLE = 0;
                    public const int MIXED = 1;
                    public const int RUSTIC = 2;
                    public const int PRECARIUS = 3;
                    public static Dictionary<int, string> CONSTRUCTION_TYPE = new Dictionary<int, string>()
                    {
                        { NOBLE, "Noble" },
                        { MIXED, "Mixto" },
                        { RUSTIC, "Rústico" },
                        { PRECARIUS, "Precario" }
                    };
                }
                public static class LIVINGPLACE_COHABITANT
                {
                    public const int BROTHER = 0;
                    public const int FATHER_MOTHER = 1;
                    public const int FATHER = 2;
                    public const int MOTHER = 3;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { BROTHER , "Hermano" },
                        { FATHER_MOTHER, "Padre y Madre" },
                        { FATHER, "Padre" },
                        { MOTHER, "Madre" }
                    };
                }
            }
            public static class RELIGION
            {
                public static class RELIGION_TYPE
                {
                    public const int CATOLIC_RELIGION = 0;
                    public const int EVANGELICAL_RELIGION = 1;
                    public const int JEHOVAHS_WITNESS = 2;
                    public const int OTHER_RELIGION = 3;
                    public static Dictionary<int, string> TYPE = new Dictionary<int, string>()
                    {
                        { CATOLIC_RELIGION, "Católica" },
                        { EVANGELICAL_RELIGION, "Evangélica" },
                        { JEHOVAHS_WITNESS, "Testigo de Jehová" },
                        { OTHER_RELIGION, "Otra" },
                    };
                }
            }
        }
        public static class Solution
        {
            public const int AcademicExchange = 1;
            public const int Admission = 2;
            public const int Cafeteria = 3;
            public const int ComputerCenter = 4;
            public const int ComputersManagement = 5;
            public const int Degree = 6;
            public const int DocumentaryProcedure = 7;
            public const int EconomicManagement = 8;
            public const int Enrollment = 9;
            public const int Escalafon = 10;
            public const int Evaluation = 11;
            public const int Geo = 12;
            public const int HelpDesk = 13;
            public const int Indicators = 14;
            public const int InstitutionalAgenda = 15;
            public const int InterestGroup = 16;
            public const int Intranet = 17;
            public const int Investigation = 18;
            public const int JobExchange = 19;
            public const int Kardex = 20;
            public const int LanguageCenter = 21;
            public const int Laurassia = 22;
            public const int Payroll = 23;
            public const int PreUniversitary = 24;
            public const int Quibuk = 25;
            public const int Reservations = 26;
            public const int ResolutiveActs = 27;
            public const int RoomReservations = 28;
            public const int Server = 29;
            public const int ServiceManagement = 30;
            public const int Sisco = 31;
            public const int TeachingManagement = 32;
            public const int TransparencyPortal = 33;
            public const int Tutoring = 34;
            public const int VirtualDirectory = 35;
            public const int VisitManagement = 36;
            public const int InstitutionalWelfare = 37;
            public const int Chat = 38;
            public const int University_Extension = 39;
            public const int Administration = 40;
            public const int Postgraduate = 41;
            public const int ContinuousTraining = 42;
            public const int TeacherInvestigation = 43; //Sistema fuera de este proyecto
            public const int TeacherHiring = 44; //Sistema fuera de este proyecto
            public const int PreProfessionalPractice = 45;
            public const int HelpDesk2 = 46; //Sistema fuera de este proyecto
            public const int ComplaintBook = 47; //Sistema fuera de este proyecto

            public const byte Forum_Tutoring_Coordinator = 134;
            public const byte Forum_Tutoring_Tutor = 234;
            public static Dictionary<int, string> Values = new Dictionary<int, string>()
            {
                { AcademicExchange, "Cooperación Internacional e Intercambio" },
                { Admission, "Admisión" },
                { Cafeteria, "Comedor" },
                { ComputerCenter, "Centro de Cómputo" },
                { ComputersManagement, "Gestión Informática" },
                { Degree, "Grados y Títulos" },
                { DocumentaryProcedure, "Trámite Documentario" },
                { EconomicManagement, "Gestión Financiera" },
                { Enrollment, "Matrícula" },
                { Escalafon, "Escalafón" },
                { Evaluation, "Evaluación de Proyección Social" },
                { Geo, "Laboratorio de Geotecnia" },
                { HelpDesk, "Soporte Técnico" },
                { Indicators, "Gestión Institucional Base Indicadores" },
                { InstitutionalAgenda, "Agenda Institucional" },
                { InterestGroup, "Grupos de Interés" },
                { Intranet, "Intranet" },
                { Investigation, "Proyecto de Investigación" },
                { JobExchange, "Bolsa de Trabajo" },
                { Kardex, "Kardex" },
                { LanguageCenter, "Centro de Idiomas" },
                { Laurassia, "Aprendizaje Virtual" },
                { Payroll, "Nómina" },
                { PreUniversitary, "Centro Pre-Universitario" },
                { Quibuk, "Biblioteca" },
                { Reservations, "Reserva de Ambientes" },
                { ResolutiveActs, "Reserva de Ambientes" },
                { RoomReservations, "Sistema de Reservación de Habitaciones" },
                { Server, "Campus" },
                { ServiceManagement, "Gestión de Servicios" },
                { Sisco, "Sistema de Información y Comunicación" },
                { TeachingManagement, "Gestión Docente" },
                { TransparencyPortal, "Portal de Transparencia" },
                { Tutoring, "Gestión de Tutorías" },
                { VirtualDirectory, "Directorio Virtual" },
                { Chat, "Chat"},
                { VisitManagement, "Gestión de Visitas" },
                { InstitutionalWelfare, "Bienestar Institucional" },
                { University_Extension, "Evaluación de Proyectos de Extensión Universitaria" },
                { Administration, "Administración" },
                { Postgraduate, "Posgrado" },
                { ContinuousTraining, "Formación Continua" },
                { TeacherInvestigation , "Sistema de Investigación Docente" },
                { TeacherHiring , "Contratación Docente" },
                { PreProfessionalPractice, "Prácticas Pre-Profesionales"},
                { HelpDesk2, "Soporte"},
                { ComplaintBook, "Libro de reclamaciones" }
            };
            public static Dictionary<int, Dictionary<int, string>> Institution = new Dictionary<int, Dictionary<int, string>>()
            {
                {
                    ConstantHelpers.Institution.Akdemic,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Evaluation, Values[Evaluation] },
                        { University_Extension, Values[University_Extension] },
                        { Indicators, Values[Indicators] },
                        { Intranet, Values[Intranet] },
                        { InterestGroup, Values[InterestGroup] },
                        { Investigation, Values[Investigation] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { Quibuk, Values[Quibuk] },
                        { Sisco, Values[Sisco] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { TransparencyPortal, Values[TransparencyPortal] },
                        { Tutoring, Values[Tutoring] }
                    }
                },
                {
                    ConstantHelpers.Institution.PMESUT,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] },
                        { Sisco, Values[Sisco] },
                        { Chat, Values[Chat]}
                    }
                },
                {
                    ConstantHelpers.Institution.UNAM,
                    new Dictionary<int, string>()
                },
                {
                    ConstantHelpers.Institution.UNAMAD,
                    new Dictionary<int, string>()
                },
                {
                    ConstantHelpers.Institution.UNICA,
                    new Dictionary<int, string>()
                    {
                        { JobExchange, Values[JobExchange] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Escalafon, Values[Escalafon] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Degree, Values[Degree] },
                        { Indicators, Values[Indicators] },
                        { Intranet, Values[Intranet] },
                        { Laurassia, Values[Laurassia] },
                        { Enrollment, Values[Enrollment] },
                        { Quibuk, Values[Quibuk] },
                        { DocumentaryProcedure, Values[DocumentaryProcedure] },
                        { TransparencyPortal, Values[TransparencyPortal] },
                    }
                },
                {
                    ConstantHelpers.Institution.UNICA_CEPU,
                    new Dictionary<int, string>()
                },
                {
                    ConstantHelpers.Institution.UNICA_POST,
                    new Dictionary<int, string>()
                },
                {
                    ConstantHelpers.Institution.UNJBG,
                    new Dictionary<int, string>()
                    {
                        { AcademicExchange, Values[AcademicExchange] },
                        { Admission, Values[Admission] },
                        { Cafeteria, Values[Cafeteria] },
                        { DocumentaryProcedure, Values[DocumentaryProcedure] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Escalafon, Values[Escalafon] },
                        { Enrollment, Values[Enrollment] },
                        { Indicators, Values[Indicators] },
                        { InstitutionalAgenda, Values[InstitutionalAgenda] },
                        { Intranet, Values[Intranet] },
                        { Investigation, Values[Investigation] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { PreUniversitary, Values[PreUniversitary] },
                        { Quibuk, Values[Quibuk] },
                        { ResolutiveActs, Values[ResolutiveActs] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { TransparencyPortal, Values[TransparencyPortal] },
                        { VirtualDirectory, Values[VirtualDirectory] },

                    }
                },
                {
                    ConstantHelpers.Institution.UNAB,
                    new Dictionary<int, string>()
                    {
                        { Admission, Values[Admission] },
                        { Degree, Values[Degree] },
                        { DocumentaryProcedure, Values[DocumentaryProcedure] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { InstitutionalWelfare, Values[InstitutionalWelfare] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] }
                    }
                },
                {
                    ConstantHelpers.Institution.UNAH,
                    new Dictionary<int, string>()
                    {
                        { Admission, Values[Admission] },
                        { Degree, Values[Degree] },
                        { DocumentaryProcedure, Values[DocumentaryProcedure] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { InstitutionalWelfare, Values[InstitutionalWelfare] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] },
                        { Evaluation, Values[Evaluation] },
                        { Indicators, Values[Indicators] },
                        { TeacherInvestigation, Values[TeacherInvestigation] }
                    }
                },
                 {
                    ConstantHelpers.Institution.UNDC,
                    new Dictionary<int, string>()
                    {
                        { Admission, Values[Admission] },
                        { Degree, Values[Degree] },
                        { DocumentaryProcedure, Values[DocumentaryProcedure] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { InstitutionalWelfare, Values[InstitutionalWelfare] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] }
                    }
                },
                 {
                    ConstantHelpers.Institution.UNIFSLB,
                    new Dictionary<int, string>()
                    {
                        { Admission, Values[Admission] },
                        { Degree, Values[Degree] },
                        { DocumentaryProcedure, Values[DocumentaryProcedure] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { InstitutionalWelfare, Values[InstitutionalWelfare] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] }
                    }
                },
                 {
                    ConstantHelpers.Institution.UNTUMBES,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] },
                        { Sisco, Values[Sisco] },
                        { Chat, Values[Chat]}
                    }
                },
                {
                    ConstantHelpers.Institution.GRAY,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] },
                        { Sisco, Values[Sisco] },
                        { Chat, Values[Chat]}
                    }
                },
                {
                    ConstantHelpers.Institution.UNAJMA,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] },
                        { Sisco, Values[Sisco] },
                        { Chat, Values[Chat]}
                    }
                },
                {
                    ConstantHelpers.Institution.UNJ,
                    new Dictionary<int, string>()
                    {
                        { Admission, Values[Admission] },
                        { Degree, Values[Degree] },
                        { DocumentaryProcedure, Values[DocumentaryProcedure] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { InstitutionalWelfare, Values[InstitutionalWelfare] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring]}
                    }
                },
                {
                    ConstantHelpers.Institution.UNSCH,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] },
                        { TeacherInvestigation, Values[TeacherInvestigation] }
                    }
                },
                {
                    ConstantHelpers.Institution.UNSM,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] }
                    }
                },
                {
                    ConstantHelpers.Institution.UNAPI,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] }
                    }
                },
                {
                    ConstantHelpers.Institution.UNFV,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] }
                    }
                },
                 {
                    ConstantHelpers.Institution.UNCP,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { Laurassia, Values[Laurassia] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { Tutoring, Values[Tutoring] }
                    }
                },
                  {
                    ConstantHelpers.Institution.ENSDF,
                    new Dictionary<int, string>()
                    {
                        { Degree, Values[Degree] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { Intranet, Values[Intranet] },
                        { JobExchange, Values[JobExchange] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { DocumentaryProcedure, Values[DocumentaryProcedure] },
                    }
                },
                   {
                    ConstantHelpers.Institution.UIGV,
                    new Dictionary<int, string>()
                    {
                        { Admission, Values[Admission] },
                        { Enrollment, Values[Enrollment] },
                        { Escalafon, Values[Escalafon] },
                        { TeachingManagement, Values[TeachingManagement] },
                        { InstitutionalWelfare, Values[InstitutionalWelfare] },
                        { EconomicManagement, Values[EconomicManagement] },
                        { Degree, Values[Degree] },
                        { Tutoring, Values[Tutoring] },
                        { JobExchange, Values[JobExchange] },
                        { Intranet, Values[Intranet] },
                        { Laurassia, Values[Laurassia] },
                        { DocumentaryProcedure, Values[DocumentaryProcedure] },
                        { Indicators, Values[Indicators] },
                    }
                },
            };
            public static Dictionary<int, string> Roles = new Dictionary<int, string>()
            {
                { AcademicExchange, ConstantHelpers.ROLES.ACADEMIC_EXCHANGE_ADMIN },
                { Admission, ConstantHelpers.ROLES.ADMISSION_ADMIN },
                { Cafeteria, ConstantHelpers.ROLES.CAFETERIA_ADMIN },
                { ComputerCenter, ConstantHelpers.ROLES.COMPUTER_CENTER_ADMIN },
                { ComputersManagement, ConstantHelpers.ROLES.COMPUTER_MANAGEMENT_ADMIN },
                { Degree, ConstantHelpers.ROLES.DEGREE_ADMIN },
                { DocumentaryProcedure, ConstantHelpers.ROLES.DOCUMENTARY_PROCEDURE_ADMIN },
                { EconomicManagement, ConstantHelpers.ROLES.ECONOMIC_MANAGEMENT_ADMIN },
                { Enrollment, ConstantHelpers.ROLES.ENROLLMENT_ADMIN },
                { Escalafon, ConstantHelpers.ROLES.ESCALAFON_ADMIN },
                { Evaluation, ConstantHelpers.ROLES.EVALUATION_ADMIN },
                { Geo, ConstantHelpers.ROLES.GEO_ADMIN },
                { HelpDesk, ConstantHelpers.ROLES.HELP_DESK_ADMIN },
                { Indicators, ConstantHelpers.ROLES.INDICATORS_ADMIN },
                { InstitutionalAgenda, ConstantHelpers.ROLES.INSTITUTIONAL_AGENDA_ADMIN },
                { InterestGroup, ConstantHelpers.ROLES.INTEREST_GROUP_ADMIN },
                { Intranet, ConstantHelpers.ROLES.INTRANET_ADMIN },
                { Investigation, ConstantHelpers.ROLES.INVESTIGATION_ADMIN },
                { JobExchange, ConstantHelpers.ROLES.JOB_EXCHANGE_ADMIN },
                { Kardex, ConstantHelpers.ROLES.KARDEX_ADMIN },
                { LanguageCenter, ConstantHelpers.ROLES.LANGUAGE_CENTER_ADMIN },
                { Laurassia, ConstantHelpers.ROLES.LAURASSIA_ADMIN },
                { Payroll, ConstantHelpers.ROLES.PAYROLL_ADMIN },
                { PreUniversitary, ConstantHelpers.ROLES.PREUNIVERSITARY_ADMIN },
                { Quibuk, ConstantHelpers.ROLES.QUIBUK_ADMIN },
                { Reservations, ConstantHelpers.ROLES.RESERVATIONS_ADMIN },
                { ResolutiveActs, ConstantHelpers.ROLES.RESOLUTIVE_ACTS_ADMIN },
                { RoomReservations, ConstantHelpers.ROLES.ROOM_RESERVATION_SYSTEM_ADMIN },
                { Server, ConstantHelpers.ROLES.SERVER_ADMIN },
                { ServiceManagement,ConstantHelpers.ROLES.SERVICE_MANAGEMENT_ADMIN },
                { Sisco, ConstantHelpers.ROLES.SISCO_ADMIN },
                { TeachingManagement, ConstantHelpers.ROLES.TEACHING_MANAGEMENT_ADMIN },
                { TransparencyPortal, ConstantHelpers.ROLES.TANSPARENCY_PORTAL_ADMIN },
                { Tutoring, ConstantHelpers.ROLES.TUTORING_ADMIN },
                { VirtualDirectory, ConstantHelpers.ROLES.VIRTUAL_DIRECTORY_ADMIN },
                { University_Extension, ConstantHelpers.ROLES.UNIVERSITARY_EXTENSION_ADMIN },
                //{ VisitManagement, ConstantHelpers.ROLES.VISIT_MANAGEMENT_ADMIN }
            };
            public static Dictionary<int, string> IntranetUrl = new Dictionary<int, string>()
            {
                { AcademicExchange, ConstantHelpers.ROLES.ACADEMIC_EXCHANGE_ADMIN },
                { Admission, ConstantHelpers.ROLES.ADMISSION_ADMIN },
                { Cafeteria, ConstantHelpers.ROLES.CAFETERIA_ADMIN },
                { ComputerCenter, ConstantHelpers.ROLES.COMPUTER_CENTER_ADMIN },
                { ComputersManagement, ConstantHelpers.ROLES.COMPUTER_MANAGEMENT_ADMIN },
                { Degree, ConstantHelpers.ROLES.DEGREE_ADMIN },
                { DocumentaryProcedure, ConstantHelpers.ROLES.DOCUMENTARY_PROCEDURE_ADMIN },
                { EconomicManagement, ConstantHelpers.ROLES.ECONOMIC_MANAGEMENT_ADMIN },
                { Enrollment, ConstantHelpers.ROLES.ENROLLMENT_ADMIN },
                { Escalafon, ConstantHelpers.ROLES.ESCALAFON_ADMIN },
                { Evaluation, ConstantHelpers.ROLES.EVALUATION_ADMIN },
                { Geo, ConstantHelpers.ROLES.GEO_ADMIN },
                { HelpDesk, ConstantHelpers.ROLES.HELP_DESK_ADMIN },
                { Indicators, ConstantHelpers.ROLES.INDICATORS_ADMIN },
                { InstitutionalAgenda, ConstantHelpers.ROLES.INSTITUTIONAL_AGENDA_ADMIN },
                { InterestGroup, ConstantHelpers.ROLES.INTEREST_GROUP_ADMIN },
                { Intranet, ConstantHelpers.ROLES.INTRANET_ADMIN },
                { Investigation, ConstantHelpers.ROLES.INVESTIGATION_ADMIN },
                { JobExchange, ConstantHelpers.ROLES.JOB_EXCHANGE_ADMIN },
                { Kardex, ConstantHelpers.ROLES.KARDEX_ADMIN },
                { LanguageCenter, ConstantHelpers.ROLES.LANGUAGE_CENTER_ADMIN },
                { Laurassia, ConstantHelpers.ROLES.LAURASSIA_ADMIN },
                { Payroll, ConstantHelpers.ROLES.PAYROLL_ADMIN },
                { PreUniversitary, ConstantHelpers.ROLES.PREUNIVERSITARY_ADMIN },
                { Quibuk, ConstantHelpers.ROLES.QUIBUK_ADMIN },
                { Reservations, ConstantHelpers.ROLES.RESERVATIONS_ADMIN },
                { ResolutiveActs, ConstantHelpers.ROLES.RESOLUTIVE_ACTS_ADMIN },
                { RoomReservations, ConstantHelpers.ROLES.ROOM_RESERVATION_SYSTEM_ADMIN },
                { Server, ConstantHelpers.ROLES.SERVER_ADMIN },
                { ServiceManagement,ConstantHelpers.ROLES.SERVICE_MANAGEMENT_ADMIN },
                { Sisco, ConstantHelpers.ROLES.SISCO_ADMIN },
                { TeachingManagement, ConstantHelpers.ROLES.TEACHING_MANAGEMENT_ADMIN },
                { TransparencyPortal, ConstantHelpers.ROLES.TANSPARENCY_PORTAL_ADMIN },
                { Tutoring, ConstantHelpers.ROLES.TUTORING_ADMIN },
                { VirtualDirectory, ConstantHelpers.ROLES.VIRTUAL_DIRECTORY_ADMIN }
            };
            public static Dictionary<int, Dictionary<int, string>> Routes = new Dictionary<int, Dictionary<int, string>>
            {
                { 0, new Dictionary<int, string>{
                    { Server, "https://localhost:11936/" },
                    { Administration, "http://localhost:5000/" },
                    { Admission, "http://localhost:54851/" },
                    { Degree, "http://localhost:5000/" },
                    { DocumentaryProcedure , "http://localhost:11288/" },
                    { EconomicManagement, "http://localhost:57286/" },
                    { Enrollment, "http://localhost:63829/" },
                    { Escalafon, "http://localhost:54509/" },
                    { Evaluation, "" },
                    { InstitutionalWelfare, "http://localhost:5000/" },
                    { Indicators, "http://localhost:53492/" },
                    { InterestGroup, "http://localhost:64854/" },
                    { Intranet, "http://localhost:50975/" },
                    { Investigation, "http://localhost:5000/" },
                    { JobExchange, "http://localhost:64728/" },
                    { Laurassia, "https://localhost:44375/" },
                    { Quibuk, "http://localhost:50975/" },
                    { Sisco, "http://localhost:51172/" },
                    { TeachingManagement, "http://localhost:54504/" },
                    { TransparencyPortal, "http://localhost:61264/" },
                    { Tutoring, "http://localhost:62564/" },
                    { University_Extension, "" },
                    { LanguageCenter, "" },
                    { Postgraduate, "" },
                    { ContinuousTraining, "" },
                    { TeacherInvestigation, "http://localhost:5006/" },
                    { AcademicExchange, "http://localhost:50885/"},
                    { HelpDesk2, ""},
                    { ResolutiveActs, "http://localhost:53570/"},
                    { TeacherHiring, ""},
                    { PreProfessionalPractice, "http://localhost:61028/"},
                    { VisitManagement , "http://localhost:35998/"},
                    { ComplaintBook , "http://localhost:5001/"},
                } },
                { ConstantHelpers.Institution.Akdemic, new Dictionary<int, string> {
                    { Server, "https://sigau-campus.akdemic.com/" },
                    { Administration, "https://sigau-administracion.akdemic.com/" },
                    { Admission, "https://sigau-admision.akdemic.com/" },
                    { Degree, "https://sigau-gradotitulo.akdemic.com/" },
                    { DocumentaryProcedure , "https://sigau-tramite.akdemic.com/" },
                    { EconomicManagement, "https://sigau-tesoreria.akdemic.com/" },
                    { Enrollment, "https://sigau-matricula.akdemic.com/" },
                    { Escalafon, "https://sigau-escalafon.akdemic.com/" },
                    { Evaluation, "" },
                    { InstitutionalWelfare, "https://sigau-bienestar.akdemic.com/" },
                    { Indicators, "https://sigau-indicador.akdemic.com/" },
                    { InterestGroup, "" },
                    { Intranet, "https://sigau-intranet.akdemic.com/" },
                    { Investigation, "https://sigau-investigacion.akdemic.com/" },
                    { JobExchange, "https://sigau-bolsa.akdemic.com/" },
                    { Laurassia, "https://sigau-aula.akdemic.com/" },
                    { Quibuk, "https://sigau-intranet.akdemic.com/" },
                    { Sisco, "" },
                    { TeachingManagement, "https://sigau-docente.akdemic.com/" },
                    { TransparencyPortal, "https://sigau-portal.akdemic.com/"},
                    { Tutoring, "https://sigau-tutoria.akdemic.com/" },
                    { University_Extension, "" },
                } },
                  { ConstantHelpers.Institution.UIGV, new Dictionary<int, string> {
                    { Server, "https://sigau-campus.akdemic.com/" },
                    { Administration, "https://sigau-administracion.akdemic.com/" },
                    { Admission, "https://sigau-admision.akdemic.com/" },
                    { Degree, "https://sigau-gradotitulo.akdemic.com/" },
                    { DocumentaryProcedure , "https://sigau-tramite.akdemic.com/" },
                    { EconomicManagement, "https://sigau-tesoreria.akdemic.com/" },
                    { Enrollment, "https://sigau-matricula.akdemic.com/" },
                    { Escalafon, "https://sigau-escalafon.akdemic.com/" },
                    { Evaluation, "" },
                    { InstitutionalWelfare, "https://sigau-bienestar.akdemic.com/" },
                    { Indicators, "https://sigau-indicador.akdemic.com/" },
                    { InterestGroup, "" },
                    { Intranet, "https://sigau-intranet.akdemic.com/" },
                    { Investigation, "https://sigau-investigacion.akdemic.com/" },
                    { JobExchange, "https://sigau-bolsa.akdemic.com/" },
                    { Laurassia, "https://sigau-aula.akdemic.com/" },
                    { Quibuk, "https://sigau-intranet.akdemic.com/" },
                    { Sisco, "" },
                    { TeachingManagement, "https://sigau-docente.akdemic.com/" },
                    { TransparencyPortal, "https://sigau-portal.akdemic.com/"},
                    { Tutoring, "https://sigau-tutoria.akdemic.com/" },
                    { University_Extension, "" },
                } },
                { ConstantHelpers.Institution.UNSM, new Dictionary<int, string> {
                    { Server, "https://campusvirtualpregrado.unsm.edu.pe/" },
                    { Admission, "https://sigauadmision.unsm.edu.pe/" },
                    { Degree, "https://sigaugradotitulo.unsm.edu.pe/" },
                    { DocumentaryProcedure , "https://sigautramite.unsm.edu.pe/" },
                    { EconomicManagement, "https://sigautesoreria.unsm.edu.pe/" },
                    { Enrollment, "https://sigaumatricula.unsm.edu.pe/" },
                    { Escalafon, "https://sigauescalafon.unsm.edu.pe/" },
                    { InstitutionalWelfare, "https://sigaubienestar.unsm.edu.pe/" },
                    { Intranet, "https://sigauintranet.unsm.edu.pe/" },
                    { Investigation, "" },
                    { JobExchange, "https://sigaubolsa.unsm.edu.pe/" },
                    { Laurassia, "https://aulavirtual.unsm.edu.pe/" },
                    { TeachingManagement, "https://sigaudocente.unsm.edu.pe/" },
                    { Tutoring, "https://sigaututoria.unsm.edu.pe/" },
                } },
                { ConstantHelpers.Institution.UNAPI, new Dictionary<int, string> {
                    { Enrollment, "http://sismatricula.unapiquitos.edu.pe" },
                    { Intranet, "http://intranet.unapiquitos.edu.pe/" },
                    { TeachingManagement, "http://sisdocente.unapiquitos.edu.pe/" }
                } },
                { ConstantHelpers.Institution.UNAMAD, new Dictionary<int, string> {
                    { Server, "https://campus.unamad.edu.pe/" },
                    { Admission, "" },
                    { Intranet, "https://intranet.unamad.edu.pe/" },
                    { Enrollment, "https://matricula.unamad.edu.pe/" },
                    { TeachingManagement, "https://gestiondocente.unamad.edu.pe/" },
                    { Laurassia, "https://aulavirtual.unamad.edu.pe/" },
                    { Quibuk, "https://biblioteca.unamad.edu.pe/" },
                    { JobExchange, "https://bolsa.unamad.edu.pe/" },
                    { EconomicManagement, "https://pagosenlinea.unamad.edu.pe/" },
                    { Degree, "" },
                    { Escalafon, "https://escalafon.unamad.edu.pe/" },
                    { TransparencyPortal, ""},
                    { InterestGroup, "" },
                    { Investigation, "https://investigacion.unamad.edu.pe/" },
                    { Sisco, "" },
                    { Tutoring, "" },
                    { Indicators, "" },
                    { Evaluation, "" },
                    { University_Extension, "" },
                    { InstitutionalWelfare, "" },
                } },
                  { ConstantHelpers.Institution.PMESUT, new Dictionary<int, string> {
                    { Server, "https://sigau-campus.akdemic.com/" },
                    { Administration, "https://sigau-administracion.akdemic.com/" },
                    { Admission, "https://sigau-admision.akdemic.com/" },
                    { Degree, "https://sigau-gradotitulo.akdemic.com/" },
                    { DocumentaryProcedure , "https://sigau-tramite.akdemic.com/" },
                    { EconomicManagement, "https://sigau-tesoreria.akdemic.com/" },
                    { Enrollment, "https://sigau-matricula.akdemic.com/" },
                    { Escalafon, "https://sigau-escalafon.akdemic.com/" },
                    { Evaluation, "" },
                    { InstitutionalWelfare, "https://sigau-bienestar.akdemic.com/" },
                    { Indicators, "" },
                    { InterestGroup, "" },
                    { Intranet, "https://sigau-intranet.akdemic.com/" },
                    { Investigation, "https://sigau-investigacion.akdemic.com/" },
                    { JobExchange, "https://sigau-bolsa.akdemic.com/" },
                    { Laurassia, "https://sigau-aula.akdemic.com/" },
                    { Quibuk, "https://sigau-intranet.akdemic.com/" },
                    { Sisco, "" },
                    { TeachingManagement, "https://sigau-docente.akdemic.com/" },
                    { TransparencyPortal, "https://sigau-portal.akdemic.com/"},
                    { Tutoring, "https://sigau-tutoria.akdemic.com/" },
                    { University_Extension, "" },
                } },
                   { ConstantHelpers.Institution.ENSDF, new Dictionary<int, string> {
                    { Server, "https://campus.eespli.edu.pe/" },
                    { Intranet, "https://intranet.escuelafolklore.edu.pe/" },
                    { Enrollment, "https://matricula.escuelafolklore.edu.pe/" },
                    { TeachingManagement, "https://gestiondocente.escuelafolklore.edu.pe/" },
                    { Admission, "https://admision.escuelafolklore.edu.pe/" },
                    { JobExchange, "https://egresados.escuelafolklore.edu.pe/" },
                    { InstitutionalWelfare, "https://bienestar.escuelafolklore.edu.pe/" },
                    { EconomicManagement, "https://tesoreria.escuelafolklore.edu.pe/" },
                    { Escalafon, "https://escalafon.escuelafolklore.edu.pe/" },
                    { DocumentaryProcedure, "https://tramite.escuelafolklore.edu.pe/" },
                    { Degree, "https://gradosytitulos.escuelafolklore.edu.pe/" },
                    { Laurassia, "https://aulavirtual2.escuelafolklore.edu.pe/" },
                    { Tutoring, "https://tutoria.escuelafolklore.edu.pe/" }
                } },
                  { ConstantHelpers.Institution.GRAY, new Dictionary<int, string> {
                 { Server, "https://sigau-campus.akdemic.com/" },
                    { Administration, "https://sigau-administracion.akdemic.com/" },
                    { Admission, "https://sigau-admision.akdemic.com/" },
                    { Degree, "https://sigau-gradotitulo.akdemic.com/" },
                    { DocumentaryProcedure , "https://sigau-tramite.akdemic.com/" },
                    { EconomicManagement, "https://sigau-tesoreria.akdemic.com/" },
                    { Enrollment, "https://sigau-matricula.akdemic.com/" },
                    { Escalafon, "https://sigau-escalafon.akdemic.com/" },
                    { Evaluation, "" },
                    { InstitutionalWelfare, "https://sigau-bienestar.akdemic.com/" },
                    { Indicators, "" },
                    { InterestGroup, "" },
                    { Intranet, "https://sigau-intranet.akdemic.com/" },
                    { Investigation, "https://sigau-investigacion.akdemic.com/" },
                    { JobExchange, "https://sigau-bolsa.akdemic.com/" },
                    { Laurassia, "https://sigau-aula.akdemic.com/" },
                    { Quibuk, "https://sigau-intranet.akdemic.com/" },
                    { Sisco, "" },
                    { TeachingManagement, "https://sigau-docente.akdemic.com/" },
                    { TransparencyPortal, "https://sigau-portal.akdemic.com/"},
                    { Tutoring, "https://sigau-tutoria.akdemic.com/" },
                    { University_Extension, "" },
                } },
                { ConstantHelpers.Institution.UNICA, new Dictionary<int, string> {
                    { Server, "https://campus.unica.edu.pe/" },
                    { Intranet, "https://intranet.unica.edu.pe/" },
                    { Enrollment, "https://matricula.unica.edu.pe/" },
                    { TeachingManagement, "https://gestiondocente.unica.edu.pe/" },
                    { Laurassia, "https://aulavirtual.unica.edu.pe/" },
                    { Quibuk, "https://biblioteca.unica.edu.pe/" },
                    { JobExchange, "https://bolsadetrabajo.unica.edu.pe/" },
                    { EconomicManagement, "https://pagosenlinea.unica.edu.pe/" },
                    { Degree, "https://gradosytitulos.unica.edu.pe/" },
                    { Escalafon, "https://escalafon.unica.edu.pe/" },
                    { Admission, "https://sisadmision.unica.edu.pe/"},
                    { InterestGroup, "" },
                    { Investigation, "" },
                    { Sisco, "" },
                    { Tutoring, "" },
                    { Indicators, "https://indicadores.unica.edu.pe/" },
                    { Evaluation, "" },
                    { University_Extension, "" },
                    { InstitutionalWelfare, "" },
                    { TransparencyPortal, "https://extranet.unica.edu.pe/" },
                    { DocumentaryProcedure, "https://tramites.unica.edu.pe/" },
                } },
                  { ConstantHelpers.Institution.UNSCH, new Dictionary<int, string> {
                      { Server, "https://campus.unsch.edu.pe/" },
                      { Administration, "https://qa-administracion.unsch.edu.pe/" },
                      { Admission, "https://qa-admision.unsch.edu.pe/" },
                      { Degree, "https://qa-grados.unsch.edu.pe/" },
                      { DocumentaryProcedure , "https://tramite.unsch.edu.pe/" },
                      { EconomicManagement, "https://tesoreria.unsch.edu.pe/" },
                      { Enrollment, "https://matricula.unsch.edu.pe/" },
                      { Escalafon, "https://qa-escalafon.unsch.edu.pe/" },
                      { Evaluation, "https://qa-rsu.unsch.edu.pe/" },
                      { InstitutionalWelfare, "https://qa-bienestar.unsch.edu.pe/" },
                      { Indicators, "https://qa-indicadores.unsch.edu.pe/" },
                      { InterestGroup, "https://qa-ginteres.unsch.edu.pe/" },
                      { Intranet, "https://intranet.unsch.edu.pe/" },
                      { TeacherInvestigation, "https://qa-investigacion.unsch.edu.pe/" },
                      { JobExchange, "https://bolsa.unsch.edu.pe/" },
                      { Laurassia, "https://aula.unsch.edu.pe/" },
                      { Quibuk, "https://qa-intranet.unsch.edu.pe/" },
                      { Sisco, "https://qa-sic.unsch.edu.pe/" },
                      { TeachingManagement, "https://docente.unsch.edu.pe/" },
                      { TransparencyPortal, "https://qa-transparencia.unsch.edu.pe/"},
                      { Tutoring, "https://tutoria.unsch.edu.pe/" },
                      { University_Extension, "https://qa-rsu.unsch.edu.pe/" },
                      { LanguageCenter, "https://qa-idiomas.unsch.edu.pe/" },
                      { Postgraduate, "https://qa-postgrado.unsch.edu.pe/" },
                      { ContinuousTraining, "https://qa-formacion.unsch.edu.pe/" },
                } },
                  //----
                { ConstantHelpers.Institution.UNAB, new Dictionary<int, string> {
                    { Server, "http://campus.unab.edu.pe/" },
                    { Admission, "http://admision.unab.edu.pe/" },
                    { Degree, "http://grados.unab.edu.pe/" },
                    { DocumentaryProcedure, "http://tramite.unab.edu.pe/" },
                    { EconomicManagement, "http://tesoreria.unab.edu.pe/" },
                    { Enrollment, "http://matricula.unab.edu.pe/" },
                    { Escalafon, "http://escalafon.unab.edu.pe/" },
                    { InstitutionalWelfare, "http://bienestar.unab.edu.pe/" },
                    { Intranet, "http://intranet.unab.edu.pe/" },
                    { JobExchange, "http://bolsa.unab.edu.pe/" },
                    { Laurassia, "http://aulavirtual.unab.edu.pe/" },
                    { TeachingManagement, "http://docente.unab.edu.pe/" },
                    { Tutoring, "http://tutoria.unab.edu.pe/" },
                } },
                 { ConstantHelpers.Institution.UNAH, new Dictionary<int, string> {
                    { Server, "https://campus.unah.edu.pe/" },
                    { Admission, "https://sisadmision.unah.edu.pe/" },
                    { Degree, "https://gradotitulo.unah.edu.pe/" },
                    { DocumentaryProcedure, "https://tramite.unah.edu.pe/" },
                    { EconomicManagement, "https://tesoreria.unah.edu.pe/" },
                    { Enrollment, "https://matricula.unah.edu.pe/" },
                    { Escalafon, "https://escalafon.unah.edu.pe/" },
                    { InstitutionalWelfare, "https://bienestar.unah.edu.pe/" },
                    { Intranet, "https://intranet.unah.edu.pe/" },
                    { JobExchange, "https://bolsa.unah.edu.pe/" },
                    { Laurassia, "https://aulavirtual.unah.edu.pe/" },
                    { TeachingManagement, "https://docente.unah.edu.pe/" },
                    { Tutoring, "https://tutoria.unah.edu.pe/" },
                    { Indicators, "https://indicadores.unah.edu.pe/" },
                    { TeacherInvestigation, "https://gpinvedocente.unah.edu.pe/" },
                    { Evaluation, "https://proseu.unah.edu.pe/" },
                } },
                  { ConstantHelpers.Institution.UNISCJSA, new Dictionary<int, string> {
                    { Server, "http://campus.uniscjsa.edu.pe/" },
                    { Admission, "http://sisadmision.uniscjsa.edu.pe/" },
                    { Degree, "http://grados.uniscjsa.edu.pe/" },
                    { DocumentaryProcedure, "http://tramite.uniscjsa.edu.pe/" },
                    { EconomicManagement, "http://tesoreria.uniscjsa.edu.pe/" },
                    { Enrollment, "http://sismatricula.uniscjsa.edu.pe/" },
                    { Escalafon, "http://escalafon.uniscjsa.edu.pe/" },
                    { InstitutionalWelfare, "http://bienestar.uniscjsa.edu.pe/" },
                    { Intranet, "http://sisintranet.uniscjsa.edu.pe/" },
                    { JobExchange, "http://bolsa.uniscjsa.edu.pe/" },
                    { Laurassia, "http://aulavirtual.uniscjsa.edu.pe/" },
                    { TeachingManagement, "http://docente.uniscjsa.edu.pe/" },
                    { Tutoring, "http://tutoria.uniscjsa.edu.pe/" },
                } },
                   { ConstantHelpers.Institution.UNAAA, new Dictionary<int, string> {
                    { Server, "http://campus.unaaa.edu.pe/" },
                    { Admission, "http://sisadmision.unaaa.edu.pe/" },
                    { Degree, "http://grados.unaaa.edu.pe/" },
                    { DocumentaryProcedure, "http://tramite.unaaa.edu.pe/" },
                    { EconomicManagement, "http://tesoreria.unaaa.edu.pe/" },
                    { Enrollment, "http://matricula.unaaa.edu.pe/" },
                    { Escalafon, "http://escalafon.unaaa.edu.pe/" },
                    { InstitutionalWelfare, "http://bienestar.unaaa.edu.pe/" },
                    { Intranet, "http://intranet.unaaa.edu.pe/" },
                    { JobExchange, "http://bolsa.unaaa.edu.pe/" },
                    { Laurassia, "http://aulavirtual.unaaa.edu.pe/" },
                    { TeachingManagement, "http://docente.unaaa.edu.pe/" },
                    { Tutoring, "http://tutoria.unaaa.edu.pe/" },
                } },
                    { ConstantHelpers.Institution.UNIFSLB, new Dictionary<int, string> {
                    { Server, "http://campus.unibagua.edu.pe/" },
                    { Admission, "http://admision.unibagua.edu.pe/" },
                    { Degree, "http://grados.unibagua.edu.pe/" },
                    { DocumentaryProcedure, "http://tramite.unibagua.edu.pe/" },
                    { EconomicManagement, "http://tesoreria.unibagua.edu.pe/" },
                    { Enrollment, "http://matricula.unibagua.edu.pe/" },
                    { Escalafon, "http://escalafon.unibagua.edu.pe/" },
                    { InstitutionalWelfare, "http://bienestar.unibagua.edu.pe/" },
                    { Intranet, "http://sisintranet.unibagua.edu.pe/" },
                    { JobExchange, "http://bolsa.unibagua.edu.pe/" },
                    { Laurassia, "http://aulavirtual.unibagua.edu.pe/" },
                    { TeachingManagement, "http://docente.unibagua.edu.pe/" },
                    { Tutoring, "http://tutoria.unibagua.edu.pe/" },
                } },
                     { ConstantHelpers.Institution.UNTUMBES, new Dictionary<int, string> {
                    { Server, "https://campussigau.untumbes.edu.pe/" },
                    { Admission, "https://admision.untumbes.edu.pe/" },
                    { Degree, "https://gradotitulo.untumbes.edu.pe/" },
                    { DocumentaryProcedure, "https://tramite.untumbes.edu.pe/" },
                    { EconomicManagement, "https://tesoreria.untumbes.edu.pe/" },
                    { Enrollment, "https://matricula.untumbes.edu.pe/" },
                    { Escalafon, "https://escalafon.untumbes.edu.pe/" },
                    { InstitutionalWelfare, "https://bienestar.untumbes.edu.pe/" },
                    { Intranet, "https://intranet.untumbes.edu.pe/" },
                    { JobExchange, "https://bolsa.untumbes.edu.pe/" },
                    { Laurassia, "https://aulavirtual.untumbes.edu.pe/" },
                    { TeachingManagement, "https://docente.untumbes.edu.pe/" },
                    { Tutoring, "https://tutoria.untumbes.edu.pe" },
                } },
                      { ConstantHelpers.Institution.UNF, new Dictionary<int, string> {
                    { Server, "https://campusvirtual.unf.edu.pe/" },
                    { Admission, "https://admision.unf.edu.pe/" },
                    { Degree, "https://gradotitulo.unf.edu.pe/" },
                    { DocumentaryProcedure, "https://tramite.unf.edu.pe/" },
                    { EconomicManagement, "https://tesoreria.unf.edu.pe/" },
                    { Enrollment, "https://matricula.unf.edu.pe/" },
                    { Escalafon, "https://escalafon.unf.edu.pe/" },
                    { InstitutionalWelfare, "https://bienestar.unf.edu.pe/" },
                    { Intranet, "https://intranet.unf.edu.pe/" },
                    { JobExchange, "https://bolsa.unf.edu.pe/" },
                    { Laurassia, "https://aulavirtual.unf.edu.pe/" },
                    { TeachingManagement, "https://docente.unf.edu.pe/" },
                    { Tutoring, "https://tutoria.unf.edu.pe/" },
                } },
                       { ConstantHelpers.Institution.UNDC, new Dictionary<int, string> {
                    { Server, "http://campus.undc.edu.pe/" },
                    { Admission, "http://admision.undc.edu.pe/" },
                    { Degree, "http://grados.undc.edu.pe/" },
                    { DocumentaryProcedure, "http://tramite.undc.edu.pe/" },
                    { EconomicManagement, "http://tesoreria.undc.edu.pe/" },
                    { Enrollment, "http://matricula.undc.edu.pe/" },
                    { Escalafon, "http://escalafon.undc.edu.pe/" },
                    { InstitutionalWelfare, "http://bienestar.undc.edu.pe/" },
                    { Intranet, "http://intranet.undc.edu.pe/" },
                    { JobExchange, "http://bolsa.undc.edu.pe/" },
                    { Laurassia, "http://aulavirtual.undc.edu.pe/" },
                    { TeachingManagement, "http://docente.undc.edu.pe/" },
                    { Tutoring, "http://tutoria.undc.edu.pe/" },
                } },
                        { ConstantHelpers.Institution.UNJ, new Dictionary<int, string> {
                    { Server, "http://campus.unj.edu.pe/" },
                    { Admission, "http://sisadmision.unj.edu.pe/" },
                    { Degree, "http://grados.unj.edu.pe/" },
                    { DocumentaryProcedure, "http://tramite.unj.edu.pe/" },
                    { EconomicManagement, "http://tesoreria.unj.edu.pe/" },
                    { Enrollment, "http://matricula.unj.edu.pe/" },
                    { Escalafon, "http://escalafon.unj.edu.pe/" },
                    { InstitutionalWelfare, "http://bienestar.unj.edu.pe/" },
                    { Intranet, "http://intranet.unj.edu.pe/" },
                    { JobExchange, "http://bolsa.unj.edu.pe/" },
                    { Laurassia, "http://aulavirtual.unj.edu.pe/" },
                    { TeachingManagement, "http://docente.unj.edu.pe/" },
                    { Tutoring, "http://tutoria.unj.edu.pe/" },
                } },
                  {
                    ConstantHelpers.Institution.UNCP, new Dictionary<int, string> {
                        { Server, "https://erpcampus.uncp.edu.pe/" },
                        { Administration, "" },
                        { Admission, "https://erpadmision.uncp.edu.pe/" },
                        { Degree, "https://erpgradosytitulos.uncp.edu.pe/" },
                        { DocumentaryProcedure , "https://erptramitedoc.uncp.edu.pe/" },
                        { EconomicManagement, "https://erpcaja.uncp.edu.pe/" },
                        { Enrollment, "https://erpmatricula.uncp.edu.pe/" },
                        { Escalafon, "https://erpescalafon.uncp.edu.pe/" },
                        { Evaluation, "https://erpresponsabilidad.uncp.edu.pe/" },
                        { InstitutionalWelfare, "https://erpbienestar.uncp.edu.pe/" },
                        { Indicators, "https://erpindicadores.uncp.edu.pe/" },
                        { InterestGroup, "" },
                        { Intranet, "https://erpintranet.uncp.edu.pe/" },
                        { Investigation, "https://erpinvestigacionformativa.uncp.edu.pe/" },
                        { JobExchange, "https://erpseguimientoegre.uncp.edu.pe/" },
                        { Laurassia, "https://erpaprendizaje.uncp.edu.pe/" },
                        { Quibuk, "" },
                        { Sisco, "" },
                        { TeachingManagement, "https://erpgestiondocente.uncp.edu.pe/" },
                        { TransparencyPortal, "https://erptransparencia.uncp.edu.pe/"},
                        { Tutoring, "https://erptutoria.uncp.edu.pe/" },
                        { University_Extension, "" },
                        { ContinuousTraining, "https://erpformacioncontinua.uncp.edu.pe/" },
                        { AcademicExchange, "https://erpcooperacion.uncp.edu.pe/"},
                        { TeacherHiring, "https://erpcontratadocente.uncp.edu.pe/"},
                        { PreProfessionalPractice, "https://erppracticas.uncp.edu.pe/"},
                        { ResolutiveActs, "https://erpresoluciones.uncp.edu.pe/" },
                        { HelpDesk2 , "https://erphelpdesk.uncp.edu.pe/"},
                        { VisitManagement , ""}

                  }
                },
                  { ConstantHelpers.Institution.EESPLI, new Dictionary<int, string> {
                    { Server, "https://campus.eespli.edu.pe/" },
                    { Admission, "" },
                    { Degree, "https://gradosytitulos.eespli.edu.pe/" },
                    { DocumentaryProcedure, "https://tramites.eespli.edu.pe/" },
                    { EconomicManagement, "https://tesoreria.eespli.edu.pe/" },
                    { Enrollment, "https://matricula.eespli.edu.pe/" },
                    { Escalafon, "" },
                    { InstitutionalWelfare, "" },
                    { Intranet, "https://intranet.eespli.edu.pe/" },
                    { JobExchange, "https://bolsa.eespli.edu.pe/" },
                    { TeachingManagement, "https://docente.eespli.edu.pe/" },
                } },
            };
        }
        public static class JobOfferApplication
        {
            public static class Status
            {
                public const int PENDING = 1;
                public const int IN_PROGRESS = 2;
                public const int ACCEPTED = 3;
                public const int REJECTED = 4;
                public const int CANCELED = 5;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDING, "Pendiente" },
                    { IN_PROGRESS, "En Proceso" },
                    { ACCEPTED, "Aceptado" },
                    { REJECTED, "Rechazado" },
                    { CANCELED, "Cancelado" }
                };
            }
        }
        public static class AGREEMENTFORMAT
        {
            public static class STATES
            {
                public const int PENDING = 0;
                public const int OBSERVATED = 1;
                public const int ACCEPTED = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDING, "Pendiente" },
                    { OBSERVATED, "Observado" },
                    { ACCEPTED, "Aceptado" }
                };
            }
        }
        public static class COMPANIES
        {
            public static class TYPES
            {
                public const byte UNIPERSONAL = 1;
                public const byte INDIVIDUAL_LIMITED_COMPANY = 2;
                public const byte ANONYMOUS_SOCIETY = 3;
                public const byte ANONYMOUS_SOCIETY_OPEN = 4;
                public const byte ANONYMOUS_SOCIETY_CLOSE = 5;
                public const byte LIMITED_LIABILITY_COMPANY = 6;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { UNIPERSONAL, "Empresa Unipersonal" },
                    { INDIVIDUAL_LIMITED_COMPANY, "Empresa Individual de Responsabilidad Limitada" },
                    { ANONYMOUS_SOCIETY, "Sociedad Anónima" },
                    { ANONYMOUS_SOCIETY_OPEN, "Sociedad Anónima Abierta" },
                    { ANONYMOUS_SOCIETY_CLOSE, "Sociedad Anónima Cerrada" },
                    { LIMITED_LIABILITY_COMPANY, "Sociedad Comercial de Responsabilidad Limitada" }
                };
            }
            public static class SIZES
            {
                public const byte MICRO = 1;
                public const byte SMALL = 2;
                public const byte MEDIUM = 3;
                public const byte BIG = 4;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { MICRO, "Micro Empresa" },
                    { SMALL, "Empresa Pequeña" },
                    { MEDIUM, "Empresa Mediana" },
                    { BIG, "Empresa Grande" }
                };
            }
        }
        public static class POSTULANTS
        {
            public static class ApplicationStatus
            {
                public const int PENDING = 0;
                public const int ADMITTED = 1;
                public const int NOTADMITTED = 2;
                public const byte MANUAL_APPROVAL = 3;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDING, "Pendiente" },
                    { ADMITTED, "Admitido" },
                    { NOTADMITTED, "No Admitido" },
                    {MANUAL_APPROVAL, "Aprobación manual" },
                };
            }
        }
        public static class SequenceOrder2
        {
            public const byte NINGUNO = 0;
            public const byte PRIMERO = 1;
            public const byte SEGUNDO = 2;
            public const byte TERCERO = 3;
            public const byte CUARTO = 4;
            public const byte QUINTO = 5;
            public const byte SEXTO = 6;
            public const byte SEPTIMO = 7;
            public const byte OCTAVO = 8;
            public const byte NOVENO = 9;
            public const byte DECIMO = 10;
            public const byte UNDECIMO = 11;
            public const byte DUODECIMO = 12;
            public const byte DECIMOTERCERO = 13;
            public const byte DECIMOCUARTO = 14;
            public const byte DECIMOQUINTO = 15;
            public const byte DECIMOSEXTO = 16;
            public const byte DECIMOSEPTIMO = 17;
            public const byte DECIMOOCTAVO = 18;
            public const byte DECIMONOVENO = 19;
            public const byte VIGESIMO = 20;
            public const byte VIGESIMOPRIMERO = 21;
            public const byte VIGESIMOSEGUNDO = 22;
            public const byte VIGESIMOTERCERO = 23;
            public const byte VIGESIMOCUARTO = 24;
            public const byte VIGESIMOQUINTO = 25;
            public const byte VIGESIMOSEXTO = 26;
            public const byte VIGESIMOSEPTIMO = 27;
            public const byte VIGESIMOOCTAVO = 28;
            public const byte VIGESIMONOVENO = 29;
            public const byte TRIGESIMO = 30;
            public static Dictionary<int, string> SEQUENCEORDER = new Dictionary<int, string>
                {
                    { NINGUNO, "" },
                    { PRIMERO, "PRIMERO" },
                    { SEGUNDO, "SEGUNDO" },
                    { TERCERO, "TERCERO" },
                    { CUARTO, "CUARTO" },
                    { QUINTO, "QUINTO" },
                    { SEXTO, "SEXTO" },
                    { SEPTIMO, "SÉPTIMO" },
                    { OCTAVO, "OCTAVO" },
                    { NOVENO, "NOVENO" },
                    { DECIMO, "DÉCIMO" },
                    { UNDECIMO, "UNDÉCIMO" },
                    { DUODECIMO, "DUODÉCIMO" },
                    { DECIMOTERCERO, "DÉCIMOTERCERO" },
                    { DECIMOCUARTO, "DÉCIMOCUARTO" },
                    { DECIMOQUINTO, "DÉCIMOQUINTO" },
                    { DECIMOSEXTO, "DÉCIMOSEXTO" },
                    { DECIMOSEPTIMO, "DÉCIMOSEPTIMO" },
                    { DECIMONOVENO, "DÉCIMONOVENO" },
                    { VIGESIMO, "VIGÉSIMO" },
                    { VIGESIMOPRIMERO, "VIGÉSIMOPRIMERO" },
                    { VIGESIMOSEGUNDO, "VIGÉSIMOSEGUNDO" },
                    { VIGESIMOTERCERO, "VIGÉSIMOTERCERO" },
                    { VIGESIMOCUARTO, "VIGÉSIMOCUARTO" },
                    { VIGESIMOQUINTO, "VIGÉSIMOQUINTO" },
                    { VIGESIMOSEXTO, "VIGÉSIMOSEXTO" },
                    { VIGESIMOSEPTIMO, "VIGÉSIMOSEPTIMO" },
                    { VIGESIMOOCTAVO, "VIGÉSIMOOCTAVO" },
                    { VIGESIMONOVENO, "VIGÉSIMONOVENO" },
                    { TRIGESIMO, "TRIGÉSIMO" },
                };
        }
        #region BIENESTAR
        public static class INSTITUTIONAL_WELFARE_SCHOLARSHIP
        {
            public static class STUDENT_STATUS
            {
                public const byte UNANSWERED = 1;
                public const byte ACCEPTED = 2;
                public const byte DENIED = 3;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                {
                    { UNANSWERED, "Postulado" },
                    { ACCEPTED, "Aceptado" },
                    { DENIED, "Denegado" }
                };
            }
        }
        public static class MEDICAL_APPOINTMENT
        {
            public static class ABSENCE_JUSTIFICATION_STATUS
            {
                public const byte PENDING = 1;
                public const byte JUSTIFIED = 2;
                public const byte JUSTIFICATION_DENIED = 3;
                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                {
                    { PENDING, "Pendiente" },
                    { JUSTIFIED, "Justificado" },
                    { JUSTIFICATION_DENIED, "Justificación Denegada" }
                };
            }
        }
        #endregion
        public static class SequenceOrder
        {
            public const byte NINGUNO = 0;
            public const byte PRIMERO = 1;
            public const byte SEGUNDO = 2;
            public const byte TERCERO = 3;
            public const byte CUARTO = 4;
            public const byte QUINTO = 5;
            //public const byte SEXTO = 6;
            //public const byte SEPTIMO = 7;
            //public const byte OCTAVO = 8;
            //public const byte NOVENO = 9;
            //public const byte DECIMO = 10;
            //public const byte UNDECIMO = 11;
            //public const byte DUODECIMO = 12;
            //public const byte DECIMOTERCERO = 13;
            //public const byte DECIMOCUARTO = 14;
            //public const byte DECIMOQUINTO = 15;
            //public const byte DECIMOSEXTO = 16;
            //public const byte DECIMOSEPTIMO = 17;
            //public const byte DECIMOOCTAVO = 18;
            //public const byte DECIMONOVENO = 19;
            //public const byte VIGESIMO = 20;
            //public const byte VIGESIMOPRIMERO = 21;
            //public const byte VIGESIMOSEGUNDO = 22;
            //public const byte VIGESIMOTERCERO = 23;
            //public const byte VIGESIMOCUARTO = 24;
            //public const byte VIGESIMOQUINTO = 25;
            //public const byte VIGESIMOSEXTO = 26;
            //public const byte VIGESIMOSEPTIMO = 27;
            //public const byte VIGESIMOOCTAVO = 28;
            //public const byte VIGESIMONOVENO = 29;
            //public const byte TRIGESIMO = 30;
            public static Dictionary<int, string> SEQUENCEORDER = new Dictionary<int, string>
                {
                    { NINGUNO, "" },
                    { PRIMERO, "PRIMERO" },
                    { SEGUNDO, "SEGUNDO" },
                    { TERCERO, "TERCERO" },
                    { CUARTO, "CUARTO" },
                    { QUINTO, "QUINTO" },
                    //{ SEXTO, "SEXTO" },
                    //{ SEPTIMO, "SÉPTIMO" },
                    //{ OCTAVO, "OCTAVO" },
                    //{ NOVENO, "NOVENO" },
                    //{ DECIMO, "DÉCIMO" },
                    //{ UNDECIMO, "UNDÉCIMO" },
                    //{ DUODECIMO, "DUODÉCIMO" },
                    //{ DECIMOTERCERO, "DÉCIMOTERCERO" },
                    //{ DECIMOCUARTO, "DÉCIMOCUARTO" },
                    //{ DECIMOQUINTO, "DÉCIMOQUINTO" },
                    //{ DECIMOSEXTO, "DÉCIMOSEXTO" },
                    //{ DECIMOSEPTIMO, "DÉCIMOSEPTIMO" },
                    //{ DECIMONOVENO, "DÉCIMONOVENO" },
                    //{ VIGESIMO, "VIGÉSIMO" },
                    //{ VIGESIMOPRIMERO, "VIGÉSIMOPRIMERO" },
                    //{ VIGESIMOSEGUNDO, "VIGÉSIMOSEGUNDO" },
                    //{ VIGESIMOTERCERO, "VIGÉSIMOTERCERO" },
                    //{ VIGESIMOCUARTO, "VIGÉSIMOCUARTO" },
                    //{ VIGESIMOQUINTO, "VIGÉSIMOQUINTO" },
                    //{ VIGESIMOSEXTO, "VIGÉSIMOSEXTO" },
                    //{ VIGESIMOSEPTIMO, "VIGÉSIMOSEPTIMO" },
                    //{ VIGESIMOOCTAVO, "VIGÉSIMOOCTAVO" },
                    //{ VIGESIMONOVENO, "VIGÉSIMONOVENO" },
                    //{ TRIGESIMO, "TRIGÉSIMO" },
                };
        }
        #region Generals
        public static class Generals
        {
            public static class Holiday
            {
                public static class Type
                {
                    public const byte National = 0;
                    public const byte Universitary = 1;
                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { National, "Nacional" },
                    { Universitary, "Universitario" }
                };
                }
            }
        }

        #endregion
        #region Agenda
        public static class Agenda
        {
            public static class AgendaEvent
            {
                public static class Type
                {
                    public const byte Informative = 0;
                    public const byte Event = 1;
                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        { Informative, "Informativa" },
                        { Event, "Evento" }
                    };
                }
            }
        }
        #endregion
        public static class Admission
        {
            public static class Postulant
            {
                public static class AdmissionState
                {
                    public const byte PENDING = 0;
                    public const byte ADMITTED = 1;
                    public const byte NOT_ADMITTED = 2;
                    public const byte MANUAL_APPROVAL = 3;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        {PENDING, "Pendiente" },
                        {ADMITTED, "Admitido" },
                        {NOT_ADMITTED, "No admitido" },
                        {MANUAL_APPROVAL, "Aprobación manual" },
                    };
                }
            }

            public static class ExamResultTypes
            {
                public const byte MANUAL = 1;
                public const byte AUTOMATIC = 2;
            }

            public static class School
            {
                public static class Type
                {
                    public const byte DIRECT_MANAGEMENT_PUBLIC_SCHOOL = 1;
                    public const byte PRIVATE_MANAGEMENT_PUBLIC_SCHOOL = 2;
                    public const byte PRIVATE_SCHOOL = 3;

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        {DIRECT_MANAGEMENT_PUBLIC_SCHOOL, "Pública de gestión directa" },
                        {PRIVATE_MANAGEMENT_PUBLIC_SCHOOL, "Pública de gestión privada" },
                        {PRIVATE_SCHOOL, "Privada" },
                    };
                }
            }
        }
        public static class Course
        {
            public static class Modality
            {
                public const byte PRESENTIAL = 1;
                public const byte SEMI_PRESENTIAL = 2;
                public const byte VIRTUAL = 3;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    {PRESENTIAL, "Presencial" },
                    {SEMI_PRESENTIAL, "Semi-Presencial" },
                    {VIRTUAL, "Virtual" }
                };
            }
        }
        public static class Authentication
        {
            public static class SingleSignOn
            {
                public const string LOCALHOST_AUTHORITY = "https://localhost:44376";
                public static Dictionary<int, string> Authorities = new Dictionary<int, string>()
                {
                    { Institution.Akdemic, "https://sigau-campus.akdemic.com" },
                    { Institution.PMESUT, "https://sigau-campus.akdemic.com" },
                    { Institution.GRAY, "https://sigau-campus.akdemic.com" },
                    { Institution.UNSCH, "https://campus.unsch.edu.pe" },
                    { Institution.UNICA, "https://campus.unica.edu.pe" },
                    { Institution.UNF, "https://campusvirtual.unf.edu.pe" },
                    { Institution.UNAB, "http://campus.unab.edu.pe" },
                    { Institution.UNAH, "https://campus.unah.edu.pe" },
                    { Institution.UNCP, "https://erpcampus.uncp.edu.pe" },
                    //{ Institution.UNCP, "https://uncp-campus.akdemic.com" },
                    { Institution.EESPLI, "https://campus.eespli.edu.pe/" },
                    { Institution.UNSM, "https://campusvirtualpregrado.unsm.edu.pe/" },
                    { Institution.UNTUMBES, "https://campussigau.untumbes.edu.pe" },

                };
            }
        }
        public static class AcademicProgram
        {
            public static class Type
            {
                public const byte UNDEFINED = 0;
                public const byte RECOGNIZED_BY_DEGREE = 1;
                public const byte FOR_DEGREE_PURPOSES = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    {UNDEFINED, "-" },
                    {RECOGNIZED_BY_DEGREE, "Reconocido por Lic." },
                    {FOR_DEGREE_PURPOSES, "Para efectos de grados" }
                };
            }
        }
        public static class CONDITIONAL_ANSWER
        {
            public const bool NO = false;
            public const bool YES = true;
            public static Dictionary<bool, string> VALUES = new Dictionary<bool, string>()
                    {
                        { NO, "No" },
                        { YES, "Sí"}
                    };
        }

        public static class TeacherPortfolio
        {
            public static class Folder
            {
                public const byte CV = 1;
                public const byte CURRICULUM = 2;
                public const byte SYLLABUS = 3;
                public const byte CALENDAR = 4;
                public const byte SCHEDULE = 5;
                public const byte CHARGUE = 6;
                public const byte COMMISSIONS = 7;
                public const byte DESIGNS = 8;
                public const byte MATERIALS = 9;
                public const byte INSTRUMENTS = 10;
                public const byte ATTENDANCE = 11;
                public const byte TRACING = 12;

                public static Dictionary<byte, string> NAMES = new Dictionary<byte, string>()
                {
                    //{ CV, "Cv Descriptivo" },
                    //{ CURRICULUM, "Plan de Estudio" },
                    //{ SYLLABUS, "Silabos" },
                    //{ CALENDAR, "Calendario" },
                    //{ SCHEDULE, "Horario" },
                    //{ CHARGUE, "Carga Lectiva" },
                    //{ COMMISSIONS, "Comisiones" },
                    { DESIGNS, "Diseños" },
                    //{ MATERIALS, "Materiales" },
                    //{ INSTRUMENTS, "Instrumentos" },
                    //{ ATTENDANCE, "Asistencia" },
                    { TRACING, "Seguimiento" },
                };

                public static Dictionary<byte, string> ROUTES = new Dictionary<byte, string>()
                {
                    //{ CV, "cv-descriptivo" },
                    //{ CURRICULUM, "plan-de-estudio" },
                    //{ SYLLABUS, "silabos" },
                    //{ CALENDAR, "calendario" },
                    //{ SCHEDULE, "horario" },
                    //{ CHARGUE, "cargalectiva" },
                    //{ COMMISSIONS, "comisiones" },
                    { DESIGNS, "diseños" },
                    //{ MATERIALS, "materiales" },
                    //{ INSTRUMENTS, "instrumentos" },
                    //{ ATTENDANCE, "asistencia" },
                    { TRACING, "seguimiento" },
                };
            }
        }

        public static class RESOLUTIVE_ACT
        {

            public static class DOCUMENT
            {
                public static class STATUS
                {
                    //Genera el secretario y en este estado el validor debe revisarlo para aprobarlo o rechazarlo
                    public const byte GENERATED = 1;
                    public const byte APPROVED = 2;
                    public const byte DENIED = 3;

                    //Una vez el secretario suscriba el director puede emitir
                    public const byte SUBSCRIBE = 4;

                    public const byte FINISHED = 5;


                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        { GENERATED, "Generado" },
                        { APPROVED, "Aprobado" },
                        { DENIED ,"Denegado" },
                        { SUBSCRIBE ,"Enviado" },
                        { FINISHED ,"Finalizado" },
                    };
                }

                public static class TYPE
                {
                    public const byte ACT = 1;
                    public const byte RESOLUTION = 2;

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        { ACT, "Acta" },
                        { RESOLUTION, "Resolución" }
                    };
                }
            }
        }

        public static class PreprofesionalPractice
        {
            public static class InternshipAspect
            {
                public static class Type
                {
                    public const byte DEVELOPMENT_PLAN = 1;
                    public const byte WEEKLY_MONITORING = 2;
                    public const byte PARTIAL_REPORT = 3;
                    public const byte FINAL_REPORT = 4;
                    public const byte DEVELOPMENT_MONITORING_REPORT = 5;
                    public const byte WORKPLACE_ASSESSMENT = 6;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { DEVELOPMENT_PLAN, "PLAN DE DESARROLLO DE PPP" },
                        { WEEKLY_MONITORING, "REGISTRO DE MONITOREO SEMANAL DEL PRACTICANTE" },
                        { PARTIAL_REPORT, "INFORME PARCIAL DESARROLLO DE PPP" },
                        { FINAL_REPORT, "INFORME FINAL DEL DESARROLLO DE PPP" },
                        { DEVELOPMENT_MONITORING_REPORT ,"INFORME DE SUPERVISIÓN DE DESARROLLO DE PP" },
                        { WORKPLACE_ASSESSMENT ,"EVALUACIÓN DEL CENTRO LABORAL" },
                    };
                }
            }
            public static class InternshipRequest
            {
                public static class Status
                {
                    public const byte PENDING = 0;
                    public const byte VALIDATED = 1;
                    public const byte ASSIGNED = 2;
                    public const byte FINISHED = 3;
                    public const byte INVALIDATED = 99;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        {PENDING, "Pendiente" },
                        {VALIDATED, "Procede" },
                        {ASSIGNED, "Asignada" },
                        { FINISHED, "Finalizado" },
                        {INVALIDATED, "No procede" }
                    };
                }
            }

            public static class InternshipRequestFile
            {
                public static class Type
                {
                    public const byte POSTULANT = 1;
                    public const byte ADVISER = 2;
                }
            }
        }

        public static class ExtracurricularArea
        {
            public static class Type
            {
                public const byte COURSE = 1;
                public const byte ACTIVITY = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        {COURSE, "Curso" },
                        {ACTIVITY, "Actividad" },
                    };
            }
        }
    }
}