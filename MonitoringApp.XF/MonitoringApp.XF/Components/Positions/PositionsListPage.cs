using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionsListPage : ContentPage
    {
        public PositionsListPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "List of Positions" }
                }
            };
        }
    }
}
