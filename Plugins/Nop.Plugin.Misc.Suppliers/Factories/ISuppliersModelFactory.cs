using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Suppliers.Models;
using Nop.Web.Areas.Admin.Models.Vendors;

namespace Nop.Plugin.Misc.Suppliers.Factories;
public interface ISuppliersModelFactory
{
    Task<SuppliersSearchModel> PrepareSupplierSearchModelAsync(SuppliersSearchModel searchModel);
    Task<SuppliersListModel> PrepareSuppliersListModelAsync(SuppliersSearchModel searchModel);
    

}
