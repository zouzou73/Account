using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountApplication
{
    public class Transaction
    {
        public DateTime Date { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public string Category { get; }

        public Transaction(DateTime date, decimal amount, string currency, string category)
        {
            Date = date;
            Amount = amount;
            Currency = currency;
            Category = category;
        }
    }

}
}
