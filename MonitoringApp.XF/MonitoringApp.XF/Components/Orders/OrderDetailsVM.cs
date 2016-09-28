using Capital.GSG.FX.Monitoring.AppDataTypes;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using System.Globalization;

namespace MonitoringApp.XF.Components.Orders
{
    public class OrderDetailsVM : BaseViewModel
    {
        private OrderFull order;
        public OrderFull Order
        {
            get { return order; }
            set
            {
                if (order != value)
                {
                    order = value;
                    OnPropertyChanged(nameof(Order));
                }
            }
        }

        public async Task GetOrderByPermanentId(int permanentId)
        {
            try
            {
                await LoadOrderByPermanentId(permanentId);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadOrderByPermanentId(permanentId);
                }
            }
        }

        private async Task LoadOrderByPermanentId(int permanentId)
        {
            Order = await OrderManager.Instance.GetOrderByPermanentId(permanentId);
        }
    }

    public class EstimatedCommissionFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? commission = value as double?;

            if (!commission.HasValue)
                return null;
            else
                return $"{commission:N2} {parameter}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
