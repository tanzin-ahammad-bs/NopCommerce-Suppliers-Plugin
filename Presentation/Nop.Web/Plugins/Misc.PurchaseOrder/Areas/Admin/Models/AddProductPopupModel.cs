namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public class AddProductPopupModel
    {
        public int ProductId { get; set; }  // REQUIRED
        public string ProductName { get; set; } // REQUIRED
        public bool Published { get; set; } // REQUIRED
    }
}
