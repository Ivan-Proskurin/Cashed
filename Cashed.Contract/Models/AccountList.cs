using System.Collections.Generic;

namespace Cashed.Logic.Contract.Models
{
    public class AccountList
    {
        public List<AccountModel> Accounts { get; set; }
        public TotalsInfoModel Totals { get; set; }
    }
}