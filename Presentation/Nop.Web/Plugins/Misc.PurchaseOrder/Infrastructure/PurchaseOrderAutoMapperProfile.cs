using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.AddProductPopup;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.PurchaseOrderList;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Infrastructure
{
    public class PurchaseOrderAutoMapperProfile : Profile, IOrderedMapperProfile
    {
        public int Order => 0;

        public PurchaseOrderAutoMapperProfile()
        {
            // Map from entity to model and vice versa
           
            CreateMap<PurchaseOrderList, PurchaseOrderModel>();
            CreateMap<PurchaseOrderModel, PurchaseOrderList>();

            CreateMap<PurchaseOrderProductMapping, PurchaseOrderProductMappingModel>();
            CreateMap<PurchaseOrderProductMappingModel, PurchaseOrderProductMapping>();

            CreateMap<SupplierProductMapping, SelectedProductModel>();
            CreateMap<SelectedProductModel, SupplierProductMapping>();

            CreateMap<ProductSupplierMapping, AddProductPopupModel>();
            CreateMap<AddProductPopupModel, ProductSupplierMapping>();

            CreateMap<PurchaseOrderProductMapping, AddProductPopupModel>();
            CreateMap<AddProductPopupModel, PurchaseOrderProductMapping>();   

        }
    }
}
