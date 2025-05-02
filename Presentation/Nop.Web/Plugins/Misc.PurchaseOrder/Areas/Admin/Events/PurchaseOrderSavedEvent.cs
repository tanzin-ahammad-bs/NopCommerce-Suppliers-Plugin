using System.Collections.Generic;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Events
{
    public class PurchaseOrderSavedEvent
    {
        public IList<SelectedProductModel> Products { get; }

        public PurchaseOrderSavedEvent(IList<SelectedProductModel> products)
        {
            Products = products;
        }
    }
}
