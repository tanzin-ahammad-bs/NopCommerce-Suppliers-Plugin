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


        //public int StartIndex { get; set; }
        public new int PageSize { get; set; }

        //public new int Page
        //{
        //    get
        //    {
        //        if (PageSize == 0) return 1; // prevent divide by zero
        //        return (StartIndex / PageSize) + 1;
        //    }
        //}

        public string ProductName { get; set; }
        public int SupplierId { get; set; }

    }
}