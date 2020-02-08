using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ExpenseTrackerSP
{
    public class Expense
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string CategoryName { get; set; }
    }
}
