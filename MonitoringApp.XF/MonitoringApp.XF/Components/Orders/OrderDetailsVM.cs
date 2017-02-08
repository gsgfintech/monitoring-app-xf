using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using System.Globalization;
using MonitoringApp.XF.ViewModels;
using MonitoringApp.XF.Managers;

namespace MonitoringApp.XF.Components.Orders
{
    public class OrderDetailsVM : BaseViewModel
    {
        public event Action<int> HistoryPointsCountChanged;

        private OrderViewModel order;
        public OrderViewModel Order
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
            await LoadOrderByPermanentId(permanentId);
        }

        private async Task LoadOrderByPermanentId(int permanentId)
        {
            Order = (await OrderManager.Instance.GetOrderByPermanentId(permanentId)).ToOrderViewModel();

            HistoryPointsCountChanged?.Invoke(Order?.History?.Count ?? 0);
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
