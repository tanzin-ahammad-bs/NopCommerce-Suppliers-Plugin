using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.AddProductPopup
{
    public record class AddProductPopupModel : BaseNopEntityModel
    {

     


        public int ProductId { get; set; }  // REQUIRED
        public string ProductName { get; set; } // REQUIRED
        public bool Published { get; set; } // REQUIRED

        public int SupplierId { get; set; }


        public int PageSize { get; set; }

    }
}
