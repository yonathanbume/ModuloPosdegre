namespace AKDEMIC.CORE.Environments
{
    public static class EnvironmentProduction
    {
        public static void Load()
        {
            LoadWebServices();
        }

        private static void LoadWebServices()
        {
            Environment.WebServices.PIDE.Methods.REST.Query.Url_Dni = "https://ws5.pide.gob.pe/Rest/Reniec/Consultar";
            Environment.WebServices.PIDE.Methods.SOAP.WDSL = "https://ws5.pide.gob.pe/services/ReniecConsultaDni";
            Environment.WebServices.PIDE.Methods.SOAP.Query.Url = "https://ws5.pide.gob.pe/services/ReniecConsultaDni?wsdl";
            //Environment.WebServices.PIDE.Methods.REST.Query.Url = "http://181.176.223.32:1337/consultar.php";
            //Environment.WebServices.PIDE.Methods.SOAP.WDSL = "http://181.176.223.32:1337/consultar2.php";
            //Environment.WebServices.PIDE.Methods.SOAP.Query.Url = "http://181.176.223.32:1337/consultar3.php";
        }
    }
}
