using Capital.GSG.FX.Data.Core.SystemData;
using Capital.GSG.FX.Data.Core.WebApi;
using MonitoringApp.XF.Managers;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Alerts
{
    public class AlertDetailsVM : BaseViewModel
    {
        private Alert alert;
        public Alert Alert
        {
            get { return alert; }
            set
            {
                if (alert != value)
                {
                    alert = value;
                    OnPropertyChanged(nameof(Alert));
                }
            }
        }

        private bool isCloseButtonVisible;
        public bool IsCloseButtonVisible
        {
            get { return isCloseButtonVisible; }
            set
            {
                isCloseButtonVisible = value;
                OnPropertyChanged(nameof(IsCloseButtonVisible));
            }
        }

        private bool isClosedTimestampVisible;
        public bool IsClosedTimestampVisible
        {
            get { return isClosedTimestampVisible; }
            set
            {
                isClosedTimestampVisible = value;
                OnPropertyChanged(nameof(IsClosedTimestampVisible));
            }
        }

        public Command CloseAlertCommand { get; private set; }
        private bool isClosing;

        public AlertDetailsVM()
        {
            CloseAlertCommand = new Command(ExecuteCloseAlertCommand, () => !isClosing);
        }

        public async Task GetAlertById(string id)
        {
            await LoadAlertById(id);
        }

        private async Task LoadAlertById(string id)
        {
            Alert = await AlertManager.Instance.GetAlertById(id);

            if (Alert == null)
            {
                IsCloseButtonVisible = false;
                IsClosedTimestampVisible = false;
            }
            else
            {
                IsCloseButtonVisible = Alert.Status == AlertStatus.OPEN;
                IsClosedTimestampVisible = !IsCloseButtonVisible;
            }
        }

        private async void ExecuteCloseAlertCommand()
        {
            isClosing = true;

            await CloseAlert(Alert.AlertId);

            isClosing = false;
        }

        private async Task CloseAlert(string id)
        {
            var result = await AlertManager.Instance.CloseAlert(id);

            if (result.Success)
            {
                await GetAlertById(id);
                await Utils.ShowToastNotification("Success", $"Successfully closed alert {id}");
            }
            else
                await Utils.ShowToastNotification("Failed", $"Failed to close alert {id}: {result.Message}");
        }
    }

    public class LevelToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AlertLevel? level = value as AlertLevel?;

            if (!level.HasValue)
                return Color.Default;

            switch (level.Value)
            {
                case AlertLevel.DEBUG:
                case AlertLevel.INFO:
                    return Color.Green;
                case AlertLevel.WARNING:
                    return CustomColors.Orange;
                case AlertLevel.ERROR:
                case AlertLevel.FATAL:
                    return Color.Red;
                default:
                    return Color.Default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AlertStatus? status = value as AlertStatus?;

            if (!status.HasValue)
                return Color.Default;

            switch (status.Value)
            {
                case AlertStatus.OPEN:
                    return Color.Red;
                case AlertStatus.CLOSED:
                    return Color.Green;
                default:
                    return Color.Default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
