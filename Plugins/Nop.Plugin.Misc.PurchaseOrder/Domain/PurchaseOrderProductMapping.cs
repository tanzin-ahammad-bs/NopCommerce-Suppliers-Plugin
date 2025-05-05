using Nop.Core;

namespace Nop.Plugin.Misc.PurchaseOrder.Domain
{
    public class PurchaseOrderProductMapping : BaseEntity
    {
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public int QuantityToOrder { get; set; }
        public decimal UnitCost { get; set; }
    }
}