using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.Models;

namespace OnlineStore.MappingProfiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            #region To Model
            CreateMap<ProductsDTO, ProductsInfoModel>();
            #endregion

            #region To DTO
            CreateMap<ProductsInfoModel, ProductsDTO>();
            #endregion
        }
    }
}
