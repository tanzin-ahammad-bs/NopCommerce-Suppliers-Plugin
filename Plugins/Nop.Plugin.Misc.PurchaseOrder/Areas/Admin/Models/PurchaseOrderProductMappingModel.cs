namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public class PurchaseOrderProductMappingModel
    {
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public int CurrentStock { get; set; }
        public int QuantityToOrder { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal => QuantityToOrder * UnitCost;
    }
}
