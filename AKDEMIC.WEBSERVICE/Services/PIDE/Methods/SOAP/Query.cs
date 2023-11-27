using System.Threading.Tasks;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Methods.SOAP
{
    public class Query : Base.Methods.SOAP
    {
        public Query() : base() { }

        public async Task<Models.SOAP.QueryResponse.Envelope> Post(string dni)
        {
            var envelope = new Models.SOAP.QueryRequest.Envelope
            {
                Body = new Models.SOAP.QueryRequest.Body
                {
                    GetDatosDni = new Models.SOAP.QueryRequest.GetDatosDni
                    {
                        PeticionConsulta = new Models.SOAP.QueryRequest.PeticionConsulta
                        {
                            NuDniConsulta = dni,
                            NuDniUsuario = CORE.Environments.Environment.WebServices.PIDE.Credentials.Dni,
                            NuRucUsuario = CORE.Environments.Environment.WebServices.PIDE.Credentials.Ruc,
                            Password = CORE.Environments.Environment.WebServices.PIDE.Credentials.Password
                        }
                    }
                }
            };

            var result = await PostXml<Models.SOAP.QueryRequest.Envelope, Models.SOAP.QueryResponse.Envelope>(envelope);

            return result;
        }
    }
}
