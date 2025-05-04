using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.PurchasedProduct
{
    public record class PurchasedProductMappingModel : BaseNopEntityModel
    {
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public int CurrentStock { get; set; }
        public int QuantityToOrder { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal => QuantityToOrder * UnitCost;
    }
}
