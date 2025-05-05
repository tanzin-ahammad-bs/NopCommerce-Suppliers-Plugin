using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.AddProductPopup
{
    public record class AddProductPopupSearchModel : BaseSearchModel
    {
        public AddProductPopupSearchModel()
        {
            if (PageSize < 1)
                PageSize = 5;
        }
        public new int PageSize { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }

    }
}