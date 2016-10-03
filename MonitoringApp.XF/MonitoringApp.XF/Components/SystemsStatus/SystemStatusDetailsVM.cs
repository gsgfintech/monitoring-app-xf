using Capital.GSG.FX.Monitoring.AppDataTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.SystemsStatus
{
    public class SystemStatusDetailsVM : BaseViewModel
    {
        public event Action<int> AttributesCountChanged;

        private SystemStatusFull systemStatus;
        public SystemStatusFull SystemStatus
        {
            get { return systemStatus; }
            set
            {
                if (systemStatus != value)
                {
                    systemStatus = value;
                    OnPropertyChanged(nameof(SystemStatus));
                }
            }
        }

        public async Task GetSystemStatusByName(string name)
        {
            try
            {
                await LoadSystemStatusByName(name);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadSystemStatusByName(name);
                }
            }
        }

        private async Task LoadSystemStatusByName(string name)
        {
            SystemStatus = await SystemStatusManager.Instance.GetSystemStatusByName(name);

            AttributesCountChanged?.Invoke(SystemStatus?.Attributes?.Count ?? 0);
        }
    }

    public class IsAliveToStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as bool?) == true)
                return "Running";
            else
                return "Stopped";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsAliveToStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as bool?) == true)
                return Color.Green;
            else
                return Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OverallStatusToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString();

            if (status == "GREEN")
                return Color.Green;
            else if (status == "YELLOW")
                return CustomColors.LightSalmon;
            else if (status == "RED")
                return Color.Red;
            else
                return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LevelToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString();

            if (status == "GREEN")
                return CustomColors.LightGreen;
            else if (status == "YELLOW")
                return CustomColors.LightSalmon;
            else if (status == "RED")
                return CustomColors.LightPink;
            else
                return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AttributesCountToCellHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? count = value as int?;

            if (!count.HasValue)
                return 20.0;

            return Math.Min(20.0, count.Value * 20.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
