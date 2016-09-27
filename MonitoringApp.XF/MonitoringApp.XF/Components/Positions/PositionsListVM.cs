using Capital.GSG.FX.Monitoring.AppDataTypes;
using Capital.GSG.FX.Utils.Portable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using System.Globalization;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionsListVM : BaseViewModel
    {
        public ObservableCollection<PositionSecuritySlim> Positions { get; set; } = new ObservableCollection<PositionSecuritySlim>();

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

        public PositionsListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshPositions(true);

            IsRefreshing = false;
        }

        public async Task RefreshPositions(bool refresh)
        {
            try
            {
                await LoadPositions(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadPositions(refresh);
                }
            }
        }

        private async Task LoadPositions(bool refresh)
        {
            var positions = await PositionManager.Instance.LoadPositions(refresh);

            Positions.Clear();

            if (!positions.IsNullOrEmpty())
            {
                foreach (var position in positions)
                    Positions.Add(position);
            }
        }
    }

    public class NumberToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? number = value as double?;

            if (!number.HasValue)
                return Color.Default;

            if (number.Value < 0)
                return Color.Red;
            else if (number.Value > 0)
                return Color.Green;
            else
                return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
