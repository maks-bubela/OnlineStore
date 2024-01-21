using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.Interfaces
{
    public interface IPasswordProcessing
    {
        string GenerateSalt();
        string GetHashCode(string pass, string salt);
    }
}
