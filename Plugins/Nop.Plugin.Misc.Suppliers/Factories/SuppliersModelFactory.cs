using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Suppliers.Models;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.Suppliers.Factories;
public class SuppliersModelFactory : ISuppliersModelFactory
{
    protected readonly ISupplierService _supplierService;
    protected readonly IUrlRecordService _urlRecordService;

    public SuppliersModelFactory(ISupplierService supplierService,
        IUrlRecordService urlRecordService)
    {
        _supplierService = supplierService;
        _urlRecordService = urlRecordService;
    }
    public async Task<SuppliersListModel> PrepareSuppliersListModelAsync(SuppliersSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get suppliers
        var suppliers = await _supplierService.GetAllSuppliersAsync(showHidden: true,
            name: searchModel.SearchName,
            email: searchModel.SearchEmail,
            pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

        //prepare list model
        var model = await new SuppliersListModel().PrepareToGridAsync(searchModel, suppliers, () =>
        {
            //fill in model values from the entity
            return suppliers.SelectAwait(async supplier =>
            {
                var supplierModel = supplier.ToModel<SupplierModel>();

                supplierModel.SeName = await _urlRecordService.GetSeNameAsync(supplier, 0, true, false);

                return supplierModel;
            });
        });

        return model;
    }

    public Task<SuppliersSearchModel> PrepareSupplierSearchModelAsync(SuppliersSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }


}
