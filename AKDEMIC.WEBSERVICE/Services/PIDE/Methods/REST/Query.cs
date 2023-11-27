using AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.DniResponse;
using AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.GradosResponse;
using AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.RucResponse;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Methods.REST
{
    public class Query : Base.Methods.REST
    {
        private readonly IActionContextAccessor _accessor;

        public Query( IActionContextAccessor accessor ) : base()
        {
            _accessor = accessor;
        }

        public async Task<ConsultarResponse> GetDni(string dni)
        {
            var requestUri = $"{REST.Url_Dni}?nuDniConsulta={dni}&nuDniUsuario={Credentials.Dni}&nuRucUsuario={Credentials.Ruc}&password={Credentials.Password}";
            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var consultarResponse = new ConsultarResponse
                {
                    Return = new Return
                    {
                        CoResultado = xmlDocument.SelectSingleNode("//coResultado")?.InnerText,
                        DatosPersona = new DatosPersona
                        {
                            ApPrimer = xmlDocument.SelectSingleNode("//apPrimer")?.InnerText,
                            ApSegundo = xmlDocument.SelectSingleNode("//apSegundo")?.InnerText,
                            Direccion = xmlDocument.SelectSingleNode("//direccion")?.InnerText,
                            EstadoCivil = xmlDocument.SelectSingleNode("//estadoCivil")?.InnerText,
                            Foto = xmlDocument.SelectSingleNode("//foto")?.InnerText,
                            Prenombres = xmlDocument.SelectSingleNode("//prenombres")?.InnerText,
                            Restriccion = xmlDocument.SelectSingleNode("//restriccion")?.InnerText,
                            Ubigeo = xmlDocument.SelectSingleNode("//ubigeo")?.InnerText
                        },
                        DeResultado = xmlDocument.SelectSingleNode("//deResultado")?.InnerText
                    }
                };

                return consultarResponse;
            });

            return result;
        }

        public async Task<DatosPrincipalesResponse> GetMainDataByRuc(string ruc)
        {
            var requestUri = $"{Credentials.Ruc}/DatosPrincipales?numruc={ruc}";
            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var consultarResponse = new DatosPrincipalesResponse
                {
                    CodDep = xmlDocument.SelectSingleNode("//cod_dep")?.InnerText,
                    CodDist = xmlDocument.SelectSingleNode("//cod_dist")?.InnerText,
                    CodProv = xmlDocument.SelectSingleNode("//cod_prov")?.InnerText,
                    DdpCiiu = xmlDocument.SelectSingleNode("//ddp_ciiu")?.InnerText,
                    DdpDoble = xmlDocument.SelectSingleNode("//ddp_doble")?.InnerText,
                    DdpEstado = xmlDocument.SelectSingleNode("//ddp_estado")?.InnerText,
                    DdpFecAct = xmlDocument.SelectSingleNode("//ddp_fecact")?.InnerText,
                    DdpFecAlt = xmlDocument.SelectSingleNode("//ddp_fecalt")?.InnerText,
                    DdpFecBaj = xmlDocument.SelectSingleNode("//ddp_fecbaj")?.InnerText,
                    DdpFlag22 = xmlDocument.SelectSingleNode("//ddp_flag22")?.InnerText,
                    DdpIdenti = xmlDocument.SelectSingleNode("//ddp_identi")?.InnerText,
                    DdpInter1 = xmlDocument.SelectSingleNode("//ddp_identi")?.InnerText,
                    DdpLlllTttt = xmlDocument.SelectSingleNode("//ddp_lllttt")?.InnerText,
                    DdpMClase = xmlDocument.SelectSingleNode("//ddp_mclase")?.InnerText,
                    DdpNombre = xmlDocument.SelectSingleNode("//ddp_nombre")?.InnerText,
                    DdpNomVia = xmlDocument.SelectSingleNode("//ddp_nomvia")?.InnerText,
                    DdpNomZon = xmlDocument.SelectSingleNode("//ddp_nomzon")?.InnerText,
                    DdpNumer1 = xmlDocument.SelectSingleNode("//ddp_numer1")?.InnerText,
                    DdpNumReg = xmlDocument.SelectSingleNode("//ddp_numreg")?.InnerText,
                    DdpNumRuc = xmlDocument.SelectSingleNode("//ddp_numruc")?.InnerText,
                    DdpReacti = xmlDocument.SelectSingleNode("//ddp_reacti")?.InnerText,
                    DdpRefer1 = xmlDocument.SelectSingleNode("//ddp_refer1")?.InnerText,
                    DdpSecuen = xmlDocument.SelectSingleNode("//ddp_secuen")?.InnerText,
                    DdpTamano = xmlDocument.SelectSingleNode("//ddp_tamano")?.InnerText,
                    DdpTipVia = xmlDocument.SelectSingleNode("//ddp_tipvia")?.InnerText,
                    DdpTipZon = xmlDocument.SelectSingleNode("//ddp_tipzon")?.InnerText,
                    DdpTpoEmp = xmlDocument.SelectSingleNode("//ddp_tpoemp")?.InnerText,
                    DdpUbigeo = xmlDocument.SelectSingleNode("//ddp_ubigeo")?.InnerText,
                    DdpUserna = xmlDocument.SelectSingleNode("//ddp_userna")?.InnerText,
                    DescCiiu = xmlDocument.SelectSingleNode("//desc_ciiu")?.InnerText,
                    DescDep = xmlDocument.SelectSingleNode("//desc_dep")?.InnerText,
                    DescDist = xmlDocument.SelectSingleNode("//desc_dist")?.InnerText,
                    DescEstado = xmlDocument.SelectSingleNode("//desc_estado")?.InnerText,
                    DescFlag22 = xmlDocument.SelectSingleNode("//desc_flag22")?.InnerText,
                    DescIdenti = xmlDocument.SelectSingleNode("//desc_identi")?.InnerText,
                    DescNumReg = xmlDocument.SelectSingleNode("//desc_numreg")?.InnerText,
                    DescProv = xmlDocument.SelectSingleNode("//desc_prov")?.InnerText,
                    DescTamano = xmlDocument.SelectSingleNode("//desc_tamano")?.InnerText,
                    DescTipVia = xmlDocument.SelectSingleNode("//desc_tipvia")?.InnerText,
                    DescTipZon = xmlDocument.SelectSingleNode("//desc_tipzon")?.InnerText,
                    DescTpoEmp = xmlDocument.SelectSingleNode("//desc_tpoemp")?.InnerText,
                    EsActivo = xmlDocument.SelectSingleNode("//esActivo")?.InnerText,
                    EsHabido = xmlDocument.SelectSingleNode("//esHabido")?.InnerText
                };

                return consultarResponse;
            });

            return result;
        }

        public async Task<DatosSecundariosResponse> GetSecondayDataByRuc(string ruc)
        {
            var requestUri = $"{Credentials.Ruc}/DatosSecundarios?numruc={ruc}";
            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var consultarResponse = new DatosSecundariosResponse
                {
                    DdsCalifi = xmlDocument.SelectSingleNode("//dds_califi")?.InnerText,
                    DdsComext = xmlDocument.SelectSingleNode("//dds_comext")?.InnerText,
                    DescComext = xmlDocument.SelectSingleNode("//desc_comext")?.InnerText,
                    DdsConsti = xmlDocument.SelectSingleNode("//dds_consti")?.InnerText,
                    DdsContab = xmlDocument.SelectSingleNode("//dds_contab")?.InnerText,
                    DescContab = xmlDocument.SelectSingleNode("//desc_contab")?.InnerText,
                    DdsDocide = xmlDocument.SelectSingleNode("//dds_docide")?.InnerText,
                    DescDocide = xmlDocument.SelectSingleNode("//desc_docide")?.InnerText,
                    DdsNrodoc = xmlDocument.SelectSingleNode("//dds_nrodoc")?.InnerText,
                    DdsDomici = xmlDocument.SelectSingleNode("//dds_domici")?.InnerText,
                    DescDomici = xmlDocument.SelectSingleNode("//desc_domici")?.InnerText,
                    DdsFecact = xmlDocument.SelectSingleNode("//dds_fecact")?.InnerText,
                    DescFactur = xmlDocument.SelectSingleNode("//desc_factur")?.InnerText,
                    DdsFecnac = xmlDocument.SelectSingleNode("//dds_fecnac")?.InnerText,
                    DdsAsient = xmlDocument.SelectSingleNode("//dds_asient")?.InnerText,
                    DdsFicha = xmlDocument.SelectSingleNode("//dds_ficha")?.InnerText,
                    DdsNfolio = xmlDocument.SelectSingleNode("//dds_nfolio")?.InnerText,
                    DdsInicio = xmlDocument.SelectSingleNode("//dds_inicio")?.InnerText,
                    DdsLicenc = xmlDocument.SelectSingleNode("//dds_licenc")?.InnerText,
                    DdsNacion = xmlDocument.SelectSingleNode("//dds_nacion")?.InnerText,
                    DdsNomcom = xmlDocument.SelectSingleNode("//dds_nomcom")?.InnerText,
                    DdsNumruc = xmlDocument.SelectSingleNode("//dds_numruc")?.InnerText,
                    DdsOrient = xmlDocument.SelectSingleNode("//dds_orient")?.InnerText,
                    DescOrient = xmlDocument.SelectSingleNode("//desc_orient")?.InnerText,
                    DdsPaispa = xmlDocument.SelectSingleNode("//dds_paispa")?.InnerText,
                    DdsPasapo = xmlDocument.SelectSingleNode("//dds_pasapo")?.InnerText,
                    DdsPatron = xmlDocument.SelectSingleNode("//dds_patron")?.InnerText,
                    DdsSexo = xmlDocument.SelectSingleNode("//dds_sexo")?.InnerText,
                    DescSexo = xmlDocument.SelectSingleNode("//desc_sexo")?.InnerText,
                    DdsTelef1 = xmlDocument.SelectSingleNode("//dds_telef1")?.InnerText,
                    DdsTelef2 = xmlDocument.SelectSingleNode("//dds_telef2")?.InnerText,
                    DdsTelef3 = xmlDocument.SelectSingleNode("//dds_telef3")?.InnerText,
                    DdsNumfax = xmlDocument.SelectSingleNode("//dds_numfax")?.InnerText,
                };

                return consultarResponse;
            });

            return result;
        }

        public async Task<List<DatosT1144Response>> GetDataT1144ByRuc(string ruc)
        {
            var requestUri = $"{Credentials.Ruc}/DatosT1144?numruc={ruc}";
            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var result = new List<DatosT1144Response>();

                var nodes = xmlDocument.SelectNodes("//multiRef");

                foreach (XmlNode node in nodes)
                {
                    result.Add(new DatosT1144Response
                    {
                        CodCiiu2 = xmlDocument.SelectSingleNode("//cod_ciiu2")?.InnerText,
                        DesCiiu2 = xmlDocument.SelectSingleNode("//des_ciiu2")?.InnerText,
                        CodCiiu3 = xmlDocument.SelectSingleNode("//cod_ciiu3")?.InnerText,
                        DesCiiu3 = xmlDocument.SelectSingleNode("//des_ciiu3")?.InnerText,
                        CodCorreo1 = xmlDocument.SelectSingleNode("//cod_correo1")?.InnerText,
                        CodCorreo2 = xmlDocument.SelectSingleNode("//cod_correo2")?.InnerText,
                        NumTelef1 = xmlDocument.SelectSingleNode("//num_telef1")?.InnerText,
                        CodDepar1 = xmlDocument.SelectSingleNode("//cod_depar1")?.InnerText,
                        DesDepar1 = xmlDocument.SelectSingleNode("//des_depar1")?.InnerText,
                        NumTelef2 = xmlDocument.SelectSingleNode("//num_telef2")?.InnerText,
                        CodDepar2 = xmlDocument.SelectSingleNode("//cod_depar2")?.InnerText,
                        DesDepar2 = xmlDocument.SelectSingleNode("//des_depar2")?.InnerText,
                        NumTelef3 = xmlDocument.SelectSingleNode("//num_telef3")?.InnerText,
                        CodDepar3 = xmlDocument.SelectSingleNode("//cod_depar3")?.InnerText,
                        DesDepar3 = xmlDocument.SelectSingleNode("//des_depar3")?.InnerText,
                        NumTelef4 = xmlDocument.SelectSingleNode("//num_telef4")?.InnerText,
                        CodDepar4 = xmlDocument.SelectSingleNode("//cod_depar4")?.InnerText,
                        DesDepar4 = xmlDocument.SelectSingleNode("//des_depar4")?.InnerText,
                        NumFax = xmlDocument.SelectSingleNode("//num_fax")?.InnerText,
                        CodDepar5 = xmlDocument.SelectSingleNode("//cod_depar5")?.InnerText,
                        DesDepar5 = xmlDocument.SelectSingleNode("//des_depar5")?.InnerText,
                        DesAsiento = xmlDocument.SelectSingleNode("//des_asiento")?.InnerText,
                        DesParreg = xmlDocument.SelectSingleNode("//des_parreg")?.InnerText,
                        DesRefnot = xmlDocument.SelectSingleNode("//des_refnot")?.InnerText,
                        IndConleg = xmlDocument.SelectSingleNode("//ind_conleg")?.InnerText,
                        DesConleg = xmlDocument.SelectSingleNode("//des_conleg")?.InnerText,
                        IndCorreo1 = xmlDocument.SelectSingleNode("//ind_correo1")?.InnerText,
                        FecConfir1 = xmlDocument.SelectSingleNode("//fec_confir1")?.InnerText,
                        IndCorreo2 = xmlDocument.SelectSingleNode("//ind_correo2")?.InnerText,
                        FecConfir2 = xmlDocument.SelectSingleNode("//fec_confir2")?.InnerText,
                        IndProind = xmlDocument.SelectSingleNode("//ind_proind")?.InnerText,
                        DesProind = xmlDocument.SelectSingleNode("//des_proind")?.InnerText,
                        NumKilom = xmlDocument.SelectSingleNode("//num_kilom")?.InnerText,
                        NumManza = xmlDocument.SelectSingleNode("//num_manza")?.InnerText,
                        NumDepar = xmlDocument.SelectSingleNode("//num_depar")?.InnerText,
                        NumLote = xmlDocument.SelectSingleNode("//num_lote")?.InnerText,
                        NumRuc = xmlDocument.SelectSingleNode("//num_ruc")?.InnerText,
                    });

                }
                return result;
            });

            return result;
        }

        public async Task<List<DatosT362Response>> GetDataT362ByRuc(string ruc)
        {
            var requestUri = $"{Credentials.Ruc}/DatosT362?numruc={ruc}";
            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var result = new List<DatosT362Response>();

                var nodes = xmlDocument.SelectNodes("//multiRef");

                foreach (XmlNode node in nodes)
                {
                    result.Add(new DatosT362Response
                    {
                        DescNumreg = xmlDocument.SelectSingleNode("//desc_numreg")?.InnerText,
                        T362Fecact = xmlDocument.SelectSingleNode("//t362_fecact")?.InnerText,
                        T362Fecbaj = xmlDocument.SelectSingleNode("//t362_fecbaj")?.InnerText,
                        T362Indice = xmlDocument.SelectSingleNode("//t362_indice")?.InnerText,
                        T362Nombre = xmlDocument.SelectSingleNode("//t362_nombre")?.InnerText,
                        T362Numreg = xmlDocument.SelectSingleNode("//t362_numreg")?.InnerText,
                        T362Numruc = xmlDocument.SelectSingleNode("//t362_numruc")?.InnerText,
                    });
                }

                return result;
            });

            return result;
        }

        public async Task<DomicilioLegalResponse> GetLegalAddressByRuc(string ruc)
        {
            var requestUri = $"{Credentials.Ruc}/DomicilioLegal?numruc={ruc}";
            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var consultarResponse = new DomicilioLegalResponse
                {
                    GetDomicilioLegalReturn = xmlDocument.SelectSingleNode("//getDomicilioLegalReturn")?.InnerText,
                };

                return consultarResponse;
            });

            return result;
        }

        public async Task<List<EstablecimientosAnexosResponse>> GetAttachedEstablishmentsByRuc(string ruc)
        {
            var requestUri = $"{Credentials.Ruc}/EstablecimientosAnexos?numruc={ruc}";
            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var result = new List<EstablecimientosAnexosResponse>();

                var nodes = xmlDocument.SelectNodes("//multiRef");

                foreach (XmlNode node in nodes)
                {
                    result.Add(new EstablecimientosAnexosResponse
                    {
                        SprUbigeo = xmlDocument.SelectSingleNode("//spr_ubigeo")?.InnerText,
                        CodDep = xmlDocument.SelectSingleNode("//cod_dep")?.InnerText,
                        DescDep = xmlDocument.SelectSingleNode("//desc_dep")?.InnerText,
                        CodProv = xmlDocument.SelectSingleNode("//cod_prov")?.InnerText,
                        DescProv = xmlDocument.SelectSingleNode("//desc_prov")?.InnerText,
                        CodDist = xmlDocument.SelectSingleNode("//cod_dist")?.InnerText,
                        DescDist = xmlDocument.SelectSingleNode("//desc_dist")?.InnerText,
                        SprNumruc = xmlDocument.SelectSingleNode("//spr_numruc")?.InnerText,
                        SprCorrel = xmlDocument.SelectSingleNode("//spr_correl")?.InnerText,
                        SprNomvia = xmlDocument.SelectSingleNode("//spr_nomvia")?.InnerText,
                        SprNumer1 = xmlDocument.SelectSingleNode("//spr_numer1")?.InnerText,
                        SprInter1 = xmlDocument.SelectSingleNode("//spr_inter1")?.InnerText,
                        SprNomzon = xmlDocument.SelectSingleNode("//spr_nomzon")?.InnerText,
                        SprRefer1 = xmlDocument.SelectSingleNode("//spr_refer1")?.InnerText,
                        SprNombre = xmlDocument.SelectSingleNode("//spr_nombre")?.InnerText,
                        SprTipest = xmlDocument.SelectSingleNode("//spr_tipest")?.InnerText,
                        DescTipest = xmlDocument.SelectSingleNode("//desc_tipest")?.InnerText,
                        SprLicenc = xmlDocument.SelectSingleNode("//spr_licenc")?.InnerText,
                        SprTipvia = xmlDocument.SelectSingleNode("//spr_tipvia")?.InnerText,
                        DescTipvia = xmlDocument.SelectSingleNode("//desc_tipvia")?.InnerText,
                        SprTipzon = xmlDocument.SelectSingleNode("//spr_tipzon")?.InnerText,
                        DescTipzon = xmlDocument.SelectSingleNode("//desc_tipzon")?.InnerText,
                        SprFecact = xmlDocument.SelectSingleNode("//spr_fecact")?.InnerText,
                        Dirección = xmlDocument.SelectSingleNode("//dirección")?.InnerText,
                    });
                }

                return result;
            });

            return result;
        }

        public async Task<EstAnexosT1150Response> GetEstAnexosT1150ByRuc(string ruc)
        {
            var requestUri = $"{Credentials.Ruc}/EstAnexosT1150?numruc={ruc}";
            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var consultarResponse = new EstAnexosT1150Response
                {
                    NumCorrel = xmlDocument.SelectSingleNode("//num_correl")?.InnerText,
                    NumKilom = xmlDocument.SelectSingleNode("//num_kilom")?.InnerText,
                    NumManza = xmlDocument.SelectSingleNode("//num_manza")?.InnerText,
                    NumDepar = xmlDocument.SelectSingleNode("//num_depar")?.InnerText,
                    NumLote = xmlDocument.SelectSingleNode("//num_lote")?.InnerText,
                };

                return consultarResponse;
            });

            return result;
        }

        public async Task<RepLegalesResponse> GetLegalRepresentativesByRuc(string ruc)
        {
            var requestUri = $"{Credentials.Ruc}/RepLegales?numruc={ruc}";
            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var consultarResponse = new RepLegalesResponse
                {
                    CodDepar = xmlDocument.SelectSingleNode("//cod_depar")?.InnerText,
                    NumOrdSuce = xmlDocument.SelectSingleNode("//num_ord_suce")?.InnerText,
                    CodCargo = xmlDocument.SelectSingleNode("//cod_cargo")?.InnerText,
                    RsoCargoo = xmlDocument.SelectSingleNode("//rso_cargoo")?.InnerText,
                    RsoVdesde = xmlDocument.SelectSingleNode("//rso_vdesde")?.InnerText,
                    RsoDocide = xmlDocument.SelectSingleNode("//rso_docide")?.InnerText,
                    DescDocide = xmlDocument.SelectSingleNode("//desc_docide")?.InnerText,
                    RsoNrodoc = xmlDocument.SelectSingleNode("//rso_nrodoc")?.InnerText,
                    RsoFecact = xmlDocument.SelectSingleNode("//rso_fecact")?.InnerText,
                    RsoFecnac = xmlDocument.SelectSingleNode("//rso_fecnac")?.InnerText,
                    RsoNombre = xmlDocument.SelectSingleNode("//rso_nombre")?.InnerText,
                    RsoNumruc = xmlDocument.SelectSingleNode("//rso_numruc")?.InnerText,
                };

                return consultarResponse;
            });

            return result;
        }

        public async Task<List<RazonSocialResponse>> SearchBusinessName(string name)
        {
            var requestUri = $"{Credentials.Ruc}/RazonSocial?RSocial={name}";

            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var result = new List<RazonSocialResponse>();

                var nodes = xmlDocument.SelectNodes("//multiRef");

                foreach (XmlNode node in nodes)
                {
                    result.Add(new RazonSocialResponse
                    {
                        CodDep = xmlDocument.SelectSingleNode("//cod_dep")?.InnerText,
                        CodDist = xmlDocument.SelectSingleNode("//cod_dist")?.InnerText,
                        CodProv = xmlDocument.SelectSingleNode("//cod_prov")?.InnerText,
                        DdpCiiu = xmlDocument.SelectSingleNode("//ddp_ciiu")?.InnerText,
                        DdpDoble = xmlDocument.SelectSingleNode("//ddp_doble")?.InnerText,
                        DdpEstado = xmlDocument.SelectSingleNode("//ddp_estado")?.InnerText,
                        DdpFecAct = xmlDocument.SelectSingleNode("//ddp_fecact")?.InnerText,
                        DdpFecAlt = xmlDocument.SelectSingleNode("//ddp_fecalt")?.InnerText,
                        DdpFecBaj = xmlDocument.SelectSingleNode("//ddp_fecbaj")?.InnerText,
                        DdpFlag22 = xmlDocument.SelectSingleNode("//ddp_flag22")?.InnerText,
                        DdpIdenti = xmlDocument.SelectSingleNode("//ddp_identi")?.InnerText,
                        DdpInter1 = xmlDocument.SelectSingleNode("//ddp_identi")?.InnerText,
                        DdpLlllTttt = xmlDocument.SelectSingleNode("//ddp_lllttt")?.InnerText,
                        DdpMClase = xmlDocument.SelectSingleNode("//ddp_mclase")?.InnerText,
                        DdpNombre = xmlDocument.SelectSingleNode("//ddp_nombre")?.InnerText,
                        DdpNomVia = xmlDocument.SelectSingleNode("//ddp_nomvia")?.InnerText,
                        DdpNomZon = xmlDocument.SelectSingleNode("//ddp_nomzon")?.InnerText,
                        DdpNumer1 = xmlDocument.SelectSingleNode("//ddp_numer1")?.InnerText,
                        DdpNumReg = xmlDocument.SelectSingleNode("//ddp_numreg")?.InnerText,
                        DdpNumRuc = xmlDocument.SelectSingleNode("//ddp_numruc")?.InnerText,
                        DdpReacti = xmlDocument.SelectSingleNode("//ddp_reacti")?.InnerText,
                        DdpRefer1 = xmlDocument.SelectSingleNode("//ddp_refer1")?.InnerText,
                        DdpSecuen = xmlDocument.SelectSingleNode("//ddp_secuen")?.InnerText,
                        DdpTamano = xmlDocument.SelectSingleNode("//ddp_tamano")?.InnerText,
                        DdpTipVia = xmlDocument.SelectSingleNode("//ddp_tipvia")?.InnerText,
                        DdpTipZon = xmlDocument.SelectSingleNode("//ddp_tipzon")?.InnerText,
                        DdpTpoEmp = xmlDocument.SelectSingleNode("//ddp_tpoemp")?.InnerText,
                        DdpUbigeo = xmlDocument.SelectSingleNode("//ddp_ubigeo")?.InnerText,
                        DdpUserna = xmlDocument.SelectSingleNode("//ddp_userna")?.InnerText,
                        DescCiiu = xmlDocument.SelectSingleNode("//desc_ciiu")?.InnerText,
                        DescDep = xmlDocument.SelectSingleNode("//desc_dep")?.InnerText,
                        DescDist = xmlDocument.SelectSingleNode("//desc_dist")?.InnerText,
                        DescEstado = xmlDocument.SelectSingleNode("//desc_estado")?.InnerText,
                        DescFlag22 = xmlDocument.SelectSingleNode("//desc_flag22")?.InnerText,
                        DescIdenti = xmlDocument.SelectSingleNode("//desc_identi")?.InnerText,
                        DescNumReg = xmlDocument.SelectSingleNode("//desc_numreg")?.InnerText,
                        DescProv = xmlDocument.SelectSingleNode("//desc_prov")?.InnerText,
                        DescTamano = xmlDocument.SelectSingleNode("//desc_tamano")?.InnerText,
                        DescTipVia = xmlDocument.SelectSingleNode("//desc_tipvia")?.InnerText,
                        DescTipZon = xmlDocument.SelectSingleNode("//desc_tipzon")?.InnerText,
                        DescTpoEmp = xmlDocument.SelectSingleNode("//desc_tpoemp")?.InnerText,
                        EsActivo = xmlDocument.SelectSingleNode("//esActivo")?.InnerText,
                        EsHabido = xmlDocument.SelectSingleNode("//esHabido")?.InnerText
                    });
                }

                return result;
            });

            return result;
        }

        public async Task<ConsultaResponse> GetGrados(string user, string password, string document)
        {
            var requestUri = $"{REST.Url_Grados}" +
                $"?usuario={user}" +
                $"&clave={password}" +
                $"&idEntidad={Credentials.IdEntidad}" +
                $"&fecha={DateTime.Now:yyyymmdd}" +
                $"&hora={DateTime.Now:hhmmss}" +
                $"&mac_wsServer={Credentials.Server_MAC}" +
                $"&ip_wsServer={Credentials.Server_IP}" +
                $"&ip_wsUser={GetRequestIP()}" +
                $"&nroDocIdentidad={document}";

            var result = await Get(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, (reader) =>
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(reader);

                var gtPersonas = new List<GTPersona>();

                var nodes = xmlDocument.SelectNodes("//gtPersona");

                foreach (XmlNode node in nodes)
                {
                    gtPersonas.Add(new GTPersona
                    {
                        TipoDocumento = xmlDocument.SelectSingleNode("//tipoDocumento")?.InnerText,
                        NroDocumento = xmlDocument.SelectSingleNode("//nroDocumento")?.InnerText,
                        ApellidoPaterno = xmlDocument.SelectSingleNode("//apellidoPaterno")?.InnerText,
                        ApellidoMaterno = xmlDocument.SelectSingleNode("//apellidoMaterno")?.InnerText,
                        Nombres = xmlDocument.SelectSingleNode("//nombres")?.InnerText,
                        AbreviaturaTitulo = xmlDocument.SelectSingleNode("//abreviaturaTitulo")?.InnerText,
                        TituloProfesional = xmlDocument.SelectSingleNode("//tituloProfesional")?.InnerText,
                        Universidad = xmlDocument.SelectSingleNode("//universidad")?.InnerText,
                        Pais = xmlDocument.SelectSingleNode("//pais")?.InnerText,
                        TipoInstitucion = xmlDocument.SelectSingleNode("//tipoInstitucion")?.InnerText,
                        TipoGestion = xmlDocument.SelectSingleNode("//tipoGestion")?.InnerText,
                        FechaEmision = xmlDocument.SelectSingleNode("//fechaEmision")?.InnerText,
                        Resolucion = xmlDocument.SelectSingleNode("//resolucion")?.InnerText,
                        FechaResolucion = xmlDocument.SelectSingleNode("//fechaResolucion")?.InnerText,
                    });
                }

                var consultarResponse = new ConsultaResponse
                {
                    RequestUri = requestUri,
                    Respuesta = new Respuesta
                    {
                        FechaSunedu = xmlDocument.SelectSingleNode("//fechaSunedu")?.InnerText,
                        HoraSunedu = xmlDocument.SelectSingleNode("//horaSunedu")?.InnerText,
                        CGenerico = xmlDocument.SelectSingleNode("//cGenerico")?.InnerText,
                        DGenerica = xmlDocument.SelectSingleNode("//dGenerica")?.InnerText,
                    },
                    GTPersonas = gtPersonas
                };

                return consultarResponse;
            });

            return result;
        }

        private string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
            {
                var csvList = GetHeaderValueAs<string>("X-Forwarded-For");
                ip = SplitCsv(csvList).FirstOrDefault();
            }

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && _accessor.ActionContext.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (string.IsNullOrWhiteSpace(ip))
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        private T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (_accessor.ActionContext.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        private static List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
    }
}
