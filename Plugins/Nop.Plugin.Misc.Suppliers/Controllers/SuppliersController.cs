using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.Suppliers.Factories;
using Nop.Plugin.Misc.Suppliers.Models;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.Suppliers.Controllers;
public class SuppliersController : BasePluginController
{
    #region Fields

    protected readonly ISuppliersModelFactory _suppliersModelFactory;
    protected readonly IPermissionService _permissionService;


    #endregion

    #region Ctor

    public SuppliersController(ISuppliersModelFactory suppliersModelFactory, IPermissionService permissionService)
    {
        _suppliersModelFactory = suppliersModelFactory;
        _permissionService = permissionService;
    }

    #endregion

    #region Methods

    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _suppliersModelFactory.PrepareSupplierSearchModelAsync(new SuppliersSearchModel());

        return View("~/Plugins/Misc.Suppliers/Views/Suppliers/List.cshtml", model);
    }

    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [HttpPost]
    public virtual async Task<IActionResult> List(SuppliersSearchModel searchModel)
    {
        //prepare model
        var model = await _suppliersModelFactory.PrepareSuppliersListModelAsync(searchModel);

        return Json(model);
    }


    #endregion
}
