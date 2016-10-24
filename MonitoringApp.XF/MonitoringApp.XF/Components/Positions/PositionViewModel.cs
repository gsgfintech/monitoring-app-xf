using Capital.GSG.FX.Data.Core.AccountPortfolio;
using System.Collections.Generic;
using System.Linq;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionViewModel : Position
    {
        public string Header { get { return $"{Cross} ({Broker})"; } }

        public double TotalPnlUsd { get { return RealizedPnlUsd ?? 0 + UnrealizedPnlUsd ?? 0; } }
    }

    public static class PositionViewModelExtensions
    {
        public static PositionViewModel ToPositionViewModel(this Position position)
        {
            if (position == null)
                return null;

            return new PositionViewModel()
            {
                AverageCost = position.AverageCost,
                Broker = position.Broker,
                Cross = position.Cross,
                LastUpdate = position.LastUpdate,
                MarketPrice = position.MarketPrice,
                MarketValue = position.MarketValue,
                PositionQuantity = position.PositionQuantity,
                RealizedPnL = position.RealizedPnL,
                RealizedPnlUsd = position.RealizedPnlUsd,
                UnrealizedPnL = position.UnrealizedPnL,
                UnrealizedPnlUsd = position.UnrealizedPnlUsd
            };
        }

        public static IEnumerable<PositionViewModel> ToPositionViewModels(this IEnumerable<Position> positions)
        {
            return positions?.Select(p => p.ToPositionViewModel());
        }
    }
}
