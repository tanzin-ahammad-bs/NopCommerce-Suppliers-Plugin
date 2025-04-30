using Nop.Core;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain
{
    public class PurchaseOrderList : BaseEntity
    {
        public int PurchaseOrderId { get; set; }
        public string SupplierName { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CreatedBy { get; set; }
    }
}
