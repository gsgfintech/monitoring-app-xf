using System;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.SystemsStatus
{
    public partial class SystemStatusDetailsPage : ContentPage
    {
        public SystemStatusDetailsVM ViewModel { get { return vm; } }

        public SystemStatusDetailsPage()
        {
            InitializeComponent();

            ViewModel.AttributesCountChanged += (count) =>
            {
                attributesViewCell.Height = Math.Max(20, 20 * count);
                attributesViewCell.ForceUpdateSize();
            };
        }

        private void OnAttributeSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
