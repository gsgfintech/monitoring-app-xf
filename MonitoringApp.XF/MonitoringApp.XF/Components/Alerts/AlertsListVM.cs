﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;
using System.Collections.Generic;
using Capital.GSG.FX.Data.Core.SystemData;
using System.Linq;
using MonitoringApp.XF.Managers;
using Capital.GSG.FX.Utils.Core;

namespace MonitoringApp.XF.Components.Alerts
{
    public class AlertsListVM : BaseViewModel
    {
        public ObservableCollection<GroupedAlertList> Alerts { get; private set; } = new ObservableCollection<GroupedAlertList>();

        private DateTime day;
        public DateTime Day
        {
            get { return day; }
            set
            {
                if (day != value)
                {
                    day = value;
                    OnPropertyChanged(nameof(Day));
                }
            }
        }

        private DateTime newDay;
        public DateTime NewDay
        {
            get { return newDay; }
            set
            {
                if (newDay != value)
                {
                    newDay = value;
                    OnPropertyChanged(nameof(NewDay));
                }
            }
        }

        public Command RefreshCommand { get; private set; }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (value != isRefreshing)
                {
                    isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        public AlertsListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshAlerts(true);

            IsRefreshing = false;
        }

        public async Task RefreshAlerts(bool refresh)
        {
            var openAlerts = (await AlertManager.Instance.LoadOpenAlerts(refresh))?.AsEnumerable().OrderByDescending(a => a.Timestamp);
            var closedAlerts = (await AlertManager.Instance.LoadAlertsClosedToday(refresh))?.AsEnumerable().OrderByDescending(a => a.Timestamp);

            Alerts.Clear();

            if (!openAlerts.IsNullOrEmpty())
                Alerts.Add(new GroupedAlertList($"Open Alerts ({openAlerts.Count()})", "OPEN", true, openAlerts));
            else
                Alerts.Add(new GroupedAlertList("Open Alerts (0)", "OPEN", false));

            if (!closedAlerts.IsNullOrEmpty())
                Alerts.Add(new GroupedAlertList($"Alerts Closed Today ({closedAlerts.Count()})", "CLOSED", false, closedAlerts));
            else
                Alerts.Add(new GroupedAlertList("Alerts Closed Today (0)", "CLOSED", false));
        }

        public async Task CloseAllAlerts()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var result = await AlertManager.Instance.CloseAllAlerts();

            if (result.Success)
            {
                await RefreshAlerts(true);
                await Utils.ShowToastNotification("Success", "Successfuly closed all alerts");
            }
            else
                await Utils.ShowToastNotification("Failed", result.Message);

            IsBusy = false;
        }
    }

    public class GroupedAlertList : ObservableCollection<Alert>
    {
        public string LongDescription { get; private set; }
        public string ShortDescription { get; private set; }
        public bool ShowCloseButton { get; set; }

        public GroupedAlertList(string longDescription, string shortDescription, bool showCloseButton)
        {
            LongDescription = longDescription;
            ShortDescription = shortDescription;
            ShowCloseButton = showCloseButton;
        }

        public GroupedAlertList(string longDescription, string shortDescription, bool showCloseButton, IEnumerable<Alert> alerts)
            : base(alerts)
        {
            LongDescription = longDescription;
            ShortDescription = shortDescription;
            ShowCloseButton = showCloseButton;
        }
    }

    public class LevelToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AlertLevel? level = value as AlertLevel?;

            if (!level.HasValue)
                return Color.Transparent;

            switch (level.Value)
            {
                case AlertLevel.DEBUG:
                case AlertLevel.INFO:
                    return CustomColors.LightSkyBlue;
                case AlertLevel.WARNING:
                    return CustomColors.LightSalmon;
                case AlertLevel.ERROR:
                case AlertLevel.FATAL:
                    return CustomColors.LightPink;
                default:
                    return Color.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
