using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyVehicle
    {
        public Guid Id { get; set; }
        public DateTime ChargeDate { get; set; }

        //Ruc de la universidad , 11 digitos
        [StringLength(11)]
        public string Ruc { get; set; }
        //Año de la Orden o Servicio
        public int Year { get; set; }
        //Mes de la Orden o Servicio , 2 digitos
        [StringLength(2)]
        public string Month { get; set; }
        //Clase de vehiculo , 1 Oficial , 2 Seguridad , 3 Pool , 4 Operativo
        public int VehicleClass { get; set; }
        //Tipo de Servicio de vehiculo
        public string Type { get; set; }
        //Nombre del Chofer Asignado
        public string DriverName { get; set; }
        //Vehiculo Asignado a
        public string Area { get; set; }
        //Actividad Asignada al Vehiculo
        public string Activity { get; set; }
        //Tipo de Combustible
        public string FuelType { get; set; }
        //Cantidad Recorrdia KM
        public double DistanceTraveled { get; set; }
        //Costo del Combustible
        public decimal FuelCost { get; set; }
        //Fecha de Vencimiento del Soat
        public DateTime SoatEndDate { get; set; }
        //Placa del vehiculo
        public string VehicleRegistrationNumber { get; set; }
        //Observaciones
        public string Observations { get; set; }
    }
}
