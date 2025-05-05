using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.PurchasedProduct
{
    public partial record class PurchasedProductSearchModel : BaseSearchModel
    {
        public string ProductName { get; set; }
        public int PurchaseOrderId { get; set; }

    }
}
