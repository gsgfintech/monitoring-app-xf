using System;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Orders
{
    public partial class OrderDetailsPage : ContentPage
    {
        public OrderDetailsVM ViewModel { get { return vm; } }

        public OrderDetailsPage()
        {
            InitializeComponent();

            ViewModel.HistoryPointsCountChanged += (count) =>
            {
                historyViewCell.Height = Math.Max(40, 20 + 20 * count);
                historyViewCell.ForceUpdateSize();
            };
        }

        private void OnHistoryPointSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
