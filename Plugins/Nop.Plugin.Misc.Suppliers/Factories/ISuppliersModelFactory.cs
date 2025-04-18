﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Plugin.Misc.Suppliers.Models;
using Nop.Web.Areas.Admin.Models.Vendors;

namespace Nop.Plugin.Misc.Suppliers.Factories;
public interface ISuppliersModelFactory
{
    Task<SuppliersSearchModel> PrepareSupplierSearchModelAsync(SuppliersSearchModel searchModel);
    Task<SuppliersListModel> PrepareSuppliersListModelAsync(SuppliersSearchModel searchModel);

    Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Supplier supplier, bool excludeProperties = false);


}
