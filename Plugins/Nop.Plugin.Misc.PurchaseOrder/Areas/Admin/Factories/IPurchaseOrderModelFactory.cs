using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.AddProductPopup;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.PurchasedProduct;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.PurchaseOrderList;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories
{
    public interface IPurchaseOrderModelFactory
    {
        Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel);
        Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel);


        Task<PurchasedProductSearchModel> PreparePurchasedProductSearchModelAsync(PurchasedProductSearchModel searchModel);
        Task<PurchasedProductListModel> PreparePurchasedProductListModelAsync(PurchasedProductSearchModel searchModel);


        Task<AddProductPopupSearchModel> PreparePopupSearchModelAsync(AddProductPopupSearchModel searchModel);
        Task<AddProductPopupListModel> PreparePopupListModelAsync(AddProductPopupSearchModel searchModel);

    }
}
