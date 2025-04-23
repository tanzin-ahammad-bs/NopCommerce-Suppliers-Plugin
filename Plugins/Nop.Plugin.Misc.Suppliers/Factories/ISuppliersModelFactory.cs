using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Plugin.Misc.Suppliers.Models;


namespace Nop.Plugin.Misc.Suppliers.Factories;
public interface ISuppliersModelFactory
{
    Task<SuppliersSearchModel> PrepareSupplierSearchModelAsync(SuppliersSearchModel searchModel);
    Task<SuppliersListModel> PrepareSuppliersListModelAsync(SuppliersSearchModel searchModel);
    Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Supplier supplier, bool excludeProperties = false);

}
