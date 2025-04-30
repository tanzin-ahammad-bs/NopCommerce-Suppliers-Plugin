using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Services
{
    public class SelectedProductService
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
