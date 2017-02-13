using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.DBLoggers
{
    public partial class DBLoggersListPage : ContentPage
    {
        public DBLoggersListPage()
        {
            InitializeComponent();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DBLoggerVM dbLogger = ((ListView)sender).SelectedItem as DBLoggerVM;

            if (dbLogger != null)
            {
                var detailsView = new DBLoggerDetailsPage();
                detailsView.ViewModel?.GetDBLoggerByName(dbLogger.DBLoggerName);
                await Navigation.PushAsync(detailsView);
            }

            ((ListView)sender).SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm?.RefreshDBLoggers(false);
        }
    }
}
