using Capital.GSG.FX.Data.Core.OrderData;
using Plugin.Toasts;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF
{
    internal static class Utils
    {
        public static async Task<INotificationResult> ShowToastNotification(string title, string description, bool isClickable = false)
        {
            var notificator = DependencyService.Get<IToastNotificator>();

            var options = new NotificationOptions()
            {
                Title = title,
                Description = description,
                IsClickable = isClickable,
                //AndroidOptions = new AndroidOptions()
                //{
                //    SmallDrawableIcon = 2130837649
                //}
            };

            return await notificator.Notify(options);
        }

        public static string ShortenOrderOrigin(OrderOrigin orderOrigin)
        {
            switch (orderOrigin)
            {
                case OrderOrigin.PositionOpen:
                    return "PO";
                case OrderOrigin.PositionClose:
                    return "PC";
                case OrderOrigin.PositionClose_ContStop:
                    return "PCS";
                case OrderOrigin.PositionClose_ContLimit:
                    return "PCL";
                case OrderOrigin.PositionClose_TE:
                    return "PCT";
                case OrderOrigin.PositionClose_CircuitBreaker:
                    return "PCB";
                case OrderOrigin.PositionReverse_Close:
                    return "PRC";
                case OrderOrigin.PositionReverse_Open:
                    return "PRO";
                case OrderOrigin.PositionDouble:
                    return "PD";
                default:
                    return orderOrigin.ToString();
            }
        }

        public static string ShortenOrderType(OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.LIMIT:
                    return "LMT";
                case OrderType.MARKET:
                    return "MKT";
                case OrderType.STOP:
                    return "STP";
                case OrderType.TRAILING_STOP:
                    return "TRAIL";
                default:
                    return orderType.ToString();
            }
        }

        public static string ShortenOrderStatus(OrderStatusCode orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatusCode.Submitted:
                    return "Sbtd";
                case OrderStatusCode.ApiCanceled:
                case OrderStatusCode.Cancelled:
                    return "Cxld";
                case OrderStatusCode.Filled:
                    return "Fld";
                default:
                    return orderStatus.ToString();
            }
        }
    }

    internal struct CustomColors
    {
        public static readonly Color LightGreen = Color.FromHex("#90EE90");
        public static readonly Color LightPink = Color.FromHex("#FFB6C1");
        public static readonly Color LightSalmon = Color.FromHex("#FFA07A");
        public static readonly Color LightSkyBlue = Color.FromHex("#87CEFA");
        public static readonly Color Orange = Color.FromHex("#FFA500");
    }

    public class NullToBoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            if (parameter?.ToString() == "HideZero")
            {
                double numVal;
                if (double.TryParse(value.ToString(), out numVal))
                    return numVal != 0;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NumberToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? number = value as double?;

            if (!number.HasValue)
                return Color.Default;

            if (number.Value < 0)
                return Color.Red;
            else if (number.Value > 0)
                return Color.Green;
            else
                return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NumberToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? number = value as double?;

            if (!number.HasValue)
                return Color.Transparent;

            if (number.Value < 0)
                return CustomColors.LightPink;
            else if (number.Value > 0)
                return CustomColors.LightGreen;
            else
                return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
