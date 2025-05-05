using System;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Services
{
    public interface IPurchaseOrderService
    {
        Task<IPagedList<PurchaseOrderList>> GetAllPurchaseOrdersAsync(string supplierName = "", DateTime? startDate = null, DateTime? endDate = null, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<PurchaseOrderProductMapping>> GetAllPurchasedProductAsync(int productId = 0, int purchaseOrderId = 0, int pageIndex = 0, int pageSize = int.MaxValue);
        //void CreatePurchaseOrder(string supplierName, decimal totalAmount);
        Task<IPagedList<ProductSupplierMapping>> GetAllPopupAsync(int productId = 0, int supplierId = 0, int pageIndex = 0, int pageSize = int.MaxValue);






    }
}
