using Nop.Core;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain
{
    public class SupplierProductMapping : BaseEntity
    {
        public int SupplierId { get; set; }

        public int ProductId { get; set; }

        public int QuantityToOrder { get; set; }

        public decimal UnitCost { get; set; }

        public decimal LineTotal { get; set; }
    }
}
