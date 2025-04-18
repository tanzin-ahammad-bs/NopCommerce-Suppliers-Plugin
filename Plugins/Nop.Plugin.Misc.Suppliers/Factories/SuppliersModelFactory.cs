using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Plugin.Misc.Suppliers.Models;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Services.Attributes;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Services.Vendors;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;
using SupplierLocalizedModel = Nop.Plugin.Misc.Suppliers.Models.SupplierLocalizedModel;

namespace Nop.Plugin.Misc.Suppliers.Factories;
public class SuppliersModelFactory : ISuppliersModelFactory
{
    #region Fields

    protected readonly CurrencySettings _currencySettings;
    protected readonly ICurrencyService _currencyService;
    protected readonly IAddressModelFactory _addressModelFactory;
    protected readonly IAddressService _addressService;
    protected readonly IAttributeParser<VendorAttribute, VendorAttributeValue> _vendorAttributeParser;
    protected readonly IAttributeService<VendorAttribute, VendorAttributeValue> _vendorAttributeService;
    protected readonly ICustomerService _customerService;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly ISupplierService _supplierService;
    protected readonly VendorSettings _vendorSettings;

    #endregion

    public SuppliersModelFactory(CurrencySettings currencySettings,
        ICurrencyService currencyService,
        IAddressModelFactory addressModelFactory,
        IAddressService addressService,
        IAttributeParser<VendorAttribute, VendorAttributeValue> vendorAttributeParser,
        IAttributeService<VendorAttribute, VendorAttributeValue> vendorAttributeService,
        ICustomerService customerService,
        IDateTimeHelper dateTimeHelper,
        IGenericAttributeService genericAttributeService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IUrlRecordService urlRecordService,
        ISupplierService supplierService,
        VendorSettings vendorSettings)
    {
        _currencySettings = currencySettings;
        _currencyService = currencyService;
        _addressModelFactory = addressModelFactory;
        _addressService = addressService;
        _vendorAttributeParser = vendorAttributeParser;
        _vendorAttributeService = vendorAttributeService;
        _customerService = customerService;
        _dateTimeHelper = dateTimeHelper;
        _genericAttributeService = genericAttributeService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _urlRecordService = urlRecordService;
        _supplierService = supplierService;
        _vendorSettings = vendorSettings;
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








    public virtual async Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Supplier supplier, bool excludeProperties = false)
    {
        Func<SupplierLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (supplier != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = supplier.ToModel<SupplierModel>();
                model.SeName = await _urlRecordService.GetSeNameAsync(supplier, 0, true, false);
            }

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(supplier, entity => entity.Name, languageId, false, false);
                locale.Description = await _localizationService.GetLocalizedAsync(supplier, entity => entity.Description, languageId, false, false);
                //locale.MetaKeywords = await _localizationService.GetLocalizedAsync(supplier, entity => entity.MetaKeywords, languageId, false, false);
                //locale.MetaDescription = await _localizationService.GetLocalizedAsync(supplier, entity => entity.MetaDescription, languageId, false, false);
                //locale.MetaTitle = await _localizationService.GetLocalizedAsync(supplier, entity => entity.MetaTitle, languageId, false, false);
                locale.SeName = await _urlRecordService.GetSeNameAsync(supplier, languageId, false, false);
            };

            //prepare associated customers
            //await PrepareAssociatedCustomerModelsAsync(model.AssociatedCustomers, supplier);

            //if (supplier.PmCustomerId > 0)
            //{
            //    var pmCustomer = await _customerService.GetCustomerByIdAsync(supplier.PmCustomerId.Value);
            //    model.PmCustomerInfo = pmCustomer.Email;
            //}

            //prepare nested search models
            //PrepareVendorNoteSearchModel(model.VendorNoteSearchModel, supplier);
        }

        //set default values for the new model
        if (supplier == null)
        {
            model.PageSize = 6;
            model.Active = true;
            //model.AllowCustomersToSelectPageSize = true;
            //model.PageSizeOptions = _supplierSettings.DefaultVendorPageSizeOptions;
            //model.PriceRangeFiltering = true;
            //model.ManuallyPriceRange = true;
            //model.PriceFrom = NopCatalogDefaults.DefaultPriceRangeFrom;
            //model.PriceTo = NopCatalogDefaults.DefaultPriceRangeTo;
        }

        //model.PrimaryStoreCurrencyCode = (await _currencyService.GetCurrencyByIdAsync(_currencySettings.PrimaryStoreCurrencyId)).CurrencyCode;

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);

        ////prepare model vendor attributes
        ////await PrepareVendorAttributeModelsAsync(model.VendorAttributes, supplier);

        ////prepare address model
        //var address = await _addressService.GetAddressByIdAsync(supplier?.AddressId ?? 0);
        //if (!excludeProperties && address != null)
        //    model.Address = address.ToModel(model.Address);
        //await _addressModelFactory.PrepareAddressModelAsync(model.Address, address);

        return model;
    }

    
}
