using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.RucResponse
{
    public class DatosSecundariosResponse
    {
        public string DdsCalifi    { get; set; } //Calificación de la conducta del contribuyente
        public string DdsComext    { get; set; } //Marca de actividad comercio exterior
        public string DescComext   { get; set; } //Descripción de comercio exterior
        public string DdsConsti    { get; set; } //Fecha de constitución
        public string DdsContab    { get; set; } //Tipo de contabilidad
        public string DescContab   { get; set; } //Descripción de tipo de contabilidad
        public string DdsDocide    { get; set; } //Tipo de documento de identidad
        public string DescDocide   { get; set; } //descripción de tipo de documento
        public string DdsNrodoc    { get; set; } //Numero de documento de identidad
        public string DdsDomici    { get; set; } //Condición de domiciliado
        public string DescDomici   { get; set; } //descripción de condición de domiciliado
        public string DdsFecact    { get; set; } //Tipo de facturación
        public string DescFactur   { get; set; } //Descripción de tipo de facturación
        public string DdsFecnac    { get; set; } //Fecha de nacimiento
        public string DdsAsient    { get; set; } //Numero de asiento inscripción RRPP
        public string DdsFicha     { get; set; } //Tomo o ficha de RRPP
        public string DdsNfolio    { get; set; } //Numero de folios en RRPP
        public string DdsInicio    { get; set; } //Fecha de inicio de actividades
        public string DdsLicenc    { get; set; } //Número de licencia municipal
        public string DdsNacion    { get; set; } //Nacionalidad
        public string DdsNomcom    { get; set; } //Nombre comercial
        public string DdsNumruc    { get; set; } //Número de RUC
        public string DdsOrient    { get; set; } //Origen de la entidad
        public string DescOrient   { get; set; } //Descripción del origen de la entidad
        public string DdsPaispa    { get; set; } //País que emitió el pasaporte
        public string DdsPasapo    { get; set; } //Número de pasaporte
        public string DdsPatron    { get; set; } //Carnet patronal
        public string DdsSexo      { get; set; } //Sexo
        public string DescSexo     { get; set; } //descripción del Sexo
        public string DdsTelef1    { get; set; } //Número de teléfono
        public string DdsTelef2    { get; set; } //Número de teléfono
        public string DdsTelef3    { get; set; } //Número de teléfono
        public string DdsNumfax    { get; set; } //Numero de FAX

    }
}
