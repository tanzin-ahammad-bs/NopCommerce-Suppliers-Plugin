using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services
{
    public class SelectedProductService : ISelectedProductService
    {
        private readonly List<SelectedProductModel> _selectedProducts = new List<SelectedProductModel>();

        public void AddProduct(SelectedProductModel product)
        {
            _selectedProducts.Add(product);
        }

        public List<SelectedProductModel> GetSelectedProducts()
        {
            return _selectedProducts;
        }

        public void ClearProducts()
        {
            _selectedProducts.Clear();
        }
    }
}
