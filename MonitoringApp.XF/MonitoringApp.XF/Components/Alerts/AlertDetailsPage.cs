using Capital.GSG.FX.Monitoring.AppDataTypes;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Alerts
{
    public class AlertDetailsPage : ContentPage
    {
        public AlertDetailsPage(AlertFull alert)
        {
            Title = $"Alert {alert.Id}";

            Content = new TableView()
            {
                Intent = TableIntent.Data,
                Root = new TableRoot()
                {
                    new TableSection("CONTENT")
                    {
                        new TextCell() { Text = alert.Subject, TextColor = Color.Black },
                        new TextCell() { Text = alert.Body, TextColor = Color.Black },
                        new TextCell() {
                            Text = alert.Level,
                            TextColor = (alert.Level == "FATAL" || alert.Level == "ERROR") ? Color.Red : alert.Level == "WARNING" ? Color.Yellow : Color.Green },
                        new TextCell() { Text = alert.Source, TextColor = Color.Black },
                        new TextCell() { Text = $"{alert.Timestamp:dd/MM/yy HH:mm:ss zzz}", TextColor = Color.Black }
                    },
                    new TableSection("INFORMATION")
                    {
                        new TextCell() { Text = alert.Status, TextColor = alert.Status == "OPEN" ? Color.Red : Color.Green },
                        new TextCell() { Text = alert.Id, TextColor = Color.Black },
                        new ViewCell()
                        {
                            View = new Button() { Text = "Close Alert" }
                        }
                    }
                }
            };
        }
    }
}
