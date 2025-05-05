using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.PurchaseOrderList
{
    public partial record class PurchaseOrderModel : BaseNopEntityModel
    {
        public int PurchaseOrderId { get; set; }
        public string SupplierName { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CreatedBy { get; set; }
    }
}
