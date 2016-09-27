﻿using Capital.GSG.FX.Monitoring.AppDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Positions
{
    public partial class PositionsListPage : ContentPage
    {
        private PositionsListVM vm;

        public PositionsListPage()
        {
            InitializeComponent();

            vm = BindingContext as PositionsListVM;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm?.RefreshPositions(false);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PositionSecuritySlim security = ((ListView)sender).SelectedItem as PositionSecuritySlim;

            if (security != null)
            {
                PositionDetailsVM detailsVm = new PositionDetailsVM();

                var detailsView = new PositionDetailsPage();
                await detailsView.ViewModel?.GetPositionByCross(security.Cross);
                await Navigation.PushAsync(detailsView);
            }

            ((ListView)sender).SelectedItem = null;
        }
    }
}
