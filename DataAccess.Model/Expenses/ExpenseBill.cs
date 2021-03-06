﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Cashed.DataAccess.Contract.Base;
using Cashed.DataAccess.Model.Base;

namespace Cashed.DataAccess.Model.Expenses
{
    public class ExpenseBill : IHasId
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public virtual ICollection<ExpenseItem> Items { get; set; }

        public decimal SumPrice { get; set; }

        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}
