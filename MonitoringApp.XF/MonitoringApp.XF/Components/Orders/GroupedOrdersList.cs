using Capital.GSG.FX.Data.Core.OrderData;
using MonitoringApp.XF.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonitoringApp.XF.Components.Orders
{
    public class GroupedOrdersList : ObservableCollection<OrderViewModel>
    {
        public string LongDescription { get; private set; }
        public string ShortDescription { get; private set; }

        public GroupedOrdersList(string longDescription, string shortDescription)
        {
            LongDescription = longDescription;
            ShortDescription = shortDescription;
        }

        public GroupedOrdersList(string longDescription, string shortDescription, IEnumerable<OrderViewModel> orders)
            : base(orders)
        {
            LongDescription = longDescription;
            ShortDescription = shortDescription;
        }
    }
}
