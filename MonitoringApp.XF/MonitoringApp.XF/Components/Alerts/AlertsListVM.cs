using Capital.GSG.FX.Monitoring.AppDataTypes;
using Capital.GSG.FX.Utils.Portable;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;

namespace MonitoringApp.XF.Components.Alerts
{
    public class AlertsListVM : BaseViewModel
    {
        public ObservableCollection<AlertSlim> OpenAlerts { get; set; } = new ObservableCollection<AlertSlim>();

        public Command RefreshCommand { get; private set; }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (value != isRefreshing)
                {
                    isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        public AlertsListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshOpenAlerts(true);

            IsRefreshing = false;
        }

        public async Task RefreshOpenAlerts(bool refresh)
        {
            try
            {
                await LoadOpenAlerts(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadOpenAlerts(refresh);
                }
            }
        }

        private async Task LoadOpenAlerts(bool refresh)
        {
            var alerts = await AlertManager.Instance.LoadOpenAlerts(refresh);

            OpenAlerts.Clear();

            if (!alerts.IsNullOrEmpty())
            {
                foreach (var alert in alerts)
                    OpenAlerts.Add(alert);
            }
        }

        public async Task<AlertFull> GetAlertById(string id)
        {
            try
            {
                return await LoadAlertById(id);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadAlertById(id);
                }

                return null;
            }
        }

        private static async Task<AlertFull> LoadAlertById(string id)
        {
            return await AlertManager.Instance.GetAlertById(id);
        }
    }

    public class LevelToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string level = value as string;

            if (string.IsNullOrEmpty(level))
                return Color.White;

            switch (level)
            {
                case "DEBUG":
                case "INFO":
                    return Color.Aqua;
                case "WARNING":
                    return Color.Yellow;
                default:
                    return Color.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
