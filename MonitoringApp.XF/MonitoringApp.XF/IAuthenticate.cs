using System.Threading.Tasks;

namespace MonitoringApp.XF
{
    public interface IAuthenticate
    {
        Task<bool> AuthenticateAsync();
    }
}
