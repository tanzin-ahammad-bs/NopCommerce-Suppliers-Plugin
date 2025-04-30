using Nop.Core;
using Nop.Core.Caching;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;
using Nop.Services.Caching;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Services;
public class SupplierService : ISupplierService
{
    protected readonly IRepository<Supplier> _supplierRepository;
    protected readonly IRepository<ProductSupplierMapping> _productsuppliermappingRepository;
    protected readonly IStaticCacheManager _staticCacheManager;

    public SupplierService(IRepository<Supplier> supplierRepository, IRepository<ProductSupplierMapping> productsuppliermappingRepository, IStaticCacheManager staticCacheManager)
    {
        _supplierRepository = supplierRepository;
        _productsuppliermappingRepository = productsuppliermappingRepository;
        _staticCacheManager = staticCacheManager;
    }
    public virtual async Task<IPagedList<Supplier>> GetAllSuppliersAsync(string name = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
    {

        var cacheKey = new CacheKey($"Nop.plugin.misc.suppliers.all-{name}-{email}");

        var suppliers = await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            return await _supplierRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(v => v.Name.Contains(name));

                if (!string.IsNullOrWhiteSpace(email))
                    query = query.Where(v => v.Email.Contains(email));

                if (!showHidden)
                    query = query.Where(v => (bool)v.Active);

                query = query.Where(v => (bool)!v.Deleted);
                query = query.OrderBy(v => v.DisplayOrder).ThenBy(v => v.Name).ThenBy(v => v.Email);

                return query;
            }, pageIndex, pageSize);
        });

        return suppliers;

    }

    public virtual async Task InsertSupplierAsync(Supplier supplier)
    {
        await _supplierRepository.InsertAsync(supplier);
        await _staticCacheManager.RemoveByPrefixAsync("Nop.plugin.misc.suppliers.all");
    }

    public virtual async Task UpdateSupplierAsync(Supplier supplier)
    {
        await _supplierRepository.UpdateAsync(supplier);
        await _staticCacheManager.RemoveByPrefixAsync("Nop.plugin.misc.suppliers.all");
        var cacheKey = new CacheKey($"Nop.plugin.misc.suppliers.id-{supplier.Id}");
        await _staticCacheManager.RemoveAsync(cacheKey);
    }

    public virtual async Task<Supplier> GetSupplierByIdAsync(int supplierId)
    {
        var cacheKey = new CacheKey($"Nop.plugin.misc.suppliers.id-{supplierId}");
        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            return await _supplierRepository.GetByIdAsync(supplierId);
        });
    }

    public virtual async Task DeleteSupplierAsync(Supplier supplier)
    {
        await _supplierRepository.DeleteAsync(supplier);
        await _staticCacheManager.RemoveByPrefixAsync("Nop.plugin.misc.suppliers.all");
        var cacheKey = new CacheKey($"Nop.plugin.misc.suppliers.id-{supplier.Id}");
        await _staticCacheManager.RemoveAsync(cacheKey);
    }

    //Insert Or Update into ProductSupplierMapping
    public async Task InsertOrUpdateProductSupplierMappingAsync(int productId, int supplierId)
    {
        var existing = await _productsuppliermappingRepository.Table
            .FirstOrDefaultAsync(x => x.ProductId == productId);

        if (existing != null)
        {
            existing.SupplierId = supplierId;
            await _productsuppliermappingRepository.UpdateAsync(existing);
        }
        else
        {
            var newMapping = new ProductSupplierMapping
            {
                ProductId = productId,
                SupplierId = supplierId
            };
            await _productsuppliermappingRepository.InsertAsync(newMapping);
        }
    }

}
