using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Plugin.Misc.Suppliers.Models;

namespace Nop.Plugin.Misc.Suppliers.Infrastructure
{
    public class SuppliersAutoMapperProfile : Profile, IOrderedMapperProfile
    {
        public int Order => 0;

        public SuppliersAutoMapperProfile()
        {
            // Map from entity to model and vice versa
            CreateMap<Supplier, SupplierModel>();
            CreateMap<SupplierModel, Supplier>();
        }
    }
}
