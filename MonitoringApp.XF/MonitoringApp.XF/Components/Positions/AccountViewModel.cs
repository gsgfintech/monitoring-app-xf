using Capital.GSG.FX.Data.Core.AccountPortfolio;
using Capital.GSG.FX.Data.Core.ContractData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitoringApp.XF.Components.Positions
{
    public class AccountViewModel
    {
        public List<AccountAttributeViewModel> Attributes { get; set; }
        public Broker Broker { get; set; }
        public DateTimeOffset LastUpdate { get; set; }
        public string Name { get; set; }

        public string Header { get { return $"{Name} ({Broker})"; } }
    }

    public class AccountAttributeViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public static class AccountViewModelExtensions
    {
        private static AccountAttributeViewModel ToAccountAttributeViewModel(this AccountAttribute attribute)
        {
            if (attribute == null)
                return null;

            return new AccountAttributeViewModel()
            {
                Key = attribute.Key,
                Value = attribute.Value + (attribute.Currency != Currency.UNKNOWN ? $" {attribute.Currency}" : "")
            };
        }

        private static List<AccountAttributeViewModel> ToAccountAttributeViewModels(this IEnumerable<AccountAttribute> attributes)
        {
            return attributes?.Select(a => a.ToAccountAttributeViewModel()).ToList();
        }

        public static AccountViewModel ToAccountViewModel(this Account account)
        {
            if (account == null)
                return null;

            return new AccountViewModel()
            {
                Attributes = account.Attributes.ToAccountAttributeViewModels(),
                Broker = account.Broker,
                LastUpdate = account.LastUpdate,
                Name = account.Name
            };
        }
    }
}
