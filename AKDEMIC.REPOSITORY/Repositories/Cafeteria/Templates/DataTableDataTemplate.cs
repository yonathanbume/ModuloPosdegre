using System;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Templates
{
    public class SupplyOrderDetailReportDataTemplate
    {
        public int providerSupplyGeneratedId { get; set; }
        public string purchaseId { get; set; }
        public string provider { get; set; }
        public string groupname { get; set; }
        public string supplyName { get; set; }
        public string ummc { get; set; }
        public string totalprice { get; set; }
        public string supplyOrderGeneratedId { get; set; }
        public decimal quantity_solicited { get; set; }
        /// <summary>
        /// meses m1 :enero
        /// m12: diciembre
        /// </summary>
        public decimal m1 { get; set; }
        public decimal m2 { get; set; }
        public decimal m3 { get; set; }
        public decimal m4 { get; set; }
        public decimal m5 { get; set; }
        public decimal m6 { get; set; }
        public decimal m7 { get; set; }
        public decimal m8 { get; set; }
        public decimal m9 { get; set; }
        public decimal m10 { get; set; }
        public decimal m11 { get; set; }
        public decimal m12 { get; set; }
        public decimal total { get; set; }
        public decimal balance { get; set; }
        public decimal progress { get; set; }
        public Guid PurchaseOrderDetailId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid ProviderSupplyId { get; set; }
    }
}
