using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Systems
{
    public class SystemsListPage : ContentPage
    {
        public SystemsListPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "List of Systems" }
                }
            };
        }
    }
}
