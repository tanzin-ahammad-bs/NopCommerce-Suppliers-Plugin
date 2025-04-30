using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models
{
    public record class SelectedProductModel : BaseNopEntityModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }

        // Editable fields (Quantity to Order, Unit Cost, and Line Total)
        public int QuantityToOrder { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal { get; set; }

        // Supplier details
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        // Optional: Can add a field to track if the product has been edited (if needed)
        public bool IsEdited { get; set; }

        public bool Published { get; set; }

        public int CurrentStock { get; set; }


    }
}
