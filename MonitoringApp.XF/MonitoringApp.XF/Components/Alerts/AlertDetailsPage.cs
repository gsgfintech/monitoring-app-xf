using Capital.GSG.FX.Monitoring.AppDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Alerts
{
    public class AlertDetailsPage : ContentPage
    {
        public AlertDetailsPage(AlertFull alert)
        {
            Title = $"Alert {alert.Id}";
            Padding = new Thickness(20);

            Grid grid = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) }
                },
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition()
                }
            };

            grid.Children.Add(new Label() { Text = "Header", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center });
            grid.Children.Add(new Label() { Text = alert.Subject, VerticalTextAlignment = TextAlignment.Center }, 1, 0);

            grid.Children.Add(new Label() { Text = "Message", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 1);
            grid.Children.Add(new Label() { Text = alert.Body, VerticalTextAlignment = TextAlignment.Center }, 1, 1);

            grid.Children.Add(new Label() { Text = "Level", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 2);
            grid.Children.Add(new Label() { Text = alert.Level, VerticalTextAlignment = TextAlignment.Center }, 1, 2);

            grid.Children.Add(new Label() { Text = "Source", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 3);
            grid.Children.Add(new Label() { Text = alert.Source, VerticalTextAlignment = TextAlignment.Center }, 1, 3);

            grid.Children.Add(new Label() { Text = "Time", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 4);
            grid.Children.Add(new Label() { Text = alert.Timestamp.ToString("dd/MM/yy HH:mm:ss zzz"), VerticalTextAlignment = TextAlignment.Center }, 1, 4);

            grid.Children.Add(new Label() { Text = "Status", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 5);
            grid.Children.Add(new Label() { Text = alert.Status, VerticalTextAlignment = TextAlignment.Center }, 1, 5);

            grid.Children.Add(new Label() { Text = "ID", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 6);
            grid.Children.Add(new Label() { Text = alert.Id, VerticalTextAlignment = TextAlignment.Center }, 1, 6);

            Content = grid;
        }
    }
}
