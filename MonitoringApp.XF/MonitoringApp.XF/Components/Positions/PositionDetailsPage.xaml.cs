using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Positions
{
    public partial class PositionDetailsPage : ContentPage
    {
        public PositionDetailsVM ViewModel { get; private set; }

        public PositionDetailsPage()
        {
            InitializeComponent();

            ViewModel = BindingContext as PositionDetailsVM;
        }
    }
}
