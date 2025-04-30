using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories
{
    public interface IPurchaseOrderModelFactory
    {
        Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel);
        Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel);
    }
}
