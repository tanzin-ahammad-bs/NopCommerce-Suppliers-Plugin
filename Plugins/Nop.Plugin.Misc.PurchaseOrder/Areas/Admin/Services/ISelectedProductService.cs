using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services
{
    public interface ISelectedProductService
    {
        void AddProduct(SelectedProductModel product);
        List<SelectedProductModel> GetSelectedProducts();
        void ClearProducts();

    }
}
