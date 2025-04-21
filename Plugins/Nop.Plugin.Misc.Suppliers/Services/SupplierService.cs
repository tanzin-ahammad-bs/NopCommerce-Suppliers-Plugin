using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Vendors;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Services;
public class SupplierService : ISupplierService
{
    protected readonly IRepository<Supplier> _supplierRepository;

    public SupplierService(IRepository<Supplier> supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }
    public virtual async Task<IPagedList<Supplier>> GetAllSuppliersAsync(string name = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
    {
        var suppliers = await _supplierRepository.GetAllPagedAsync(query =>
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

        return suppliers;
    }




    public virtual async Task InsertSupplierAsync(Supplier supplier)
    {
        await _supplierRepository.InsertAsync(supplier);
    }

    public virtual async Task UpdateSupplierAsync(Supplier supplier)
    {
        await _supplierRepository.UpdateAsync(supplier);
    }



    public virtual async Task<Supplier> GetSupplierByIdAsync(int supplierId)
    {
        return await _supplierRepository.GetByIdAsync(supplierId, cache => default);
    }



    public virtual async Task DeleteSupplierAsync(Supplier supplier)
    {
        await _supplierRepository.DeleteAsync(supplier);
    }



}
