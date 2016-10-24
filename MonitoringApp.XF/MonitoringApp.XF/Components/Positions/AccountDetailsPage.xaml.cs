
using System;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Positions
{
    public partial class AccountDetailsPage : ContentPage
    {
        public AccountDetailsVM ViewModel { get { return vm; } }

        public AccountDetailsPage()
        {
            InitializeComponent();

            ViewModel.AttributesCountChanged += (count) =>
            {
                attributesViewCell.Height = Math.Max(40, 20 + 20 * count);
                attributesViewCell.ForceUpdateSize();
            };
        }

        private void OnAttributeSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
