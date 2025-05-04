using Nop.Core.Domain.Catalog;
using Nop.Core;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain
{
    public class PurchaseOrderProductMapping : BaseEntity
    {
     

        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }

        // Add these:
        public int QuantityToOrder { get; set; }
        public decimal UnitCost { get; set; }


    }
}