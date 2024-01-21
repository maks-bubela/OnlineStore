using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.Models;

namespace OnlineStore.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            #region To Model
            CreateMap<OrderDTO, OrderCreateModel>();
            #endregion

            #region To DTO
            CreateMap<OrderCreateModel, OrderDTO>();
            #endregion
        }
    }
}
