using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.RucResponse
{
    public class EstablecimientosAnexosResponse
    {
        public string SprUbigeo    { get; set; } //Código de ubigeo
        public string CodDep       { get; set; } //Código de departamento
        public string DescDep      { get; set; } //Descripción departamento
        public string CodProv      { get; set; } //Código de distrito
        public string DescProv     { get; set; } //Descripción provincia
        public string CodDist      { get; set; } //Código de Provincia
        public string DescDist     { get; set; } //descripción distrito
        public string SprNumruc    { get; set; } //Listado de establecimientos anexos
        public string SprCorrel    { get; set; } //Código del establecimiento
        public string SprNomvia    { get; set; } //Nombre de la vía
        public string SprNumer1    { get; set; } //Numero/Kilometro/Manzana
        public string SprInter1    { get; set; } //Interior/Dpto/lote
        public string SprNomzon    { get; set; } //Nombre de la zona
        public string SprRefer1    { get; set; } //Referencia de la ubicación
        public string SprNombre    { get; set; } //Nombre del establecimiento
        public string SprTipest    { get; set; } //Código de tipo de establecimiento
        public string DescTipest   { get; set; } //Descripción de tipo de establecimiento
        public string SprLicenc    { get; set; } //Numero de licencia municipal
        public string SprTipvia    { get; set; } //Tipo de vía
        public string DescTipvia   { get; set; } //descripción de tipo de vía
        public string SprTipzon    { get; set; } //Tipo de zona
        public string DescTipzon   { get; set; } //descripción de tipo de zona
        public string SprFecact    { get; set; } //Fecha y hora de actualización
        public string Dirección    { get; set; } //Dirección

    }
}
