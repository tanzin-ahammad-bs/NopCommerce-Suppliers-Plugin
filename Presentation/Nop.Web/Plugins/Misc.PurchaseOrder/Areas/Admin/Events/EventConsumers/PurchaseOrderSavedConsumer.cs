using Nop.Services.Catalog;
using Nop.Services.Events;

namespace Nop.Plugin.Misc.PurchaseOrder.Events
{
    public class PurchaseOrderSavedConsumer : IConsumer<PurchaseOrderSavedEvent>
    {
        private readonly IProductService _productService;

        public PurchaseOrderSavedConsumer(IProductService productService)
        {
            _productService = productService;
        }

        public async Task HandleEventAsync(PurchaseOrderSavedEvent eventMessage)
        {
            foreach (var item in eventMessage.Products)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity += item.QuantityToOrder;
                    await _productService.UpdateProductAsync(product);
                }
            }
        }
    }

}
