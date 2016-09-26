using Capital.GSG.FX.Monitoring.AppDataTypes;
using Capital.GSG.FX.Utils.Portable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;
using System.ComponentModel;

namespace MonitoringApp.XF.Components.Alerts
{
    public class AlertsListVM : INotifyPropertyChanged
    {
        public ObservableCollection<AlertSlim> OpenAlerts { get; set; } = new ObservableCollection<AlertSlim>();

        public event PropertyChangedEventHandler PropertyChanged;

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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
