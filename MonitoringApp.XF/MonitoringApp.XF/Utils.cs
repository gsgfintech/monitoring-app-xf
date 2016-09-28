using System;
using System.Globalization;
using Xamarin.Forms;

namespace MonitoringApp.XF
{
    internal static class Utils
    {
        public static string ShortenOrderOrigin(string orderOrigin)
        {
            if (string.IsNullOrEmpty(orderOrigin))
                return orderOrigin;

            switch (orderOrigin)
            {
                case "PositionOpen":
                    return "PO";
                case "PositionDouble":
                    return "PD";
                case "PositionClose":
                    return "PC";
                case "PositionClose_ContStop":
                    return "PCS";
                case "PositionClose_ContLimit":
                    return "PCL";
                case "PositionClose_TE":
                    return "PCT";
                case "PositionClose_CircuitBreaker":
                    return "PCB";
                case "PositionReverse_Close":
                    return "PRC";
                case "PositionReverse_Open":
                    return "PRO";
                default:
                    return orderOrigin;
            }
        }

        public static string ShortenOrderType(string orderType)
        {
            if (string.IsNullOrEmpty(orderType))
                return orderType;

            switch (orderType)
            {
                case "MARKET":
                    return "MKT";
                case "STOP":
                    return "STP";
                case "LIMIT":
                    return "LMT";
                case "TRAILING_STOP":
                    return "TRAIL";
                default:
                    return orderType;
            }
        }

        public static string ShortenOrderStatus(string orderStatus)
        {
            if (string.IsNullOrEmpty(orderStatus))
                return orderStatus;

            switch (orderStatus)
            {
                case "Submitted":
                    return "Sbtd";
                case "Filled":
                    return "Fld";
                case "Cancelled":
                    return "Cxld";
                default:
                    return orderStatus;
            }
        }
    }

    internal static class CustomColors
    {
        public static readonly Color LightGreen = Color.FromHex("#90EE90");
        public static readonly Color LightPink = Color.FromHex("#FFB6C1");
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
}
