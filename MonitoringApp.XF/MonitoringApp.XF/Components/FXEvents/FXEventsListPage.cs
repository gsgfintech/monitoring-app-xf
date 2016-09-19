using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.FXEvents
{
    public class FXEventsListPage : ContentPage
    {
        public FXEventsListPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "List of FX Events" }
                }
            };
        }
    }
}
