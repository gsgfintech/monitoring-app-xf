using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Alerts
{
    public class AlertsListPage : ContentPage
    {
        public AlertsListPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "List of Alerts" }
                }
            };
        }
    }
}
