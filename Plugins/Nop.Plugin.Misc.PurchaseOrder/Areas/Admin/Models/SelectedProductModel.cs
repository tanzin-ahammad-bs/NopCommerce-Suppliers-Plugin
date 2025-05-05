using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public record class SelectedProductModel : BaseNopEntityModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int QuantityToOrder { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public bool IsEdited { get; set; }
        public bool Published { get; set; }
        public int CurrentStock { get; set; }

    }
}
