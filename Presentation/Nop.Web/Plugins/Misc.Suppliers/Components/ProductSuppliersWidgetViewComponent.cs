using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.Suppliers.Services;
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
        var suppliers = await _supplierService.GetAllSuppliersAsync();

        var supplierList = suppliers.Select(s => new SelectListItem
        {
            Text = s.Name,
            Value = s.Id.ToString()
        }).ToList();

        // Extract ProductId from additionalData (typically a ProductModel)
        int productId = 0;
        if (additionalData != null && additionalData.GetType().GetProperty("Id") != null)
        {
            productId = (int)(additionalData.GetType().GetProperty("Id")?.GetValue(additionalData) ?? 0);
        }

        ViewBag.ProductId = productId;

        return View("~/Plugins/Misc.Suppliers/Views/Suppliers/_ProductSupplierDropdown.cshtml", supplierList);
    }




}
