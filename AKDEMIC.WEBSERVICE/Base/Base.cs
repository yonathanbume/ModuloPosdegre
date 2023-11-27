using AKDEMIC.CORE.Helpers;
using AKDEMIC.WEBSERVICE.Base.Models;

namespace AKDEMIC.WEBSERVICE.Base
{
    public class Base
    {
        protected Credentials Credentials { get; set; }
        protected RestUrl REST { get; set; }
        protected SoapUrl SOAP { get; set; }

        public Base()
        {
            REST = new RestUrl
            {
                Url_Dni = "https://ws5.pide.gob.pe/Rest/Reniec/Consultar",
                ////Url_Dni = "http://181.176.223.32:1337/consultar.php",
                Url_Ruc = "https://ws3.pide.gob.pe/Rest/Sunat",
                Url_Grados = "https://ws3.pide.gob.pe/Rest/Sunedu/Grados"
            };

            SOAP = new SoapUrl
            {
                Url = "https://ws5.pide.gob.pe/services/ReniecConsultaDni?wsdl",
                WDSL = "https://ws5.pide.gob.pe/services/ReniecConsultaDni"
            };


            switch (ConstantHelpers.GENERAL.Institution.Value)
            {
                case ConstantHelpers.Institution.UNICA:
                    Credentials = new Credentials
                    {
                        Dni = "40205066",
                        Password = "40205066",
                        Ruc = "20148421014"
                    };
                    break;

                case ConstantHelpers.Institution.UNAB:
                    Credentials = new Credentials
                    {
                        Dni = "15862643",
                        Password = "15862643",
                        Ruc = "20542068281",
                        IdEntidad = "270001",
                        Server_MAC = "00:16:3C:3A:F5:8D",
                        Server_IP = "67.211.211.27"
                    };
                    break;
            }
        }
    }
}
