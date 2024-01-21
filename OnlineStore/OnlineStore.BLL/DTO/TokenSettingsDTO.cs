using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace OnlineStore.BLL.DTO
{
    public class TokenSettingsDTO
    {
        public ClaimsIdentity Identity { get; set; }
        public int LifeTime { get; set; }
    }
}
