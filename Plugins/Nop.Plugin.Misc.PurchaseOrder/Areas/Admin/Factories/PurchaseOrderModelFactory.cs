using Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Services;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Services;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Factories
{
    public class PurchaseOrderModelFactory : IPurchaseOrderModelFactory
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderModelFactory(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        public async Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(searchModel);

            // Fetch data
            var purchaseOrders = await _purchaseOrderService.GetAllPurchaseOrdersAsync(
                supplierName: searchModel.SupplierName,
                startDate: searchModel.StartDate,
                endDate: searchModel.EndDate,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize
            );

            // Convert to model and prepare grid
            var model = await new PurchaseOrderListModel().PrepareToGridAsync<PurchaseOrderListModel, PurchaseOrderModel, PurchaseOrderList>(
                searchModel, purchaseOrders, () =>
                {
                    return purchaseOrders.Select(po => new PurchaseOrderModel
                    {
                        Id = po.Id,
                        PurchaseOrderId = po.PurchaseOrderId,
                        SupplierName = po.SupplierName,
                        CreationDate = po.CreationDate,
                        TotalAmount = po.TotalAmount,
                        CreatedBy = po.CreatedBy
                    }).ToAsyncEnumerable();
                });


            return model;
        }

        public Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(searchModel);
            searchModel.SetGridPageSize();
            return Task.FromResult(searchModel);
        }
    }
}
