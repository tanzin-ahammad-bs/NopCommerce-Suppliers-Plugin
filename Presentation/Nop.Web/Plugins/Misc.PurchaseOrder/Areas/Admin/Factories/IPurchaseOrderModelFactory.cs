using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.PurchasedProduct;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories
{
    public interface IPurchaseOrderModelFactory
    {
        Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel);
        Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel);


        Task<PurchasedProductSearchModel> PreparePurchasedProductSearchModelAsync(PurchasedProductSearchModel searchModel);
        Task<PurchasedProductListModel> PreparePurchasedProductListModelAsync(PurchasedProductSearchModel searchModel);


    }
}
