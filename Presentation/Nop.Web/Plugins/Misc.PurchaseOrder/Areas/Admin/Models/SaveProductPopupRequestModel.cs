using System.Collections.Generic;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models
{
    public class SaveProductPopupRequestModel
    {
        public int SupplierId { get; set; }
        public List<int> SelectedProductIds { get; set; }
    }
}
