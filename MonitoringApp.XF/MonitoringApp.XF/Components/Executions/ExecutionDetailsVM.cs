using Capital.GSG.FX.Data.Core.AccountPortfolio;
using MonitoringApp.XF.Managers;
using MonitoringApp.XF.ViewModels;
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

        public async Task GetExecutionById(Broker broker, string id)
        {
            await LoadExecutionById(broker, id);
        }

        private async Task LoadExecutionById(Broker broker, string id)
        {
            Trade = (await ExecutionManager.Instance.GetExecutionById(broker, id)).ToExecutionViewModel();
        }
    }
}
