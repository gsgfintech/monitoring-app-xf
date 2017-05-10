using Capital.GSG.FX.Data.Core.AccountPortfolio;
using Capital.GSG.FX.Data.Core.ContractData;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionDetailsVM : BaseViewModel
    {
        private PositionViewModel position;
        public PositionViewModel Position
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

        public async Task GetPositionByCross(Broker broker, string account, Cross cross)
        {
            await LoadPositionByCross(broker, account, cross);
        }

        private async Task LoadPositionByCross(Broker broker, string account, Cross cross)
        {
            Position = (await PositionManager.Instance.GetPositionByCross(broker, account, cross)).ToPositionViewModel();
        }
    }
}
