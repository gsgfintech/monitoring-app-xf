using Capital.GSG.FX.Data.Core.AccountPortfolio;
using System.Collections.Generic;
using System.Linq;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionViewModel : Position
    {
        public string Header { get { return $"{Cross} ({Broker})"; } }
    }

    public static class PositionViewModelExtensions
    {
        public static PositionViewModel ToPositionViewModel(this Position position)
        {
            if (position == null)
                return null;

            return new PositionViewModel()
            {
                Account = position.Account,
                AverageCost = position.AverageCost,
                Broker = position.Broker,
                Cross = position.Cross,
                LastUpdate = position.LastUpdate,
                PositionQuantity = position.PositionQuantity
            };
        }

        public static IEnumerable<PositionViewModel> ToPositionViewModels(this IEnumerable<Position> positions)
        {
            return positions?.Select(p => p.ToPositionViewModel());
        }
    }
}
