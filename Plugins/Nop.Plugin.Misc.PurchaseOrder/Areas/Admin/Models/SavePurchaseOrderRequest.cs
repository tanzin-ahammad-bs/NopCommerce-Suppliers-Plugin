namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public class SavePurchaseOrderRequest
    {
        public string SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ProductQuantityDto> Products { get; set; }
    }

    public class ProductQuantityDto
    {
        public int ProductId { get; set; }
        public int QuantityToOrder { get; set; }
        public decimal UnitCost { get; set; } // ✅ Added field
    }
}
