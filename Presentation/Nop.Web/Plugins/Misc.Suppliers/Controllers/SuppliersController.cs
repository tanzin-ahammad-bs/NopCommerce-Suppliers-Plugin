using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Plugin.Misc.Suppliers.Factories;
using Nop.Plugin.Misc.Suppliers.Models;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Services.Attributes;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Vendors;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.Suppliers.Controllers;
public class SuppliersController : BasePluginController
{
    #region Fields

    protected readonly ForumSettings _forumSettings;
    protected readonly IAddressService _addressService;
    protected readonly IAttributeParser<AddressAttribute, AddressAttributeValue> _addressAttributeParser;
    protected readonly IAttributeParser<VendorAttribute, VendorAttributeValue> _vendorAttributeParser;
    protected readonly IAttributeService<VendorAttribute, VendorAttributeValue> _vendorAttributeService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly IPictureService _pictureService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly ISuppliersModelFactory _suppliersModelFactory;
    protected readonly ISupplierService _supplierService;
    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public SuppliersController(ForumSettings forumSettings,
        IAddressService addressService,
        IAttributeParser<AddressAttribute, AddressAttributeValue> addressAttributeParser,
        IAttributeParser<VendorAttribute, VendorAttributeValue> vendorAttributeParser,
        IAttributeService<VendorAttribute, VendorAttributeValue> vendorAttributeService,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IGenericAttributeService genericAttributeService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IPermissionService permissionService,
        IPictureService pictureService,
        IUrlRecordService urlRecordService,
        ISuppliersModelFactory suppliersModelFactory,
        ISupplierService supplierService)
    {
        _forumSettings = forumSettings;
        _addressService = addressService;
        _addressAttributeParser = addressAttributeParser;
        _vendorAttributeParser = vendorAttributeParser;
        _vendorAttributeService = vendorAttributeService;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _genericAttributeService = genericAttributeService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _pictureService = pictureService;
        _urlRecordService = urlRecordService;
        _suppliersModelFactory = suppliersModelFactory;
        _supplierService = supplierService;
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









    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _suppliersModelFactory.PrepareSupplierModelAsync(new SupplierModel(), null);

        return View("~/Plugins/Misc.Suppliers/Views/Suppliers/Create.cshtml", model);
    }







    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [HttpPost]
    public virtual async Task<IActionResult> Create(SupplierModel model, bool continueEditing, IFormCollection form)
    {
        //parse vendor attributes
        //var vendorAttributesXml = await ParseVendorAttributesAsync(form);
        //var warnings = (await _vendorAttributeParser.GetAttributeWarningsAsync(vendorAttributesXml)).ToList();
        //foreach (var warning in warnings)
        //{
        //    ModelState.AddModelError(string.Empty, warning);
        //}

        if (ModelState.IsValid)
        {
            var supplier = model.ToEntity<Supplier>();
            supplier.Deleted = false;
            supplier.DisplayOrder = 1;
            supplier.PageSize = 5;

            await _supplierService.InsertSupplierAsync(supplier);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewVendor",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewVendor"), supplier.Id), supplier);

            //search engine name
            model.SeName = await _urlRecordService.ValidateSeNameAsync(supplier, model.SeName, supplier.Name, true);
            await _urlRecordService.SaveSlugAsync(supplier, model.SeName, 0);

            //address
            //var address = model.Address.ToEntity<Address>();
            //address.CreatedOnUtc = DateTime.UtcNow;

            //some validation
            //if (address.CountryId == 0)
            //    address.CountryId = null;
            //if (address.StateProvinceId == 0)
            //    address.StateProvinceId = null;
            //await _addressService.InsertAddressAsync(address);
            //vendor.AddressId = address.Id;
            await _supplierService.UpdateSupplierAsync(supplier);

            //vendor attributes
            //await _genericAttributeService.SaveAttributeAsync(vendor, NopVendorDefaults.VendorAttributes, vendorAttributesXml);

            //locales
            //await UpdateLocalesAsync(supplier, model);

            //update picture seo file name
            /*await UpdatePictureSeoNamesAsync(supplier);*/

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Vendors.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = supplier.Id });
        }

        //prepare model
        model = await _suppliersModelFactory.PrepareSupplierModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }






    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a vendor with the specified id
        var supplier = await _supplierService.GetSupplierByIdAsync(id);
        if (supplier == null)
            return RedirectToAction("List");
        else if ((bool)supplier.Deleted)
            return RedirectToAction("List");

        //prepare model
        var model = await _suppliersModelFactory.PrepareSupplierModelAsync(null, supplier);

        //if (!_forumSettings.AllowPrivateMessages && model.PmCustomerId > 0)
        //    _notificationService.WarningNotification("Private messages are disabled. Do not forget to enable them.");

        return View("~/Plugins/Misc.Suppliers/Views/Suppliers/Edit.cshtml", model);
    }



    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [HttpPost]
    public virtual async Task<IActionResult> Edit(SupplierModel model, bool continueEditing, IFormCollection form)
    {
        //try to get a vendor with the specified id
        var supplier = await _supplierService.GetSupplierByIdAsync(model.Id);
        if (supplier == null)
            return RedirectToAction("List");
        else if ((bool)supplier.Deleted)
            return RedirectToAction("List");

        //parse vendor attributes
        //var vendorAttributesXml = await ParseVendorAttributesAsync(form);
        //var warnings = (await _vendorAttributeParser.GetAttributeWarningsAsync(vendorAttributesXml)).ToList();
        //foreach (var warning in warnings)
        //{
        //    ModelState.AddModelError(string.Empty, warning);
        //}

        //custom address attributes
        //var customAttributes = await _addressAttributeParser.ParseCustomAttributesAsync(form, NopCommonDefaults.AddressAttributeControlName);
        //var customAttributeWarnings = await _addressAttributeParser.GetAttributeWarningsAsync(customAttributes);
        //foreach (var error in customAttributeWarnings)
        //{
        //    ModelState.AddModelError(string.Empty, error);
        //}

        if (ModelState.IsValid)
        {
            //var prevPictureId = supplier.PictureId;
            supplier = model.ToEntity(supplier);
            await _supplierService.UpdateSupplierAsync(supplier);

            //vendor attributes
            //await _genericAttributeService.SaveAttributeAsync(supplier, NopVendorDefaults.VendorAttributes, vendorAttributesXml);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditVendor",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditVendor"), supplier.Id), supplier);

            //search engine name
            model.SeName = await _urlRecordService.ValidateSeNameAsync(supplier, model.SeName, supplier.Name, true);
            await _urlRecordService.SaveSlugAsync(supplier, model.SeName, 0);

            //address
            //var address = await _addressService.GetAddressByIdAsync(supplier.AddressId);
            //if (address == null)
            //{
            //    address = model.Address.ToEntity<Address>();
            //    address.CustomAttributes = customAttributes;
            //    address.CreatedOnUtc = DateTime.UtcNow;

            //    //some validation
            //    if (address.CountryId == 0)
            //        address.CountryId = null;
            //    if (address.StateProvinceId == 0)
            //        address.StateProvinceId = null;

            //    await _addressService.InsertAddressAsync(address);
            //    vendor.AddressId = address.Id;
            //    await _vendorService.UpdateVendorAsync(supplier);
            //}
            //else
            //{
            //    address = model.Address.ToEntity(address);
            //    address.CustomAttributes = customAttributes;

            //    //some validation
            //    if (address.CountryId == 0)
            //        address.CountryId = null;
            //    if (address.StateProvinceId == 0)
            //        address.StateProvinceId = null;

            //    await _addressService.UpdateAddressAsync(address);
            //}

            //locales
            //await UpdateLocalesAsync(supplier, model);

            //delete an old picture (if deleted or updated)
            //if (prevPictureId > 0 && prevPictureId != vendor.PictureId)
            //{
            //    var prevPicture = await _pictureService.GetPictureByIdAsync(prevPictureId);
            //    if (prevPicture != null)
            //        await _pictureService.DeletePictureAsync(prevPicture);
            //}
            //update picture seo file name
            //await UpdatePictureSeoNamesAsync(supplier);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Vendors.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = supplier.Id });
        }

        //prepare model
        model = await _suppliersModelFactory.PrepareSupplierModelAsync(model, supplier, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }







    #endregion
}
