using Capital.GSG.FX.Data.Core.SystemData;
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
            SystemStatusAttribute attribute = ((ListView)sender).SelectedItem as SystemStatusAttribute;

            if (attribute != null)
                DisplayAlert(attribute.Name, $"{attribute.Value} ({attribute.Level})", "OK");

            ((ListView)sender).SelectedItem = null;
        }
    }
}
