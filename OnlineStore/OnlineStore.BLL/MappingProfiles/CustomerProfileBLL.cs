using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.DAL.Entities;

namespace OnlineStore.BLL.MappingProfiles
{
    public class CustomerProfileBLL : Profile
    {
        public CustomerProfileBLL()
        {
            #region To DTO
            CreateMap<Customer, CustomerDTO>();
            #endregion

            #region from DTO
            CreateMap<CustomerDTO, Customer>();
            #endregion
        }
    }
}
