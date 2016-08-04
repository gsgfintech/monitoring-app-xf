using Microsoft.WindowsAzure.MobileServices;
using Net.Teirlinck.FX.Data.ContractData;
using Net.Teirlinck.FX.Data.ExecutionData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.GSG.FX.Monitor.App.XF
{
    public class ExecutionItem
    {
        public string Id { get; set; }

        public string AccountNumber { get; set; }

        public int? ClientId { get; set; }

        public string ClientOrderRef { get; set; }

        public int? CumulativeQuantity { get; set; }

        public string Exchange { get; set; }

        public DateTime ExecutionTime { get; set; }

        public int OrderId { get; set; }

        public int PermanentID { get; set; }

        public double Price { get; set; }

        public ExecutionSide Side { get; set; }

        public Cross Cross { get; set; }

        public double? Commission { get; set; }

        public Currency CommissionCcy { get; set; }

        public double? CommissionUsd { get; set; }

        public double? RealizedPnL { get; set; }

        public double? RealizedPnlPips { get; set; }

        public double? RealizedPnlUsd { get; set; }

        public string Strategy { get; set; }

        public TimeSpan TradeDuration { get; set; }

        public string OrderOrigin { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
