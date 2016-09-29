using Capital.GSG.FX.Monitoring.AppDataTypes;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Alerts
{
    public class AlertDetailsVM : BaseViewModel
    {
        private AlertFull alert;
        public AlertFull Alert
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
            try
            {
                await LoadAlertById(id);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadAlertById(id);
                }
            }
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
                IsCloseButtonVisible = Alert.Status == "OPEN";
                IsClosedTimestampVisible = !IsCloseButtonVisible;
            }
        }

        private async void ExecuteCloseAlertCommand()
        {
            isClosing = true;

            await CloseAlertAuthenticated(Alert.Id);

            isClosing = false;
        }

        private async Task CloseAlertAuthenticated(string id)
        {
            try
            {
                await CloseAlert(id);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await CloseAlert(id);
                }
            }
        }

        private async Task CloseAlert(string id)
        {
            if (await AlertManager.Instance.CloseAlert(id))
                await GetAlertById(id);
        }
    }

    public class LevelToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString();

            if (string.IsNullOrEmpty(status))
                return Color.Default;

            switch (status)
            {
                case "DEBUG":
                case "INFO":
                    return Color.Green;
                case "WARNING":
                    return CustomColors.Orange;
                case "FATAL":
                case "ERROR":
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
            string status = value?.ToString();

            if (string.IsNullOrEmpty(status))
                return Color.Default;

            switch (status)
            {
                case "CLOSED":
                    return Color.Green;
                case "OPEN":
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
}
