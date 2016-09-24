using Capital.GSG.FX.Monitoring.AppDataTypes;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Executions
{
    public class ExecutionDetailsPage : ContentPage
    {
        private const string USD = "USD";
        private const double SmallFontSize = 10;

        public ExecutionDetailsPage(ExecutionFull execution)
        {
            Title = $"Trade {execution.Id}";
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
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition()
                }
            };

            grid.Children.Add(new Label() { Text = "Side", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center });
            grid.Children.Add(new Label() { Text = execution.Side, VerticalTextAlignment = TextAlignment.Center }, 1, 0);

            grid.Children.Add(new Label() { Text = "Quantity", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 1);
            grid.Children.Add(new Label() { Text = $"{execution.Quantity:N0}", VerticalTextAlignment = TextAlignment.Center }, 1, 1);

            grid.Children.Add(new Label() { Text = "Pair", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 2);
            grid.Children.Add(new Label() { Text = $"{execution.Cross}", VerticalTextAlignment = TextAlignment.Center }, 1, 2);

            grid.Children.Add(new Label() { Text = "Price", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 3);
            grid.Children.Add(new Label() { Text = $"{execution.Price}", VerticalTextAlignment = TextAlignment.Center }, 1, 3);

            grid.Children.Add(new Label() { Text = "Time", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 4);
            grid.Children.Add(new Label() { Text = $"{execution.ExecutionTime:dd/MM/yy HH:mm:ss zzz}", VerticalTextAlignment = TextAlignment.Center }, 1, 4);

            grid.Children.Add(new Label() { Text = "PnL", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 5);
            if (execution.RealizedPnlUsd.HasValue)
            {
                if (execution.RealizedPnlPips.HasValue)
                {
                    grid.Children.Add(new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label() { Text = $"{execution.RealizedPnlUsd} {USD}", VerticalTextAlignment = TextAlignment.Center },
                            new Label() { Text = $" ({execution.RealizedPnlPips} pips)", FontSize = SmallFontSize, VerticalTextAlignment = TextAlignment.Center }
                        }
                    }, 1, 5);
                }
                else
                    grid.Children.Add(new Label() { Text = $"{execution.RealizedPnlUsd} {USD}", VerticalTextAlignment = TextAlignment.Center }, 1, 5);
            }

            grid.Children.Add(new Label() { Text = "Duration", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 6);
            grid.Children.Add(new Label() { Text = execution.TradeDuration, VerticalTextAlignment = TextAlignment.Center }, 1, 6);

            grid.Children.Add(new Label() { Text = "Execution ID", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 7);
            grid.Children.Add(new Label() { Text = execution.Id, VerticalTextAlignment = TextAlignment.Center }, 1, 7);

            grid.Children.Add(new Label() { Text = "Order ID", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 8);
            grid.Children.Add(new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label() { Text = execution.OrderId.ToString(), VerticalTextAlignment = TextAlignment.Center },
                    new Label() { Text = $" (perm ID: {execution.PermanentID})", FontSize = SmallFontSize, VerticalTextAlignment = TextAlignment.Center }
                }
            }, 1, 8);

            grid.Children.Add(new Label() { Text = "Origin", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 9);
            grid.Children.Add(new Label() { Text = execution.OrderOrigin, VerticalTextAlignment = TextAlignment.Center }, 1, 9);

            grid.Children.Add(new Label() { Text = "Strategy", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 10);
            grid.Children.Add(new Label() { Text = execution.Strategy, VerticalTextAlignment = TextAlignment.Center }, 1, 10);

            grid.Children.Add(new Label() { Text = "Commission", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 11);
            if (execution.CommissionUsd.HasValue)
            {
                if (execution.Commission.HasValue && !string.IsNullOrEmpty(execution.CommissionCcy) && execution.CommissionCcy != USD)
                {
                    grid.Children.Add(new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label() { Text = $"{execution.CommissionUsd:N2} {USD}", VerticalTextAlignment = TextAlignment.Center },
                            new Label() { Text = $" ({execution.Commission:N2} {execution.CommissionCcy})", FontSize = SmallFontSize, VerticalTextAlignment = TextAlignment.Center }
                        }
                    }, 1, 11);
                }
                else
                    grid.Children.Add(new Label() { Text = $"{execution.CommissionUsd:N2} {USD}", VerticalTextAlignment = TextAlignment.Center }, 1, 11);
            }

            grid.Children.Add(new Label() { Text = "Account", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 12);
            grid.Children.Add(new Label() { Text = execution.AccountNumber, VerticalTextAlignment = TextAlignment.Center }, 1, 12);

            grid.Children.Add(new Label() { Text = "Exchange", FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 0, 13);
            grid.Children.Add(new Label() { Text = execution.Exchange, VerticalTextAlignment = TextAlignment.Center }, 1, 13);

            Content = grid;
        }
    }
}
