using System.Diagnostics;

namespace AKDEMIC.CORE.Environments
{
    public enum EnvironmentType
    {
        Debug,
        Production,
        Staging
    }

    public class Environment
    {
        public static void Load()
        {
            //if (environmentType == EnvironmentType.Production)
            //{
                //EnvironmentProduction.Load();
            //}
            //else if (environmentType == EnvironmentType.Staging)
            //{
            //    //EnvironmentStaging.Load();
            //}
            //else
            //{
            //    EnvironmentDebug.Load();
            //}
        }

        public class WebServices
        {
            public class PIDE
            {
                public class Credentials
                {
                    public static string Dni { get; set; } = "15862643";
                    public static string Password { get; set; } = "15862643";
                    public static string Ruc { get; set; } = "20542068281";
                    public static string IdEntidad { get; set; } = "270001";
                    public static string Server_MAC { get; set; } = "00:16:3C:3A:F5:8D";
                    public static string Server_IP { get; set; } = "67.211.211.27";
                }

                public class Methods
                {
                    public class REST
                    {
                        public class Query
                        {
                            public static string Url_Dni { get; internal set; } = "https://ws5.pide.gob.pe/Rest/Reniec/Consultar";
                            public static string Url_Ruc { get; internal set; } = "https://ws3.pide.gob.pe/Rest/Sunat";
                            public static string Url_Grados { get; internal set; } = "https://ws3.pide.gob.pe/Rest/Sunedu/Grados";
                        }
                    }

                    public class SOAP
                    {
                        public static string WDSL { get; internal set; } = "https://ws5.pide.gob.pe/services/ReniecConsultaDni";

                        public class Query
                        {
                            public static string Url { get; internal set; } = "https://ws5.pide.gob.pe/services/ReniecConsultaDni?wsdl";
                        }
                    }
                }
            }
        }
    }
}
