using OnlineStore.BLL.DTO;


namespace OnlineStore.Interfaces
{
    public interface IAuthOptions
    {
        string GetSymmetricSecurityKey(TokenSettingsDTO settingsDto);
    }
}
