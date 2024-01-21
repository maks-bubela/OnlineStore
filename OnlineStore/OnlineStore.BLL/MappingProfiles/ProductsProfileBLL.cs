using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.DAL.Entities;

namespace OnlineStore.BLL.MappingProfiles
{
    public class ProductsProfileBLL : Profile
    {
        public ProductsProfileBLL()
        {
            #region To DTO
            CreateMap<Products, ProductsDTO>();
            #endregion

            #region from DTO
            CreateMap<ProductsDTO, Products>();
            #endregion
        }
    }
}
