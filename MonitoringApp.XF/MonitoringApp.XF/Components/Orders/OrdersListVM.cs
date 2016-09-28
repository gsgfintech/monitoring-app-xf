using Capital.GSG.FX.Monitoring.AppDataTypes;
using Capital.GSG.FX.Utils.Portable;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using System.Globalization;

namespace MonitoringApp.XF.Components.Orders
{
    public class OrdersListVM : BaseViewModel
    {
        public ObservableCollection<OrderSlim> TodaysOrders { get; set; } = new ObservableCollection<OrderSlim>();

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

        public OrdersListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshTodaysOrders(true);

            IsRefreshing = false;
        }

        public async Task RefreshTodaysOrders(bool refresh)
        {
            try
            {
                await LoadTodaysOrders(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadTodaysOrders(refresh);
                }
            }
        }

        private async Task LoadTodaysOrders(bool refresh)
        {
            var orders = await OrderManager.Instance.LoadTodaysOrders(refresh);

            TodaysOrders.Clear();

            if (!orders.IsNullOrEmpty())
            {
                foreach (var alert in orders)
                    TodaysOrders.Add(alert);
            }
        }
    }

    public class QuantityShortener : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            int qty;

            if (!int.TryParse(value.ToString(), out qty))
                return null;

            return $"{qty / 1000:N0}K";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CrossShortener : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().Replace("USD", "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OriginShortener : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Utils.ShortenOrderOrigin(value?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString();

            if (string.IsNullOrEmpty(status))
                return Color.Transparent;

            switch (status)
            {
                case "Filled":
                    return CustomColors.LightGreen;
                case "Cancelled":
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

    public class StatusToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString();

            if (string.IsNullOrEmpty(status))
                return Color.Default;

            switch (status)
            {
                case "Filled":
                    return Color.Green;
                case "Cancelled":
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

    public class TypeShortener : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Utils.ShortenOrderType(value?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
