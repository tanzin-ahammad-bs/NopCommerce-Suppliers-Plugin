using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.Suppliers.Models;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.Suppliers.Components;
public class ProductSuppliersWidgetViewComponent : NopViewComponent
{
    private readonly ISupplierService _supplierService;

    public ProductSuppliersWidgetViewComponent(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }




    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var productModel = additionalData as ProductModel;

        if (productModel == null || productModel.Id == 0)
            return Content(string.Empty);

        var suppliers = await _supplierService.GetAllSuppliersAsync();
        var selectList = suppliers.Select(s => new SelectListItem
        {
            Text = s.Name,
            Value = s.Id.ToString()
        }).ToList();

        var model = new SupplierProductModel
        {
            ProductId = productModel.Id,
            SupplierId = 0,
            Suppliers = selectList
        };

        return View("~/Plugins/Misc.Suppliers/Views/Suppliers/ProductSupplierEdit.cshtml", model);
    }






}
