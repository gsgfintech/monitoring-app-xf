using Capital.GSG.FX.Data.Core.SystemData;
using MonitoringApp.XF.Managers;
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

        private SystemStatus systemStatus;
        public SystemStatus SystemStatus
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
            await LoadSystemStatusByName(name);
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
            SystemStatusLevel? status = value as SystemStatusLevel?;

            if (!status.HasValue)
                return Color.Default;

            switch (status.Value)
            {
                case SystemStatusLevel.GREEN:
                    return Color.Green;
                case SystemStatusLevel.YELLOW:
                    return CustomColors.LightSalmon;
                case SystemStatusLevel.RED:
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

    public class LevelToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SystemStatusLevel? status = value as SystemStatusLevel?;

            if (!status.HasValue)
                return Color.Transparent;

            switch (status.Value)
            {
                case SystemStatusLevel.GREEN:
                    return CustomColors.LightGreen;
                case SystemStatusLevel.YELLOW:
                    return CustomColors.LightSalmon;
                case SystemStatusLevel.RED:
                    return CustomColors.LightPink;
                default:
                    return Color.Transparent;
            }
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
