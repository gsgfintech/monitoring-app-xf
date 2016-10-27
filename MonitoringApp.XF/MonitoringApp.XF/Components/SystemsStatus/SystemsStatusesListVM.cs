using Capital.GSG.FX.Utils.Portable;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;
using Capital.GSG.FX.Data.Core.SystemData;

namespace MonitoringApp.XF.Components.SystemsStatus
{
    public class SystemsStatusesListVM : BaseViewModel
    {
        public ObservableCollection<SystemStatus> Systems { get; set; } = new ObservableCollection<SystemStatus>();

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

        public SystemsStatusesListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
        }

        internal async Task<GenericActionResult> DoStartStop(string systemName)
        {
            SystemStatus system = Systems.FirstOrDefault(s => s.Name == systemName);

            if (system != null)
            {
                if (system.IsAlive) // will stop
                    return await SystemStatusManager.Instance.StopSystem(systemName);
                else // will start
                    return await SystemStatusManager.Instance.StartSystem(systemName);
            }
            else
                return new GenericActionResult()
                {
                    Message = $"Unknown system {systemName}",
                    Success = false
                };
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshSystemsStatuses(true);

            IsRefreshing = false;
        }

        public async Task RefreshSystemsStatuses(bool refresh)
        {
            try
            {
                await LoadSystemsStatuses(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadSystemsStatuses(refresh);
                }
            }
        }

        private async Task LoadSystemsStatuses(bool refresh)
        {
            var systems = await SystemStatusManager.Instance.LoadSystemsStatuses(refresh);

            Systems.Clear();

            if (!systems.IsNullOrEmpty())
            {
                foreach (var system in systems)
                    Systems.Add(system);
            }
        }
    }

    public class IsAliveToButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value as bool?) == true) ? "STOP" : "START";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
