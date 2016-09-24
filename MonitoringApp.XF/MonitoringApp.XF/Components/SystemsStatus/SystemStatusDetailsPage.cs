using Capital.GSG.FX.Monitoring.AppDataTypes;
using Capital.GSG.FX.Utils.Portable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.SystemsStatus
{
    public class SystemStatusDetailsPage : ContentPage
    {
        public SystemStatusDetailsPage(SystemStatusFull system)
        {
            Title = system.Name;
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
                    new RowDefinition()
                }
            };

            // IsAlive
            grid.Children.Add(new Label() { Text = "State", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center });
            grid.Children.Add(new Label() { Text = system.IsAlive ? "Running" : "Stopped", VerticalTextAlignment = TextAlignment.Center }, 1, 0);

            // OverallStatus
            grid.Children.Add(new Label() { Text = "Overall Status", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 1);
            grid.Children.Add(new Label() { Text = system.OverallStatus, VerticalTextAlignment = TextAlignment.Center }, 1, 1);

            // StartTime
            grid.Children.Add(new Label() { Text = "Start Time", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 2);
            grid.Children.Add(new Label() { Text = system.StartTime.ToString("dd/MM/yy HH:mm zzz"), VerticalTextAlignment = TextAlignment.Center }, 1, 2);

            // LastHearFrom
            grid.Children.Add(new Label() { Text = "Last HB", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 3);
            grid.Children.Add(new Label() { Text = system.LastHeardFrom.ToString("dd/MM/yy HH:mm zzz"), VerticalTextAlignment = TextAlignment.Center }, 1, 3);

            if (!system.Attributes.IsNullOrEmpty())
            {
                int rowIndex = 4;

                foreach (var attribute in system.Attributes)
                {
                    grid.RowDefinitions.Add(new RowDefinition());

                    grid.Children.Add(new Label() { Text = attribute.Name, FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, rowIndex);

                    Color bg;
                    switch (attribute.Level)
                    {
                        case "GREEN":
                            bg = Color.Green;
                            break;
                        case "YELLOW":
                            bg = Color.Yellow;
                            break;
                        default:
                            bg = Color.Red;
                            break;
                    }

                    grid.Children.Add(new Label() { Text = attribute.Value, BackgroundColor = bg, VerticalTextAlignment = TextAlignment.Center }, 1, rowIndex++);
                }
            }

            Content = grid;
        }
    }
}
