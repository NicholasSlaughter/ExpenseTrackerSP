using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ExpenseTrackerSP
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }

        [OneToOne(CascadeOperations =CascadeOperation.CascadeRead)]
        public Expense PublicExpense { get; set; }

        [ForeignKey(typeof(Expense))]
        public int ExpenseId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Expense PublicNotification { get; set; }

        [ForeignKey(typeof(Expense))]
        public int NotificationId { get; set; }
    }
}
