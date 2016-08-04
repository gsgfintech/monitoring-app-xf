using System.Threading.Tasks;

namespace Capital.GSG.FX.Monitor.App.XF
{
    public interface IAuthenticate
    {
        Task<bool> AuthenticateAsync();

        Task<bool> LogoutAsync();
    }
}
