using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.Suppliers.Domain;
namespace Nop.Plugin.Misc.Suppliers.Services;
public interface ISupplierService
{
    Task<IPagedList<Supplier>> GetAllSuppliersAsync(string name = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
    Task InsertSupplierAsync(Supplier supplier);

    Task UpdateSupplierAsync(Supplier supplier);

    Task<Supplier> GetSupplierByIdAsync(int supplierId);


}
