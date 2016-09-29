using Capital.GSG.FX.Monitoring.AppDataTypes;
using Capital.GSG.FX.Utils.Portable;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;
using System.Collections.Generic;

namespace MonitoringApp.XF.Components.Alerts
{
    public class AlertsListVM : BaseViewModel
    {
        public ObservableCollection<GroupedAlertList> Alerts { get; private set; } = new ObservableCollection<GroupedAlertList>();

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
            await RefreshAlerts(true);

            IsRefreshing = false;
        }

        public async Task RefreshAlerts(bool refresh)
        {
            try
            {
                await LoadAlerts(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadAlerts(refresh);
                }
            }
        }

        private async Task LoadAlerts(bool refresh)
        {
            var openAlerts = await AlertManager.Instance.LoadOpenAlerts(refresh);
            var alertsClosedToday = await AlertManager.Instance.LoadAlertsClosedToday(refresh);

            Alerts.Clear();

            if (!openAlerts.IsNullOrEmpty())
                Alerts.Add(new GroupedAlertList($"Open Alerts ({openAlerts.Count})", "OPEN", true, openAlerts));
            else
                Alerts.Add(new GroupedAlertList("Open Alerts (0)", "OPEN", false));

            if (!alertsClosedToday.IsNullOrEmpty())
                Alerts.Add(new GroupedAlertList($"Alerts Closed Today ({alertsClosedToday.Count})", "CLOSED", false, alertsClosedToday));
            else
                Alerts.Add(new GroupedAlertList("Alerts Closed Today (0)", "CLOSED", false));
        }

        public async Task CloseAllAlertsAuthenticated()
        {
            try
            {
                await CloseAllAlerts();
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await CloseAllAlerts();
                }
            }
        }

        private async Task CloseAllAlerts()
        {
            if (await AlertManager.Instance.CloseAllAlerts())
                await RefreshAlerts(false);
        }
    }

    public class GroupedAlertList : ObservableCollection<AlertSlim>
    {
        public string LongDescription { get; private set; }
        public string ShortDescription { get; private set; }
        public bool ShowCloseButton { get; set; }

        public GroupedAlertList(string longDescription, string shortDescription, bool showCloseButton)
        {
            LongDescription = longDescription;
            ShortDescription = shortDescription;
            ShowCloseButton = showCloseButton;
        }

        public GroupedAlertList(string longDescription, string shortDescription, bool showCloseButton, IEnumerable<AlertSlim> alerts)
            : base(alerts)
        {
            LongDescription = longDescription;
            ShortDescription = shortDescription;
            ShowCloseButton = showCloseButton;
        }
    }

    public class LevelToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string level = value as string;

            if (string.IsNullOrEmpty(level))
                return Color.Transparent;

            switch (level)
            {
                case "DEBUG":
                case "INFO":
                    return CustomColors.LightSkyBlue;
                case "WARNING":
                    return CustomColors.LightSalmon;
                default:
                    return CustomColors.LightPink;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
