using Capital.GSG.FX.Utils.Portable;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using System.Globalization;
using System.Linq;

namespace MonitoringApp.XF.Components.Orders
{
    public class OrdersListVM : BaseViewModel
    {
        public ObservableCollection<GroupedOrdersList> Orders { get; set; } = new ObservableCollection<GroupedOrdersList>();

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

        private DateTime day;
        public DateTime Day
        {
            get { return day; }
            set
            {
                if (day != value)
                {
                    day = value;
                    OnPropertyChanged(nameof(Day));
                }
            }
        }

        private DateTime newDay;
        public DateTime NewDay
        {
            get { return newDay; }
            set
            {
                if (newDay != value)
                {
                    newDay = value;
                    OnPropertyChanged(nameof(NewDay));
                }
            }
        }

        public Command ChangeDayCommand { get; private set; }

        public OrdersListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
            ChangeDayCommand = new Command(ExecuteChangeDayCommand);

            Day = DateTime.Today;
            NewDay = Day;
        }

        private async void ExecuteChangeDayCommand()
        {
            if (NewDay == Day)
                return;

            Day = NewDay;

            await RefreshOrders(false);
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshOrders(true);

            IsRefreshing = false;
        }

        public async Task RefreshOrders(bool refresh)
        {
            try
            {
                await LoadOrders(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadOrders(refresh);
                }
            }
        }

        private async Task LoadOrders(bool refresh)
        {
            var orders = await OrderManager.Instance.LoadOrders(Day, refresh);

            Orders.Clear();

            if (!orders.IsNullOrEmpty())
            {
                // 1. Active orders
                var activeOrders = orders.Where(o => o.Status.ToLower().Contains("submit") || o.Status == "PendingCancel");

                if (!activeOrders.IsNullOrEmpty())
                    Orders.Add(new GroupedOrdersList($"Active Orders ({activeOrders.Count()})", "ACTIVE", activeOrders));
                else
                    Orders.Add(new GroupedOrdersList("Active Orders (0)", "ACTIVE"));

                // 2. Inactive orders
                var inactiveOrders = orders.Where(o => !o.Status.ToLower().Contains("submitted"));

                if (!inactiveOrders.IsNullOrEmpty())
                    Orders.Add(new GroupedOrdersList($"Inactive Orders ({inactiveOrders.Count()})", "INACTIVE", inactiveOrders));
                else
                    Orders.Add(new GroupedOrdersList("Inactive Orders (0)", "INACTIVE"));
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
