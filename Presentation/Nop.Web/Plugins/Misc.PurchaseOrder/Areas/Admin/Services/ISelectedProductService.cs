using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services
{
    public interface ISelectedProductService
    {
        void AddProduct(SelectedProductModel product);
        List<SelectedProductModel> GetSelectedProducts();
        void ClearProducts();

    }
}
