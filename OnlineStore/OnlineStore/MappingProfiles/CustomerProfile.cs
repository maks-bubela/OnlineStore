using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.Models;

namespace OnlineStore.MappingProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            #region To Model
            CreateMap<CustomerDTO, CustomerLoginModel>();
            CreateMap<CustomerDTO, CustomerRegistrationModel>();
            #endregion

            #region To DTO
            CreateMap<CustomerLoginModel, CustomerDTO>();
            CreateMap<CustomerRegistrationModel, CustomerDTO>();
            #endregion
        }
    }
}
