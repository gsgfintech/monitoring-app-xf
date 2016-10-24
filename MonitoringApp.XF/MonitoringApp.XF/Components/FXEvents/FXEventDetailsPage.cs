using Capital.GSG.FX.Data.Core.MarketData;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.FXEvents
{
    public class FXEventDetailsPage : ContentPage
    {
        public FXEventDetailsPage(FXEvent fxEvent)
        {
            Title = fxEvent.Title;
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
                    new RowDefinition(),
                    new RowDefinition(),
                }
            };

            grid.Children.Add(new Label() { Text = "Currency", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center });
            grid.Children.Add(new Label() { Text = fxEvent.Currency.ToString(), VerticalTextAlignment = TextAlignment.Center }, 1, 0);

            grid.Children.Add(new Label() { Text = "Impact", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 1);
            grid.Children.Add(new Label() { Text = fxEvent.Level.ToString(), VerticalTextAlignment = TextAlignment.Center }, 1, 1);

            grid.Children.Add(new Label() { Text = "Time", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 2);
            grid.Children.Add(new Label() { Text = $"{fxEvent.Timestamp:dd/MM/yy HH:mm zzz}", VerticalTextAlignment = TextAlignment.Center }, 1, 2);

            grid.Children.Add(new Label() { Text = "Actual", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 3);
            grid.Children.Add(new Label() { Text = fxEvent.Actual, VerticalTextAlignment = TextAlignment.Center }, 1, 3);

            grid.Children.Add(new Label() { Text = "Forecast", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 4);
            grid.Children.Add(new Label() { Text = fxEvent.Forecast, VerticalTextAlignment = TextAlignment.Center }, 1, 4);

            grid.Children.Add(new Label() { Text = "Previous", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 5);
            grid.Children.Add(new Label() { Text = fxEvent.Previous, VerticalTextAlignment = TextAlignment.Center }, 1, 5);

            grid.Children.Add(new Label() { Text = "Info", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 6);
            grid.Children.Add(new Label() { Text = fxEvent.Explanation, VerticalTextAlignment = TextAlignment.Center }, 1, 6);

            grid.Children.Add(new Label() { Text = "ID", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 7);
            grid.Children.Add(new Label() { Text = fxEvent.EventId, VerticalTextAlignment = TextAlignment.Center }, 1, 7);

            Content = grid;
        }
    }
}
