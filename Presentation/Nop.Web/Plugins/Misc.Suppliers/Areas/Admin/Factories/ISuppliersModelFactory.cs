using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;
public interface ISuppliersModelFactory
{
    Task<SuppliersSearchModel> PrepareSupplierSearchModelAsync(SuppliersSearchModel searchModel);
    Task<SuppliersListModel> PrepareSuppliersListModelAsync(SuppliersSearchModel searchModel);
    Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Supplier supplier, bool excludeProperties = false);

}
