using Nop.Core;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Services
{
    public interface IPurchaseOrderService
    {
        Task<IPagedList<PurchaseOrderList>> GetAllPurchaseOrdersAsync(string supplierName = "", DateTime? startDate = null, DateTime? endDate = null, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<PurchaseOrderProductMapping>> GetAllPurchasedProductAsync(string productName = null, int purchaseOrderId = 0, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<ProductSupplierMapping>> GetAllPopupAsync(string productName = null, int supplierId = 0, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
