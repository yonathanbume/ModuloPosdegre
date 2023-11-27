namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Templates
{
    public class PurchaseOrderDetailReportDataTemplate
    {
        public string supplyOrderGeneratedId { get;  set; }
        public string supplyName { get;  set; }
        /// <summary>
        /// cantidad total de insumos en la Orden
        /// </summary>
        public decimal quantity { get;  set; }
        public string ummc { get;  set; }
        public string status { get;  set; }
        /// <summary>
        /// precio total de la orden para ese producto
        /// </summary>
        public string totalprice { get;  set; }
        /// <summary>
        /// precio total de la solicitud para ese producto
        /// </summary>
        public string total{ get; set; }
        /// <summary>
        /// precio unitario de la orden
        /// </summary>
        public string unitprice { get;  set; }
        /// <summary>
        /// cantidad solicitada (solicitudes sumas)
        /// </summary>
        public decimal quantity_solicited { get;  set; }
        public int providerSupplyGeneratedId { get;  set; }
    }
    public class ReferralGuideTemplate
    {
        public string supplyOrderGeneratedId { get; set; }
        public string supplyName { get; set; }

        public string ummc { get; set; }
        public string status { get; set; }
        /// <summary>
        /// precio total de la orden para ese producto
        /// </summary>
        public string totalprice { get; set; }
        /// <summary>
        /// precio unitario de la orden
        /// </summary>
        public string unitprice { get; set; }
        /// <summary>
        /// cantidad solicitada (solicitudes sumas)
        /// </summary>
        public decimal quantity_solicited { get; set; }
        public int providerSupplyGeneratedId { get; set; }
        public string package { get; set; }
    }
}
