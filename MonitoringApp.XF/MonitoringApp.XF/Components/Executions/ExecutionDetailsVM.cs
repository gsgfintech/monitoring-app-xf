using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Executions
{
    public class ExecutionDetailsVM : BaseViewModel
    {
        private ExecutionViewModel trade;
        public ExecutionViewModel Trade
        {
            get { return trade; }
            set
            {
                if (trade != value)
                {
                    trade = value;
                    OnPropertyChanged(nameof(Trade));
                }
            }
        }

        public async Task GetExecutionById(string id)
        {
            try
            {
                await LoadExecutionById(id);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadExecutionById(id);
                }
            }
        }

        private async Task LoadExecutionById(string id)
        {
            Trade = (await ExecutionManager.Instance.GetExecutionById(id)).ToExecutionViewModel();
        }
    }
}
