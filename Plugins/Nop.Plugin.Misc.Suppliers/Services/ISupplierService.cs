using Nop.Core;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Services;
public interface ISupplierService
{
    Task<IPagedList<Supplier>> GetAllSuppliersAsync(string name = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
    Task InsertSupplierAsync(Supplier supplier);
    Task UpdateSupplierAsync(Supplier supplier);
    Task<Supplier> GetSupplierByIdAsync(int supplierId);
    Task DeleteSupplierAsync(Supplier supplier);
    Task InsertOrUpdateProductSupplierMappingAsync(int productId, int supplierId);

}
