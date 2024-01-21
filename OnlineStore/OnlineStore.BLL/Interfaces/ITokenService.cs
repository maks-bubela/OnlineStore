using OnlineStore.BLL.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.Interfaces
{
    public interface ITokenService
    {
        Task<int> GetTokenSettingsAsync(EnvirementTypes type);
    }
}
