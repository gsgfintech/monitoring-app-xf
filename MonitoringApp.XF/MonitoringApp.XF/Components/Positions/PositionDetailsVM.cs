using Capital.GSG.FX.Monitoring.AppDataTypes;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionDetailsVM : BaseViewModel
    {
        private PositionSecurityFull position;
        public PositionSecurityFull Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        }

        public async Task GetPositionByCross(string cross)
        {
            try
            {
                await LoadPositionByCross(cross);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadPositionByCross(cross);
                }
            }
        }

        private async Task LoadPositionByCross(string cross)
        {
            Position = await PositionManager.Instance.GetPositionByCross(cross);
        }
    }
}
