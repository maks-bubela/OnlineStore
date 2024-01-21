using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.DAL.Entities;


namespace OnlineStore.BLL.MappingProfiles
{
    public class OrderProfileBLL : Profile
    {
        public OrderProfileBLL()
        {
            #region To DTO
            CreateMap<Order, OrderDTO>();
            #endregion

            #region from DTO
            CreateMap<OrderDTO, Order>();
            #endregion
        }
    }
}
