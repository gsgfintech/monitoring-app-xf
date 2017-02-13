using MonitoringApp.XF.Components.Alerts;
using MonitoringApp.XF.Components.Executions;
using MonitoringApp.XF.Components.FXEvents;
using MonitoringApp.XF.Components.Home;
using MonitoringApp.XF.Components.Orders;
using MonitoringApp.XF.Components.Positions;
using MonitoringApp.XF.Components.SystemsStatus;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.MainMenu
{
    public class MainMenuPage : ContentPage
    {
        private ListView listView;
        public ListView ListView { get { return listView; } }

        public MainMenuPage()
        {
            listView = new ListView()
            {
                ItemsSource = PopulateMenu(),
                ItemTemplate = new DataTemplate(() =>
                {
                    var imageCell = new ImageCell();
                    imageCell.SetBinding(TextCell.TextProperty, "Title");
                    imageCell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
                    return imageCell;
                }),
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None
            };

            Padding = new Thickness(0, 40, 0, 0);
            Icon = "hamburger.png";
            Title = "Stratedge.me Monitor";

            Content = new StackLayout()
            {
                Children =
                {
                    listView
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
        }

        private List<MainMenuPageItem> PopulateMenu()
        {
            List<MainMenuPageItem> mainMenuPageItems = new List<MainMenuPageItem>();

            mainMenuPageItems.Add(new MainMenuPageItem()
            {
                IconSource = "contacts.png",
                TargetType = typeof(HomePage),
                Title = "Overview"
            });

            mainMenuPageItems.Add(new MainMenuPageItem()
            {
                IconSource = "contacts.png",
                TargetType = typeof(SystemsStatusesListPage),
                Title = "Systems"
            });

            mainMenuPageItems.Add(new MainMenuPageItem()
            {
                IconSource = "contacts.png",
                TargetType = typeof(PositionsListPage),
                Title = "Positions"
            });

            mainMenuPageItems.Add(new MainMenuPageItem()
            {
                IconSource = "contacts.png",
                TargetType = typeof(AlertsListPage),
                Title = "Alerts"
            });

            mainMenuPageItems.Add(new MainMenuPageItem()
            {
                IconSource = "contacts.png",
                TargetType = typeof(ExecutionsListPage),
                Title = "Trades"
            });

            mainMenuPageItems.Add(new MainMenuPageItem()
            {
                IconSource = "contacts.png",
                TargetType = typeof(OrdersListPage),
                Title = "Orders"
            });

            mainMenuPageItems.Add(new MainMenuPageItem()
            {
                IconSource = "contacts.png",
                TargetType = typeof(FXEventsListPage),
                Title = "FX Events"
            });

            mainMenuPageItems.Add(new MainMenuPageItem()
            {
                IconSource = "contacts.png",
                TargetType = typeof(TradeEngines.TradeEnginesListPage),
                Title = "Trade Engines"
            });

            mainMenuPageItems.Add(new MainMenuPageItem()
            {
                IconSource = "contacts.png",
                TargetType = typeof(DBLoggers.DBLoggersListPage),
                Title = "DB Loggers"
            });

            return mainMenuPageItems;
        }
    }
}
