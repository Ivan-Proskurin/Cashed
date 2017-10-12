﻿using System;

namespace Logic.Cashed.Contract.Models
{
    public class ExpenseItemModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Cost { get; set; }
        public decimal Quantity { get; set; }
        public string Comment { get; set; }
    }
}
