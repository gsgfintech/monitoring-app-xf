using Capital.GSG.FX.Data.Core.AccountPortfolio;
using System;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Positions
{
    public class AccountDetailsVM : BaseViewModel
    {
        public event Action<int> AttributesCountChanged;

        private AccountViewModel account;
        public AccountViewModel Account
        {
            get { return account; }
            set
            {
                if (account != value)
                {
                    account = value;
                    OnPropertyChanged(nameof(Account));
                }
            }
        }

        public async Task GetAccount(Broker broker, string accountName)
        {
            try
            {
                await LoadAccount(broker, accountName);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadAccount(broker, accountName);
                }
            }
        }

        private async Task LoadAccount(Broker broker, string accountName)
        {
            Account = (await PositionManager.Instance.GetAccount(broker, accountName)).ToAccountViewModel();

            AttributesCountChanged?.Invoke(Account?.Attributes?.Count ?? 0);
        }
    }
}
