using Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Services;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.PurchasedProduct;
using Nop.Services.Catalog;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Factories
{
    public class PurchaseOrderModelFactory : IPurchaseOrderModelFactory
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IProductService _productService;

        public PurchaseOrderModelFactory(IPurchaseOrderService purchaseOrderService, IProductService productService)
        {
            _purchaseOrderService = purchaseOrderService;
            _productService = productService;
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





        // Purchased Product
        public async Task<PurchasedProductListModel> PreparePurchasedProductListModelAsync(PurchasedProductSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(searchModel);

            var purchaseOrders = await _purchaseOrderService.GetAllPurchasedProductAsync(
                productId: searchModel.ProductId,
                purchaseOrderId: searchModel.PurchaseOrderId,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize
            );

            var model = await new PurchasedProductListModel().PrepareToGridAsync<PurchasedProductListModel, PurchasedProductMappingModel, PurchaseOrderProductMapping>(
                searchModel, purchaseOrders, () => GetPurchasedProductMappingModelsAsync(purchaseOrders));

            return model;
        }

        private async IAsyncEnumerable<PurchasedProductMappingModel> GetPurchasedProductMappingModelsAsync(IEnumerable<PurchaseOrderProductMapping> purchaseOrders)
        {
            foreach (var po in purchaseOrders)
            {
                var product = await _productService.GetProductByIdAsync(po.ProductId);
                if (product != null)
                {
                    yield return new PurchasedProductMappingModel
                    {
                        Id = po.Id,
                        PurchaseOrderId = po.PurchaseOrderId,
                        ProductId = po.ProductId,
                        QuantityToOrder = po.QuantityToOrder,
                        UnitCost = po.UnitCost,
                        ProductName = product.Name,
                        Sku = product.Sku,
                        CurrentStock = product.StockQuantity
                    };
                }
            }
        }





        public Task<PurchasedProductSearchModel> PreparePurchasedProductSearchModelAsync(PurchasedProductSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(searchModel);
            searchModel.SetGridPageSize();
            return Task.FromResult(searchModel);
        }







    }
}
