namespace AKDEMIC.CORE.Environments
{
    public class EnvironmentDebug
    {
        public static void Load()
        {
            LoadWebServices();
        }

        private static void LoadWebServices()
        {
            Environment.WebServices.PIDE.Methods.REST.Query.Url_Dni = "http://181.176.223.32:1337/consultar.php";
            Environment.WebServices.PIDE.Methods.SOAP.WDSL = "http://181.176.223.32:1337/consultar2.php";
            Environment.WebServices.PIDE.Methods.SOAP.Query.Url = "http://181.176.223.32:1337/consultar3.php";
        }
    }
}
